using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace MarkMars.Database
{
    public class SQLServerDatabase : IDatabase
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static SqlConnection m_Connection = null;

        /// <summary>
        /// sqlserver数据库构造函数
        /// </summary>
        /// <param name="strServerName">服务器名称</param>
        /// <param name="strDatabaseName">数据库名称</param>
        /// <param name="strUserName">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <param name="bIntegratedSecurity">综合安全</param>
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

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <returns>DataTable</returns>
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

        /// <summary>
        /// 获取DataRow
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <returns>DataRow</returns>
        public DataRow GetRowBySQL(string strSQL)
        {
            DataTable dTable = GetTableBySQL(strSQL);

            if (dTable.Rows.Count == 0)
                return null;
            else
                return dTable.Rows[0];
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        public void ExecuteSQL(string strSQL)
        {
            SqlCommand cmd = new SqlCommand(null, m_Connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSQL;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 获得最大id
        /// </summary>
        /// <returns>最大id</returns>
        public int GetLastID()
        {
            SqlCommand cmd = new SqlCommand(null, m_Connection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select @@identity";
            return System.Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// 判断Access库中是否有相应的表
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns>是否存在</returns>
        public bool TableExists(string strTableName)
        {
            string sql = "select * from sysobjects where type='U' and name='" + strTableName + "'";
            SqlDataAdapter sqlda = new SqlDataAdapter(sql, m_Connection);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            return (ds.Tables[0].Rows.Count != 0);
        }


        /// <summary>
        /// 判断字段是否在表中存在
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <param name="strFieldName">字段名</param>
        /// <returns></returns>
        public bool FieldExists(string strTableName, string strFieldName)
        {
            string sql = "select * from syscolumns where id=object_id('" + strTableName + "') and name='" + strFieldName + "'";
            SqlDataAdapter sqlda = new SqlDataAdapter(sql, m_Connection);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            return (ds.Tables[0].Rows.Count != 0);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection()
        {
            m_Connection.Close();
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public void OpenConnection()
        {
            m_Connection.Open();
        }
    }
}
