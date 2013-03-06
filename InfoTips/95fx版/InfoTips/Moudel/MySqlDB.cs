using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace InfoTips
{
    class MySqlDB
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static MySqlConnection m_Connection = null;

        /// <summary>
        /// 数据库构造函数
        /// </summary>
        /// <param name="strServerName">服务器名称</param>
        /// <param name="strDatabaseName">数据库名称</param>
        /// <param name="strUserName">用户名</param>
        /// <param name="strPassword">密码</param>
        public MySqlDB(string strServerName, string strDatabaseName, string strUserName, string strPassword)
        {
            string strConnection = string.Format("server={0};database={1};port=3306;user id={2};password={3};CharSet=utf8;Allow Zero Datetime=true", strServerName, strDatabaseName, strUserName, strPassword);
            m_Connection = new MySqlConnection(strConnection);
            OpenConnection();
        }
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableBySQL(string strSQL)
        {
            MySqlCommand cmd = new MySqlCommand(strSQL, m_Connection);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

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
            MySqlCommand cmd = new MySqlCommand(strSQL, m_Connection);
            cmd.ExecuteNonQuery();
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
