using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 插件配置文件
    /// </summary>
    public static class PluginConfig
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="path">插件路径</param>
        /// <param name="groupName">配置节点名称</param>
        /// <returns>配置文件信息</returns>
        internal static Dictionary<string, ConfigModel> ReadPluginConfigs(string path, string groupName)
        {
            Dictionary<string, ConfigModel> configs = new Dictionary<string, ConfigModel>();
            string xmlPath = path + "\\Config.xml";
            if (File.Exists(xmlPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlNodeList groupNodes = doc.GetElementsByTagName(groupName);
                if (groupNodes.Count > 0)
                {
                    foreach (XmlNode group in groupNodes)
                    {
                        foreach (XmlNode item in group.ChildNodes)
                        {
                            if (item.Attributes != null)
                            {
                                string key = item.Name;
                                string TextName = item.Attributes["TextName"].Value;
                                string Value = item.Attributes["Value"].Value;
                                string Type = item.Attributes["Type"] != null ? item.Attributes["Type"].Value : "";
                                string Group = group.Attributes["TextName"].Value;
                                configs.Add(key, new ConfigModel { Key = key, Group = Group, TextName = TextName, Type = Type, Value = Value });
                            }
                        }
                    }
                }
            }
            return configs;
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="xml">xml配置</param>
        /// <returns>配置文件信息</returns>
        public static Dictionary<string, ConfigModel> ConvertToModel(string xml, string groupName)
        {
            Dictionary<string, ConfigModel> configs = new Dictionary<string, ConfigModel>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList groupNodes = doc.GetElementsByTagName(groupName);
            if (groupNodes.Count > 0)
            {
                foreach (XmlNode group in groupNodes)
                {
                    foreach (XmlNode item in group.ChildNodes)
                    {
                        if (item.Attributes != null)
                        {
                            string key = item.Name;
                            string TextName = item.Attributes["TextName"].Value;
                            string Value = item.Attributes["Value"].Value;
                            string Type = item.Attributes["Type"] != null ? item.Attributes["Type"].Value : "";
                            string Group = group.Attributes["TextName"].Value;
                            configs.Add(key, new ConfigModel { Key = key, Group = Group, TextName = TextName, Type = Type, Value = Value });
                        }
                    }
                }
            }
            return configs;
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="config">xml配置</param>
        /// <returns>配置文件信息</returns>
        public static string ConvertToXml(Dictionary<string, ConfigModel> config, Dictionary<string, ConfigModel> enabled)
        {
            List<string> groupNames = config.Values.ToList().Select(p => p.Group).Distinct().ToList();
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + Environment.NewLine);
            xml.Append("<Config>" + Environment.NewLine);
            foreach (string groupName in groupNames)
            {
                xml.Append("  <Group TextName=\"" + groupName + "\">" + Environment.NewLine);
                List<ConfigModel> groupConfigs = config.Where(p => p.Value.Group == groupName).Select(p => p.Value).ToList();
                foreach (var item in groupConfigs)
                {
                    xml.Append("    <" + item.Key + "   TextName=\"" + item.TextName + "\" Value=\"" + item.Value + "\"  Type=\"" + item.Type + "\" />" + Environment.NewLine);
                }
                xml.Append("  </Group>" + Environment.NewLine);
            }

            xml.Append("  <Enabled TextName = \"线程启用(默认启用)\" >" + Environment.NewLine);
            foreach (var key in enabled.Keys)
            {
                xml.Append("    <" + enabled[key].Key + " TextName = \"" + enabled[key].TextName + "\"  Value = \"" + enabled[key].Value + "\" />" + Environment.NewLine);
            }
            xml.Append("  </Enabled >" + Environment.NewLine);

            xml.Append("</Config>" + Environment.NewLine);
            return xml.ToString();
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="path">插件路径</param>
        /// <returns>配置文件信息</returns>
        internal static bool WritePluginConfigs(Dictionary<string, ConfigModel> config, Dictionary<string, ConfigModel> enabled, string path)
        {
            string xml = ConvertToXml(config, enabled);
            string xmlPath = path + "\\Config.xml";
            File.WriteAllText(xmlPath, xml, Encoding.UTF8);
            return true;
        }
    }
}