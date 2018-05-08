using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            double expected = 0.111111, actual = 0.111110;
            Assert.AreEqual(expected, actual, 0.001, "test result:false");
        }
        [TestMethod]
        public void TestMethod2()
        {
            //年从1月1号起
            //string param = "2016";
            string param = "2016-1-1";
            DateTime dateTime = Convert.ToDateTime(param);
            bool expected = true;
            bool actual = dateTime == Convert.ToDateTime(DateTime.Now.AddMonths(-10).AddDays(-23).ToString("yyyy-MM-dd"));//2016-11-24-->2016-1-1
            Assert.AreEqual(expected, actual, "测试通过");
        }


        /// <summary>
        /// decimal相加,不能与null相加
        /// </summary>
        [TestMethod]
        public void DecimalTest()
        {
            decimal? d = null;
            d += decimal.Parse("11.29");
            var result = d;//结果为null
        }

    }
}
