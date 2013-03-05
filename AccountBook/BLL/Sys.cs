using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AccountBook.BLL
{
    /// <summary>  
    /// 作者：MarkMars  
    /// 时间：2013-2-19 16:42:21  
    /// 公司:  
    /// 版权：2013-2013  
    /// CLR版本：4.0.30319.17929  
    /// 博客地址：http://www.wakealone.com  
    /// Class1说明：本代码版权归MarkMars所有，使用时必须带上MarkMars博客地址  
    /// 唯一标识：41079b85-d40d-4b69-80e2-651e29e537bb  
    /// </summary>  
    public class Sys
    {
        private static DAL.Sys dal = null;

        static Sys()
        {
            if (dal == null)
            {
                dal = new DAL.Sys();
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
            return dal.GetToolBarByPage(pageName);
        }

        /// <summary>
        /// 获取所有已被授权的子按钮
        /// </summary>
        /// <param name="pageName">页面</param>
        /// <param name="parent">父按钮</param>
        /// <returns>已被授权的子按钮列表</returns>
        public DataTable GetToolBarByPageAndParent(String pageName, String parent)
        {
            return dal.GetToolBarByPageAndParent(pageName, parent);
        }
    }
}
