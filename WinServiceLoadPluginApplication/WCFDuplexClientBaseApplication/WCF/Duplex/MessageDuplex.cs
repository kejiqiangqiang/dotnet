using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    
    /// <summary>
    /// 双工通信通道--建立此通道将用于服务端与客户端回调建立连接
    /// </summary>
    public class MessageDuplex : DuplexClientBase<IMessageService>,IMessageService
    {
        /// <summary>
        /// 构建双工通信通道
        /// </summary>
        /// <param name="callbackInstance">客户端回调对象</param>
        /// <param name="binding"></param>
        /// <param name="remoteAddress"></param>
        public MessageDuplex(IMessageClient callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {

        }

        /// <summary>
        /// 注册客户端连接
        /// </summary>
        public void RegisterClient()
        {
            base.Channel.RegisterClient();
        }
    }

}
