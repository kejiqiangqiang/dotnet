using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 双工通信服务端
    /// </summary>
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class MessageService : IMessageService
    {
        public static List<IMessageClient> ClientCallbackList { get; set; }
        private static Mutex mutex = new Mutex();
        public MessageService()
        {
            if (MessageService.ClientCallbackList == null)
            {
                MessageService.ClientCallbackList = new List<IMessageClient>();
            }
        }

        /// <summary>
        /// 注册客户端连接
        /// </summary>
        public void RegisterClient()
        {
            var client = OperationContext.Current.GetCallbackChannel<IMessageClient>();
            var id = OperationContext.Current.SessionId;
            MessageService.ClientCallbackList.Add(client);
            OperationContext.Current.Channel.Closing += new EventHandler(Channel_Closing);
        }

        /// <summary>
        /// 向客户端发送消息--服务端主动（但需要客户端主动注册连接以建立通道），不需要在接口中协定，直接通过通道回调客户端接收消息方法
        /// </summary>
        /// <param name="pluginId"></param>
        /// <param name="taskId"></param>
        /// <param name="messageType"></param>
        /// <param name="message"></param>
        public void SendMessage(string pluginId, string taskId, string messageType, string message)
        {
            try
            {
                mutex.WaitOne();
                foreach (var item in MessageService.ClientCallbackList)
                {
                    item.ReceiveMessage(new MessageModel()
                    {
                        PluginId = pluginId,
                        TaskId = taskId,
                        MessageType = messageType,
                        Message = message
                    });
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
            
        }

        /// <summary>
        /// 关闭客户端通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Channel_Closing(object sender,EventArgs e)
        {
            try
            {
                mutex.WaitOne();
                MessageService.ClientCallbackList.Remove((IMessageClient)sender);
            }
            finally 
            {
                mutex.ReleaseMutex();
            }
        }

        #region

        ///// <summary>
        ///// 销毁所有客户端连接
        ///// </summary>
        //public void Dispose()
        //{
        //    try
        //    {
        //        mutex.WaitOne();
        //        MessageService.ClientCallbackList.Clear();
        //    }
        //    finally
        //    {
        //        mutex.ReleaseMutex();
        //    }
        //}

        #endregion

        
    }
}
