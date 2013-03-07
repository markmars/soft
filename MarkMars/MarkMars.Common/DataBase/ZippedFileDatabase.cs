using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MarkMars.Common.Database
{
	/// <summary>
	/// 压缩文件中的mdb数据库. 这个压缩文件, 未必是zip格式. 这里的zip,仅仅代表被压缩而已
	/// 
	/// 在压缩的时候,使用的是7z格式, 来自[http://sevenzipsharp.codeplex.com], 一个提供C#接口的开源库
	/// </summary>
	public class ZippedFileDatabase : IDatabase
	{
		public string ExternalFileName
		{
			get;
			private set;
		}

		public string ExternalPassword
		{
			get;
			private set;
		}

		public string InternalFileName
		{
			get;
			private set;
		}

		public string InternalPassword
		{
			get;
			private set;
		}

		public FileDatabase UnzippedFileDatabase
		{
			get;
			private set;
		}

		public ZippedFileDatabase(string strZippedFileName, string strPasswordForZip, string strInternalFileName, string strInternalPassword, string strZIPFormat)
		{
			ExternalFileName = strZippedFileName;
			ExternalPassword = strPasswordForZip;
			InternalFileName = strInternalFileName;
			InternalPassword = strInternalPassword;

			string strTempName = TempFileManager.GetNewFileName();
            ZipWrapper.Unzip(ExternalFileName, strInternalFileName, strTempName, strPasswordForZip);
			UnzippedFileDatabase = new FileDatabase(strTempName, strInternalPassword, false);
		}

		public void Save()
		{
			SaveCopy(ExternalFileName);
		}

		public void SaveCopy(string strTargetFileName)
		{
			ZipWrapper.Zip_7Z(UnzippedFileDatabase.FileName, "project.mdb", strTargetFileName, null);
		}

        public void SaveOriginalCopy(string strTargetFileName)
        {
            System.IO.File.Copy(ExternalFileName, strTargetFileName, true);
        }

		public DataTable GetTableBySQL(string strSQL)
		{
			return UnzippedFileDatabase.GetTableBySQL(strSQL);
		}

		public DataRow GetRowBySQL(string strSQL)
		{
			return UnzippedFileDatabase.GetRowBySQL(strSQL);
		}

		public void ExecuteSQL(string strSQL)
		{
			UnzippedFileDatabase.ExecuteSQL(strSQL);
		}

		public int GetLastID()
		{
			return UnzippedFileDatabase.GetLastID();
		}

		public bool TableExists(string strTableName)
		{
			return UnzippedFileDatabase.TableExists(strTableName);
		}

		public bool FieldExists(string strTableName, string strFieldName)
		{
			return UnzippedFileDatabase.FieldExists(strTableName, strFieldName);
		}

		public void CloseConnection()
		{
			UnzippedFileDatabase.CloseConnection();
		}

		public void OpenConnection()
		{
			UnzippedFileDatabase.OpenConnection();
		}
	}
}
