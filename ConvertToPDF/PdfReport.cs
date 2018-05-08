using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
using System.Data;


namespace iTextPdf
{
    public class PdfReport
    {

        BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        string path;//= string.Format("../../Pdf/{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
        public PdfReport(string path)
        {
            this.path = path;
        }

        //生成巡检报告pdf
        public byte[] GeneratePDFReportByte(PdfPTable headPdfTable, PdfPTable navPdfTable, List<PdfPTable> chartPdfTables)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer;
            //pdf文件
            Document document = new Document(PageSize.A4.Rotate());//横向A4
            //pdf尺寸样式
            //document.SetPageSize(new Rectangle(842 ,595));
            document.SetMargins(0, 0, 0, 0);
            //pdf表格
            PdfPTable table = new PdfPTable(1);//PdfPTable table = new PdfPTable(4);
            //pdf表格样式
            table.DefaultCell.Border = PdfPCell.NO_BORDER;
            table.WidthPercentage = 100f;
            table.SetWidths(new int[] { 1 });//table.SetWidths(new int[] { 1, 1, 1, 1 });
            table.SplitLate = true;//pdf分页

            //单元格元素加入到pdf表格
            PdfPCell style = new PdfPCell();
            style.Border = PdfPCell.NO_BORDER;
            PdfPCell headPdfCell = new PdfPCell(headPdfTable, style);
            PdfPCell navPdfCell = new PdfPCell(navPdfTable, style);
            //PdfPCell chartPdfCell = new PdfPCell(chartPdfTable, style);
            table.AddCell(headPdfTable);
            table.AddCell(navPdfTable);
            foreach (var chartPdfTable in chartPdfTables)
            {
                PdfPCell chartPdfCell = new PdfPCell(chartPdfTable, style);
                table.AddCell(chartPdfCell);
            }

            //using (document)
            //{
            //    //将文件写入流中（内存中）
            //    PdfWriter writer = PdfWriter.GetInstance(document, ms);

            //    //打开pdf文件
            //    document.Open();
            //    //pdf表格加入到pdf文件
            //    document.Add(table);
            

            //    //字节流写入到文件并关闭文件
            //    //document.Close();//using(document){todo}自动关闭

            //    //1、using (document)关闭，ms也被关闭--//但是关闭了document，ms也被关闭了，这是由于writer默认会在关闭document时关闭Stream，设置writer.CloseStream = false;
            //    ////获取字节流
            //    //buffer = new byte[ms.Length];
            //    //ms.Position = 0;
            //    //ms.Read(buffer, 0, buffer.Length);//读取文件内容(1次读ms.Length/1024M)  
            //    //ms.Flush();
            //    //ms.Close();//将document写入到ms，并且是document关闭时才完全写完，即在document关闭之前会用到ms，因此必须后关闭ms--//但是关闭了document，ms也被关闭了，这是由于writer默认会在关闭document时关闭Stream，设置writer.CloseStream = false;
            //}

            //1、using (document)关闭，ms也被关闭--不用using，手动关闭document
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            writer.CloseStream = false;
            document.Open();
            document.Add(table);
            document.Close();//必须先关闭，即先将document完全写入到ms，才能保证ms的完整，从而保证buffer完整--//但是关闭了document，ms也被关闭了，这是由于writer默认会在关闭document时关闭Stream，设置writer.CloseStream = false;
            //1、using (document)关闭，ms也被关闭--//但是关闭了document，ms也被关闭了，这是由于writer默认会在关闭document时关闭Stream，设置writer.CloseStream = false;
            //获取字节流
            buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);//读取文件内容(1次读ms.Length/1024M)  
            ms.Flush();
            //document.Close();//并且必须先将document完全写入到ms才能关闭，佛则也导致ms不完整
            ms.Close();//两个流关闭顺序错误导致生成的pdf不完整，无法打卡pdf--同VIPBrief中，将html转换为pdf

            return buffer;

        }

        //生成巡检报告pdf
        public void GeneratePDFReport(PdfPTable headPdfTable, PdfPTable navPdfTable, List<PdfPTable> chartPdfTables)
        {
            //pdf文件
            Document document = new Document(PageSize.A4.Rotate());//横向A4
            //pdf尺寸样式
            //document.SetPageSize(new Rectangle(842 ,595));
            document.SetMargins(0, 0, 0, 0);
            //pdf表格
            PdfPTable table = new PdfPTable(1);//PdfPTable table = new PdfPTable(4);
            //pdf表格样式
            table.DefaultCell.Border = PdfPCell.NO_BORDER;
            table.WidthPercentage = 100f;
            table.SetWidths(new int[] { 1 });//table.SetWidths(new int[] { 1, 1, 1, 1 });
            table.SplitLate = true;//pdf分页

            //单元格元素加入到pdf表格
            PdfPCell style = new PdfPCell();
            style.Border = PdfPCell.NO_BORDER;
            PdfPCell headPdfCell = new PdfPCell(headPdfTable, style);
            PdfPCell navPdfCell = new PdfPCell(navPdfTable, style);
            //PdfPCell chartPdfCell = new PdfPCell(chartPdfTable, style);
            table.AddCell(headPdfTable);
            table.AddCell(navPdfTable);
            foreach (var chartPdfTable in chartPdfTables)
            {
                PdfPCell chartPdfCell = new PdfPCell(chartPdfTable, style);
                table.AddCell(chartPdfCell);
            }

            using (document)
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path,
                 FileMode.Create));

                //打开pdf文件
                document.Open();
                //pdf表格加入到pdf文件
                document.Add(table);
                
                //字节流写入到文件并关闭文件
                //document.Close();//using(document){todo}自动关闭
            }

        }

        //头部及概览2
        public PdfPTable GetHeadPdfTable2(string projectCode,string roomName, string imgPath, List<ChartData> chartDatas)
        {
            PdfPTable headTable = new PdfPTable(5);
            //headTable.SpacingBefore = 20f;
            headTable.WidthPercentage = 100f;
            headTable.SetWidths(new int[] { 1, 1, 1, 1, 1 });
            //cell样式优先级更高
            //headTable.DefaultCell.Border = PdfPCell.SUBJECT;
            PdfPCell cell;
            //图片
            Image image = Image.GetInstance(imgPath);
            //image.SetAbsolutePosition(,);
            cell = new PdfPCell(image, true);
            cell.MinimumHeight = 20f;
            cell.Border = PdfPCell.SUBJECT;
            cell.BorderColor = BaseColor.BLUE;
            cell.BorderWidth = 2f;
            headTable.AddCell(cell);

            //空白
            cell = new PdfPCell();
            cell.Border = PdfPCell.SUBJECT;
            cell.BorderColor = BaseColor.BLUE;
            cell.BorderWidth = 2f;
            headTable.AddCell(cell);

            //标题
            cell = new PdfPCell(new Phrase(projectCode, new Font(BF_Light, 20, Font.BOLD, BaseColor.BLACK)));
            cell.Border = PdfPCell.SUBJECT;
            cell.BorderColor = BaseColor.BLUE;
            cell.BorderWidth = 2f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headTable.AddCell(cell);

            //空白
            cell = new PdfPCell();
            cell.Border = PdfPCell.SUBJECT;
            cell.BorderColor = BaseColor.BLUE;
            cell.BorderWidth = 2f;
            headTable.AddCell(cell);
            //空白
            headTable.AddCell(cell);

            return headTable;
        }

        //pdf头部
        public PdfPTable GetHeadPdfTable(string imgPath, string projectName)
        {
            PdfPTable headTable = new PdfPTable(5);
            //headTable.SpacingBefore = 20f;
            headTable.WidthPercentage = 100f;
            headTable.SetWidths(new int[]{1,1,1,1,1});
            //cell样式优先级更高
            //headTable.DefaultCell.Border = PdfPCell.SUBJECT;
            PdfPCell cell;
            //图片
            Image image = Image.GetInstance(imgPath);
            //image.SetAbsolutePosition(,);
            cell = new PdfPCell(image, true);
            cell.MinimumHeight = 20f;
            cell.Border = PdfPCell.SUBJECT;
            cell.BorderColor = new BaseColor(0, 159, 232);
            cell.BorderWidth = 2f;
            headTable.AddCell(cell);

            //空白
            cell = new PdfPCell();
            cell.Border = PdfPCell.SUBJECT;
            cell.BorderColor = new BaseColor(0, 159, 232);
            cell.BorderWidth = 2f;
            headTable.AddCell(cell);

            //标题
            cell = new PdfPCell(new Phrase(projectName,new Font(BF_Light,20,Font.BOLD,BaseColor.BLACK)));
            cell.Border = PdfPCell.SUBJECT;
            cell.BorderColor = new BaseColor(0, 159, 232);
            cell.BorderWidth = 2f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headTable.AddCell(cell);

            //空白
            cell = new PdfPCell();
            cell.Border = PdfPCell.SUBJECT;
            cell.BorderColor = new BaseColor(0, 159, 232);
            cell.BorderWidth = 2f;
            headTable.AddCell(cell);
            //空白
            headTable.AddCell(cell);

            return headTable;
        }

        //pdf房间设备类型告警概览
        public PdfPTable GetNavPdfTable(string roomName, List<ChartData> chartDatas)
        {
            PdfPTable navPdfTable = new PdfPTable(1);
            navPdfTable.WidthPercentage = 100f;
            //房间标题
            PdfPTable titlePdfTable = new PdfPTable(5);
            titlePdfTable.SetWidths(new int[] { 3, 4, 1, 1, 1 });
            
            //设备类型概览
            var deviceTypes = chartDatas.Select(p => new { p.DeviceType, p.DeviceTypeName }).Distinct();
            PdfPTable devTypesPdfTable = new PdfPTable(deviceTypes.Count());
            //某一设备类型告警信息
            PdfPTable devTypeInfoPdfTable = new PdfPTable(1);
            //空单元格
            PdfPCell spaceCell = new PdfPCell();
            spaceCell.Border = PdfPCell.NO_BORDER;
            //临时cell
            PdfPCell cell = new PdfPCell();

            cell = new PdfPCell(new Phrase(DateTime.UtcNow.ToString("yyyy年MM月dd日"),new Font(BF_Light,10,Font.ITALIC)));
            cell.Border = PdfPCell.NO_BORDER;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;
            titlePdfTable.AddCell(cell);
            //titlePdfTable.AddCell(spaceCell);
            //titlePdfTable.AddCell(spaceCell);

            cell = new PdfPCell(new Phrase(roomName + "设备巡检记录表", new Font(BF_Light, 12, Font.BOLD)));
            cell.Border = PdfPCell.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            titlePdfTable.AddCell(cell);

            //titlePdfTable.AddCell(spaceCell);

            cell = new PdfPCell(new Phrase("运行员工：", new Font(BF_Light, 6, Font.NORMAL)));
            cell.Border = PdfPCell.NO_BORDER;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;

            titlePdfTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("运行主管：", new Font(BF_Light, 6, Font.NORMAL)));
            cell.Border = PdfPCell.NO_BORDER;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;

            titlePdfTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("管理处：", new Font(BF_Light, 6, Font.NORMAL)));
            cell.Border = PdfPCell.NO_BORDER;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;

            titlePdfTable.AddCell(cell);

            //房间标题
            cell = new PdfPCell(titlePdfTable);
            //cell.MinimumHeight = 20;
            cell.Border = PdfPCell.SUBJECT;
            navPdfTable.AddCell(cell);

            //设备类型概览及告警信息
            int i = 0;
            foreach (var devType in deviceTypes)
            {
                //每次都new，引用常见bug
                //某一设备类型告警信息
                devTypeInfoPdfTable = new PdfPTable(1);

                //某一设备类型设备数据
                var deviceDatas = chartDatas.Where(p => p.DeviceType == devType.DeviceType);

                //某一设备类型名称
                cell = new PdfPCell(new Phrase(devType.DeviceTypeName,new Font(BF_Light,8,Font.NORMAL)));
                cell.Border = PdfPCell.NO_BORDER;
                devTypeInfoPdfTable.AddCell(cell);

                //某设备类型设备台数
                cell = new PdfPCell(new Phrase(deviceDatas.Count().ToString()+"台", new Font(BF_Light, 8, Font.NORMAL)));
                cell.Border = PdfPCell.NO_BORDER;
                devTypeInfoPdfTable.AddCell(cell);

                //设备告警信息
                foreach (var devData in deviceDatas)
                {
                    var deviceName = devData.DeviceName;
                    var axis = devData.Axis;
                    var series = devData.Series;

                    var alarmInfo = deviceName+"";
                    var textColor = BaseColor.BLACK;
                    foreach (var yAxis in axis.yAxises)
                    {
                        var curYseries = series.Where(p => p.SeriesNum == yAxis.SeriesNum).ToList();
                        if (curYseries == null || curYseries.Count <= 0) { continue; }
                        var maxValue = curYseries.Max(p => p.data.ydata.Max(pp => pp));
                        var minValue = curYseries.Min(p => p.data.ydata.Max(pp => pp));
                        if (yAxis.MaxValue.HasValue && maxValue > yAxis.MaxValue.Value || yAxis.MinValue.HasValue && minValue < yAxis.MinValue.Value)
                        {
                            textColor = BaseColor.RED;
                            if (yAxis.MaxValue.HasValue && maxValue > yAxis.MaxValue.Value)
                            {
                                alarmInfo += yAxis.Title + "过高/";
                            }
                            if (yAxis.MinValue.HasValue && minValue < yAxis.MinValue.Value)
                            {
                                alarmInfo += yAxis.Title + "过低/";
                            }

                        }

                    }
                    alarmInfo = alarmInfo.TrimEnd('/');
                    if (textColor == BaseColor.BLACK) { alarmInfo = ""; }

                    cell = new PdfPCell(new Phrase(alarmInfo, new Font(BF_Light, 6, Font.NORMAL, textColor)));
                    cell.Border = PdfPCell.NO_BORDER;
                    devTypeInfoPdfTable.AddCell(cell);


                }

                //bug--devTypeInfoPdfTable叠加了
                //PdfPCell构造函数与devTypeInfoPdfTable引用导致叠加（devTypeInfoPdfTable只new了一次，故每次都引用的同一内存）
                //解决--每次都new
                cell = new PdfPCell(devTypeInfoPdfTable);
                //cell.Border = PdfPCell.NO_BORDER;
                cell.Border = PdfPCell.LEFT_BORDER;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                //分割线
                if (i == 0) { 
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.PaddingLeft = 4f;
                }
                i++;
                devTypesPdfTable.AddCell(cell);

            }

            cell = new PdfPCell(devTypesPdfTable);
            cell.Border = PdfPCell.NO_BORDER;
            cell.PaddingTop = 3f;
            navPdfTable.AddCell(cell);

            return navPdfTable;
        }

        //pdf设备数据图表列表
        public List<PdfPTable> GetChartTablePdfTable(List<ChartData> chartDatas)
        {
            //以行为单元，方便分页
            List<PdfPTable> chartPdfTables = new List<PdfPTable>();

            //pdfpcell必须为pdfptable列的整数倍，否则最后一行无法显示出
            PdfPTable chartPdfTable = new PdfPTable(4);
            chartPdfTable.DefaultCell.Border = PdfPCell.NO_BORDER;
            chartPdfTable.SplitLate = true;
            PdfPCell chartTablePdfTable = new PdfPCell();
            chartTablePdfTable.Border = PdfPCell.NO_BORDER;
            PdfPCell style = new PdfPCell();
            style.Border = PdfPCell.NO_BORDER;
            style.PaddingTop = 25f;
            chartTablePdfTable.PaddingRight = 5f;

            //曲线坐标轴范围及刻度的自适应及提取设备报警配置临界标准值
            chartDatas.ForEach(p =>
            {
                p.Axis.yAxises.ForEach(pp =>
                {
                    //曲线坐标轴范围及刻度的自适应
                    var max = p.Series.Where(ppp => ppp.SeriesNum == pp.SeriesNum).Max(ppp => ppp.data.ydata.Max());
                    //报警配置提取的标准值与曲线数据值综合最大（否则标准线可能超出）
                    var finalMax = pp.MaxValue.HasValue ? pp.MaxValue > max ? pp.MaxValue : max : max;
                    dynamic d = Standard(Convert.ToDouble(finalMax), 0, 4);//pp.ScaleCount=4
                    if (d != null)
                    {
                        pp.Scale = Convert.ToSingle(d.tmpstep);
                        pp.ScaleCount = d.cornumber;
                    }
                    //提取设备报警配置临界标准值(放这里需要DeviceID、DataCode、alarmConfig)
                    //已提出去

                });

            });

            //pdfpcell必须为pdfptable列的整数倍，否则最后一行无法显示出
            for (int i = 0; i < ((chartDatas.Count - 1) / 4 + 1) * 4; i++)
            {
                

                if (i < chartDatas.Count)
                {
                    chartTablePdfTable = new PdfPCell(this.GetChartPdfTable(chartDatas[i].DeviceName, chartDatas[i].Axis, chartDatas[i].Series),style);
                }
                else 
                {
                    chartTablePdfTable = new PdfPCell();
                    chartTablePdfTable.Border = PdfPCell.NO_BORDER;
                    
                    //chartTablePdfTable.PaddingTop = 30f;
                    //chartTablePdfTable.PaddingRight = 20f;

                }
                if (i % 4 == 0)
                {
                    chartTablePdfTable.PaddingLeft = 5f;
                }
                chartTablePdfTable.PaddingTop = 25f;
                chartTablePdfTable.PaddingRight = 5f;
                chartPdfTable.AddCell(chartTablePdfTable);

                //一行为单元，方便分页
                if ((i + 1) % 4 == 0)
                {
                    chartPdfTables.Add(chartPdfTable);
                    chartPdfTable = new PdfPTable(4);
                    chartPdfTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                }
            }
            return chartPdfTables;
        }

        
        //单个pdf图表区生成
        public PdfPTable GetChartPdfTable(string device,Axis axis, List<Series> series)
        {
            ////pdfpcell必须为pdfptable列的整数倍，否则最后一行无法显示出
            //PdfPTable chartPdfTable = new PdfPTable(4);
            //chartPdfTable.DefaultCell.Border = PdfPCell.NO_BORDER;

            

            PdfPTable chartTablePdfTable;
            chartTablePdfTable = new PdfPTable(1);
            //chartTablePdfTable.DefaultCell.Border = PdfPCell.NO_BORDER;
            chartTablePdfTable.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY; 
            //图表
            PdfPCell chartCell;
            
            //名称、标题
            PdfPTable chartTitlePdfTable;
            PdfPCell chartTitlePdfTableCell;
            PdfPCell chartTitlePdfCell;

            ////pdfpcell必须为pdfptable列的整数倍，否则最后一行无法显示出
            //for (int i = 0; i < ((4 - 1) / 4 + 1) * 4; i++) { }
                //只要有一个y轴上的曲线有越线即报警且仅报一次
                var alarmInfo = "";
                var textColor = BaseColor.GREEN;
                foreach (var yAxis in axis.yAxises)
	            {
                    var curYseries = series.Where(p => p.SeriesNum == yAxis.SeriesNum).ToList();
                    if (curYseries == null || curYseries.Count <= 0) { continue; }
                    var maxValue = curYseries.Max(p => p.data.ydata.Max(pp => pp));
                    var minValue = curYseries.Min(p => p.data.ydata.Max(pp => pp));
                    if (yAxis.MaxValue.HasValue && maxValue > yAxis.MaxValue.Value || yAxis.MinValue.HasValue && minValue < yAxis.MinValue.Value)
                    {
                        textColor = BaseColor.RED;
                        if (yAxis.MaxValue.HasValue && maxValue > yAxis.MaxValue.Value) 
                        {
                            alarmInfo += yAxis.Title + "过高/";
                        }
                        if (yAxis.MinValue.HasValue && minValue < yAxis.MinValue.Value)
                        {
                            alarmInfo += yAxis.Title + "过低/";
                        }

                    }
                    //else{}

	            } 
                if(textColor==BaseColor.GREEN){ alarmInfo = "正常"; }

                //foreach (var item in new int[] { 1, 2, 3, 4 })//if(i<length)({}else{AddCell(new(""))}
                //{
                    //chartTablePdfTable = new PdfPTable(1);
                    //chartTablePdfTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    chartTitlePdfTable = new PdfPTable(5);
                    chartTitlePdfTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    chartTitlePdfTable.SetWidths(new int[]{2,1,1,1,1});
                    //名称、标题
                    //设备
                    chartTitlePdfCell = new PdfPCell(new Phrase(device, new Font(BF_Light, 10)));
                    chartTitlePdfCell.Border = PdfPCell.NO_BORDER;
                    chartTitlePdfTable.AddCell(chartTitlePdfCell);
                    for (int i = 1,j=0; i < 10; i++)
                    {
                        if (i % 5 == 0)//i=5(即第二行第一个)
                        {
                            //告警信息
                            
                            chartTitlePdfCell = new PdfPCell(new Phrase(alarmInfo.TrimEnd('/'), new Font(BF_Light, 8,Font.NORMAL,textColor)));

                            chartTitlePdfCell.Border = PdfPCell.NO_BORDER;

                            chartTitlePdfTable.AddCell(chartTitlePdfCell);

                        }
                        else
                        {
                            //曲线图例
                            if (j < series.Count)
                            {
                                for (; j < series.Count; j++, i++)
                                {
                                    if (i % 5 == 0) { break; }

                                    //图例的标题与图例分开
                                    PdfPTable legend = new PdfPTable(2);
                                    legend.SetWidths(new int[] { 4, 1 });
                                    PdfPCell titleCell = new PdfPCell(new Phrase(series[j].Title, new Font(BF_Light, 5, Font.NORMAL, BaseColor.BLACK)));
                                    PdfPCell legendCell = new PdfPCell(new Phrase("—", new Font(BF_Light, 4, Font.NORMAL, series[j].BaseColor)));
                                    titleCell.Border = PdfPCell.NO_BORDER;
                                    titleCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    legendCell.Border = PdfPCell.NO_BORDER;
                                    legendCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    legendCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    legend.AddCell(titleCell);
                                    legend.AddCell(legendCell);
                                    chartTitlePdfCell = new PdfPCell(legend);

                                    //chartTitlePdfCell = new PdfPCell(new Phrase(series[j].Title + "—", new Font(BF_Light, 6, Font.NORMAL, series[j].BaseColor)));
                                    chartTitlePdfCell.Border = PdfPCell.NO_BORDER;
                                    chartTitlePdfTable.AddCell(chartTitlePdfCell);
                                }
                                i--;
                            }
                            else 
                            {
                                chartTitlePdfCell = new PdfPCell(new Phrase(""));
                                chartTitlePdfCell.Border = PdfPCell.NO_BORDER;
                                chartTitlePdfTable.AddCell(chartTitlePdfCell);
                            }
                            
                        }
                    }

                    //名称、标题
                    chartTitlePdfTableCell = new PdfPCell(chartTitlePdfTable);
                    //只要Table的外边框--SECTION+RECTANGLE-->凵+Π=口
                    chartTitlePdfTableCell.Border = PdfPCell.SECTION;
                    chartTitlePdfTableCell.BorderColor = BaseColor.LIGHT_GRAY;
                    chartTablePdfTable.AddCell(chartTitlePdfTableCell);
                    //图表
                    chartCell = this.GetChartAsPdfCell(axis, series);
                    chartCell.BorderColor = BaseColor.LIGHT_GRAY;
                    chartTablePdfTable.AddCell(chartCell);

                    //chartPdfTable.AddCell(chartTablePdfTable);
                    
                //}
            //return chartPdfTable;
                return chartTablePdfTable;

        }

        //画图表chart
        private PdfPCell GetChartAsPdfCell(Axis axis, List<Series> series)
        {
            PdfPCell cell = new PdfPCell();
            cell.MinimumHeight = 130f;
            cell.Border = Rectangle.RECTANGLE;//RECTANGLE
            //画图的类，并和cell关联                        
            GenerateChart chart = new GenerateChart(axis, series);
            cell.CellEvent = chart;
            return cell;
        }

        

        //设备数据及其图表信息
        public class ChartData
        {
            //设备类型编号
            public int DeviceType { get; set; }
            //设备类型名称
            public string DeviceTypeName { get; set; }
            //设备编号
            public string DeviceID { get; set; }

            //设备
            public string DeviceName { get; set; }
            //设备告警信息
            public string AlarmInfo { get; set; }
            //坐标轴
            public Axis Axis { get; set; }
            //数据
            public List<Series> Series { get; set; }
        }
        //曲线
        public class Series
        {
            //曲线数据
            public Points data { get; set; }
            //对应y轴坐标
            public int SeriesNum { get; set; }

            //曲线颜色(曲线颜色不一定与y坐标轴颜色一致，因为可能存在多条曲线公用同一y轴)
            public BaseColor BaseColor { get; set; }

            //曲线标题
            public string Title { get; set; }
            //数据标识
            public int DataCode { get; set; }
        }
        //坐标轴
        public class Axis
        {
            public xAxis xAxis { get; set; }
            public List<yAxis> yAxises { get; set; }
        }



        //x轴
        public class xAxis
        {
            //x轴刻度
            public float Scale { get; set; }
            //x轴刻度数
            public int ScaleCount { get; set; }
            public string Title { get; set; }
            public string Unit { get; set; }
        }
        //y轴
        public class yAxis
        {
            //y轴刻度
            public float Scale { get; set; }
            //y轴刻度数
            public int ScaleCount { get; set; }
            //标题
            public string Title { get; set; }
            //单位
            public string Unit { get; set; }
            //多条y轴时，定义y轴相对位置...-2,-1,0,1,2,3...
            public int SeriesNum { get; set; }
            //y坐标轴颜色
            public BaseColor BaseColor { get; set; }
            //标杆最大值
            public float? MaxValue { get; set; }
            //标杆最小值
            public float? MinValue { get; set; }
        }
        //曲线数据点
        public class Points
        {
            public List<int> xdata { get; set; }
            public List<float> ydata { get; set; }
        }

        /// <summary>
        /// 曲线坐标轴范围及刻度的自适应算法
        /// </summary>
        /// dynamic d = Standard(Convert.ToDouble(291.5), 0, 4);
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

        //画图表chart实现类
        class GenerateChart : IPdfPCellEvent
        {
            Axis axis;
            List<Series> series;

            float marginLeft;
            float marginRight;
            float marginTop;
            float marginBottom;
            //BaseFont BF_Light = BaseFont.CreateFont(ConfigurationManager.AppSettings["ReportFont"], BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            //画图样式参数以及数据参数--以字段形式通过初始化器初始化，画图重写方法是不提供数据参数入口的，因此参数作为类字段供重写方法使用
            public GenerateChart(Axis axis, List<Series> series)
            {
                this.marginBottom = 10f;
                this.marginTop = 40f;
                this.marginLeft = this.marginRight = 25f;
                this.axis = axis;
                this.series = series;
            }


            /// <summary>
            /// 画图逻辑代码在重写方法CellLayout内
            /// </summary>
            /// <param name="cell"></param>
            /// <param name="position"></param>
            /// <param name="canvases"></param>
            public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
            {
                //PdfContentByte cb = canvases[PdfPTable.LINECANVAS];//指向的同一个引用，导致冲突
                PdfContentByte cb = canvases[PdfPTable.BACKGROUNDCANVAS];
                PdfContentByte cbline = canvases[PdfPTable.LINECANVAS];
                cbline.SaveState();
                cb.SaveState();
                //画笔粗细
                cb.SetLineWidth(0.4f);
                cbline.SetLineWidth(0.2f);
                cbline.SetRGBColorStroke(0x00, 0x00, 0x00);

                //注意Rectangle的四条边基于坐标原点为左下角
                float leftX = position.Left + marginLeft;
                float bottomY = position.Bottom + marginBottom;
                float righX = position.Right - marginRight;
                float topY = position.Top - marginTop;

                //x轴单位刻度值
                float xScale = (righX - leftX) / (this.axis.xAxis.Scale * (this.axis.xAxis.ScaleCount+2));//x轴前后各空出一格
                float yScale;
                //画x轴
                cb.MoveTo(leftX, bottomY);
                cb.LineTo(righX, bottomY);
                cb.Stroke();
                //画x轴突出的刻度
                float xAxisTextLineWidth = 3f;
                float xAxisTextSpaceAdjust = 1.5f;
                for (float x = 0; x <= this.axis.xAxis.Scale * (this.axis.xAxis.ScaleCount); x += this.axis.xAxis.Scale)
                {
                    float xPoint = leftX + (xScale * (this.axis.xAxis.Scale+x));//前面空一格
                    cb.MoveTo(xPoint, bottomY);
                    cb.LineTo(xPoint, bottomY + xAxisTextLineWidth);
                    cb.Stroke();
                    //x轴文本
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, new Phrase(string.Format("{0}", x), new Font(BF_Light, 6)), xPoint + xAxisTextSpaceAdjust, bottomY - 2 * xAxisTextLineWidth, 0);

                }

                //y轴突出的刻度长度
                float yAxisTextLineWidth = 3f;
                float yAxisTextSpaceAdjust = 2f;
                //y轴间间隙
                float yGab = 10f;
                //避免标杆竖线重合重复画线(包括同一y轴和不同y轴)
                List<int> cblineIndex = new List<int>();
                //y轴可能有多条，SeriesNum表示y轴位置...-2,-1,0,1,2,3...(0表示标准y轴)
                foreach (var yAxis in this.axis.yAxises)
                {
                    //画y轴时画笔颜色
                    //cb.SetRGBColorStroke(0x00, 0x00, 0x00);
                    cb.SetColorStroke(yAxis.BaseColor);
                    //y轴位置
                    float yAxisPosition = leftX;
                    if (yAxis.SeriesNum <= 0)
                    {
                        yAxisPosition = leftX + yAxis.SeriesNum * yGab;
                        yAxisTextLineWidth = 3f;//虽然初始化为3f，但缺少这句导致当先画右边的y轴时再画左边y轴时，导致左边y轴刻度方向相反
                    }
                    else
                    {
                        yAxisPosition = righX + (yAxis.SeriesNum - 1) * yGab;
                        yAxisTextLineWidth = -3.2f;
                    }
                    //y轴单位刻度值
                    yScale = (topY - bottomY) / (yAxis.Scale * (yAxis.ScaleCount));
                    //画y轴
                    cb.MoveTo(yAxisPosition, bottomY);
                    cb.LineTo(yAxisPosition, topY);
                    cb.Stroke();
                    //画y轴突出的刻度
                    for (float y = 0; y <= yAxis.Scale * (yAxis.ScaleCount); y += yAxis.Scale)
                    {
                        float yPoint = bottomY + (yScale * y);
                        cb.MoveTo(yAxisPosition, yPoint);
                        cb.LineTo(yAxisPosition + yAxisTextLineWidth, yPoint);
                        cb.Stroke();
                        //y轴文本
                        ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, new Phrase(string.Format("{0}", y), new Font(BF_Light, 6)), yAxis.SeriesNum <= 0 ? yAxisPosition - yAxisTextLineWidth : yAxisPosition - 4 * yAxisTextLineWidth, yPoint - yAxisTextSpaceAdjust, 0);//yAxisPosition = (leftX)righX + ...,左边、右边刻度文本位置分别基于leftX、righX,因此+、-不同

                    }
                    //画y轴基准线//目前只画左边一条y轴上的基准线
                    if(yAxis.SeriesNum==0&&yAxis.MaxValue.HasValue)
                    {
                        cb.SetLineDash(2,2,0);
                        cb.SetColorStroke(BaseColor.RED);
                        cb.MoveTo(leftX, bottomY+ yScale*yAxis.MaxValue.Value);
                        cb.LineTo(righX, bottomY + yScale * yAxis.MaxValue.Value);
                        cb.Stroke();
                        cb.SetLineDash(0);
                        cb.SetColorStroke(yAxis.BaseColor);

                    }
                    if (yAxis.SeriesNum == 0 && yAxis.MinValue.HasValue)
                    {
                        cb.SetLineDash(2, 2, 0);
                        cb.SetColorStroke(BaseColor.RED);
                        cb.MoveTo(leftX, bottomY + yScale * yAxis.MinValue.Value);
                        cb.LineTo(righX, bottomY + yScale * yAxis.MinValue.Value);
                        cb.Stroke();
                        cb.SetLineDash(0);
                        cb.SetColorStroke(yAxis.BaseColor);

                    }

                    //画y轴标题
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, new Phrase(string.Format("{0}", yAxis.Title+yAxis.Unit), new Font(BF_Light, 5)), yAxis.SeriesNum <= 0 ? yAxisPosition - yAxisTextLineWidth * (-3) : yAxisPosition - 4 * yAxisTextLineWidth / 2, bottomY + yAxis.Scale * (yAxis.ScaleCount) * yScale + yAxisTextSpaceAdjust * 2, 0);

                    //描点并连线
                    //多组数据公用y轴
                    //某一y轴的一系列曲线
                    List<Series> curYseries = this.series.Where(p => p.SeriesNum == yAxis.SeriesNum).ToList();
                    //避免标杆竖线重合重复画线(包括同一y轴和不同y轴)
                    //List<int> cblineIndex = new List<int>();

                    //var maxValue = series.Max(p => p.data.ydata.Max(pp => pp));
                    //var minValue = series.Min(p => p.data.ydata.Max(pp => pp));

                    //画某一y轴的一系列曲线
                    foreach (var line in curYseries)
                    {
                        
                        //画曲线时画笔颜色
                        //cb.SetRGBColorStroke(0xFF, 0x45, 0x00);
                        cb.SetColorStroke(line.BaseColor);
                        var xdata = line.data.xdata;
                        var ydata = line.data.ydata;
                        //某条曲线描点并连线
                        for (int xIndex = 0; xIndex < xdata.Count; xIndex++)
                        {
                            float value = ydata[xIndex];
                            float xPoint = leftX + (this.axis.xAxis.Scale + xdata[xIndex]) * xScale;
                            float yPoint = bottomY + yScale * value;

                            //画曲线
                            if (xIndex == 0)
                            {
                                cb.MoveTo(xPoint, yPoint);
                            }
                            else
                            {
                                cb.LineTo(xPoint, yPoint);
                            }

                            //描点并连线画曲线同时画竖线
                            //避免重复画竖线--
                            if (!cblineIndex.Contains(xIndex))
                            {

                                //竖线文本
                                //List<object> remark = new List<object>();
                                //指定位置处画竖线
                                if (yAxis.MaxValue.HasValue && value > yAxis.MaxValue.Value || yAxis.MinValue.HasValue && value < yAxis.MinValue.Value)
                                {
                                    //标记此位置已有竖线，不再重复画线
                                    cblineIndex.Add(xIndex);
                                    //竖线
                                    cbline.MoveTo(xPoint, topY);
                                    cbline.LineTo(xPoint, bottomY + 2 * xAxisTextLineWidth);
                                    cbline.Stroke();
                                    //竖线文本
                                    //remark.Add(series.Select(p=>new {title = p.Title,value = p.data.xdata[xIndex]}));
                                    //竖线文本数
                                    int textNum = 1;
                                    //循环所有曲线(包括不同y轴上的)找到竖线与曲线交点值并在交点处画圆点
                                    this.series.OrderByDescending(p => p.Title).ToList().ForEach(p =>
                                    {
                                        
                                        var title = p.Title + ":";
                                        //var alarmInfo = "";//若有多次越过标杆线则此处会重复概览的alarmInfo
                                        //竖线标题
                                        ColumnText.ShowTextAligned(cbline, Element.ALIGN_RIGHT, new Phrase(string.Format("{0}", title), new Font(BF_Light, 3.8f)), xPoint - xAxisTextSpaceAdjust, topY + textNum * xAxisTextLineWidth*2, 0);
                                        //竖线值及其竖线与曲线交点处画圆点
                                        var valueColor = BaseColor.GREEN;
                                        float lineValue = p.data.ydata[xIndex];
                                        
                                        //曲线对应的y轴
                                        var yaxis = this.axis.yAxises.Where(pp => pp.SeriesNum == p.SeriesNum).FirstOrDefault();
                                        //y轴单位刻度值
                                        var yscale = (topY - bottomY) / (yaxis.Scale * (yaxis.ScaleCount));
                                        //仅在超标曲线交点处画圆点
                                        if (yaxis.MaxValue.HasValue && lineValue > yaxis.MaxValue.Value || yaxis.MinValue.HasValue && lineValue < yaxis.MinValue.Value)
                                        {
                                            //画交点圆点
                                            cbline.Ellipse(xPoint - 1, bottomY + lineValue * yscale - 1, xPoint + 1, bottomY + lineValue * yscale + 1);
                                            cbline.SetColorStroke(BaseColor.RED);//画圆圈颜色
                                            cbline.SetColorFill(BaseColor.RED);//圆圈填充颜色
                                            //cbline.SetLineWidth(0.2f);//画圆圈粗度同竖线粗度
                                            cbline.FillStroke();
                                            
                                        }
                                        cbline.SetColorFill(BaseColor.BLACK);//恢复颜色//否则画字体颜色会受到影响
                                        cbline.SetColorStroke(BaseColor.BLACK);//单纯设为黑色无效需同时SetColorFill
                                        //是否超标
                                        if (yaxis.MaxValue.HasValue && lineValue > yaxis.MaxValue.Value)
                                        {
                                            valueColor = BaseColor.RED;
                                            //alarmInfo += p.Title + "过高/";
                                        }
                                        if (yaxis.MinValue.HasValue && lineValue < yaxis.MinValue.Value)
                                        {
                                            valueColor = BaseColor.RED;
                                            //alarmInfo += p.Title + "过低/";
                                        }
                                        //竖线值
                                        //设置一段文字各部分颜色--各颜色文字分开，但坐标如何自适应；Element.ALIGN_RIGHT+Element.ALIGN_LEFT
                                        ColumnText.ShowTextAligned(cbline, Element.ALIGN_LEFT, new Phrase(string.Format("{0}", lineValue), new Font(BF_Light, 3.8f, Font.NORMAL, valueColor)), xPoint + xAxisTextSpaceAdjust, topY + textNum * xAxisTextLineWidth*2, 0);
                                        textNum++;
                                    });
                                }

                            }


                        }

                        //以当前颜色画曲线
                        cb.Stroke();
                    }

                    //以当前颜色画曲线，但是此时颜色为最后一次设置的颜色，产生bug
                    //cb.Stroke();

                }

                cb.RestoreState();
                cbline.RestoreState();

            }
        }
    }
}