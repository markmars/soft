using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace MarkMars.Common
{
    public class Http
    {
        public class DownloadFailedEventArgs : EventArgs
        {
            private String failedMessage = String.Empty;

            public DownloadFailedEventArgs(String failedMessage)
            {
                this.failedMessage = failedMessage;
            }

            public String FailedMessage
            {
                get { return this.failedMessage; }
            }
        }

        public delegate void DownloadCompletedEventHandler(Object sender, EventArgs e);
        public delegate void DownloadFailedEventHandler(Object sender, DownloadFailedEventArgs e);
        public event DownloadCompletedEventHandler OnDownloadCompleted;
        public event DownloadFailedEventHandler OnDownloadFailed;

        private readonly Int32 bufferSize = 1024 * 128; //128K
        private readonly Int32 threadSleepTime = 500; //500毫秒
        private readonly String tempPath = "Temp";

        /// <summary>
        /// 获取文件大小。
        /// </summary>
        /// <param name="url">文件的 URL。</param>
        /// <returns>文件大小。</returns>
        public Int64 GetFileSize(String url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Referer = url.Substring(0, url.LastIndexOf("/") + 1);
                httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.2;)";
                WebResponse webResponse = httpWebRequest.GetResponse();
                Int64 fileSize = webResponse.ContentLength;
                webResponse.Close();
                httpWebRequest.Abort();
                httpWebRequest = null;

                if (this.OnDownloadCompleted != null)
                {
                    this.OnDownloadCompleted(this, new EventArgs());
                }

                return fileSize;
            }
            catch (Exception ex)
            {
                if (this.OnDownloadFailed != null)
                {
                    this.OnDownloadFailed(this, new DownloadFailedEventArgs(String.Format("获取 {0} 文件大小出错！\n{1}", url.Substring(url.LastIndexOf("/") + 1), ex.Message)));
                }

                return -1;
            }
        }

        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="url">待下载文件的 URL。</param>
        /// <param name="destinationFilePath">目标路径。</param>
        /// <param name="destinationFileName">目标文件名。</param>
        /// <returns>文件大小。</returns>
        public Int64 Download(String url, String destinationPath, String destinationFileName)
        {
            return this.MultiThreadedDownloads(url, destinationPath, destinationFileName, 1);
        }

        /// <summary>
        /// 多线程下载文件。
        /// </summary>
        /// <param name="url">待下载文件的 URL。</param>
        /// <param name="destinationFilePath">目标路径。</param>
        /// <param name="destinationFileName">目标文件名。</param>
        /// <param name="threadCount">线程数。</param>
        /// <returns>文件大小。</returns>
        public Int64 MultiThreadedDownloads(String url, String destinationPath, String destinationFileName, Int32 threadCount)
        {
            if (!destinationPath.EndsWith("\\"))
            {
                destinationPath += "\\";
            }

            String destinationFileFullName = destinationPath + destinationFileName;
            Int64 fileSize = 0;

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Referer = url.Substring(0, url.LastIndexOf("/") + 1);
                httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.2;)";
                WebResponse webResponse = httpWebRequest.GetResponse();
                fileSize = webResponse.ContentLength; //取得目标文件的长度 
                webResponse.Close();
                httpWebRequest.Abort();
                httpWebRequest = null;
            }
            catch (Exception ex)
            {
                if (this.OnDownloadFailed != null)
                {
                    this.OnDownloadFailed(this, new DownloadFailedEventArgs(String.Format("下载 {0} 错误！\n{1}", url.Substring(url.LastIndexOf("/") + 1), ex.Message)));
                }

                return -1;
            }

            //根据线程数初始化数组
            Boolean[] threadIsEnd = new Boolean[threadCount]; //线程接收文件是否结束标志
            String[] threadFileName = new String[threadCount]; //线程接收文件的文件名
            Int64[] threadFileStart = new Int64[threadCount]; //线程接收文件的起始位置
            Int64[] threadFileSize = new Int64[threadCount]; //线程接收文件的大小
            String[] threadErrorMessage = new String[threadCount]; //线程接收文件的失败信息（空字符串表示接收成功）
            Int32[] threadRetryTimes = new Int32[threadCount]; //线程接收失败后重试次数
            Int64 sizePerThread = fileSize / threadCount; //平均分配每个线程应该接收文件的大小
            String tempFullPath = String.Format(@"{0}\{1}\", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), this.tempPath);
            String tempFileName = Guid.NewGuid().ToString();

            if (!Directory.Exists(tempFullPath))
            {
                Directory.CreateDirectory(tempFullPath);
            }

            //为数组赋值 
            for (Int32 idx = 0; idx < threadCount; idx++)
            {
                threadIsEnd[idx] = false; //线程结束标志的初始值为假
                
                if (idx == 0)
                {
                    threadFileName[idx] = destinationFileFullName; //第一个线程接收文件直接保存到目标文件
                }
                else
                {
                    threadFileName[idx] = String.Format("{0}{1}{2}.dat", tempFullPath, tempFileName, idx); //线程接收文件的临时文件名
                }

                threadFileStart[idx] = sizePerThread * idx; //线程接收文件的起始点
                threadErrorMessage[idx] = String.Empty;
                threadRetryTimes[idx] = 0;

                if (idx == threadCount - 1)
                {
                    threadFileSize[idx] = sizePerThread + fileSize % threadCount; //线程接收文件的长度
                }
                else
                {
                    threadFileSize[idx] = sizePerThread; //线程接收文件的长度
                }
            }

            Thread t = null;
            Object[] paramList = null;
            for (Int32 idx = 0; idx < threadCount; idx++)
            {
                t = new Thread(new ParameterizedThreadStart(this.Receive));
                paramList = new Object[10];
                paramList[0] = idx;
                paramList[1] = url;
                paramList[2] = threadFileName;
                paramList[3] = threadFileStart;
                paramList[4] = threadFileSize;
                paramList[5] = threadIsEnd;
                paramList[6] = threadCount;
                paramList[7] = destinationFileFullName;
                paramList[8] = threadErrorMessage;
                paramList[9] = threadRetryTimes;
                t.Start(paramList);
            }

            return fileSize;
        }

        /// <summary>
        /// 接收线程。
        /// </summary>
        /// <param name="o"></param>
        private void Receive(Object o)
        {
            Object[] paramList = o as Object[];
            Int32 currentThreadIndex = Convert.ToInt32(paramList[0]);
            String url = paramList[1].ToString();
            String[] threadFileName = paramList[2] as String[];
            Int64[] threadFileStart = paramList[3] as Int64[];
            Int64[] threadFileSize = paramList[4] as Int64[];
            Boolean[] threadIsEnd = paramList[5] as Boolean[];
            String[] threadErrorMessage = paramList[8] as String[];
            Int32[] threadRetryTimes = paramList[9] as Int32[];
            Byte[] bytes = new Byte[this.bufferSize];
            Int32 readBytes = 0;
            FileStream fs = new FileStream(threadFileName[currentThreadIndex], FileMode.Create, FileAccess.Write);
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Referer = url.Substring(0, url.LastIndexOf("/") + 1);
                httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.2;)";
                //接收的起始位置及接收的长度
                httpWebRequest.AddRange(Convert.ToInt32(threadFileStart[currentThreadIndex]), Convert.ToInt32(threadFileStart[currentThreadIndex] + threadFileSize[currentThreadIndex] - 1));
                WebResponse webResponse = httpWebRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();//获得接收流
                readBytes = stream.Read(bytes, 0, this.bufferSize);
                while (readBytes > 0)
                {
                    fs.Write(bytes, 0, readBytes);
                    readBytes = stream.Read(bytes, 0, this.bufferSize);
                }

                fs.Flush();
                fs.Close();
                fs.Dispose();
                stream.Close();
                stream.Dispose();
                webResponse.Close();
                httpWebRequest.Abort();
                httpWebRequest = null;
            }
            catch (Exception ex)
            {
                fs.Close();
                fs.Dispose();

                if (threadRetryTimes[currentThreadIndex] < 3) //最多重试3次
                {
                    threadRetryTimes[currentThreadIndex]++;

                    Thread.Sleep(this.threadSleepTime);
                    Thread t = new Thread(new ParameterizedThreadStart(this.Receive));
                    t.Start(paramList);

                    return;
                }
                else
                {
                    threadErrorMessage[currentThreadIndex] = ex.Message;
                }
            }

            threadIsEnd[currentThreadIndex] = true;
            Int32 threadCount = Convert.ToInt32(paramList[6]);
            Boolean allThreadIsEnd = true;
            String message = String.Empty;
            for (Int32 idx = 0; idx < threadCount; idx++)
            {
                if (!threadIsEnd[idx])//有未结束线程，等待
                {
                    allThreadIsEnd = false;
                    break;
                }

                if (!String.IsNullOrEmpty(threadErrorMessage[idx]))
                {
                    message = threadErrorMessage[idx];
                }
            }

            if (allThreadIsEnd)
            {
                if (String.IsNullOrEmpty(message))
                {
                    //合并各线程接收的文件
                    this.MergeFiles(threadCount, Convert.ToString(paramList[7]), threadFileName);
                }
                else
                {
                    if (this.OnDownloadFailed != null)
                    {
                        this.OnDownloadFailed(this, new DownloadFailedEventArgs(String.Format("下载 {0} 错误！\n{1}", url.Substring(url.LastIndexOf("/") + 1), message)));
                    }
                }
            }
        }

        /// <summary>
        /// 合并文件。
        /// </summary>
        private void MergeFiles(Int32 threadCount, String destinationFileFullName, String[] threadFileName)
        {
            FileStream fs = new FileStream(destinationFileFullName, FileMode.Append, FileAccess.Write);
            FileStream fsTemp = null;
            Int32 readBytes;
            Byte[] bytes = new Byte[this.bufferSize];
            for (Int32 idx = 1; idx < threadCount; idx++) //从第二个线程接收的临时文件开始合并
            {
                fsTemp = new FileStream(threadFileName[idx], FileMode.Open, FileAccess.Read);
                readBytes = fsTemp.Read(bytes, 0, this.bufferSize);

                while (readBytes > 0)
                {
                    fs.Write(bytes, 0, readBytes);
                    readBytes = fsTemp.Read(bytes, 0, this.bufferSize);
                }

                fsTemp.Close();
                File.Delete(threadFileName[idx]);
            }

            fs.Close();
            fs.Dispose();

            if (this.OnDownloadCompleted != null)
            {
                this.OnDownloadCompleted(this, new EventArgs());
            }
        }
    }
}