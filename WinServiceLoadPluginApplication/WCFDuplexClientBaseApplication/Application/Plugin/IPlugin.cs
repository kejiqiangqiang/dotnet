using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin:IDisposable
    {
        /// <summary>
        /// 插件编号
        /// </summary>
        string PluginId { get; set; }
        /// <summary>
        /// 插件名称
        /// </summary>
        string PluginName { get; set; }
        /// <summary>
        /// 插件路径
        /// </summary>
        string PluginPath { get; set; }
        /// <summary>
        /// 插件配置文件
        /// </summary>
        Dictionary<string, ConfigModel> Config { get; set; }
    }
}
