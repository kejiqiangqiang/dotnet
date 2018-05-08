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
    /// webapi请求
    /// </summary>
    public class WebApiHttpRequestHttpClient
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
                if (!url.ToLower().StartsWith("https://"))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                }

                var formByteArray = WebApiHttpRequestHttpClient.GetFormDataByteArrayContent(parameters);
                var fileByteArray = WebApiHttpRequestHttpClient.GetFileByteArrayContent(files);
                MultipartFormDataContent multipartFormDataContent = null;

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
                    //var values = new List<KeyValuePair<string, string>>();
                    var d = "";
                    foreach (var key in parameters)
                    {
                        d = d+ key.Key + "=" + key.Value + "&";
                    }
                    d = d + "ii=0";
                    var contenStr = new StringContent(d, Encoding.UTF8);
                    contenStr.Headers.Remove("Content-Type");//必须
                    contenStr.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//1.根据需求设置

                    using (contenStr)
                    {
                        try
                        {
                            var result = client.PostAsync(url, contenStr).Result;
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
            string resultJson = WebApiHttpRequestHttpClient.CreateHttpPostResponse(url, formData, files);

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
                    if (obj == null)
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