using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCFDuplexClientBaseApplication;

namespace WindowsService
{
    public partial class TaskService : ServiceBase
    {
        public TaskService()
        {
            InitializeComponent();
            //initial log
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        /// <summary>
        /// 1.计时器定时引发事件
        /// 服务应用程序设计为长时间运行的，所以它通常轮询或监视系统中的情况。 
        /// 监视是在 OnStart 方法中设置的。 但是， OnStart 实际上不进行监视。 
        /// 服务的操作开始之后， OnStart 方法必须返回到操作系统。 它不能始终循环或阻止。 
        /// 若要设置简单的轮询机制，可以使用 System.Timers.Timer 组件，如下所示：在 OnStart 方法中，在组件上设置参数，然后将 Enabled 属性设置为 true。 计时器定期在你的代码中引发事件，此时你的服务可以进行监视
        /// 2.开启新的线程
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            //// Set up a timer to trigger every minute.  
            //this.RunTimmer();

            //异步快速启动
            ThreadPool.QueueUserWorkItem(s =>
            {
                ServerApplication.StartApplication();
            });
        }

        protected override void OnStop()
        {
            //ServerApplication.UnloadPlugin();
        }

        public void RunTimmer()
        {
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();  
        }
        private int eventId = 1;
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }  

    }
}
