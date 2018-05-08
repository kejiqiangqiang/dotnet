using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            #region process run cmd

            Process proc = new Process();
            //proc.StartInfo.WorkingDirectory = targetDir;//UseShellExecute必须设为true时，必须指定
            proc.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";//若在注册表或环境变量中有.exe的路径，则可以直接调用exe（也可以绝对路径），一般为系统和安装的程序，若为用户自定义exe，用绝对路径
            //proc.StartInfo.Arguments = @"";//this is argument
            proc.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
            proc.StartInfo.CreateNoWindow = false;//不显示程序窗口
            //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//设置DOS窗口不显示//WindowStyle为Hidden时，UseShellExecute必须设为false
            proc.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
            proc.StartInfo.RedirectStandardOutput = false;  //由调用程序获取输出信息
            proc.StartInfo.RedirectStandardError = false;   //重定向标准错误输出

            //向cmd窗口写入命令
            string cmd0 = @"md F:\\bat0 \r\n md F:\\bat1";
            string cmd = @"E:\SVN\NewEFOS-PCApply\trunk\EFOS.PCApply\EFOS.PCApply.UI\upload\HighChartsPdf\94\20170527104920.bat";
            try
            {
                proc.Start();
                proc.StandardInput.WriteLine(cmd0);
                proc.StandardInput.AutoFlush = true;
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                throw ex;//Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            #endregion
        }
    }
}
