using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebApiHttpRequest
{
    /// <summary>
    /// 模拟前端表单文件部分数据
    /// </summary>
    public class FileFormData
    {
        /// <summary>
        /// 服务端接收文件参数名（前端表单文件控件的name="xxx"，必须与服务端方法内接收文件的参数的参数名一致，该参数才能接收到该文件）如：void upload(int userID,HttpPostedFile xxx)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件名（xxxxxx.txt）
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件流数据
        /// </summary>
        public Stream HttpInputStream { get; set; }
        
    }

    ///// <summary>
    ///// 解析数据
    ///// </summary>
    //class JsonData
    //{
    //    public bool success { get; set; }
    //    public object data { get; set; }
    //}

    /// <summary>
    /// 错误信息
    /// </summary>
    internal class Errors
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public string success { get; set; }
    }

    /// <summary>
    /// webapi请求--模拟http post
    /// </summary>
    public class WebApiHttpRequestStreamMultipart
    {

        #region 模拟http post--ContentType:"application/x-www-form-urlencoded;";--不支持文件上传
        /// <summary>
        /// 模拟http post请求--ContentType:"application/x-www-form-urlencoded";
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string CreateHttpPostResponse(string url, Dictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";

            request.Headers.Add("X_REG_CODE", "288a633ccc1");
            request.Headers.Add("X_MACHINE_ID", "a306b7c51254cfc5e22c7ac0702cdf87");
            request.Headers.Add("X_REG_SECRET", "de308301cf381bd4a37a184854035475d4c64946");
            request.Headers.Add("X_STORE", "0001");
            request.Headers.Add("X_BAY", "0001-01");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language", "zh-CN");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Accept = "*/*";

            request.CookieContainer = new CookieContainer();

            //如果需要POST数据   
            StringBuilder buffer = new StringBuilder();
            if (!(parameters == null || parameters.Count == 0))
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }

                //Encoding encoding = Encoding.GetEncoding("gb2312");
                Encoding encoding = Encoding.GetEncoding("utf-8");
                byte[] data = encoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            //读取服务器端返回的消息 
            string sReturnString = sr.ReadToEnd();

            try
            {
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    try
                    {
                        Errors resultData = JsonConvert.DeserializeObject<Errors>(sReturnString);
                        throw new Exception(resultData.message);
                    }
                    catch
                    {
                        throw new Exception(sReturnString);
                    }
                }
                return sReturnString;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                    res.Dispose();
                    res = null;
                }
                if (s != null)
                {
                    s.Close();
                    s.Dispose();
                    s = null;

                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
            }
        }


        /// <summary>
        /// 模拟http get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string CreateHttpGetResponse(string url, Dictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;

            StringBuilder buffer = new StringBuilder();
            if (!(parameters == null || parameters.Count == 0))
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("?{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url + buffer.ToString()) as HttpWebRequest;
            }
            request.Method = "GET";

            request.Headers.Add("X_REG_CODE", "288a633ccc1");
            request.Headers.Add("X_MACHINE_ID", "a306b7c51254cfc5e22c7ac0702cdf87");
            request.Headers.Add("X_REG_SECRET", "de308301cf381bd4a37a184854035475d4c64946");
            request.Headers.Add("X_STORE", "0001");
            request.Headers.Add("X_BAY", "0001-01");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language", "zh-CN");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Accept = "*/*";

            request.CookieContainer = new CookieContainer();


            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            //读取服务器端返回的消息 
            string sReturnString = sr.ReadLine();
            return sReturnString;
        }

        /// <summary>
        /// 加载远程数据或控制远程方法
        /// </summary>
        public static T Load<T>(string url, Dictionary<string, object> parameters)// where T : new()
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            Dictionary<string, string> formData = new Dictionary<string, string>();
            foreach (var item in parameters)
            {
                if (!(item.Value is string))
                {
                    formData.Add(item.Key, JsonConvert.SerializeObject(item.Value, timeFormat));
                }
                else
                {
                    formData.Add(item.Key, item.Value.ToString());
                }
            }
            string resultJson = WebApiHttpRequestStreamMultipart.CreateHttpPostResponse(url, formData);

            var type = typeof(T);
            if (type.IsValueType || type.IsEnum)
            {
                //值类型
                T resultData = JsonConvert.DeserializeObject<T>(resultJson);
                return resultData;
            }
            else if (type == typeof(string))
            {
                object json = resultJson;
                return (T)json;
            }
            else
            {
                //JsonData jsonData = JsonConvert.DeserializeObject<JsonData>(resultJson);
                //var data = jsonData.data.ToString();
                //T resultData = JsonConvert.DeserializeObject<T>(data);
                T resultData = JsonConvert.DeserializeObject<T>(resultJson);
                return resultData;
            }
        }

        #endregion

        #region 模拟http post--ContentType:"multipart/form-data;";--支持文件上传

        /// <summary>
        /// 模拟http post请求--ContentType:"multipart/form-data";
        /// 文件流上传--表单中有文件提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters">模拟表单项请求参数普通数据</param>
        /// <param name="formFileData">模拟表单项请求参数文件数据</param>
        /// <returns></returns>
        public static string CreateHttpPostedFileResponse(string url, Dictionary<string, string> parameters, List<FileFormData> fileFormDatas)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.Timeout = 3600000;//3600s
            request.KeepAlive = true;

            request.Headers.Add("X_REG_CODE", "288a633ccc1");
            request.Headers.Add("X_MACHINE_ID", "a306b7c51254cfc5e22c7ac0702cdf87");
            request.Headers.Add("X_REG_SECRET", "de308301cf381bd4a37a184854035475d4c64946");
            request.Headers.Add("X_STORE", "0001");
            request.Headers.Add("X_BAY", "0001-01");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            request.Headers.Add("Accept-Language", "zh-CN");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");//返回值为文件流时使用（如导出Excel的ToXls.aspx），否则导致一般的返回值乱码，继而导致反序列化失败
            request.Accept = "*/*";
            //request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.CookieContainer = new CookieContainer();
            // 边界符
            var boundary = "----" + DateTime.Now.Ticks.ToString("x");
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);

            //文件数据模板  
            string fileFormdataTemplate =
                "--" + boundary + "\r\n" +
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                "Content-Type: application/octet-stream\r\n" +//application/octet-stream八进制流文件,支持任何文件上传、下载
                "\r\n";//空行
            //+"\r\n";//1、最后一个看起来是空行，其实是文件流的位置--(用抓包工具监听)查看服务端接收到的报文PostData数据格式多一个空行
            //2、文件内容结束之后仍需要一个换行（(用抓包工具监听)查看服务端接收到的报文PostData数据格式，发现zip文件参数数据流内容与下一个参数开始的分隔符以及整个请求体结束的分隔符直接缺少换行)--导致txt文件末尾的换行缺失和上传zip失败原因
            //3、把换行符加入到 请求体中每个请求参数的文本数据内容或文件流内容结束后与其紧接着后面的另一个请求体参数的开始分隔符以及整个请求体结束分隔符中，以便统一处理(原来是普通文本数据参数直接在文本数据模板dataFormdataTemplate中加入了，但是文件数据参数没有加，导致缺失文件末尾换行符，zip无法正确解析)--为第一个参数时又不需要这个换行，加判断或采用4
            //4、写完文件流数据后加一个换行符\r\n
            string newLine = "\r\n";
            Byte[] newLineBytes = Encoding.UTF8.GetBytes(newLine);

            //文本数据模板  
            string dataFormdataTemplate =
                "--" + boundary + "\r\n" +
                "Content-Disposition: form-data; name=\"{0}\"\r\n" +
                "\r\n" +//空行
                "{1}\r\n";
            //结尾  
            var footer = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");
            //大文件可提高性能//也可以不使用MemoryStream，直接将分段的文件流写入请求流中
            MemoryStream memoryStream = new MemoryStream();
            Stream requestStream = request.GetRequestStream();

            //key-value表单参数
            foreach (var item in parameters)
            {
                string formdata = string.Format(dataFormdataTemplate, item.Key, item.Value);
                byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(formdata);
                memoryStream.Write(buffer, 0, buffer.Length);//普通表单数据不使用MemoryStream
                requestStream.Write(buffer, 0, buffer.Length);//直接写到请求流中
            }

            //普通表单数据一次性写入请求流
            #region 由于MemoryStream暂时不知道如何清空，因此此处不能使用同一个MemoryStream，否则导致重复，因此再创建一个新的MemoryStream或者对普通表单数据不使用MemoryStream,直接写到请求流中
            #region 或者先全部写入MemoryStream，再一次性从MemoryStream写入请求流
            #endregion
            ////memoryStream.Position = 0;
            //memoryStream.WriteTo(requestStream);//普通表单数据不使用MemoryStream

            ////memoryStream.Flush();//对于MemoryStream并不是清空内存，而是什么都不做
            #endregion

            //表单文件参数
            //大文件分段上传，每次上传不超过4k//也可以不使用MemoryStream，直接将分段的文件流写入请求流中
            int bufferLength = 4096;
            foreach (var item in fileFormDatas)
            {
                MemoryStream memoryStream1 = new MemoryStream();//

                string formdata = string.Format(fileFormdataTemplate, item.Name, item.FileName);
                byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(formdata);

                //memoryStream.Write(buffer,0,buffer.Length);
                memoryStream1.Write(buffer, 0, buffer.Length);

                using (Stream fileFormDataStream = item.HttpInputStream)
                {
                    //已上传的字节数
                    int writed = 0;
                    //每次实际读取的字节数
                    var readBytes = 0;
                    //剩余字节数
                    var residueBytes = item.HttpInputStream.Length;
                    //缓冲区
                    byte[] streamBuffer = new byte[residueBytes > bufferLength ? bufferLength : residueBytes];
                    //BinaryReader binaryReader = new BinaryReader(fileFormDataStream);
                    while ((readBytes = fileFormDataStream.Read(streamBuffer, 0, residueBytes > bufferLength ? bufferLength : (int)residueBytes)) > 0)//每次读取不超过4k//不加剩余字节与一次读取最大字节判断也行，读取字节不足时，不影响读取，返回字节数为实际读取到的字节数
                    {
                        //memoryStream.Write(streamBuffer, 0, readBytes);
                        //memoryStream.WriteTo(requestStream);
                        memoryStream1.Write(streamBuffer, 0, readBytes);
                        //memoryStream.Flush();//对于MemoryStream并不是清空内存，而是什么都不做
                        writed += readBytes;
                        residueBytes -= readBytes;
                    }
                }
                memoryStream1.WriteTo(requestStream);
                memoryStream1.Close();

                //4、写完文件流数据后加一个换行符\r\n
                requestStream.Write(newLineBytes, 0, newLineBytes.Length);

                ////返回读取流字节数，返回0则表示流结尾
                //int size = memoryStream.Read(buffer, 0, buffer.Length);
                ////int size = binaryReader.Read(buffer, 0, bufferLength);
                //using (Stream stream2 = request.GetRequestStream())
                //{
                //    while (size > 0)
                //    {
                //        stream2.Write(buffer, 0, buffer.Length);
                //        offset += size;
                //        size = stream.Read(buffer, 0, bufferLength);
                //    }
                //}
            }
            //结尾
            requestStream.Write(footer, 0, footer.Length);
            requestStream.Close();
            //requestStream.Dispose();
            //memoryStream.Close();
            //memoryStream1.Close();
            //memoryStream.Dispose();

            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            //读取服务器端返回的消息 
            string sReturnString = sr.ReadToEnd();

            try
            {
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    try
                    {
                        Errors resultData = JsonConvert.DeserializeObject<Errors>(sReturnString);
                        throw new Exception(resultData.message);
                    }
                    catch
                    {
                        throw new Exception(sReturnString);
                    }
                }
                return sReturnString;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                    res.Dispose();
                    res = null;
                }
                if (s != null)
                {
                    s.Close();
                    s.Dispose();
                    s = null;

                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
            }
        }

        /// <summary>
        /// 加载远程数据或控制远程方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters">模拟表单项请求参数普通数据</param>
        /// <param name="formFileData">模拟表单项请求参数文件数据</param>
        /// <returns></returns>
        public static T LoadMultipartFormData<T>(string url, Dictionary<string, object> parameters, List<FileFormData> fileFormDatas)// where T : new()
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            Dictionary<string, string> formData = new Dictionary<string, string>();
            //普通表单项请求参数
            foreach (var item in parameters)
            {
                if (!(item.Value is string))
                {
                    //序列化--非字符串的需要进行序列化
                    formData.Add(item.Key, JsonConvert.SerializeObject(item.Value, timeFormat));
                }
                else
                {
                    formData.Add(item.Key, item.Value.ToString());
                }
            }
            ////表单项文件请求参数
            //foreach (var item in formFileData)
            //{

            //}
            //请求并返回字符串
            string resultJson = WebApiHttpRequestStreamMultipart.CreateHttpPostedFileResponse(url, formData, fileFormDatas);
            //处理返回值--反序列化
            var type = typeof(T);
            if (type.IsValueType || type.IsEnum)
            {
                //值类型
                T resultData = JsonConvert.DeserializeObject<T>(resultJson);
                return resultData;
            }
            else if (type == typeof(string))
            {
                object json = resultJson;
                return (T)json;
            }
            else
            {
                //JsonData jsonData = JsonConvert.DeserializeObject<JsonData>(resultJson);
                //var data = jsonData.data.ToString();
                //T resultData = JsonConvert.DeserializeObject<T>(data);
                T resultData = JsonConvert.DeserializeObject<T>(resultJson);
                return resultData;
            }
        }
        #endregion

    }
}