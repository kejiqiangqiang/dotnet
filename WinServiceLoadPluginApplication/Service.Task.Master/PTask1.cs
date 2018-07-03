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
    public class PTask1: TaskBase
    {
        bool isfirst = true;
        MasterInfoBLL masterInfoBLL = null;

        public PTask1()
        {
            this.IntervalTime = 10 * 60;//加载插件类赋值为1了，构造方法内赋的值会被重新赋值为1，因此每次执行方法都需要重新赋值
        }
        public override string TaskName
        {
            get { return "数据同步"; }
        }
        public override void Service(DateTime now)
        {
            //加载插件类赋值为1了，构造方法内赋的值会被重新赋值为1，因此每次执行方法都需要重新赋值
            this.IntervalTime = 10;//60;//可能是数据库异常，因此60s最好
            try
            {
                if (this.masterInfoBLL == null)
                {
                    this.masterInfoBLL = new MasterInfoBLL();
                }
                //do something
            }
            catch (Exception e)
            {
                this.WriteError(e);
                
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

       
    }
}
