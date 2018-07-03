using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Master.Model;
using Data.Tools;
using WCFDuplexClientBaseApplication;
namespace Service.Task.Master
{
    public class PlatformMonitorTask : TaskBase
    {
        bool send = true;
        MasterInfoBLL masterInfoBLL = null;
        List<Apply_Platform> platforms = null;
        string mobiles = null;

        public PlatformMonitorTask()
        {
            this.IntervalTime = 10 * 60;//加载插件类赋值为1了，构造方法内赋的值会被重新赋值为1，因此每次执行方法都需要重新赋值
        }
        public override string TaskName
        {
            get { return "EFOS平台数据中断监测"; }
        }
        public override void Service(DateTime now)
        {
            //加载插件类赋值为1了，构造方法内赋的值会被重新赋值为1，因此每次执行方法都需要重新赋值
            this.IntervalTime = 10;//60;//可能是数据库异常，因此60s最好
            string message = "";
            try
            {
                if (this.masterInfoBLL == null)
                {
                    this.masterInfoBLL = new MasterInfoBLL();
                    this.platforms = this.masterInfoBLL.getMonitorPlatform();
                    //this.mobiles = this.masterInfoBLL.getNotifyPhones();//为了动态读取修改接收人，每次重新加载
                }

                //数据库可访问性测试
                this.masterInfoBLL.getMonitorPlatform();
                //为了动态读取修改接收人，每次重新加载
                this.mobiles = this.masterInfoBLL.getNotifyPhones();

                foreach (var item in this.platforms)
                {
                    if (!this.platformMonitor(masterInfoBLL, now, item))
                    {
                        //message += item.PlatformName+";";
                        message += item.PlatformCode + ";";
                    }
                }
                if (!string.IsNullOrEmpty(message))
                {
                    this.WriteError(new Exception((this.send ? "接收人：[" + this.mobiles + "] 发送短信--" : "仍未恢复：") + "数据中断告警!\n平台编号:\n" + message));

                    if (this.send && !string.IsNullOrEmpty(this.mobiles))
                    {
                        this.send = false;
                        this.SendOMMessage(4082635, this.mobiles, "[\"" + this.TaskName + "\"," + "\"" + message + "\"]");
                    }
                }
                else
                {
                    this.WatchMessage("服务器运行正常", MessageType.Message);
                }
            }
            catch (Exception e)
            {
                this.WriteError(e);
                this.WriteError(new Exception((this.send ? "接收人：[" + this.mobiles + "] 发送短信--" : "仍未恢复：") + "数据库异常告警!\n"));
                if (this.send && !string.IsNullOrEmpty(this.mobiles))
                {
                    this.send = false;
                    this.SendOMMessage(3963035, this.mobiles, "[\"" + this.TaskName + "\"]");
                }
            }


        }

        /// <summary>
        /// 监测平台是否正常运行
        /// </summary>
        /// <param name="now"></param>
        /// <param name="platform"></param>
        public bool platformMonitor(MasterInfoBLL masterInfoBLL, DateTime now, Apply_Platform platform)
        {
            bool isRunning = true;
            isRunning = masterInfoBLL.isNormalRunning(now, platform.SQLConnectionString);
            return isRunning;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phones"></param>
        public void SendOMMessage(int templateid, string mobiles, string paramters)
        {
            //var appKey = ServiceContext.AppKey;
            //var appSecret = ServiceContext.AppSecret;

            //int templateid;
            //Int32.TryParse(ServiceContext.TemplateId,out templateid);

            new NetEaseSMSUtil().SendMessageService(ServiceContext.AppKey, ServiceContext.AppSecret, templateid, mobiles, paramters);
        }
    }
}
