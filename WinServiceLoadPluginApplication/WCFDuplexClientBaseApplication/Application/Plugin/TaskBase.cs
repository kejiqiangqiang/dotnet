using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public abstract class TaskBase : MarshalByRefObject, IDisposable
    {
        Thread Thread = null;

        
        /// <summary>
        /// 线程任务编号
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public abstract string TaskName { get; }

        /// <summary>
        /// 获取当前插件路径
        /// </summary>
        public string TaskPath { get; set; }

        /// <summary>
        /// 当前插件信息
        /// </summary>
        public PluginBase Plugin { get; set; }

        /// <summary>
        /// 线程执行间隔时间，以秒为单位
        /// </summary>
        public int IntervalTime { set; get; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public TaskStatus Status { set; get; }

        /// <summary>
        /// 累计出错次数
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 最后一次出错时间
        /// </summary>
        public DateTime? LastErrorTime { get; set; }

        /// <summary>
        /// 最后一次出错信息
        /// </summary>
        public string LastErrorInfo { get; set; }

        /// <summary>
        /// 最后一次执行时间
        /// </summary>
        public int ExecSecond { get; set; }

        private int ExecCount = 0;

        /// <summary>
        /// 配置文件
        /// </summary>
        public Dictionary<string, ConfigModel> Config { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialization()
        {

        }

        /// <summary>
        /// 线程服务
        /// </summary>
        private void DoService()
        {
            while (true)
            {
                try
                {
                    this.Service(DateTime.Now);
                }
                catch (ThreadAbortException)
                {

                }
                catch (Exception e)
                {
                    //写异步日志
                    this.Plugin.WriteError(this.TaskId, e);
                    this.LastErrorTime = DateTime.Now;
                    this.LastErrorInfo = e.Message + "," + e.StackTrace;
                    this.ErrorCount = this.ErrorCount + 1;
                }
                ExecCount++;
                int intervalTime = this.IntervalTime == 0 ? 1 : this.IntervalTime;
                Thread.Sleep(intervalTime * 1000);
            }
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        public void Start()
        {
            if (this.Thread == null || this.Thread.ThreadState == ThreadState.Aborted)
            {
                this.Thread = new Thread(new ThreadStart(this.DoService));
            }
            if (this.Thread.ThreadState == ThreadState.Unstarted)
            {
                this.Thread.IsBackground = true;
                this.Thread.Start();
            }
            this.Status = TaskStatus.Runing;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public abstract void Service(DateTime now);

        /// <summary>
        /// 重置插件
        /// </summary>
        public virtual void ReSet()
        {
            this.Config = PluginConfig.ReadPluginConfigs(this.TaskPath, "Group");
        }

        /// <summary>
        /// 终止线程
        /// </summary>
        public virtual void Abort()
        {
            this.Dispose();
            if (this.Thread.ThreadState == (ThreadState.Aborted | ThreadState.Background))
            {
                return;
            }
            this.Status = TaskStatus.Stop;
            this.Thread.Abort();
        }

        /// <summary>
        /// 挂起全部线程
        /// </summary>
        public void Suspend()
        {
            if (this.Thread.ThreadState == ThreadState.Suspended)
            {
                return;
            }
            this.Thread.Suspend();
            this.Status = TaskStatus.Suspend;
        }

        /// <summary>
        /// 继续挂起全部线程
        /// </summary>
        public virtual void Resume()
        {
            if (this.Thread.ThreadState == (ThreadState.Suspended | ThreadState.Background))
            {
                this.Thread.Resume();
            }
            this.Status = TaskStatus.Runing;
        }

        /// <summary>
        /// 任务销毁时执行
        /// </summary>
        public virtual void Dispose()
        {

        }

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="e">错误信息</param>
        public void WriteError(Exception e)
        {
            this.ErrorCount++;
            this.LastErrorInfo = e.Message;
            this.LastErrorTime = DateTime.Now;
            if (this.Plugin != null)
            {
                this.Plugin.WriteError(this.TaskId, e);
            }
        }

        /// <summary>
        /// 写消息到客户端
        /// </summary>
        public void WatchMessage(string msg, MessageType msgType)
        {
            if (this.Plugin != null)
            {
                this.Plugin.WatchMessage(this.TaskId, msg, msgType);
            }
        }

        /// <summary>
        /// 对象 无限生存期
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            //对象 无限生存期
            return null;
        }

       
    }
}