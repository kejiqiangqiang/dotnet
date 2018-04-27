using HttpRequest.Head;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HttpRequest.Test
{
    class Program
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool system(string str);
        static void Main(string[] args)
        {
            var bll = new HttpRequestFileSize();
            string url = "http://localhost:8765/upload/DeviceArchivesFile/94/使用说明.txt";
            var fileSize = bll.GetFileSize(url,null);
            Console.WriteLine("fileSize={0}", fileSize);
            //Console.ReadKey();
            system("pause");
        }
    }
}
