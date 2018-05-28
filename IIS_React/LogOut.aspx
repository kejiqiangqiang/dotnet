<%@ Page Language="C#" AutoEventWireup="true"%>
<%
    //配置文件的登录页
    string login=System.Configuration.ConfigurationManager.AppSettings["login"];
    var user = EFOS.Master.Business.WebKeyManager.LoginUser;
    if (user != null)
    {
        //找到集团的默认登录页
        var masterRedisConfig = new EFOS.Redis.MasterRedisConfig(EFOS.Master.Business.WebKeyManager.MasterRedisPath);
        var userBloc = masterRedisConfig.Apply_UserBloc.Where(p => p.UserID == user.UserID).Select(p => p.BlocCode).OrderBy (p=>p).FirstOrDefault();
        var shortName = masterRedisConfig.Apply_Bloc.Where(p => p.BlocCode == userBloc).Select(p => p.ShortName).FirstOrDefault();
        login = string.IsNullOrEmpty(shortName) ? login : (shortName + "login.html");
    }
    
    #region 多线程清空各分站
    
    if (user != null)
    {
        //跨域时清除当前登录所有分站session、cookie[sessionid](程序池多工作进程或者多服务器负载均衡，session采用Inproc模式时，无法清除干净session，此时需要清空客户端cookie[sessionid])、cookie[sessionid])
        user.RegisterSites.ForEach(p => {
            //多线程清空各分站
                //请求分站清空登录信息--可以在分站单独写一个处理程序或者方法
                var req = p + "/app.ashx?Name=EFOS.PCApply.Business.WebKeyManager.ClearSessionID";
                try
                {
                    EFOS.Tools.WebapiHttpRequest0.CreateHttpGetResponse(req, null); 
                }
                catch (Exception)
                {
                    
                }
            
            });
    }
    #endregion
    
    //清除主站登录信息
    EFOS.Master.Business.WebKeyManager.LoginUser = null;

    string backurl = Request.QueryString["backurl"];
    //重定向到登录页
    //login_A.js:1、回跳地址backurl为空，跳转到用户对应的分站默认页 2、回跳地址backurl不为空，跳转到backurl
    //对于分站跳转--ajax登录成功(ajax成功后，不必带上token到backurl(登陆成功后也可以加上，第二次就不会调用GetToken.aspx)，第二次访问可以调用GetToken.aspx，直接获取主站cookie中的token，并放入backurl，带回分站保存到分站cookie)
    string redirectUrl = string.IsNullOrEmpty(backurl) ? login : (login + "?backurl=" + HttpUtility.UrlEncode(backurl));
    Response.Redirect(redirectUrl);

%>
