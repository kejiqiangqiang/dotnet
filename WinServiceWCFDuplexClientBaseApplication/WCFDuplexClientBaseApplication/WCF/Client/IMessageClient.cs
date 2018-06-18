using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 双工通信客户端接口
    /// </summary>
    public interface IMessageClient
    {
        /// <summary>
        /// 双工通信客户端接收消息
        /// </summary>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(MessageModel message);
    }

}
