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

namespace WindowsFormsThreadProgressBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
            this.InitializeControls();
            //初始化进度条线程
            this.InitializeThread();
        }

        private const int MAX_PROGRESS_VALUE = 100;
        private int _currentProgressBarPosition;
        private int _progressBarStep;
        private bool isOk = false;
        private bool isRun = false;
        private Thread _progressBarControllerThread;
        private static AutoResetEvent are = new AutoResetEvent(true);
        private static object obj = new object();
        /// <summary>
        /// 设置或获取当前进度条的步长。
        /// </summary>
        private int ProgressBarStep
        {
            get { return _progressBarStep; }
            set { this.CtrlProgressBar_Demo.Step=value;_progressBarStep = value; }
        }

        /// <summary>
        /// 设置或获取当前进度条的位置。
        /// </summary>
        private int CurrentProgressBarPosition
        {
            get { return _currentProgressBarPosition; }
            set { _currentProgressBarPosition = value; }
        }


        /// <summary>
        /// 设置或获取进度条控制线程。
        /// </summary>
        private Thread ProgressBarControllerThread
        {
            get { return _progressBarControllerThread; }
            set { _progressBarControllerThread = value; }
        }

        /// <summary>
        /// 初始化控件。
        /// </summary>
        private void InitializeControls()
        {
            this.ProgressBarStep = 1;
            this.CurrentProgressBarPosition = 0;
            this.CtrlProgressBar_Demo.Maximum = Form1.MAX_PROGRESS_VALUE;
            this.CtrlButton_Start.Enabled = true;
            this.CtrlButton_Pause.Enabled = false;
            this.CtrlButton_Continue.Enabled = false;
        }


        private void CtrlButton_Start_Click(object sender, EventArgs e)
        {
            //start progressbar
            //判断ThreadState状态，用&运算
            if (!isOk&&(this.ProgressBarControllerThread.ThreadState&ThreadState.Unstarted)!=0)
            {
                this.ProgressBarControllerThread.Start();
                isRun = true;
                this.CtrlButton_Start.Enabled = false;
                this.CtrlButton_Pause.Enabled = true;
                this.CtrlButton_Continue.Enabled = true;
            }
            //do something

        }

        private void CtrlButton_Pause_Click(object sender, EventArgs e)
        {
            //ThreadState.Running本身等于0，不能用&运算
            //此时线程切换到主线程，此时this.ProgressBarControllerThread.ThreadState==Backgouund|WaitSleepJoin，被挂起
            //if ((this.ProgressBarControllerThread.ThreadState & ThreadState.Stopped) == 0 && (this.ProgressBarControllerThread.ThreadState & ThreadState.WaitSleepJoin) == 0)
            if (!isOk && (this.ProgressBarControllerThread.ThreadState & ThreadState.WaitSleepJoin) != 0)
            {
                isRun = false;
                //are.WaitOne();//此处切换到主线程，会阻值当前线程，导致界面卡死//应该在需要控制的线程this.ProgressBarControllerThread内使用
                this.CtrlButton_Pause.Enabled = false;
                this.CtrlButton_Continue.Enabled = true;
                //this.richTextBox1.AppendText("暂停中...\n");
                this.ModifyRichTextBoxStatusCallback("暂停中...\n");
            }
            
        }

        private void CtrlButton_Continue_Click(object sender, EventArgs e)
        {
            if (!isOk && (this.ProgressBarControllerThread.ThreadState & ThreadState.WaitSleepJoin) != 0)
            {
                isRun = true;
                are.Set();//此信号可以被this.ProgressBarControllerThread线程接收
                this.CtrlButton_Pause.Enabled = true;
                this.CtrlButton_Continue.Enabled = false;
            }
        }

        /// <summary>
        /// 初始化线程。
        /// </summary>
        private void InitializeThread()
        {
            this.ProgressBarControllerThread = new Thread(new ThreadStart(this.UpgradeProgressBarStatus))
            {
                IsBackground = true
            };
        }

        /// <summary>
        /// 更新进度条的状态。
        /// </summary>
        private void UpgradeProgressBarStatus()
        {
            while (!isOk)
            {
                if (isRun)//暂停按钮将其赋值为false(在线程1中)，之后多线程中会切换回到此处线程（线程2），仍会继续执行此处，即仍会打印一次"进行中..."  
                {   
                    //放在靠前
                    this.ModifyRichTextBoxStatusCallback("进行中..." + this.CurrentProgressBarPosition + "%\n");
                    
                    if (this.CurrentProgressBarPosition < Form1.MAX_PROGRESS_VALUE)
                    {
                        Thread.Sleep(50);//当前线程休眠0.05秒，模拟其他事务。
                        this.CurrentProgressBarPosition += this.ProgressBarStep;
                        new MethodInvoker(this.ModifyProgressBarStatusCallback).Invoke();//因为是跨线程访问控件，因此使用委托调用ModifyProgressBarStatusCallback方法。
                        ////放在靠前
                        //this.ModifyRichTextBoxStatusCallback("进行中..." + this.CurrentProgressBarPosition + "%\n");
                    }
                    else
                    {
                        isOk = true;//最终完成后isOk=true
                        //break;
                    }
                }
                else
                {
                    //收到信号前不会继续执行
                    are.WaitOne();
                }
                //放在此处会多执行一次（最终完成后isOk=true，最后一次仍会执行一次）
                //this.ModifyRichTextBoxStatusCallback("进行中..." + this.CurrentProgressBarPosition + "%\n");


            }
            //完成后禁用按钮
            this.CtrlButton_PauseDisabled();
        }

        /// <summary>
        /// 修改进度条的状态。
        /// </summary>
        private void ModifyProgressBarStatusCallback()
        {
            if (this.CtrlProgressBar_Demo.InvokeRequired)
            {
                this.CtrlProgressBar_Demo.Invoke(new MethodInvoker(this.ModifyProgressBarStatusCallback));
            }
            else
            {
                this.CtrlProgressBar_Demo.PerformStep();
            }
        }

        /// <summary>
        /// 修改进文本框的状态。
        /// </summary>
        private void ModifyRichTextBoxStatusCallback(string iMessage)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                //this.richTextBox1.Invoke(new Action<string>(this.ModifyRichTextBoxStatusCallback),iMessage);
                this.richTextBox1.Invoke((Action<string>)(p => this.ModifyRichTextBoxStatusCallback(p)), iMessage);
                //lambda表达式不能直接转换为System.Delegate（抽象类），必须转换为具体的某种委托类型，才能作为System.Delegate类型的参数使用
                //this.richTextBox1.Invoke(p => this.ModifyRichTextBoxStatusCallback(p), iMessage);

            }
            else
            {
                this.richTextBox1.AppendText(iMessage);
            }
        }

        private void CtrlButton_PauseDisabled()
        {
            if (this.CtrlButton_Pause.InvokeRequired)
            {
                this.CtrlButton_Pause.Invoke(new Action(this.CtrlButton_PauseDisabled));
            }
            else
            {
                this.CtrlButton_Pause.Enabled = false;
                this.CtrlButton_Continue.Enabled = false;
            }
        }
         
    }
}
