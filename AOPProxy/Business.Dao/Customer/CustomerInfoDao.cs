using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using BusinessProxy;

namespace Business.Dao
{
    /// <summary>
    /// 会员信息表数据访问
    /// </summary>
    public partial class CustomerInfoDao : ITemplateBaseDao
    {
        public DataTable getCustomerInfo()
        {
            string sql = @"select * from CustomerInfo";
            SqlParameterCollection ps = new SqlParameterCollection();
            ps.Add("Id", "111", DbType.Int32);
            DataTable dt = this.ExecuteTable(CommandType.Text,sql,ps);
            return dt;
        }



    }


}
