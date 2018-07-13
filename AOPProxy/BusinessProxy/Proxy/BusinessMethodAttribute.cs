using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessProxy
{
    /// <summary>
    /// 拦截方法属性类--过滤方法、传递拦截方法参数
    /// </summary>
    public class BusinessMethodAttribute:Attribute
    {
        Transaction transOption = Transaction.None;
        bool logOption = false;
        string description;

        /// <summary>
        /// 是否开启事务
        /// </summary>
        public Transaction Transaction
        {
            get { return this.transOption; }
        }

        /// <summary>
        /// 是否写日志
        /// </summary>
        public bool LogOption
        {
            get
            {
                return this.logOption;
            }
        }

        /// <summary>
        /// 日志描述
        /// </summary>
        public string Description
        {
            get { return this.description; }
        }


    }
    public enum Transaction
    {
        /// <summary>
        /// 不开启事物
        /// </summary>
        None = 1,

        /// <summary>
        /// 开启普通事物
        /// </summary>
        Lock = 2,

        /// <summary>
        /// 允许脏读的事务
        /// </summary>
        WithNoLock = 3
    }
}
