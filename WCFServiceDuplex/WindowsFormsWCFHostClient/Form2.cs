using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsWCFHostClient
{
    public partial class Form2 : Form
    {
        public event ShowMessageDelegate ChangeTextBoxValue;
        public Form2()
        {
            InitializeComponent();
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            //WindowsFormsWCFHostClient.Client.Service01Client client = new Client.Service01Client();
            //client.ReceiveMessageEvent += ShowMessage;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //事件在Form1中创建Form2后绑定为Form1中操作更新UI的方法
            if (this.ChangeTextBoxValue!=null)
            {
                //执行Form1中操作更新UI的方法
                this.ChangeTextBoxValue(this.richTextBox1.Text);
            }
        }

        public void ShowMessage_Form1ToForm2(string message)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.Invoke(new ShowMessageDelegate(ShowMessage_Form1ToForm2), new object[] { message });
            }
            else
            {
                this.richTextBox1.AppendText(message + "\n");
            }
        }

        public delegate void ShowMessageDelegate(string message);

    }
}
