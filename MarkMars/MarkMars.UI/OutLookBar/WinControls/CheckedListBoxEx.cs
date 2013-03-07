using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MarkMars.UI.OutLookBar.WinControls
{
    /// <summary>
    /// 为CheckedListBox控件扩展了一个ReadOnly属性。
    /// </summary>
    [ToolboxItem(false)]
    public class CheckedListBoxEx : System.Windows.Forms.CheckedListBox
    {
        #region 类变量
        private Boolean readOnly = false;
        private Color oldColor;
        #endregion

        #region 构造函数
        public CheckedListBoxEx()
        {
            this.oldColor = this.ForeColor;
        }
        #endregion

        #region 扩展属性
        public Boolean ReadOnly
        {
            get { return this.readOnly; }
            set
            {
                if (this.readOnly != value)
                {
                    this.readOnly = value;
                    if (this.readOnly)
                    {
                        this.oldColor = this.ForeColor;
                        this.ForeColor = SystemColors.GrayText;
                    }
                    else
                    {
                        this.ForeColor = this.oldColor;
                    }

                    this.Invalidate();
                }
            }
        }
        #endregion

        #region 重写方法
        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            if (this.readOnly)
            {
                ice.NewValue = ice.CurrentValue;
            }
            else
            {
                base.OnItemCheck(ice);
            }
        }
        #endregion
    }
}
