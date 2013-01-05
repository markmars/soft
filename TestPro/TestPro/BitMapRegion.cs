using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TestPro
{
    public class BitmapRegion
    {
        public BitmapRegion()
        { }

        public static void CreateControlRegion(Control control, Bitmap bitmap)
        {
            //如果控件或者图象为空  
            if (control == null || bitmap == null)
            {
                return;
            }
            //将控件设置成图象一样的尺寸  
            control.Width = bitmap.Width;
            control.Height = bitmap.Height;
            //如果处理的是一个窗体对象  
            if (control is System.Windows.Forms.Form)
            {
                Form form = control as Form;//强制转换为Form实例  
                //将窗体的尺寸设置成比图片稍微大一点，这样就不用显示窗体边框  
                form.Width += 15;
                form.Height += 35;
                //设置窗体无边框  
                form.FormBorderStyle = FormBorderStyle.None;
                //设置窗体背景  
                form.BackgroundImage = bitmap;
                //根据图片计算路径  
                GraphicsPath graphicsPath = CalculateControlGraphicsPath(bitmap);
                //应用区域  
                form.Region = new Region(graphicsPath);
            }
            //如果处理的是一个按钮对象  
            else if (control is System.Windows.Forms.Button)
            {
                Button button = control as Button;//强制转换为Button实例  
                //不显示文字  
                button.Text = "";
                //当鼠标处在上方时更改光标状态  
                button.Cursor = Cursors.Hand;
                //设置背景图片  
                button.BackgroundImage = bitmap;
                //根据图片计算路径  
                GraphicsPath graphicsPath = CalculateControlGraphicsPath(bitmap);
                //应用区域  
                button.Region = new Region(graphicsPath);
            }

        }
        /// <summary></summary>  
        /// 通过逼近的方式扫描图片的轮廓  
        ///   
        /// <param name="bitmap">要扫描的图片  
        /// <returns></returns>  
        private static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            //将图片的(0,0)处的颜色定义为透明色  
            Color transparentColor = bitmap.GetPixel(0, 0);
            //存储发现的第一个不透明的象素的列值（即x坐标），这个值将定义我们扫描不透明区域的边缘  
            int opaquePixelX = 0;
            //从纵向开始  
            for (int y = 0; y < bitmap.Height; y++)
            {
                opaquePixelX = 0;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    if (bitmap.GetPixel(x, y) != transparentColor)
                    {
                        //标记不透明象素的位置  
                        opaquePixelX = x;
                        //记录当前位置  
                        int nextX = x;
                        for (nextX = opaquePixelX; nextX < bitmap.Width; nextX++)
                        {
                            if (bitmap.GetPixel(nextX, y) == transparentColor)
                            {
                                break;
                            }
                        }

                        graphicsPath.AddRectangle(new Rectangle(opaquePixelX, y, nextX - opaquePixelX, 1));
                        x = nextX;
                    }
                }
            }
            return graphicsPath;
        }
    }
}