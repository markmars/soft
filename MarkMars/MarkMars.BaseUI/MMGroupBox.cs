using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BaseUI
{
    public partial class MMGroupBox : GroupBox
    {
        public static Color GroupBoxTextColor
        {
            get
            {
                return Color.FromArgb(20, 105, 133);
            }
        }

        /// <summary>
        /// 初始化控件。
        /// </summary>
        public void Initialize()
        {
            this.ForeColor = GroupBoxTextColor;
        }
    }
}
