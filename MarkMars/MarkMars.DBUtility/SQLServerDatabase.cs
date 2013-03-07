using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace MarkMars.DBUtility
{
    public class SQLServerDatabase : IDatabase
    {
        private static SqlConnection m_Connection = null;

        public SQLServerDatabase(string strServerName, string strDatabaseName, string strUserName, string strPassword, bool bIntegratedSecurity)
		{
            string strConnection = "data source = " + strServerName + ";initial catalog = " + strDatabaseName;
			if (bIntegratedSecurity)
			{
                strConnection += ";Integrated Security = SSPI";
			}
			else
			{
                strConnection += ";user id = ";
                strConnection += strUserName;
                strConnection += ";password = ";
                strConnection += strPassword;
			}
            m_Connection = new SqlConnection(strConnection);
			OpenConnection();
		}

        public DataTable GetTableBySQL(string strSQL)
        {
            SqlCommand cmd = new SqlCommand(null, m_Connection);
            cmd.CommandType = CommandType.Text;
			cmd.CommandText = strSQL;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

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
            SqlCommand cmd = new SqlCommand(null, m_Connection);
            cmd.CommandType = CommandType.Text;
			cmd.CommandText = strSQL;
            cmd.ExecuteNonQuery();
        }

        public int GetLastID()
        {
            SqlCommand cmd = new SqlCommand(null, m_Connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select @@identity";
            return System.Convert.ToInt32(cmd.ExecuteScalar());
        }

        //判断Access库中是否有相应的表
        public bool TableExists(string strTableName)
        {
            string sql = "select * from sysobjects where type='U' and name='" + strTableName + "'";
            SqlDataAdapter sqlda = new SqlDataAdapter(sql, m_Connection);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            return (ds.Tables[0].Rows.Count != 0);
        }


        //判断字段是否存在 FieldExists
        public bool FieldExists(string strTableName, string strFieldName)
        {
            string sql = "select * from syscolumns where id=object_id('" + strTableName + "') and name='" + strFieldName + "'";
            SqlDataAdapter sqlda = new SqlDataAdapter(sql, m_Connection);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            return (ds.Tables[0].Rows.Count != 0);
        }

        public void CloseConnection()
        {
            m_Connection.Close();
        }

		public void OpenConnection()
		{
			m_Connection.Open();
		}
    }
}
