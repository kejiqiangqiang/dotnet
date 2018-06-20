using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 插件任务线程控制接口
    /// </summary>
    [ServiceContract]
    public interface ITaskControlService
    {
        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<PluginModel> GetPlugins();

        /// <summary>
        /// 获取插件的配置文件
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        [OperationContract]
        string GetPluginConfig(string pluginId);

        /// <summary>
        /// 修改插件的配置文件
        /// </summary>
        /// <param name="pluginId">插件编号</param>
        /// <param name="xmlConfig">xml配置文件</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool SaveConfig(string pluginId, string xmlConfig);

        /// <summary>
        /// 获取插件下的线程信息
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<TaskModel> GetPluginTasks(string pluginId);

        /// <summary>
        /// 控制当前插件中的线程
        /// </summary>
        /// <param name="pluginId">插件编号</param>
        /// <param name="taskId">任务编号</param>
        /// <param name="type">控制类型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool Control(string pluginId, string taskId, TaskControlType type);

    }

    /// <summary>
    /// 控制类型
    /// </summary>
    public enum TaskControlType
    {
        /// <summary>
        /// 无任何操作
        /// </summary>
        None = 0,

        /// <summary>
        /// 重置线程
        /// </summary>
        ReSet = 1,

        /// <summary>
        /// 挂起全部线程
        /// </summary>
        Suspend = 2,

        /// <summary>
        /// 继续运行被挂起的线程
        /// </summary>
        Resume = 3,

        /// <summary>
        /// 终止全部线程
        /// </summary>
        Abort = 4,

        /// <summary>
        /// 开始全部线程
        /// </summary>
        Start = 5
    }

}
