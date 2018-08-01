using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebExecCmd
{
    public class ExecuteCmd
    {

        public static string[] RunCmd(string cmd,string args)
        {
            string[] result= new string[2];
            System.Diagnostics.Process proc = ExecuteCmd.CreateCmdProcess(cmd,args);
            //proc.Start();
            try
            {
                //命令执行初始目录为工作目录pStartInfo.WorkingDirectory，默认值为应用程序所在目录（当通过ProcessStartInfo.Arguments执行命令时，初始目录为cmd.exe所在目录(此时应用程序已经部署在iis)）
                var working = proc.StartInfo.WorkingDirectory;
                //2.输入执行命令
                ////proc.StandardInput.WriteLine(@"f:");
                ////proc.StandardInput.WriteLine(@"echo a>>a.txt");
                //proc.StandardInput.WriteLine(@"cd /d f:&echo a>>a.txt");//一次执行

                //proc.StandardInput.WriteLine(@"cd /d E:\\github.com\\dotnet\\WebExecCmd && git add ." + "&exit");
                
                //proc.StandardInput.AutoFlush = true;

                var handle = proc.Handle;
                var exit = proc.ExitCode;
                var error = proc.StandardError.ReadToEnd();
                var output = proc.StandardOutput.ReadToEnd();
                result[0] = output;
                result[1] = error;

                proc.WaitForExit(5000);
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
            return result;
        }

        /// <summary>
        /// 执行cmd命令
        /// 1.iis应用程序池用户权限设置
        /// 2.iis应用程序池设置允许读取用户配置
        /// 3.iis应用程序池设置允许可执行32位可执行程序
        /// </summary>
        /// 
        /// <param name="cmd">应用程序(环境变量path中定义或应用程序全路径)或批处理文件路径</param>
        /// <param name="args">字符串命令行(字符串命令行若包含空格，需要三重转义)或批处理文件路径</param>
        /// ProcessStartInfo.Arguments三重转义
        /// string command = @"""C:\Program Files\WinRAR\winrar.exe""  a -ep1 ""C:\Users\Administrator\AppData\Roaming\KPP\6CA24687FFDB9DBA\Output\epub\epub.zip""  ""C:\Users\Administrator\AppData\Roaming\KPP\6CA24687FFDB9DBA\Output\epub\mimetype""";
        /// startInfo.Arguments = @"/C (" + command + @")";
        
        public static System.Diagnostics.Process CreateCmdProcess(string cmd, string args)
        {
            ////cmd.exe
            //cmd="cmd";
            ////command
            //args = @"/c "+@"""f:&&echo hello>>a.txt&exit""";
            //    //+ "\r\n"
            //    //+ "call echo hi>>a.txt"
            //    //+ "\r\n";
            //    //+"pause";
            //bat
            cmd = HttpContext.Current.Server.MapPath("/") + "git_add.bat";

            ////svn.exe
            //cmd="svn";
            ////command
            //args = @"update \"E:\\svn\\MyProject1\"";
            //    //+ "\r\n"
            //    //+ "pause";

            //cmd = "git";
            //args = "cd E:\\github.com\\dotnet\\WebExecCmd && git add .";

            System.Diagnostics.ProcessStartInfo pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
            //pStartInfo.FileName=cmd;
            //1.启动参数执行命令
            pStartInfo.Arguments = args;//1.启动参数执行命令,如浏览器程序时设置为网址//2.或通过StandardInput.WriteLine("cmd")执行命令

            pStartInfo.CreateNoWindow=true;
            pStartInfo.UseShellExecute=false;//是否使用外壳运行//RedirectStandardXXX为true或WindowStyle为Hidden时UseShellExecute必须为false
            
            pStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pStartInfo.RedirectStandardError = true;
            pStartInfo.RedirectStandardInput = true;
            pStartInfo.RedirectStandardOutput = true;
            pStartInfo.StandardErrorEncoding = System.Text.Encoding.GetEncoding("GB2312");
            pStartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312");

            //pStartInfo.WorkingDirectory="f:";//cmd命令执行初始目录
            return System.Diagnostics.Process.Start(pStartInfo);
        }
    }
}