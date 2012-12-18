using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IPDL
{
    public partial class FrmDaiLiInfo : Form
    {
        public FrmDaiLiInfo()
        {
            InitializeComponent();
            cmbox_协议.SelectedIndex = 0;
        }
        public FrmDaiLiInfo(DaiLi dl)
        {
            InitializeComponent();
            cmbox_协议.SelectedItem = dl.type;
            txt_ip.Text = dl.ip;
            txt_port.Text = dl.port;
            txt_info.Text = dl.info;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (txt_ip.Text.Contains("..") || txt_ip.Text.StartsWith(".") || txt_ip.Text.EndsWith("."))
            {
                MessageBox.Show("ip地址格式不正确");
                return;
            }
            int port = 0;
            if (int.TryParse(txt_port.Text, out port) && (port > 0 && port < 65535))
            {
                DaiLi dl = new DaiLi(cmbox_协议.SelectedText, txt_ip.Text, txt_port.Text, "未验证", txt_info.Text);
                this.Tag = dl;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("端口需要大于0小于65535");
                return;
            }
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
