using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
namespace PicEditer
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class PicManage1
    {
        /// <summary>
        /// 将Image转化为byte数组
        /// </summary>
        /// <param name="imageIn">图片</param>
        /// <returns></returns>
        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            if (imageIn == null)
                return null;
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        /// <summary>
        /// 将byte数组转化为Image
        /// </summary>
        /// <param name="byteArrayIn">图片数组</param>
        /// <returns></returns>
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0)
                return null;
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        /// <summary>
        /// 将图片缩放为指定大小
        /// </summary>
        /// <param name="originalImagePath">源图片路径</param>
        /// <param name="thumbnailPath">缩放后图片存放路径</param>
        /// <param name="width">缩放图片宽</param>
        /// <param name="height">缩放图片高</param>
        /// <param name="zoomtype">缩放类型</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ZoomType zoomtype)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (zoomtype)
            {
                case ZoomType.HW://指定高宽缩放（可能变形）               
                    break;
                case ZoomType.W://指定宽，高按比例                   
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ZoomType.H://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ZoomType.Cut://指定高宽裁减（不变形）               
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        /// <summary>
        /// 将图片缩放为指定大小
        /// </summary>
        /// <param name="originalImagePath">源图片路径</param>
        /// <param name="width">缩放图片宽</param>
        /// <param name="height">缩放图片高</param>
        /// <param name="mode">缩放类型</param>
        /// <returns></returns>
        public static Image RetrunImage(string originalImagePath, int width, int height, ZoomType zoomtype)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (zoomtype)
            {
                case ZoomType.HW://指定高宽缩放（可能变形）               
                    break;
                case ZoomType.W://指定宽，高按比例                   
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ZoomType.H://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ZoomType.Cut://指定高宽裁减（不变形）               
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);
            //g.Clear(ColorTranslator.FromHtml("#fffbd9"));
            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                return bitmap;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                // bitmap.Dispose();  一定不能有 不然返回图片出错
                g.Dispose();
            }
        }
        /// <summary>
        /// 以逆时针为方向对图像进行旋转
        /// </summary>
        /// <param name="b">位图流</param>
        /// <param name="angle">旋转角度[0,360](前台给的)</param>
        /// <returns></returns>
        public static Bitmap Rotate(Bitmap b, int angle)
        {
            angle = angle % 360; //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);
            //重至绘图的所有变换
            g.ResetTransform();
            g.Save();
            g.Dispose();
            //dsImage.Save("yuancd.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return dsImage;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ImgTitle"></param>
        /// <param name="dt"></param>
        /// <param name="optcol"></param>
        /// <param name="numcol"></param>
        /// <param name="path"></param>
        public void GetRectangleImage(string ImgTitle, DataTable dt, string optcol, string numcol, string path)
        {
            int ImageWidth = dt.Rows.Count * 60 + 120;
            if (ImageWidth < 200)
            {
                ImageWidth = ImageWidth + 150;
            }
            Bitmap objBitMap = new Bitmap(ImageWidth, 240);
            Graphics objGraphics;
            objGraphics = Graphics.FromImage(objBitMap);
            objGraphics.Clear(Color.White);
            objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            objGraphics.DrawString(ImgTitle, new Font("宋体", 10), Brushes.DarkBlue, new PointF(90, 5));

            string maxHight = dt.Select("", "" + numcol + " DESC", DataViewRowState.CurrentRows)[0][numcol].ToString();
            int rate = 0;
            if (maxHight.Length < 2) //like <10
            {
                rate = 1;
            }
            else if (maxHight.Length < 3)// <100
            {
                rate = 10;
            }
            else if (maxHight.Length < 4)
            {
                rate = 100;
            }
            else if (maxHight.Length < 5)
            {
                rate = 1000;
            }
            else if (maxHight.Length < 6)
            {
                rate = 10000;
            }
            else if (maxHight.Length < 7)
            {
                rate = 100000;
            }
            else
            {
                rate = 1000000;
            }
            int i = 0; int curX = 0; int curY = 0;

            foreach (DataRow dr in dt.Rows)
            {
                i++;
                double num = Convert.ToInt32(dr[numcol]) / rate * 20;
                int numHeight = (int)Math.Round(num, 0);
                curX = i * 60 + 35;
                curY = 220 - numHeight;
                objGraphics.FillRectangle(new SolidBrush(GetColor(i)), curX, curY, 25, numHeight + 5);
                objGraphics.DrawRectangle(Pens.Black, curX, curY, 25, numHeight + 5);
                PointF descLeg = new PointF(curX, 230);
                string option = dr[optcol].ToString().Length > 8 ? dr[optcol].ToString().Substring(0, 8) : dr[optcol].ToString();
                objGraphics.DrawString(option, new Font("宋体", 7), Brushes.Black, descLeg);
                PointF numLeg = new PointF(curX, curY - 15);//add 10
                objGraphics.DrawString(dr[numcol].ToString(), new Font("Arial", 8), Brushes.Blue, numLeg);
            }

            ///x and y raw
            Point pfrom = new Point(50, 225);
            Point pto = new Point(ImageWidth, 225);
            objGraphics.DrawLine(Pens.Black, pfrom, pto);
            Point pvfrom = new Point(50, 0);
            Point pvto = new Point(50, 225);
            objGraphics.DrawLine(Pens.OrangeRed, pvfrom, pvto);
            ///step
            ///
            for (int j = 0; j < 11; j++)
            {
                Point ppfrom = new Point(50, 225 - j * 20);
                Point ppto = new Point(54, 225 - j * 20);
                objGraphics.DrawLine(Pens.DarkOrange, ppfrom, ppto);
                int stepNum = rate * j;
                objGraphics.DrawString(stepNum.ToString(), new Font("Arial", 8), Brushes.DarkBlue, new PointF(5, 220 - j * 20));
            }
            objBitMap.Save(path, ImageFormat.Jpeg);
            objGraphics.Dispose();
            objBitMap.Dispose();

        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="dt"></param>
        /// <param name="numcol"></param>
        /// <param name="optcol"></param>
        /// <param name="ratecol"></param>
        /// <param name="path"></param>
        public void GetPieImage(string Title, DataTable dt, string numcol, string optcol, string ratecol, string path)
        {
            int ImgHight = dt.Rows.Count * 20 + 220;
            Bitmap objBitMap = new Bitmap(650, ImgHight);
            Graphics objGraphics;
            objGraphics = Graphics.FromImage(objBitMap);
            objGraphics.Clear(Color.Transparent);
            objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            objGraphics.DrawString(Title, new Font("宋体", 10), Brushes.DarkBlue, new PointF(5, 5));
            PointF symbolLeg = new PointF(200, 50);
            PointF descLeg = new PointF(230, 48);

            float sglCurrentAngle = 0;
            float sglTotalAngle = 0;
            int k = 0;
            foreach (DataRow dr in dt.Rows)
            {
                k++;
                float ratio = float.Parse(dr[ratecol].ToString()) / 10;//100;
                sglCurrentAngle = ratio * 36;

                objGraphics.FillRectangle(new SolidBrush(GetColor(k)), symbolLeg.X, symbolLeg.Y, 20, 10);
                objGraphics.DrawRectangle(Pens.Black, symbolLeg.X, symbolLeg.Y, 20, 10);
                string opt = dr[optcol].ToString().Length > 18 ? dr[optcol].ToString().Substring(0, 18) : dr[optcol].ToString();
                objGraphics.DrawString(opt + " Count:" + dr[numcol] + " Ratio:" + dr[ratecol] + "%", new Font("宋体", 10), Brushes.Black, descLeg);
                symbolLeg.Y += 15;
                descLeg.Y += 15;

                objGraphics.FillPie(new SolidBrush(GetColor(k)), 20, 80, 150, 150, sglTotalAngle, sglCurrentAngle);
                objGraphics.DrawPie(Pens.Black, 20, 80, 150, 150, sglTotalAngle, sglCurrentAngle);
                sglTotalAngle += sglCurrentAngle;
            }
            objBitMap.Save(path, ImageFormat.Jpeg);
            objGraphics.Dispose();
            objBitMap.Dispose();

        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ImgTitle"></param>
        /// <param name="dt"></param>
        /// <param name="optColName"></param>
        /// <param name="numColName"></param>
        /// <param name="path"></param>
        public void GetLineImage(string ImgTitle, DataTable dt, string optColName, string numColName, string path)
        {
            int ImageWidth = dt.Rows.Count * 70 + 120;
            if (ImageWidth < 200)
            {
                ImageWidth = ImageWidth + 150;
            }
            Bitmap objBitMap = new Bitmap(ImageWidth, 240);
            Graphics objGraphics;
            objGraphics = Graphics.FromImage(objBitMap);
            objGraphics.Clear(Color.Transparent);
            objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            objGraphics.DrawString(ImgTitle, new Font("宋体", 10), Brushes.DarkBlue, new PointF(90, 5));

            string maxHight = dt.Select("", "" + numColName + " DESC", DataViewRowState.CurrentRows)[0][numColName].ToString();
            int rate = 0;
            if (maxHight.Length < 2) //like <10
            {
                rate = 1;
            }
            else if (maxHight.Length < 3)// <100
            {
                rate = 10;
            }
            else if (maxHight.Length < 4)
            {
                rate = 100;
            }
            else if (maxHight.Length < 5)
            {
                rate = 1000;
            }
            else if (maxHight.Length < 6)
            {
                rate = 10000;
            }
            else if (maxHight.Length < 7)
            {
                rate = 100000;
            }
            else
            {
                rate = 1000000;
            }

            int i = 0;
            int LineX = 50; int LineY = 225;
            int curLineX = 0; int curLineY = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (i > 20) break;
                double num = Convert.ToInt32(dr[numColName]) / rate * 20;
                int numHeight = (int)Math.Round(num, 0);
                curLineX = (i * 70) + 30;
                curLineY = 225 - numHeight;
                objGraphics.DrawLine(Pens.DarkBlue, new Point(LineX, LineY), new Point(curLineX, curLineY));
                LineX = curLineX;
                LineY = curLineY;
                PointF descLeg = new PointF(curLineX, 230);
                string option = dr[optColName].ToString().Length > 8 ? dr[optColName].ToString().Substring(0, 8) : dr[optColName].ToString();
                objGraphics.DrawString(option, new Font("宋体", 7), Brushes.Black, descLeg);

                PointF numLeg = new PointF(curLineX, 205 - numHeight);//add 10
                objGraphics.DrawString(dr[numColName].ToString(), new Font("Arial", 8), Brushes.Black, numLeg);
                objGraphics.DrawEllipse(new Pen(Brushes.Black), curLineX, 223, 2, 3);
                objGraphics.FillEllipse(Brushes.DarkBlue, curLineX, 223, 2, 3);
            }

            ///x and y raw
            Point pfrom = new Point(50, 225);
            Point pto = new Point(ImageWidth, 225);
            objGraphics.DrawLine(Pens.Black, pfrom, pto);
            Point pvfrom = new Point(50, 0);
            Point pvto = new Point(50, 225);
            objGraphics.DrawLine(Pens.DarkOrange, pvfrom, pvto);
            ///step
            ///
            for (int j = 0; j < 11; j++)
            {
                Point ppfrom = new Point(50, 225 - j * 20);
                Point ppto = new Point(54, 225 - j * 20);
                objGraphics.DrawLine(Pens.Orange, ppfrom, ppto);
                int stepNum = rate * j;
                objGraphics.DrawString(stepNum.ToString(), new Font("Arial", 8), Brushes.DarkBlue, new PointF(5, 220 - j * 20));
            }
            objBitMap.Save(path, ImageFormat.Jpeg);
            objGraphics.Dispose();
            objBitMap.Dispose();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        private Color GetColor(int itemIndex)
        {
            Color objColor;
            string lastNumeric = itemIndex.ToString();
            if (lastNumeric.Length > 1)
            {
                lastNumeric = lastNumeric.Substring(lastNumeric.Length - 1);
            }
            if (lastNumeric == "0")
            {
                objColor = Color.OrangeRed;
            }
            else if (lastNumeric == "1")
            {
                objColor = Color.LimeGreen;
            }
            else if (lastNumeric == "2")
            {
                objColor = Color.Orange;
            }
            else if (lastNumeric == "3")
            {
                objColor = Color.AliceBlue;
            }
            else if (lastNumeric == "4")
            {
                objColor = Color.Yellow;
            }
            else if (lastNumeric == "5")
            {
                objColor = Color.Red;
            }
            else if (lastNumeric == "6")
            {
                objColor = Color.SteelBlue; //ColorTranslator.FromHtml("");
            }
            else if (lastNumeric == "7")
            {
                objColor = Color.GreenYellow;
            }
            else if (lastNumeric == "8")
            {
                objColor = Color.Goldenrod;
            }
            else //if (itemIndex == 9)
            {
                objColor = Color.IndianRed;
            }
            return objColor;
        }


    }
    /// <summary>
    /// 缩放类型
    /// </summary>
    public enum ZoomType
    {
        /// <summary>
        /// 指定高宽缩放（可能变形） 
        /// </summary>
        HW,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H,
        /// <summary>
        /// 指定宽，高按比例
        /// </summary>
        W,
        /// <summary>
        /// 指定高宽裁减（不变形） 
        /// </summary>
        Cut
    }
}