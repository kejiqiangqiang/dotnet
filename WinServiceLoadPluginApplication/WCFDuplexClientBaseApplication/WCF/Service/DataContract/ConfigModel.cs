using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 配置文件
    /// </summary>
    [DataContract]
    public class ConfigModel
    {
        /// <summary>
        /// 数据Key
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        [DataMember]
        public string TextName { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        [DataMember]
        public string Group { get; set; }
    }
}