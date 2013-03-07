using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MarkMars.Common.Database
{
    public static class Utility
    {
        public static int? ReadNullableIntFromDataRow(DataRow row, string strField)
        {
            if (row.IsNull(strField))
                return null;
            else
                return System.Convert.ToInt32(row[strField]);
        }

        public static int ReadIntFromDataRow(DataRow row, string strField, int nValueForNull = 0)
        {
            if (row.IsNull(strField))
                return nValueForNull;
            else
                return System.Convert.ToInt32(row[strField]);
        }

        public static decimal ReadDecimalFromDataRow(DataRow row, string strField, decimal dValueForNull = 0)
        {
            if (row.IsNull(strField))
                return dValueForNull;
            else
                return System.Convert.ToDecimal(row[strField]);
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

		public static string GetSQLStringForNullable(DateTime? n)
		{
			return n == null ? "null" : "'" + n.ToString() + "'";
		}
    }
}
