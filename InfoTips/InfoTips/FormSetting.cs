using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using Microsoft.Win32;
using DevExpress.XtraTreeList.Nodes;
using AutoUpdate;
using System.Xml;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;

namespace InfoTips
{
    public partial class FormSetting : BaseForm
    {
        public FormSetting()
        {
            InitializeComponent();
        }
        private void FormSetting_Load(object sender, EventArgs e)
        {
            InitSetting();
        }
        public void InitSetting()
        {
            XMLHandle app = new XMLHandle();
            #region 自动启动
            if (app.GetAppValue("自动启动") == "1")
                checkBox_自动启动.Checked = true;
            else
                checkBox_自动启动.Checked = false;
            #endregion
            #region 语音提醒
            if (app.GetAppValue("语音提醒") == "1")
                checkBox_语音提醒.Checked = true;
            else
            {
                checkBox_语音提醒.Checked = false;
                comboBox语音文件.Enabled = false;
            }
            if (!string.IsNullOrEmpty(app.GetAppValue("语音文件")))
                comboBox语音文件.SelectedItem = app.GetAppValue("语音文件");
            else
                comboBox语音文件.SelectedIndex = 0;
            #endregion
            #region 自动弹窗
            if (app.GetAppValue("自动弹窗") == "1")
                checkBox_自动弹窗.Checked = true;
            else
            {
                checkBox_自动弹窗.Checked = false;
                comboBox弹窗时间.Enabled = false;
            }
            if (!string.IsNullOrEmpty(app.GetAppValue("弹窗时间")))
                comboBox弹窗时间.SelectedItem = app.GetAppValue("弹窗时间") + "秒";
            else
                comboBox弹窗时间.SelectedIndex = 0;
            #endregion
            #region 财经日历
            if (!string.IsNullOrEmpty(app.GetAppValue("财经日历时间")))
                comboBox财经日历.SelectedItem = app.GetAppValue("财经日历时间") + "分钟前";
            else
                comboBox财经日历.SelectedIndex = 0;
            #endregion
            #region 新闻选择
            for (int i = 0; i < treeList1.Nodes.Count; i++)
            {
                string strCaiJi = app.GetAppValue(treeList1.Nodes[i].GetValue(0).ToString() + "采集");
                int nTime = Convert.ToInt32(app.GetAppValue(treeList1.Nodes[i].GetValue(0).ToString() + "时间"));
                if (nTime < 60)
                    treeList1.Nodes[i].SetValue(1, nTime + "秒");
                else
                    treeList1.Nodes[i].SetValue(1, nTime / 60 + "分钟");
                treeList1.Nodes[i].SetValue(2, strCaiJi == "1" ? true : false);
            }
            #endregion
            #region 评论中心
            for (int i = 0; i < treeList2.Nodes.Count; i++)
            {
                string strCaiJi = app.GetAppValue(treeList2.Nodes[i].GetValue("名称").ToString() + "采集");
                int nTime = Convert.ToInt32(app.GetAppValue(treeList2.Nodes[i].GetValue("名称").ToString() + "时间"));
                if (nTime < 60)
                    treeList2.Nodes[i].SetValue("时间", nTime + "秒");
                else
                    treeList2.Nodes[i].SetValue("时间", nTime / 60 + "分钟");
                treeList2.Nodes[i].SetValue("采集", strCaiJi == "1" ? true : false);
            }
            #endregion
            #region 火线速递
            for (int i = 0; i < treeList3.Nodes.Count; i++)
            {
                string strHuoXian = app.GetAppValue(treeList1.Nodes[i].GetValue(0).ToString() + "采集");
                int nTime = Convert.ToInt32(app.GetAppValue(treeList1.Nodes[i].GetValue(0).ToString() + "时间"));
                treeList1.Nodes[i].SetValue(1, nTime + "秒");
                treeList1.Nodes[i].SetValue(2, strHuoXian == "1" ? true : false);
            }
            #endregion

            #region
            //XmlFiles xf = new XmlFiles(Application.StartupPath + "\\NaviBar.xml");
            //foreach (XmlNode xn in xf.GetNodeList("NaviBars"))
            //{
            //    TreeListColumn tlc = new TreeListColumn();
            //    tlc.Caption = "名称";
            //    tlc.FieldName = "名称";
            //    tlc.MinWidth = 34;
            //    tlc.Name = "treeListColumn1";
            //    tlc.OptionsColumn.AllowEdit = false;
            //    tlc.OptionsColumn.AllowMove = false;
            //    tlc.OptionsColumn.AllowSize = false;
            //    tlc.OptionsColumn.AllowSort = false;
            //    tlc.OptionsColumn.ReadOnly = true;
            //    tlc.SortOrder = System.Windows.Forms.SortOrder.Descending;
            //    tlc.Visible = true;
            //    TreeListColumn tlc1 = new TreeListColumn();
            //    tlc1.Caption = "显示";
            //    tlc1.FieldName = "显示";
            //    tlc1.MinWidth = 34;
            //    tlc1.Name = "treeListColumn2";
            //    tlc1.OptionsColumn.AllowEdit = false;
            //    tlc1.OptionsColumn.AllowMove = false;
            //    tlc1.OptionsColumn.AllowSize = false;
            //    tlc1.OptionsColumn.AllowSort = false;
            //    tlc1.OptionsColumn.ReadOnly = true;
            //    tlc1.SortOrder = System.Windows.Forms.SortOrder.Descending;
            //    tlc1.Visible = true;

            //    XtraTabPage xtp = new XtraTabPage();
            //    xtp.Text = xn.Attributes["Name"].Value;
            //    xtraTabControl1.TabPages.Add(xtp);
            //    TreeList tl = new TreeList();
            //    tl.Columns.AddRange(new TreeListColumn[] { tlc, tlc1 });
            //    tl.Dock = DockStyle.Fill;
            //    tl.OptionsLayout.AddNewColumns = false;
            //    tl.OptionsMenu.EnableColumnMenu = false;
            //    tl.OptionsMenu.EnableFooterMenu = false;
            //    tl.OptionsSelection.EnableAppearanceFocusedCell = false;
            //    tl.OptionsView.ShowFocusedFrame = false;
            //    tl.OptionsView.ShowIndicator = false;
            //    tl.OptionsView.ShowRoot = false;
            //    tl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            //    this.repositoryItemComboBox1});
            //    tl.RowHeight = 23;
            //    tl.Size = new System.Drawing.Size(624, 168);
            //    tl.TabIndex = 1;
            //    tl.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.Light;
            //    xtp.Controls.Add(tl);
            //    foreach (XmlNode xn1 in xn.ChildNodes)
            //    {
            //        tl.AppendNode(new object[] { xn1.Attributes["Name"].Value, xn1.Attributes["Enable"].Value == "1" ? true : false }, null);
            //    }
            //}
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox_语音提醒.Checked && string.IsNullOrEmpty(comboBox语音文件.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择语音提醒铃声文件!");
                return;
            }
            if (checkBox_自动弹窗.Checked && string.IsNullOrEmpty(comboBox弹窗时间.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择弹窗显示时间!");
                return;
            }
            try
            {
                XMLHandle app = new XMLHandle();
                #region 自动启动
                if (checkBox_自动启动.Checked)
                {
                    app.SetAppValue("自动启动", "1");
                    RunWhenStart(true, Application.ProductName, Application.StartupPath + @"\\InfoTips.exe\");
                }
                else
                {
                    app.SetAppValue("自动启动", "0");
                    RunWhenStart(false, Application.ProductName, Application.StartupPath + @"\\InfoTips.exe\");
                }
                #endregion
                #region 语音提醒
                if (checkBox_语音提醒.Checked)
                    app.SetAppValue("语音提醒", "1");
                else
                    app.SetAppValue("语音提醒", "0");
                app.SetAppValue("语音文件", comboBox语音文件.SelectedItem.ToString());
                #endregion
                #region 自动弹窗
                if (checkBox_自动弹窗.Checked)
                    app.SetAppValue("自动弹窗", "1");
                else
                    app.SetAppValue("自动弹窗", "0");
                app.SetAppValue("弹窗时间", comboBox弹窗时间.SelectedItem.ToString().Replace("秒", ""));
                #endregion
                #region 财经日历
                app.SetAppValue("财经日历时间", comboBox财经日历.SelectedItem.ToString().Replace("分钟前", ""));
                #endregion
                #region 新闻选择
                foreach (TreeListNode tln in treeList1.Nodes)
                {
                    if (tln.GetValue(1).ToString().Contains("秒"))
                        app.SetAppValue(tln.GetValue(0).ToString() + "时间", tln.GetValue(1).ToString().Replace("秒", ""));
                    else
                        app.SetAppValue(tln.GetValue(0).ToString() + "时间", (Convert.ToInt32(tln.GetValue(1).ToString().Replace("分钟", "")) * 60).ToString());
                    app.SetAppValue(tln.GetValue(0).ToString() + "采集", Convert.ToInt32(tln.GetValue(2)) == 1 ? "1" : "0");
                }
                #endregion
                #region 评论中心
                #region
                //foreach (TreeListNode tln in treeList2.Nodes)
                //{
                //    if (tln.GetValue("时间").ToString().Contains("秒"))
                //        app.SetAppValue(tln.GetValue("名称").ToString() + "时间", tln.GetValue("时间").ToString().Replace("秒", ""));
                //    else
                //        app.SetAppValue(tln.GetValue("名称").ToString() + "时间", (Convert.ToInt32(tln.GetValue("时间").ToString().Replace("分钟", "")) * 60).ToString());
                //    app.SetAppValue(tln.GetValue("名称").ToString() + "采集", Convert.ToInt32(tln.GetValue("时间")) == 1 ? "1" : "0");
                //}
                #endregion
                foreach (TreeListNode tln in treeList2.Nodes)
                {
                    app.SetAppValue(tln.GetValue("名称").ToString() + "时间", "600");
                    app.SetAppValue(tln.GetValue("名称").ToString() + "采集", Convert.ToInt32(tln.GetValue("采集")) == 1 ? "1" : "0");
                }
                #endregion
                #region 火线速递
                foreach (TreeListNode tln in treeList3.Nodes)
                {
                    app.SetAppValue(tln.GetValue(0).ToString() + "时间", tln.GetValue(1).ToString().Replace("秒", ""));
                    app.SetAppValue(tln.GetValue(0).ToString() + "采集", Convert.ToInt32(tln.GetValue(2)) == 1 ? "1" : "0");
                }
                #endregion
                app.SaveAppConfig();
                MessageBox.Show("保存成功！");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch { MessageBox.Show("保存失败！"); }
        }
        private void checkBox_自动弹窗_CheckedChanged(object sender, EventArgs e)
        {
            comboBox弹窗时间.Enabled = checkBox_自动弹窗.Checked;
        }
        private void checkBox_语音提醒_CheckedChanged(object sender, EventArgs e)
        {
            comboBox语音文件.Enabled = checkBox_语音提醒.Checked;
        }
        /// <summary> 
        /// 开机启动项 
        /// </summary> 
        /// <param name=\"Started\">是否启动</param> 
        /// <param name=\"name\">启动值的名称</param> 
        /// <param name=\"path\">启动程序的路径</param> 
        public static void RunWhenStart(bool Started, string name, string path)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");
            if (Started == true)
            {
                try
                {
                    Run.SetValue(name, path);
                    HKLM.Close();
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message.ToString(), "友情提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
                catch (Exception) { }
            }
        }
    }
}