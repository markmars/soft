using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MarkMars.UI
{
	public partial class FormError : Form
	{
		public FormError(string strShortError, string strLongError)
		{
			InitializeComponent();

			this.label1.Text = strShortError;
			this.textBoxDetail.Text = strLongError;
		}

		private void buttonShowHideDetail_Click(object sender, EventArgs e)
		{
			if (this.buttonShowHideDetail.Text == "显示详细信息")
			{
                this.Height = 484;
				this.buttonShowHideDetail.Text = "隐藏详细信息";
			}
			else
			{
				this.Height = 140;
				this.buttonShowHideDetail.Text = "显示详细信息";
			}
		}
	}
}
