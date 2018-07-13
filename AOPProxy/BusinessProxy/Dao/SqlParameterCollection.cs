using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace BusinessProxy
{
    public class SqlParameterCollection : List<SqlParameter>
    {

        /// <summary>
        /// 根据参数名称返回参数信息
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns>值</returns>
        public DbParameter this[string parameterName]
        {
            get
            {
                DbParameter value = null;
                foreach (SqlParameter ps in this)
                {
                    value = ps as DbParameter;
                    if (value.ParameterName==parameterName)
                    {
                        break; 
                    }
                }
                return value;
            }
        }


        /// <summary>
        /// 增加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">长度</param>
        public void Add(string name, object value, DbType dbType, int size)
        {
            this.Add(name, value, dbType, size, ParameterDirection.Input);
        }

        /// <summary>
        /// 增加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        public void Add(string name, object value, DbType dbType)
        {
            this.Add(name, value, dbType, ParameterDirection.Input);
        }

        /// <summary>
        /// 增加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="direction">输入输出类型</param>
        public void Add(string name, object value, DbType dbType, ParameterDirection direction)
        {
            SqlParameter ps = new SqlParameter();
            ps.ParameterName = name;
            ps.Value = value == null ? DBNull.Value : value;
            ps.DbType = dbType;
            ps.Direction = direction;
            this.Add(ps);
        }

        /// <summary>
        /// 增加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">长度</param> 
        /// <param name="direction">输入输出类型</param> 
        public void Add(string name, object value, DbType dbType, int size, ParameterDirection direction)
        {
            SqlParameter ps = new SqlParameter();
            ps.ParameterName = name;
            ps.Value = value == null ? DBNull.Value : value;
            ps.DbType = dbType;
            ps.Size = size;
            ps.Direction = direction;
            this.Add(ps);
        }

        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="name">参数名称</param>
        public void Remove(string name)
        {
            SqlParameter o = this[name] as SqlParameter;
            if (o != null)
            {
                this.Remove(o);
            }
        }

    }
}