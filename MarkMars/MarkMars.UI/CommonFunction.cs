using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MarkMars.UI
{
    public class CommonFunction
    {
        [DllImport("TL_RarUtil")]
        private static extern Int32 TL_RarTest(String ARarFile);

        [DllImport("TL_RarUtil")]
        private static extern int TL_RarTestByPassword(string ARarFile, string APassword);

        #region 公共属性
        public static String WordVersion
        {
            get;
            set;
        }
        #endregion

        #region 压缩、解压方法
        /// <summary>
        /// 解压：将文件解压到某个文件夹中。 注意：要对路径加上双引号，避免带空格的路径，在rar命令中失效
        /// </summary>
        /// <param name="rarRunPath">rar.exe的名称及路径</param>
        /// <param name="fromRarPath">被解压的rar文件路径</param>
        /// <param name="toRarPath">解压后的文件存放路径</param>
        /// <param name="rarName">被解压的rar文件名称</param>
        /// <returns></returns>
        public static String unCompressRAR(String rarRunPath, String fromRarFile, String toRarPath)
        {
            String rar;
            String commandInfo;

            try
            {
                //2011-03-18 Youker 调用外部delphi编写的dll检测压缩压缩是否正常。
                //if (TL_RarTest(fromRarFile) != 0)
                //{
                //    return "异常的压缩文件，解压失败！";
                //}
                bool b经过混淆 = false;
                //先判断一下待解压文件是否经过混淆
                FileStream fsRar = new FileStream(fromRarFile, FileMode.Open, FileAccess.Read);
                int b = fsRar.ReadByte();
                fsRar.Close();
                if (b != 0x52)     //R:0x52
                {
                    b经过混淆 = true;
                    string strTempFile = System.IO.Path.GetTempFileName();
                    File.Copy(fromRarFile, strTempFile, true);
                    FileStream fs = new FileStream(strTempFile, FileMode.Open);
                    fs.WriteByte(0x52);
                    fs.Close();
                    fromRarFile = strTempFile;
                }

                if (Directory.Exists(toRarPath) == false)
                {
                    Directory.CreateDirectory(toRarPath);
                }

                rar = "\"" + rarRunPath + "\"";
                toRarPath = "\"" + toRarPath + "\"";
                commandInfo = "x \"" + fromRarFile + "\" " + toRarPath + " -y -ptruelore";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = rar;
                startInfo.Arguments = commandInfo;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //Windows 2003设置此属性无效。
                //fromRarPath = "\"" + fromRarPath + "\"";
                //startInfo.WorkingDirectory = fromRarPath;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                process.Dispose();

                if (b经过混淆)
                    System.IO.File.Delete(fromRarFile);

                return String.Empty;
            }
            catch
            {
                return "解压缩失败！" + fromRarFile;
            }
        }


        /// <summary>
        /// 解压：将文件解压到某个文件夹中。 注意：要对路径加上双引号，避免带空格的路径，在rar命令中失效
        /// </summary>
        /// <param name="rarRunPath">rar.exe的名称及路径</param>
        /// <param name="fromRarPath">被解压的rar文件路径</param>
        /// <param name="toRarPath">解压后的文件存放路径</param>
        /// <param name="rarName">被解压的rar文件名称</param>
        /// <returns></returns>
        public static String unCompressRAR(String rarRunPath, String fromRarPath, String toRarPath, String rarName)
        {
            String rar;
            String commandInfo;

            try
            {
                String fromRarFile = fromRarPath + "\\" + rarName;
                bool b经过混淆 = false;

                //先判断一下待解压文件是否经过混淆
                FileStream fsRar = new FileStream(fromRarFile, FileMode.Open, FileAccess.Read);
                int b = fsRar.ReadByte();
                fsRar.Close();
                if (b != 0x52)     //R:0x52
                {
                    b经过混淆 = true;
                    string strTempFile = System.IO.Path.GetTempFileName();
                    File.Copy(fromRarFile, strTempFile, true);
                    FileStream fs = new FileStream(strTempFile, FileMode.Open);
                    fs.WriteByte(0x52);
                    fs.Close();
                    fromRarFile = strTempFile;
                }

                //2011-03-18 Youker 调用外部delphi编写的dll检测压缩压缩是否正常。
                if (TL_RarTest(fromRarFile) != 0)    //注：TL_RarTest中已经处理了空格问题，自动增加了双引号！
                {
                    return "异常的压缩文件，解压失败！";
                }


                if (Directory.Exists(toRarPath) == false)
                {
                    Directory.CreateDirectory(toRarPath);
                }
                rar = "\"" + rarRunPath + "\"";
                rarName = "\"" + fromRarFile + "\"";
                toRarPath = "\"" + toRarPath + "\"";
                commandInfo = "x " + rarName + " " + toRarPath + " -y -ptruelore";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = rar;
                startInfo.Arguments = commandInfo;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //Windows 2003设置此属性无效。
                //fromRarPath = "\"" + fromRarPath + "\"";
                //startInfo.WorkingDirectory = fromRarPath;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                process.Dispose();

                if (b经过混淆)
                    System.IO.File.Delete(fromRarFile);

                return String.Empty;
            }
            catch (Exception ee)
            {
                return string.Format("解压缩异常:{0}\nfromRarPath:{1}\nrarName:{2}",
                    ee.Message,
                    fromRarPath,
                    rarName);
            }
        }

        /// <summary>
        /// 压缩(压缩包不包含当前路径)  注意：要对路径加上双引号，避免带空格的路径，在rar命令中失效
        /// </summary>
        /// <param name="rarRunPath">rar.exe的名称及路径</param>
        /// <param name="fromRarPath">被压缩的文件夹</param>
        /// <param name="toRarPath">保存的文件路径</param>
        /// <param name="rarName">保存的文件名称</param>
        /// <returns></returns>
        public static String CompressRAR(String rarRunPath, String fromRarPath, String toRarPath, String rarName)
        {
            return CompressRAR(rarRunPath, fromRarPath, toRarPath, rarName, false);
        }
        public static String CompressRAR(String rarRunPath, String fromRarPath, String toRarPath, String rarName, bool b混淆)
        {
            String rar;
            String commandInfo;

            try
            {
                if (Directory.Exists(fromRarPath) == false)
                {
                    Directory.CreateDirectory(fromRarPath);
                }
                String fullRarFileName = FilePathCombine(toRarPath, rarName + ".rar");
                if (File.Exists(fullRarFileName) == true)
                {
                    File.Delete(fullRarFileName);
                }

                rar = "\"" + rarRunPath + "\"";
                rarName = "\"" + rarName + ".rar\"";
                fromRarPath = "\"" + fromRarPath + "\"";

                commandInfo = "a " + rarName + " " + fromRarPath + " -y -ptruelore -ep1 -r -s- -rr ";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = rar;
                startInfo.Arguments = commandInfo;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.WorkingDirectory = toRarPath;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                process.Dispose();

                //2011-03-18 Youker 调用外部delphi编写的dll检测压缩压缩是否正常。
                if (TL_RarTest(fullRarFileName) != 0)
                {
                    return "压缩失败！";
                }

                if (b混淆)
                {
                    FileStream fs = new FileStream(fullRarFileName, FileMode.Open);
                    fs.WriteByte(0x53);
                    fs.Close();
                }

                return String.Empty;
                //CompressRarTest(rar, fullRarFileName);
            }
            catch
            {
                return "压缩失败！";
            }
        }


        /// <summary>
        /// 压缩(压缩包不包含当前路径)  注意：要对路径加上双引号，避免带空格的路径，在rar命令中失效
        /// </summary>
        /// <param name="rarRunPath">rar.exe的名称及路径</param>
        /// <param name="fromRarPath">被压缩的文件夹</param>
        /// <param name="toRarPath">保存的文件路径</param>
        /// <param name="rarName">保存的文件名称</param>
        /// <returns></returns>
        public static void CompressRAR(String rarRunPath, String fromRarPath, String toRarPath, String rarName, String argument)
        {
            CompressRAR(rarRunPath, fromRarPath, toRarPath, rarName, argument, false);
        }
        public static void CompressRAR(String rarRunPath, String fromRarPath, String toRarPath, String rarName, String argument, bool b混淆)
        {
            String rar;
            String commandInfo;

            try
            {
                if (Directory.Exists(fromRarPath) == false)
                {
                    Directory.CreateDirectory(fromRarPath);
                }
                String fullRarFileName = FilePathCombine(toRarPath, rarName + ".rar");
                if (File.Exists(fullRarFileName) == true)
                {
                    File.Delete(fullRarFileName);
                }

                rar = "\"" + rarRunPath + "\"";
                rarName = "\"" + rarName + ".rar\"";
                fromRarPath = "\"" + fromRarPath + "\"";

                commandInfo = "a " + rarName + " " + fromRarPath + " -y -ptruelore -ep1 -r -s- -rr " + argument + " ";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = rar;
                startInfo.Arguments = commandInfo;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.WorkingDirectory = toRarPath;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                process.Dispose();

                //2011-03-18 Youker 调用外部delphi编写的dll检测压缩压缩是否正常。
                if (TL_RarTest(fullRarFileName) != 0)
                {
                    throw new Exception("压缩失败！");
                }
                //CompressRarTest(rar, fullRarFileName);

                if (b混淆)
                {
                    FileStream fs = new FileStream(fullRarFileName, FileMode.Open);
                    fs.WriteByte(0x53);
                    fs.Close();
                }
            }
            catch
            {
                throw new Exception("压缩失败！");
            }
        }

        /// <summary>
        /// 测试压缩后的文件是否正常。
        /// </summary>
        /// <param name="rarRunPath"></param>
        /// <param name="testRarFileName"></param>
        public static void CompressRarTest(String rarRunPath, String testRarFileName)
        {
            String commandInfo;

            try
            {
                commandInfo = "t -ptruelore \"" + testRarFileName + "\"";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = rarRunPath;
                startInfo.Arguments = commandInfo;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                StreamReader streamReader = process.StandardOutput;

                process.WaitForExit();

                if (streamReader.ReadToEnd().ToLower().IndexOf("error") >= 0)
                {
                    process.Close();
                    process.Dispose();

                    throw new Exception("压缩失败！");
                }

                process.Close();
                process.Dispose();
            }
            catch
            {
                throw new Exception("压缩失败！");
            }
        }

        public static String CompressFileRAR(String rarRunPath, String compressFile, String toRarPath, String rarFileName, bool b混淆)
        {
            return CompressFileRAR(rarRunPath, compressFile, toRarPath, rarFileName, b混淆, "truelore");
        }

        /// <summary>
        /// 压缩文件  注意：要对路径加上双引号，避免带空格的路径，在rar命令中失效
        /// </summary>
        /// <param name="compressFile">待压缩的文件</param>
        /// <param name="rarFileName">压缩文件的名称</param>
        /// 
        public static String CompressFileRAR(String rarRunPath, String compressFile, String toRarPath, String rarFileName, bool b混淆, string strPassword)
        {
            String rar;
            String commandInfo;
            try
            {
                if (!File.Exists(compressFile))
                {
                    return "待压缩的文件不存在！";
                }

                String fullRarFileName = FilePathCombine(toRarPath, rarFileName + ".rar");
                if (File.Exists(fullRarFileName))
                {
                    File.SetAttributes(fullRarFileName, FileAttributes.Normal);
                    File.Delete(fullRarFileName);
                }

                rar = "\"" + rarRunPath + "\"";
                rarFileName = "\"" + rarFileName + ".rar\"";
                compressFile = "\"" + compressFile + "\"";

                commandInfo = "a " + rarFileName + " " + compressFile + " -y -p" + strPassword + " -ep1 -s- -rr ";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = rar;
                startInfo.Arguments = commandInfo;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.WorkingDirectory = toRarPath;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                process.Dispose();

                if (TL_RarTestByPassword(fullRarFileName, strPassword) != 0)
                    return "压缩失败";

                //2011-03-18 Youker 调用外部delphi编写的dll检测压缩压缩是否正常。
                //if (TL_RarTest(fullRarFileName) != 0)
                //{
                //    return "压缩失败！";
                //}
                if (b混淆)
                {
                    FileStream fs = new FileStream(fullRarFileName, FileMode.Open);
                    fs.WriteByte(0x53);
                    fs.Close();
                }

                return "";
            }
            catch (Exception ex)
            {
                return "压缩失败！" + ex.Message;
            }
        }

        #endregion

        #region 注册表操作
        /// <summary>
        /// 设置网页页眉和页脚。
        /// </summary>
        /// <param name="footer">页脚内容。</param>
        /// <param name="header">页眉内容。</param>
        private static void Access_Registry_WebPrint(String footer, String header, String shrink_to_fit)
        {
            // 2011-04-28 added by chenx 
            // 设置登陆用户的写入权限
            RegistrySecurity mSec = new RegistrySecurity();
            RegistryAccessRule rule = new RegistryAccessRule(
                    Environment.UserDomainName + "\\" + Environment.UserName,
                    RegistryRights.WriteKey,
                    InheritanceFlags.ContainerInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow);
            mSec.AddAccessRule(rule);

            RegistryKey hklm = Registry.CurrentUser;
            RegistryKey pageSetup = hklm.OpenSubKey("Software\\Microsoft\\Internet Explorer\\PageSetup", true);
            if (pageSetup != null)
            {
                try
                {
                    // 2011-04-28 added by chenx
                    // 向该注册表应用Windows访问控制安全性
                    pageSetup.SetAccessControl(mSec);
                }
                catch { }
                pageSetup.SetValue("header", header);
                pageSetup.SetValue("footer", footer);

                //部分IE浏览器的页边距有问题
                pageSetup.SetValue("margin_left", 0.1968504);   //5mm
                pageSetup.SetValue("margin_right", 0.1968504);   //5mm
                pageSetup.SetValue("margin_top", 0.4724409);   //12mm
                pageSetup.SetValue("margin_bottom", 0.4724409);   //12mm

                try
                {
                    //把IE8中页面设置中的自动缩小字体填充勾选去掉。否则按照字体大小匹配书签出问题。
                    pageSetup.SetValue("Shrink_To_Fit", shrink_to_fit);
                }
                catch
                {
                }
            }

            //IE7中设置打印预览100%。
            WTRegedit("iexplore.exe", "00000100", RegistryValueKind.DWord);

            RegistryKey printBackground = hklm.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main", true);
            if (printBackground != null)
            {
                try
                {
                    // 2011-04-28 added by chenx
                    // 向该注册表应用Windows访问控制安全性
                    printBackground.SetAccessControl(mSec);
                }
                catch { }

                printBackground.SetValue("Print_Background", "no");
            }
        }

        /// <summary>
        /// 设置页眉、页脚(针对web打印的情况)
        /// </summary>
        /// <param name="footer"></param>
        /// <param name="header"></param>
        public static void SetHeadFoot_WebPrint(String footer, String header)
        {
            Access_Registry_WebPrint(footer, header, "no");
            SetBrowserEmulation("00000001");
        }

        /// <summary>
        /// 还原页眉页脚设置(针对web打印的情况)
        /// </summary>
        public static void RestoreHeadFoot_WebPrint()
        {
            Access_Registry_WebPrint("&w&b页码,&p/&P", "&u&b&d", "no");
        }

        /// <summary>
        /// 向注册表中写数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tovalue"></param>
        private static void WTRegedit(String name, String tovalue, RegistryValueKind registryValueKind)
        {
            try
            {
                RegistryKey hklm = Registry.LocalMachine;
                RegistryKey feature_Stf_Scale_Min = hklm.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_STF_Scale_Min", true);
                if (feature_Stf_Scale_Min == null)
                {
                    feature_Stf_Scale_Min = hklm.CreateSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_STF_Scale_Min");
                }

                feature_Stf_Scale_Min.SetValue(name, tovalue, registryValueKind);
            }
            catch
            {
            }
        }

        //新建文件打开关联的应用程序到注册表中
        public static Boolean SetAssociatedFile(String appPath, String fileTypeName)
        {
            try
            {
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey("", true); //打开注册表 

                RegistryKey vrpKey = regKey.OpenSubKey(fileTypeName);
                if (vrpKey != null)
                {
                    regKey.DeleteSubKey(fileTypeName, true);
                }

                regKey.CreateSubKey(fileTypeName);
                vrpKey = regKey.OpenSubKey(fileTypeName, true);
                vrpKey.SetValue("", "Exec");

                vrpKey = regKey.OpenSubKey("Exec", true);
                if (vrpKey != null)
                {
                    regKey.DeleteSubKeyTree("Exec"); //如果等于空 就删除注册表DSKJIVR 
                }

                regKey.CreateSubKey("Exec");
                vrpKey = regKey.OpenSubKey("Exec", true);
                vrpKey.CreateSubKey("shell");
                vrpKey = vrpKey.OpenSubKey("shell", true); //写入必须路径 
                vrpKey.CreateSubKey("open");
                vrpKey = vrpKey.OpenSubKey("open", true);
                vrpKey.CreateSubKey("command");
                vrpKey = vrpKey.OpenSubKey("command", true);
                String pathString = "\"" + appPath + "\" \"%1\"";
                vrpKey.SetValue("", pathString); //写入数据 

                return true;
            }
            catch
            {
                return false;
            }
        }

        //删除注册表中文件关联
        public static Boolean DelAssociatedFile(String fileTypeName)
        {
            try
            {
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey("", true);

                RegistryKey vrpKey = regKey.OpenSubKey(fileTypeName);
                if (vrpKey != null)
                {
                    regKey.DeleteSubKey(fileTypeName, true);
                }

                if (vrpKey != null)
                {
                    regKey.DeleteSubKeyTree("Exec");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置浏览器模式（在兼容性视图中显示所有网站）。
        /// </summary>
        public static void SetBrowserEmulation(String modeValue)
        {
            try
            {
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\BrowserEmulation", true);
                if (regKey != null)
                {
                    regKey.SetValue("AllSitesCompatibilityMode", modeValue, RegistryValueKind.DWord);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 文件、路径操作
        /// <summary>
        /// 获取系统临时目录文件夹路径。
        /// </summary>
        /// <returns></returns>
        public static String GetUserTempPath()
        {
            String temp = Environment.GetEnvironmentVariable("TEMP");
            DirectoryInfo directoryInfo = new DirectoryInfo(temp);
            String userTempPath = directoryInfo.FullName + "\\TrueLore\\";

            return userTempPath;
        }

        /// <summary>
        /// 2个文件路径的合并
        /// </summary>
        /// <param name="path1">路径1</param>
        /// <param name="path2">路径2</param>
        /// <returns>返回合并后的路径</returns>
        public static String FilePathCombine(String path1, String path2)
        {
            String result = null;
            if (path1.EndsWith(Path.DirectorySeparatorChar.ToString()) && path2.StartsWith(Path.DirectorySeparatorChar.ToString()))
            {
                result = path1.Substring(0, path1.LastIndexOf(Path.DirectorySeparatorChar.ToString())).ToString() + path2;
            }
            else if (path1.EndsWith(Path.DirectorySeparatorChar.ToString()) && path2.StartsWith(Path.DirectorySeparatorChar.ToString()) == false)
            {
                result = path1 + path2;
            }
            else if (path1.EndsWith(Path.DirectorySeparatorChar.ToString()) == false && path2.StartsWith(Path.DirectorySeparatorChar.ToString()) == true)
            {
                result = path1 + path2;
            }
            else
            {
                result = path1 + Path.DirectorySeparatorChar.ToString() + path2;
            }

            return result;
        }

        public static bool TryDeleteFolder(string strFolder)
        {
            bool bOK;
            try
            {
                System.IO.Directory.Delete(strFolder, true);
                bOK = true;
            }
            catch
            {
                bOK = false;
            }

            return bOK;
        }

        /// <summary>
        /// 删除今天之前的所有临时文件夹。
        /// </summary>
        public static void TryDeleteBeforeTodayTempFolder()
        {
            String userTempPath = CommonFunction.GetUserTempPath();
            if (Directory.Exists(userTempPath))
            {
                String[] directories = Directory.GetDirectories(userTempPath);
                foreach (String directorie in directories)
                {
                    if (Directory.GetCreationTime(directorie) < DateTime.Now.Date)
                    {
                        try
                        {
                            System.IO.Directory.Delete(directorie, true);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除指定目录
        /// </summary>
        /// <param name="DeleteInDir"></param>
        /// <param name="szDirPath">指定目录</param>
        /// <param name="iRootDirPath">1:清空指定目录下所有文件及子目录;2:删除指定目录</param>
        /// <returns></returns>
        public static void DeleteInDir(String szDirPath, Int32 iRootDirPath)
        {
            if (szDirPath.Trim() == "" || !Directory.Exists(szDirPath))
                return;
            DirectoryInfo dirInfo = new DirectoryInfo(szDirPath);

            FileInfo[] fileInfos = dirInfo.GetFiles();
            if (fileInfos != null && fileInfos.Length > 0)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    if (fileInfo.Attributes != FileAttributes.Normal)
                    {
                        fileInfo.Attributes = FileAttributes.Normal;
                    }
                    File.Delete(fileInfo.FullName); //删除文件
                }
            }

            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            if (dirInfos != null && dirInfos.Length > 0)
            {
                foreach (DirectoryInfo childDirInfo in dirInfos)
                {
                    DeleteInDir(childDirInfo.FullName, 2); //递归
                }
            }
            if (iRootDirPath == 2)
            {
                Directory.Delete(dirInfo.FullName, true); //删除目录
            }
        }

        /// <summary>
        /// 检查文件名是否有效。
        /// </summary>
        /// <param name="FileName"></param>
        public static Boolean CheckFileName(String fileName)
        {
            Boolean result = false;

            FileInfo fileInfo = new FileInfo(fileName);
            String[] strList = new String[] { "#", "/", " " };

            foreach (String str in strList)
            {
                if (fileInfo.Name.Contains(str))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 鼠标右键选中树节点
        /// </summary>
        /// <param name="treeView">树</param>
        /// <param name="x">鼠标X坐标</param>
        /// <param name="y">鼠标Y坐标</param>
        /// <returns>被选中的树节点</returns>
        public static TreeNode MouseRightButtonSelectedNode(TreeView treeView, Int32 x, Int32 y)
        {
            System.Drawing.Point clickPoint = new System.Drawing.Point(x, y);
            TreeNode currentNode = treeView.GetNodeAt(clickPoint);
            if (currentNode != null)
            {
                treeView.SelectedNode = currentNode;
            }

            return currentNode;
        }

        /// <summary>
        /// 判断文件是否已经加密
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="rarRunPath"></param>
        /// <returns></returns>
        public static Boolean IsFileEncrypted(String sourceFileName, String rarRunPath)
        {
            Boolean isEncrypted = false;
            String tempPath = CommonFunction.GetUserTempPath() + Guid.NewGuid().ToString() + "\\";
            String keyFileName = "Key.enc";

            String errorMessage = CommonFunction.unCompressRAR(rarRunPath, Path.GetDirectoryName(sourceFileName), tempPath, Path.GetFileName(sourceFileName));
            if (!String.IsNullOrEmpty(errorMessage))
            {
                throw new Exception("异常的文件，无法解压！");
            }

            if (File.Exists(tempPath + keyFileName))
            {
                isEncrypted = true;
            }

            CommonFunction.DeleteInDir(tempPath, 1);

            return isEncrypted;
        }
        #endregion

        #region 获取本地的Word的版本的信息
        /// <summary>
        /// 获取本地的Word的版本的信息
        /// </summary>
        /// <returns>获取的本地Word的版本信息</returns>
        public static String GetLocalWordVersion(out String message)
        {
            // 返回结果
            String result = String.Empty;
            message = String.Empty;

            //打开注册表
            String VBASubKeyPath = @"SOFTWARE\Microsoft\Shared Tools\AddIn Designer\Visual Basic for Applications IDE";
            RegistryKey VBASubKey = Registry.LocalMachine.OpenSubKey(VBASubKeyPath);

            RegistryKey optionKey = Registry.ClassesRoot.OpenSubKey("Word.Application\\CurVer");
            if (optionKey == null)
            {
                message = "请安装完整版本Office 2003或以上完整版本！";
                //RegistryKey optionKey2003 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Office\\11.0\\Word\\InstallRoot\\");
                //RegistryKey optionKey2007 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Office\\12.0\\Word\\InstallRoot\\");
                //RegistryKey optionKey2010 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Office\\14.0\\Word\\InstallRoot\\");
                //if (optionKey2003 == null && optionKey2007 == null && optionKey2010 == null)
                //{
                //    message = "请安装完整版本Office 2003或以上版本！";
                //}

                //if (optionKey2003 != null)
                //{
                //    result = "2003";
                //}
                //else if (optionKey2007 != null || optionKey2010 != null)
                //{
                //    result = "2007";
                //}
            }
            else
            {
                String wordVersion = optionKey.GetValue("").ToString();
                if (wordVersion == "Word.Application.11")
                {
                    // 判断是否安装了VBA
                    if (VBASubKey == null)
                    {
                        message = "您的Word版本为2003简化版，需要能升级到完整版才能完成操作！";
                    }
                    else
                    {
                        result = "2003";
                    }
                }
                else if (wordVersion == "Word.Application.12" || wordVersion == "Word.Application.14")
                {
                    result = "2007";
                }
                else
                {
                    message = "您本地的Word版本不支持，需要升级到2003完整版或者2007才能完成操作！";
                }
            }

            return result;
        }

        public static Microsoft.Win32.RegistryKey GetOptionKey()
        {
            //打开注册表
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot;
            String SubKeyPath = null;
            Microsoft.Win32.RegistryKey optionKey = null;

            // 修改高版本的Word 2007
            SubKeyPath = @"Word.Document\CurVer";
            optionKey = regKey.OpenSubKey(SubKeyPath, true);

            return optionKey;
        }
        #endregion

        #region 获取电脑硬件序列号
        [DllImport("TrueLoreGetCPUSN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 TL_GetCPU_SN([MarshalAs(UnmanagedType.LPStr)] StringBuilder outCPUValue);

        [DllImport("TrueLoreGetCPUSN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 TL_GetMAC_SN([MarshalAs(UnmanagedType.LPStr)] StringBuilder outMACValue);

        [DllImport("TrueLoreGetCPUSN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 TL_GetIDE_SN([MarshalAs(UnmanagedType.LPStr)] StringBuilder outIDEValue);

        //获取机器名
        //返回值：0：OK， -1：错误
        [DllImport("TrueloreGetCPUSN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 TL_GetComputerName([MarshalAs(UnmanagedType.LPStr)] StringBuilder outComputerName);

        /// <summary>
        /// 获取 CPU 序列号。
        /// </summary>
        /// <returns>CPU 序列号。</returns>
        public static String GetCPUSN()
        {
            StringBuilder sb = new StringBuilder(500);
            Int32 result = TL_GetCPU_SN(sb);
            if (result == -1)
            {
                return String.Empty;
            }

            return sb.ToString();

            //String cpuSN = String.Empty;
            //ManagementClass mc = new ManagementClass("Win32_Processor");
            //ManagementObjectCollection moc = mc.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    cpuSN = mo.Properties["ProcessorId"].Value.ToString();
            //}

            //return cpuSN;
        }

        /// <summary>
        /// 获取硬盘序列号。
        /// </summary>
        /// <returns>硬盘序列号。</returns>
        public static String GetIDESN()
        {
            StringBuilder sb = new StringBuilder(500);
            Int32 result = TL_GetIDE_SN(sb);
            if (result == -1)
            {
                return String.Empty;
            }

            return sb.ToString();
            //String ideSN = String.Empty;
            //ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            //ManagementObjectCollection moc = mc.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    ideSN = mo.Properties["Model"].Value.ToString();
            //}

            //return ideSN;
        }

        /// <summary>
        /// 获取网卡 MAC 序列号。
        /// </summary>
        /// <returns>网卡 MAC 序列号。</returns>
        public static String GetMacSN()
        {
            StringBuilder sb = new StringBuilder(500);
            Int32 result = TL_GetMAC_SN(sb);
            if (result == -1)
            {
                return String.Empty;
            }

            return sb.ToString();
            //String macSN = "";
            //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //ManagementObjectCollection moc = mc.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    if ((Boolean)mo["IPEnabled"])
            //    {
            //        macSN = mo.Properties["MacAddress"].Value.ToString();
            //    }
            //    mo.Dispose();
            //}

            //return macSN;
        }

        /// <summary>
        /// 获取机器名。
        /// </summary>
        /// <returns>机器名。</returns>
        public static String GetComputerName()
        {
            StringBuilder sb = new StringBuilder(500);
            Int32 result = TL_GetComputerName(sb);
            if (result == -1)
            {
                return String.Empty;
            }

            return sb.ToString();
        }
        #endregion

        #region 绑定枚举类型到下拉控件
        /// <summary>
        /// 把指定的枚举类型绑定到指定的列表控件。
        /// </summary>
        /// <param name="control">列表控件。</param>
        /// <param name="enumType">枚举类型。</param>
        public static void BindEnumToComboBoxControl(ComboBox control, Type enumType, Type resourceEnum, Int32 selectedIndex)
        {
            String[] names = Enum.GetNames(enumType);
            Array values = Enum.GetValues(enumType);

            DataTable dtEnum = new DataTable();
            dtEnum.Columns.Add("Value");
            dtEnum.Columns.Add("Text");
            DataRow drEnum = null;

            for (Int32 i = 0, namesCount = names.Length; i < namesCount; i++)
            {
                ResourceManager resourceManager = new ResourceManager(resourceEnum);
                drEnum = dtEnum.NewRow();
                drEnum["Text"] = resourceManager.GetString(names[i]);
                drEnum["Value"] = (Int32)values.GetValue(i);
                dtEnum.Rows.Add(drEnum);
            }

            control.DisplayMember = "Text";
            control.ValueMember = "Value";
            control.DataSource = dtEnum;
            if (dtEnum.Rows.Count >= 1 && selectedIndex >= 0)
            {
                control.SelectedIndex = selectedIndex;
            }
        }
        #endregion

        #region 绑定枚举类型到下拉控件
        /// <summary>
        /// 把指定的枚举类型绑定到指定的列表控件。
        /// </summary>
        /// <param name="control">列表控件。</param>
        /// <param name="enumType">枚举类型。</param>
        public static void BindEnumToComboBoxControl(System.Web.UI.WebControls.DropDownList control, Type enumType, Int32 selectedIndex)
        {
            String[] names = Enum.GetNames(enumType);
            Array values = Enum.GetValues(enumType);

            DataTable dtEnum = new DataTable();
            dtEnum.Columns.Add("Value");
            dtEnum.Columns.Add("Text");
            DataRow drEnum = null;

            for (Int32 i = 0, namesCount = names.Length; i < namesCount; i++)
            {
                drEnum = dtEnum.NewRow();
                drEnum["Text"] = names[i];
                drEnum["Value"] = (Int32)values.GetValue(i);
                dtEnum.Rows.Add(drEnum);
            }

            control.DataTextField = "Text";
            control.DataValueField = "Value";
            control.DataSource = dtEnum;
            control.DataBind();
            if (dtEnum.Rows.Count >= 1 && selectedIndex >= 0)
            {
                control.SelectedIndex = selectedIndex;
            }
        }
        #endregion

        #region 秒转换为分钟字符串
        public static String ConvertSecondToMinString(Int32 second)
        {
            String minString = String.Empty;
            Int32 min = Convert.ToInt32(Math.Floor(Convert.ToDecimal(second / 60)));
            Int32 sec = second % 60;

            if (min > 0)
            {
                minString = min.ToString() + "分";
            }

            if (sec > 0)
            {
                minString += sec.ToString() + "秒";
            }

            return minString;
        }
        #endregion

        #region 将颜色转换为整形值
        /// <summary>
        /// 将颜色转换为整形值
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Int32 ParseRGB(Color color)
        {
            return (Int32)(((uint)color.B << 16) | (ushort)(((ushort)color.G << 8) | color.R));
        }
        #endregion

        #region 将整形值还原为颜色
        /// <summary>
        /// 将整形值还原为颜色。
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color RGB(Int32 color)
        {
            Int32 r = 0xFF & color;

            Int32 g = 0xFF00 & color;
            g >>= 8;

            Int32 b = 0xFF0000 & color;
            b >>= 16;

            return Color.FromArgb(r, g, b);
        }
        #endregion

        #region 检查应用程序是否在运行
        /// <summary>
        /// 检查应用程序是否在运行
        /// </summary>
        /// <returns></returns>
        public static Boolean CheckAppIsRun(String appName)
        {
            Boolean isRun = false;

            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();

            foreach (System.Diagnostics.Process myProcess in myProcesses)
            {
                if (Path.GetFileNameWithoutExtension(appName) == myProcess.ProcessName)
                {
                    isRun = true;
                }
            }

            //同步基元对于.net应用程序的exe
            //Mutex pobjMyMutex = null;

            //try
            //{
            //    Boolean isCreateNew = false;

            //    pobjMyMutex = new Mutex(true, appName, out isCreateNew);

            //    if (isCreateNew)
            //    {
            //        isRun = false;
            //    }
            //    else
            //    {
            //        isRun = true;
            //    }
            //}
            //finally
            //{
            //    if (pobjMyMutex != null)
            //    {
            //        pobjMyMutex.Close();
            //    }
            //}

            return isRun;
        }
        #endregion

        #region 检查文件名称字符串是合法
        /// <summary>
        /// 检查文件名称字符串是合法
        /// </summary>
        /// <param name="fileName">文件名称（不包含路径）</param>
        /// <returns></returns>
        public static Boolean IsValidFileName(String fileName)
        {
            Boolean isValid = true;
            String[] strList = new String[] { " ", "#", "/", @"\", @"\/", ":", "*", "?", "<", ">", "|", "\r\n" };

            if (String.IsNullOrEmpty(fileName))
            {
                isValid = false;
            }
            else
            {
                foreach (String errStr in strList)
                {
                    if (fileName.Contains(errStr))
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            return isValid;
        }
        #endregion

        #region DataTable联合查询
        public static DataTable Join(DataTable First, DataTable Second, DataColumn[] FJC, DataColumn[] SJC)
        {
            //创建一个新的DataTable
            DataTable table = new DataTable("Join");

            // Use a DataSet to leverage DataRelation
            using (DataSet ds = new DataSet())
            {
                //把DataTable Copy到DataSet中
                ds.Tables.AddRange(new DataTable[] { First.Copy(), Second.Copy() });

                DataColumn[] parentcolumns = new DataColumn[FJC.Length];
                for (Int32 i = 0; i < parentcolumns.Length; i++)
                {
                    parentcolumns[i] = ds.Tables[0].Columns[FJC[i].ColumnName];
                }

                DataColumn[] childcolumns = new DataColumn[SJC.Length];
                for (Int32 i = 0; i < childcolumns.Length; i++)
                {
                    childcolumns[i] = ds.Tables[1].Columns[SJC[i].ColumnName];
                }

                //创建关联
                DataRelation r = new DataRelation(String.Empty, parentcolumns, childcolumns, false);
                ds.Relations.Add(r);

                //为关联表创建列
                for (Int32 i = 0; i < First.Columns.Count; i++)
                {
                    table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);
                }

                for (Int32 i = 0; i < Second.Columns.Count; i++)
                {
                    //看看有没有重复的列，如果有在第二个DataTable的Column的列明后加_Second
                    if (!table.Columns.Contains(Second.Columns[i].ColumnName))
                    {
                        table.Columns.Add(Second.Columns[i].ColumnName, Second.Columns[i].DataType);
                    }
                    else
                    {
                        table.Columns.Add(Second.Columns[i].ColumnName + "_Second", Second.Columns[i].DataType);
                    }
                }

                table.BeginLoadData();

                foreach (DataRow firstrow in ds.Tables[0].Rows)
                {
                    //得到行的数据
                    DataRow[] childrows = firstrow.GetChildRows(r);
                    if (childrows != null && childrows.Length > 0)
                    {
                        object[] parentarray = firstrow.ItemArray;

                        foreach (DataRow secondrow in childrows)
                        {
                            object[] secondarray = secondrow.ItemArray;
                            object[] joinarray = new object[parentarray.Length + secondarray.Length];
                            Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);
                            Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);
                            table.LoadDataRow(joinarray, true);
                        }
                    }
                }

                table.EndLoadData();
            }

            return table;
        }
        #endregion

        #region 取得字符串的长度
        /// <summary>
        /// 取得字符串的长度
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串长度</returns>
        public static String getStringValueLength(String value)
        {
            String result = "4";
            if (!String.IsNullOrEmpty(value))
            {
                // 数组
                byte[] bytes = Encoding.Default.GetBytes(value);
                if (bytes.Length > 4)
                {
                    result = bytes.Length.ToString();
                }
            }

            // 返回长度结果
            return result;
        }
        #endregion

        #region 取得大写的数字
        /// <summary>
        /// 取得大写的数字
        /// </summary>
        public static String GetDXStrNumber(String value)
        {
            // 结果
            String result = null;
            switch (value)
            {
                case "1":
                    result = "一";
                    break;
                case "2":
                    result = "二";
                    break;
                case "3":
                    result = "三";
                    break;
                case "4":
                    result = "四";
                    break;
                case "5":
                    result = "五";
                    break;
                case "6":
                    result = "六";
                    break;
                case "7":
                    result = "七";
                    break;
                case "8":
                    result = "八";
                    break;
                case "9":
                    result = "九";
                    break;
                case "10":
                    result = "十";
                    break;
                case "11":
                    result = "十一";
                    break;
                case "12":
                    result = "十二";
                    break;
                case "13":
                    result = "十三";
                    break;
                case "14":
                    result = "十四";
                    break;
                case "15":
                    result = "十五";
                    break;
            }

            return result;
        }
        #endregion

        #region 启动其他的应用程序
        ///   <summary>   
        ///   启动其他的应用程序   
        ///   </summary>   
        ///   <param   name="file">应用程序名称</param>   
        ///   <param   name="workdirectory">应用程序工作目录</param>   
        ///   <param   name="args">命令行参数</param>   
        ///   <param   name="style">窗口风格</param>   
        public static Boolean StartProcess(String appName, String workDirectory, String args, ProcessWindowStyle style)
        {
            try
            {
                Process myprocess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(appName, args);
                startInfo.WindowStyle = style;
                startInfo.WorkingDirectory = workDirectory;
                myprocess.StartInfo = startInfo;
                myprocess.StartInfo.UseShellExecute = false;
                myprocess.Start();

                return true;
            }
            catch (Exception ex)
            {
                TrueLoreMessageBox.ShowError("启动应用程序时出错！原因：" + Environment.NewLine + ex.Message);
            }

            return false;
        }
        #endregion

        #region 比较2个Decimal的值
        /// <summary>
        /// 比较2个Decimal的值
        /// </summary>
        /// <param name="value1">value1</param>
        /// <param name="value2">value2</param>
        /// <returns></returns>
        public static Int32 CompareDecimalValue(String value1, String value2)
        {
            // 返回结果
            Int32 result = 0;
            // 取得比较的结果:value1>value2返回1.=返回0.<返回-1
            result = Decimal.Parse(value1).CompareTo(Decimal.Parse(value2));
            // 返回该结果
            return result;
        }
        #endregion

        #region 如果是数字按数字进行比较。如果不是数字按字符串比较
        /// <summary>
        /// 如果是数字按数字进行比较。如果不是数字按字符串比较
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Int32 CampareDecimalAndStringValue(String value1, String value2)
        {
            // 返回结果
            Int32 result = 0;
            try
            {
                // 取得比较的结果:value1>value2返回1.=返回0.<返回-1
                result = Decimal.Parse(value1).CompareTo(Decimal.Parse(value2));
            }
            catch
            {
                result = value1.CompareTo(value2);
            }
            // 返回该结果
            return result;
        }
        #endregion

        #region 二进制计算错误
        public static String GetBinaryNumber(String value)
        {
            StringBuilder result = new StringBuilder();

            char[] charList = value.ToCharArray();
            for (Int32 i = charList.Length - 1; i >= 0; i--)
            {
                result.Append(charList[i]);
            }

            return result.ToString();
        }
        #endregion

        #region 判断验证是否全部是半角
        /// <summary>
        /// 判断验证是否全部是半角
        /// </summary>
        public static void CheckBanJiao(String[] messageList, String[] valueList)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (Int32 i = 0; i < valueList.Length; i++)
            {
                if (valueList[i].Length != Encoding.Default.GetByteCount(valueList[i]))
                {
                    strBuilder.Append(messageList[i]);
                    strBuilder.Append("必须输入半角!\r\n");
                }
            }

            if (strBuilder.Length > 0)
            {
                throw new Exception(strBuilder.ToString());
            }
        }
        #endregion

        #region 将数字转成大写的RMB
        /// <summary>
        /// 将数字转成大写的RMB
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static String formateRMBMoney(String money)
        {
            try
            {
                if (String.IsNullOrEmpty(money))
                {
                    money = "0";
                }

                Decimal num = Convert.ToDecimal(money);

                return formateRMBMoney(num);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="num">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static String formateRMBMoney(Decimal money)
        {
            String str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            String str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            String str3 = "";    //从原num值中取出的值 
            String str4 = "";    //数字的字符串形式 
            String str5 = "";  //人民币大写金额形式 
            Int32 i;    //循环变量 
            Int32 j;    //num的值乘以100的字符串长度 
            String ch1 = "";    //数字的汉语读法 
            String ch2 = "";    //数字位的汉字读法 
            Int32 nzero = 0;  //用来计算连续的零值是几个 
            Int32 temp;            //从原num值中取出的值 

            money = Math.Round(Math.Abs(money), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(money * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (money == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }
        #endregion

        #region 格式化货币
        /// <summary>
        /// 格式化货币
        /// </summary>
        public static String formateMoney(String param)
        {
            // 结果
            String result = "0.00";
            if (!String.IsNullOrEmpty(param))
            {
                // 判断是不是0
                Decimal doubleParam = Convert.ToDecimal(param);
                if (!doubleParam.Equals(0))
                {
                    // 转换
                    result = String.Format("{0:N2}", doubleParam);
                    if (!result.Contains("."))
                    {
                        result = result + ".00";
                    }
                }
            }
            // 结果返回
            return result;
        }

        /// <summary>
        /// 格式化数字
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static String formateMoney(Decimal param)
        {
            // 结果
            String result = "0.00";
            if (!param.Equals(0))
            {
                // 转换
                result = String.Format("{0:N2}", param);
                if (!result.Contains("."))
                {
                    result = result + ".00";
                }
            }
            // 结果返回
            return result;
        }
        #endregion

        #region 保留小数点的多少位
        /// <summary>
        /// 保留小数点的多少位
        /// </summary>
        public static String FormatNumber(String param, Int32 count)
        {
            // 结果
            String result = "0.00";

            if (String.IsNullOrEmpty(param) == false)
            {
                Double doubleParam = 0;
                try
                {
                    // Value转换成Double
                    doubleParam = Double.Parse(param);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                // 四舍五入
                doubleParam = Math.Round(doubleParam, count);

                // 转换
                result = String.Format("{0:N" + count + "}", doubleParam);
                if (result.Contains(".") == false && count > 0)
                {
                    result = result + ".";
                    for (Int32 i = 0; i < count; i++)
                    {
                        result = result + "0";
                    }
                }
            }

            return result;
        }
        #endregion

        #region 比较2个字符串的长度
        /// <summary>
        /// 比较2个字符串的长度
        /// </summary>
        /// <param name="parmValue1">比较的字符串1</param>
        /// <param name="parmValue2">比较的字符串2</param>
        /// <returns>返回的结果</returns>
        public static Boolean CompareTwoValue(String parmValue1, String parmValue2)
        {
            // 初始化变量
            Boolean result = false;

            if (String.IsNullOrEmpty(parmValue1) == true)
            {
                if (String.IsNullOrEmpty(parmValue2) == true)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(parmValue2) == true)
                {
                    result = false;
                }
                else
                {
                    if (ReplaceChar(parmValue1).CompareTo(ReplaceChar(parmValue2)) == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            // 返回结果
            return result;
        }

        /// <summary>
        /// 去掉特殊的字符
        /// </summary>
        /// <param name="parmValue">parmValue</param>
        /// <returns>返回去掉的字符串</returns>
        public static String ReplaceChar(String parmValue)
        {
            return parmValue.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("　", "");
        }
        #endregion

        #region 复制文件夹
        /// <summary>
        /// 复制文件夹[循环遍历]
        /// </summary>
        /// <param name="SourcePath">原始路径</param>
        /// <param name="DestinPath">目地的路径</param>
        /// <returns></returns>
        public static Boolean CopyFolder(String SourcePath, String DestinPath)
        {
            if (Directory.Exists(SourcePath))
            {
                if (!Directory.Exists(DestinPath))
                {
                    Directory.CreateDirectory(DestinPath);
                }
                String sourcePath = SourcePath;//[变化的]原始路径
                String destinPath = DestinPath;//[变化的]目地的路径
                Queue<String> source = new Queue<String>();//存原始文件夹路径
                Queue<String> destin = new Queue<String>();//存目地的文件夹路径
                Boolean IsHasChildFolder = true;//是否有子文件夹
                String tempDestinPath = String.Empty;//临时目地的,将被存于destin中
                while (IsHasChildFolder)
                {
                    String[] fileList = Directory.GetFileSystemEntries(sourcePath);// 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                    for (Int32 i = 0; i < fileList.Length; i++)// 遍历所有的文件和目录
                    {
                        tempDestinPath = destinPath + "\\" + Path.GetFileName(fileList[i]);//取得子文件路径
                        if (Directory.Exists(fileList[i]))//存在文件夹时
                        {
                            source.Enqueue(fileList[i]);//当前的子目录的原始路径进队列
                            destin.Enqueue(tempDestinPath);//当前的子目录的目地的路径进队列
                            if (!Directory.Exists(tempDestinPath))
                            {
                                Directory.CreateDirectory(tempDestinPath);
                            }
                        }
                        else//存在文件
                        {
                            File.Copy(fileList[i], tempDestinPath, true);//复制文件
                        }
                    }

                    if (source.Count > 0 && source.Count == destin.Count)//存在子文件夹时
                    {
                        sourcePath = source.Dequeue();
                        destinPath = destin.Dequeue();
                    }
                    else
                    {
                        IsHasChildFolder = false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 得到对应编号的BOOL名称
        public static String GetBooleanText(String boolValue)
        {
            String boolText = "";
            switch (boolValue)
            {
                case "0":
                    boolText = "否";
                    break;
                case "1":
                    boolText = "是";
                    break;
            }

            return boolText;
        }
        #endregion

        #region 数字转换成汉字
        /// <summary>
        /// 获取数字对应的汉字。
        /// </summary>
        /// <param name="number">数字。</param>
        /// <returns>数字对应的汉字。</returns>
        public static String GetChineseString(Int64 number)
        {
            if (number < 0)
            {
                return "数字超出范围！";
            }

            if (number == 0)
            {
                return "零";
            }

            String result = String.Empty;
            String numberStr = "零一二三四五六七八九";
            String[] unitStr = new String[] { "", "十", "百", "千" };
            String[] unitStr2 = new String[] { "", "万", "亿", "万亿", "兆" };

            for (Int32 idx = 0; idx < number.ToString().Length; idx++)
            {
                Int32 temp = (Int32)((Int64)(number / (Int64)Math.Pow(10, idx)) % 10);
                Int32 unit = (Int32)idx / 4;

                if (idx % 4 == 0)
                {
                    result = unitStr2[unit] + result;
                }

                result = numberStr[temp] + unitStr[idx % 4] + result;
            }

            result = Regex.Replace(result, "(零[十百千])+", "零");
            result = Regex.Replace(result, "零{2,}", "零");
            result = Regex.Replace(result, "零([万亿])", "$1");
            result = result.TrimEnd('零');

            return result;
        }
        #endregion

        #region 判断是否联网
        /// <summary>
        /// 检测是否可以上网
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        public static Boolean CheckIsConnectNetWork()
        {
            int i = 0;

            return InternetGetConnectedState(out i, 0);
        }
        #endregion

        #region 读取Html文件中某一元素某一属性的值
        public static Boolean ReadHtmlElementValue(HtmlDocument htmlDoc, String id, String attribute, Boolean defaultvalue)
        {
            if (htmlDoc != null)
            {
                HtmlElement htmlElement = null;
                htmlElement = htmlDoc.GetElementById(id);
                if (htmlElement != null)
                {
                    String strValue = htmlElement.GetAttribute(attribute).ToLower();
                    if (strValue == "true")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    TrueLoreMessageBox.ShowWarning(String.Format("Html文档中没有【{0}】元素！", id));
                }
            }
            return defaultvalue;
        }

        public static string ReadHtmlElementValue(HtmlDocument htmlDoc, String id, String attribute, string defaultvalue)
        {
            if (htmlDoc != null)
            {
                HtmlElement htmlElement = null;
                htmlElement = htmlDoc.GetElementById(id);
                if (htmlElement != null)
                {
                    String strValue = htmlElement.GetAttribute(attribute);
                    return strValue;
                }
                else
                {
                    TrueLoreMessageBox.ShowWarning(String.Format("Html文档中没有【{0}】元素！", id));
                }
            }
            return defaultvalue;
        }
        #endregion

        #region 小写数字转大写数字
        private static readonly Char[] chnText = new Char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
        private static readonly Char[] chnDigit = new Char[] { '十', '百', '千', '万', '亿' };
        public static String GetChineseString(String strDigit)
        {
            // 检查输入数字 
            Decimal dec = 0;
            if (!Decimal.TryParse(strDigit, out dec))
            {
                throw new Exception("输入数值不正确");
            }

            if (dec <= -10000000000000000m || dec >= 10000000000000000m)
            {
                throw new Exception("输入数值太大或太小，超出范围。");
            }

            StringBuilder sb = new StringBuilder();
            // 提取符号部分 
            if ("+" == strDigit.Substring(0, 1)) // '+'在最前 
            {
                strDigit = strDigit.Substring(1);
            }
            else if ("-" == strDigit.Substring(0, 1)) // '-'在最前 
            {
                sb.Append('负');
                strDigit = strDigit.Substring(1);
            }
            else if ("+" == strDigit.Substring(strDigit.Length - 1, 1)) // '+'在最后 
            {
                strDigit = strDigit.Substring(0, strDigit.Length - 1);
            }
            else if ("-" == strDigit.Substring(strDigit.Length - 1, 1)) // '-'在最后 
            {
                sb.Append('负');
                strDigit = strDigit.Substring(0, strDigit.Length - 1);
            }

            // 提取整数和小数部分 
            Int32 indexOfPoint;
            if (-1 == (indexOfPoint = strDigit.IndexOf('.'))) // 如果没有小数部分 
            {
                sb.Append(ConvertIntegral(strDigit));
            }
            else // 有小数部分 
            {
                // 先转换整数部分 
                if (0 == indexOfPoint) // 如果“.”是第一个字符 
                {
                    sb.Append('零');
                }
                else
                {
                    sb.Append(ConvertIntegral(strDigit.Substring(0, indexOfPoint)));
                }
                // 再转换小数部分 
                if (strDigit.Length - 1 != indexOfPoint) // 如果“.”不是最后一个字符 
                {
                    sb.Append('点');
                    sb.Append(ConvertFractional(strDigit.Substring(indexOfPoint + 1)));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转换整数
        /// </summary>
        /// <param name="strIntegral"></param>
        /// <returns></returns>
        private static String ConvertIntegral(String strIntegral)
        {
            // 去掉数字前面所有的'0' 
            // 并把数字分割到字符数组中 
            Char[] integral = ((long.Parse(strIntegral)).ToString()).ToCharArray();
            // 变成中文数字并添加中文数位 
            StringBuilder sbInt = new StringBuilder();
            Int32 i;
            Int32 digit;
            digit = integral.Length - 1;
            // 处理最高位到十位的所有数字 
            for (i = 0; i < integral.Length - 1; i++)
            {
                sbInt.Append(chnText[integral[i] - '0']);
                if (0 == digit % 4)     // '万' 或 '亿'
                {
                    if (4 == digit || 12 == digit)
                    {
                        sbInt.Append(chnDigit[3]); // '万'
                    }
                    else if (8 == digit)
                    {
                        sbInt.Append(chnDigit[4]); // '亿'
                    }
                }
                else         // '十'，'百'或'千'
                {
                    sbInt.Append(chnDigit[digit % 4 - 1]);
                }
                digit--;
            }
            // 如果个位数不是'0'
            // 或者个位数为‘0’但只有一位数
            // 则添加相应的中文数字
            if ('0' != integral[integral.Length - 1] || 1 == integral.Length)
            {
                sbInt.Append(chnText[integral[i] - '0']);
            }

            // 遍历整个字符串
            i = 0;
            while (i < sbInt.Length)
            {
                Int32 j = i;

                Boolean bDoSomething = false;
                // 查找所有相连的“零X”结构
                while (j < sbInt.Length - 1 && "零" == sbInt.ToString().Substring(j, 1))
                {
                    String strTemp = sbInt.ToString().Substring(j + 1, 1);

                    // 如果是“零万”或者“零亿”则停止查找
                    if ("万" == strTemp || "亿" == strTemp)
                    {
                        bDoSomething = true;
                        break;
                    }
                    j += 2;
                }

                if (j != i) // 如果找到“零X”结构，则全部删除
                {
                    sbInt = sbInt.Remove(i, j - i);
                    // 除了在最尾处，或后面不是"零万"或"零亿"的情况下, 
                    // 其他处均补入一个“零”
                    if (i <= sbInt.Length - 1 && !bDoSomething)
                    {
                        sbInt = sbInt.Insert(i, '零');
                        i++;
                    }
                }

                if (bDoSomething) // 如果找到"零万"或"零亿"结构
                {
                    sbInt = sbInt.Remove(i, 1); // 去掉'零'
                    i++;
                    continue;
                }
                // 指针每次可移动2位
                i += 2;
            }

            // 遇到“亿万”变成“亿零”或"亿"
            Int32 index = sbInt.ToString().IndexOf("亿万");
            if (-1 != index)
            {
                if (sbInt.Length - 2 != index &&  // 如果"亿万"不在最后
                 (index + 2 < sbInt.Length && "零" != sbInt.ToString().Substring(index + 2, 1))) // 并且其后没有"零"
                {
                    sbInt = sbInt.Replace("亿万", "亿零", index, 2);
                }
                else
                {
                    sbInt = sbInt.Replace("亿万", "亿", index, 2);
                }
            }
            // 开头为“一十”改为“十”
            if (sbInt.Length > 1 && "一十" == sbInt.ToString().Substring(0, 2))
            {
                sbInt = sbInt.Remove(0, 1);
            }

            return sbInt.ToString();
        }

        /// <summary>
        /// 转换小数部分
        /// </summary>
        /// <param name="strFractional"></param>
        /// <param name="bToRMB"></param>
        /// <returns></returns>
        private static String ConvertFractional(String strFractional)
        {
            Char[] fractional = strFractional.ToCharArray();

            StringBuilder sbFrac = new StringBuilder();

            // 变成中文数字
            for (Int32 i = 0; i < fractional.Length; i++)
            {
                sbFrac.Append(chnText[fractional[i] - '0']);
            }

            return sbFrac.ToString();
        }
        #endregion

        #region 全角转半角
        public static string Convert全角2半角(string str)
        {
            StringBuilder sb = new StringBuilder(str);

            string str全角 = "１２３４５６７８９０，．（）＋－×％ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ";
            string str半角 = "1234567890,.()+-*%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int i = 0; i < str全角.Length; i++)
            {
                sb.Replace(str全角[i], str半角[i]);
            }

            return sb.ToString();
        }
        #endregion

        #region 将SQL中单引号替换成2个单引号
        public static string GetString4SQL(string s)
        {
            return s.Replace("'", "''");
        }
        #endregion

        #region Object转字符串
        public static String ObjectToString(object obj)
        {
            return (obj == null ? String.Empty : obj.ToString());
        }
        #endregion
    }
}
