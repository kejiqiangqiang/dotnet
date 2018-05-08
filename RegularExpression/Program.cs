using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularExpression
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 正则表达式

            //报警值提取
            string expression = "1_1==1&&1_47>300||1_48<500";
            var key = "1_47"+">";
            string pattern = @"1_47>\d+\.*\d*";
            Regex regMatch = new Regex(pattern);
            var match = regMatch.Match(expression);
            var value = Convert.ToSingle(match.Value.Replace(key, ""));
            //1_1+2_1>180---->180
            RegularExpression.GetAlarmConfigValue(">", expression);

            //以x开头加两个空格后接纯数字结尾
            string reg_split = @"^x\s{2}\d*$";
            string expression1 = "x  998";
            string expression2 = "x 998";
            string expression3 = "x  998xx";
            string expression4 = "x  998xxx  998";
            string expression5 = "x  x  998xxx  998";




            Regex regex_split = new Regex(reg_split);

            var matches = regex_split.IsMatch(expression1);
            var matches2 = regex_split.IsMatch(expression2);
            var matches3 = regex_split.IsMatch(expression3);
            var matches4 = regex_split.IsMatch(expression4);
            var matches5 = regex_split.IsMatch(expression5);


            //string ex1 = "[1号变压器].[A相电流]+[1号变压器].[B相电流]-[2号变压器].[A相电流]";
            //string ex2 = "[1号变压器].[A相电流]";
            //string ex3 = "[1号变压器].[A相电流]>45";
            //string ex4 = "[1号变压器].[启停状态]==1";
            //string ex5 = "Max([1号变压器],[1号变压器].[B相电压],[1号变压器].[C相电压])";
            //string ex6 = "([1号变压器].[A相电压]+[1号变压器].[B相电压]+[1号变压器].[C相电压])>5&&([1号变压器]+[1号变压器].[B相电流]+[1号变压器].[C相电流])>7";
            //string ex7 = "[1号变压器].[A相电流]";
            //string ex8 = "[1号变压器]+[2号变压器]+[3号变压器]";
            //string ex9 = "([1号变压器]+[1号变压器].[B相电压]+[1号变压器].[C相电压])>5&&([1号变压器]+[1号变压器].[B相电流]+[1号变压器].[C相电流])>7";
            //string ex10 = "[1号变压器].[A相电流]*1.72";
            //string ex11 = "[1号变压器]*1.72";

            //var rs1 = RegularExpression.RegexExpressionNest(ex1);
            //var rs2 = RegularExpression.RegexExpressionNest(ex2);
            //var rs3 = RegularExpression.RegexExpressionNest(ex3);
            //var rs4 = RegularExpression.RegexExpressionNest(ex4);
            //var rs5 = RegularExpression.RegexExpressionNest(ex5);
            //var rs6 = RegularExpression.RegexExpressionNest(ex6);
            //var rs7 = RegularExpression.RegexExpressionNest(ex7);
            //var rs8 = RegularExpression.RegexExpressionNest(ex8);
            //var rs9 = RegularExpression.RegexExpressionNest(ex9);
            //var rs10 = RegularExpression.RegexExpressionNest(ex10);
            //var rs11 = RegularExpression.RegexExpressionNest(ex11);

            string ex0 = "1_1+1.173+1_2";
            string ex1 = "1_1+1_2";
            string ex2 = "1_1==1_2";
            string ex3 = "1_1==0";
            string ex4 = "1_1*1.732";
            string ex5 = "1_1*25";
            string ex6 = "1_1*25+1_2>=1_5+1_3>=0+1_3<=0";


            var rs0 = RegularExpression.RegexExpressionNest(ex0);
            var rs1 = RegularExpression.RegexExpressionNest(ex1);
            var rs2 = RegularExpression.RegexExpressionNest(ex2);
            var rs3 = RegularExpression.RegexExpressionNest(ex3);
            var rs4 = RegularExpression.RegexExpressionNest(ex4);
            var rs5 = RegularExpression.RegexExpressionNest(ex5);
            var rs6 = RegularExpression.RegexExpressionNest(ex6);

            string ex001 = "20.2*1_1*25+1_2>=1_5+1_3>=0+1_3<=0";
            //var rs001 = RegularExpression.RegexExpressionNested(ex001);
            var rs001 = RegularExpression.RegexExpressionNesteded(ex001);
            #endregion
        }
    }
}
