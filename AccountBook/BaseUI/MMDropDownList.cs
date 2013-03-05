using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace BaseUI
{
    public partial class MMDropDownList : ComboBox
    {
        /// <summary>
        /// 初始化控件。
        /// </summary>
        public void Initialize()
        {
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
