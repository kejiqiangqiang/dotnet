using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCFDuplexClientBaseApplication;
using WeifenLuo.WinFormsUI.Docking;

namespace WindowsFormsApplication
{
    public partial class FormPlugin : DockContent//:Form->:DockContent
    {
        bool isStop = false;
        /// <summary>
        /// 注册客户端及接收服务端消息事件
        /// </summary>
        MessageClient MessageClient = null;

        //窗体间参数传递--FormLeft_Load-->treeView1_AfterSelect-->FormLeft_NodeSelectEvent-->this...-->WCF:TaskControlService
        /// <summary>
        /// 接收参数
        /// </summary>
        public string PluginId { get; set; }
        /// <summary>
        /// 接收参数
        /// </summary>
        public string WcfUrl { get; set; }

        string xmlConfig = null;
        string TaskID = "";
        public FormPlugin()
        {
            FormPlugin.CheckForIllegalCrossThreadCalls = false;//禁用跨线程检查

            InitializeComponent();

            // 初始化ListView 
            this.taskListView.Left = 0;
            this.taskListView.Top = 0;
            this.taskListView.Width = 700;
            this.taskListView.Height = this.ClientRectangle.Height;
            this.taskListView.GridLines = true;
            this.taskListView.FullRowSelect = true;
            this.taskListView.View = View.Details;
            this.taskListView.Scrollable = true;
            this.taskListView.MultiSelect = false;
            this.taskListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            // 针对数据库的字段名称，建立与之适应显示表头 
            //this.taskListView.Columns.Add("线程编号", 100, HorizontalAlignment.Left);
            this.taskListView.Columns.Add("线程名称", 150, HorizontalAlignment.Left);
            this.taskListView.Columns.Add("执行频率(秒)", 100, HorizontalAlignment.Left);
            this.taskListView.Columns.Add("出错次数", 80, HorizontalAlignment.Left);
            this.taskListView.Columns.Add("运行状态", 80, HorizontalAlignment.Left);
            this.taskListView.Columns.Add("最后一次出错信息", 300, HorizontalAlignment.Left);
            //this.taskListView.Columns.Add("最后一次用时(秒)", 150, HorizontalAlignment.Left);
            this.taskListView.Visible = true;
            taskListView.HideSelection = false;
        }

        /// <summary>
        /// 加载务线程列表并向服务端注册以接收服务端输出的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPlugin_Load(object sender, EventArgs e)
        {
            //服务端回调客户端方法需要新启线程或者线程重入，避免UI线程卡死
            ThreadPool.QueueUserWorkItem(s=>{
                //向服务端注册消息客户端以及回调方法
                MessageClient = new MessageClient(this.WcfUrl);
                MessageClient.ReceiveMessageEvent += ClientApplication_ReceiveMessageEvent;
                //加载任务线程列表
                this.ShowTaskInfo();
            });
            
        }

        private void FormPlugin_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageClient.ReceiveMessageEvent -= ClientApplication_ReceiveMessageEvent;
            MessageClient.Close();
        }
        private void taskListView_Click(object sender, EventArgs e)
        {
            TaskModel taskModel = this.taskListView.SelectedItems[0].Tag as TaskModel;
            if (taskModel != null)
            {
                //准备显示选中的任务线程从服务端输出的消息
                this.richTextBoxRemote.Text = "";//点击不同的任务线程，先清空消息
                this.TaskID = taskModel.TaskId;//赋值为当前点击的任务线程
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.richTextBoxRemote.Text = "";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isStop = !isStop;
            btnStop.Text = isStop ? "开始监视" : "停止监视";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Dictionary<string, ConfigModel> configs = new Dictionary<string, ConfigModel>();
            foreach (Control item in this.panelMain.Controls)
            {
                foreach (var txtItem in item.Controls)
                {
                    if (txtItem is TextBox)
                    {
                        TextBox txt = (TextBox)txtItem;
                        ConfigModel pc = txt.Tag as ConfigModel;
                        if (pc!=null)
                        {
                            pc.Value = txt.Text;
                            configs.Add(pc.Key, pc);
                        }
                    }
                }
            }
            Dictionary<string, ConfigModel> enabledConfig = PluginConfig.ConvertToModel(this.xmlConfig, "Enabled");
            string xmlConfig = PluginConfig.ConvertToXml(configs, enabledConfig);
            ClientApplication.SavePluginConfig(this.PluginId, this.WcfUrl, xmlConfig);
            MessageBox.Show("修改成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 加载任务列表及插件配置信息
        /// </summary>
        private void ShowTaskInfo()
        {
            //加载任务列表
            this.taskListView.Items.Clear();
            List<TaskModel> models = ClientApplication.GetPluginTasks(this.WcfUrl, this.PluginId);
            foreach (var task in models)
            {
                ListViewItem first = new ListViewItem(task.TaskName);
                first.Tag = task;
                first.SubItems.Add(task.IntervalTime.ToString());
                first.SubItems.Add(task.ErrorCount.ToString());
                first.SubItems.Add(task.Status.ToString());
                first.SubItems.Add(task.LastErrorInfo);
                first.Selected = true;
                if (!string.IsNullOrEmpty(task.LastErrorInfo))
                {
                    first.ForeColor = Color.Red;
                }
                this.taskListView.Items.Add(first);
            }
            //加载配置信息
            this.ShowPluinConfigInfo();
            
        }
        /// <summary>
        /// 加载配置信息
        /// </summary>
        private void ShowPluinConfigInfo()
        {
            ThreadPool.QueueUserWorkItem(th =>
            {
                //加载配置信息
                xmlConfig = ClientApplication.GetPluginConfig(this.WcfUrl, this.PluginId);
                Dictionary<string, ConfigModel> configs = PluginConfig.ConvertToModel(xmlConfig, "Group");
                List<string> groupNames = configs.Values.ToList().Select(p => p.Group).Distinct().ToList();
                foreach (var item in groupNames)
                {
                    GroupBox groupBox = new GroupBox();
                    groupBox.Text = item;
                    groupBox.Dock = DockStyle.Top;

                    int n = 6;
                    int height = 40;
                    foreach (ConfigModel groupConfig in configs.Values)
                    {
                        Label lbl = new Label();
                        lbl.Text = groupConfig.TextName;
                        lbl.AutoSize = true;
                        lbl.Location = new Point(12, 19 + n);

                        TextBox txt = new TextBox();
                        txt.Text = groupConfig.Value;
                        txt.Location = new Point(150, 14 + n);
                        txt.Width = 600;
                        txt.Tag = groupConfig;
                        txt.BorderStyle = BorderStyle.FixedSingle;
                        txt.Height = 23;

                        groupBox.Controls.Add(lbl);
                        groupBox.Controls.Add(txt);
                        n = n + 30;
                        height = height + 28;
                    }
                    groupBox.Height = height;
                    this.panelMain.Controls.Add(groupBox);
                }
            });
        } 

        /// <summary>
        /// 服务端回调客户端方法
        /// </summary>
        /// <param name="message"></param>
        private void ClientApplication_ReceiveMessageEvent(MessageModel message)
        {
            try
            {
                //isStop总开关，一个地方关闭，所有线程均不打印消息
                if (message != null && message.TaskId == this.TaskID && !isStop)
                {
                    //message.Message += System.Environment.NewLine;
                    switch (message.MessageType)
                    {
                        case "Error": this.LogError(message.Message); break;
                        case "Message": this.LogMessage(message.Message); break;
                        case "Warning": this.LogWarning(message.Message); break;
                        default: this.LogMessage(message.Message); break;
                    }
                }
            }
            catch
            {
            }
        }

        #region 日志记录、支持其他线程访问
        public delegate void LogAppendDelegate(Color color, string text);

        /// <summary> 
        /// 追加显示文本 
        /// </summary> 
        /// <param name="color">文本颜色</param> 
        /// <param name="text">显示文本</param> 
        public void LogAppend(Color color, string text)
        {
            richTextBoxRemote.SelectionColor = color;
            richTextBoxRemote.AppendText(text + "\n");
        }
        /// <summary> 
        /// 显示错误日志 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogError(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            richTextBoxRemote.Invoke(la, Color.Red, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + text);
        }
        /// <summary> 
        /// 显示警告信息 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogWarning(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            richTextBoxRemote.Invoke(la, Color.OrangeRed, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + text);
        }
        /// <summary> 
        /// 显示信息 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            richTextBoxRemote.Invoke(la, Color.Green, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + text);
        }
        #endregion

    }
}
