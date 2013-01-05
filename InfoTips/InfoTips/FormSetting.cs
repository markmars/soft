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

namespace InfoTips
{
    public partial class FormSetting : Form
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
            #region 新闻选择
            foreach (TreeListNode tln in treeList1.Nodes)
            {
                string strCaiJi = app.GetAppValue(tln.GetValue(0).ToString() + "采集");
                int nTime = Convert.ToInt32(app.GetAppValue(tln.GetValue(0).ToString() + "时间"));
                if (nTime < 60)
                    tln.SetValue(1, nTime + "秒");
                else
                    tln.SetValue(1, nTime / 60 + "分钟");
                tln.SetValue(2, strCaiJi == "1" ? true : false);
            }
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