using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHttpRequest
{
    class Program
    {
        static void Main(string[] args)
        {

            #region 模拟http post时ContentType("application/x-www-form-urlencoded")时，+会被编码为空格，因此为避免丢失，将+替换为%2B
            string url = "http://localhost:8300" + "/app.ashx?Name=EFOS.Master.Business.SimulateHttpRequestPostCross_DomainUploadFile.PlusUrlEncoding";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("stringWithPlus", "1+1测试");
            var resut = WebApiHttpRequestStreamMultipart.Load<string>(url, parameters);
            #endregion

            #region 模拟http post请求--ContentType:"multipart/form-data";--表单中有文件提交
            string url1 = "http://localhost:8300" + "/app.ashx?Name=EFOS.Master.Business.SimulateHttpRequestPostCross_DomainUploadFile.UploadFileFormData";
            Dictionary<string, object> parameters1 = new Dictionary<string, object>();
            List<FileFormData> fileFormDatas = new List<FileFormData>();
            parameters1.Add("projectCode", 7);
            fileFormDatas.Add(new FileFormData()
            {
                Name = "inputFile",
                FileName = "test.txt",
                HttpInputStream = new FileStream("F:/upload/test.txt", FileMode.Open)
                //FileName = "test1.txt",
                //HttpInputStream = new FileStream("F:/upload/test1.txt", FileMode.Open)
                //FileName = "test.pdf",
                //HttpInputStream = new FileStream("F:/upload/test.pdf", FileMode.Open)

                //FileName = "test.rar",
                //HttpInputStream = new FileStream("F:/upload/test.rar", FileMode.Open)
                //FileName = "text.zip",
                //HttpInputStream = new FileStream("F:/upload/text.zip", FileMode.Open)

            });
            var resut1 = WebApiHttpRequestStreamMultipart.LoadMultipartFormData<bool>(url1, parameters1, fileFormDatas);
            #endregion

        }
    }
}
