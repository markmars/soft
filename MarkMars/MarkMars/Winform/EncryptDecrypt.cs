using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MarkMars.Winform
{
	public static class EncryptDecrypt
	{
		private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
		public static string Encrypt(string encryptString)
		{
			byte[] rgbKey = Encoding.UTF8.GetBytes("markmars");
			byte[] rgbIV = Keys;
			byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
			DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
			cStream.Write(inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock();
			return Convert.ToBase64String(mStream.ToArray());
		}

		public static string Decrypt(string decryptString)
		{
            byte[] rgbKey = Encoding.UTF8.GetBytes("markmars");
			byte[] rgbIV = Keys;
			byte[] inputByteArray = Convert.FromBase64String(decryptString);
			DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
			cStream.Write(inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock();
			return Encoding.UTF8.GetString(mStream.ToArray());
		}
	}
}
