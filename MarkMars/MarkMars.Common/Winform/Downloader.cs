using System;
using System.IO;
using System.Net;

namespace MarkMars.Common
{
    public class Downloader
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

        private readonly Int32 bufferSize = 1024 * 1024; //1M

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
        /// <param name="url">文件的 URL。</param>
        /// <param name="destinationFilePath">目标路径。</param>
        /// <param name="destinationFileName">目标文件名。</param>
        /// <returns>下载文件大小。</returns>
        public Int64 Download(String url, String destinationPath, String destinationFileName)
        {
            if (!destinationPath.EndsWith("\\"))
            {
                destinationPath += "\\";
            }

            try
            {
                String destinationFileFullName = destinationPath + destinationFileName;
                if (File.Exists(destinationFileFullName))
                {
                    File.SetAttributes(destinationFileFullName, FileAttributes.Normal);
                    File.Delete(destinationFileFullName);
                }

                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Referer = url.Substring(0, url.LastIndexOf("/") + 1);
                httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.2;)";
                WebResponse webResponse = httpWebRequest.GetResponse();
                Int64 fileSize = webResponse.ContentLength;
                Stream stream = webResponse.GetResponseStream();
                FileStream fs = new FileStream(destinationFileFullName, FileMode.Create);
                Byte[] buffer = new Byte[this.bufferSize];
                Int32 readBytes = stream.Read(buffer, 0, this.bufferSize);
                while (readBytes != 0)
                {
                    fs.Write(buffer, 0, readBytes);
                    readBytes = stream.Read(buffer, 0, this.bufferSize);
                }

                fs.Flush();
                fs.Close();
                fs.Dispose();
                stream.Close();
                stream.Dispose();
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
                    this.OnDownloadFailed(this, new DownloadFailedEventArgs(String.Format("下载 {0} 错误！\n{1}", url.Substring(url.LastIndexOf("/") + 1), ex.Message)));
                }

                return -1;
            }
        }
    }
}