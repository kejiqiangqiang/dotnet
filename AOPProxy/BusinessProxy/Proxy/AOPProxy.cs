using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace BusinessProxy
{
    /// <summary>
    /// 执行函数事件的委托
    /// </summary>
    /// <param name="msg">IMessage</param>
    /// <param name="methodReturnMsg">IMethodReturnMessage</param>
    /// <param name="target">目标类被代理的对象</param>
    /// <param name="attr">方法属性的描述</param>
    public delegate void DelegateExecuteMessage(IMessage msg, IMethodReturnMessage methodReturnMsg, MarshalByRefObject target, BusinessMethodAttribute attr);


    public sealed class AOPProxy:RealProxy
    {
        /// <summary>
        /// 构造方法执行前事件
        /// </summary> 
        public event DelegateExecuteMessage EventConstructionBegin;

        /// <summary>
        /// 构造方法执行后事件
        /// </summary> 
        public event DelegateExecuteMessage EventConstructionEnd;

        /// <summary>
        /// 方法执行前事件
        /// </summary> 
        public event DelegateExecuteMessage EventMethodBegin;

        /// <summary>
        /// 方法执行后事件
        /// </summary> 
        public event DelegateExecuteMessage EventMethodEnd;

        MarshalByRefObject _target=null;

        Type type = null;

        public AOPProxy(Type type, MarshalByRefObject target)
            : base(type)
        {
            this._target = target;
            this.type = type;
        }

        public override IMessage Invoke(IMessage msg)
        {
            //throw new NotImplementedException();

            IMethodCallMessage call = (IMethodCallMessage)msg;
            IConstructionCallMessage ctor = call as IConstructionCallMessage;
            IMethodReturnMessage back = null;
            //filter method
            object[] attrs = call.MethodBase.GetCustomAttributes(typeof(BusinessMethodAttribute), true);
            bool isAopMethod = attrs.Length > 0;
            if (ctor!=null)
            {

                //构造开始时
                //do something
                if (this.EventConstructionBegin != null)
                {
                    this.EventConstructionBegin(msg, back, this._target, null);
                }

                //执行远程对象构造方法
                MarshalByRefObject mbro = this.GetTransparentProxy() as MarshalByRefObject;
                RealProxy realProxy = RemotingServices.GetRealProxy(this._target);
                realProxy.InitializeServerObject(ctor);
                back = EnterpriseServicesHelper.CreateConstructionReturnMessage(ctor, mbro);

                //构造结束时
                //do something
                if (this.EventConstructionEnd != null)
                {
                    this.EventConstructionEnd(msg, back, this._target, null);
                }

                return back;
            }


            //方法执行开始时
            //do something
            if (this.EventMethodBegin!=null&&isAopMethod)
            {
                this.EventMethodBegin(msg,back,this._target,attrs[0] as BusinessMethodAttribute);
            }

            //执行远程对象方法
            back = RemotingServices.ExecuteMessage(this._target,call);

            //方法执行结束时
            //do something
            if (this.EventMethodEnd != null && isAopMethod)
            {
                this.EventMethodEnd(msg, back, this._target, attrs[0] as BusinessMethodAttribute);
            }
            return back;
        }
    }
}
