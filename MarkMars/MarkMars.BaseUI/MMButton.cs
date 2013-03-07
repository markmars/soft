using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MarkMars.BaseUI
{
    public partial class MMButton : Button
    {
        #region Button颜色配置
        public static Color DisableBackColor
        {
            get
            {
                return Color.FromArgb(193, 188, 167);
            }
        }
        public static Color EnableBackColor
        {
            get
            {
                return Color.FromArgb(187, 214, 236);
            }
        }
        #endregion

        /// <summary>
        /// 初始化控件。
        /// </summary>
        public void Initialize()
        {
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackColor = EnableBackColor;
        }

        public Boolean EnabledEx
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                if (!value)
                {
                    this.BackColor = DisableBackColor;
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                else
                {
                    this.BackColor = EnableBackColor;
                    this.Cursor = System.Windows.Forms.Cursors.Hand;
                }

                this.Enabled = value;
            }
        }
    }
}
