using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace InfoTips
{
    public class Utility
    {
        public static List<Control> m_listForm = new List<Control>();
        public static string m_skinName = "Lilian";
        public string Check_Href(string html)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<a.*?>|</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex.Replace(html, "");
            return html;
        }
        public string getUrlSource(string strUrl, string strEncoding)
        {
            string lsResult;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strUrl);
                req.Timeout = 10000;
                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(rep.GetResponseStream(), Encoding.GetEncoding(strEncoding));
                lsResult = sr.ReadToEnd();
            }
            catch
            {
                lsResult = "";
                return lsResult;
            }
            return lsResult;
        }
        public void 筛选数据(DevExpress.XtraBars.BarManager bar, DevExpress.XtraTreeList.TreeList treelist)
        {
            foreach (DevExpress.XtraBars.BarItem c in bar.Items)
            {
                if (c.GetType().FullName != "DevExpress.XtraBars.BarCheckItem")
                    continue;
                if (((DevExpress.XtraBars.BarCheckItem)c).Description == "1")
                {
                    if (!((DevExpress.XtraBars.BarCheckItem)c).Checked)
                    {
                        foreach (TreeListNode tln in treelist.Nodes)
                        {
                            if (tln.GetValue(2).ToString() == ((DevExpress.XtraBars.BarCheckItem)c).Caption)
                                tln.Visible = false;
                        }
                    }
                    else
                    {
                        foreach (TreeListNode tln in treelist.Nodes)
                        {
                            if (tln.GetValue(2).ToString() == ((DevExpress.XtraBars.BarCheckItem)c).Caption)
                                tln.Visible = true;
                        }
                    }
                }
            }
            if (treelist.Nodes.Count > 0)
                treelist.SetFocusedNode(treelist.Nodes[0]);
        }
    }
}
