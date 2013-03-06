using System;
using System.Collections.Generic;
using System.Text;

namespace InfoTips
{
    public partial class MyUserControl : System.Windows.Forms.UserControl
    {
        public MyUserControl()
        {
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MyUserControl
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Name = "MyUserControl";
            this.Size = new System.Drawing.Size(1095, 348);
            this.ResumeLayout(false);

        }
    }
}
