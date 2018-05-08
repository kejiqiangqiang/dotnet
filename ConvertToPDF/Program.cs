using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;


using System.Collections;
using System.Text.RegularExpressions;

//微软接口
//using Microsoft.Office12
//using Microsoft.Office.Core;
//using Microsoft.Office.Interop.Excel;
//Spire.XLS控件
using Spire.Pdf;
//using Spire.Xls;
//using Spire.Xls.Converter;
//using System.Data;
using System.Threading;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.VisualBasic.FileIO;
using iTextDemo;
using iTextPdf;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Web;
using System.Diagnostics;
using Spire.Pdf.HtmlConverter;
using iTextSharp.tool.xml;
using Microsoft.International.Converters.PinYinConverter;
using Newtonsoft.Json.Linq;
using WebApiHttpRequestTest;

namespace ConvertToPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 微软自带的excel to pdf
            #endregion

            #region excel to pdf
            //var strRes = string.Empty;
            //string source = "D:\\pdf.xlsx";
            //string target = "D:\\pdf.pdf";
            //// load Excel file
            //Workbook workBook = new Workbook();
            //workBook.LoadFromFile(source);

            //#region 设置pdf样式后不能识别excel中的图表
            ////// Set PDF template
            ////PdfDocument pdfDocument = new PdfDocument();
            ////pdfDocument.PageSettings.Orientation = PdfPageOrientation.Landscape;
            ////pdfDocument.PageSettings.Width = 842;
            ////pdfDocument.PageSettings.Height = 595;
            //////Convert Excel to PDF using the template above
            ////PdfConverter pdfConverter = new PdfConverter(workBook);
            ////PdfConverterSettings settings = new PdfConverterSettings();
            ////settings.TemplateDocument = pdfDocument;
            ////pdfDocument = pdfConverter.Convert(settings);
            ////// Save and preview PDF
            ////pdfDocument.SaveToFile(target);//设置pdf样式后不能识别excel中的图表
            //#endregion
            ////样式设置不生效
            ////workBook.SaveToFile(target, settings);

            ////直接可以（但不能满足样式）
            //workBook.SaveToFile(target, Spire.Xls.FileFormat.PDF);
            #endregion

            #region mutiply Series mutiply AxisY
            ////创建几个图形的对象
            //Series series1 = CreateSeries("员工人数", ViewType.Line, dt, 0);
            //Series series2 = CreateSeries("人均月薪", ViewType.Line, dt, 1);
            //Series series3 = CreateSeries("成本TEU", ViewType.Line, dt, 2);
            //Series series4 = CreateSeries("人均生产率", ViewType.Line, dt, 3);
            //Series series5 = CreateSeries("占2005年3月人数比例", ViewType.Line, dt, 4);


            //List<Series> list = new List<Series>() { series1, series2, series3, series4, series5 };
            //chartControl1.Series.AddRange(list.ToArray());
            //chartControl1.Legend.Visible = false;
            //chartControl1.SeriesTemplate.LabelsVisibility = DefaultBoolean.True;

            //for (int i = 0; i < list.Count; i++)
            //{
            //    list[i].View.Color = colorList[i];

            //    CreateAxisY(list[i]);
            //}
            #endregion

            #region Spire.Pdf html to pdf
            ////Create a pdf document.
            //Spire.Pdf.PdfDocument pdfDocument = new Spire.Pdf.PdfDocument();
            //pdfDocument.PageSettings.Width = 842;
            //pdfDocument.PageSettings.Height = 595;
            ////String url = "http://world.huanqiu.com/article/2017-04/10429665.html";
            ////String url = "http://192.168.3.179:992/community/project.html?projectCode=7#!operateAnalysis?type=nav$efos_pc%2F0.1.0%2Fcommunity%2Fproject%2Fmodule%2F$101";
            //String url = @"F:\VIP简报切图\新建文本文档1.html";
            //FileStream fs = new FileStream(url,FileMode.Open);
            //StreamReader sr = new StreamReader(fs,Encoding.Default);
            //string html = sr.ReadToEnd();// 此时html代码中路径必须为绝对路径（如背景图片）
            //var pdfhtmllayout = new PdfHtmlLayoutFormat();
            //var pdfpagesetting = new PdfPageSettings();
            //Thread thread = new Thread(() =>
            //{ 
            //    //pdfDocument.LoadFromHTML(url,false, true, true); 
            //    pdfDocument.LoadFromHTML(html, true, pdfpagesetting, pdfhtmllayout); 
            //});
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
            //thread.Join();
            ////Save pdf file.
            //pdfDocument.SaveToFile("F:\\VIP简报切图\\viphtml.pdf",FileFormat.PDF);
            //pdfDocument.Close();
            ////Launching the Pdf file.
            ////System.Diagnostics.Process.Start("D:\\html.pdf");

            #endregion

            #region iText html to pdf
            ///* 1、两个流关闭顺序错误导致生成的pdf不完整，无法打卡pdf
            // * 2、html代码中路径必须为绝对路径（如背景图片）
            // * 3、支持的html样式有限
            // * 
            // */
            //// Create the PDF  
            //var path = "F:\\Demo\\itextsharp\\viphtmlitextsharp.pdf";
            //var doc = new Document();//PageSize.A4
            //var writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            ////打开文档
            //doc.Open();
            ////解析子路径获取该页面的html
            //String url = @"F:\Demo\itextsharp\vipbrieftemplate.html";
            //FileStream fs = new FileStream(url, FileMode.Open);
            //StreamReader sr = new StreamReader(fs, Encoding.Default);
            //string htmlDoc = sr.ReadToEnd();// 此时html代码中路径必须为绝对路径（如背景图片）

            ////byte[] array = System.Text.Encoding.UTF8.GetBytes(htmlDoc);
            //byte[] array = System.Text.Encoding.Default.GetBytes(htmlDoc);

            //MemoryStream stream = new MemoryStream(array);

            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, stream, (Stream)null, System.Text.Encoding.Default, new SongFontFactory());

            //////指定文件预设开档时的缩放为100%
            ////PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //////将pdfDest设定的资料写到PDF档
            ////PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            ////writer.SetOpenAction(action);

            ////两个流关闭顺序错误导致生成的pdf不完整，无法打卡pdf
            //doc.Close();
            //stream.Close();
            //sr.Close();
            //fs.Close();

            #endregion

            #region iTextSharp.text.pdf 画pdf表格table及图表chart
            ////ReportDemo demo = new ReportDemo();
            ////demo.GeneratePDFReport();
            //string path = string.Format("../../Pdf/{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
            //PdfReport pdfReport = new PdfReport(path);
            ////内部类访问必须带着父类类名，尽管该内部类是public的
            //iTextPdf.PdfReport.Axis axis = new PdfReport.Axis();
            //List<iTextPdf.PdfReport.Series> series = new List<PdfReport.Series>();

            ////类中引用类类型字段仅保存该字段类的引用
            //axis.xAxis = new PdfReport.xAxis() { Scale = 4, ScaleCount = 5, Title = "时间" };
            ////axis.xAxis.Scale = 1;
            ////axis.xAxis.ScaleCount = 23;
            ////axis.xAxis.Title = "时间";
            //axis.yAxises = new List<PdfReport.yAxis>();

            //axis.yAxises.Add(new iTextPdf.PdfReport.yAxis() { Scale = 200, ScaleCount = 5, SeriesNum = 0, BaseColor = BaseColor.BLACK, Title = "温度", MaxValue = 500, Unit = "℃" });
            //axis.yAxises.Add(new iTextPdf.PdfReport.yAxis() { Scale = 0.25f, ScaleCount = 4, SeriesNum = 1, BaseColor = BaseColor.BLACK, Title = "负载率", MaxValue = 0.8f });

            //series.Add(new iTextPdf.PdfReport.Series()
            //{
            //    Title = "A相",
            //    SeriesNum = 0,
            //    BaseColor = BaseColor.BLUE,
            //    data = new PdfReport.Points()
            //    {
            //        xdata = new List<int>() { 0, 4, 8, 12, 16, 20 },
            //        ydata = new List<float>() { 500, 600, 500, 600, 500, 490 }
            //    }
            //});

            //series.Add(new iTextPdf.PdfReport.Series()
            //{
            //    Title = "B相",
            //    SeriesNum = 0,
            //    BaseColor = BaseColor.CYAN,
            //    data = new PdfReport.Points()
            //    {
            //        xdata = new List<int>() { 0, 4, 8, 12, 16, 20 },
            //        ydata = new List<float>() { 100, 200, 100, 200, 100, 200 }
            //    }
            //});

            //series.Add(new iTextPdf.PdfReport.Series()
            //{
            //    Title = "C相",
            //    SeriesNum = 0,
            //    BaseColor = BaseColor.GRAY,
            //    data = new PdfReport.Points()
            //    {
            //        xdata = new List<int>() { 0, 4, 8, 12, 16, 20 },
            //        ydata = new List<float>() { 200, 300, 200, 300, 200, 300 }
            //    }
            //});



            //series.Add(new iTextPdf.PdfReport.Series()
            //{
            //    Title = "负载率",
            //    SeriesNum = 1,
            //    BaseColor = BaseColor.RED,
            //    data = new PdfReport.Points()
            //    {
            //        xdata = new List<int>() { 0, 4, 8, 12, 16, 20 },
            //        ydata = new List<float>() { 0.25f, 0.5f, 0.75f, 0.8f, 0.9f, 0.5f }
            //    }
            //});

            //List<iTextPdf.PdfReport.ChartData> chartDatas = new List<PdfReport.ChartData>();
            //for (int i = 0; i < 3; i++)
            //{
            //    chartDatas.Add(new PdfReport.ChartData() { DeviceName = i + "#设备", Axis = axis, Series = series, DeviceType = 1, DeviceTypeName = "变压器" });
            //}
            //for (int i = 3; i < 5; i++)
            //{
            //    chartDatas.Add(new PdfReport.ChartData() { DeviceName = i + "#设备", Axis = axis, Series = series, DeviceType = 2, DeviceTypeName = "配电柜" });
            //}
            //for (int i = 5; i < 8; i++)
            //{
            //    chartDatas.Add(new PdfReport.ChartData() { DeviceName = i + "#设备", Axis = axis, Series = series, DeviceType = 3, DeviceTypeName = "低压开关柜" });
            //}

            //var head = pdfReport.GetHeadPdfTable("../../Pdf/logo巡检记录表.png", "联合广场");
            //var nav = pdfReport.GetNavPdfTable("1#配电房", chartDatas);
            //var charts = pdfReport.GetChartTablePdfTable(chartDatas);
            ////pdfReport.GeneratePDFReport(head, nav, charts);

            ////文件流上传到服务器

            //#region //1、iTextPdf生成的PDF流，保存时PdfWriter有特殊处理，文件末尾有EOF，但是将PDF流直接用FileStream接收文件字节流并保存为pdf文件，则无法打开此文件，此时文件末尾无EOF
            //byte[] buffer = pdfReport.GeneratePDFReportByte(head, nav, charts);
            //UploadToServer(buffer, string.Format("/InspectReportPdfByte/94/"), string.Format("2_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss")));
            //#endregion

            //#region //2、直接上传一个PDF文件(pdf文件已存在)的字节流到服务器，能正常打开文件
            //////先生成文件
            ////pdfReport.GeneratePDFReport(head, nav, charts);
            //////再读取文件流
            ////FileStream fs = new FileStream(path, FileMode.Open);
            ////byte[] bts = new byte[fs.Length];
            ////fs.Read(bts, 0, bts.Length);
            ////UploadToServer(bts, string.Format("/InspectReportPdfByte/94/"), string.Format("2_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss")));
            //#endregion

            //#region 最终确定是，文件不完整，原因有2种
            ////1、流的关闭顺序
            ////2、DocWriter.CloseStream = false;--默认会在关闭document时关闭Stream，
            //#endregion

            #endregion

            #region 批量文件压缩打包下载
            //var path = string.Format("../../Pdf/");
            //var filePaths = Directory.GetFiles(path);

            //string fullPath = Path.GetFullPath(path);

            //string currentDirectory = Directory.GetCurrentDirectory();
            //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            //var zipFilePath = fullPath + "projectName"+"巡检报告.zip";
            //DownLoad(filePaths, zipFilePath);
            #endregion
        }

        #region 重写字体
        /// <summary>
        /// 重写iTextSharp字体(仅仅使用宋体)
        /// </summary>
        public class SongFontFactory : IFontProvider
        {
            public Font GetFont(String fontname, String encoding, Boolean embedded, float size, int style, BaseColor color)
            {

                BaseFont bf3 = BaseFont.CreateFont(@"c:\windows\fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fontContent = new Font(bf3, size, style, color);
                return fontContent;

            }
            public Boolean IsRegistered(String fontname)
            {
                return false;
            }
        }
        #endregion

        #region mutiply Series mutiply AxisY
        /// <summary>
        /// 准备数据内容
        /// </summary>
        /// <returns></returns>
        private DataTable CreateData()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(new DataColumn("类型"));
            dt.Columns.Add(new DataColumn("2005-1月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-2月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-3月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-4月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-5月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-6月", typeof(decimal)));

            dt.Rows.Add(new object[] { "员工人数", 437, 437, 414, 397, 387, 378 });
            dt.Rows.Add(new object[] { "人均月薪", 3964, 3961, 3979, 3974, 3967, 3972 });
            dt.Rows.Add(new object[] { "成本TEU", 3104, 1339, 3595.8, 3154.5, 2499.8, 3026 });
            dt.Rows.Add(new object[] { "人均生产率", 7.1, 3.06, 8.69, 7.95, 6.46, 8.01 });
            dt.Rows.Add(new object[] { "占2005年3月人数比例", 1.06, 1.06, 1, 0.96, 0.93, 0.91 });

            return dt;
        }

        ///// <summary>
        ///// 根据数据创建一个图形展现
        ///// </summary>
        ///// <param name="caption">图形标题</param>
        ///// <param name="viewType">图形类型</param>
        ///// <param name="dt">数据DataTable</param>
        ///// <param name="rowIndex">图形数据的行序号</param>
        ///// <returns></returns>
        //private Series CreateSeries(string caption, ViewType viewType, DataTable dt, int rowIndex)
        //{
        //    Series series = new Series(caption, viewType);
        //    for (int i = 1; i < dt.Columns.Count; i++)
        //    {
        //        string argument = dt.Columns[i].ColumnName;//参数名称
        //        decimal value = (decimal)dt.Rows[rowIndex][i];//参数值
        //        series.Points.Add(new SeriesPoint(argument, value));
        //    }

        //    //必须设置ArgumentScaleType的类型，否则显示会转换为日期格式，导致不是希望的格式显示
        //    //也就是说，显示字符串的参数，必须设置类型为ScaleType.Qualitative
        //    series.ArgumentScaleType = ScaleType.Qualitative;
        //    series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;//显示标注标签

        //    return series;
        //}

        ///// <summary>
        ///// 创建图表的第二坐标系
        ///// </summary>
        ///// <param name="series">Series对象</param>
        ///// <returns></returns>
        //private SecondaryAxisY CreateAxisY(Series series)
        //{
        //    SecondaryAxisY myAxis = new SecondaryAxisY(series.Name);
        //    ((XYDiagram)chartControl1.Diagram).SecondaryAxesY.Add(myAxis);
        //    ((LineSeriesView)series.View).AxisY = myAxis;
        //    myAxis.Title.Text = series.Name;
        //    myAxis.Title.Alignment = StringAlignment.Far; //顶部对齐
        //    myAxis.Title.Visible = true; //显示标题
        //    myAxis.Title.Font = new Font("宋体", 9.0f);

        //    Color color = series.View.Color;//设置坐标的颜色和图表线条颜色一致

        //    myAxis.Title.TextColor = color;
        //    myAxis.Label.TextColor = color;
        //    myAxis.Color = color;

        //    return myAxis;
        //}


        #endregion

        #region 批量文件压缩打包下载
        /// <summary>
        /// 批量文件压缩打包下载
        /// </summary>
        /// <param name="files">文件绝对路径（物理路径）</param>
        ///  <param name="zipFilePath">保存压缩文件绝对路径</param>
        public static void DownLoad(string[] files, string zipFilePath)
        {

            MemoryStream ms = new MemoryStream();
            byte[] buffer = null;

            using (ZipFile zipFile = ZipFile.Create(ms))
            {
                zipFile.BeginUpdate();
                zipFile.NameTransform = new MyNameTransfom();
                foreach (var file in files)
                {
                    if (File.Exists(file))
                        zipFile.Add(file);

                }
                zipFile.CommitUpdate();
                buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);   //读取文件内容(1次读ms.Length/1024M)  
                ms.Flush();
                ms.Close();
            }

            FileStream fileStream = new FileStream(zipFilePath, FileMode.Create);
            fileStream.Write(buffer, 0, buffer.Length);

        }

        /// <summary>
        /// 处理生成的文件夹内容，否则会把生成的pdf目录也压缩进去
        /// </summary>
        public class MyNameTransfom : ICSharpCode.SharpZipLib.Core.INameTransform
        {

            #region INameTransform 成员

            public string TransformDirectory(string name)
            {
                return null;
            }

            public string TransformFile(string name)
            {
                return Path.GetFileName(name);
            }

            #endregion
        }

        #endregion

        #region 文件流上传到服务器
        /// <summary>
        /// 上传文件流到远程服务器
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool UploadToServer(byte[] buffer, string fileDir, string fileName)
        {
            //移至Mater，方便同时调试
            //string path = "http://localhost:8765" + "/app.ashx?Name=EFOS.APPApply.Business.InspectReportBLL.UploadPdfFromStringBuffer";
            //string path = "http://localhost:8300" + "/app.ashx?Name=EFOS.Master.Business.SimulateHttpRequestPostCross_DomainUploadFile.UploadPdfFromStringBuffer";
            string path = "http://localhost:8300" + "/app.ashx?Name=EFOS.Master.Business.SimulateHttpRequestPostCross_DomainUploadFile.UploadPdfFromByteBuffer";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            //byte经Base64转换为字符串
            string stringBuffer = Convert.ToBase64String(buffer);
            //经过Convert.ToBase64String()转换过的参数返回的字符串中，经过request post后发现，+都变成了空格
            stringBuffer = stringBuffer.Replace("+", "%2B");//先将+编码为%2B//在服务端使用字符串前将%2B替换为+替换回来即可
            ////Load方法Request内Post参数参数的处理编码url编码转换ContentType("application/x-www-form-urlencoded")
            parameters.Add("buffer", buffer);
            //parameters.Add("stringBuffer", stringBuffer);
            parameters.Add("fileDir", fileDir);
            parameters.Add("fileName", fileName);
            //--var result = WebapiHttpRequest0.Load<bool>(path, parameters);//Load方法Request内Post参数参数的处理编码为ContentType("application/x-www-form-urlencoded")
            var result = true;
            //文件流成功上传，但是文件无法打开，缺少EOF？
            return result;
        }
        #endregion

    }
}
