using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Data.OleDb;

namespace MarkMars.Common.Database
{
    public class SQLServerCEDatabase : IFileDatabase
    {
        private SqlCeConnection m_Connection = null;
        private string m_strFileName = null;

        public SQLServerCEDatabase(string strFileName, string strPassword, bool bReadOnly)
		{
            m_strFileName = strFileName;

            if (bReadOnly)
            {
                try
                {
                    OpenReadOnly(strFileName, strPassword);
                }
                catch
                {
                    OpenReadWrite(strFileName, strPassword);
                }
            }
            else
                OpenReadWrite(strFileName, strPassword);
		}

        private void OpenReadOnly(string strFileName, string strPassword)
        {
            string strConnection = string.Format("Data Source='{0}';Password={1};File Mode=Read Only;ssce:temp file directory='{2}'",
                    strFileName,
                    strPassword,
                    System.IO.Path.GetTempPath());
            m_Connection = new SqlCeConnection(strConnection);
            OpenConnection();
        }

        private void OpenReadWrite(string strFileName, string strPassword)
        {
            string strConnection = string.Format("Data Source='{0}';Password={1}",
                    strFileName,
                    strPassword);
            m_Connection = new SqlCeConnection(strConnection);
            OpenConnection();
        }

        public DataTable GetTableBySQL(string strSQL)
        {
            SqlCeCommand cmd = new SqlCeCommand(null, m_Connection);
            cmd.CommandType = CommandType.Text;
			cmd.CommandText = strSQL;

            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);

            DataTable dTable = new DataTable();
            da.Fill(dTable);

            return dTable;
        }

        public DataRow GetRowBySQL(string strSQL)
        {
            DataTable dTable = GetTableBySQL(strSQL);

            if (dTable.Rows.Count == 0)
                return null;
            else
                return dTable.Rows[0];
        }

        public void ExecuteSQL(string strSQL)
        {
            SqlCeCommand cmd = new SqlCeCommand(null, m_Connection);
            cmd.CommandType = CommandType.Text;
			cmd.CommandText = strSQL;
            cmd.ExecuteNonQuery();
        }

        public int GetLastID()
        {
            SqlCeCommand cmd = new SqlCeCommand(null, m_Connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select @@identity";
            return System.Convert.ToInt32(cmd.ExecuteScalar());
        }

        //判断Access库中是否有相应的表
        public bool TableExists(string strTableName)
        {
            string strSQL = string.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{0}'",
                strTableName);
            DataRow row = GetRowBySQL(strSQL);
            if (row == null)
                return false;
            else
                return true;
        }


        //判断字段是否存在 FieldExists
        public bool FieldExists(string strTableName, string strFieldName)
        {
            string strSQL = string.Format("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{0}' and COLUMN_NAME=N'{1}'",
                strTableName,
                strFieldName);
            DataRow row = GetRowBySQL(strSQL);
            if (row == null)
                return false;
            else
                return true;
        }

        public void CloseConnection()
        {
            m_Connection.Close();
        }

		public void OpenConnection()
		{
			m_Connection.Open();
		}

        public string FileName
        {
            get
            {
                return m_strFileName;
            }
        }
    }
}
