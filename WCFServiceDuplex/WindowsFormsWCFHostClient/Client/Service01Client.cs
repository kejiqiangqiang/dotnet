using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsWCFHostClient.ServiceReference1;

namespace WindowsFormsWCFHostClient.Client
{
    /// <summary>
    /// 接收消息委托
    /// </summary>
    /// <param name="message">消息对象</param>
    public delegate void ReceiveMessageDelegate(string message);

    //设置回调函数执行线程为独立线程（不能在当前上下文线程执行回调，否则可能导致死锁(如UI线程)）
    [CallbackBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class Service01Client : IService1Callback
    {
        InstanceContext _instanceContext;
        InstanceContext InstanceContext
        {
            get { return _instanceContext; }
            set { _instanceContext = value; }
        }

        ////普通方法内新建窗口卡死
        ////子窗口UI线程显示回调信息
        //WindowsFormsWCFHostClient.Form1 newForm1 = null;

        /// <summary>
        /// 接收消息事件--//通过事件订阅发布进行发布消息
        /// </summary>
        public event ReceiveMessageDelegate ReceiveMessageEvent = null;

        public Service01Client()
        {
            this.InstanceContext = new InstanceContext(this);
        }
        public string getValue(int value)
        {
            string v = "";

            ////1.通过客户端代理调用服务端方法
            //using (DuplexChannelFactory<IService1> _channelFactory=new  DuplexChannelFactory<IService1>(this.InstanceContext,"WCFServiceDuplex.Service1"))
            //{
            //    IService1 proxy = _channelFactory.CreateChannel();
            //    proxy.GetData(value);
            //}
            
            //2.通过客户端代理调用服务端方法
            using (Service1Client client = new Service1Client(this.InstanceContext))
            {
                v=client.GetData(value);
            }
            return v;
        }

        /// <summary>
        /// 客户端实现回调接口
        /// </summary>
        /// <param name="message"></param>
        public void NotifyClientMsg(string message)
        {
            #region 使用事件进行窗体间通信
            ////1.应在当前UI线程添加事件，而不是new一个UI类去操作UI--其实是新开了一个子窗口即新的子UI线程，操作的并不是原来的UI线程(此UI子线程会在执行完毕自动回收即窗口退出)
            ////WindowsFormsWCFHostClient.Form1 newForm1 = new WindowsFormsWCFHostClient.Form1();
            ////Form1.newForm1 = new WindowsFormsWCFHostClient.Form1();
            ////Form1.newForm1 = new WindowsFormsWCFHostClient.Form2();//从不是创建他的线程访问richTextBox1控件，导致线程间访问异常
            //Form1.newForm1.Name = "newForm1";
            //Form1.newForm1.Text = "newForm1";
            //Form1.newForm1.Show();//新开窗口显示
            ////WindowsFormsWCFHostClient.Form1.ShowMessageDelegate showMessageDelegate = new WindowsFormsWCFHostClient.Form1.ShowMessageDelegate(Form1.newForm1.ShowMessage);
            //WindowsFormsWCFHostClient.Form2.ShowMessageDelegate showMessageDelegate = new WindowsFormsWCFHostClient.Form2.ShowMessageDelegate(Form1.newForm1.ShowMessage);

            //Form1.newForm1.Invoke(showMessageDelegate, message);

            #endregion

            //Form1.newForm1.Show();//

            //事件注册的方式调用--//通过事件订阅发布进行发布消息
            if (this.ReceiveMessageEvent != null)
            {
                this.ReceiveMessageEvent(message);
            }
        }

    }
}
