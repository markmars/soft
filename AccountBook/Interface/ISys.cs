using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Interface
{
    /// <summary>  
    /// 作者：MarkMars  
    /// 时间：2013-2-19 16:47:00  
    /// 公司:  
    /// 版权：2013-2013  
    /// CLR版本：4.0.30319.17929  
    /// 博客地址：http://www.wakealone.com  
    /// Class1说明：本代码版权归MarkMars所有，使用时必须带上MarkMars博客地址  
    /// 唯一标识：32b9b9b9-0c9e-4af3-84d9-24706280e3e2  
    /// </summary>
    public interface ISys
    {
        /// <summary>
        /// 获取所有已被授权的按钮
        /// </summary>
        /// <param name="pageName">页面</param>
        /// <returns>已被授权的按钮列表</returns>
        DataTable GetToolBarByPage(String pageName);

        /// <summary>
        /// 获取所有已被授权的子按钮
        /// </summary>
        /// <param name="pageName">页面</param>
        /// <param name="parent">父按钮</param>
        /// <returns>已被授权的子按钮列表</returns>
        DataTable GetToolBarByPageAndParent(String pageName, String parent);
    }
}
