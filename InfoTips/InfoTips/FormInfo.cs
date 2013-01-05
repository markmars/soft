using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace InfoTips
{
    public partial class FormInfo : Form
    {
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

        int count = 1;
        public FormInfo()
        {
            InitializeComponent();
        }
        public FormInfo(List<新闻信息> ls)
        {
            InitializeComponent();
            foreach (新闻信息 xw in ls)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = xw.type + "   " + xw.date + "   " + xw.title;
                lvi.Tag = xw.url;
                lvi.ForeColor = Color.Blue;
                listView1.Items.Add(lvi);
            }
        }
        private void TextInfo_Load(object sender, EventArgs e)
        {
            int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            this.Location = new Point(x, y);
            AnimateWindow(this.Handle, 500, AW_SLIDE | AW_ACTIVE | AW_VER_NEGATIVE);
        }
        private void TextInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            //AnimateWindow(this.Handle, 500, AW_CENTER | AW_HIDE);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count >= Convert.ToUInt32(this.Tag))
                this.Close();
            count++;
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.FocusedItem == null)
                return;
            FormBroswer info = new FormBroswer(listView1.FocusedItem.Tag.ToString(), listView1.FocusedItem.Text.Split(new string[] { "   " }, StringSplitOptions.None)[2]);
            info.Show();
        }
    }
}
