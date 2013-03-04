using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WindowsFormsApplication2
{
    public class MyPanel : Panel
    {

        #region 定义字段
        /// <summary>
        /// 透明度
        /// </summary>
        private int _alpha = 100;
        /// <summary>
        /// 背景颜色
        /// </summary>
        private Color _bkcolor = DefaultBackColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        private Color _bdcolor = DefaultBackColor;

        /// <summary>
        ///  
        /// </summary>
        private const int Band = 5;
        /// <summary>
        /// 最小宽度
        /// </summary>
        private const int MinWidth = 10;
        /// <summary>
        /// 鼠标状态
        /// </summary>
        private EnumMousePointPosition m_MousePointPosition;
        /// <summary>
        /// 鼠标按下点
        /// </summary>
        private Point DownPoint;
        /// <summary>
        /// 鼠标移动点
        /// </summary>
        private Point CurrPoint;
        #endregion

        #region 定义属性
        /// <summary>
        /// 透明度
        /// </summary>
        public int Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
                this.Invalidate();
                this.Update();
            }
        }
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color bkColor
        {
            get
            {
                return _bkcolor;
            }
            set
            {
                _bkcolor = value;
                this.Invalidate();
                this.Update();
            }
        }
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color bdColor
        {
            get
            {
                return _bdcolor;
            }
            set
            {
                _bdcolor = value;
                this.Invalidate();
                this.Update();
            }
        }
        #endregion

        #region 构造函数

        public MyPanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(MyMouseDown);
            this.MouseLeave += new System.EventHandler(MyMouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(MyMouseMove);
        }

        /// <summary>
        /// 背景透明panel
        /// </summary>
        /// <param name="alpha">透明度</param>
        /// <param name="back">背景颜色</param>
        /// <param name="border">边框颜色</param>
        public MyPanel(int alpha, Color back, Color border)
        {
            this._alpha = alpha;
            this._bkcolor = back;
            this._bdcolor = border;

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(MyMouseDown);
            this.MouseLeave += new System.EventHandler(MyMouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(MyMouseMove);
        }

        #endregion

        #region 私有函数

        #region 根据鼠标位置获取鼠标状态
        /// <summary>
        /// 根据鼠标位置获取鼠标状态
        /// </summary>
        /// <param name="size"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private EnumMousePointPosition MousePointPosition(Size size, System.Windows.Forms.MouseEventArgs e)
        {

            if ((e.X >= -1 * Band) | (e.X <= size.Width) | (e.Y >= -1 * Band) | (e.Y <= size.Height))
            {
                if (e.X < Band)
                {
                    if (e.Y >= Band && e.Y <= -1 * Band + size.Height)
                    {
                        return EnumMousePointPosition.MouseSizeLeft;
                    }
                }
                else
                {
                    if (e.X > -1 * Band + size.Width)
                    {
                        if (e.Y >= Band && e.Y <= -1 * Band + size.Height)
                        {
                            return EnumMousePointPosition.MouseSizeRight;
                        }
                    }
                    else
                    {
                        if (e.Y >= Band && e.Y <= -1 * Band + size.Height)
                        {
                            return EnumMousePointPosition.MouseDrag;
                        }
                    }
                }
            }

            return EnumMousePointPosition.MouseSizeNone;
        }
        #endregion


        #endregion

        #region 重载函数

        #region 开启 WS_EX_TRANSPARENT,使控件支持透明
        /// <summary>
        /// 开启 WS_EX_TRANSPARENT,使控件支持透明
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // 开启 WS_EX_TRANSPARENT,使控件支持透明
                return cp;
            }
        }
        #endregion

        #region 不绘制背景
        protected override void OnPaintBackground(PaintEventArgs paintg)
        {
            //不绘制背景
        }
        #endregion

        #region 绘制图形
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics bufg = Graphics.FromImage(bmp);
            bufg.DrawRectangle(new Pen(Color.FromArgb(this._alpha, this._bdcolor)), new Rectangle(0, 0, this.Size.Width, this.Size.Height));
            bufg.FillRectangle(new SolidBrush(Color.FromArgb(this._alpha, this._bkcolor)), 0, 0, this.Size.Width, this.Size.Height); //绘制背景
            g.DrawImage(bmp, 0, 0);
            bufg.Dispose();
            bmp.Dispose();
        }

        #endregion

        #endregion

        #region 定义事件
        #region 鼠标按下事件
        private void MyMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DownPoint.X = e.X;
            DownPoint.Y = e.Y;
            CurrPoint.X = e.X;
            CurrPoint.Y = e.Y;
        }
        #endregion

        #region 鼠标离开事件
        /// <summary>
        /// 鼠标离开事件需要改进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyMouseLeave(object sender, EventArgs e)
        {
            m_MousePointPosition = EnumMousePointPosition.MouseSizeNone;
            this.Cursor = Cursors.Arrow;
        }
        #endregion

        #region 鼠标移动事件
        private void MyMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Control lCtrl = (sender as Control);

            if (e.Button == MouseButtons.Left)
            {
                switch (m_MousePointPosition)
                {
                    case EnumMousePointPosition.MouseDrag:
                        lCtrl.Left = lCtrl.Left + e.X - DownPoint.X;
                        break;
                    case EnumMousePointPosition.MouseSizeRight:
                        lCtrl.Width = lCtrl.Width + e.X - CurrPoint.X;
                        CurrPoint.X = e.X;
                        CurrPoint.Y = e.Y; //'记录光标拖动的当前点   
                        break;
                    case EnumMousePointPosition.MouseSizeLeft:
                        lCtrl.Left = lCtrl.Left + e.X - DownPoint.X;
                        lCtrl.Width = lCtrl.Width - (e.X - DownPoint.X);
                        break;
                    default:
                        break;
                }
                if (lCtrl.Width < MinWidth) lCtrl.Width = MinWidth;
                if (lCtrl.Width > this.Width) lCtrl.Width = this.Width;
            }
            else
            {
                m_MousePointPosition = MousePointPosition(lCtrl.Size, e); //'判断光标的位置状态   
                switch (m_MousePointPosition) //'改变光标   
                {
                    case EnumMousePointPosition.MouseSizeNone:
                        this.Cursor = Cursors.Arrow; //'箭头   
                        break;
                    case EnumMousePointPosition.MouseDrag:
                        this.Cursor = Cursors.SizeAll; //'四方向   
                        break;
                    case EnumMousePointPosition.MouseSizeLeft:
                        this.Cursor = Cursors.SizeWE; //'东西   
                        break;
                    case EnumMousePointPosition.MouseSizeRight:
                        this.Cursor = Cursors.SizeWE; //'东西   
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 光标状态
    /// </summary>
    enum EnumMousePointPosition
    {
        MouseSizeNone = 0, //'无   
        MouseSizeRight = 1, //'拉伸右边框   
        MouseSizeLeft = 2, //'拉伸左边框   
        MouseDrag = 9 // '鼠标拖动   
    }
}