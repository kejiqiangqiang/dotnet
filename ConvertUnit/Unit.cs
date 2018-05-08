using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertUnit
{
    class Unit
    {

        //递归执行次数
        private static int execute = 0;
        private static string[] sperator = { "," };


        /// <summary>
        /// 递归获取数据相应单位
        /// </summary>
        /// <param name="data"></param>
        /// <param name="units">单位进制为10³且从小到大以逗号分隔"kWh,mwh"</param>
        /// <returns></returns>
        public static object GetUnit(decimal data, string units)
        {
            execute++;

            var unit = units.Split(sperator, System.StringSplitOptions.RemoveEmptyEntries);
            if (units == null) { return new { Data = data, Unit = "" }; }
            if (data / 1000 < 1 || execute > unit.Length)//数据太大常用单位可能也不够，此时应该结束递归
            {
                //数据太大常用单位可能也不够，此时应该以倒数第二次递归data的作为数值，即最后一次递归传入的data*1000以还原
                var val = execute > unit.Length ? data * 1000 : data;
                //数据太大常用单位可能也不够，此时应该取最后一个单位
                execute = execute <= unit.Length ? execute : unit.Length;
                var ob = new { Data = val, Unit = unit[execute - 1] };
                //静态变量重置为0
                execute = 0;
                return ob;
            }
            else
            {
                //data=data/1000;
                return GetUnit(data / 1000, units);
            }
        }


        /// <summary>
        /// 曲线坐标轴范围及刻度的自适应算法
        /// </summary>
        /// dynamic d = Standard(291.5d, 0, 4);
        /// int tmpstep = Convert.ToInt32(d.tmpstep);
        /// <param name="cormax">最大值</param>
        /// <param name="cormin">最小值</param>
        /// <param name="cornumber">刻度数</param>
        /// <returns></returns>
        public static object Standard(double cormax, double cormin, int cornumber)
        {
            double tmpmax, tmpmin, corstep, tmpstep, temp;
            int tmpnumber, extranumber;
            if (cormax <= cormin)
                return null;
            corstep = (cormax - cormin) / cornumber;
            if (Math.Pow(10, Convert.ToInt32(Math.Log(corstep) / Math.Log(10))) == corstep)
            {
                temp = Math.Pow(10, Convert.ToInt32(Math.Log(corstep) / Math.Log(10)));
            }
            else
            {
                temp = Math.Pow(10, (Convert.ToInt32(Math.Log(corstep) / Math.Log(10)) + 1));
            }
            tmpstep = Math.Round((corstep / temp), 6, MidpointRounding.AwayFromZero);
            //选取规范步长
            if (tmpstep >= 0 && tmpstep <= 0.1)
            {
                tmpstep = 0.1;
            }
            else if (tmpstep >= 0.100001 && tmpstep <= 0.2)
            {
                tmpstep = 0.2;
            }
            else if (tmpstep >= 0.200001 && tmpstep <= 0.25)
            {
                tmpstep = 0.25;
            }
            else if (tmpstep >= 0.250001 && tmpstep <= 0.5)
            {
                tmpstep = 0.5;
            }
            else
            {
                tmpstep = 1;
            }
            tmpstep = tmpstep * temp;
            if (Convert.ToInt32(cormin / tmpstep) != (cormin / tmpstep))
            {
                if (cormin < 0)
                {
                    cormin = (-1) * Math.Ceiling(Math.Abs(cormin / tmpstep)) * tmpstep;
                }
                else
                {
                    cormin = Convert.ToInt32(Math.Abs(cormin / tmpstep)) * tmpstep;
                }

            }
            if (Convert.ToInt32(cormax / tmpstep) != (cormax / tmpstep))
            {
                cormax = Convert.ToInt32(cormax / tmpstep + 1) * tmpstep;
            }
            tmpnumber = Convert.ToInt32((cormax - cormin) / tmpstep);
            if (tmpnumber < cornumber)
            {
                extranumber = cornumber - tmpnumber;
                tmpnumber = cornumber;
                if (extranumber % 2 == 0)
                {
                    cormax = cormax + tmpstep * Convert.ToInt32(extranumber / 2);
                }
                else
                {
                    cormax = cormax + tmpstep * Convert.ToInt32(extranumber / 2 + 1);
                }
                cormin = cormin - tmpstep * Convert.ToInt32(extranumber / 2);
            }
            cornumber = tmpnumber;
            return new { cormax = cormax, cormin = cormin, cornumber = cornumber, tmpstep = tmpstep };
        }

    }
}
