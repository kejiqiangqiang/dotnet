using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace WebApiHttpRequest
{
    /// <summary>
    /// 解析数据
    /// </summary>
    class JsonData
    {
        public bool success { get; set; }
        public object data { get; set; }
    }

    /// <summary>
    /// 错误信息
    /// </summary>
    internal class Error
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
    /// webapi请求
    /// </summary>
    public class WebApiHttpRequestHttpClient0
    {
        /// <summary>
        /// 获取文件集合对应的ByteArrayContent集合
        /// </summary>
        /// <param name="files">要上传的文件</param>
        /// <returns>提供基于字节数组的 HTTP 内容</returns>
        private static List<ByteArrayContent> GetFileByteArrayContent(Dictionary<string, string> files = null)
        {
            if (files == null)
            {
                return null;
            }
            List<ByteArrayContent> list = new List<ByteArrayContent>();
            foreach (var key in files.Keys)
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(files[key]));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Path.GetFileName(key)
                };
                list.Add(fileContent);
            }
            return list;
        }

        /// <summary>
        /// 获取键值集合对应的ByteArrayContent集合
        /// </summary>
        /// <param name="parameters">form参数</param>
        /// <returns>提供基于字节数组的 HTTP 内容</returns>
        private static List<ByteArrayContent> GetFormDataByteArrayContent(Dictionary<string, string> parameters)
        {
            List<ByteArrayContent> list = new List<ByteArrayContent>();
            foreach (var key in parameters.Keys)
            {
                var dataContent = new ByteArrayContent(Encoding.UTF8.GetBytes(parameters[key]));
                dataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = key
                };
                list.Add(dataContent);
            }
            return list;
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
        /// http post请求
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="parameters">form参数</param>
        /// <param name="files">form文件</param>
        /// <returns></returns>
        private static string CreateHttpPostResponse(string url, Dictionary<string, string> parameters, Dictionary<string, string> files)
        {
            string resultText = null;
            using (HttpClient client = new HttpClient())
            {
                //设定要响应的数据格式
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                var formByteArray = WebApiHttpRequestHttpClient0.GetFormDataByteArrayContent(parameters);
                var fileByteArray = WebApiHttpRequestHttpClient0.GetFileByteArrayContent(files);

                MultipartFormDataContent multipartFormDataContent = null;
                FormUrlEncodedContent frormUrlEncodedContent = null;

                if (fileByteArray != null)
                {
                    //有文件的上传方式
                    using (multipartFormDataContent = new MultipartFormDataContent())
                    {
                        Action<List<ByteArrayContent>> act = (dataContents) =>
                        {
                            foreach (var byteArrayContent in dataContents)
                            {
                                multipartFormDataContent.Add(byteArrayContent);
                            }
                        };
                        if (formByteArray != null) { act(formByteArray); }
                        if (fileByteArray != null) { act(fileByteArray); }
                        try
                        {
                            var result = client.PostAsync(url, multipartFormDataContent).Result;
                            resultText = result.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {
                            resultText = ex.Message;
                        }
                    }
                }
                else
                {
                    //无文件的提交方式
                    var values = new List<KeyValuePair<string, string>>();
                    foreach (var key in parameters.Keys)
                    {
                        values.Add(new KeyValuePair<string, string>(key, parameters[key]));
                    }
                    using (frormUrlEncodedContent = new FormUrlEncodedContent(values))
                    {
                        try
                        {
                            var result = client.PostAsync(url, frormUrlEncodedContent).Result;
                            resultText = result.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {
                            resultText = ex.Message;
                        }
                    }
                }
            }
            return resultText;
        }

        /// <summary>
        /// 加载远程数据或控制远程方法
        /// </summary>
        ///  <param name="url">请求路径</param>
        /// <param name="parameters">form参数</param>
        /// <param name="files">form文件</param>  
        public static T Load<T>(string url, Dictionary<string, object> parameters, Dictionary<string, string> files = null)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
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
            string resultJson = WebApiHttpRequestHttpClient0.CreateHttpPostResponse(url, formData, files);

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
                JsonData jsonData = JsonConvert.DeserializeObject<JsonData>(resultJson);
                if (jsonData.data == null)
                {
                    //return default(T);
                    object obj = JsonConvert.DeserializeObject<object>(resultJson);
                    if (obj==null)
                    {
                        return default(T);
                    }
                    return (T)obj;
                }
                var data = jsonData.data.ToString();
                T resultData = JsonConvert.DeserializeObject<T>(data);
                return resultData;
            }
        }
    }
}