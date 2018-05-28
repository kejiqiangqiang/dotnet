<%@ Page Language="C#" AutoEventWireup="true"  %>
<%
    var loginUser =EFOS.Master.Business.WebKeyManager.LoginUser;
    if ( loginUser== null)
    {
        Response.Redirect("~/LogOut.aspx");
    }
    else
    {
        //同登录成功后根据用户类型跳转，不能直接进master(场景：分站登录成功后，关闭窗口(凭证仍未过期)，直接访问主站地址，导致本不该进入master的用户进入到master)
        
        //1、同Login方法
        //登录信息未过期即登录成功
        var masterRedisConfig = new EFOS.Redis.MasterRedisConfig(EFOS.Master.Business.WebKeyManager.MasterRedisPath);
        List<EFOS.Data.Master.Model.Apply_Platform> ps = masterRedisConfig.GetUserPlatform(loginUser.UserID);
        string defaultPlatformUrl = null;
        if (loginUser.UserType == 2 || loginUser.UserType == 3)
        {
            defaultPlatformUrl = ps != null && ps.Count > 0 ? ps[0].WebSiteUrl : null;
            defaultPlatformUrl = defaultPlatformUrl + "?PassportUri=" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/";
        }
        //2、同login_A.js（不完全相同：此处不会有backurl的情况即backurl肯定为空）
        //登录成功后跳转
        string backurl = HttpContext.Current.Request.QueryString["backurl"];
        //if (!string.IsNullOrEmpty(backurl))
        //{
        //    backurl = backurl;
        //}
        if (string.IsNullOrEmpty(backurl) && loginUser.UserType <= 1)
        {
            backurl = "master.html";
        }
        if (string.IsNullOrEmpty(backurl) && (loginUser.UserType == 2 || loginUser.UserType == 3))
        {
            backurl = defaultPlatformUrl;
        }
        
        //不能对整个路径编码(不能将url中的?、&等分割符编码，否则会导致认为是aspx请求安全验证不通过，默认配置<pages validateRequest="true" />)
        //backurl = HttpUtility.UrlEncode(backurl);//http...?PassportUri=http://...
        Response.Redirect(backurl);
    }
%>