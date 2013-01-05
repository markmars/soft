using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace IPDL
{
    public partial class FrmSetting : Form
    {
        public FrmSetting()
        {
            InitializeComponent();
            InitSetting();
        }

        private void btn_添加验证_Click(object sender, EventArgs e)
        {
            FrmSet fs = new FrmSet();
            if (fs.ShowDialog() == DialogResult.OK)
            {
                YanZheng yz = fs.Tag as YanZheng;
                listView_YanZhengZiYuan.Items.Add(new ListViewItem(new string[] { yz.name, yz.url }));
            }
        }
        private void btn_修改验证_Click(object sender, EventArgs e)
        {
            YanZheng yz = new YanZheng(
                listView_YanZhengZiYuan.SelectedItems[0].SubItems[0].Text,
                listView_YanZhengZiYuan.SelectedItems[0].SubItems[1].Text);

            FrmSet fs = new FrmSet(yz);
            if (fs.ShowDialog() == DialogResult.OK)
            {
                yz = fs.Tag as YanZheng;
                listView_YanZhengZiYuan.SelectedItems[0].Text = yz.name;
                listView_YanZhengZiYuan.SelectedItems[0].SubItems[1].Text = yz.url;
            }
        }
        private void btn_删除验证_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除这些验证资源地址吗？删除后不能恢复！", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            foreach (ListViewItem lvi in listView_YanZhengZiYuan.SelectedItems)
                listView_YanZhengZiYuan.Items.Remove(lvi);
        }
        private void btn_导入_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            op.RestoreDirectory = true;
            op.Filter = "文本文件(*.txt)|*.txt";
            DialogResult result = op.ShowDialog();
            if (result == DialogResult.OK)
            {
                string aa = op.FileName;
                StreamReader sr = new StreamReader(@aa);
                string txtInfo = sr.ReadToEnd();
                Regex rg = new Regex(@"(.+)##((http://).+)##(0|1)");
                MatchCollection mas = rg.Matches(txtInfo);
                foreach (Match m in mas)
                {
                    listView_YanZhengZiYuan.Items.Add(
                        new ListViewItem(
                            new string[]{
                                m.ToString().Split(new char[] { ':' })[0],
                                m.ToString().Split(new char[] { ':' })[1]
                            }));
                    if (m.ToString().Split(new char[] { ':' })[2] == "1")
                        listView_YanZhengZiYuan.Items[listView_YanZhengZiYuan.Items.Count - 1].Checked = true;
                }
            }
        }
        private void btn_导出_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文件(*.txt)|*.txt";
            sfd.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.FileName = "独醒代理验证资源" + DateTime.Now.ToString("yyyy-M-d");
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sfd.FileName.ToString(), FileMode.Create);

                string txtInfo = string.Empty;
                foreach (ListViewItem lvi in listView_YanZhengZiYuan.Items)
                    txtInfo += lvi.SubItems[0].Text + "##" + lvi.SubItems[1].Text + "##" + (lvi.Checked ? "1" : "0") + "\n";

                try
                {
                    char[] charData = txtInfo.ToCharArray();
                    byte[] byData = new byte[charData.Length];
                    Encoder en = Encoding.UTF8.GetEncoder();
                    en.GetBytes(charData, 0, charData.Length, byData, 0, true);
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(byData, 0, byData.Length);
                }
                catch
                {
                    MessageBox.Show("导出验证资源出现错误");
                    return;
                }
                fs.Dispose();
                MessageBox.Show("已导出");
            }
        }
        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (SaveSetting())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void InitSetting()
        {
            IniFile inifile;
            string strPath = Application.StartupPath + "\\Setting.ini";
            if (!File.Exists(strPath))
            {
                MessageBox.Show("配置文件缺失，程序进行自动生成");
                FileStream fs = new FileStream(strPath, FileMode.Create);
                fs.Dispose();
                inifile = new IniFile(strPath);
                inifile.SetValue("连接超时时间", "时间", "5");
                inifile.SetValue("验证超时时间", "时间", "15");
                inifile.SetValue("并发线程数目", "个数", "50");
                inifile.SetValue("验证资源", "资源1", "独醒资源##http://www.wakealone.com##1");
                inifile.SetValue("验证资源", "资源2", "百度一下##http://www.baidu.com##0");
                inifile.SetValue("验证资源", "资源3", "谷歌一下##http://www.google.com##0");
                Utility.m_连接超时时间 = 5000;
                Utility.m_验证超时时间 = 15000;
                Utility.m_并发线程数量 = 50;
                Utility.m_验证地址 = new List<string>();
                Utility.m_验证地址.Add("http://www.baidu.com");
            }
            inifile = new IniFile(strPath);
            txt_连接超时时间.Text = inifile.GetValue("连接超时时间", "时间");
            txt_验证超时时间.Text = inifile.GetValue("验证超时时间", "时间");
            txt_并发线程数目.Text = inifile.GetValue("并发线程数目", "个数");
            List<string> ls = new List<string>();
            ls = inifile.GetSectionValues("验证资源");
            foreach (string str in ls)
            {
                Regex rg = new Regex(@"(.+)##((http://).+)##(0|1)");
                Match m = rg.Match(str);
                listView_YanZhengZiYuan.Items.Add(
                        new ListViewItem(
                            new string[]{
                                m.ToString().Split(new string[] { "##" },StringSplitOptions.None)[0],
                                m.ToString().Split(new string[] { "##" },StringSplitOptions.None)[1]
                            }));
                if (m.ToString().Split(new string[] { "##" }, StringSplitOptions.None)[2] == "1")
                    listView_YanZhengZiYuan.Items[listView_YanZhengZiYuan.Items.Count - 1].Checked = true;
            }
        }
        private bool SaveSetting()
        {
            if (string.IsNullOrEmpty(txt_连接超时时间.Text) || string.IsNullOrEmpty(txt_验证超时时间.Text) || string.IsNullOrEmpty(txt_并发线程数目.Text))
            {
                MessageBox.Show("参数不能为空");
                return false;
            }
            if (listView_YanZhengZiYuan.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选中一个验证资源！");
                return false;
            }

            string strPath = Application.StartupPath + "\\Setting.ini";
            if (!File.Exists(strPath))
            {
                FileStream fs = new FileStream(strPath, FileMode.Create);
                fs.Dispose();
            }
            IniFile inifile = new IniFile(strPath);
            inifile.SetValue("连接超时时间", "时间", txt_连接超时时间.Text);
            inifile.SetValue("验证超时时间", "时间", txt_验证超时时间.Text);
            inifile.SetValue("并发线程数目", "个数", txt_并发线程数目.Text);
            inifile.DelSection("验证资源");
            for (int i = 0; i < listView_YanZhengZiYuan.Items.Count; i++)
            {
                inifile.SetValue("验证资源", "资源" + (i + 1), listView_YanZhengZiYuan.Items[i].SubItems[0].Text + "##" + listView_YanZhengZiYuan.Items[i].SubItems[1].Text + "##" + (listView_YanZhengZiYuan.Items[i].Checked ? "1" : "0"));
            }

            Utility.m_连接超时时间 = int.Parse(txt_连接超时时间.Text) * 1000;
            Utility.m_验证超时时间 = int.Parse(txt_验证超时时间.Text) * 1000;
            Utility.m_并发线程数量 = int.Parse(txt_并发线程数目.Text);
            Utility.m_验证地址 = new List<string>();
            foreach (ListViewItem lvi in listView_YanZhengZiYuan.CheckedItems)
                Utility.m_验证地址.Add(lvi.SubItems[1].Text);
            return true;
        }
    }
}
