using System.Web;
using System.Web.UI;
using System.Text;

namespace MarkMars.Common.Webform
{
    /// <summary>
    /// .net通用JS函数封装
    /// </summary>
    public class JS
    {

        /// <summary>
        /// 客户端打开窗口
        /// </summary>
        /// <param name="strUrl">Url地址</param>
        public static void OpenWindow(string strUrl)
        {
            HttpContext.Current.Response.Write("<script language=javascript>");
            HttpContext.Current.Response.Write("window.open('" + strUrl + "');");
            HttpContext.Current.Response.Write("</script>");
        }

        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(System.Web.UI.Page page, string strScript)
        {
            page.RegisterStartupScript("message", "<script language='javascript' defer>" + strScript + "</script>");
        }

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirect(System.Web.UI.Page page, string msg, string url)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<script language='javascript' defer>");
            strBuilder.AppendFormat("alert('{0}');", msg);
            strBuilder.AppendFormat("top.location.href='{0}'", url);
            strBuilder.Append("</script>");
            page.RegisterStartupScript("message", strBuilder.ToString());
        }

        /// <summary>
        /// 给控件添加单击显示输入对话框的事件
        /// </summary>
        /// <param name="Control">控件的指针</param>
        /// <param name="msg">内容</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl Control, string msg)
        {
            Control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }

        /// <summary>
        /// 清空panel里面所有textbox内容
        /// </summary>
        /// <param name="pan">要清空的panel指针</param>
        public static void clearAll(System.Web.UI.WebControls.Panel pan)
        {
            foreach (Control panl in pan.Controls)
                if (panl.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                    ((System.Web.UI.WebControls.TextBox)panl).Text = "";
        }

        /// <summary>
        /// 窗体加载以后弹出对话框
        /// </summary>
        /// <param name="msg">内容</param>
        public static void Alert(string msg)
        {
            Page pages;
            pages = HttpContext.Current.Handler as System.Web.UI.Page;
            msg = msg.Replace("'", "");
            msg = msg.Replace("\"", "");
            msg = msg.Replace("\n", @"\n").Replace("\r", @"\r").Replace("\"", @"\""");

            pages.Controls.Add(new System.Web.UI.LiteralControl("<script language=javascript>alert('" + msg + "');</script>"));
        }

        /// <summary>
        /// 窗体没有加载的时候如pageload的时候探出对话框
        /// </summary>
        /// <param name="msg"></param>
        public static void Alert_none(string msg)
        {
            msg = msg.Replace("'", "");
            msg = msg.Replace("\"", "");
            msg = msg.Replace("\n", @"\n").Replace("\r", @"\r").Replace("\"", @"\""");
            string retu = " alert('" + msg + "');";
            ClientWrite2(retu);
        }

        /// <summary>
        /// 加载以后写自己的脚本
        /// </summary>
        /// <param name="yourJs">JS脚本</param>
        public static void ClientWrite(string yourJs)
        {
            Page pages;
            pages = HttpContext.Current.Handler as System.Web.UI.Page;
            pages.Controls.Add(new System.Web.UI.LiteralControl("<script language=javascript>" + yourJs + "</script>"));
        }
        
        /// <summary>
        /// 加载以前写自己的脚本
        /// </summary>
        /// <param name="yourJs">JS脚本</param>
        public static void ClientWrite2(string yourJs)
        {
            Page pages;
            pages = HttpContext.Current.Handler as System.Web.UI.Page;
            pages.Response.Write("<script language=javascript>");
            pages.Response.Write(yourJs);
            pages.Response.Write(" </script>");
        }

        /// <summary>
        /// 得到刷新界面的字符串
        /// </summary>
        /// <returns></returns>
        public static string RefreshWin()
        {
            return "window.location=window.location.href;";
        }
        
        /// <summary>
        /// 打开小窗体
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        public static void OpenLittleWindow(string url, int width, int height, int top, int left)
        {
            string str;
            str = "javascript:var popup;popup=window.open('{url}',null,'scrollbars=yes,status=no,width={width},height={height},top={top},left={left}');popup.opener=self.opener;self.close();";
            str = str.Replace("{width}", width.ToString());
            str = str.Replace("{height}", height.ToString());
            str = str.Replace("{top}", top.ToString());
            str = str.Replace("{left}", left.ToString());
            str = str.Replace("{url}", url);
        }

        /// <summary>
        /// 回车-〉tab
        /// </summary>
        /// <param name="page"></param>
        public static void ToTab()
        {
            Page page;
            page = HttpContext.Current.Handler as System.Web.UI.Page;
            System.Text.StringBuilder scriptFunction = new StringBuilder();
            scriptFunction.Append("<script language='javascript'>");
            scriptFunction.Append("       function returnTotab()");
            scriptFunction.Append("         {");
            scriptFunction.Append("          if(event.keyCode==13)    ");
            scriptFunction.Append("             {event.keyCode=9;     ");
            scriptFunction.Append("               return true;}       ");
            scriptFunction.Append("          } ");
            scriptFunction.Append("</script>");
            page.RegisterStartupScript("totab", scriptFunction.ToString());

        }
        
        /// <summary>
        /// tab->enter
        /// </summary>
        /// <param name="page"></param>
        public static void tabToEnter()
        {
            Page page;
            page = HttpContext.Current.Handler as System.Web.UI.Page;
            System.Text.StringBuilder scriptFunction = new StringBuilder();
            scriptFunction.Append("<script language='javascript'>");
            scriptFunction.Append("    function Tcheck()");
            scriptFunction.Append("         {");
            scriptFunction.Append("         if(event.keyCode==8||event.keyCode==9) ");
            scriptFunction.Append("          return true;");
            scriptFunction.Append("         else ");
            scriptFunction.Append("         {");
            scriptFunction.Append("          if(((event.keyCode>=48)++(event.keyCode<=57))||((event.keyCode>=96)++(event.keyCode<=105)))");
            scriptFunction.Append("              return true;");
            scriptFunction.Append("          else");
            scriptFunction.Append("          if(event.keyCode==13||event.keyCode==110||event.keyCode==190||event.keyCode==39)");
            scriptFunction.Append("             {event.keyCode=9;");
            scriptFunction.Append("               return true;}");
            scriptFunction.Append("            else");
            scriptFunction.Append("              return false;");
            scriptFunction.Append("        }");
            scriptFunction.Append("          }     ");
            scriptFunction.Append("</script>");
            page.RegisterStartupScript("switch", scriptFunction.ToString());
        }
        
        /// <summary>
        /// attachEvent
        /// </summary>
        /// <param name="controlToFocus"></param>
        /// <param name="page"></param>
        public static void attachEvent(Control[] controlToFocus)
        {
            Page page;
            page = HttpContext.Current.Handler as System.Web.UI.Page;
            System.Text.StringBuilder scriptFunction = new StringBuilder();
            string scriptClientId;
            scriptFunction.Append("<script language='javascript'>");

            foreach (Control con in controlToFocus)
            {
                scriptClientId = con.ClientID;
                scriptFunction.Append("document.getElementById('" + scriptClientId + "').attachEvent('onkeydown', Tcheck);");
            }
            scriptFunction.Append("</script>");
            page.RegisterStartupScript("attach", scriptFunction.ToString());
        }
        
        /// <summary>
        ///
        /// </summary>
        /// <param name="controlToFocus"></param>
        /// <param name="page"></param>
        /// <param name="eventStr"></param>
        /// <param name="FuncStr"></param>
        public static void AttachEvent(Control[] controlToFocus, string eventStr, string FuncStr)
        {
            Page page;
            page = HttpContext.Current.Handler as System.Web.UI.Page;
            System.Text.StringBuilder scriptFunction = new StringBuilder();
            string scriptClientId;
            scriptFunction.Append("<script language='javascript'>");
            foreach (Control con in controlToFocus)
            {
                scriptClientId = con.ClientID;
                scriptFunction.Append("document.getElementById('" + scriptClientId + "').attachEvent('" + eventStr + "', " + FuncStr + ");");
            }
            scriptFunction.Append("</script>");
            page.RegisterStartupScript("attach2", scriptFunction.ToString());
        }
        
        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        public static void NumOnlyFun()
        {
            Page page;
            page = HttpContext.Current.Handler as System.Web.UI.Page;
            System.Text.StringBuilder scriptFunction = new StringBuilder();
            scriptFunction.Append("<script language='javascript'>");
            scriptFunction.Append("       function isNum()");
            scriptFunction.Append("         {");
            scriptFunction.Append("              if(event.keyCode==8||event.keyCode==9) ");
            scriptFunction.Append("                  return true;");
            scriptFunction.Append("             else ");
            scriptFunction.Append("             {");
            scriptFunction.Append("          if(((event.keyCode>=48)++(event.keyCode<=57))||((event.keyCode>=96)++(event.keyCode<=105)))");
            scriptFunction.Append("              return true;");
            scriptFunction.Append("          else");
            scriptFunction.Append("                return false;");
            scriptFunction.Append("        }");
            scriptFunction.Append("          } ");
            scriptFunction.Append("</script>");
            page.RegisterStartupScript("numonly", scriptFunction.ToString());
        }
    }
}