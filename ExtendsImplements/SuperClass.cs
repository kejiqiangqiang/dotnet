using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendsImplements
{
    
    #region 抽象类方法测试
    //public abstract class A
    #endregion

    /// <summary>
    /// 基类
    /// </summary>
    public class A
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string member = "A";//virtual

        public virtual string GetMember() { return "A.member=" + member; }

        /// <summary>
        /// 属性
        /// </summary>
        public virtual string Property { get { return "A"; } set { value = ""; } }//virtual

        /// <summary>
        /// 父类普通方法
        /// </summary>
        public string Parent()
        {
            return "父类普通方法";
        }
        /// <summary>
        /// 父类虚方法
        /// </summary>
        public virtual string ParentVirtual()
        {
            return "父类virtual虚方法";
        }

        #region 抽象方法测试

        /// <summary>
        /// 父类抽象方法
        /// </summary>
        //public abstract string ParentAbstract();

        #endregion

    }
    
}
