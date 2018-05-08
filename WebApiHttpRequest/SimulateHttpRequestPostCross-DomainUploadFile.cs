using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApiHttpRequest
{
    class SimulateHttpRequestPostCross_DomainUploadFile
    {

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="buffer">字节数组</param>//由于客户端调用参数没有做字节参数的处理，可经Base64转换为字符串
        /// <param name="fileDir">文件相对文件夹路径(/pdf/94/)</param>
        /// <param name="fileName">文件名(2017.pdf)</param>
        public bool UploadPdfFromByteBuffer(byte[] buffer, string fileDir, string fileName)//方法名不可相同，反射没有作重载
        {
            //string basePath = HttpContext.Current.Server.MapPath("/upload");
            string basePath = "F:\\upload";
            string fullDir = basePath + fileDir;
            string fullPath = fullDir + fileName;
            if (!Directory.Exists(fullDir))
            {
                Directory.CreateDirectory(fullDir);
            }
            //文件流仅创建文件，不会创建文件夹
            FileStream stream = new FileStream(fullPath, FileMode.Create);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            return true;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stringBuffer">字节数组经Base64转换后的字符串</param>
        /// <param name="fileDir">文件相对文件夹路径(/pdf/94/)</param>
        /// <param name="fileName">文件名(2017.pdf)</param>
        public bool UploadPdfFromStringBuffer(string stringBuffer, string fileDir, string fileName)//方法名不可相同，反射没有作重载
        {
            //string basePath = HttpContext.Current.Server.MapPath("/upload");
            string basePath = "F:\\upload";
            string fullDir = basePath + fileDir;
            string fullPath = fullDir + fileName;
            if (!Directory.Exists(fullDir))
            {
                Directory.CreateDirectory(fullDir);
            }
            //文件流仅创建文件，不会创建文件夹
            FileStream stream = new FileStream(fullPath, FileMode.Create);
            //经过Convert.ToBase64String()转换过的参数返回的字符串中，经过request post后发现，+都变成了空格
            stringBuffer = stringBuffer.Replace("%2B", "+");//先将+编码为%2B//在服务端使用字符串前将%2B替换为+替换回来即可
            byte[] buffer = Convert.FromBase64String(stringBuffer);
            ////Load方法Request内Post参数参数的处理编码url编码转换ContentType("application/x-www-form-urlencoded")
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();

            //文件成功上传，但是文件无法打开，缺少EOF？
            return true;
        }

        /// <summary>
        /// 测试模拟http post时ContentType("application/x-www-form-urlencoded")时，+会被编码为空格，因此为避免丢失，将+替换为%2B
        /// 
        /// URLencodeing是为访问地址编码，通常就是对为了对URL后边的参数加密获取传递中文的时候用
        /// 一般编码成UTF-8要不后台接到的时候通常是乱码的。
        /// httpencoding 看来是对一次HTTP请求编码，也就是设置返回页面的编码格式。
        /// 
        /// 一般URL编码前台用js自带函数encodeURI，后台用java.net.URLEncoder
        ///HTTP编码的话我只用过javax.servlet.ServletRequest的setCharacterEncoding方法
        /// </summary>
        /// <param name="stringWithPlus"></param>
        /// <returns></returns>
        public string PlusUrlEncoding(string stringWithPlus)
        {
            return stringWithPlus;
        }


        /// <summary>
        /// 服务端接收文件
        /// 模拟http post请求--ContentType:"multipart/form-data";--表单中有文件提交
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        public bool UploadFileFormData(string projectCode,HttpPostedFile inputFile)
        {
            if (inputFile == null) { return false; }
            string dir = "/upload/UploadFileFormData/" + projectCode.ToString()+"/";
            string fileDir = HttpContext.Current.Server.MapPath(dir);
            string filePath = fileDir+inputFile.FileName;
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            inputFile.SaveAs(filePath);
            return true;
        }

    }
}
