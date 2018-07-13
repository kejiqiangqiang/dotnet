using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.SqlClient;

namespace BusinessProxy
{
    /// <summary>
    /// 数据访问接口
    /// </summary>
    public interface ITemplateBaseDao
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        SqlDataContext DataContext{get;set;}
    }
}