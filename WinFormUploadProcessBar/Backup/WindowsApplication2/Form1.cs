using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WindowsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string ftpServerIP = "";
        string ftpUserID = "";
        string ftpPassword = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            this.label2.Text = Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString();
            ftpServerIP = Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString();
        }
        public void httpDownload(string URL, string filename, System.Windows.Forms.ProgressBar prog, System.Windows.Forms.Label label1)
        {
            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[2048];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    //System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    label1.Text = "当前补丁下载进度" + percent.ToString() + "%";
                    Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                so.Close();
                st.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        private void ftpDownload(string filePath, string fileName, ProgressBar prog)
        {
            FtpWebRequest reqFTP,ftpsize;
            
            float percent = 0;

            try
            {
                FileStream outputStream = new FileStream(fileName, FileMode.Create);
                ftpsize = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + filePath));
                ftpsize.UseBinary = true;
                ftpsize.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + filePath));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                ftpsize.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse re = (FtpWebResponse)ftpsize.GetResponse();
                long totalBytes = re.ContentLength;
                re.Close();

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                    
                }
                long totalDownloadedByte = 0;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    totalDownloadedByte = readCount + totalDownloadedByte;
                    //Application.DoEvents();

                    outputStream.Write(buffer, 0, readCount);

                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }

                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    label1.Text = "当前补丁下载进度" + percent.ToString() + "%";
                    Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void UPloadFile(string filepath, ProgressBar prog)
        {
            FtpWebRequest reqFTP;
            float percent = 0;
            try
            {
                FileInfo finfo = new FileInfo(filepath);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://"+ ftpServerIP + "/" + finfo.Name));
                reqFTP.KeepAlive = false;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);//用户，密码
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;//向服务器发出下载请求命令
                reqFTP.ContentLength = finfo.Length;//为request指定上传文件的大小

                WebResponse response = reqFTP.GetResponse();

                reqFTP.ContentLength = finfo.Length;
                int buffLength = 1024;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = finfo.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                int allbye = (int)finfo.Length;
                if (prog != null)
                {
                    prog.Maximum = (int)allbye;

                }
                int startbye = 0;
                while (contentLen != 0)
                {
                    startbye = contentLen + startbye;
                    strm.Write(buff, 0, contentLen);

                    if (prog != null)
                    {
                        prog.Value = (int)startbye;
                    }
                    contentLen = fs.Read(buff, 0, buffLength);
                    percent = (float)startbye / (float)allbye * 100;
                    label1.Text = "当前补丁下载进度" + percent.ToString() + "%";
                    Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                strm.Close();
                fs.Close();
                response.Close();
                MessageBox.Show("文件上传成功！");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(System.Windows.Forms.Form.ActiveForm, "上传失败，原因:" + ex.Message);
                return;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            httpDownload("http://" + ftpServerIP + "/crm.rar", Application.StartupPath + "/downloads/crm1.rar", progressBar1, label1);
            MessageBox.Show("下载完毕！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ftpDownload("crm.rar", Application.StartupPath + "/downloads/crm2.rar", progressBar1);
            MessageBox.Show("下载完毕！");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.InitialDirectory = Application.StartupPath;
            op.RestoreDirectory = true;
            op.Filter = "压缩文件(*.zip)|*.zip|压缩文件(*.rar)|*.rar|所有文件(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                string aa = op.FileName;
                UPloadFile(aa,progressBar1);
            }
            

        }

    }
}