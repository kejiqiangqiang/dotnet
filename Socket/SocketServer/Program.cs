using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    /// <summary>
    /// 服务端
    /// </summary>
    class SocketServer
    {
        /// <summary>
        /// 定义与客户端通信的套接字，用于监听客户端发来的消息
        /// </summary>
        static Socket socketWatch;
        /// <summary>
        /// 存储客户端信息
        /// </summary>
        //static Dictionary<string, Socket> clientConnections = new Dictionary<string, Socket>();
        static ConcurrentDictionary<string, Socket> clientConnections = new ConcurrentDictionary<string, Socket>();//thread safe

        static void Main(string[] args)
        {

            //套接字(IP4寻址协议，流式连接，Tcp协议)
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IP
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //EndPoint网络终结点（IP:Port）
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8098);
            //Socket监听绑定的网络终节点
            socketWatch.Bind(endPoint);
            //置于侦听状态（并设置侦听队列长度）
            socketWatch.Listen(20);

            //负责监听客户端的线程:创建一个监听线程
            Thread threadWatch = new Thread(WatchConnection);
            //线程设置为与后台同步，随着主线程结束而结束
            threadWatch.IsBackground = true;
            threadWatch.Start();

            Console.WriteLine("服务端开启监听......");
            Console.WriteLine("点击输入任意键回车退出程序.....");
            Console.ReadKey();
            Console.WriteLine("退出监听，并关闭程序。");
        }

        /// <summary>
        /// 监听客户端发来的请求  
        /// </summary>
        static void WatchConnection()
        {
            Socket conn;

            //监听客户端发送的连接
            while (true)
            {
                try
                {
                    //为监听到的客户端连接创建新的服务端Socket（客户端用的是Socket.Connect(IPEndPoint)）
                    conn = socketWatch.Accept();//线程将阻塞，直到接收到客户端连接，之后往下执行，再次执行循环到此处，继续阻塞等待客户端连接
                }
                catch (Exception ex)
                {
                    //侦听异常
                    Console.WriteLine(ex.Message);
                    break;
                }

                string remoteEndPoint = conn.RemoteEndPoint.ToString();

                //保存客户端信息  
                clientConnections.TryAdd(remoteEndPoint, conn);

                //服务端显示与客户端连接情况
                Console.WriteLine("成功与" + remoteEndPoint + "客户端建立连接！\t\n");

                //获取客户端的IP和端口号
                IPAddress clientIP = (conn.RemoteEndPoint as IPEndPoint).Address;
                int clientPort = (conn.RemoteEndPoint as IPEndPoint).Port;

                //发送数据到客户端
                string sendmsg = "连接服务端成功！\r\n" + "本地IP：" + clientIP + "，本地端口：" + clientPort.ToString()+"\r\n";
                byte[] arr = Encoding.UTF8.GetBytes(sendmsg);
                conn.Send(arr);

                //获取客户网络端终结点
                IPEndPoint netpoint = conn.RemoteEndPoint as IPEndPoint;
                //////

                //创建一个与客户端通信的通信线程
                ParameterizedThreadStart pts = new ParameterizedThreadStart(Receive);
                Thread thread = new Thread(pts);
                //线程设置为与后台同步，随着主线程结束而结束
                thread.IsBackground = true;
                thread.Start(conn);
            }
        }

        /// <summary>
        /// 接收客户端发送的信息
        /// </summary>
        /// <param name="socketClientPara">客户端套接字对象</param>
        static void Receive(object socketClientPara)
        {
            Socket socket = socketClientPara as Socket;
            //监听客户端发送的数据
            while (true)
            {
                //创建一个内存缓冲区，其大小为1024*1024字节  即1M     
                byte[] arrServerRecMsg = new byte[1024 * 1024];
                try
                {
                    //将接收到的客户端信息存入到内存缓冲区
                    int length = socket.Receive(arrServerRecMsg);//线程将阻塞，直到接收到客户端消息，之后往下执行，再次执行循环到此处，继续阻塞等待客户端消息
                    //信息
                    string msg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);//指定解码的字节数，否则导致打印出1M包含空数据的信息

                    Console.WriteLine("客户端：" + socket.RemoteEndPoint.ToString() + "：" + CurrentTime() + "\r\n" + msg + "\r\n");

                    //发送消息至客户端
                    //socket.Send(Encoding.UTF8.GetBytes("测试服务端Server是否可以发送数据给客户端Client\r\n"));
                    socket.Send(arrServerRecMsg.Take(length).ToArray());
                }
                catch (Exception ex)
                {
                    //客户端套接字监听异常
                    Socket s = null;
                    clientConnections.TryRemove(socket.RemoteEndPoint.ToString(), out s);

                    Console.WriteLine("Client Count:" + clientConnections.Count);

                    Console.WriteLine("客户端" + socket.RemoteEndPoint + "已经中断连接" + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                    //关闭之前accept出来的和客户端进行通信的套接字 
                    socket.Close();
                    break;
                }
            }
        }

        static DateTime CurrentTime()
        {
            return DateTime.Now;
        }


    }
}
