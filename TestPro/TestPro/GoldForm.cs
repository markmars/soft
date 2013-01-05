using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestPro
{
    public partial class GoldForm : Form
    {
        private int currentX;//图片的当前横坐标  
        private int currentY;//图片的当前纵坐标  
        private int screenHeight;//屏幕高度  
        private int screenWidth;//屏幕宽度  
        private int counter;//图片数量  
        private int increment;//移动增量  
        private int interval;//移动时间间隔  

        private Bitmap bmpFlake = Properties.Resources.snow;
        /// <summary></summary>  
        /// 构造函数  
        /// <param name="interval">移动间隔  
        /// <param name="currentX">飘动窗体的横坐标  
        public GoldForm(int interval, int currentX)
        {
            this.interval = interval + 10;
            this.currentX = currentX;
            InitializeComponent();

            BitmapRegion.CreateControlRegion(this, bmpFlake);
        }
        private void GoldForm_Load_1(object sender, EventArgs e)
        {
            //获取屏幕的工作区域，不包括状态栏  
            Rectangle rectangleWorkArea = Screen.PrimaryScreen.WorkingArea;
            screenHeight = rectangleWorkArea.Height;
            screenWidth = rectangleWorkArea.Width;
            timer1.Interval = interval;//设置timer的间隔  
            this.Left = currentX;//设置窗体的起始横坐标  
            timer1.Start();//运行timer  
        } 
        //timer的事件  
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            currentY += 5;
            counter++;

            Random random = new Random();
            if (counter == 15)
            {
                if ((random.Next(10) - 5) > 0)
                {
                    increment = 1;
                }
                else
                {
                    increment = -1;
                }
                counter = 0;
            }

            currentX += increment;
            if (currentY > screenHeight)
            {
                currentY = 0;
                currentX = random.Next(screenWidth);
                interval = random.Next(50, 100);
            }
            //设置窗体位置，相当于移动窗体  
            this.Location = new Point(currentX, currentY);

            timer1.Interval = interval;
            timer1.Start();
        }
    }
}
