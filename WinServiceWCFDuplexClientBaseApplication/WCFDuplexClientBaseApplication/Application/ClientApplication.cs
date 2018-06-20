using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 客户端代理程序
    /// </summary>
    public class ClientApplication
    {
        /// <summary>
        /// 获取服务插件列表
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static List<PluginModel> GetPlugins(string uri)
        {
            var models = new List<PluginModel>();
            EndpointAddress remoteAddress = new EndpointAddress("net.tcp://"+uri+"/TaskControlService");
            using (TaskControlClient client = new TaskControlClient(new NetTcpBinding(SecurityMode.None),remoteAddress))
            {
                models = client.GetPlugins().ToList();
            }
            return models;
        }



    }
}