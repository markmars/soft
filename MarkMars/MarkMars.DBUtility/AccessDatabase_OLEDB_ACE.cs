using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace MarkMars.DBUtility
{
    internal class AccessDatabase_OLEDB_ACE : IFileDatabase
    {
        private OleDbConnection m_Connection = null;

		public AccessDatabase_OLEDB_ACE(string strMDBPath, string strPassword)
        {
            string strConnection;
			if (strPassword == null)
				strConnection = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}",
					strMDBPath);
			else
				strConnection = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Jet OLEDB:Database Password={1}",
					strMDBPath,
					strPassword);
            m_Connection = new OleDbConnection(strConnection);
			OpenConnection();
        }

        public string FileName
        {
			get
			{
				return m_Connection.DataSource;
			}
        }

        public DataTable GetTableBySQL(string strSQL)
        {
            DataTable dTable = new DataTable("MyTable");
            OleDbDataAdapter da = new OleDbDataAdapter(strSQL, m_Connection);
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
            OleDbCommand cmd = new OleDbCommand(strSQL, m_Connection);
            cmd.ExecuteNonQuery();
        }

		public int GetLastID()
		{
			OleDbCommand selcmd = new OleDbCommand();
			selcmd.Connection = m_Connection;
			selcmd.CommandText = "select @@identity";
			selcmd.CommandType = CommandType.Text;
			return System.Convert.ToInt32(selcmd.ExecuteScalar());
		}

        public void CloseConnection()
        {
            m_Connection.Close();
        }

        //判断Access库中是否有相应的表
        public bool TableExists(string strTableName)
        {
            DataTable schemaTable = m_Connection.GetOleDbSchemaTable(
                OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "TABLE" });

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
            DataTable columnTable = m_Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, strTableName, null });
            foreach (DataRow dr in columnTable.Rows)
            {
                if (string.Compare(strFieldName, dr["COLUMN_NAME"].ToString(), true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

		public void OpenConnection()
		{
			m_Connection.Open();
		}
    }
}
