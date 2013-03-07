using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace MarkMars.Common.Winform
{
    class RegisterClass
    {
        //步骤一: 获得CUP序列号和硬盘序列号的实现代码如下:
        //获得CPU的序列号

        bool Stupids = true;
        bool Cat = false;
        public string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        //取得设备硬盘的卷标号

        public string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc =
                 new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk =
                 new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }


        //步骤二: 收集硬件信息生成机器码, 代码如下: 
        //生成机器码

        public string CreateCode()
        {
            string temp = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
            string[] strid = new string[24];//
            for (int i = 0; i < 24; i++)//把字符赋给数组
            {
                strid[i] = temp.Substring(i, 1);
            }
            temp = "";
            //Random rdid = new Random();
            for (int i = 0; i < 24; i++)//从数组随机抽取24个字符组成新的字符生成机器三
            {
                //temp += strid[rdid.Next(0, 24)];
                temp += strid[i + 3 >= 24 ? 0 : i + 3];
            }
            return GetMd5(temp);
        }

        //步骤三: 使用机器码生成软件注册码, 代码如下:
        //使用机器码生成注册码
        public int[] intCode = new int[127];//用于存密钥

        public void setIntCode()//给数组赋值个小于10的随机数
        {
            //Random ra = new Random();
            //for (int i = 1; i < intCode.Length;i++ )
            //{
            //    intCode[i] = ra.Next(0, 9);
            //}
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i + 3 > 9 ? 0 : i + 3;
            }
        }
        public int[] intNumber = new int[25];//用于存机器码的Ascii值
        public char[] Charcode = new char[25];//存储机器码字

        //生成注册码
        public string GetCode(string code)
        {
            if (code != "")
            {
                //把机器码存入数组中
                setIntCode();//初始化127位数组
                for (int i = 1; i < Charcode.Length; i++)//把机器码存入数组中
                {
                    Charcode[i] = Convert.ToChar(code.Substring(i - 1, 1));
                }//
                for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。
                {
                    intNumber[j] =
                       intCode[Convert.ToInt32(Charcode[j])] +
                       Convert.ToInt32(Charcode[j]);

                }
                string strAsciiName = null;//用于存储机器码
                for (int j = 1; j < intNumber.Length; j++)
                {
                    //MessageBox.Show((Convert.ToChar(intNumber[j])).ToString());
                    //判断字符ASCII值是否0－9之间

                    if (intNumber[j] >= 48 && intNumber[j] <= 57)
                    {
                        strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                    }
                    //判断字符ASCII值是否A－Z之间

                    else if (intNumber[j] >= 65 && intNumber[j] <= 90)
                    {
                        strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                    }
                    //判断字符ASCII值是否a－z之间


                    else if (intNumber[j] >= 97 && intNumber[j] <= 122)
                    {
                        strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                    }
                    else//判断字符ASCII值不在以上范围内
                    {
                        if (intNumber[j] > 122)//判断字符ASCII值是否大于z
                        {
                            strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
                        }
                        else
                        {
                            strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
                        }

                    }
                    //label3.Text = strAsciiName;//得到注册码
                }
                return strAsciiName;
            }
            else
            {
                return "";
            }
        }


        //步骤四: 用户输入注册码注册软件, 演示代码如下:

        //注册
        public bool RegistIt(string currentCode, string realCode)
        {
            if (realCode != "")
            {
                if (currentCode.TrimEnd().Equals(realCode.TrimEnd()))
                {
                    Microsoft.Win32.RegistryKey retkey =
                         Microsoft.Win32.Registry.CurrentUser.
                         OpenSubKey("software", true).CreateSubKey("StupidsCat").
                         CreateSubKey("StupidsCat.ini").
                         CreateSubKey(currentCode.TrimEnd());
                    retkey.SetValue("StupidsCat", "BBC6D58D0953F027760A046D58D52786");

                    retkey = Microsoft.Win32.Registry.LocalMachine.
                        OpenSubKey("software", true).CreateSubKey("StupidsCat").
                         CreateSubKey("StupidsCat.ini").
                         CreateSubKey(currentCode.TrimEnd());
                    retkey.SetValue("StupidsCat", "BBC6D58D0953F027760A046D58D52786");

                    return Stupids;
                }
                else
                {
                    return Cat;
                }
            }
            else { return Cat; }
        }

        public bool BoolRegist(string sn)
        {
            string[] keynames; bool flag = false;
            Microsoft.Win32.RegistryKey localRegKey = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey userRegKey = Microsoft.Win32.Registry.CurrentUser;
            try
            {
                keynames = localRegKey.OpenSubKey("software\\StupidsCat\\StupidsCat.ini\\" + GetMd5(sn)).GetValueNames();
                foreach (string name in keynames)
                {
                    if (name == "StupidsCat")
                    {
                        if (localRegKey.OpenSubKey("software\\StupidsCat\\StupidsCat.ini\\" + GetMd5(sn)).GetValue("StupidsCat").ToString() == "BBC6D58D0953F027760A046D58D52786")
                            flag = true;
                    }
                }
                keynames = userRegKey.OpenSubKey("software\\StupidsCat\\StupidsCat.ini\\" + GetMd5(sn)).GetValueNames();
                foreach (string name in keynames)
                {
                    if (name == "StupidsCat")
                    {
                        if (flag && userRegKey.OpenSubKey("software\\StupidsCat\\StupidsCat.ini\\" + GetMd5(sn)).GetValue("StupidsCat").ToString() == "BBC6D58D0953F027760A046D58D52786")
                            return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                localRegKey.Close();
                userRegKey.Close();
            }
        }

        public string GetMd5(object text)
        {
            string path = text.ToString();

            MD5CryptoServiceProvider MD5Pro = new MD5CryptoServiceProvider();
            Byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(text.ToString());
            Byte[] byteResult = MD5Pro.ComputeHash(buffer);

            string md5result = BitConverter.ToString(byteResult).Replace("-", "");
            return md5result;
        }
    }
}