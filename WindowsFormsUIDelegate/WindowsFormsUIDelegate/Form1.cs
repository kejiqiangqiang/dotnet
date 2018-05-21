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

namespace WindowsFormsUICallBack
{
    public partial class Form1 : Form
    {
        private bool isStop = true;
        public delegate void Process(string iMessage);
        private delegate void SetProgress(int progress);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string msg = "";
            Process p = new Process(this.progressBarIncrease);
            IAsyncResult rs = p.BeginInvoke(msg, null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.btnStop();
        }

        private void progressBarIncrease(string iMessage)
        {
            int progress = 0;
            int step = 1;

            while (progress < 100)
            {
                if (!this.isStop )
                {
                    //do something
                    Thread.Sleep(50);
                    progress += step;
                    //更新UI主界面
                    this.updateProgressBar(progress);
                    this.updateRichTextBox(progress.ToString());
                }
            }
            Process p = new Process(this.updateRichTextBox);
            p("progressBarIncrease Delegate has been Done");
            //this.button1.Text = "完成";
            //this.button1.Enabled = false;//执行按钮赋值时线程切换至UI线程，无法继续执行操作按钮//应使用委托
            Process p0 = new Process(this.buttonDisabled);
            p0("完成");
        }

        private void btnStop()
        {
            isStop = !isStop;
            this.button1.Text = isStop ? "开始" : "暂停";
        }

        private void updateProgressBar(int progress)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProgress setProgress = new SetProgress(this.updateProgressBar);
                this.progressBar1.Invoke(setProgress, progress);
            }
            else
            {
                this.progressBar1.Value = progress;
                this.label1.Text = progress + "%";
            }
        }
        private void updateRichTextBox(string iMessage)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                Process p = new Process(this.updateRichTextBox);
                this.richTextBox1.Invoke(p, iMessage);
            }
            else
            {
                var text = iMessage + "\r\n";
                //this.richTextBox1.Text += text;
                this.richTextBox1.AppendText(text);
            }
        }

        private void buttonDisabled(string buttonText)
        {
            if (this.button1.InvokeRequired)
            {
                Process p = new Process(this.buttonDisabled);
                this.button1.Invoke(p, buttonText);
            }
            else
            {
                this.button1.Text = buttonText;
                this.button1.Enabled = false;
            }
        }
    }
}
