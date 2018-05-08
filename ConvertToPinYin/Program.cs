using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertToPinYin
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 汉字转换为拼音
            var x1 = PinYinConverter.GetPinYins("亿达别苑长城");
            var x2 = PinYinConverter.GetFirstPinYins("亿达别苑长城");
            #endregion
        }
    }
}
