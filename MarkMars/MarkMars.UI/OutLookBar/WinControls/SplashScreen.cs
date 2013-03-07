// SplashScreen.cs - Version 1.0
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;

namespace MarkMars.UI.OutLookBar.WinControls
{
    public sealed class SplashScreen : Control
    {
        #region private static class members
        private static SplashScreen m_Instance = null;
        private static object m_LockObject = new object();
        public static int SplashScreenStatus = 0;
        #endregion

        #region Interop definitions
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static IntPtr SetParent(IntPtr hChild, IntPtr hParent);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const int WS_EX_TOPMOST = unchecked((int)0x00000008L);
        private const int WS_EX_TOOLWINDOW = unchecked((int)0x00000080);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const int HWND_TOPMOST = -1;
        #endregion

        #region private instance members
        private string m_strTitleString;
        private string m_strCommentaryString;
        private StringFormat m_stringFormat;
        private int m_nTextOffsetY;
        private int m_nTextOffsetX;
        private int m_nLeading;
        #endregion

        #region construction and disposal
        #region Comments
        /// <summary>
        /// A control for displaying a topmost top-level window
        /// </summary>
        #endregion
        private SplashScreen()
        {
            TitleString = string.Empty;
            CommentaryString = string.Empty;

            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            m_stringFormat = new StringFormat();
            m_stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
            m_stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            m_nTextOffsetY = 0;
            m_nTextOffsetX = 45;
            m_nLeading = 6;

            Visible = false;
            Width = 420;
            Height = 320;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion

        #region protected overrides
        protected override void OnHandleCreated(EventArgs e)
        {
            if (Handle != IntPtr.Zero)
            {
                IntPtr hWndDeskTop = GetDesktopWindow();
                SetParent(Handle, hWndDeskTop);
            }

            base.OnHandleCreated(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Bounds.Width > 0 && Bounds.Height > 0 && Visible)
            {
                try
                {
                    Rectangle rect = new Rectangle(0, 0, Bounds.Width, Bounds.Height);
                    Graphics g = e.Graphics;
                    g.SetClip(e.ClipRectangle);
                    if (BackgroundImage == null)
                    {
                        SolidBrush solidBrush = new SolidBrush(BackColor);
                        g.FillRectangle(solidBrush, rect);
                        solidBrush.Dispose();
                    }
                    else
                    {
                        g.DrawImage(BackgroundImage, rect, 0, 0, BackgroundImage.Width, BackgroundImage.Height, GraphicsUnit.Pixel);
                    }
                }
                catch (Exception exception)
                {
                    System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame(true);
                    Console.WriteLine("\nException: {0}, \n\t{1}, \n\t{2}, \n\t{3}\n", this.GetType().ToString(), stackFrame.GetMethod().ToString(), stackFrame.GetFileLineNumber(), exception.Message);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Bounds.Width > 0 && Bounds.Height > 0 && Visible)
            {
                try
                {
                    Graphics g = e.Graphics;
                    g.SetClip(e.ClipRectangle);
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    float nClientHeight = ClientRectangle.Height;
                    //start the text two thirds down:
                    m_nTextOffsetY = Convert.ToInt32(Math.Ceiling(((nClientHeight / 4) * 2))) + m_nLeading;

                    if (TitleString != string.Empty)
                    {
                        Font fontTitle = new Font(Font, FontStyle.Bold);
                        SizeF sizeF = g.MeasureString(TitleString, fontTitle, ClientRectangle.Width, m_stringFormat);
                        m_nTextOffsetY += Convert.ToInt32(Math.Ceiling(sizeF.Height));
                        RectangleF rectangleF = new RectangleF(ClientRectangle.Left + m_nTextOffsetX, ClientRectangle.Top + m_nTextOffsetY, sizeF.Width, sizeF.Height);
                        SolidBrush brushFont = new SolidBrush(ForeColor);
                        g.DrawString(TitleString, fontTitle, brushFont, rectangleF, m_stringFormat);
                        brushFont.Dispose();
                        fontTitle.Dispose();

                        m_nTextOffsetY += m_nLeading;
                    }

                    if (CommentaryString != string.Empty)
                    {
                        SizeF sizeF = g.MeasureString(CommentaryString, Font, ClientRectangle.Width, m_stringFormat);
                        m_nTextOffsetY += Convert.ToInt32(Math.Ceiling(sizeF.Height));
                        RectangleF rectangleF = new RectangleF(ClientRectangle.Left + m_nTextOffsetX, ClientRectangle.Top + m_nTextOffsetY, sizeF.Width, sizeF.Height);
                        SolidBrush brushFont = new SolidBrush(ForeColor);
                        g.DrawString(CommentaryString, Font, brushFont, rectangleF, m_stringFormat);
                        brushFont.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame(true);
                    Console.WriteLine("\nException: {0}, \n\t{1}, \n\t{2}, \n\t{3}\n", this.GetType().ToString(), stackFrame.GetMethod().ToString(), stackFrame.GetFileLineNumber(), exception.Message);
                }
            }
        }

        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);

            if (BackgroundImage != null)
            {
                Width = BackgroundImage.Width;
                Height = BackgroundImage.Height;
                Reorient();
            }

            Refresh();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOOLWINDOW | WS_EX_TOPMOST;
                cp.Parent = IntPtr.Zero;
                return cp;
            }
        }
        #endregion

        #region private methods

        private void Reorient()
        {
            Rectangle screenArea = SystemInformation.WorkingArea;
            int nX = (screenArea.Width - Width) / 2;
            int nY = (screenArea.Height - Height) / 2;
            Location = new Point(nX, nY);
        }

        #endregion

        #region public static methods

        #region Comments
        /// <summary>
        /// Begins the splash screen display
        /// </summary>
        /// <remarks>
        /// Always ensure that a call to EndDisplay will follow a call to this method
        /// </remarks>
        #endregion
        public static void BeginDisplay()
        {
            Instance.Reorient();
            Instance.Visible = true;
            SplashScreenStatus = 1;
            if (!Instance.Created)
            {
                Instance.CreateControl();
            }
            SetWindowPos(Instance.Handle, (System.IntPtr)HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            Instance.Refresh();
        }

        #region Comments
        /// <summary>
        /// Ends the splash screen display
        /// </summary>
        /// <remarks>
        /// Always ensure that a call to this method follows a call to BeginDisplay
        /// </remarks>
        #endregion
        public static void EndDisplay()
        {
            Instance.TitleString = "";
            Instance.CommentaryString = "";
            SplashScreenStatus = 0;
            Instance.Visible = false;
        }

        #region Comments
        /// <summary>
        /// Sets the title text displayed by the splash screen
        /// </summary>
        #endregion
        public static void SetTitleString(string title)
        {
            Instance.TitleString = title;
        }

        #region Comments
        /// <summary>
        /// Sets the commentary text displayed by the splash screen
        /// </summary>
        #endregion
        public static void SetCommentaryString(string commentary)
        {
            Instance.CommentaryString = commentary;
        }

        #region Comments
        /// <summary>
        /// Sets an image to be displayed in the background by the splash screen
        /// </summary>
        #endregion
        public static void SetBackgroundImage(Image image)
        {
            Instance.BackgroundImage = image;
        }

        #endregion

        #region public static properties
        #region Comments
        /// <summary>
        /// Gets the current instance of the splash screen
        /// </summary>
        #endregion
        public static SplashScreen Instance
        {
            get
            {
                lock (m_LockObject)
                {
                    if (m_Instance == null || m_Instance.IsDisposed)
                    {
                        m_Instance = new SplashScreen();
                    }
                    return m_Instance;
                }
            }
        }
        #endregion

        #region private properties

        private string CommentaryString
        {
            get
            {
                return m_strCommentaryString;
            }
            set
            {
                m_strCommentaryString = value;
                Refresh();
            }
        }

        private string TitleString
        {
            get
            {
                return m_strTitleString;
            }
            set
            {
                m_strTitleString = value;
                Refresh();
            }
        }
        #endregion
    }
}
