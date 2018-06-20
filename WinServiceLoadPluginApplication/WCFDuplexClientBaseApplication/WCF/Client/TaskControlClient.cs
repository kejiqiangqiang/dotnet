using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 控制客户端
    /// </summary>
    public class TaskControlClient : ClientBase<ITaskControlService>, ITaskControlService
    {
        /// <summary>
        /// 由B+C构造客户端
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="remoteAddress"></param>
        public TaskControlClient(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        { 
        }
        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PluginModel> GetPlugins()
        {
            return base.Channel.GetPlugins();
        }

        /// <summary>
        /// 获取插件的配置文件
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public string GetPluginConfig(string pluginId)
        {
            return base.Channel.GetPluginConfig(pluginId);
        }

        /// <summary>
        /// 修改插件的配置文件
        /// </summary>
        /// <param name="pluginId">插件编号</param>
        /// <param name="xmlConfig">xml配置文件</param>
        /// <returns>是否成功</returns>
        public bool SaveConfig(string pluginId, string xmlConfig)
        {
            return base.Channel.SaveConfig(pluginId,xmlConfig);
        }

        /// <summary>
        /// 获取插件下的线程信息
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public IEnumerable<TaskModel> GetPluginTasks(string pluginId)
        {
            return base.Channel.GetPluginTasks(pluginId);
        }

        /// <summary>
        /// 控制当前插件中的线程
        /// </summary>
        /// <param name="pluginId">插件编号</param>
        /// <param name="taskId">任务编号</param>
        /// <param name="type">控制类型</param>
        /// <returns>是否成功</returns>
        public bool Control(string pluginId, string taskId, TaskControlType type)
        {
            return base.Channel.Control(pluginId,taskId,type);
        }

    }

}
