using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 递归单位转换
            var ob = Unit.GetUnit(999.9m, "kWh,mWh");
            var ob1 = Unit.GetUnit(9999.9m, "kWh,mWh");
            var ob2 = Unit.GetUnit(9999999.9m, "kWh,mWh");
            #endregion

            #region 刻度步长的自适应算法
            //最大值、最小值、刻度数
            dynamic d = Unit.Standard(291.5d, 0, 4);
            int tmpstep = Convert.ToInt32(d.tmpstep);
            #endregion

        }
    }
}
