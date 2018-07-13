using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq.Mapping;

namespace BusinessProxy
{
    /// <summary>
    /// 数据访问类抽象基类--执行SqlCommand数据访问
    /// </summary>
    public abstract class BaseTemplateDao : ITemplateBaseDao
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public SqlDataContext DataContext { get; set; }

        #region ExecuteNonQuery
        /// <summary>
        /// 执行数据库操作返回受影响的行数
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="dbParameter">参数集合</param>
        /// <returns>受影响的行数</returns>
        protected int ExecuteNonQuery(CommandType cmdType, string cmdText, SqlParameterCollection dbParameter)
        {
            this.DataContext.SqlCommand.Parameters.Clear();
            if (dbParameter != null)
            {
                this.DataContext.SqlCommand.Parameters.AddRange(dbParameter.ToArray());
            }
            this.DataContext.SqlCommand.CommandText = cmdText;
            this.DataContext.SqlCommand.CommandType = cmdType;
            return this.DataContext.SqlCommand.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行数据库操作返回受影响的行数
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <returns>受影响的行数</returns>
        protected int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            int n = this.ExecuteNonQuery(cmdType, cmdText, null);
            return n;
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// 执行数据库操作返回结果集
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="dbParameter">参数集合</param>
        /// <returns>受影响的行数</returns>
        protected SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, SqlParameterCollection dbParameter)
        {
            this.DataContext.SqlCommand.Parameters.Clear();
            if (dbParameter != null)
            {
                this.DataContext.SqlCommand.Parameters.AddRange(dbParameter.ToArray());
            }
            this.DataContext.SqlCommand.CommandText = cmdText;
            this.DataContext.SqlCommand.CommandType = cmdType;
            SqlDataReader dr = this.DataContext.SqlCommand.ExecuteReader();
            return dr;
        }

        /// <summary>
        /// 执行数据库操作返回结果集
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <returns>受影响的行数</returns>
        protected SqlDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            SqlDataReader dr = this.ExecuteReader(cmdType, cmdText, null);
            return dr;
        }

        /// <summary>
        /// 执行数据库操作返回结果集
        /// </summary>
        /// <param name="cmdText">sql语句</param>
        /// <param name="dbParameter">参数集合</param>
        /// <returns>受影响的行数</returns>
        protected SqlDataReader ExecuteReader(string cmdText, SqlParameterCollection dbParameter)
        {
            return this.ExecuteReader(CommandType.Text, cmdText, dbParameter);
        }

        /// <summary>
        /// 执行数据库操作返回结果集
        /// </summary>
        /// <param name="cmdText">sql语句</param>
        /// <returns>受影响的行数</returns>
        public SqlDataReader ExecuteReader(string cmdText)
        {
            return this.ExecuteReader(CommandType.Text, cmdText, null);
        }

        #endregion

        #region ExecuteTable
        /// <summary>
        /// 执行数据库操作返回数据表
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="dbParameter">参数集合</param>
        /// <returns>回数据表</returns>
        public DataTable ExecuteTable(CommandType cmdType, string cmdText, SqlParameterCollection dbParameter)
        {
            this.DataContext.SqlCommand.Parameters.Clear();
            if (dbParameter != null)
            {
                this.DataContext.SqlCommand.Parameters.AddRange(dbParameter.ToArray());
            }
            DataTable dt = new DataTable();
            this.DataContext.SqlCommand.CommandType = cmdType;
            this.DataContext.SqlCommand.CommandText = cmdText;
            this.DataContext.SqlDataAdapter.Fill(dt);
            return dt;
        }
        /// <summary>
        /// 执行数据库操作返回数据表
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <returns>回数据表</returns>
        public DataTable ExecuteTable(CommandType cmdType, string cmdText)
        {
            DataTable dt = this.ExecuteTable(cmdType, cmdText, null);
            return dt;
        }
        /// <summary>
        /// 执行数据库操作返回DataTable
        /// </summary>
        /// <param name="cmdText">sql语句</param>
        /// <param name="dbParameter">参数集合</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteTable(string cmdText, SqlParameterCollection dbParameter)
        {
            return this.ExecuteTable(CommandType.Text, cmdText, dbParameter);
        }
        /// <summary>
        /// 执行数据库操作返回DataTable
        /// </summary>
        /// <param name="cmdText">sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteTable(string cmdText)
        {
            return this.ExecuteTable(CommandType.Text, cmdText, null);
        }
        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="dbParameter">参数集合</param>
        /// <returns>受影响的行数</returns>
        protected object ExecuteScalar(CommandType cmdType, string cmdText, SqlParameterCollection dbParameter)
        {
            this.DataContext.SqlCommand.Parameters.Clear();
            if (dbParameter != null)
            {
                this.DataContext.SqlCommand.Parameters.AddRange(dbParameter.ToArray());
            }
            this.DataContext.SqlCommand.CommandType = cmdType;
            this.DataContext.SqlCommand.CommandText = cmdText;
            object val = this.DataContext.SqlCommand.ExecuteScalar();
            return val;
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <returns>受影响的行数</returns>
        protected object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            object val = this.ExecuteScalar(cmdType, cmdText, null);
            return val;
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="cmdText">sql语句</param>
        /// <param name="dbParameter">参数集合</param>
        /// <returns>受影响的行数</returns>
        protected object ExecuteScalar(string cmdText, SqlParameterCollection dbParameter)
        {
            return this.ExecuteScalar(CommandType.Text, cmdText, dbParameter);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="cmdText">sql语句</param>
        /// <returns>受影响的行数</returns>
        protected object ExecuteScalar(string cmdText)
        {
            return this.ExecuteScalar(CommandType.Text, cmdText, null);
        }
        #endregion

        #region ExecutePaging
        /// <summary>
        /// 执行动态分页方法
        /// </summary>
        /// <param name="cmdText">分页的SQL语句</param>
        /// <param name="dbParameter">动态参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="sortName">排序列</param>
        /// <param name="sortOrder">desc/asc</param>
        /// <returns>数据</returns>
        protected DataTable ExecutePaging(string cmdText, SqlParameterCollection dbParameter, int pageIndex, int pageSize, string sortName, string sortOrder, out int total)
        {
            if (string.IsNullOrEmpty(sortName))
            {
                throw new ArgumentNullException("sortName", "排序列不能为空");
            }
            if (string.IsNullOrEmpty(sortOrder))
            {
                throw new ArgumentNullException("sortOrder", "排序方向不能为空");
            }
            StringBuilder sb = new StringBuilder();
            if (dbParameter == null)
            {
                dbParameter = new SqlParameterCollection();
            }
            sb.Append("select count(0) from (" + cmdText + ") t where 1=1 ");
            total = (Int32)this.ExecuteScalar(sb.ToString(), dbParameter);
            sb.Clear();
            sb.Append("select * from ");
            sb.Append("(");
            cmdText = "select ROW_NUMBER() over (order by " + sortName + " " + sortOrder + ") as RowID," + cmdText.Trim().Remove(0, 6);
            sb.Append(cmdText);
            sb.Append(")a where RowID between (@pageIndex-1)*@pagesize+1 AND @pageIndex * @pagesize ");

            if (sortName.IndexOf('.') >= 0)
            {
                sortName = sortName.Substring(sortName.IndexOf('.') + 1, sortName.Length - sortName.IndexOf('.') - 1);
            }
            sb.Append("order by " + sortName + " " + sortOrder);
            dbParameter.Add("pageIndex", pageIndex, DbType.Int32);
            dbParameter.Add("pageSize", pageSize, DbType.Int32);
            DataTable data = this.ExecuteTable(sb.ToString(), dbParameter);
            return data;
        }
        #endregion

        /// <summary>
        /// 执行数据库操作返回DataSet
        /// </summary>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="dbParameter">参数集合</param>
        /// <returns>DataSet</returns>
        protected DataSet ExecuteDataSet(CommandType cmdType, string cmdText, SqlParameterCollection dbParameter)
        {
            this.DataContext.SqlCommand.Parameters.Clear();
            if (dbParameter != null)
            {
                this.DataContext.SqlCommand.Parameters.AddRange(dbParameter.ToArray());
            }
            DataSet dt = new DataSet();
            this.DataContext.SqlCommand.CommandType = cmdType;
            this.DataContext.SqlCommand.CommandText = cmdText;
            this.DataContext.SqlDataAdapter.Fill(dt);
            return dt;
        }




    }
}