using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCFDuplexClientBaseApplication;
using WeifenLuo.WinFormsUI.Docking;

namespace WindowsFormsApplication
{
    //以事件委托的方式将加载任务线程列表到右侧tab页列表的工作交给主窗口线程
    public delegate void NodeSelectDelegate(PluginModel pageModel, string url);

    public partial class FormLeft : DockContent//:Form->:DockContent
    {
        //以事件委托的方式将加载任务线程列表到右侧tab页列表的工作交给主窗口线程
        public event NodeSelectDelegate NodeSelectEvent = null;

        public FormLeft()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载插件列表到左侧树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLeft_Load(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            ImageList imgs = new ImageList();
            imgs.Images.Add(Properties.Resources.Proxy);
            this.treeView1.ImageList=imgs;

            TreeNode checkNode = null;
            //以服务主机名称为根节点
            TreeNode serverNode = new TreeNode();
            string ServerName = ServiceConfig.ServiceName;
            string Host = ServiceConfig.Host + ":" + ServiceConfig.Port;
            serverNode.ImageIndex = 0;
            serverNode.Text = ServerName;
            serverNode.Tag = Host;
            serverNode.Expand();
            //获取插件并加入到子节点
            List<PluginModel> pluginModels = new List<PluginModel>();
            try
            {
                pluginModels = ClientApplication.GetPlugins(Host);
                foreach (var pluginModel in pluginModels)
                {
                    TreeNode node = new TreeNode();
                    node.ImageIndex = 0;
                    node.Text = pluginModel.PluginName;
                    node.Tag = pluginModel;
                    if (checkNode == null)
                    {
                        node.Checked = true;
                        checkNode = node;
                    }
                    serverNode.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                serverNode.Text += ex.Message;
            }
            this.treeView1.Nodes.Add(serverNode);
            this.treeView1.SelectedNode = checkNode;
 
        }
        /// <summary>
        /// 选中某一插件节点加载插件任务线程列表到右侧tab页以列表形式展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.NodeSelectEvent!=null)
            {
                PluginModel type = e.Node.Tag as PluginModel;
                if (type == null)
                {
                    return;
                }
                //以事件委托的方式将加载任务线程列表到右侧tab页列表的工作交给主窗口线程
                this.NodeSelectEvent(type, e.Node.Parent.Tag.ToString());
            }
        }

        private void FrmAddServer_AddServerEvent(bool isOk)
        {
            this.FormLeft_Load(this, null);
        }

    }
}
