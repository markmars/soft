using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MarkMars.DBUtility
{
    public class FileDatabase : IFileDatabase
    {
        private IFileDatabase m_imp = null;
        private string m_strFileName = null;

        public FileDatabase(string strFile, string strPassword, bool bReadonly)
        {
            m_strFileName = strFile;

            BinaryFileFormat bff = BinaryFileFormatReader.GetFormat(strFile);
            switch (bff)
            {
                case BinaryFileFormat.mdb:
                    m_imp = new AccessDatabase(strFile, strPassword, bReadonly);
                    break;
                case BinaryFileFormat.SQLServerCE:
                    m_imp = new SQLServerCEDatabase(strFile, strPassword, bReadonly);
                    break;
                default:
                    throw new Exception("未知的数据库文件格式");
            }
        }

        public void OpenConnection()
        {
            m_imp.OpenConnection();
        }

        public void CloseConnection()
        {
            m_imp.CloseConnection();
        }

        public bool FieldExists(string strTableName, string strFieldName)
        {
            return m_imp.FieldExists(strTableName, strFieldName);
        }

        public bool TableExists(string strTableName)
        {
            return m_imp.TableExists(strTableName);
        }

        public int GetLastID()
        {
            return m_imp.GetLastID();
        }

        public void ExecuteSQL(string strSQL)
        {
            m_imp.ExecuteSQL(strSQL);
        }

        public DataRow GetRowBySQL(string strSQL)
        {
            return m_imp.GetRowBySQL(strSQL);
        }

        public DataTable GetTableBySQL(string strSQL)
        {
            return m_imp.GetTableBySQL(strSQL);
        }

        public string FileName
        {
            get
            {
                return m_strFileName;
            }
        }

		public void SaveCopy(string strFileName)
		{
			m_imp.CloseConnection();
			System.IO.File.Copy(m_imp.FileName, strFileName, true);
			m_imp.OpenConnection();
		}
    }
}
