using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.UI;
using System.Web.Hosting;
using System.IO;
using System.Threading;
using System.Web.SessionState;

namespace UnitTest
{
    [TestClass]
    public class EnergyCurveTest//2、在default.aspx继承Page
    {

        [TestMethod]
        public void GetTotalEnergyTrendTest()
        {

            #region Session测试失败
            //////设置数据库连接字符串//不能直接赋值，否则WebKeyManager.set_Platform(Apply_Platform value)报错
            ////WebKeyManager.Platform = new Apply_Platform() { PlatformCode = "001",
            ////                                                SQLConnectionString = "Data Source=192.168.3.11\\dev;timeout=210;Initial Catalog=EFOS.Community;User ID=sa;Password=111111"
            ////                                                };//PlatformCode = "001"//可以不要


            ////设置数据库连接字符串//通过Session间接赋值
            ////HttpContext==null，不能直接用，必须在页面请求中，web容器自动实例化HttpContext
            
            /////实例化HttpContext
            
            ////捕获响应中的输出
            //TextWriter tw = new StringWriter();
            //System.Threading.Thread.GetDomain().SetData(".appPath", "E:\\SVN\\NewEFOS-PCApply\\trunk\\EFOS.PCApply\\UnitTest\\");
            //System.Threading.Thread.GetDomain().SetData(".appVPath", "/");
            ////实例化wr
            //HttpWorkerRequest wr = new SimpleWorkerRequest("default.aspx", "friendId=1300000000", tw);
            ////实例化HttpContext
            //HttpContext.Current = new HttpContext(wr);

            //bool expected = true;
            //bool actual = HttpContext.Current != null ? true : false;
            //Assert.AreEqual(expected, actual, "HttpContext实例化失败");

            ////向Session中添加键值对
            ////HttpContext.Current.Session==nullSession实例化失败----Session必须依赖于Web页面请求而存在,以上通过HttpWorkerRequest"成功"实例化了上下文HttpContext,但是Session为null--
            ////----去改测试的WebKeyManager，不从Session中获取SQLConnectionString,直接赋值WebKeyManager的Platform字段,从而初始化BaseBusiness的连接
            ////----或者改BaseBusiness,不通过WebKeyManager,绕过WebKeyManager,直接在BaseBusiness.ReaderConnectionString()返回SQLConnectionString
            ////bool actual1 = HttpContext.Current.Session != null ? true : false;            
            ////Assert.AreEqual(expected, actual1, "Session实例化失败");
            
            //Apply_Platform platform = new Apply_Platform(){
            //                                                PlatformCode = "001",
            //                                                SQLConnectionString = "Data Source=192.168.3.11\\dev;timeout=210;Initial Catalog=EFOS.Community;User ID=sa;Password=111111"
            //                                                };//PlatformCode = "001"//可以不要;
            ////HttpContext.Current.Session.Add("Platform",platform);
            #endregion


            #region 绕过Session,在BLL里面重写BaseBusiness.ReaderConnectionString()(开始的思路是修改BaseBusiness,一想要改基类不大好,于是乎就想到了重写，nice)
            //测试时,只需在BLL重写,测试完成注释掉即可
                #region 测试用
                /*
                /// <summary>
                /// 测试用来获取数据库连接字符串,测试通过即可注释
                /// </summary>
                /// <returns></returns>
                public override string ReaderConnectionString()
                {
                    string connectionString = "Data Source=192.168.3.11\\dev;timeout=210;Initial Catalog=EFOS.Community;User ID=sa;Password=111111";
                    return connectionString;
                }
                 */
                #endregion
            #endregion

            //通过父类BaseBusiness读取数据库连接(由于Session获取失败,重写父类方法来初始化数据库连接字符串)
            //BLL的Dao则读取该数据库连接，从而访问数据库
            AirConditionEnergyManageBLL Service = new AirConditionEnergyManageBLL();

            //加断点调试
            ColumnChart result = Service.GetTotalEnergyTrend(2, 2, Convert.ToDateTime("2016-11-18"), TimeType.Day);
            bool expected = true;
            bool actual = result != null ? true : false; 
            Assert.AreEqual(expected, actual, "测试未通过");
        }

        ////2、Session必须依赖于在aspx页面如default.aspx页面请求,default.aspx.cs继承Page类
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);

        //    this.Session["Platform"] = new Apply_Platform(){
        //                                                PlatformCode = "001",
        //                                                SQLConnectionString = "Data Source=192.168.3.11\\dev;timeout=210;Initial Catalog=EFOS.Community;User ID=sa;Password=111111"
        //                                               };//PlatformCode = "001"//可以不要;
        //}

    }
}
