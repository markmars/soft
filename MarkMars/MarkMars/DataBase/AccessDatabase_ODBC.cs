using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace MarkMars.Database
{
	public class AccessDatabase_ODBC : IFileDatabase
    {
        private OdbcConnection m_Connection = null;
		private string m_strFileName = null;

        public AccessDatabase_ODBC(string strMDBPath, string strPassword)
        {
            string strConnection;
			if (strPassword == null)
				strConnection = "Driver={Microsoft Access Driver (*.mdb)};" + string.Format("Dbq={0};Uid=Admin;Pwd=;",
					strMDBPath);
			else
				strConnection = "Driver={Microsoft Access Driver (*.mdb)};" + string.Format("Dbq={0};Uid=Admin;Pwd={1};",
					strMDBPath,
					strPassword);
			m_Connection = new OdbcConnection(strConnection);
			OpenConnection();
			m_strFileName = strMDBPath;
        }

        public string FileName
        {
			get
			{
				return m_strFileName;
			}
        }

        public DataTable GetTableBySQL(string strSQL)
        {
            DataTable dTable = new DataTable("MyTable");
			OdbcDataAdapter da = new OdbcDataAdapter(strSQL, m_Connection);
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
			OdbcCommand cmd = new OdbcCommand(strSQL, m_Connection);
            cmd.ExecuteNonQuery();
        }

		public int GetLastID()
		{
			OdbcCommand selcmd = new OdbcCommand();
			selcmd.Connection = m_Connection;
			selcmd.CommandText = "select @@identity";
			selcmd.CommandType = CommandType.Text;
			return System.Convert.ToInt32(System.Convert.ToInt32(selcmd.ExecuteScalar()));
		}

        public void CloseConnection()
        {
            m_Connection.Close();
        }

        //判断Access库中是否有相应的表
        public bool TableExists(string strTableName)
        {
            DataTable schemaTable = m_Connection.GetSchema("Tables");

			foreach (DataRow row in schemaTable.Rows)
			{
				if (string.Compare(row["TABLE_NAME"].ToString(), strTableName, true) == 0)
					return true;
			}
            return false;
        }

        //判断字段是否存在 FieldExists
        public bool FieldExists(string strTableName, string strFieldName)
        {
			DataTable schemaTable = m_Connection.GetSchema("Columns");

			foreach (DataRow row in schemaTable.Rows)
			{
				if (string.Compare(row["TABLE_NAME"].ToString(), strTableName, true) == 0 && string.Compare(row["COLUMN_NAME"].ToString(), strFieldName, true) == 0)
					return true;
			}
			return false;
        }

		public void OpenConnection()
		{
			m_Connection.Open();
		}
    }
}
