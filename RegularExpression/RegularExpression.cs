using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularExpression
{
    class RegularExpression
    {

        /// <summary>
        /// 正则表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object RegexExpression(string expression)
        {
            //按分隔符提取所需字符串--split
            //按所需字符串提取所需字符串--match
            string reg_split_test = @"[\d|A-z|\u4E00-\u9FFF]{0,100}[\.][\d|A-z|\u4E00-\u9FFF]{0,100}[]|[[][\d|A-z|\u4E00-\u9FFF]{0,100}[\]]";
            string reg_match = @"[\d|A-z|\u4E00-\u9FFF]{0,100}[\.][\d|A-z|\u4E00-\u9FFF]{0,100}[]|[[][\d|A-z|\u4E00-\u9FFF]{0,100}[\]]";
            string reg_split = @"[\ |\~|\^|\(|\)|\%|\+|\-|\*|\/|\=|\||\,|\<|\>|\&|\||\!|Max|Min|Sum]{1,}[\d*|\.*|\d*]*";
            //string[] results_split = null;
            //string[] results_matches = null ;
            Regex regex_split_test = new Regex(reg_split_test);
            Regex regex_match = new Regex(reg_match);
            Regex regex_split = new Regex(reg_split);

            var results_split_test = regex_split_test.Split(expression);

            var matches = regex_match.Matches(expression);
            var results_matches = matches;

            var results_split = regex_split.Split(expression);//Split有空格
            results_split = results_split.Where(p => p != null && p != "").ToArray();
            return results_matches;
        }

        /// <summary>
        /// 正则表达式不包含某字符串
        /// //不包含某字符串[^(subexpression)]不能实现不包含--误解了取反:^[expression]或^(expression)表示从一行的开头匹配；[^expression]表示不包含单个字符，注意是单个字符
        /// //不包含某字符串正确实现--负预测先行：(?!subexpression)
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string[] RegexExpressionNest(string expression)
        {
            //不包含某字符串[^(subexpression)]不能实现
            //string reg_split = @"[\ |\~|\^|\(|\)|\%|\+|\-|\*|\/|\=|\||\,|\<|\>|\&|\||\!|Max|Min|Sum]+" + @"[\d+\.\d+|^(\d+_\d+)]";//1_2*1.732中的1.732与1_1+1_2中1_2的1有问题
            string reg_split = @"[\ |\~|\^|\(|\)|\%|\+|\-|\*|\/|\=|\||\,|\<|\>|\&|\||\!|Max|Min|Sum]+" + @"[^(\d+_\d+)]*";//1_1+1_2中1_2正常，1_2*1.732中的1.732有问题            
            //不包含某字符串正确实现--负预测先行：(?!subexpression)
            string regex = @"[\ |\~|\^|\(|\)|\%|\+|\-|\*|\/|\=|\||\,|\<|\>|\&|\||\!|Max|Min|Sum]+" + @"((?!(\d+_\d+))\d*\.*\d*)*";//负预测先行：(?!subexpression)只作判断，不会捕捉字符，因此要在使用时要另配和捕捉匹配
            var ex = Regex.Split(expression, regex);

            Regex regex_split = new Regex(reg_split);
            var results_split = regex_split.Split(expression);//Split有空格
            results_split = results_split.Where(p => p != null && p != "").ToArray();
            return results_split;
        }

        /// <summary>
        /// 还未解决bug--"20.2*1_1*25+1_2>=1_5+1_3>=0+1_3<=0";即包含常数在前面的情况
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string[] RegexExpressionNested(string expression)
        {
            //不包含某字符串正确实现--负预测先行：(?!subexpression)
            //string regex = @"[\ |\~|\^|\(|\)|\%|\+|\-|\*|\/|\=|\||\,|\<|\>|\&|\||\!|Max|Min|Sum]+" + @"((?!(\d+_\d+))\d*\.*\d*)*";//负预测先行：(?!subexpression)只作判断，不会捕捉字符，因此要在使用时要另配和捕捉匹配
            string regex = @"((?!(\d+_\d+))\d*\.*\d*)*" + @"[\ |\~|\^|\(|\)|\%|\+|\-|\*|\/|\=|\||\,|\<|\>|\&|\||\!|Max|Min|Sum]+" + @"((?!(\d+_\d+))\d*\.*\d*)*";
            Regex regex_split = new Regex(regex);
            var results_split = regex_split.Split(expression);//Split有空格
            results_split = results_split.Where(p => p != null && p != "").ToArray();
            return results_split;
        }

        /// <summary>
        /// 正向提取--解决bug--20.2*1_1*25+1_2>=1_5+1_3>=0+1_3<=0,即包含常数在前面的情况
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string[] RegexExpressionNesteded(string expression)
        {
            //string regex = @"((?!(\d+_\d+))\d*\.*\d*)*" + @"[\ |\~|\^|\(|\)|\%|\+|\-|\*|\/|\=|\||\,|\<|\>|\&|\||\!|Max|Min|Sum]+" + @"((?!(\d+_\d+))\d*\.*\d*)*";
            string regex = @"\d+_\d+";
            Regex regex_match = new Regex(regex);
            var matches = regex_match.Matches(expression);//正向
            string[] results_match = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                results_match[i] = matches[i].Value;
            }
            return results_match;
        }


        /// <summary>
        /// 获取告警配置临界值
        /// </summary>
        /// <param name="key">大于号(>)或小于号(<)</param>
        /// <param name="express"></param>
        /// <returns></returns>
        public static decimal? GetAlarmConfigValue(string key, string express)
        {
            decimal? value = null;
            string basepattern = @"=*\d+\.*\d*$";
            string pattern = key + basepattern;
            Regex reg = new Regex(pattern);
            if (string.IsNullOrEmpty(express))
            {
                return value;
            }
            if (reg.IsMatch(express))
            {
                var match = reg.Match(express);
                value = Convert.ToDecimal(match.Value.Replace(key, "").Replace("=", ""));
            }

            //等于==情况与<一起处理
            else if (key == "<" && express.Contains("=="))
            {
                reg = new Regex("==" + basepattern);
                var match = reg.Match(express);
                value = Convert.ToDecimal(match.Value.Replace(key, "").Replace("=", ""));
            }

            return value;
        }

    }
}
