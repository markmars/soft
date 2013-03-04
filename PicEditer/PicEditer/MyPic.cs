using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PicEditer
{
    class MyPic
    {
        Bitmap m_Bitmap;
        public MyPic(Bitmap bitMap)
        {
            m_Bitmap = bitMap;
        }
        /// <summary>
        /// 提取灰度法
        /// 为了将位图的颜色设置为灰度或其他颜色，需要使用GetPixel来读取当前像素的颜色--->计算灰度值--->使用SetPixel应用新的颜色
        /// f(i,j)=(curColor.R+curColor.G+curColor.B)/3
        /// ret = (int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
        /// </summary>
        public Bitmap 处理灰度(float fR, float fG, float fB)
        {
            if (m_Bitmap == null)
                return null;
            Bitmap bm = m_Bitmap;
            Color curColor;
            int ret;

            for (int i = 0; i < bm.Width; i++)//二维图像数组循环
            {
                for (int j = 0; j < bm.Height; j++)
                {
                    //读取当前像素的RGB颜色值
                    curColor = bm.GetPixel(i, j);
                    //利用公式计算灰度值（加权平均法）
                    ret = (int)(curColor.R * fR + curColor.G * fG + curColor.B * fB);
                    //设置该点像素的灰度值，R=G=B=ret
                    bm.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                }
            }
            return bm;
        }
        public Bitmap 处理大小(Bitmap bitMap)
        {
            //生成80*100的缩略图
            Image mythumbnail = bitMap.GetThumbnailImage(80, 100, null, IntPtr.Zero);

            return (Bitmap)mythumbnail;
        }
        /// <summary>
        /// 像素法
        /// </summary>
        /// <param name="curBitmap"></param>
        private void PixelFun(Bitmap curBitmap)
        {
            int width = curBitmap.Width;
            int height = curBitmap.Height;
            for (int i = 0; i < width; i++) //这里如果用i<curBitmap.Width做循环对性能有影响
            {
                for (int j = 0; j < height; j++)
                {
                    Color curColor = curBitmap.GetPixel(i, j);
                    int ret = (int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
                    curBitmap.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                }
            }
        }

        /// <summary>
        /// 内存拷贝法
        /// </summary>
        /// <param name="curBitmap"></param>
        private unsafe void MemoryCopy(Bitmap curBitmap)
        {
            int width = curBitmap.Width;
            int height = curBitmap.Height;

            Rectangle rect = new Rectangle(0, 0, curBitmap.Width, curBitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = curBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);//curBitmap.PixelFormat

            IntPtr ptr = bmpData.Scan0;
            int bytesCount = bmpData.Stride * bmpData.Height;
            byte[] arrDst = new byte[bytesCount];
            Marshal.Copy(ptr, arrDst, 0, bytesCount);

            for (int i = 0; i < bytesCount; i += 3)
            {
                byte colorTemp = (byte)(arrDst[i + 2] * 0.299 + arrDst[i + 1] * 0.587 + arrDst[i] * 0.114);
                arrDst[i] = arrDst[i + 1] = arrDst[i + 2] = (byte)colorTemp;

            }
            Marshal.Copy(arrDst, 0, ptr, bytesCount);
            curBitmap.UnlockBits(bmpData);
        }

        /// <summary>
        /// 指针法
        /// </summary>
        /// <param name="curBitmap"></param>
        private unsafe void PointerFun(Bitmap curBitmap)
        {
            int width = curBitmap.Width;
            int height = curBitmap.Height;

            Rectangle rect = new Rectangle(0, 0, curBitmap.Width, curBitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = curBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);//curBitmap.PixelFormat
            byte temp = 0;
            int w = bmpData.Width;
            int h = bmpData.Height;
            byte* ptr = (byte*)(bmpData.Scan0);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);
                    ptr[0] = ptr[1] = ptr[2] = temp;
                    ptr += 3; //Format24bppRgb格式每个像素占3字节
                }
                ptr += bmpData.Stride - bmpData.Width * 3;//每行读取到最后“有用”数据时，跳过未使用空间XX
            }
            curBitmap.UnlockBits(bmpData);
        }
    }
}
