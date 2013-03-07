using System;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace MarkMars.UI
{
    public static class TrueLoreMessageBox
    {
        /// <summary>
        /// 显示消息。
        /// </summary>
        /// <param name="message">消息内容。</param>
        public static void ShowMessage(String message)
        {
            MessageBox.Show(message, "消息");
        }

        /// <summary>
        /// 显示警告消息框。
        /// </summary>
        /// <param name="warning">消息内容。</param>
        public static void ShowWarning(String warning)
        {
            MessageBox.Show(warning, "警告信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示警示消息框。
        /// </summary>
        /// <param name="warning">消息内容。</param>
        public static void ShowInformation(String information)
        {
            MessageBox.Show(information, "警示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示错误消息框。
        /// </summary>
        /// <param name="error">消息内容。</param>
        public static void ShowError(String error)
        {
			ShowError(error, null);
        }

        /// <summary>
        /// 显示错误消息框。
        /// </summary>
        /// <param name="error">消息内容。</param>
		public static void ShowError(String error, Exception ex, params Object[] args)
        {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(error);

			Exception e = ex;
			while (e != null)
			{
				sb.AppendLine(string.Empty);
				sb.AppendLine("异常信息:");
				sb.AppendLine(e.Message);
				sb.AppendLine(e.StackTrace);

				e = e.InnerException;
            }

			sb.AppendLine(string.Empty);
			sb.AppendLine("堆栈信息:");
			StackFrame[] stacks = new StackTrace().GetFrames();
			foreach (StackFrame stack in stacks)
			{
				if (stack.GetFileName() == null)
					sb.AppendLine(stack.GetMethod().DeclaringType.FullName + "." + stack.GetMethod().ToString());
				else
					sb.AppendLine(string.Format("{0} {1} {2}",
						stack.GetFileName(),
						stack.GetFileLineNumber(),
						stack.GetMethod().DeclaringType.FullName + "." + stack.GetMethod().ToString()));
			}

			if (args != null && args.Length > 0)
			{
				sb.AppendLine(string.Empty);
				sb.AppendLine("附加信息:");
				foreach (object o in args)
				{
					if (o == null)
						sb.AppendLine(string.Empty);
					else
						sb.AppendLine(o.ToString());
				}
			}


			FormError f = new FormError(error, sb.ToString());
			f.ShowDialog();
        }

        /// <summary>
        /// 显示询问消息框。
        /// </summary>
        /// <param name="confirm">消息内容。</param>
        public static Boolean ShowBooleanConfirm(String confirm)
        {
            DialogResult dialogResult = MessageBox.Show(confirm, "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 显示询问消息框。
        /// </summary>
        /// <param name="confirm">消息内容。</param>
        public static DialogResult ShowConfirm(String confirm)
        {
            return MessageBox.Show(confirm, "确认信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 显示询问消息框。
        /// </summary>
        /// <param name="confirm">消息内容。</param>
        public static Int32 ShowIntConfirm(String confirm)
        {
            Int32 m_Result = 0;
            
            DialogResult dialogResult = MessageBox.Show(confirm, "确认信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                m_Result = 1;
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                m_Result = -1;
            }
            
            return m_Result;
        }
    }
}
