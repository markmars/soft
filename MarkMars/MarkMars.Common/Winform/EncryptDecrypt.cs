using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MarkMars.Common.Winform
{
    public class EncryptDecrypt
    {
        private static readonly Byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }; //默认密钥向量
        private static readonly String encryptionKey = "1qaz3edc";

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="sourceString">要加密的字符串。</param>
        /// <param name="encryptionKey">密钥。</param>
        /// <returns>加密后的字符串。</returns>
        public static String EncryptString(String sourceString)
        {
            try
            {
                Byte[] rgbKey = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 8));
                Byte[] rgbIV = iv;
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(sourceString);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="sourceString">要解密的字符串。</param>
        /// <param name="encryptionKey">密钥。</param>
        /// <returns>解密后的字符串。</returns>
        public static String DecryptString(String sourceString)
        {
            try
            {
                Byte[] rgbKey = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 8));
                Byte[] rgbIV = iv;
                Byte[] inputByteArray = Convert.FromBase64String(sourceString);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
