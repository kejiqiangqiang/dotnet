namespace WindowsFormsThreadProgressBar
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CtrlButton_Start = new System.Windows.Forms.Button();
            this.CtrlButton_Pause = new System.Windows.Forms.Button();
            this.CtrlButton_Continue = new System.Windows.Forms.Button();
            this.CtrlProgressBar_Demo = new System.Windows.Forms.ProgressBar();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // CtrlButton_Start
            // 
            this.CtrlButton_Start.Location = new System.Drawing.Point(12, 226);
            this.CtrlButton_Start.Name = "CtrlButton_Start";
            this.CtrlButton_Start.Size = new System.Drawing.Size(75, 23);
            this.CtrlButton_Start.TabIndex = 0;
            this.CtrlButton_Start.Text = "Start";
            this.CtrlButton_Start.UseVisualStyleBackColor = true;
            this.CtrlButton_Start.Click += new System.EventHandler(this.CtrlButton_Start_Click);
            // 
            // CtrlButton_Pause
            // 
            this.CtrlButton_Pause.Location = new System.Drawing.Point(105, 226);
            this.CtrlButton_Pause.Name = "CtrlButton_Pause";
            this.CtrlButton_Pause.Size = new System.Drawing.Size(75, 23);
            this.CtrlButton_Pause.TabIndex = 1;
            this.CtrlButton_Pause.Text = "Pause";
            this.CtrlButton_Pause.UseVisualStyleBackColor = true;
            this.CtrlButton_Pause.Click += new System.EventHandler(this.CtrlButton_Pause_Click);
            // 
            // CtrlButton_Continue
            // 
            this.CtrlButton_Continue.Location = new System.Drawing.Point(197, 226);
            this.CtrlButton_Continue.Name = "CtrlButton_Continue";
            this.CtrlButton_Continue.Size = new System.Drawing.Size(75, 23);
            this.CtrlButton_Continue.TabIndex = 2;
            this.CtrlButton_Continue.Text = "Continue";
            this.CtrlButton_Continue.UseVisualStyleBackColor = true;
            this.CtrlButton_Continue.Click += new System.EventHandler(this.CtrlButton_Continue_Click);
            // 
            // CtrlProgressBar_Demo
            // 
            this.CtrlProgressBar_Demo.Location = new System.Drawing.Point(12, 162);
            this.CtrlProgressBar_Demo.Name = "CtrlProgressBar_Demo";
            this.CtrlProgressBar_Demo.Size = new System.Drawing.Size(260, 23);
            this.CtrlProgressBar_Demo.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1, 1);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(284, 140);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.CtrlProgressBar_Demo);
            this.Controls.Add(this.CtrlButton_Continue);
            this.Controls.Add(this.CtrlButton_Pause);
            this.Controls.Add(this.CtrlButton_Start);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CtrlButton_Start;
        private System.Windows.Forms.Button CtrlButton_Pause;
        private System.Windows.Forms.Button CtrlButton_Continue;
        private System.Windows.Forms.ProgressBar CtrlProgressBar_Demo;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

