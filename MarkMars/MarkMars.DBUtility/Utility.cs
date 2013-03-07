using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace MarkMars.DBUtility
{
    public static class Utility
    {
        public static string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string Convert全角2半角(string str)
        {
            StringBuilder sb = new StringBuilder(str);

            string str全角 = "１２３４５６７８９０，．（）＋－×％ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ";
            string str半角 = "1234567890,.()+-*%abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Debug.Assert(str全角.Length == str半角.Length);
            for (int i = 0; i < str全角.Length; i++)
                sb.Replace(str全角[i], str半角[i]);

            return sb.ToString();
        }
    }
}
