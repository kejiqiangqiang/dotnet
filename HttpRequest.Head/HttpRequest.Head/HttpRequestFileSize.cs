using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequest.Head
{
    public class HttpRequestFileSize
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
        /// http request head 方法，获取文件大小（不下载文件的条件下）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFileSize(string url, Dictionary<string, string> parameters)
        {
            //创建http请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //设置请求属性及头部参数
            request.Method = "HEAD";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json";
            //请求数据
            //StringBuilder buffer = new StringBuilder();
            #region application/x-www-form-urlencoded
            
            //int i = 0;
            //foreach (string key in parameters.Keys)
            //{
            //    if (i > 0)
            //    {
            //        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
            //    }
            //    else
            //    {
            //        buffer.AppendFormat("{0}={1}", key, parameters[key]);
            //    }
            //    i++;
            //}
            #endregion
            #region request.ContentType = "application/json
            //string postJson = JsonConvert.SerializeObject(new {});
            #endregion
            //buffer.Append(postJson);
            //请求数据字节
            //byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
            //请求流
            //Stream requestStream = request.GetRequestStream();
            //字节写入流
            //requestStream.Write(data, 0, data.Length);
            //requestStream.Close();

            //请求的响应对象
            HttpWebResponse response = null; ;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            string responseReturnString = Math.Round(response.ContentLength / 1024.0,1,MidpointRounding.AwayFromZero).ToString();//单位k

            //响应流
            //Stream responseStream = response.GetResponseStream();
            //string responseReturnString = responseStream.Length.ToString();//responseStream.Length此流不支持查找操作
            ////读取服务器端返回的消息 
            //StreamReader reader = new StreamReader(responseStream);
            //string responseReturnString = reader.ReadToEnd();
            //reader.Close();
            //responseStream.Close();
            //响应状态
            try
            {
                //返回状态码非200
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    //返回异常信息
                    try
                    {
                        Error errorResultData = JsonConvert.DeserializeObject<Error>(responseReturnString);
                        throw new Exception(errorResultData.message);
                    }
                    catch (Exception)
                    {
                        throw new Exception(responseReturnString); ;
                    }
                }
                //返回状态码200，正常返回
                return responseReturnString;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                    response = null;
                }
            }
        }
    }
}
