using System.Runtime.Serialization;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 消息实体数据协定
    /// </summary>
    [DataContract]
    public class MessageModel
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        [DataMember]
        public string MessageType { get; set; }
        /// <summary>
        /// 任务编号
        /// </summary>
        [DataMember]
        public string TaskId { get; set; }
        /// <summary>
        /// 插件编号
        /// </summary>
        [DataMember]
        public string PluginId { get; set; } 
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
