using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 停止状态
        /// </summary>
        Stop = 0,

        /// <summary>
        /// 运行状态
        /// </summary>
        Runing = 1,

        /// <summary>
        /// 挂起
        /// </summary>
        Suspend = 2,
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 消息
        /// </summary>
        Message = 1,
        /// <summary>
        /// 警告
        /// </summary>
        Warning = 2,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 3
    }
}
