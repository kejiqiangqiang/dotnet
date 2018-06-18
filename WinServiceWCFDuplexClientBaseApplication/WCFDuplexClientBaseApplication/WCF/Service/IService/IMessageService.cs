using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 双工通信服务端接口
    /// </summary>
    [ServiceContract(CallbackContract=typeof(IMessageClient))]
    public interface IMessageService
    {
        /// <summary>
        /// 注册客户端连接
        /// </summary>
        [OperationContract]
        void RegisterClient();
    }

}
