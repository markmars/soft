using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MarkMars.Winform
{
    public static class BinaryFileFormatReader
    {
        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <param name="strFile">文件完整路径</param>
        /// <returns></returns>
        public static BinaryFileFormat GetFormat(string strFile)
        {
            FileStream fs = new FileStream(strFile, FileMode.Open, FileAccess.Read);
            byte[] bs = new byte[128];
            fs.Read(bs, 0, 128);
            fs.Close();

            if (IsMDBFile(bs))
                return BinaryFileFormat.mdb;
            else if (Is7zFile(bs))
                return BinaryFileFormat._7z;
            else if (IsZipFile(bs))
                return BinaryFileFormat.zip;
            else if (IsSQLServerCE40File(bs))
                return BinaryFileFormat.SQLServerCE;
            else
                return BinaryFileFormat.unknown;
        }

        /// <summary>
        /// 判断是否mdb文件
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        private static bool IsMDBFile(byte[] bs)
        {
            byte[] bsHeader = new byte[] { 0, 1, 0, 0, 0x53, 0x74, 0x61, 0x6E, 0x64, 0x61, 0x72, 0x64, 0x20, 0x4A, 0x65, 0x74, 0x20, 0x44, 0x42 };

            for (int i = 0; i < bsHeader.Length; i++)
            {
                if (bs[i] != bsHeader[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 判断是否7z文件
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        private static bool Is7zFile(byte[] bs)
        {
            byte[] bsHeader = new byte[] { 0x37, 0x7A };


            for (int i = 0; i < bsHeader.Length; i++)
            {
                if (bs[i] != bsHeader[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 判断是否zip文件
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        private static bool IsZipFile(byte[] bs)
        {
            byte[] bsHeader = new byte[] { 0x50, 0x4B };


            for (int i = 0; i < bsHeader.Length; i++)
            {
                if (bs[i] != bsHeader[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 判断是否sdf文件
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        private static bool IsSQLServerCE40File(byte[] bs)
        {
            byte[] bsHeader = new byte[] { 0x06, 0xA5, 0x54, 0xC5 };

            for (int i = 0; i < bsHeader.Length; i++)
            {
                if (bs[i] != bsHeader[i])
                    return false;
            }

            return true;
        }
    }
}
