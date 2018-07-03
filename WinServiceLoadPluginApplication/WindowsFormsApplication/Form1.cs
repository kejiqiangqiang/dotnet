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

namespace WindowsFormsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击按钮获取插件列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string serverName = ServiceConfig.ServiceName;
            string host = ServiceConfig.Host + ":" + ServiceConfig.Port;
            TreeNode serverNode = new TreeNode();
            serverNode.ImageIndex = 0;
            serverNode.Text = serverName;
            serverNode.Tag = host;
            serverNode.Expand();
            TreeNode hceckNode = null;
            //插件
            List<PluginModel> pluginModels = new List<PluginModel>();
            try
            {
                pluginModels = ClientApplication.GetPlugins(host);
                foreach (var pluginModel in pluginModels)
                {
                    TreeNode node = new TreeNode();
                    node.ImageIndex = 0;
                    node.Text = pluginModel.PluginName;
                    node.Tag = pluginModel;
                    if (hceckNode == null)
                    {
                        node.Checked = true;
                        hceckNode = node;
                    }
                    serverNode.Nodes.Add(node);
                }
            }
            catch (Exception xx)
            {
                serverNode.Text = serverNode.Text + xx.Message;
            }

            this.treeView1.Nodes.Add(serverNode);
            this.treeView1.SelectedNode = hceckNode;

            this.richTextBox1.AppendText("获取插件列表成功\n");
            
            //1.注册客户端并绑定接收消息事件
            ThreadPool.QueueUserWorkItem(s => {
                MessageClient MessageClient = new MessageClient(host);
                MessageClient.ReceiveMessageEvent += ShowMessage;
            });

            /******* 必须由服务端方法调用，不能在此处Winform调用（客户端）进行模拟操作，否则因注册的客户端与此处不是同一通道，即客户端注册的通道与此处发起回调的通道不一致，最终无法模拟出服务端主动发送消息到客户端 ******/
            ////2.服务端主动发送消息到客户端（通过调用客户端回调方法）--由服务端方法调用--此处直接在Winform调用
            //this.WatchMessage(taskID: "1", msg: "接收到服务端主动发送的消息", msgType: MessageType.Message);
        }

        /// <summary>
        /// winform显示信息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMessage(MessageModel msg)
        {
            string message = msg.TaskId + "\n" + msg.PluginId + "\n" + msg.MessageType + "\n" + msg.Message + "\n";
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.Invoke(new Action(() => this.ShowMessage(msg)));
            }
            else
            {
                this.richTextBox1.AppendText(message);
            }
        }

        ///// <summary>
        ///// 输出消息监视到客户端
        ///// </summary>
        ///// <param name="taskID">任务编号</param>
        ///// <param name="msg">消息</param>
        ///// <param name="msgType">消息类型</param>
        //internal void WatchMessage(string taskID, string msg, MessageType msgType)
        //{
        //    ServerApplication.SendMessage(new MessageModel
        //    {
        //        TaskId = taskID,
        //        PluginId = this.ToString(),
        //        MessageType = msgType.ToString(),
        //        Message = msg,
        //    }, AppDomain.CurrentDomain);
        //}

    }
}
