using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Task.Master
{
    /// <summary>
    /// 插件的环境参数
    /// </summary>
    public static class ServiceContext
    {
        /// <summary>
        /// 运营Redis地址
        /// </summary>
        public static string MasterRedisPath { get; set; }

        /// <summary>
        /// 当前运营数据库地址
        /// </summary>
        public static string SQLConnectionString { get; set; }
        /// <summary>
        /// 网易短信appKey
        /// </summary>
        public static string AppKey { get; set; }
        /// <summary>
        /// 网易短信appSecret
        /// </summary>
        public static string AppSecret { get; set; }
        /// <summary>
        /// 网易短信templateid
        /// </summary>
        public static string TemplateId { get; set; }
    }
}