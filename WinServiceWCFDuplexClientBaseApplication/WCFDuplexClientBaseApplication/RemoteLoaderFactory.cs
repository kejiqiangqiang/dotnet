using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// Interface that can be run over the remote AppDomain boundary.
    /// </summary>
    internal interface IRemoteInterface
    {
        object Invoke();
    }

    /// <summary>
    /// Factory class to create objects exposing IRemoteInterface
    /// </summary>
    internal class RemoteLoaderFactory : MarshalByRefObject
    {
        private const BindingFlags bfi = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;

        public RemoteLoaderFactory() { }

        /// <summary> Factory method to create an instance of the type whose name is specified,
        /// using the named assembly file and the constructor that best matches the specified parameters. </summary>
        /// <param name="assemblyFile"> The name of a file that contains an assembly where the type named typeName is sought. </param>
        /// <param name="typeName"> The name of the preferred type. </param>
        /// <param name="constructArgs"> An array of arguments that match in number, order, and type the parameters of the constructor to invoke, or null for default constructor. </param>
        /// <returns> The return value is the created object represented as ILiveInterface. </returns>
        public object Create(string assemblyFile, string typeName, object[] constructArgs)
        {
            return Activator.CreateInstanceFrom(assemblyFile, typeName, false, bfi, null, constructArgs, null, null, null).Unwrap();
        }

        /// <summary>
        /// 加载服务插件程序集
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="baseTypeName">基类路径</param>
        /// <returns>对象</returns>
        public object LoadFramePlugin(string path,string baseType)
        {
            try
            {
                Assembly abs = Assembly.LoadFrom(path);
                Type[] types = abs.GetTypes();
                foreach (Type type in types)
                {
                    if (type.BaseType != null && type.BaseType.FullName == baseType)
                    {
                        var instance = this.Create(path, type.FullName, null);
                        return instance;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}