using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient
{
    public partial class Main : Form
    {
        //定义一个客户端套接字
        //定义一个负责监听服务端请求的线程
        Socket socketClient = null;
        Thread threadClient = null;
        public Main()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            TextBox.CheckForIllegalCrossThreadCalls = false;
            this.btnSendMsg.Visible = false;
            this.textSendMsg.Visible = false;
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.btnConnect.Enabled = false;
            //创建套接字监听
            this.socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(this.textIP.Text);//127.0.0.1
            IPEndPoint endPoint = new IPEndPoint(ipAddress, int.Parse(this.textPort.Text));//8098

            try
            {
                //客户端套接字连接到服务端网络节点上，用的是Socket.Connect(IPEndPoint)
                this.socketClient.Connect(endPoint);
                this.btnSendMsg.Visible = true;
                this.textSendMsg.Visible = true;
            }
            catch (Exception)
            {
                Debug.WriteLine("连接失败\r\n");
                this.richTextMsg.AppendText("连接失败\r\n");
                return;
            }

            this.threadClient = new Thread(Receive);
            this.threadClient.IsBackground = true;
            this.threadClient.Start();

        }

        /// <summary>
        /// 向服务器发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            this.SendMessage(this.textSendMsg.Text);
            this.textSendMsg.Clear();
        }

        /// <summary>
        /// 接收服务端发送的消息
        /// </summary>
        void Receive()
        {
            //信号量
            int sem = 0;//连接服务器成功后显示服务器发送的连接成功的信息，再将值置为1
            while (true)
            {
                //定义一个1M的内存缓冲区，用于临时性存储接收到的消息
                byte[] arrRecMsg = new byte[1024 * 1024];
                try
                {
                    //将接收到的服务端信息存入到内存缓冲区
                    int length = this.socketClient.Receive(arrRecMsg);//线程将阻塞，直到接收到服务端消息，之后往下执行，再次执行循环到此处，继续阻塞等待服务端消息
                    //信息
                    string msg = Encoding.UTF8.GetString(arrRecMsg,0,length);//指定解码的字节数，否则导致打印出1M包含空数据的信息
                    var now = CurrentTime();
                    if (sem == 1)
                    {
                        var text = "服务器：" + now + "\r\n" + msg + "\r\n";
                        this.richTextMsg.AppendText(text);
                        Debug.WriteLine(text);
                    }
                    else
                    {
                        sem = 1;
                        this.richTextMsg.AppendText(msg + "\r\n");
                        Debug.WriteLine(msg + "\r\n");
                    }
                }
                catch (Exception)
                {
                    this.richTextMsg.AppendText("远程服务器已经中断连接" + "\r\n");
                    Debug.WriteLine("远程服务器已经中断连接" + "\r\n");
                    break;
                }
            }
        }

        void SendMessage(string msg)
        {
            //字符串转换为机器可以识别的字节数组 
            byte[] arrSendMsg = Encoding.UTF8.GetBytes(msg);
            //调用客户端套接字发送字节
            this.socketClient.Send(arrSendMsg);

            var text = "我：" + CurrentTime() + "\r\n" + msg + "\r\n";
            this.richTextMsg.AppendText(text);
            Debug.WriteLine(text);

        }

        static DateTime CurrentTime()
        {
            return DateTime.Now;
        }


    }
}
