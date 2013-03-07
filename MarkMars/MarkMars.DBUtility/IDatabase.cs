using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.OleDb;

namespace MarkMars.DBUtility
{
    internal interface IDatabase
    {
        DataTable GetTableBySQL(string strSQL);
        DataRow GetRowBySQL(string strSQL);
        void ExecuteSQL(string strSQL);
        int GetLastID();
        bool TableExists(string strTableName);
        bool FieldExists(string strTableName, string strFieldName);
        void CloseConnection();
		void OpenConnection();
    }
}
