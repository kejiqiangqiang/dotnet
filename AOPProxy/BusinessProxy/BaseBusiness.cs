using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BusinessProxy
{
    /// <summary>
    /// 业务逻辑基类
    /// </summary>
    public class BaseBusiness : AbsBusiness
    {
        ///// <summary>
        ///// 实时数据查询
        ///// </summary>
        //public ApplyRedisRealData ApplyRedisRealData { get; private set; }

        ///// <summary>
        ///// 应用平台配置信息
        ///// </summary>
        //public ApplyRedisConfig ApplyRedisConfig { get; private set; }


        ///// <summary>
        ///// 主要平台数据
        ///// </summary>
        //public MasterRedisConfig MasterRedisConfig { get; private set; }

        /// <summary>
        /// 应用平台业务逻辑
        /// </summary>
        public BaseBusiness()
        {
            //this.ApplyRedisRealData = new ApplyRedisRealData(WebKeyManager.MasterRedisPath, WebKeyManager.Platform.DataServerRedisUrl);
            //this.ApplyRedisConfig = new ApplyRedisConfig(WebKeyManager.Platform.DataServerRedisUrl);
            //this.MasterRedisConfig = new MasterRedisConfig(WebKeyManager.MasterRedisPath);
        }

        
        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <returns>解密后的ConnectionString</returns>
        public override string ReaderConnectionString()
        {
            //<add key="EFOS.Master" value="Data Source=192.168.3.179;timeout=210;Initial Catalog=EFOS.Master;User ID=sa;Password=111111"/>
            string connectionString = "Data Source=192.168.3.179;timeout=210;Initial Catalog=EFOS.Master;User ID=sa;Password=111111";//WebKeyManager.Platform.SQLConnectionString;
            return connectionString;
        }

        /// <summary>
        /// 执行异常日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public override void WriteException(IMessage message, Exception e)
        {
            
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="description"></param>
        public override void WriteLog(IMessage message, string description)
        {

        }
        /// <summary>
        /// 写系统操作日志
        /// </summary>
        /// <param name="description">操作信息</param>
        public void WriteLog(string description)
        {
            
        }

        /// <summary>
        /// 写项目操作日志
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="description">操作信息</param>
        public void WriteLog(int projectCode, string description)
        {
            #region 关于代理IP
            //string userHostAddress = null;
            //string serverVariables = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            ////如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            //if (!string.IsNullOrEmpty(serverVariables))
            //{
            //    userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            //}
            //else//否则直接读取REMOTE_ADDR获取客户端IP地址
            //{
            //    userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //}
            #endregion

        }

        /// <summary>
        /// 写项目操作日志
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="description">操作信息</param>
        public void WriteLog(int projectCode, string description, string userName)
        {
            
        }

        /// <summary>
        /// 将IList转换为DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ConvertToDataTable(IList list, string[] name, Type[] tof)
        {
            if (list == null)
            {
                return null;
            }

            DataTable dt = new DataTable();
            for (int i = 0; i < name.Length; i++)
                dt.Columns.Add(name[i], tof[i]);
            foreach (dynamic l in list)
            {
                var t = l.GetType();
                System.Reflection.PropertyInfo[] pInfo = t.GetProperties();
                DataRow dr = dt.NewRow();
                // 遍历公共属性
                foreach (System.Reflection.PropertyInfo pio in pInfo)
                {
                    //string fieldName = pio.Name;        // 公共属性的Name

                    //Type pioType = pio.PropertyType;    // 公共属性的类型
                    if (name.Contains(pio.Name))
                    {
                        dr[pio.Name] = pio.GetValue(l, null);
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable ConvertToDataTable(IList list, string[] name)
        {
            if (list == null)
            {
                return null;
            }

            DataTable dt = new DataTable();
            for (int i = 0; i < name.Length; i++)
                dt.Columns.Add(name[i], typeof(string));
            foreach (dynamic l in list)
            {
                var t = l.GetType();
                System.Reflection.PropertyInfo[] pInfo = t.GetProperties();
                DataRow dr = dt.NewRow();
                // 遍历公共属性
                foreach (System.Reflection.PropertyInfo pio in pInfo)
                {
                    //string fieldName = pio.Name;        // 公共属性的Name

                    //Type pioType = pio.PropertyType;    // 公共属性的类型
                    if (name.Contains(pio.Name))
                    {
                        dr[pio.Name] = pio.GetValue(l, null);
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public string ConvertDataTableToArray(DataTable dt)
        {
            string str = "[";
            string ls = "";
            int cl = dt.Columns.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ls = "[";
                for (int j = 0; j < cl; j++)
                {
                    if (dr[j] != null && dr[j].ToString() != "")
                        ls += "" + dr[j] + ",";
                    else ls += "null,";
                }
                if (ls != "[")
                    ls = ls.Substring(0, ls.Length - 1) + "],";
                else ls = "";
                str += ls;
            }
            if (str != "[")
                str = str.Substring(0, str.Length - 1) + "]";
            else str = "";
            return str;
        }

        /// <summary>
        /// 将数值转换为Excel表格的列序号
        /// </summary>
        /// <param name="i">自然列序号，如1,2,3,4,…</param>
        /// <returns>Excel表格中的列序号，例如A,B,C,…,AA,AB,…,BA,BB,…</returns>
        public string ConvertIntToExcelCol(int i)
        {
            string str = "";
            str = ((i <= 26) ? "" : ((char)((int)('A') + i / 26 - 1)).ToString()) + ((char)((int)('A') + i % 26 - 1)).ToString();
            return str;
        }

        public enum PharseEnum { 小时 = 0, 日, 周, 月, 季, 年, 双时, 双日, 双周, 双月, 半时, 班次 }

        public override void ChangeDataTable(List<string> tableNames)
        {
            //var sqlDataOnChange = (from t in tableNames
            //                       select new SqlDataOnChange
            //                       {
            //                           TableName = t
            //                       }).ToList();
            //this.ApplyRedisConfig.SetSqlDataOnChange(sqlDataOnChange);
        }

        
    }
}
