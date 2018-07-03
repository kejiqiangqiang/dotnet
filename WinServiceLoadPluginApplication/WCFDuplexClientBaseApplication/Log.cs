using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 写异步日志
    /// </summary>
    internal class Log
    {
        //记录系统日志的类
        private Thread threadWriteLog = null;
        private Thread threadReadLog = null;

        private static Mutex mutex = null;

        public string msg = "";
        public string DebugMsg = "";
        public DateTime dte = DateTime.Now;
        public string fName = "";
        public ArrayList sLog = null;
        private string folderPath = "";

        /// <summary>
        /// 写异步日志
        /// </summary>
        public Log(string folderPath)
        {
            this.folderPath = folderPath;
            mutex = new Mutex();
            threadWriteLog = new Thread(new ThreadStart(WriteLog));
            threadReadLog = new Thread(new ThreadStart(ReadLog));

            threadWriteLog.IsBackground = true;
            threadReadLog.IsBackground = true;

            threadWriteLog.Start();
            threadReadLog.Start();
        }

        private void ReleaseMutex()
        {
            try
            {
                mutex.ReleaseMutex();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 互斥锁
        /// </summary>
        public void WriteLog()
        {
            lock (this)
            {
                try
                {
                    mutex.WaitOne();
                    if (!string.IsNullOrEmpty(msg))
                    {
                        WriteLog(msg);
                    }
                }
                catch
                {
                }
                finally
                {
                    this.ReleaseMutex();
                }
            }
        }
        public void ReadLog()
        {
            lock (this)
            {
                try
                {
                    mutex.WaitOne();
                    sLog = ReadLog(fName);
                }
                finally
                {
                    this.ReleaseMutex();
                }
            }
        }

        private void WriteLog(string msg)
        {
            dte = DateTime.Now;
            string folder = folderPath;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string FileName = folder + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            //判断是否存在文件
            if (!File.Exists(FileName))
            {
                //如果文件不存在，则创建文件后向文件添加日志                
                StreamWriter sw = File.CreateText(FileName);
                if (msg != "")
                {
                    sw.WriteLine(dte.ToString("yyyy-MM-dd HH:mm:ss.fff") + "         " + msg);
                }
                sw.Close();
                sw.Dispose();
            }
            else
            {
                //如果存在文件，则向文件添加日志
                using (StreamWriter sr = new StreamWriter(FileName, true))
                {
                    if (msg != "")
                    {
                        sr.WriteLine(dte.ToString("yyyy-MM-dd HH:mm:ss.fff") + "         " + msg);
                    }
                    sr.Close();
                    sr.Dispose();
                }
            }
        }

        private ArrayList ReadLog(string fName)
        {
            string folder = this.folderPath;
            folder = folder.Substring(0, folder.LastIndexOf("\\") + 1) + "Log";
            string fn = folder + "\\" + fName + ".txt";
            ArrayList arr = new ArrayList();
            //判断是否存在文件
            if (File.Exists(fn))
            {
                //如果存在文件，则向文件添加日志                
                StreamReader sw = new StreamReader(fn, true);
                while (sw.Peek() != -1)
                {
                    arr.Add(sw.ReadLine());
                }
                sw.Close();
                sw.Dispose();
            }
            return arr;
        }

        public void Close()
        {
            threadWriteLog.Abort();
            threadReadLog.Abort();
        }
    }
}