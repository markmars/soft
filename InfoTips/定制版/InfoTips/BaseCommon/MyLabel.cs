using System;
using System.Collections.Generic;
using System.Text;

namespace InfoTips
{
    public class MyLabel : System.Windows.Forms.Label
    {
        public MyLabel()
        {
            this.BackColor = System.Drawing.Color.Transparent;
            this.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AutoSize = false;
        }
    }
}
