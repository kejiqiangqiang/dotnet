using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 插件任务线程控制服务端
    /// </summary>
    [ServiceBehavior]
    public class TaskControlService : ITaskControlService
    {
        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PluginModel> GetPlugins()
        {
            List<PluginModel> models = new List<PluginModel>();
            //foreach (var item in ServerApplication.Plugins)
            //{
            //    PluginModel model = new PluginModel();
            //    model.PluginName = item.PluginName;
            //    model.PluginId = item.PluginId;
            //    model.PluginPath = item.PluginPath;
            //    models.Add(model);
            //}
            return models;
        }

    }
}
