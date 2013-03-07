using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MarkMars.Common
{
    public class Ftp
    {
        /// <summary>
        /// 文件信息结构。
        /// </summary>
        public struct FileStruct
        {
            public String Flags;
            public String Owner;
            public String Group;
            public Boolean IsDirectory;
            public DateTime CreateTime;
            public String Name;
        }

        /// <summary>
        /// 文件列表类型。
        /// </summary>
        public enum FileListStyle
        {
            UnixStyle,
            WindowsStyle,
            Unknown
        }

        /// <summary>
        /// 主机。
        /// </summary>
        String ftpServerIP;

        /// <summary>
        /// 用户名。
        /// </summary>
        String ftpUserID;

        /// <summary>
        /// 用户密码。
        /// </summary>
        String ftpPassword;

        /// <summary>
        /// 是否使用被动模式。
        /// </summary>
        Boolean usePassive;

        /// <summary>
        /// 编码方式。
        /// </summary>
        Encoding ftpEncoding;

        FtpWebRequest reqFTP;

        /// <summary>
        /// FTP 根路径。
        /// </summary>
        String ftpRootPath = String.Empty;

        /// <summary>
        /// 获取 FTP 的根路径。
        /// </summary>
        public String FTPRootPath
        {
            get { return "ftp://" + this.ftpServerIP + "/"; }
        }

        /// <summary>
        /// FTP 当前路径。
        /// </summary>
        String ftpCurrentPath = String.Empty;

        /// <summary>
        /// 获取或设置 FTP 的当前路径。
        /// </summary>
        public String FTPCurrentPath
        {
            set
            {
                this.ftpCurrentPath = value;
                if (this.ftpCurrentPath.Substring(this.ftpCurrentPath.Length - 1) != "/")
                {
                    this.ftpCurrentPath += "/";
                }
            }
            get
            {
                if (String.IsNullOrEmpty(this.ftpCurrentPath))
                {
                    return this.FTPRootPath;
                }
                else
                {
                    return this.ftpCurrentPath;
                }
            }
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        public Ftp(String ftpServerIP, String ftpUserID, String ftpPassword)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
            this.usePassive = true;
            this.ftpEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        public Ftp(String ftpServerIP, String ftpUserID, String ftpPassword, Boolean usePassive)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
            this.usePassive = usePassive;
            this.ftpEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        public Ftp(String ftpServerIP, String ftpUserID, String ftpPassword, Encoding ftpEncoding)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
            this.usePassive = true;
            this.ftpEncoding = ftpEncoding;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        public Ftp(String ftpServerIP, String ftpUserID, String ftpPassword, Boolean usePassive, Encoding ftpEncoding)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
            this.usePassive = usePassive;
            this.ftpEncoding = ftpEncoding;
        }

        /// <summary>
        /// 连接Ftp。
        /// </summary>
        /// <param name="path"></param>
        private void Connect(String path)
        {
            this.reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            this.reqFTP.UsePassive = this.usePassive;
            this.reqFTP.UseBinary = true;
            this.reqFTP.KeepAlive = false;
            this.reqFTP.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
        }

        /// <summary>
        /// 获取文件列表。
        /// </summary>
        /// <param name="fileList">文件列表。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String GetFileList(out String[] fileList)
        {
            fileList = new String[0];
            FileStruct[] fileAndDirectoryList;
            String errMsg = this.GetFileAndDirectoryStructList(String.Empty, out fileAndDirectoryList);
            if (!String.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            return this.GetFileOrDirectoryListByFileStructList(fileAndDirectoryList, false, out fileList);
        }

        /// <summary>
        /// 获取文件列表。
        /// </summary>
        /// <param name="path">获取文件的路径。</param>
        /// <param name="fileList">文件列表。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String GetFileList(String path, out String[] fileList)
        {
            fileList = new String[0];
            FileStruct[] fileAndDirectoryList;
            String errMsg = this.GetFileAndDirectoryStructList(path, out fileAndDirectoryList);
            if (!String.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            return this.GetFileOrDirectoryListByFileStructList(fileAndDirectoryList, false, out fileList);
        }

        /// <summary>
        /// 获取目录列表。
        /// </summary>
        /// <param name="directoryList">目录列表。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String GetDirectoryList(out String[] directoryList)
        {
            directoryList = new String[0];
            FileStruct[] fileAndDirectoryList;
            String errMsg = this.GetFileAndDirectoryStructList(String.Empty, out fileAndDirectoryList);
            if (!String.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            return this.GetFileOrDirectoryListByFileStructList(fileAndDirectoryList, true, out directoryList);
        }

        /// <summary>
        /// 获取目录列表。
        /// </summary>
        /// <param name="path">获取目录的路径。</param>
        /// <param name="directoryList">目录列表。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String GetDirectoryList(String path, out String[] directoryList)
        {
            directoryList = new String[0];
            FileStruct[] fileAndDirectoryList;
            String errMsg = this.GetFileAndDirectoryStructList(path, out fileAndDirectoryList);
            if (!String.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            return this.GetFileOrDirectoryListByFileStructList(fileAndDirectoryList, true, out directoryList);
        }

        /// <summary>
        /// 获取文件和目录结构列表。
        /// </summary>
        /// <param name="fileAndDirectoryList">文件和目录结构列表。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        private String GetFileAndDirectoryStructList(out FileStruct[] fileAndDirectoryList)
        {
            return this.GetFileAndDirectoryStructList(String.Empty, out fileAndDirectoryList);
        }

        /// <summary>
        /// 获取文件和目录结构列表。
        /// </summary>
        /// <param name="path">获取文件和目录的路径。</param>
        /// <param name="fileAndDirectoryList">文件和目录结构列表。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        private String GetFileAndDirectoryStructList(String path, out FileStruct[] fileAndDirectoryList)
        {
            fileAndDirectoryList = new FileStruct[0];
            StringBuilder result = new StringBuilder();
            try
            {
                WebResponse response = null;
                try
                {
                    if (!path.EndsWith("/"))
                    {
                        path += "/";
                    }
                    this.Connect(this.FTPCurrentPath + path);
                    reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    response = reqFTP.GetResponse();
                }
                catch (Exception ex)
                {
                    return "FTP连接失败！\n" + ex.Message;
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), this.ftpEncoding);
                String line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                if (result.Length == 0)
                {
                    return String.Empty;
                }

                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                reader.Dispose();
                response.Close();
                fileAndDirectoryList = GetListByDataString(result.ToString());
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "获取失败！\n" + ex.Message;
            }
        }

        /// <summary> 
        /// 根据 FTP 返回的列表字符信息获得文件和目录结构列表。
        /// </summary>
        /// <param name="dataString">FTP 返回的列表字符信息。</param>
        /// <returns>文件和目录结构列表。</returns>
        private FileStruct[] GetListByDataString(String dataString)
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            String[] dataRecords = dataString.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
            foreach (String s in dataRecords)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct();
                    f.Name = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            f = ParseFileStructFromUnixStyleRecord(s);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (!(f.Name == "." || f.Name == ".."))
                    {
                        myListArray.Add(f);
                    }
                }
            }
            return myListArray.ToArray();
        }

        /// <summary>
        /// 从 Windows 格式中返回文件结构信息。
        /// </summary>
        /// <param name="Record">文件信息。</param>
        /// <returns>文件结构。</returns>
        private FileStruct ParseFileStructFromWindowsStyleRecord(String Record)
        {
            FileStruct f = new FileStruct();
            String processstr = Record.Trim();
            String dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            String timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == " <DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                String[] strs = processstr.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);  // true); 
                processstr = strs[1];
                f.IsDirectory = false;
            }
            f.Name = processstr;
            return f;
        }

        /// <summary> 
        /// 判断文件列表的方式 Window 方式还是 Unix 方式。
        /// </summary>
        /// <param name="recordList">文件信息列表。</param>
        /// <returns>文件列表类型。</returns>
        private FileListStyle GuessFileListStyle(String[] recordList)
        {
            foreach (String s in recordList)
            {
                if (s.Length > 10 && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return FileListStyle.UnixStyle;
                }
                else if (s.Length > 8 && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }

            return FileListStyle.Unknown;
        }

        /// <summary>
        /// 从 Unix 格式中返回文件结构信息。
        /// </summary>
        /// <param name="Record">文件信息。</param>
        /// <returns>文件结构。</returns>
        private FileStruct ParseFileStructFromUnixStyleRecord(String Record)
        {
            FileStruct f = new FileStruct();
            String processstr = Record.Trim();
            f.Flags = processstr.Substring(0, 10);
            f.IsDirectory = (f.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();
            CutSubstringFromStringWithTrim(ref processstr, ' ', 0);  //跳过一部分 
            f.Owner = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Group = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            CutSubstringFromStringWithTrim(ref processstr, ' ', 0);  //跳过一部分 
            String yearOrTime = processstr.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
            if (yearOrTime.IndexOf(":") >= 0)  //time 
            {
                processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
            }

            //修改人：Dengt 修改时间：2012-05-16 有的FTP服务器返回的报文的文件名列有可能为空，导致转换出错，这种记录直接忽略掉（即将文件名改为“.”）。
            try
            {
                f.CreateTime = DateTime.Parse(CutSubstringFromStringWithTrim(ref processstr, ' ', 8));
                f.Name = processstr;  //最后就是名称 
            }
            catch
            {
                f.CreateTime = DateTime.MaxValue;
                f.Name = ".";
            }

            return f;
        }

        /// <summary>
        /// 按照一定的规则进行字符串截取。
        /// </summary>
        /// <param name="s">截取的字符串。</param>
        /// <param name="c">查找的字符。</param>
        /// <param name="startIndex">查找的位置。</param>
        private String CutSubstringFromStringWithTrim(ref String s, Char c, Int32 startIndex)
        {
            Int32 pos1 = s.IndexOf(c, startIndex);
            String retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }

        /// <summary>
        /// 根据文件和目录结构列表获取文件或目录列表。
        /// </summary>
        /// <param name="fileAndDirectoryList">文件和目录结构列表。</param>
        /// <param name="isDirectory">是否获取目录列表。</param>
        /// <param name="fileOrDirectoryList">文件或目录列表。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        private String GetFileOrDirectoryListByFileStructList(FileStruct[] fileAndDirectoryList, Boolean isDirectory, out String[] fileOrDirectoryList)
        {
            List<String> fileNameList = new List<String>();
            foreach (FileStruct file in fileAndDirectoryList)
            {
                if (file.IsDirectory == isDirectory)
                {
                    fileNameList.Add(file.Name);
                }
            }

            fileOrDirectoryList = new String[fileNameList.Count];
            fileNameList.CopyTo(fileOrDirectoryList);
            return String.Empty;
        }

        /// <summary>
        /// 文件上传。
        /// </summary>
        /// <param name="fileFullName">要上传的文件全名（包含文件路径）。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String Upload(String fileFullName)
        {
            return this.Upload(fileFullName, Path.GetFileName(fileFullName));
        }

        /// <summary>
        /// 文件上传。
        /// </summary>
        /// <param name="fileFullName">要上传的文件全名（包含文件路径）。</param>
        /// <param name="newFileName">上传后的文件名（不包含文件路径）。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String Upload(String fileFullName, String newFileName)
        {
            FileInfo fileInfo = new FileInfo(fileFullName);
            String uri = this.FTPCurrentPath + newFileName;
            try
            {
                this.Connect(uri);//连接
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                // 上传文件时通知服务器文件的大小
                reqFTP.ContentLength = fileInfo.Length;
                // 缓冲大小设置为kb 
                Int32 buffLength = 2048;
                Byte[] buff = new Byte[buffLength];
                Int32 contentLen;
                // 打开一个文件流(System.IO.FileStream) 去读上传的文件
                FileStream fs = fileInfo.OpenRead();
                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();
                // 每次读文件流的kb
                contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入upload stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流
                strm.Flush();
                strm.Close();
                strm.Dispose();
                fs.Close();
                fs.Dispose();

                long fileSize = 0;
                String errMsg = this.GetFileSize(newFileName, out fileSize);
                if (!String.IsNullOrEmpty(errMsg))
                {
                    return errMsg;
                }

                if (fileInfo.Length != fileSize)
                {
                    return "文件上传不完整！";
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "文件上传失败！\n" + ex.Message;
            }
        }

        /// <summary>
        /// 文件下载。
        /// </summary>
        /// <param name="destinationFilePath">目标路径。</param>
        /// <param name="destinationFileName">目标文件名。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String Download(String destinationPath, String destinationFileName)
        {
            return this.Download(Path.GetFileName(destinationFileName), destinationPath, destinationFileName);
        }

        /// <summary>
        /// 文件下载。
        /// </summary>
        /// <param name="destinationFileName">源文件名。</param>
        /// <param name="destinationFilePath">目标路径。</param>
        /// <param name="destinationFileName">目标文件名。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String Download(String sourceFileName, String destinationPath, String destinationFileName)
        {
            try
            {
                String onlyFileName = Path.GetFileName(destinationFileName);
                if (!destinationPath.EndsWith("\\"))
                {
                    destinationPath += "\\";
                }
                String newFileName = destinationPath + onlyFileName;
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                if (File.Exists(newFileName))
                {
                    File.SetAttributes(newFileName, FileAttributes.Normal);
                    File.Delete(newFileName);
                }

                String url = this.FTPCurrentPath + sourceFileName;
                FtpWebResponse response = null;
                try
                {
                    this.Connect(url);//连接 
                    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                    response = (FtpWebResponse)reqFTP.GetResponse();
                }
                catch (Exception ex)
                {
                    return "FTP连接失败！\n" + ex.Message;
                }

                Stream ftpStream = response.GetResponseStream();
                Int32 bufferSize = 2048;
                Int32 readCount;
                Byte[] buffer = new Byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                ftpStream.Dispose();
                outputStream.Flush();
                outputStream.Close();
                outputStream.Dispose();
                response.Close();

                long fileSize = 0;
                String errMsg = this.GetFileSize(sourceFileName, out fileSize);
                if (!String.IsNullOrEmpty(errMsg))
                {
                    return errMsg;
                }

                FileInfo fileInfo = new FileInfo(newFileName);
                if (fileInfo.Length != fileSize)
                {
                    return "文件下载不完整！";
                }

                return String.Empty;
            }
            catch (Exception ex)
            {
                return "文件下载失败！\n" + ex.Message;
            }
        }

        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String DeleteFile(String fileName)
        {
            try
            {
                if (fileName.StartsWith("/"))
                {
                    fileName.Substring(1);
                }

                String uri = this.FTPCurrentPath + fileName;
                this.Connect(uri);//连接 
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "删除失败！\n" + ex.Message;
            }
        }

        /// <summary>
        /// 创建目录。
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String MakeDir(String dirName)
        {
            try
            {
                if (dirName.StartsWith("/"))
                {
                    dirName.Substring(1);
                }

                String uri = this.FTPCurrentPath + dirName;
                this.Connect(uri);//连接 
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "创建目录失败！\n" + ex.Message;
            }
        }

        /// <summary>
        /// 删除目录。
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String DeleteDir(String dirName)
        {
            try
            {
                if (dirName.StartsWith("/"))
                {
                    dirName.Substring(1);
                }

                String uri = this.FTPCurrentPath + dirName;
                this.Connect(uri);//连接 
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "删除目录失败！\n" + ex.Message;
            }
        }

        /// <summary>
        /// 获取文件大小。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public String GetFileSize(String filename, out long fileSize)
        {
            fileSize = 0;
            try
            {
                String uri = this.FTPCurrentPath + filename;
                this.Connect(uri);//连接 
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                fileSize = response.ContentLength;
                response.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "获得文件大小失败！\n" + ex.Message;
            }
        }

        /// <summary>
        /// 文件重命名。
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String Rename(String currentFilename, String newFilename)
        {
            try
            {
                if (currentFilename.StartsWith("/"))
                {
                    currentFilename.Substring(1);
                }

                if (newFilename.StartsWith("/"))
                {
                    newFilename = newFilename.Substring(1);
                }

                String uri = this.FTPCurrentPath + currentFilename;
                this.Connect(uri);//连接 
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = String.Format("/{0}{1}", this.FTPCurrentPath.Replace(this.FTPRootPath, ""), newFilename);

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                response.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "文件重命名失败！\n" + ex.Message;
            }
        }

        /// <summary>
        /// 检查文件是否存在。
        /// </summary>
        /// <param name="fileName">要检查的文件名。</param>
        /// <param name="isExists">是否存在。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String CheckFileExists(String fileName, out Boolean isExists)
        {
            return this.CheckFileOrDirectoryExists(0, String.Empty, fileName, out isExists);
        }

        /// <summary>
        /// 检查文件是否存在。
        /// </summary>
        /// <param name="path">检查的路径。</param>
        /// <param name="directoryName">要检查的文件名。</param>
        /// <param name="isExists">是否存在。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String CheckFileExists(String path, String fileName, out Boolean isExists)
        {
            return this.CheckFileOrDirectoryExists(0, path, fileName, out isExists);
        }

        /// <summary>
        /// 检查目录是否存在。
        /// </summary>
        /// <param name="directoryName">要检查的目录名。</param>
        /// <param name="isExists">是否存在。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String CheckDirectoryExists(String directoryName, out Boolean isExists)
        {
            return this.CheckFileOrDirectoryExists(1, String.Empty, directoryName, out isExists);
        }

        /// <summary>
        /// 检查目录是否存在。
        /// </summary>
        /// <param name="path">检查的路径。</param>
        /// <param name="directoryName">要检查的目录名。</param>
        /// <param name="isExists">是否存在。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        public String CheckDirectoryExists(String path, String directoryName, out Boolean isExists)
        {
            return this.CheckFileOrDirectoryExists(1, path, directoryName, out isExists);
        }

        /// <summary>
        /// 检查文件或目录是否存在。
        /// </summary>
        /// <param name="type">类型（0：文件；1：目录）。</param>
        /// <param name="path">检查的路径。</param>
        /// <param name="fileOrDirectoryName">要检查的文件或目录名。</param>
        /// <param name="isExists">是否存在。</param>
        /// <returns>操作失败信息（空字符串表示操作成功）。</returns>
        private String CheckFileOrDirectoryExists(Byte type, String path, String fileOrDirectoryName, out Boolean isExists)
        {
            String[] fileOrDirectoryList;
            String errMsg = String.Empty;
            isExists = false;

            if (type == 0)
            {
                errMsg = this.GetFileList(path, out fileOrDirectoryList);
            }
            else
            {
                errMsg = this.GetDirectoryList(path, out fileOrDirectoryList);
            }

            if (!String.IsNullOrEmpty(errMsg))
            {
                return errMsg;
            }

            foreach (String fileOrDirectory in fileOrDirectoryList)
            {
                if (fileOrDirectory.ToUpper() == fileOrDirectoryName.ToUpper())
                {
                    isExists = true;
                    break;
                }
            }

            return String.Empty;
        }
    }
}