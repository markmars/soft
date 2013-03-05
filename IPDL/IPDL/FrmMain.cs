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
        ListViewColumnSorter lvwColumnSorter;
        int m_count = 0, lockIndex = 0;
        List<Thread> list_threads;

        public FrmMain()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            lvwColumnSorter = new ListViewColumnSorter();
            this.listView_Info.ListViewItemSorter = lvwColumnSorter;

            InitSetting();
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出？", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            SaveSetting();
        }
        private void btn_设置_Click(object sender, EventArgs e)
        {
            FrmSetting fs = new FrmSetting();
            fs.ShowDialog();
        }
        private void btn_验证全部_Click(object sender, EventArgs e)
        {
            if (listView_Info.Items.Count == 0)
                return;
            if (btn_验证全部.Text != "停止验证")
            {
                list_threads = new List<Thread>();
                List<int> ls = new List<int>();
                foreach (ListViewItem lvi in listView_Info.Items)
                    ls.Add(lvi.Index);

                btn_验证全部.Text = "停止验证";
                btn_设置.Enabled = false;
                btn_验证选中.Enabled = false;
                btn_添加代理.Enabled = false;
                btn_修改选中.Enabled = false;
                btn_删除.Enabled = false;
                btn_清理.Enabled = false;
                btn_导入.Enabled = false;
                btn_粘贴.Enabled = false;
                toolStripStatusLabel_baifenbi.Text = "0.00%";
                toolStripProgressBar.Maximum = ls.Count;
                for (int i = 0; i < Utility.m_并发线程数量; i++)
                {
                    Thread thread = new Thread(Ping);
                    thread.IsBackground = true;
                    thread.Start(ls);
                    list_threads.Add(thread);
                }
            }
            else
            {
                btn_验证全部.Text = "停止中...";
                btn_验证全部.Enabled = false;
                Thread thread = new Thread(StopPing);
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
        }
        private void btn_验证选中_Click(object sender, EventArgs e)
        {
            if (listView_Info.SelectedItems.Count == 0)
                return;
            if (btn_验证选中.Text != "停止验证")
            {
                list_threads = new List<Thread>();
                List<int> ls = new List<int>();
                foreach (ListViewItem lvi in listView_Info.SelectedItems)
                    ls.Add(lvi.Index);

                btn_验证选中.Text = "停止验证";
                btn_设置.Enabled = false;
                btn_验证全部.Enabled = false;
                btn_添加代理.Enabled = false;
                btn_修改选中.Enabled = false;
                btn_删除.Enabled = false;
                btn_清理.Enabled = false;
                btn_导入.Enabled = false;
                btn_粘贴.Enabled = false;
                toolStripStatusLabel_baifenbi.Text = "0.00%";
                toolStripProgressBar.Maximum = ls.Count;
                for (int i = 0; i < Utility.m_并发线程数量; i++)
                {
                    Thread thread = new Thread(Ping);
                    thread.IsBackground = true;
                    thread.Start(ls);
                    list_threads.Add(thread);
                }
            }
            else
            {
                Thread thread = new Thread(StopPing);
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
        }
        private void btn_设置代理_Click(object sender, EventArgs e)
        {
            if (listView_Info.SelectedItems.Count == 0)
            {
                MessageBox.Show("无选中代理");
                return;
            }
            if (SetDaiLi(listView_Info.SelectedItems[0].SubItems[1].Text + ":" + listView_Info.SelectedItems[0].SubItems[2].Text))
                toolStripStatusLabel_ZhuangTai.Text = "代理状态：" + listView_Info.SelectedItems[0].SubItems[1].Text + ":" + listView_Info.SelectedItems[0].SubItems[2].Text;
            else
                toolStripStatusLabel_ZhuangTai.Text = "代理状态：未设置";
        }
        private void btn_取消代理_Click(object sender, EventArgs e)
        {
            if (DelDaiLi())
                toolStripStatusLabel_ZhuangTai.Text = "代理状态：未设置";
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
            toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
        }
        private void btn_清理_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除“验证失败”的代理吗？\r\n数据删除后不能恢复！有些代理过段时间可能还可以用！", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            foreach (ListViewItem lvi in listView_Info.Items)
            {
                if (lvi.SubItems[3].Text == "验证失败")
                    listView_Info.Items.Remove(lvi);
            }
            toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
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
                Regex rg = new Regex(@"((\d{1,3}\.){3}\d{1,3}):(\d{1,5}):(.*)");
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
                                m.ToString().Split(new char[] { ':' }).Length==3?m.ToString().Split(new char[] { ':' })[2]:""
                            }));
                    toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
                }
            }
        }
        private void btn_粘贴_Click(object sender, EventArgs e)
        {

            string txtInfo = Clipboard.GetData(DataFormats.Text).ToString(); ;
            Regex rg = new Regex(@"((\d{1,3}\.){3}\d{1,3}):(\d{1,5}):(.*)");
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
                                m.ToString().Split(new char[] { ':' }).Length==3?m.ToString().Split(new char[] { ':' })[2]:""
                            }));
                toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
            }
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
                    txtInfo += lvi.SubItems[1].Text + ":" + lvi.SubItems[2].Text + ":" + lvi.SubItems[4].Text + "\n";

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
            if (listView_Source.CheckedItems.Count == 0)
            {
                MessageBox.Show("无选中下载地址");
                return;
            }
            btn_下载代理资源.Enabled = false;
            m_count = 0;
            toolStripStatusLabel_baifenbi.Text = "0.00%";
            toolStripProgressBar.Maximum = listView_Source.CheckedItems.Count;
            foreach (ListViewItem lvi in listView_Source.CheckedItems)
            {
                Thread thread = new Thread(GetIPs);
                thread.IsBackground = true;
                thread.Start(new ZiYuan(lvi.SubItems[0].Text, lvi.SubItems[1].Text));
            }
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
        private void listView_Info_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
        }
        private void listView_Info_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void listView_Info_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                StreamReader sr = new StreamReader(filePath);
                string txtInfo = sr.ReadToEnd();
                Regex rg = new Regex(@"((\d{1,3}\.){3}\d{1,3}):(\d{1,5}):(.*)");
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
                                m.ToString().Split(new char[] { ':' }).Length==3?m.ToString().Split(new char[] { ':' })[2]:""
                            }));
                    toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
                }
                sr.Close();
            }
        }
        private void listView_Info_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (btn_验证全部.Text == "停止验证" || btn_验证选中.Text == "停止验证")
                return;
            switch ((int)e.KeyChar)
            {
                case 1:
                    {
                        foreach (ListViewItem lvi in listView_Info.Items)
                            lvi.Selected = true;
                    }
                    break;
                case 3:
                    {
                        string txtInfo = string.Empty;
                        foreach (ListViewItem lvi in listView_Info.SelectedItems)
                            txtInfo += lvi.SubItems[1].Text + ":" + lvi.SubItems[2].Text + ":" + lvi.SubItems[4].Text + "\n";
                        Clipboard.SetData(DataFormats.Text, txtInfo);
                    }
                    break;
                case 4:
                    {
                        foreach (ListViewItem lvi in listView_Info.SelectedItems)
                            listView_Info.Items.Remove(lvi);
                    }
                    break;
                case 19:
                    {
                        if (listView_Info.SelectedItems.Count == 0)
                            break;
                        btn_导出选定_Click(null, null);
                    }
                    break;
                case 22:
                    btn_粘贴_Click(null, null);
                    break;
                case 24:
                    {
                        string txtInfo = string.Empty;
                        foreach (ListViewItem lvi in listView_Info.SelectedItems)
                        {
                            txtInfo += lvi.SubItems[1].Text + ":" + lvi.SubItems[2].Text + ":" + lvi.SubItems[4].Text + "\n";
                            listView_Info.Items.Remove(lvi);
                            toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
                        }
                        Clipboard.SetData(DataFormats.Text, txtInfo);
                    }
                    break;
            }
        }
        private void listView_Info_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查点击的列是不是现在的排序列.  
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // 重新设置此列的排序方法.  
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序  
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // 用新的排序方法对ListView排序  
            this.listView_Info.Sort();
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
        private void listView_Source_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void listView_Source_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                StreamReader sr = new StreamReader(filePath);
                string txtInfo = sr.ReadToEnd();
                Regex rg = new Regex(@"(.+)##((http://).+)##(0|1)");
                MatchCollection mas = rg.Matches(txtInfo);
                foreach (Match m in mas)
                {
                    listView_Source.Items.Add(
                        new ListViewItem(
                            new string[]{
                                m.ToString().Split(new string[] { "##" },StringSplitOptions.None)[0],
                                m.ToString().Split(new string[] { "##" },StringSplitOptions.None)[1]
                            }));
                    if (m.ToString().Split(new string[] { "##" }, StringSplitOptions.None)[2] == "1")
                        listView_Source.Items[listView_Source.Items.Count - 1].Checked = true;
                }
                sr.Close();
            }
        }
        private void listView_Source_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((int)e.KeyChar)
            {
                case 1:
                    {
                        foreach (ListViewItem lvi in listView_Info.Items)
                            lvi.Selected = true;
                    }
                    break;
                case 3:
                    {
                        string txtInfo = string.Empty;
                        foreach (ListViewItem lvi in listView_Source.SelectedItems)
                            txtInfo += lvi.SubItems[0].Text + "##" + lvi.SubItems[1].Text + "##" + (lvi.Checked ? "1" : "0") + "\n";
                        Clipboard.SetData(DataFormats.Text, txtInfo);
                    }
                    break;
                case 4:
                    {
                        foreach (ListViewItem lvi in listView_Info.SelectedItems)
                            listView_Info.Items.Remove(lvi);
                    }
                    break;
                case 19:
                    {
                        if (listView_Info.SelectedItems.Count == 0)
                            break;
                        btn_导出选定_Click(null, null);
                    }
                    break;
                case 22:
                    {
                        string txtInfo = Clipboard.GetData(DataFormats.Text).ToString(); ;
                        Regex rg = new Regex(@"(.+)##((http://).+)##(0|1)");
                        MatchCollection mas = rg.Matches(txtInfo);
                        foreach (Match m in mas)
                        {
                            listView_Source.Items.Add(
                                new ListViewItem(
                                    new string[]{
                                m.ToString().Split(new string[] { "##" },StringSplitOptions.None)[0],
                                m.ToString().Split(new string[] { "##" },StringSplitOptions.None)[1]
                            }));
                            if (m.ToString().Split(new string[] { "##" }, StringSplitOptions.None)[2] == "1")
                                listView_Source.Items[listView_Source.Items.Count - 1].Checked = true;
                        }
                    }
                    break;
                case 24:
                    {
                        string txtInfo = string.Empty;
                        foreach (ListViewItem lvi in listView_Source.SelectedItems)
                        {
                            txtInfo += lvi.SubItems[0].Text + "##" + lvi.SubItems[1].Text + "##" + (lvi.Checked ? "1" : "0") + "\n";
                            listView_Source.Items.Remove(lvi);
                        }
                        Clipboard.SetData(DataFormats.Text, txtInfo);
                    }
                    break;
            }
        }

        private void GetIPs(object o)
        {
            ZiYuan zy = o as ZiYuan;
            string strHTML = GetHtmlCode(zy.name, zy.url);
            if (strHTML == "error")
                return;
            strHTML = ReplaceStrHtml(strHTML);
            AddInfo(strHTML);
            m_count++;
            toolStripProgressBar.Value = m_count;
            toolStripStatusLabel_baifenbi.Text = ((m_count) * 100.0 / toolStripProgressBar.Maximum).ToString("0.00") + "%";
            if (toolStripProgressBar.Value == toolStripProgressBar.Maximum)
            {
                toolStripStatusLabel_baifenbi.Text = "完成";
                toolStripProgressBar.Value = 0;
                btn_下载代理资源.Enabled = true;
                m_count = 0;
            }
            Thread.CurrentThread.Abort();
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
            Regex rg = new Regex(@"((\d{1,3}\.){3}\d{1,3}):(\d{1,5}):(.*)");
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
                                match.ToString().Split(new char[] { ':' }).Length==3?match.ToString().Split(new char[] { ':' })[2]:""
                            }));
                toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
            }
        }
        private bool SetDaiLi(string ipserver)
        {
            try
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
                rk.SetValue("ProxyEnable", 1);
                rk.SetValue("ProxyServer", ipserver);
                rk.Close();
            }
            catch (Exception e) { MessageBox.Show("发生未知错误，请联系作者"); return false; }
            return true;
        }
        private bool DelDaiLi()
        {
            try
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
                rk.SetValue("ProxyEnable", 0);
                rk.Close();
            }
            catch (Exception e) { MessageBox.Show("发生未知错误，请联系作者"); return false; }
            return true;
        }
        private void Ping(object o)
        {
            List<int> ls = o as List<int>;
            string sUrl = Utility.m_验证地址[0];
            while (lockIndex != ls.Count)
            {
                int i = lockIndex;
                lock (this)
                {
                    lockIndex++;
                }
                double time = 0;
                string sIP = listView_Info.Items[ls[i]].SubItems[1].Text + ":" + listView_Info.Items[ls[i]].SubItems[2].Text;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sUrl);
                WebProxy webPro = new WebProxy();
                webPro.Address = new Uri("http://" + sIP);
                DateTime beginTime = DateTime.Now;
                listView_Info.Items[ls[i]].SubItems[3].Text = "验证中...";

                try
                {
                    request.Timeout = Utility.m_验证超时时间;
                    request.ReadWriteTimeout = Utility.m_连接超时时间;
                    request.ContentType = "text/xml";
                    request.Proxy = webPro;
                    request.GetResponse();
                    DateTime endTime = DateTime.Now;
                    time = (int)(endTime - beginTime).TotalMilliseconds;
                }
                catch
                {
                    request.Abort();
                    time = -1;
                }
                if (time == -1)
                    listView_Info.Items[ls[i]].SubItems[3].Text = "验证失败";
                else
                    listView_Info.Items[ls[i]].SubItems[3].Text = (time / 1000).ToString("0.00");
                m_count++;
                toolStripProgressBar.Value = m_count;
                toolStripStatusLabel_baifenbi.Text = ((m_count) * 100.0 / toolStripProgressBar.Maximum).ToString("0.00") + "%";
                if (toolStripProgressBar.Value == toolStripProgressBar.Maximum)
                {
                    toolStripStatusLabel_baifenbi.Text = "完成";
                    toolStripProgressBar.Value = 0;
                    if (btn_验证全部.Text == "停止验证")
                    {
                        btn_验证全部.Text = "验证全部";
                    }
                    if (btn_验证选中.Text == "停止验证")
                    {
                        btn_验证选中.Text = "验证选中";
                    }
                    btn_验证全部.Enabled = true;
                    btn_验证选中.Enabled = true;
                    btn_设置.Enabled = true;
                    btn_添加代理.Enabled = true;
                    btn_修改选中.Enabled = true;
                    btn_删除.Enabled = true;
                    btn_清理.Enabled = true;
                    btn_导入.Enabled = true;
                    btn_粘贴.Enabled = true;
                    lockIndex = 0;
                    m_count = 0;
                }
            }
            Thread.CurrentThread.Abort();
        }
        private void StopPing()
        {
            foreach (Thread th in list_threads)
            {
                if (th.ThreadState != ThreadState.Stopped)
                {
                    th.Abort();
                    th.Join(10);
                }
            }
            toolStripStatusLabel_baifenbi.Text = "完成";
            toolStripProgressBar.Value = 0;
            if (btn_验证全部.Text == "停止中...")
            {
                btn_验证全部.Text = "验证全部";
                btn_验证全部.Enabled = true;
            }
            if (btn_验证选中.Text == "停止中...")
            {
                btn_验证选中.Text = "验证选中";
                btn_验证选中.Enabled = true;
            }
            btn_验证全部.Enabled = true;
            btn_验证选中.Enabled = true;
            btn_设置.Enabled = true;
            btn_添加代理.Enabled = true;
            btn_修改选中.Enabled = true;
            btn_删除.Enabled = true;
            btn_清理.Enabled = true;
            btn_导入.Enabled = true;
            btn_粘贴.Enabled = true;
            lockIndex = 0;
            m_count = 0;
            Thread.CurrentThread.Abort();
        }
        private void InitSetting()
        {
            StreamReader sr;
            string txtInfo;
            Regex rg;
            MatchCollection mas;
            if (File.Exists(Application.StartupPath + "\\dllb.dat"))
            {
                sr = new StreamReader(Application.StartupPath + "\\dllb.dat");
                txtInfo = sr.ReadToEnd();
                rg = new Regex(@"((\d{1,3}\.){3}\d{1,3}):(\d{1,5}):(.*)");
                mas = rg.Matches(txtInfo);
                foreach (Match m in mas)
                {
                    listView_Info.Items.Add(
                        new ListViewItem(
                            new string[]{
                                "HTTP",
                                m.ToString().Split(new char[] { ':' })[0],
                                m.ToString().Split(new char[] { ':' })[1],
                                "未验证",
                                m.ToString().Split(new char[] { ':' }).Length==3?m.ToString().Split(new char[] { ':' })[2]:""
                            }));
                    toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;
                }
                sr.Close();
            }

            if (File.Exists(Application.StartupPath + "\\zylb.dat"))
            {
                sr = new StreamReader(Application.StartupPath + "\\zylb.dat");
                txtInfo = sr.ReadToEnd();
                rg = new Regex(@"(.+)##((http://).+)##(0|1)");
                mas = rg.Matches(txtInfo);
                foreach (Match m in mas)
                {
                    listView_Source.Items.Add(
                        new ListViewItem(
                            new string[]{
                                m.ToString().Split(new string[] { "##" },StringSplitOptions.None)[0],
                                m.ToString().Split(new string[] { "##" },StringSplitOptions.None)[1]
                            }));
                    if (m.ToString().Split(new string[] { "##" }, StringSplitOptions.None)[2] == "1")
                        listView_Source.Items[listView_Source.Items.Count - 1].Checked = true;
                }
                sr.Close();
            }
            toolStripStatusLabel_GeShu.Text = "代理个数：" + listView_Info.Items.Count + ",当前选中：" + listView_Info.SelectedItems.Count;

            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            if (rk.GetValue("ProxyEnable").ToString() == "1")
                toolStripStatusLabel_ZhuangTai.Text = "代理状态：" + rk.GetValue("ProxyServer");
            rk.Close();

            IniFile inifile;
            string strPath = Application.StartupPath + "\\Setting.ini";
            if (!File.Exists(strPath))
            {
                MessageBox.Show("配置文件缺失，程序进行自动生成");
                FileStream fs = new FileStream(strPath, FileMode.Create);
                fs.Dispose();
                inifile = new IniFile(strPath);
                inifile.SetValue("连接超时时间", "时间", "5");
                inifile.SetValue("验证超时时间", "时间", "30");
                inifile.SetValue("并发线程数目", "个数", "50");
                inifile.SetValue("验证资源", "资源1", "独醒资源##http://www.wakealone.com##1");
                inifile.SetValue("验证资源", "资源2", "百度一下##http://www.baidu.com##0");
                inifile.SetValue("验证资源", "资源3", "谷歌一下##http://www.google.com##0");
            }
            inifile = new IniFile(strPath);
            Utility.m_连接超时时间 = int.Parse(inifile.GetValue("连接超时时间", "时间")) * 1000;
            Utility.m_验证超时时间 = int.Parse(inifile.GetValue("验证超时时间", "时间")) * 1000;
            Utility.m_并发线程数量 = int.Parse(inifile.GetValue("并发线程数目", "个数"));
            Utility.m_验证地址 = new List<string>();
            Utility.m_验证地址.Add("http://www.baidu.com");
        }
        private void SaveSetting()
        {
            FileStream fs = new FileStream(Application.StartupPath + "\\dllb.dat", FileMode.Create);
            string txtInfo = string.Empty;
            foreach (ListViewItem lvi in listView_Info.Items)
                txtInfo += lvi.SubItems[1].Text + ":" + lvi.SubItems[2].Text + ":" + lvi.SubItems[4].Text + "\n";
            try
            {
                byte[] bs = Encoding.Default.GetBytes(txtInfo);
                fs.Write(bs, 0, bs.Length);
            }
            catch { return; }

            fs = new FileStream(Application.StartupPath + "\\zylb.dat", FileMode.Create);
            txtInfo = string.Empty;
            foreach (ListViewItem lvi in listView_Source.Items)
                txtInfo += lvi.SubItems[0].Text + "##" + lvi.SubItems[1].Text + "##" + (lvi.Checked ? "1" : "0") + "\n";
            try
            {
                byte[] bs = Encoding.UTF8.GetBytes(txtInfo);
                fs.Write(bs, 0, bs.Length);
            }
            catch { return; }

            fs.Dispose();
        }
    }
}
