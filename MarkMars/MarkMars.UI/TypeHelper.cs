using System;

namespace MarkMars.UI
{
    public static class TypeHelper
    {
        public static String DateTimeToString(DateTime dateTime)
        {
            if (dateTime == null || dateTime >= DateTime.MaxValue.Date || dateTime <= DateTime.MinValue)
            {
                return String.Empty;
            }

            return dateTime.ToString("yyyy-MM-dd HH:mm");
        }

        public static String DateTimeToContainNullString(DateTime dateTime)
        {
            if (dateTime == null)
            {
                return String.Empty;
            }

            return dateTime.ToString("yyyy-MM-dd HH:mm");
        }

        public static String DateTimeToLongString(DateTime dateTime)
        {
            if (dateTime == null || dateTime >= DateTime.MaxValue.Date || dateTime <= DateTime.MinValue)
            {
                return String.Empty;
            }

            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static String DateTimeToShortString(DateTime dateTime)
        {
            if (dateTime == null || dateTime >= DateTime.MaxValue.Date || dateTime <= DateTime.MinValue)
            {
                return String.Empty;
            }

            return dateTime.ToString("yyyy-MM-dd");
        }

        public static String ObjectToLongString(object objDateTime)
        {
            if (objDateTime == DBNull.Value || objDateTime == null)
            {
                return String.Empty;
            }

            DateTime dateTime = Convert.ToDateTime(objDateTime);

            if (dateTime == null || dateTime >= DateTime.MaxValue.Date || dateTime <= DateTime.MinValue)
            {
                return String.Empty;
            }

            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static String ObjectToShortString(object objDateTime)
        {
            if (objDateTime == DBNull.Value || objDateTime == null)
            {
                return String.Empty;
            }

            DateTime dateTime = Convert.ToDateTime(objDateTime);

            if (dateTime == null || dateTime >= DateTime.MaxValue.Date || dateTime <= DateTime.MinValue)
            {
                return String.Empty;
            }

            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
