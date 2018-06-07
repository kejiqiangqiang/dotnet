using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsWCFHostClient.Client;

namespace WindowsFormsWCFHostClient
{
    public partial class Form1 : Form
    {
        #region 使用事件进行窗体间通信

        //子窗口UI线程显示回调信息
        //public static WindowsFormsWCFHostClient.Form1 newForm1 = null;
        public static WindowsFormsWCFHostClient.Form2 newForm1 = new WindowsFormsWCFHostClient.Form2();//null;//从不是创建他的线程访问richTextBox1控件，导致线程间访问异常//使用事件
        


        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //可直接操作UI
            this.richTextBox1.AppendText("开始"+ "\n");

            //3.SynchronizationContext与某个线程绑定,以SendOrPostCallback委托的形式将相应的操作封送到SynchronizationContext对应的线程中执行
            SynchronizationContext syncContext = SynchronizationContext.Current;
            
            #region 线程池中

            ////在线程池中执行事件回调操作UI
            //ThreadPool.QueueUserWorkItem(s =>
            //{
            //    //1.线程池中执行事件回调操作UI
            //    Service01Client service1Client = new Service01Client();
            //    service1Client.ReceiveMessageEvent += ShowMessage;//线程池中执行事件回调操作UI
            //    //服务端方法内有回调客户端方法--//事件注册的方式调用:this.ReceiveMessageEvent(message);//--服务端调用时切换到UI线程调用ShowMessage操作UI//并阻塞服务端getValue方法，直到执行完//然后服务端方法getValue返回值--不会阻塞，是异步执行的(IsOneWay)
            //    string v = service1Client.getValue(100);//服务方法调用返回与客户端回调方法执行阻塞--不会阻塞，是异步执行的(IsOneWay)
                
            //    //线程池中则不能直接操作UI
            //    //this.richTextBox1.AppendText("服务返回值：" + v + "\n");

            //    //2.线程池中委托操作UI
            //    this.ShowMessage("线程池中委托操作UI--服务返回值：" + v + "\n");

            //    //3.线程池中通过同步上下文可在另一个线程中直接操作UI
            //    syncContext.Post(c1 => {
            //        this.richTextBox1.AppendText("线程池中通过同步上下文可在另一个线程中直接操作UI--服务返回值：" + v + "\n");
            //    },null);

            //});

            //可直接操作UI
            //this.richTextBox1.AppendText("\"完成\"--线程池中方法并未完成(异步)" + "\n");

            #endregion

            #region //服务端方法内有回调客户端方法并且操作UI时卡死//4.--回调接口的实现类ConcurrencyMode.Reentrant，即可解决回调操作UI卡死问题

            //4.--回调接口的实现类ConcurrencyMode.Reentrant，即可解决回调操作UI卡死问题
            Service01Client service1Client = new Service01Client();
            //--通过事件订阅发布进行订阅主题
            service1Client.ReceiveMessageEvent += ShowMessage;//直接执行事件回调操作UI卡死
            //service1Client.ReceiveMessageEvent += newForm1.ShowMessage_Form1ToForm2;//
            //newForm1.Show();//卡死

            //服务端方法内有回调客户端方法--//事件注册的方式调用:this.ReceiveMessageEvent(message);//--服务端调用时切换到UI线程调用ShowMessage操作UI//并阻塞服务端getValue方法，直到执行完//然后服务端方法getValue返回值--不会阻塞，是异步执行的(IsOneWay)
            string v = service1Client.getValue(100);//服务方法调用返回与客户端回调方法执行阻塞--不会阻塞，是异步执行的(IsOneWay)

            //可直接操作UI
            this.richTextBox1.AppendText("客户端回调执行与服务端方法执行返回为异步（通过回调方法协定为IsOneWay异步）--服务返回值："+v+"\n");

            #endregion

        }

        /// <summary>
        /// 窗体间跨线程操作UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //先将主窗口数据在子窗口显示出来
            newForm1.ShowMessage_Form1ToForm2(this.richTextBox1.Text);

            newForm1.ChangeTextBoxValue += ShowMessage;
            //newForm1.ShowDialog();//1.关闭子窗口不释放资源2.子窗口没有关闭焦点无法到主窗口
            newForm1.Show();//1.关闭子窗口释放资源2.焦点无限制

            //先将主窗口数据在子窗口显示出来
            //newForm1.ShowMessage_Form1ToForm2(this.richTextBox1.Text);

        }

        /// <summary>
        /// //当服务端通过事件注册此方法并回调至此方法此处时，UI界面卡死--
        /// 
        /// #region 3.ynchronizationContext与某个线程绑定,以SendOrPostCallback委托的形式将相应的操作封送到SynchronizationContext对应的线程中执行
        /// //3.通过同步上下文可在另一个线程中直接操作UI
        /// //    改写public void ShowMessage(string message)方法
        /// //    syncContext.Post(c1 => {
        /// //       this.richTextBox1.AppendText(message + "\n");
        /// //    },null);
        /// #endregion
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                //当服务端通过事件注册此方法并回调至此方法此处时，UI界面卡死--？？？
                this.richTextBox1.Invoke(new ShowMessageDelegate(ShowMessage), new object[] { message });
            }
            else
            {
                this.richTextBox1.AppendText(message + "\n");
            }
        }

        public delegate void ShowMessageDelegate(string message);

    }
}
