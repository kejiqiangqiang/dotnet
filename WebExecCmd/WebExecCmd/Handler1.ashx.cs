using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebExecCmd
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var result = ExecuteCmd.RunCmd("","");
            context.Response.ContentType = "text/plain";
            context.Response.Write(result[0] + "\r\n" + result[1]);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}