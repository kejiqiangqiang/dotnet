using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace BusinessProxy
{
    /// <summary>
    /// 拦截类属性类
    /// </summary>
    public class BusinessClassAttribute:ProxyAttribute
    {
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            //创建对象
            MarshalByRefObject obj = base.CreateInstance(serverType);
            //远程对象代理
            AOPProxy proxy = new AOPProxy(serverType, obj);
            //事件处理程序//订阅方法被调用(拦截方法的调用)
            proxy.EventMethodBegin += new DelegateExecuteMessage(this.MethodBegin);
            proxy.EventMethodEnd += new DelegateExecuteMessage(this.MethodEnd);
            proxy.EventConstructionEnd += new DelegateExecuteMessage(this.ConstructionEnd);
            //得到对象代理的透明代理(代理必须经过透明代理层见MSDN)
            MarshalByRefObject proxyMbro = (MarshalByRefObject)proxy.GetTransparentProxy();
            return proxyMbro;
        }

        /// <summary>
        /// 构造方法结束
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="methodReturnMsg"></param>
        /// <param name="target"></param>
        /// <param name="attr"></param>
        public void ConstructionEnd(IMessage msg, IMethodReturnMessage methodReturnMsg, MarshalByRefObject target, BusinessMethodAttribute attr)
        {
            AbsBusiness Interceptor = target as AbsBusiness;
            //Interceptor Interceptor = target as Business;
            //if (Interceptor == null)
            //{
            //    throw new NotImplementedException("类未实现拦截接口\"Interceptor\"");
            //}
            Interceptor.ConstructionEnd(msg, methodReturnMsg, attr);
        }

        /// <summary>
        /// 方法调用开始
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="methodReturnMsg"></param>
        /// <param name="target"></param>
        /// <param name="attr"></param>
        public void MethodBegin(IMessage msg, IMethodReturnMessage methodReturnMsg, MarshalByRefObject target, BusinessMethodAttribute attr)
        {
            (target as AbsBusiness).MethodBegin(msg, methodReturnMsg, attr);
        }

        /// <summary>
        /// 方法调用结束
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="methodReturnMsg"></param>
        /// <param name="target"></param>
        /// <param name="attr"></param>
        public void MethodEnd(IMessage msg, IMethodReturnMessage methodReturnMsg, MarshalByRefObject target, BusinessMethodAttribute attr)
        {
            (target as AbsBusiness).MethodEnd(msg, methodReturnMsg, attr);
        }

    }
}
