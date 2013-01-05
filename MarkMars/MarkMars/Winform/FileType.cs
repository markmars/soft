using System;
using System.Collections.Generic;
using System.Text;

namespace MarkMars.Winform
{
    public class FileType
    {
        public static void CheckTrueFileName(string filePath)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string bx = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                bx = buffer.ToString();
                buffer = r.ReadByte();
                bx += buffer.ToString();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("检测文件发生错误:" + Environment.NewLine + e.StackTrace);
                return;
            }
            r.Close();
            fs.Close();
            string str真实文件类型 = bx;
            string str带格式的文件名 = System.IO.Path.GetFileName(filePath);
            string str不带格式的文件名 = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string str文件格式 = System.IO.Path.GetExtension(filePath);
        }

        public enum 文件格式
        {
            JPG = 255216,
            GIF = 7173,
            BMP = 6677,
            PNG = 13780,
            COM = 7790,
            EXE = 7790,
            DLL = 7790,
            RAR = 8297,
            ZIP = 8075,
            XML = 6063,
            HTML = 6033,
            ASPX = 239187,
            CS = 117115,
            JS = 119105,
            TXT = 210187,
            SQL = 255254,
            BAT = 64101,
            BTSEED = 10056,
        }
    }
}
