using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 插件信息
    /// </summary>
    [DataContract]
    public class PluginModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public string PluginId { get; set; }

        /// <summary>
        /// 插件名称
        /// </summary>
        [DataMember]
        public string PluginName { get; set; }

        /// <summary>
        /// 插件路径
        /// </summary>
        [DataMember]
        public string PluginPath { get; set; }

        ///// <summary>
        ///// 配置信息
        ///// </summary>
        //[DataMember]
        //public string ConfigXml { get; set; }

        ///// <summary>
        ///// 线程任务集合
        ///// </summary>
        //[DataMember]
        //public IEnumerable<TaskModel> TaskModels { get; set; }

    }
}
