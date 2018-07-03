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
            foreach (var item in ServerApplication.Plugins)
            {
                PluginModel model = new PluginModel();
                model.PluginName = item.PluginName;
                model.PluginId = item.PluginId;
                model.PluginPath = item.PluginPath;
                models.Add(model);
            }
            ////简单操作处理
            //foreach (var item in ServerApplication.Plugins)
            //{
            //    PluginModel model = new PluginModel();
            //    model.PluginName = "插件名称";
            //    model.PluginId = "插件Id";
            //    model.PluginPath = "插件路径";
            //    models.Add(model);
            //}
            return models;
        }

        /// <summary>
        /// 获取插件的配置文件
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public string GetPluginConfig(string pluginId)
        {
            string xml = null;
            PluginBase plugin = ServerApplication.Plugins.Where(p => p.PluginId == pluginId).FirstOrDefault();
            if (plugin!=null)
            {
                xml=plugin.ConfigXml;
            }
            return xml;
        }

        /// <summary>
        /// 修改插件的配置文件
        /// </summary>
        /// <param name="pluginId">插件编号</param>
        /// <param name="xmlConfig">xml配置文件</param>
        /// <returns>是否成功</returns>
        public bool SaveConfig(string pluginId, string xmlConfig)
        {
            PluginBase plugin = ServerApplication.Plugins.Where(p => p.PluginId == pluginId).FirstOrDefault();
            if (plugin != null)
            {
                plugin.ConfigXml = xmlConfig;
            }
            return true;
        }

        /// <summary>
        /// 获取插件下的线程信息
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public IEnumerable<TaskModel> GetPluginTasks(string pluginId)
        {
            List<TaskModel> taskModels = new List<TaskModel>();
            PluginBase plugin = ServerApplication.Plugins.Where(p => p.PluginId == pluginId).FirstOrDefault();
            if (plugin != null&&plugin.Tasks!=null)
            {
                foreach (var task in plugin.Tasks)
                {
                    taskModels.Add(new TaskModel
                    {
                        TaskId = task.TaskId,
                        TaskName = task.TaskName,
                        IntervalTime = task.IntervalTime,
                        Status = task.Status.ToString(),
                        ErrorCount = task.ErrorCount,
                        ExecSecond = task.ExecSecond,
                        LastErrorInfo = task.LastErrorInfo,
                        LastErrorTime = task.LastErrorTime
                    });
                }
            }
            return taskModels;
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
            PluginBase plugin = ServerApplication.Plugins.Where(p => p.PluginId == pluginId).FirstOrDefault();
            if (plugin != null && plugin.Tasks != null)
            {
                TaskBase task = plugin.Tasks.Where(p => p.TaskId == taskId).FirstOrDefault();
                if (task != null)
                {
                    switch (type)
                    {
                        case TaskControlType.None:
                            break;
                        case TaskControlType.ReSet:
                            task.ReSet();
                            break;
                        case TaskControlType.Suspend:
                            break;
                        case TaskControlType.Resume:
                            break;
                        case TaskControlType.Abort:
                            task.Abort();
                            break;
                        case TaskControlType.Start:
                            task.Start();
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
        }

    }
}
