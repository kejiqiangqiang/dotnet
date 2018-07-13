using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BusinessProxy
{
    /// <summary>
    /// 变化委托
    /// </summary>
    /// <param name="tableName">当前编号的表</param>
    public delegate void OnChange(string tableName);

    /// <summary>
    ///  数据库表变化通知，需要在监视的数据库中执行如下命令
    ///  Select DATABASEpRoPERTYEX('EFOS.Master','IsBrokerEnabled')
    ///  GO
    ///  ALTER DATABASE [EFOS.Master] SET NEW_BROKER WITH ROLLBACK IMMEDIATE;
    ///  ALTER DATABASE [EFOS.Master] SET ENABLE_BROKER;
    ///  exec sp_changedbowner @loginame = 'sa'  
    /// </summary>
    public static class SqlServerOnChangeNotice
    {
        static string connectionString = null;
        static Dictionary<string, string> dic = new Dictionary<string, string>();

        /// <summary>
        /// 变化委托
        /// </summary>
        public static event OnChange OnChange;

        /// <summary>
        /// 开始监视
        /// </summary>
        public static void Start(string connectionString)
        {
            SqlServerOnChangeNotice.connectionString = connectionString;
            SqlDependency.Stop(connectionString);
            SqlDependency.Start(connectionString);
        }

        /// <summary>
        /// 结束监视
        /// </summary>
        public static void Stop()
        {
            SqlDependency.Stop(connectionString);
        }

        /// <summary>
        /// 监视表的数据
        /// </summary>
        private static void MonitorItem(string tableName)
        {
            var conn = new SqlConnection(SqlServerOnChangeNotice.connectionString);
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = @"select c.name  from sys.columns c,sys.objects o
                                    where 
                                    c.object_id=o.object_id and
                                    o.name='" + tableName + "'";

                List<string> columns = new List<string>();
                using (SqlDataReader colsdr = cmd.ExecuteReader())
                {
                    while (colsdr.Read())
                    {
                        columns.Add(colsdr["name"].ToString());
                    }
                }
                string cols = string.Join(",", columns.ToArray());
                cols = cols.TrimEnd(",".ToCharArray());

                cmd.CommandText = "SELECT " + cols + " FROM dbo." + tableName;
                SqlDependency dependency = new SqlDependency();
                dependency.AddCommandDependency(cmd);
                dic.Add(dependency.Id, tableName);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                dr = cmd.ExecuteReader();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                    dr = null;
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
                if (conn != null)
                {
                    if (conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Dispose();
                    conn = null;
                }
            }
        }

        /// <summary>
        /// 监视表的数据
        /// </summary>
        public static void Monitor(List<string> tableNames)
        {
            dic.Clear();
            foreach (var tableName in tableNames)
            {
                MonitorItem(tableName);
            }
        }

        static void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency sqlDependency = (sender as SqlDependency);
            var tableName = dic[sqlDependency.Id];
            dic.Remove(sqlDependency.Id);
            sqlDependency.OnChange -= dependency_OnChange;
            sqlDependency = null;
            if (OnChange != null)
            {
                OnChange(tableName);
            }
            MonitorItem(tableName);
        }
    }
}