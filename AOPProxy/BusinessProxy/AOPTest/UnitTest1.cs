using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AOPTest.Business;

namespace AOPTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var bll = new CustomerInfoBLL();
            var rs=bll.GetModel(1);
            Assert.IsNotNull(rs,"失败");
        }
    }
}
