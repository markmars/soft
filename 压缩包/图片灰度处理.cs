using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 图片灰度处理
{
    //    为了加快图像的处理速度，在图像处理算法中，往往需要把彩色图像转换为灰度图像

    //24位彩色图像每个像素用3个字节表示，每个字节对应着R、G、B分量的亮度。当RGB分量值不同时，表现为彩色图像，当RGB分量值相同时，表现为灰度图像。

    //求灰度值的方法：

    //    平均值法
    //    将彩色图像中的三分量亮度求平均得到一个灰度图。
    //    f(i,j)=(R(i,j)+G(i,j)+B(i,j)) /3
    //    加权平均法
    //    根据重要性及其它指标，将三个分量以不同的权值进行加权平均。由于人眼对绿色的敏感最高，对蓝色敏感最低，因此，按下式对RGB三分量进行加权平均能得到较合理的灰度图像。
    //    f(i,j)=0.30R(i,j)+0.59G(i,j)+0.11B(i,j))

    class 图片灰度处理
    {
        string curFileName;
        Bitmap curBitmap;
        public 图片灰度处理()
        {
            open_Click(null, null);
            pixel_Click(null, null);
            save_Click(null, null);
        }
        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnDlg = new OpenFileDialog();//创建OpenFileDialog对象
            //为图像选择一个筛选器
            opnDlg.Filter = "所有图像文件 | *.bmp; *.pcx; *.png; *.jpg; *.gif;" +
                "*.tif; *.ico; *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf|" +
                "位图( *.bmp; *.jpg; *.png;...) | *.bmp; *.pcx; *.png; *.jpg; *.gif; *.tif; *.ico|" +
                "矢量图( *.wmf; *.eps; *.emf;...) | *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf";
            opnDlg.Title = "打开图像文件";
            if (opnDlg.ShowDialog() == DialogResult.OK)
            {
                curFileName = opnDlg.FileName;
                try
                {
                    curBitmap = (Bitmap)Image.FromFile(curFileName);//使用Image.FromFile创建图像对象
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }

            ////使控件的整个图面无效并导致重绘控件
            //Invalidate();//对窗体进行重新绘制,这将强制执行Paint事件处理程序
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (curBitmap == null)
            {
                return;
            }
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Title = "保存为";
            saveDlg.OverwritePrompt = true;
            saveDlg.Filter =
                "BMP文件 (*.bmp) | *.bmp|" +
                "Gif文件 (*.gif) | *.gif|" +
                "JPEG文件 (*.jpg) | *.jpg|" +
                "PNG文件 (*.png) | *.png";
            saveDlg.ShowHelp = true;
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveDlg.FileName;
                string strFilExtn = fileName.Remove(0, fileName.Length - 3);
                switch (strFilExtn)
                {
                    case "bmp":
                        curBitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "jpg":
                        curBitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "gif":
                        curBitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "tif":
                        curBitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                    case "png":
                        curBitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 当一个应用程序需要进行绘制时，他必须通过Graphics对象来执行绘制操作
        /// 获取Graphics对象的方法有好几种，这里我们使用窗体Paint事件的PaintEventArgs属性来获取一个与窗体相关联的Graphics对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;//获取Graphics对象
            if (curBitmap != null)
            {
                g.DrawImage(curBitmap, 160, 20, curBitmap.Width, curBitmap.Height);//使用DrawImage方法绘制图像
            }
        }
        /// <summary>
        /// 提取灰度法
        /// 为了将位图的颜色设置为灰度或其他颜色，需要使用GetPixel来读取当前像素的颜色--->计算灰度值--->使用SetPixel应用新的颜色
        /// </summary>
        private void pixel_Click(object sender, EventArgs e)
        {
            if (curBitmap != null)
            {
                Color curColor;
                int ret;
                //二维图像数组循环  
                for (int i = 0; i < curBitmap.Width; i++)
                {
                    for (int j = 0; j < curBitmap.Height; j++)
                    {
                        //读取当前像素的RGB颜色值
                        curColor = curBitmap.GetPixel(i, j);
                        //利用公式计算灰度值（加权平均法）
                        ret = (int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
                        //设置该点像素的灰度值，R=G=B=ret
                        curBitmap.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                    }
                }
                //使控件的整个图面无效并导致重绘控件
                Invalidate();//对窗体进行重新绘制,这将强制执行Paint事件处理程序
            }
        }
    }
}
