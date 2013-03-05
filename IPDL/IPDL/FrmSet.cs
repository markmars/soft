using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IPDL
{
    public partial class FrmSet : Form
    {
        public FrmSet()
        {
            InitializeComponent();
        }
        public FrmSet(YanZheng yz)
        {
            InitializeComponent();
            txt_Name.Text = yz.name;
            txt_url.Text = yz.url;
        }
        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Name.Text))
            {
                MessageBox.Show("资源名称不能为空");
                return;
            }
            if (txt_url.Text.StartsWith("http://"))
            {
                YanZheng yz = new YanZheng(txt_Name.Text, txt_url.Text);
                this.Tag = yz;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("资源地址格式不正确");
                return;
            }
        }
        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
