//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data;
//using System.Data.SqlServerCe;
//using System.Data.OleDb;

//namespace MarkMars.Database
//{
//    public class SQLServerCEDatabase : IFileDatabase
//    {
//        /// <summary>
//        /// 连接字符串
//        /// </summary>
//        private SqlCeConnection m_Connection = null;

//        /// <summary>
//        /// 文件完整路径
//        /// </summary>
//        private string m_strFileName = null;

//        /// <summary>
//        /// sqlserver compact数据构造函数
//        /// </summary>
//        /// <param name="strFileName">文件完整路径</param>
//        /// <param name="strPassword">文件密码</param>
//        /// <param name="bReadOnly">是否只读</param>
//        public SQLServerCEDatabase(string strFileName, string strPassword, bool bReadOnly)
//        {
//            m_strFileName = strFileName;

//            if (bReadOnly)
//            {
//                try
//                {
//                    OpenReadOnly(strFileName, strPassword);
//                }
//                catch
//                {
//                    OpenReadWrite(strFileName, strPassword);
//                }
//            }
//            else
//                OpenReadWrite(strFileName, strPassword);
//        }

//        /// <summary>
//        /// 只读打开
//        /// </summary>
//        /// <param name="strFileName">文件完整路径</param>
//        /// <param name="strPassword">文件密码</param>
//        private void OpenReadOnly(string strFileName, string strPassword)
//        {
//            string strConnection = string.Format("Data Source={0};Password={1};File Mode=Read Only;ssce:temp file directory='{2}'",
//                    strFileName,
//                    strPassword,
//                    System.IO.Path.GetTempPath());
//            m_Connection = new SqlCeConnection(strConnection);
//            OpenConnection();
//        }

//        /// <summary>
//        /// 读写打开
//        /// </summary>
//        /// <param name="strFileName">文件完整路径</param>
//        /// <param name="strPassword">文件密码</param>
//        private void OpenReadWrite(string strFileName, string strPassword)
//        {
//            string strConnection = string.Format("Data Source={0};Password={1}",
//                    strFileName,
//                    strPassword);
//            m_Connection = new SqlCeConnection(strConnection);
//            OpenConnection();
//        }

//        /// <summary>
//        /// 获得DataTable
//        /// </summary>
//        /// <param name="strSQL">sql语句</param>
//        /// <returns>DataTable</returns>
//        public DataTable GetTableBySQL(string strSQL)
//        {
//            SqlCeCommand cmd = new SqlCeCommand(null, m_Connection);
//            cmd.CommandType = CommandType.Text;
//            cmd.CommandText = strSQL;

//            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);

//            DataTable dTable = new DataTable();
//            da.Fill(dTable);

//            return dTable;
//        }

//        /// <summary>
//        /// 获得DataRow
//        /// </summary>
//        /// <param name="strSQL">sql语句</param>
//        /// <returns>DataRow</returns>
//        public DataRow GetRowBySQL(string strSQL)
//        {
//            DataTable dTable = GetTableBySQL(strSQL);

//            if (dTable.Rows.Count == 0)
//                return null;
//            else
//                return dTable.Rows[0];
//        }

//        /// <summary>
//        /// 执行sql语句
//        /// </summary>
//        /// <param name="strSQL">sql语句</param>
//        public void ExecuteSQL(string strSQL)
//        {
//            SqlCeCommand cmd = new SqlCeCommand(null, m_Connection);
//            cmd.CommandType = CommandType.Text;
//            cmd.CommandText = strSQL;
//            cmd.ExecuteNonQuery();
//        }

//        /// <summary>
//        /// 获得最大id
//        /// </summary>
//        /// <returns>最大id</returns>
//        public int GetLastID()
//        {
//            SqlCeCommand cmd = new SqlCeCommand(null, m_Connection);
//            cmd.CommandType = CommandType.Text;
//            cmd.CommandText = "select @@identity";
//            return System.Convert.ToInt32(cmd.ExecuteScalar());
//        }

//        /// <summary>
//        /// 判断Access库中是否有相应的表
//        /// </summary>
//        /// <param name="strTableName">表名</param>
//        /// <returns>是否存在</returns>
//        public bool TableExists(string strTableName)
//        {
//            string strSQL = string.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{0}'",
//                strTableName);
//            DataRow row = GetRowBySQL(strSQL);
//            if (row == null)
//                return false;
//            else
//                return true;
//        }


//        /// <summary>
//        /// 判断字段是否在表中存在
//        /// </summary>
//        /// <param name="strTableName">表名</param>
//        /// <param name="strFieldName">字段名</param>
//        /// <returns>是否存在</returns>
//        public bool FieldExists(string strTableName, string strFieldName)
//        {
//            string strSQL = string.Format("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{0}' and COLUMN_NAME=N'{1}'",
//                strTableName,
//                strFieldName);
//            DataRow row = GetRowBySQL(strSQL);
//            if (row == null)
//                return false;
//            else
//                return true;
//        }

//        /// <summary>
//        /// 关闭连接
//        /// </summary>
//        public void CloseConnection()
//        {
//            m_Connection.Close();
//        }

//        /// <summary>
//        /// 打开连接
//        /// </summary>
//        public void OpenConnection()
//        {
//            m_Connection.Open();
//        }

//        /// <summary>
//        /// 文件完整路径
//        /// </summary>
//        public string FileName
//        {
//            get
//            {
//                return m_strFileName;
//            }
//        }
//    }
//}
