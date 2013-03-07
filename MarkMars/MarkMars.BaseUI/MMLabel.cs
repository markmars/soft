using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaseUI
{
    public partial class MMLabel:Label
    {
        private Color labelForColor = Color.Black;
        public Color LabelForColor
        {
            get
            {
                return this.labelForColor;
            }
            set
            {
                this.labelForColor = value;
            }
        }

        /// <summary>
        /// 初始化控件。
        /// </summary>
        public void Initialize()
        {
            this.ForeColor = this.labelForColor;
        }
    }
}
