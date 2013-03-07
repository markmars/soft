using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MarkMars.Common
{
	public class ZipWrapper
	{
		public static void Zip_7Z(string strSourceFile, string strInternalFile, string strTargetFile, string strPassword)
		{
			string strTempFile = TempFileManager.GetNewFileName();

			SevenZip.SevenZipCompressor szc = new SevenZip.SevenZipCompressor();
			szc.CompressionLevel = SevenZip.CompressionLevel.Fast;
			Dictionary<string, string> dictFile = new Dictionary<string, string>();
			dictFile.Add(strInternalFile, strSourceFile);

			if (string.IsNullOrEmpty(strPassword))
				szc.CompressFileDictionary(dictFile, strTempFile);
			else
				szc.CompressFileDictionary(dictFile, strTempFile, strPassword);

            if (BinaryFileFormatReader.GetFormat(strTempFile) != BinaryFileFormat._7z)
                throw new Exception("压缩失败, 无法保存");

			System.IO.File.Copy(strTempFile, strTargetFile, true);

			System.IO.File.Delete(strTempFile);
		}


		public static void Unzip(string strSourceFile, string strInternalFile, string strTargetFile, string strPassword)
		{
			SevenZip.SevenZipExtractor sze;
			if (string.IsNullOrEmpty(strPassword))
				sze = new SevenZip.SevenZipExtractor(strSourceFile);
			else
				sze = new SevenZip.SevenZipExtractor(strSourceFile, strPassword);

			StreamWriter sw = new StreamWriter(strTargetFile);
			sze.ExtractFile(strInternalFile, sw.BaseStream);
			sw.Close();
		}

		public static bool Verify(string strSourceFile, string strPassword)
		{
			FileStream fs = File.OpenRead(strSourceFile);

			Byte[] header = new Byte[10];
			fs.Read(header, 0, header.Length);
			String headerString = new UTF8Encoding().GetString(header);

			Stream srRar;
			if (headerString == "TrueLoreCA" || headerString == "RrueLoreCA")
			{
				Byte[] signCount = new Byte[4];
				fs.Read(signCount, 0, signCount.Length);
				Int32 signLength = BitConverter.ToInt32(signCount, 0);

				srRar = new OffsetStream(fs, signLength + 14);
			}
			else if (headerString.StartsWith("Rar!"))
			{
				srRar = fs;
			}
			else
				return false;

			String applicationPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "bin\\7z.dll";
			if(File.Exists(applicationPath))
				SevenZip.SevenZipExtractor.SetLibraryPath(applicationPath);

			SevenZip.SevenZipExtractor sze = new SevenZip.SevenZipExtractor(srRar, "truelore");
			bool bOK = sze.Check();
			sze.Dispose();
			fs.Close();
			return bOK;
		}
	}
}
