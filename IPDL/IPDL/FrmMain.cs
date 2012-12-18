using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace IPDL
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            listView_Source.Items.Add(new ListViewItem(new string[] { "烈火每日代理", "http://www.veryhuo.com/res/ip/" }));
            listView_Source.Items[0].Checked = true;
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出【独醒代理】？", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            //SaveSetting();
        }
        private void btn_设置_Click(object sender, EventArgs e)
        {

        }
        private void btn_验证全部_Click(object sender, EventArgs e)
        {
            List<int> lsInts = new List<int>();
            foreach (ListViewItem lvi in listView_Info.Items)
                lsInts.Add(lvi.Index);

            Thread t = new Thread(_Ping);
            t.Start(lsInts);
        }
        private void btn_验证选中_Click(object sender, EventArgs e)
        {
            List<int> lsInts = new List<int>();
            foreach (ListViewItem lvi in listView_Info.SelectedItems)
                lsInts.Add(lvi.Index);

            Thread t = new Thread(_Ping);
            t.Start(lsInts);
        }
        private void btn_设置代理_Click(object sender, EventArgs e)
        {
            if (listView_Info.SelectedItems.Count == 0)
            {
                MessageBox.Show("无选中代理");
                return;
            }
            SetDaiLi(listView_Info.SelectedItems[0].SubItems[1].Text + ":" + listView_Info.SelectedItems[0].SubItems[2].Text);
        }
        private void btn_取消代理_Click(object sender, EventArgs e)
        {
            DelDaiLi();
        }
        private void btn_添加代理_Click(object sender, EventArgs e)
        {
            FrmDaiLiInfo fdl = new FrmDaiLiInfo();
            if (fdl.ShowDialog() == DialogResult.OK)
            {
                DaiLi dl = fdl.Tag as DaiLi;
                listView_Info.Items.Add(new ListViewItem(new string[] { dl.type, dl.ip, dl.port, dl.time, dl.info }));
            }
        }
        private void btn_修改选中_Click(object sender, EventArgs e)
        {
            DaiLi dl = new DaiLi(
                listView_Info.SelectedItems[0].SubItems[0].Text,
                listView_Info.SelectedItems[0].SubItems[1].Text,
                listView_Info.SelectedItems[0].SubItems[2].Text,
                listView_Info.SelectedItems[0].SubItems[3].Text,
                listView_Info.SelectedItems[0].SubItems[4].Text);

            FrmDaiLiInfo fdl = new FrmDaiLiInfo(dl);
            if (fdl.ShowDialog() == DialogResult.OK)
            {
                dl = fdl.Tag as DaiLi;
                listView_Info.SelectedItems[0].Text = dl.type;
                listView_Info.SelectedItems[0].SubItems[1].Text = dl.ip;
                listView_Info.SelectedItems[0].SubItems[2].Text = dl.port;
                listView_Info.SelectedItems[0].SubItems[3].Text = dl.time;
                listView_Info.SelectedItems[0].SubItems[4].Text = dl.info;
            }
        }
        private void btn_删除_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除这些代理地址吗？数据删除后不能恢复！", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            foreach (ListViewItem lvi in listView_Info.SelectedItems)
                listView_Info.Items.Remove(lvi);
        }
        private void btn_清理_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除“连接失败”的代理吗？\r\n数据删除后不能恢复！有些代理过段时间可能还可以用！", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            foreach (ListViewItem lvi in listView_Info.Items)
            {
                if (lvi.SubItems[3].Text == "验证失败")
                    listView_Info.Items.Remove(lvi);
            }
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
                Regex rg = new Regex(@"((\d{1,3}\.){3}\d{1,3}):(\d{1,5})");
                MatchCollection mas = rg.Matches(txtInfo);
                foreach (Match m in mas)
                {
                    listView_Info.Items.Add(
                        new ListViewItem(
                            new string[]{
                                "HTTP",
                                m.ToString().Split(new char[] { ':' })[0],
                                m.ToString().Split(new char[] { ':' })[1],
                                "未验证",
                                ""
                            }));
                }
            }
        }
        private void btn_粘贴_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                string txtInfo = (String)iData.GetData(DataFormats.Text);
                Regex rg = new Regex(@"((\d{1,3}\.){3}\d{1,3}):(\d{1,5})");
                MatchCollection mas = rg.Matches(txtInfo);
                foreach (Match m in mas)
                {
                    listView_Info.Items.Add(
                        new ListViewItem(
                            new string[]{
                                "HTTP",
                                m.ToString().Split(new char[] { ':' })[0],
                                m.ToString().Split(new char[] { ':' })[1],
                                "未验证",
                                ""
                            }));
                }
            }
            else
                MessageBox.Show("无法检测出剪切板的数据");
        }
        private void btn_导出选定_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文件(*.txt)|*.txt";
            sfd.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.FileName = "独醒代理" + DateTime.Now.ToString("yyyy-M-d");
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sfd.FileName.ToString(), FileMode.Create);

                string txtInfo = string.Empty;
                foreach (ListViewItem lvi in listView_Info.SelectedItems)
                    txtInfo += lvi.SubItems[1].Text + ":" + lvi.SubItems[2].Text + "\n";

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
                    MessageBox.Show("导出代理地址出现错误");
                    return;
                }
                fs.Dispose();
                MessageBox.Show("已导出");
            }
        }
        private void btn_添加资源_Click(object sender, EventArgs e)
        {
            FrmSource fs = new FrmSource();
            if (fs.ShowDialog() == DialogResult.OK)
            {
                ZiYuan zy = fs.Tag as ZiYuan;
                listView_Source.Items.Add(new ListViewItem(new string[] { zy.name, zy.url }));
            }
        }
        private void btn_修改资源_Click(object sender, EventArgs e)
        {
            ZiYuan zy = new ZiYuan(
                listView_Source.SelectedItems[0].SubItems[0].Text,
                listView_Source.SelectedItems[0].SubItems[1].Text);

            FrmSource fs = new FrmSource(zy);
            if (fs.ShowDialog() == DialogResult.OK)
            {
                zy = fs.Tag as ZiYuan;
                listView_Source.SelectedItems[0].Text = zy.name;
                listView_Source.SelectedItems[0].SubItems[1].Text = zy.url;
            }
        }
        private void btn_删除资源_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除这些资源地址吗？删除后不能恢复！", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            foreach (ListViewItem lvi in listView_Source.SelectedItems)
                listView_Source.Items.Remove(lvi);
        }
        private void btn_下载代理资源_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(GetIPs);
            thread.IsBackground = true;
            thread.Start();
        }
        private void listView_Info_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DaiLi dl = new DaiLi(
                listView_Info.FocusedItem.SubItems[0].Text,
                listView_Info.FocusedItem.SubItems[1].Text,
                listView_Info.FocusedItem.SubItems[2].Text,
                listView_Info.FocusedItem.SubItems[3].Text,
                listView_Info.FocusedItem.SubItems[4].Text);

            FrmDaiLiInfo fdl = new FrmDaiLiInfo(dl);
            if (fdl.ShowDialog() == DialogResult.OK)
            {
                dl = fdl.Tag as DaiLi;
                listView_Info.FocusedItem.Text = dl.type;
                listView_Info.FocusedItem.SubItems[1].Text = dl.ip;
                listView_Info.FocusedItem.SubItems[2].Text = dl.port;
                listView_Info.FocusedItem.SubItems[3].Text = dl.time;
                listView_Info.FocusedItem.SubItems[4].Text = dl.info;
            }
        }
        private void listView_Source_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ZiYuan zy = new ZiYuan(
                listView_Source.FocusedItem.SubItems[0].Text,
                listView_Source.FocusedItem.SubItems[1].Text);

            FrmSource fs = new FrmSource(zy);
            if (fs.ShowDialog() == DialogResult.OK)
            {
                zy = fs.Tag as ZiYuan;
                listView_Source.FocusedItem.Text = zy.name;
                listView_Source.FocusedItem.SubItems[1].Text = zy.url;
            }
        }

        private void GetIPs()
        {
            foreach (ListViewItem lvi in listView_Source.CheckedItems)
            {
                string strHTML = GetHtmlCode(lvi.SubItems[0].Text, lvi.SubItems[1].Text);
                if (strHTML == "error")
                    continue;
                strHTML = ReplaceStrHtml(strHTML);
                AddInfo(strHTML);
            }
        }
        private string GetHtmlCode(string name, string url)
        {
            string str = string.Empty;
            WebClient wc = new WebClient();
            Stream myStream = null;
            try { myStream = wc.OpenRead(url); }
            catch
            {
                MessageBox.Show("读取网页失败!\n【" + name + "】有问题！\n请检测此网址：" + url);
                return "error";
            }
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("gb2312"));
            str = sr.ReadToEnd();
            myStream.Close();
            return str;
        }
        private string ReplaceStrHtml(string strHTML)
        {
            string z = "3", m = "4", k = "2", l = "9", d = "0", b = "5", i = "7", w = "6", r = "8", c = "1";
            strHTML = strHTML.Replace("</td><td>", ":");
            strHTML = strHTML.Replace("<SCRIPT type=text/javascript>document.write(", "").Replace("\"", "").Replace(")</SCRIPT>", "").Replace("+z", z).Replace("+m", m).Replace("+k", k).Replace("+l", l).Replace("+d", d).Replace("+b", b).Replace("+i", i).Replace("+w", w).Replace("+r", r).Replace("+c", c);
            strHTML = strHTML.Replace("<tr>", "").Replace("</tr>", "").Replace("<td>", "").Replace("</td>", "").Trim();
            return strHTML;
        }
        private void AddInfo(string strHTML)
        {
            Regex rg = new Regex(@"((\d{1,3}\.){3}\d{1,3}):(\d{1,5})");
            MatchCollection mas = rg.Matches(strHTML);
            foreach (Match match in mas)
            {
                listView_Info.Items.Add(
                    new ListViewItem(
                        new string[]{
                                "HTTP",
                                match.ToString().Split(new char[] { ':' })[0],
                                match.ToString().Split(new char[] { ':' })[1],
                                "未验证",
                                ""
                            }));
            }
        }
        private void SetDaiLi(string ipserver)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            rk.SetValue("ProxyEnable", 1);
            rk.SetValue("ProxyServer", ipserver);
            rk.Close();
            MessageBox.Show("成功！！！");
        }
        private void DelDaiLi()
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            rk.SetValue("ProxyEnable", 0);
            rk.Close();
            MessageBox.Show("成功！！！");
        }
        private void _Ping(object o)
        {
            List<int> listviewItems = o as List<int>;
            foreach (int i in listviewItems)
            {
                listView_Info.Items[i].SubItems[3].Text = "验证中...";
                PingHelper ph = new PingHelper();
                ph.PingComplete += new PingHelper.DlgPingCompleteHandler(ph_PingComplete);
                ph.PingIP(listView_Info.Items[i].SubItems[1].Text, i);
                Thread.Sleep(20);
            }
        }
        void ph_PingComplete(object sender, System.Net.NetworkInformation.PingCompletedEventArgs p, params object[] parameters)
        {
            int index = 0;
            int.TryParse(parameters[0].ToString(), out index);
            if (p.Reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                listView_Info.Items[index].SubItems[3].Text = p.Reply.RoundtripTime.ToString();
            else
                listView_Info.Items[index].SubItems[3].Text = "验证失败";
        }
        private void InitSetting()
        {

        }
        private void SaveSetting()
        {
            //INIFile file = new INIFile("Client.ini");
        }
    }
}
