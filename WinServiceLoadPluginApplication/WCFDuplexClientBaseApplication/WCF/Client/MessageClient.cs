using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 接收消息委托
    /// </summary>
    /// <param name="message"></param>
    public delegate void ReceiveMessageDelegate(MessageModel message);

    /// <summary>
    /// 双工通信客户端
    /// </summary>
    public class MessageClient:IMessageClient
    {
        /// <summary>
        /// 接收消息事件
        /// </summary>
        public event ReceiveMessageDelegate ReceiveMessageEvent=null;
        public MessageDuplex messageDuplex = null;

        /// <summary>
        /// 构建双工通信通道
        /// </summary>
        /// <param name="wcfUri"></param>
        public MessageClient(string wcfUri)
        {
            EndpointAddress address = new EndpointAddress("net.tcp://" + wcfUri + "/MessageService");
            messageDuplex = new MessageDuplex(this, new NetTcpBinding(SecurityMode.None), address);
            messageDuplex.RegisterClient();
        }

        /// <summary>
        /// 双工通信客户端接收消息
        /// </summary>
        /// <param name="message"></param>
        public void ReceiveMessage(MessageModel message)
        {
            if (this.ReceiveMessageEvent!=null)
            {
                this.ReceiveMessageEvent(message);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
                messageDuplex.Close();
            }
            catch { }
        }

    }

}
