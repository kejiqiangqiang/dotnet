namespace SocketClient
{
    partial class Main
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textSendMsg = new System.Windows.Forms.TextBox();
            this.richTextMsg = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textIP = new System.Windows.Forms.TextBox();
            this.textPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(507, 18);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "连接服务器";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(507, 300);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsg.TabIndex = 1;
            this.btnSendMsg.Text = "发送消息";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(279, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "端口号：";
            // 
            // textSendMsg
            // 
            this.textSendMsg.Location = new System.Drawing.Point(34, 302);
            this.textSendMsg.Name = "textSendMsg";
            this.textSendMsg.Size = new System.Drawing.Size(467, 21);
            this.textSendMsg.TabIndex = 6;
            // 
            // richTextMsg
            // 
            this.richTextMsg.Location = new System.Drawing.Point(34, 89);
            this.richTextMsg.Name = "richTextMsg";
            this.richTextMsg.Size = new System.Drawing.Size(548, 192);
            this.richTextMsg.TabIndex = 7;
            this.richTextMsg.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "消息";
            // 
            // textIP
            // 
            this.textIP.Location = new System.Drawing.Point(90, 20);
            this.textIP.Name = "textIP";
            this.textIP.Size = new System.Drawing.Size(149, 21);
            this.textIP.TabIndex = 9;
            this.textIP.Text = "127.0.0.1";
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(338, 20);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(67, 21);
            this.textPort.TabIndex = 10;
            this.textPort.Text = "8098";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 347);
            this.Controls.Add(this.textPort);
            this.Controls.Add(this.textIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextMsg);
            this.Controls.Add(this.textSendMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.btnConnect);
            this.Name = "Main";
            this.Text = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textSendMsg;
        private System.Windows.Forms.RichTextBox richTextMsg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textIP;
        private System.Windows.Forms.TextBox textPort;
    }
}

