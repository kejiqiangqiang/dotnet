using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendsImplements
{
    /// <summary>
    /// 子类
    /// </summary>
    public class B : A
    {
        #region 字段、属性--字段和属性统称成员变量

        /// <summary>
        /// 隐藏(默认new)字段
        /// </summary>
        public string member = "B";

        /// <summary>
        /// override重写覆盖字段
        /// </summary>
        //override string member;

        /// <summary>
        /// 更改上转型对象字段值，父类字段值不变，仅更改子类字段值
        /// </summary>
        /// <returns></returns>
        public override string GetMember()
        {
            return "B.base.member(A.member)=" + base.member + "|" + "B.member=" + member;
        }

        ///// <summary>
        ///// 隐藏属性
        ///// </summary>
        //public string Property { get; set; }

        /// <summary>
        /// 重写覆盖属性
        /// </summary>
        public override string Property { get { return "B"; } set { ;} }

        #endregion

        /// <summary>
        /// 子类方法与父类普通方法签名相同------子类方法将隐藏父类普通方法（即同签名方法默认加关键字new）
        /// </summary>
        public string Parent()//override不能对对父类普通方法使用，必须是对父类virtual或abstract
        {
            return "子类方法与父类普通方法签名相同";
        }

        /// <summary>
        /// 子类方法override重写覆盖(new隐藏)父类virtual方法--------子类方法将重写覆盖(隐藏)父类virtual方法
        /// </summary>
        public override string ParentVirtual()//new
        {
            return "子类方法override重写覆盖(new隐藏)父类virtual方法";
        }

        #region 重写抽象方法测试
        /// <summary>
        /// 子类方法override重写覆盖(必须重写覆盖且只能用override)父类abstract方法--------子类方法将重写覆盖父类abstract方法
        /// </summary>
        //public override string ParentAbstract()
        //{
        //    return "子类方法override重写覆盖父类abstract方法";
        //} 
        #endregion
    }

}
