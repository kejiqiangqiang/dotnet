using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.ServiceReference1;
//添加服务引用(必须先让服务运行起来(无论寄宿在哪))
//方法一：直接右键添加服务引用
//方法二：通过SvcUtil.exe工具生成客户端代码，拷贝到项目Service References下
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Service1Client client = new Service1Client())
            {
                Console.WriteLine("调用WCF服务方法:string GetData(int value)\n" + client.GetData(10));
                Console.Read();
            }
        }
    }
}
