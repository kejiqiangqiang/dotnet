using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 客户端、服务端、Win服务三者纽带程序
    /// 静态变量保存插件信息，供后续WCF客户端代理与WCF应用程序通讯
    /// </summary>
    public class ServerApplication
    {
        /// <summary>
        /// //WCF服务宿主
        /// </summary>
        public static List<ServiceHost> ServiceHosts { get; set; }
        /// <summary>
        /// 发送消息给客户端
        /// </summary>
        static MessageService messageServer = null;
        
        ///// <summary>
        ///// 插件列表
        ///// </summary>
        //public static List<FramePlugin> Plugins { get; set; }
        public static List<string> Plugins { get; set; }

        /// <summary>
        /// win服务中加载并运行应用程序（插件及其任务线程）及其启动WCF宿主
        /// </summary>
        public static void StartApplication()
        {
            //加载运行插件及其任务线程
            ServerApplication.LoadPlugin();//下个版本实现加载插件并运行任务线程//暂时仅执行简单操作
            //宿主初始化WCF服务
            ServerApplication.InitialWCFHost();

            //模拟运行插件并发送消息到客户端
            ServerApplication.RunTimmer();

        }

        /// <summary>
        /// 宿主初始化WCF服务
        /// </summary>
        public static void InitialWCFHost()
        {
            string serverHost = ServiceConfig.Host;
            int port = ServiceConfig.Port;

            //控制服务
            ServiceHost controlHost = WCFHost.TcpHost(typeof(TaskControlService), typeof(ITaskControlService), serverHost, port, "mex");
            //消息服务
            ServiceHost messageHost = WCFHost.TcpHost(typeof(MessageService), typeof(IMessageService), serverHost, port, "mex");

            ServerApplication.ServiceHosts = new List<ServiceHost>();
            ServerApplication.ServiceHosts.Add(controlHost);
            ServerApplication.ServiceHosts.Add(messageHost);
        }


        #region 服务端插件中实现--此处演示

        public static void RunTimmer()
        {
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(ServerApplication.OnTimer);
            timer.Start();
        }
        public static void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
            ServerApplication.WatchMessage("taskId", "接收到服务端主动发送的消息", MessageType.Message);
        }  

        /// <summary>
        /// 输出消息监视到客户端
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <param name="msg">消息</param>
        /// <param name="msgType">消息类型</param>
        internal static void WatchMessage(string taskID, string msg, MessageType msgType)
        {
            ServerApplication.SendMessage(new MessageModel
            {
                TaskId = taskID,
                PluginId = "PluginId",
                MessageType = msgType.ToString(),
                Message = msg,
            }, AppDomain.CurrentDomain);
        }

        #endregion

        /// <summary>
        /// 发消息给客户端
        /// </summary>
        /// <param name="model">消息对象</param>
        /// <param name="appDomain">主程序</param>
        public static void SendMessage(MessageModel model, AppDomain appDomain)
        {
            if (messageServer==null)
            {
                //1.直接通过new创建服务对象来调用服务的有回调客户端的方法
                messageServer = new MessageService();

                ////2.在AppDomain中建立程序集中指定类的对象
                ////按引用封送方式
                ////如果一种类型继承自MarshalByRefObject就标志着此类型所实例化出来的对象可以按引用封送传递到另一应用程序域中，所谓引用封送就是将对象传递到另一AppDomain后对对象所做的修改照实的反应到原对象身上，而不是创建副本对副本进行修改。这里面有个意外就是静态字段，静态字段的数据总是保存在当前应用程序域的Loader堆中，对静态字段的访问没有办法创建代理，所以就意味着按引用封送的类型的静态字段并没有引用原对象的静态字段，对目标类型的静态字段所做的修改也不会反映到原对象中。
                //string path = appDomain.BaseDirectory.TrimEnd('/').TrimEnd('\\') + "\\";
                //RemoteLoaderFactory factory = (RemoteLoaderFactory)appDomain.CreateInstance("WCFDuplexClientBaseApplication", "WCFDuplexClientBaseApplication.RemoteLoaderFactory").Unwrap();
                //messageServer = (MessageService)factory.Create(path + "WCFDuplexClientBaseApplication.dll", "WCFDuplexClientBaseApplication.MessageService", null);

            }
            messageServer.SendMessage(model.PluginId, model.TaskId, model.MessageType, model.Message);
        }

        /*---------------//下个版本实现加载插件并运行任务线程//暂时仅执行简单操作------------*/
        /// <summary>
        /// 加载服务插件
        /// </summary>
        public static void LoadPlugin()
        {
            ////加载服务插件
            //ServerApplication.Plugins = new List<FramePlugin>();
            //List<DirectoryInfo> dirs = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetDirectories().Where(p => p.Name.ToUpper().IndexOf("PlugIn.".ToUpper()) > 0).ToList();
            //foreach (DirectoryInfo item in dirs)
            //{
            //    Stopwatch st = new Stopwatch();
            //    st.Start();
            //    FramePlugin plugin = ServerApplication.ReadFramePluginItem(item.FullName);
            //    st.Stop();
            //    if (plugin != null)
            //    {
            //        Plugins.Add(plugin);
            //        logsw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",插件：" + plugin.PluginName + " 加载成功，耗时：" + st.ElapsedMilliseconds / 1000.0 + "秒。");
            //        logsw.Flush();
            //    }
            //}


            //下个版本实现加载插件并运行任务线程//暂时仅执行简单操作
            ServerApplication.Plugins = new List<string>();
            ServerApplication.Plugins.Add("从当前应用程序域下文件夹列表读取特定文件夹下的服务插件程序集");
        }

    }
}