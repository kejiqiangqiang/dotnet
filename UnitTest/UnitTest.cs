//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//using System.Collections.Generic;
//using System.IO;
//using EFOS.Data.Master.Model;
//using EFOS.PCApply.Business;
//using EFOS.PCApply.HighChart;
//using EFOS.Service.Master;
//using System.Configuration;
//using System.Data;

//namespace UnitTest
//{
//    [TestClass]
//    public class UnitTest
//    {
//        #region 测试用
//        /*
//        /// <summary>
//        /// 测试用来获取数据库连接字符串,测试通过即可注释
//        /// </summary>
//        /// <returns></returns>
//        public override string ReaderConnectionString()
//        {
//            string connectionString = "Data Source=192.168.3.11\\dev;timeout=210;Initial Catalog=EFOS.Community;User ID=sa;Password=111111";
//            return connectionString;
//        }
//        */
//        #endregion

//        #region WCF测试
//        [TestMethod]
//        public void GetAlarmInfoHistoryTest()
//        {
//            #region 实时数据WCF服务地址（此地址为WCF实时数据服务地址，此服务调用Redis（调用Redis服务地址由此服务实现指定），获取Redis服务内数据）
//            //DataServerWcfUrl = "http://192.168.3.11:9511/service/wcfaction "//测试时直接将WebKeyManager.Platform.DataServerWcfUrl替换
//            //List<ResultRealData> resultRealDatas = ApplyService.GetRealDataByDataKey(projectCode, pointCodes, "http://192.168.3.11:9511/service/wcfaction ");//测试用，不提交
//            #endregion

//            #region BLL内部WCF服务测试(跨数据库数据获取)
//            //测试BLL内部WCF服务(远程服务跨数据库)
//            string wcfUri = MasterPath.WcfPath;

//            //思考如何通过读取配置初始化
//            //<!--运营数据服务地址-->
//            //<add key="MasterWcfUri" value="http://192.168.3.11:8000/service/wcfaction" />

//            //读取配置文件
//            MasterPath.WcfPath = ConfigurationManager.AppSettings["MasterWcfUri"];

//            //强行赋值
//            //MasterPath.WcfPath = "http://192.168.3.11:8000/service/wcfaction";
//            //WCF远程服务跨数据库
//            List<Apply_DeviceType> deviceTypes = new ConfigService().GetDeviceType();

//            bool expected = true;
//            bool actual = deviceTypes.Count > 0;
//            #endregion

//            #region BLL测试
//            //AlarmInfoHistoryBLL Service = new AlarmInfoHistoryBLL();
//            //int count;
//            ////var result = Service.GetAlarmInfoHistory(2, false, null, null, Convert.ToDateTime("2016-10-18"), Convert.ToDateTime("2016-11-23"), null, 1, out count);            
//            //var result = Service.GetAlarmInfoHistory(2, false, null, null, null, null,null,1,out count);
//            //bool expected = true;
//            //bool actual = result != null ? true : false;
//            #endregion

//            Assert.AreEqual(expected, actual, "测试未通过");
//        }
//        #endregion

//        #region 能耗

//        [TestMethod]
//        public void GetCurrentEnergyTest()
//        {

//            MasterPath.WcfPath = ConfigurationManager.AppSettings["MasterWcfUri"];
//            //SQLConnectionString = "Data Source=192.168.3.11\\dev;timeout=210;Initial Catalog=EFOS.Community;User ID=sa;Password=111111",//重写可解决
//            //DataServerWcfUrl = "http://192.168.3.11:9511/service/wcfaction "//测试时直接将WebKeyManager.Platform.DataServerWcfUrl替换
//            //List<ResultRealData> resultRealDatas = ApplyService.GetRealDataByDataKey(projectCode, pointCodes, "http://192.168.3.11:9511/service/wcfaction ");//测试用，不提交
//            AirConditionEnergyManageBLL Service = new AirConditionEnergyManageBLL();
//            var result = Service.GetCurrentEnergy(2, 2, Convert.ToDateTime("2016-11-27"), TimeType.Year);
            
//        }
//        #endregion

//        #region 曲线图表+曲线数据结构测试
//        /// <summary>
//        /// 曲线图表+曲线数据结构测试
//        /// </summary>
//        [TestMethod]
//        public void GetAirConditionEnergyAnalysis()
//        {
//            MasterPath.WcfPath = ConfigurationManager.AppSettings["MasterWcfUri"];
//            AirConditionEnergyManageBLL Service = new AirConditionEnergyManageBLL();
//            var result = Service.GetTotalEnergyTrend(2,2,Convert.ToDateTime("2016-11-27"),TimeType.Day);

//        }
//        #endregion

//        #region 曲线表格数据
//        /// <summary>
//        /// 曲线表格数据
//        /// </summary>
//        [TestMethod]
//        public void GetAirConditionEnergyAnalysisData()
//        {
//            MasterPath.WcfPath = ConfigurationManager.AppSettings["MasterWcfUri"];
//            AirConditionEnergyManageBLL Service = new AirConditionEnergyManageBLL();
//            var result0 = Service.GetTotalEnergyTrendData(2, Convert.ToDateTime("2016-11-27"), TimeType.Day);
//            var result1 = Service.GetDeviceEnergyTrendData(2, Convert.ToDateTime("2016-11-27"), TimeType.Day);
//            var result2 = Service.GetItemEnergyPieData(2, Convert.ToDateTime("2016-11-27"), TimeType.Week);
//            //匿名对象
//            //var title = result2.GetType().GetProperty("title").GetValue(result2,null);
//            //var data = result2.GetType().GetProperty("data").GetValue(result2, null);

//            var cname0 = result0.Columns[0].ToString();
//            var cname = result0.Columns[0].ColumnName;
//            var pivotcolumn = result0.Columns[0].Table;

//            #region 行转列
//            //行转列
//            //列结构
//            DataTable srcTable = result0;
//            DataTable pivotTable = new DataTable();
//            //指定要作为ColumnName的那列索引columnHeadIndex
//            int columnHeadIndex = 0;
//            DataColumn columnHead = srcTable.Columns[columnHeadIndex];
//            //pivotTable第一列列名
//            pivotTable.Columns.Add(columnHead.ColumnName);
//            foreach (DataRow row in srcTable.Rows)
//            {
//                //每一行的columnHeadIndex列对应数据值依次作为pivotTable的列名
//                pivotTable.Columns.Add(row[columnHeadIndex].ToString());
//            }

//            //数据(srcTable列数即pivotTable行数)
//            foreach (DataColumn column in srcTable.Columns)
//            {
//                //跳过columnHeadIndex所指定的列(此列一般是表项，不是数据)
//                if (column == columnHead)
//                {
//                    continue;
//                }
//                //保存列数据作为pivotTable行数据
//                object[] newRow = new object[srcTable.Rows.Count+1];
//                newRow[0] = column.ColumnName;
//                for (int i = 0; i < srcTable.Rows.Count; i++)
//                {
//                    newRow[i + 1] = srcTable.Rows[i][column];
//                }
//                pivotTable.Rows.Add(newRow);//执行次数（srcTable列数即pivotTable行数）
//            }

//            #endregion

//            var str = Service.Export(2, Convert.ToDateTime("2016-11-27"), TimeType.Day);
//        }
//        #endregion

//        #region Web页面Session测试

//        #endregion

//        #region 房间参数
//        [TestMethod]
//        public void RoomParams()
//        {
//            MasterPath.WcfPath = ConfigurationManager.AppSettings["MasterWcfUri"];
//            Info_RoomParamsBLL service = new Info_RoomParamsBLL();
//            var projectDeviceTypes = service.GetDeviceType(94);

//            //var dataMappingDataItems = service.GetDataMappingDataItem(94);
//        }
//        #endregion
//    }
//}

