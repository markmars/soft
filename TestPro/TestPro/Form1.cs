using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TestPro
{
    public partial class Form1 : Form
    {
        #region 窗体边框暗影成效变量声明
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        #endregion
        #region 动画显示
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        public const Int32 AW_HOR_POSITIVE = 0x00000001; //自左向右显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;//自右向左显示窗口。当使用了 AW_CENTER 标志时该标志被忽略
        public const Int32 AW_VER_POSITIVE = 0x00000004; //自顶向下显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_VER_NEGATIVE = 0x00000008;//自下向上显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_CENTER = 0x00000010;//若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展
        public const Int32 AW_HIDE = 0x00010000;//隐藏窗口，缺省则显示窗口
        public const Int32 AW_ACTIVATE = 0x00020000;//激活窗口。在使用了AW_HIDE标志后不要使用这个标志
        public const Int32 AW_SLIDE = 0x00040000;//使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        public const Int32 AW_BLEND = 0x00080000; //使用淡入效果。只有当hWnd为顶层窗口的时候才可以使用此标志
        #endregion
        public Form1()
        {
            InitializeComponent();
            //AnimateWindow(this.Handle, 500, AW_SLIDE + AW_VER_NEGATIVE);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //AnimateWindow(this.Handle, 300, AW_SLIDE + AW_VER_NEGATIVE + AW_HIDE);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            GoldForm gf = new GoldForm(1, 0);
            gf.Show();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ColorChange();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ColorChange();
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            ColorChange();
        }

        private void ColorChange()
        {
            this.BackColor = Color.FromArgb(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }
        public static string MD5ForPHP(string s)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider HashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return BitConverter.ToString(HashMD5.ComputeHash(Encoding.UTF8.GetBytes(s))).Replace("-", "").ToLower();
        }
        public static string GetUCenterPassword(string Password, string Salt)
        {
            return MD5ForPHP(MD5ForPHP(Password) + Salt);
        }
    }
}
