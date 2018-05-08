using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using EFOS.Service.Apply;
using EFOS.PCApply.Business;
using EFOS.Data.Master.Model;
using EFOS.Service.Master;

namespace UnitTest
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string platformCode = ConfigurationManager.AppSettings["PlatformCode"];
            string PassportUri = ConfigurationManager.AppSettings["PassportUri"];

            string backUrl = Request.QueryString["backUrl"];
            if (WebKeyManager.LoginUser != null)
            {
                bool isSessionEnd = Authorization.CheackSessionEnd(PassportUri, WebKeyManager.LoginUser.TokenValue);
                if (isSessionEnd)
                {
                    Session.Abandon();
                    WebKeyManager.LoginUser = null;
                    WebKeyManager.Platform = null;
                }
                else
                {
                    if (backUrl != null)
                    {
                        Response.Redirect(backUrl);
                    }
                    else
                    {
                        Response.Redirect(WebKeyManager.Industry.DefaultPage + "default.html");
                    }
                }
            }

            if (WebKeyManager.LoginUser == null)
            {
                WebKeyManager.TimeOut = 3000;
                WebKeyManager.LoginUser = Authorization.Verification(PassportUri);
                List<Apply_Platform> platforms = new ConfigService().GetPlatform(WebKeyManager.LoginUser.UserID);
                if (platforms == null || platforms.Count == 0)
                {
                    throw new Exception("该用户没有任何平台的操作权限!");
                }
                Apply_Platform platform = platforms.Where(p => p.PlatformCode == platformCode).FirstOrDefault();
                if (platform == null)
                {
                    //如果没有该平台的权限，则默认一个有权限的操作平台
                    Response.Redirect(platforms[0].WebSiteUrl);
                }
                WebKeyManager.Platform = platform;
                WebKeyManager.Industry = new ConfigService().GetIndustry(WebKeyManager.Platform.IndustryCode);
                if (backUrl != null)
                {
                    Response.Redirect(backUrl);
                }
                else
                {
                    //项目用户
                    if (WebKeyManager.LoginUser.UserType == 3)
                    {
                        List<Apply_Project> projects = new EFOS.Service.Apply.AuthorizeService(WebKeyManager.Platform.DataServerWcfUrl).GetUserProject(null, null, WebKeyManager.LoginUser.UserID);

                        if (projects.Count > 0)
                        {
                            string linkPage = WebKeyManager.Industry.DefaultPage + "project.html" + "?projectCode=" + projects[0].ProjectCode + "&projectName=" + System.Web.HttpUtility.UrlEncode(projects[0].ProjectName);
                            Response.Redirect(linkPage);
                        }
                        else
                        {
                            throw new Exception("该用户没有任何项目的操作权限!");
                        }
                    }
                    else
                    {
                        //其他用户
                        Response.Redirect(WebKeyManager.Industry.DefaultPage + "default.html");
                    }
                }
            }
        }
    }
}



