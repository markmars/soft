using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace InfoTips
{
    public partial class FormInfo : DevExpress.XtraEditors.XtraForm
    {
        #region 窗体效果
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        //下面是可用的常量，按照不合的动画结果声明本身须要的
        private const int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口，该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口，该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口，该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口，该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记该标记
        private const int AW_HIDE = 0x10000;//隐蔽窗口
        private const int AW_ACTIVE = 0x20000;//激活窗口，在应用了AW_HIDE标记后不要应用这个标记
        private const int AW_CENTER = 0x0010;//若应用了AW_HIDE标记，则使窗口向内重叠；不然向外扩大
        private const int AW_SLIDE = 0x40000;//应用滑动类型动画结果，默认为迁移转变动画类型，当应用AW_CENTER标记时，这个标记就被忽视
        private const int AW_BLEND = 0x80000;//应用淡入淡出结果
        #endregion
        List<新闻信息> m_ls;
        int m_index = 0;
        int nCount = 0;
        public FormInfo()
        {
            InitializeComponent();
        }
        public FormInfo(List<新闻信息> ls)
        {
            InitializeComponent();
            m_ls = ls;
            ShowInfo();
            CheckReadOnly();
        }
        private void TextInfo_Load(object sender, EventArgs e)
        {
            int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            this.Location = new Point(x, y);
            AnimateWindow(this.Handle, 1000, AW_SLIDE | AW_ACTIVE | AW_VER_NEGATIVE);
        }
        private void TextInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnimateWindow(this.Handle, 500, AW_CENTER | AW_HIDE);
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void myLabel上一页_Click(object sender, EventArgs e)
        {
            m_index--;
            ShowInfo();
            CheckReadOnly();
        }
        private void myLabel下一页_Click(object sender, EventArgs e)
        {
            m_index++;
            ShowInfo();
            CheckReadOnly();
        }
        private void label标题_Click(object sender, EventArgs e)
        {
            Process.Start(m_ls[m_index].url);
        }
        private void ShowInfo()
        {
            label分类.Text = m_ls[m_index].type;
            label时间.Text = m_ls[m_index].date;
            label标题.Text = m_ls[m_index].title;

            myLabel页面.Text = m_index + 1 + "/" + m_ls.Count;
        }
        private void CheckReadOnly()
        {
            if (m_index == 0)
                myLabel下一页.Enabled = true;
            else
                myLabel下一页.Enabled = false;


            if (m_index == m_ls.Count - 1)
                myLabel上一页.Enabled = true;
            else
                myLabel上一页.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nCount == 10)
                this.Close();
            nCount++;
        }
    }
}
