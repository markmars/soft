using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MarkMars.Database
{
    public static class Utility
    {
        public static int? ReadIntFromDataRow(DataRow row, string strField)
        {
            if (row.IsNull(strField))
                return null;
            else
                return System.Convert.ToInt32(row[strField]);
        }

        public static string GetSQLStringForNullable(int? n)
        {
            return n == null ? "null" : n.ToString();
        }

        public static string GetSQLStringForNullable(bool? n)
        {
            return n == null ? "null" : n.ToString();
        }

        public static string GetSQLStringForNullable(double? n)
        {
            return n == null ? "null" : n.ToString();
        }
    }
}
