using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace BaseUI
{
    public partial class MMTextBox:TextBox
    {
        public Color TextBoxReadOnlyBackColor
        {
            get
            {
                return Color.FromArgb(223, 230, 232);
            }
        }

        /// <summary>
        /// 初始化控件。
        /// </summary>
        public void Initialize(Boolean readOnly)
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            if (readOnly)
            {
                this.BackColor = TextBoxReadOnlyBackColor;
                this.ReadOnly = true;
            }
        }
    }
}
