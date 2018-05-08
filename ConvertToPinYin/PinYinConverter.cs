using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.International.Converters.PinYinConverter;

namespace ConvertToPinYin
{
    class PinYinConverter
    {
        #region 汉字转换为拼音
        #region 汉字转换为拼音、拼音首字母
        /// <summary>   
        /// 汉字转化为拼音  
        /// </summary>   
        /// <param name="str">汉字</param>   
        /// <returns>全拼</returns>   
        public static string GetPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                //用try catch来判断是否为汉字
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary>   
        /// 汉字转化为拼音首字母  
        /// </summary>   
        /// <param name="str">汉字</param>   
        /// <returns>首字母</returns>   
        public static string GetFirstPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary>
        /// 获得中文字符串的拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPinYin(string str)
        {
            string py = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (ChineseChar.IsValidChar(str[i]))//判断当前字符是不是汉字
                {
                    ChineseChar cc = new ChineseChar(str[i]);//构造方法             
                    py += cc.Pinyins[0].TrimEnd('1', '2', '3', '4', '5').ToLower();
                }
                else//不是汉字的话 加本身
                {
                    py += str[i];
                }

            }

            return py;
        }


        /// <summary>
        /// 获得中文字符串的拼音--返回多音字不同拼写的组合(多音字(拼写、读音))
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> GetPinYins(string str)
        {
            //string py = "";
            List<string> pinYins = new List<string>() { "" };
            for (int i = 0; i < str.Length; i++)
            {
                List<string> charPinYins = new List<string>();
                if (ChineseChar.IsValidChar(str[i]))//判断当前字符是不是汉字
                {
                    ChineseChar cc = new ChineseChar(str[i]);//构造方法             
                    //py += cc.Pinyins[0].TrimEnd('1', '2', '3', '4', '5').ToLower();

                    charPinYins = cc.Pinyins.Where(p => p != null).Select(p => p.TrimEnd('1', '2', '3', '4', '5').ToLower()).Distinct().ToList();
                }
                else//不是汉字的话 加本身
                {
                    //py += str[i];

                    charPinYins.Add(str[i].ToString());
                }
                pinYins = pinYins.Join(charPinYins, p => 1, pc => 1, (p, pc) => p + pc).ToList();
            }

            //return py;
            return pinYins;
        }

        /// <summary>
        /// 获得中文字符串的拼音首字母--返回多音字不同首字母拼写的组合(多音字(拼写、读音))
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> GetFirstPinYins(string str)
        {
            //string py = "";
            List<string> pinYins = new List<string>() { "" };
            for (int i = 0; i < str.Length; i++)
            {
                List<string> charPinYins = new List<string>();
                if (ChineseChar.IsValidChar(str[i]))//判断当前字符是不是汉字
                {
                    ChineseChar cc = new ChineseChar(str[i]);//构造方法             
                    //py += cc.Pinyins[0].TrimEnd('1', '2', '3', '4', '5').ToLower();

                    //charPinYins = cc.Pinyins.Where(p => p != null).Select(p => p.TrimEnd('1', '2', '3', '4', '5').ToLower()).Distinct().ToList();
                    charPinYins = cc.Pinyins.Where(p => p != null).Select(p => p[0].ToString().ToUpper()).Distinct().ToList();
                }
                else//不是汉字的话 加本身
                {
                    //py += str[i];

                    charPinYins.Add(str[i].ToString());
                }
                pinYins = pinYins.Join(charPinYins, p => 1, pc => 1, (p, pc) => p + pc).ToList();
            }

            //return py;
            return pinYins;
        }

        #endregion
        #endregion
    }
}
