using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IPDL
{
    public partial class FrmSource : Form
    {
        public FrmSource()
        {
            InitializeComponent();
        }
        public FrmSource(ZiYuan zy)
        {
            InitializeComponent();
            txt_Name.Text = zy.name;
            txt_url.Text = zy.url;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Name.Text))
            {
                MessageBox.Show("名称不能为空");
                return;
            }
            if (txt_url.Text.StartsWith("http://"))
            {
                ZiYuan zy = new ZiYuan(txt_Name.Text, txt_url.Text);
                this.Tag = zy;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("网页地址格式不正确");
                return;
            }
        }
        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
