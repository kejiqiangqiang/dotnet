using System;
using System.Runtime.Serialization;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 线程信息
    /// </summary>
    [DataContract]
    public class TaskModel
    {
        /// <summary>
        /// 线程任务编号
        /// </summary>
        [DataMember]
        public string TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string TaskName { get; set; }

        /// <summary>
        /// 执行间隔时间:秒
        /// </summary>
        [DataMember]
        public int IntervalTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [DataMember]
        public string Status { get; set; }
        
        /// <summary>
        /// 累计出错次数
        /// </summary>
        [DataMember]
        public int ErrorCount { get; set; }

        /// <summary>
        /// 最后一次出错时间
        /// </summary>
        [DataMember]
        public DateTime? LastErrorTime { get; set; }

        /// <summary>
        /// 最后一次出错信息
        /// </summary>
        [DataMember]
        public string LastErrorInfo { get; set; }

        /// <summary>
        /// 最后一次执行时间
        /// </summary>
        [DataMember]
        public int ExecSecond { get; set; }

    }
}
