using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 服务配置
    /// </summary>
    public static class ServiceConfig
    {
        static DataTable data = new DataTable("Server");

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        static ServiceConfig()
        {
            data.Columns.Add("Host");
            data.Columns.Add("ServiceDependedOns");
            data.Columns.Add("ServiceName");
            data.Columns.Add("Port");

            var configPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + "\\Config.xml";
            data.ReadXml(configPath);

            ServiceConfig.Host = data.Rows[0]["Host"].ToString();
            ServiceConfig.ServiceDependedOns = data.Rows[0]["ServiceDependedOns"].ToString();
            ServiceConfig.ServiceName = data.Rows[0]["ServiceName"].ToString();
            ServiceConfig.Port = int.Parse(data.Rows[0]["Port"].ToString());
        }

        /// <summary>
        /// 地址
        /// </summary>
        public static string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public static int Port { get; set; }

        /// <summary>
        /// 服务启动依赖项
        /// </summary>
        public static string ServiceDependedOns { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public static string ServiceName { get; set; }
    }
}