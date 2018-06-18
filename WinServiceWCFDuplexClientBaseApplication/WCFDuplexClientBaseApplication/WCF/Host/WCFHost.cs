using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// WCF宿主
    /// </summary>
    public class WCFHost
    {
        /// <summary>
        /// Tcp宿主
        /// </summary>
        /// <param name="instanceType">实例</param>
        /// <param name="interfaceType">接口</param>
        /// <param name="port">端口</param>
        /// <param name="mexName">元数据mexName</param>
        /// <returns>服务</returns>
        public static ServiceHost TcpHost(Type instanceType, Type interfaceType, string serverHost, int port, string mexName)
        {
            NetTcpBinding NetBinding = new NetTcpBinding();
            NetBinding.Security.Mode = SecurityMode.None;
            NetBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            NetBinding.MaxBufferPoolSize = 2147483647;
            NetBinding.MaxBufferSize = 2147483647;
            NetBinding.MaxReceivedMessageSize = 2147483647;
            NetBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            NetBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            NetBinding.ReaderQuotas.MaxDepth = 2147483647;
            NetBinding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            NetBinding.ReaderQuotas.MaxStringContentLength = 2147483647;

            Uri uri = new Uri("net.tcp://" + serverHost + ":" + port + "/");

            ServiceHost host = new ServiceHost(instanceType, uri);
            ServiceEndpoint myServiceEndpoint = host.AddServiceEndpoint(interfaceType, NetBinding, instanceType.Name);

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            int mexPort = port + 1;
            behavior.HttpGetEnabled = true;
            behavior.HttpGetUrl = new Uri("http://" + serverHost + ":" + mexPort + "/" + instanceType.Name + "/" + mexName);

            host.Description.Behaviors.Add(behavior);

            host.Open();

            return host;
        }

        /// <summary>
        /// 可以跨域的Http宿主
        /// </summary>
        /// <param name="instanceType">实例</param>
        /// <param name="interfaceType">接口</param>
        /// <param name="wcfUri">wcfUri</param>
        /// <returns>服务</returns>
        public static ServiceHost HttpHost(Type instanceType, Type interfaceType, string wcfUri)
        {
            ServiceHost host = null;
            try
            {
                #region HostAction
                //Host
                string actionUrl = wcfUri + "/service";
                Uri baseAddress = new Uri(actionUrl);
                host = new ServiceHost(instanceType, baseAddress);

                //NetBinding
                BasicHttpBinding NetBinding = new BasicHttpBinding();
                NetBinding.Security.Mode = BasicHttpSecurityMode.None;
                NetBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                NetBinding.MaxBufferPoolSize = 2147483647;
                NetBinding.MaxBufferSize = 2147483647;
                NetBinding.MaxReceivedMessageSize = 2147483647;
                NetBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
                NetBinding.ReaderQuotas.MaxArrayLength = 2147483647;

                //Endpoint
                ServiceEndpoint myServiceEndpoint = host.AddServiceEndpoint(interfaceType, NetBinding, instanceType.Name);

                //MetadataBehavior
                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                behavior.HttpGetEnabled = true;
                host.Description.Behaviors.Add(behavior);

                //Open
                host.Open();
                #endregion

                //跨越访问
                WCFHost.Accesspolicy(host, wcfUri);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "发布服务地址:" + wcfUri);
            }
            return host;
        }

        /// <summary>
        /// 跨越访问
        /// </summary>
        private static void Accesspolicy(ServiceHost host, string wcfUri)
        {
            host.Description.ConfigurationName = "clientaccesspolicy";
            Uri Uri = new Uri(wcfUri);
            ServiceHost clientaccesspolicyHost = new ServiceHost(typeof(DomainService), Uri);
            clientaccesspolicyHost.Open();
        }

    }
}