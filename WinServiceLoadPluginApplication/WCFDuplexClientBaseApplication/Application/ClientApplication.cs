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
            EndpointAddress edpTcp = new EndpointAddress("net.tcp://" + uri + "/TaskControlService");
            using (TaskControlClient client = new TaskControlClient(new NetTcpBinding(SecurityMode.None), edpTcp))
            {
                IEnumerable<PluginModel> list = client.GetPlugins();
                if (list != null)
                {
                    models = list.ToList();
                }
            }
            return models;
        }
        /// <summary>
        /// 获取插件的线程列表
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public static List<TaskModel> GetPluginTasks(string uri, string pluginId)
        {
            var models = new List<TaskModel>();
            EndpointAddress edpTcp = new EndpointAddress("net.tcp://" + uri + "/TaskControlService");
            using (TaskControlClient client = new TaskControlClient(new NetTcpBinding(SecurityMode.None), edpTcp))
            {
                IEnumerable<TaskModel> list = client.GetPluginTasks(pluginId);
                if (list != null)
                {
                    models = list.ToList();
                }
            }
            return models;

        }
        /// <summary>
        /// 获取插件配置文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public static string GetPluginConfig(string uri,string pluginId)
        {
            string xml = null;
            EndpointAddress edpTcp = new EndpointAddress("net.tcp://" + uri + "/TaskControlService");
            using (TaskControlClient client = new TaskControlClient(new NetTcpBinding(SecurityMode.None), edpTcp))
            {
                xml = client.GetPluginConfig(pluginId);
            }
            return xml;
        }

        /// <summary>
        /// 修改配置文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="pluginId"></param>
        /// <param name="xml"></param>
        public static bool SavePluginConfig(string uri, string pluginId, string xml)
        {
            EndpointAddress edpTcp = new EndpointAddress("net.tcp://" + uri + "/TaskControlService");
            using (TaskControlClient client = new TaskControlClient(new NetTcpBinding(SecurityMode.None), edpTcp))
            {
                try
                {
                    client.SaveConfig(pluginId, xml);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 控制当前插件中的线程
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="pluginId"></param>
        /// <param name="taskId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Control(string uri,string pluginId,string taskId,TaskControlType type)
        {
            bool rs = false;
            EndpointAddress edpTcp = new EndpointAddress("net.tcp://" + uri + "/TaskControlService");
            using (TaskControlClient client = new TaskControlClient(new NetTcpBinding(SecurityMode.None), edpTcp))
            {
                rs=client.Control(pluginId, taskId, type);
            }
            return rs;
        }

    }
}