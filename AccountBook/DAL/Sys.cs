using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AccountBook.DAL
{
    /// <summary>  
    /// 作者：MarkMars  
    /// 时间：2013-2-19 16:41:04  
    /// 公司:  
    /// 版权：2013-2013  
    /// CLR版本：4.0.30319.17929  
    /// 博客地址：http://www.wakealone.com  
    /// Class1说明：本代码版权归MarkMars所有，使用时必须带上MarkMars博客地址  
    /// 唯一标识：3c2304f3-34fb-44d2-946f-340e69c91406  
    /// </summary>  
    public class Sys
    {
        private static MarkMars.DBUtility.AccessDatabase DBHelper = null;

        static Sys()
        {
            if (DBHelper == null)
            {
                DBHelper = new MarkMars.DBUtility.AccessDatabase(Environment.CurrentDirectory + "MMDT.MMD", null);
            }
        }

        /// <summary>
        /// 获取所有已被授权的按钮
        /// </summary>
        /// <param name="pageName">页面</param>
        /// <param name="inToolbar">按钮是否在工具栏中显示</param>
        /// <returns>已被授权的按钮列表</returns>
        public DataTable GetToolBarByPage(String pageName)
        {
            String sql = String.Format(@"
                SELECT DISTINCT * FROM Sys_ToolBar 
                WHERE PageName = '{0}' AND A.InToolbar = '1' 
                ORDER BY Sequence", pageName);
            return DBHelper.GetTableBySQL(sql);
        }

        /// <summary>
        /// 获取所有已被授权的子按钮
        /// </summary>
        /// <param name="pageName">页面</param>
        /// <param name="parent">父按钮</param>
        /// <returns>已被授权的子按钮列表</returns>
        public DataTable GetToolBarByPageAndParent(String pageName, String parent)
        {
            String sql = String.Format(@"
                SELECT DISTINCT * FROM Sys_ToolBar 
                WHERE PageName = '{0}' AND PARENT='{1}' AND A.InToolbar = '1' 
                ORDER BY Sequence", pageName, parent);
            return DBHelper.GetTableBySQL(sql);
        }
    }
}
