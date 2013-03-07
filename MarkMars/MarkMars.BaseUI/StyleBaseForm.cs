using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaseUI
{
    public partial class StyleBaseForm : Form
    {
        //Form BackColor
        public static Color FormBackColor
        {
            get
            {
                return Color.FromArgb(255, 255, 255);
            }
        }

        public StyleBaseForm()
        {
            InitializeComponent();

            this.BackColor = FormBackColor;
        }
    }
}
