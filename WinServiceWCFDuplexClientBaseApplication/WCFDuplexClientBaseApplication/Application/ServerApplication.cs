using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 客户端、服务端、Win服务
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






        /// <summary>
        /// win服务中加载并运行应用程序（插件及其任务线程）及其启动WCF宿主
        /// </summary>
        public void StartApplication()
        {
            //加载运行插件及其任务线程
            this.LoadPlugin();//下个版本实现加载插件并运行任务线程//暂时仅执行简单操作
            //宿主初始化WCF服务
            this.InitialWCFHost();
        }

        /// <summary>
        /// 宿主初始化WCF服务
        /// </summary>
        public void InitialWCFHost()
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

        /// <summary>
        /// 发消息给客户端
        /// </summary>
        /// <param name="model">消息对象</param>
        /// <param name="appDomain">主程序</param>
        public static void SendMessage(MessageModel model, AppDomain appDomain)
        {
            if (messageServer==null)
            {
                //new MessageService
                messageServer = new MessageService();

                //

            }
            messageServer.SendMessage(model.PluginId, model.TaskId, model.MessageType, model.Message);
        }

        /*---------------//下个版本实现加载插件并运行任务线程//暂时仅执行简单操作------------*/
        /// <summary>
        /// 加载服务插件
        /// </summary>
        public void LoadPlugin()
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


        }

    }
}