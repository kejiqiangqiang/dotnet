using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
//添加服务引用(必须先让服务运行起来(无论寄宿在哪))
//方法一：直接右键添加服务引用
//方法二：通过SvcUtil.exe工具生成客户端代码，拷贝到项目Service References下
namespace Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Service1)))
            {
                //可以直接在宿主宿主宿主程序的配置文件app.config中配置(并且服务应用程序或者服务类库中不能重复配置，否则地址已在使用异常)
                //host.AddServiceEndpoint(
                //    typeof(IService1),
                //    new WSHttpBinding(),
                //    "http://127.0.0.1:999/xxxService/Calculator");
                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri("http://127.0.0.1:999/xxxService/metadata");
                    host.Description.Behaviors.Add(behavior);
                }
                //启动停止事件
                host.Opened += (sender, e) => { Console.WriteLine("服务已启动\n按任意键终止..."); };
                host.Open();
                host.Closed += (sender, e) => { Console.WriteLine("\n服务已停止\n按任意键退出..."); };
                //按任意键停止
                Console.ReadKey();
                host.Close();
                Console.Read();

            }
        }
    }
}
