using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaseUI
{
    public partial class MMPanel : Panel
    {
        public static Color PanelBackColor
        {
            get
            {
                return Color.Transparent;
            }
        }

        /// <summary>
        /// 初始化控件。
        /// </summary>
        public void Initialize()
        {
            this.Padding = new Padding(10);
            this.BackColor = PanelBackColor;
        }
    }
}
