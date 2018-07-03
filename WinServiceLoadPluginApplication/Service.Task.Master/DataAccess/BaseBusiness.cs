
using LinqToSQL.Extend;
using System.Runtime.Remoting.Messaging;
using System;
using System.Collections.Generic;
using Service.Task.Master;

namespace Service.Task.Master
{
    public class BaseBusiness : AbsBusiness
    {
        /// <summary>
        /// 读取连接字符串时
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <returns>解密后的ConnectionString</returns>
        public override string ReaderConnectionString()
        {
            return ServiceContext.SQLConnectionString;
        }
        
        public override void WriteException(IMessage message, System.Exception e)
        {

        }

        public override void WriteLog(IMessage message, string description)
        {

        }

        public override void ChangeDataTable(List<string> tableNames)
        {
            //throw new NotImplementedException();

            var sqlDataOnChange = (from t in tableNames
                                   select new SqlDataOnChange
                                   {
                                       TableName = t
                                   }).ToList();
            ServiceContext.MasterRedisConfig.SetSqlDataOnChange(sqlDataOnChange);
        }

    }
}


