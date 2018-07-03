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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            FormLeft formLeft = new FormLeft();
            formLeft.NodeSelectEvent += this.FormLeft_NodeSelectEvent;
            formLeft.CloseButton = false;
            this.dockPanel1.DockLeftPortion = 0.2;
            formLeft.Show(dockPanel1, DockState.DockLeft);
        }
        /// <summary>
        /// 以事件委托的方式将加载任务线程列表到右侧tab页列表的工作交给主窗口线程
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="wcfUrl"></param>
        private void FormLeft_NodeSelectEvent(PluginModel pageModel, string wcfUrl)
        {
            FormPlugin frm = this.FindDockContent(pageModel.PluginId);
            if (frm == null)
            {
                frm = new FormPlugin();
                frm.Text = pageModel.PluginName;
                frm.Tag = pageModel.PluginId;
                frm.WcfUrl = wcfUrl;
                frm.PluginId = pageModel.PluginId;
                frm.Show(dockPanel1, DockState.Document);
            }
            frm.Activate();
        }
        private FormPlugin FindDockContent(string frmID)
        {
            foreach (DockContent item in this.dockPanel1.Contents)
            {
                if (item is FormPlugin && item.Tag != null && item.Tag.ToString() == frmID)
                {
                    return (FormPlugin)item;
                }
            }
            return null;
        }
    }
}
