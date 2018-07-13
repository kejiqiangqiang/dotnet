using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BusinessProxy
{
    /// <summary>
    /// 被拦截类接口
    /// </summary>
    public interface Interceptor
    {
        /// <summary>
        /// 构造方法结束
        /// </summary>
        void ConstructionEnd(IMessage msg, IMethodReturnMessage methodReturnMsg, BusinessMethodAttribute attr);
        /// <summary>
        ///  方法调用开始
        /// </summary> 
        void MethodBegin(IMessage msg, IMethodReturnMessage methodReturnMsg, BusinessMethodAttribute attr);
        /// <summary>
        /// 方法调用结束
        /// </summary> 
        void MethodEnd(IMessage msg, IMethodReturnMessage methodReturnMsg, BusinessMethodAttribute attr);
    }
}
