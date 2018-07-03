using System;
using System.Collections.Generic;
//using LinqToSQL.Extend;
//using EFOS.Data;
//using EFOS.Redis;
using WCFDuplexClientBaseApplication;

namespace Service.Task.Master
{
    public class ServicePlugin : PluginBase
    {
        MasterRedisConfig masterRedisConfig = null;

        public override void Initialize()
        {
            try
            {
                ServiceContext.SQLConnectionString = this.Config["ConnectionString"].Value;
                ServiceContext.MasterRedisPath = this.Config["RedisPath"].Value;
                ServiceContext.AppKey = this.Config["NetEaseAppKey"].Value;
                ServiceContext.AppSecret = this.Config["NetEaseAppSecret"].Value;
                ServiceContext.TemplateId = this.Config["NetEaseTemplateId"].Value;

                //int port = int.Parse(this.Config["WCFPort"].Value);
            }
            catch (Exception ex)
            {
                base.WriteError(ex);
            }



            //添加任务线程
            this.AddTask(new PlatformMonitorTask());
            this.AddTask(new PTask1());
            this.AddTask(new PTask2());

            //开始所有配置为启用的线程
            this.StartTasks();


        }
    }
}