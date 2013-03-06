using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InfoTips
{
    public partial class Form快捷键 : BaseForm
    {
        private IntPtr PHandle;
        public Form快捷键(IntPtr handle)
        {
            InitializeComponent();
            PHandle = handle;

            XMLHandle app = new XMLHandle();
            app.LoadAppConfig();
            贵金属1.SelectedItem = app.GetAppValue("贵金属快捷开");
            贵金属2.SelectedItem = app.GetAppValue("贵金属快捷关");
            市场报价1.SelectedItem = app.GetAppValue("市场报价快捷开");
            市场报价2.SelectedItem = app.GetAppValue("市场报价快捷关");
            外汇行情1.SelectedItem = app.GetAppValue("外汇行情快捷开");
            外汇行情2.SelectedItem = app.GetAppValue("外汇行情快捷关");
            全球股指1.SelectedItem = app.GetAppValue("全球股指快捷开");
            全球股指2.SelectedItem = app.GetAppValue("全球股指快捷关");
            金价恒信贵1.SelectedItem = app.GetAppValue("金价恒信贵快捷开");
            金价恒信贵2.SelectedItem = app.GetAppValue("金价恒信贵快捷关");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region
            List<string> ls = new List<string>();
            ls.Add(贵金属1.SelectedIndex.ToString());
            ls.Remove("0");
            if (!ls.Contains(贵金属2.SelectedIndex.ToString()))
            {
                ls.Add(贵金属2.SelectedIndex.ToString());
                ls.Remove("0");
                if (!ls.Contains(市场报价1.SelectedIndex.ToString()))
                {
                    ls.Add(市场报价1.SelectedIndex.ToString());
                    ls.Remove("0");
                    if (!ls.Contains(市场报价2.SelectedIndex.ToString()))
                    {
                        ls.Add(市场报价2.SelectedIndex.ToString());
                        ls.Remove("0");
                        if (!ls.Contains(外汇行情1.SelectedIndex.ToString()))
                        {
                            ls.Add(外汇行情1.SelectedIndex.ToString());
                            ls.Remove("0");
                            if (!ls.Contains(外汇行情2.SelectedIndex.ToString()))
                            {
                                ls.Add(外汇行情2.SelectedIndex.ToString());
                                ls.Remove("0");
                                if (!ls.Contains(全球股指1.SelectedIndex.ToString()))
                                {
                                    ls.Add(全球股指1.SelectedIndex.ToString());
                                    ls.Remove("0");
                                    if (!ls.Contains(全球股指2.SelectedIndex.ToString()))
                                    {
                                        ls.Add(全球股指2.SelectedIndex.ToString());
                                        ls.Remove("0");
                                        if (!ls.Contains(金价恒信贵1.SelectedIndex.ToString()))
                                        {
                                            ls.Add(金价恒信贵1.SelectedIndex.ToString());
                                            ls.Remove("0");
                                            if (!ls.Contains(金价恒信贵2.SelectedIndex.ToString()))
                                            {
                                                ls.Add(金价恒信贵2.SelectedIndex.ToString());
                                                ls.Remove("0");
                                            }
                                            else
                                            {
                                                MessageBox.Show("不能有相同热键");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("不能有相同热键");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("不能有相同热键");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("不能有相同热键");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("不能有相同热键");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("不能有相同热键");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("不能有相同热键");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("不能有相同热键");
                    return;
                }
            }
            else
            {
                MessageBox.Show("不能有相同热键");
                return;
            }
            #endregion

            XMLHandle app = new XMLHandle();
            app.SetAppValue("贵金属快捷开", 贵金属1.SelectedItem.ToString());
            app.SetAppValue("贵金属快捷关", 贵金属2.SelectedItem.ToString());
            app.SetAppValue("市场报价快捷开", 市场报价1.SelectedItem.ToString());
            app.SetAppValue("市场报价快捷关", 市场报价2.SelectedItem.ToString());
            app.SetAppValue("外汇行情快捷开", 外汇行情1.SelectedItem.ToString());
            app.SetAppValue("外汇行情快捷关", 外汇行情2.SelectedItem.ToString());
            app.SetAppValue("全球股指快捷开", 全球股指1.SelectedItem.ToString());
            app.SetAppValue("全球股指快捷关", 全球股指2.SelectedItem.ToString());
            app.SetAppValue("金价恒信贵快捷开", 金价恒信贵1.SelectedItem.ToString());
            app.SetAppValue("金价恒信贵快捷关", 金价恒信贵2.SelectedItem.ToString());
            app.SaveAppConfig();

            UnRegistHotKey();
            RegistHotKey(贵金属1.SelectedItem.ToString());
            RegistHotKey(贵金属2.SelectedItem.ToString());
            RegistHotKey(市场报价1.SelectedItem.ToString());
            RegistHotKey(市场报价2.SelectedItem.ToString());
            RegistHotKey(外汇行情1.SelectedItem.ToString());
            RegistHotKey(外汇行情2.SelectedItem.ToString());
            RegistHotKey(全球股指1.SelectedItem.ToString());
            RegistHotKey(全球股指2.SelectedItem.ToString());
            RegistHotKey(金价恒信贵1.SelectedItem.ToString());
            RegistHotKey(金价恒信贵2.SelectedItem.ToString());

            MessageBox.Show("保存成功！");
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void RegistHotKey(string value)
        {
            switch (value)
            {
                case "F1":
                    HotKey.RegisterHotKey(this.PHandle, 100, HotKey.KeyModifiers.None, Keys.F1);
                    break;
                case "F2":
                    HotKey.RegisterHotKey(this.PHandle, 101, HotKey.KeyModifiers.None, Keys.F2);
                    break;
                case "F3":
                    HotKey.RegisterHotKey(this.PHandle, 102, HotKey.KeyModifiers.None, Keys.F3);
                    break;
                case "F4":
                    HotKey.RegisterHotKey(this.PHandle, 103, HotKey.KeyModifiers.None, Keys.F4);
                    break;
                case "F5":
                    HotKey.RegisterHotKey(this.PHandle, 104, HotKey.KeyModifiers.None, Keys.F5);
                    break;
                case "F6":
                    HotKey.RegisterHotKey(this.PHandle, 105, HotKey.KeyModifiers.None, Keys.F6);
                    break;
                case "F7":
                    HotKey.RegisterHotKey(this.PHandle, 106, HotKey.KeyModifiers.None, Keys.F7);
                    break;
                case "F8":
                    HotKey.RegisterHotKey(this.PHandle, 107, HotKey.KeyModifiers.None, Keys.F8);
                    break;
                case "F9":
                    HotKey.RegisterHotKey(this.PHandle, 108, HotKey.KeyModifiers.None, Keys.F9);
                    break;
                case "F10":
                    HotKey.RegisterHotKey(this.PHandle, 109, HotKey.KeyModifiers.None, Keys.F10);
                    break;
                case "F11":
                    HotKey.RegisterHotKey(this.PHandle, 110, HotKey.KeyModifiers.None, Keys.F11);
                    break;
                case "F12":
                    HotKey.RegisterHotKey(this.PHandle, 111, HotKey.KeyModifiers.None, Keys.F12);
                    break;
            }
        }
        private void UnRegistHotKey()
        {
            for (int i = 100; i < 112; i++)
                HotKey.UnregisterHotKey(this.PHandle, i);
        }
    }
}
