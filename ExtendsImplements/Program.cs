using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendsImplements
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 类继承
            A A = new A();
            B B = new B();
            A A_B = new B();

            var x1 = A.Parent();
            var x2 = A.ParentVirtual();
            var x3 = B.Parent();
            var x4 = B.ParentVirtual();
            var v1 = A.member;
            var v2 = A.Property;
            var v3 = B.member;
            var v4 = B.Property;

            var v5 = A_B.member;
            var v6 = A_B.Property;
            A_B.member = "new";
            var v7 = A_B.GetMember();
            var v8 = A_B.member;
            var v9 = A_B.Property;

            var x5 = A_B.Parent();
            var x6 = A_B.ParentVirtual();
            //var x7 = A_B.ParentAbstract();
            #endregion
        }
    }
}
