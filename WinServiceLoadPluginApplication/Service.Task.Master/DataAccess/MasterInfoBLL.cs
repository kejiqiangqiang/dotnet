using Data.Master.DAL;
using Data.Master.Model;
using LinqToSQL.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Task.Master
{
    public class MasterInfoBLL : BaseBusiness
    {
        Apply_PlatformDao Apply_PlatformDao { get; set; }
        Apply_OperationalStaffDao Apply_OperationalStaffDao { get; set; }

        /// <summary>
        /// 获取需要监控平台
        /// </summary>
        /// <returns></returns>
        [BusinessMethod]
        public List<Apply_Platform> getMonitorPlatform()
        {
            var models = Apply_PlatformDao.Entity.Where(p => p.IsMonitor).ToList();
            return models;
        }
        /// <summary>
        /// 获取通知人电话
        /// </summary>
        /// <returns></returns>
        [BusinessMethod]
        public string getNotifyPhones()
        {
            string mobiles = null;
            var phones = Apply_OperationalStaffDao.Entity.Select(p => p.Phone).ToList();
            if (phones != null && phones.Count > 0)
            {
                mobiles = Newtonsoft.Json.JsonConvert.SerializeObject(phones);
            }
            return mobiles;
        }
        /// <summary>
        /// 根据历史数据是否存储为最新数据判断平台是否正常运行(以结果为导向)
        /// </summary>
        /// <param name="dateNow"></param>
        /// <param name="sqlConnectionString"></param>
        /// <returns></returns>
        [BusinessMethod]
        public bool isNormalRunning(DateTime dateNow, string sqlConnectionString)
        {
            bool isRunning = true;
            string dbName = new Regex(@"Initial Catalog=([\s\S]+?);").Match(sqlConnectionString).Result("$1") + dateNow.ToString("yyyyMM");
            string tableName = "EFOS_HisData" + dateNow.ToString("dd");
            //长城所有数据存储频率均为1小时
            //string tableName = "EFOS_HourHisData";
            this.Apply_PlatformDao.DataContext.Connection.ChangeDatabase(dbName);
            string sql = @" select top 1 ProjectCode from " + tableName + @"
                            where DataCode=1 and PointCode not like 'P%' and CollectTime >='" + dateNow.AddMinutes(-10) + @"'
";
            var sqlDataReader = this.Apply_PlatformDao.ExecuteReader(sql);//dateNow.AddMinutes(-10)//长城所有数据存储频率均为1小时//dateNow.AddHours(-1)
            isRunning = sqlDataReader.HasRows;
            return isRunning;
        }
    }
}
