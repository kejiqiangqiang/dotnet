using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 插件基类
    /// </summary>
    public abstract class PluginBase : MarshalByRefObject
    {
        private Log log = null;

        /// <summary>
        /// 插件编号
        /// </summary>
        public string PluginId { get; set; }
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; set; }
        /// <summary>
        /// 插件路径
        /// </summary>
        public string PluginPath { get; set; }
        /// <summary>
        /// 插件配置文件
        /// </summary>
        public Dictionary<string, ConfigModel> Config { get; private set; }
        /// <summary>
        /// 插件下线程是否开启配置(默认开启)
        /// </summary>
        public Dictionary<string, ConfigModel> TaskEnabled { get; private set; }

        /// <summary>
        /// 当前插件下线程列表
        /// </summary>
        public List<TaskBase> Tasks { get; set; }
        /// <summary>
        /// 插件应用程序域
        /// </summary>
        public AppDomain PluginDomain { get; set; }
        /// <summary>
        /// win服务框架主应用程序域
        /// </summary>
        public AppDomain FrameDomain { get; set; }

        /// <summary>
        /// 获取或设置配置文件(XML字符串)
        /// </summary>
        public string ConfigXml
        {
            get { var xml = PluginConfig.ConvertToXml(this.Config, this.TaskEnabled); return xml; }
            set
            {
                try
                {
                    //set1
                    string xmlPath = this.PluginPath + "\\Config.xml";
                    File.WriteAllText(xmlPath, value);
                    //set2
                    this.Config = PluginConfig.ReadPluginConfigs(this.PluginPath, "Group");
                    //set3
                    foreach (var task in this.Tasks)
                    {
                        task.Config = this.Config;
                    }

                }
                catch (Exception e)
                {
                     this.WriteError(e);
                    throw e;
                }
            }
        }

        /// <summary>
        /// 初始化属性
        /// </summary>
        /// <param name="path">当前路径</param>
        public void Initialize(string path)
        {
            //拷贝配置文件到插件目录
            this.PluginPath = path;
            //配置文件
            this.Config = PluginConfig.ReadPluginConfigs(this.PluginPath, "Group");
            //线程是否开启
            this.TaskEnabled = PluginConfig.ReadPluginConfigs(this.PluginPath, "Enabled");
            //插件编号
            this.PluginId = new DirectoryInfo(this.PluginPath).Name.ToUpper();
            var index = this.PluginPath.ToUpper().IndexOf("PlugIn.".ToUpper()) + "PlugIn.".Length;
            this.PluginName = this.PluginPath.Substring(index, this.PluginPath.Length - index);
            this.Tasks = new List<TaskBase>();
        }

        /// <summary>
        /// 初始化任务列表
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 增加线程任务
        /// </summary>
        /// <param name="task">任务</param>
        public void AddTask(TaskBase task)
        {
            var className = task.GetType().Name;
            //默认（若没有配置）加入启用线程配置
            if (!this.TaskEnabled.ContainsKey(className))
            {
                this.TaskEnabled.Add(className, new ConfigModel { Key = className, TextName = task.TaskName, Value = "true", Group = "Enabled" });
            }
            //如果有配置，若不是配置为启用，则不启用
            if (this.TaskEnabled[className].Value.ToLower() != "true")
            {
                return;
            }

            task.Config = this.Config;
            task.Plugin = this;
            task.TaskId = Guid.NewGuid().ToString();
            task.TaskPath = this.PluginPath;
            //task.IntervalTime = 60;//任务子类中自行设置线程任务执行频率
            this.Tasks.Add(task);

            ////异步初始化线程，启动线程
            //ThreadPool.QueueUserWorkItem(s =>
            //{
            //    try
            //    {
            //        //初始化并启动任务线程
            //        task.Initialization();
            //        task.Start();
            //    }
            //    catch (Exception ex)
            //    {
            //        var ee = new Exception(task.TaskName + "任务启动失败," + ex.Message + ex.StackTrace);
            //        task.ErrorCount = 1;
            //        task.LastErrorInfo = ex.Message;
            //        task.LastErrorTime = DateTime.Now;
            //        this.WriteError(ee);
            //    }
            //});
        }

        /// <summary>
        /// //异步初始化线程，启动线程
        /// </summary>
        public void StartTasks()
        {
            foreach (var task in this.Tasks)
            {

                //异步初始化线程，启动线程
                ThreadPool.QueueUserWorkItem(s =>
                {
                    try
                    {
                        //初始化并启动任务线程
                        task.Initialization();
                        task.Start();
                    }
                    catch (Exception ex)
                    {
                        var ee = new Exception(task.TaskName + "任务启动失败," + ex.Message + ex.StackTrace);
                        task.ErrorCount = 1;
                        task.LastErrorInfo = ex.Message;
                        task.LastErrorTime = DateTime.Now;
                        this.WriteError(ee);
                    }
                });
            }
        }


        /// <summary>
        /// 输出消息监视到客户端
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        internal void WatchMessage(string taskId,string msg,MessageType msgType)
        {
            ServerApplication.SendMessage(new MessageModel(){
             PluginId=this.PluginId, TaskId=taskId, Message=msg, MessageType=msgType.ToString()
            },this.FrameDomain);
        }

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="e">错误信息</param>
        public void WriteError(Exception e)
        {
            if (log == null)
            {
                log = new Log(this.PluginPath);
            }
            //写异步日志
            log.msg = e.Message + System.Environment.NewLine + e.StackTrace + System.Environment.NewLine + "==================================================================================================";
            log.WriteLog();
            ////并打印消息到客户端
            //this.WatchMessage("", (e.Message + Environment.NewLine + "堆栈信息" + e.StackTrace), MessageType.Error);
        } 

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="e"></param>
        /// <param name="taskId">线程编号</param>
        internal void WriteError(string taskId, Exception e)
        {
            if (log == null)
            {
                log = new Log(this.PluginPath);
            }
            //写异步日志
            log.msg = e.Message + System.Environment.NewLine + e.StackTrace + System.Environment.NewLine + "==================================================================================================";
            log.WriteLog();
            //并打印消息到客户端
            this.WatchMessage(taskId, (e.Message + Environment.NewLine + "堆栈信息" + e.StackTrace), MessageType.Error);
        }

        /// <summary>
        /// 无限生命周期
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {

        }
        
    }
}