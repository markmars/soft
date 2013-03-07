using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using MarkMars.UI.Properties;

namespace MarkMars.UI.TrueLorePanel
{
	/// <summary>
	/// Base class for the panel or xpanderpanel control. 
	/// </summary>
	/// <remarks>
	/// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
	/// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
	/// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
	/// PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
	/// REMAINS UNCHANGED.
	/// </remarks>
	/// <copyright>Copyright ?2006-2007 Uwe Eichkorn</copyright>
	public abstract class BasePanel : ScrollableControl, IPanel
	{
		#region Constants
		private const int m_iCaptionHeight = 25;
        public const int CaptionSpacing = 6;
        public const int RightImagePositionX = 21;
        public const int ImagePaddingTop = 5;
		#endregion

		#region FieldsPrivate
		private Font m_captionFont;
        private Rectangle m_imageRectangle;
        private Color m_captionForeColor;
		private bool m_bShowBorder;
        private Size m_imageSize;
        private ColorScheme m_eColorScheme;
		
        private Image m_image;
		private System.Drawing.Color m_colorCaptionGradientBegin = ProfessionalColors.OverflowButtonGradientBegin;
		private System.Drawing.Color m_colorCaptionGradientMiddle = ProfessionalColors.OverflowButtonGradientMiddle;
		private System.Drawing.Color m_colorCaptionGradientEnd = ProfessionalColors.OverflowButtonGradientEnd;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the style of the panel.
		/// </summary>
		[Description("Style of the panel")]
        [DefaultValue(0)]
		[Category("Appearance")]
		public abstract PanelStyle PanelStyle
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the image that is displayed on a Panels caption.
		/// </summary>
		[Description("Gets or sets the image that is displayed on a Panels caption.")]
		[Category("Appearance")]
		public Image Image
		{
			get { return this.m_image; }
			set
			{
                if (value != this.m_image)
                {
                    this.m_image = value;
                    this.Invalidate(this.CaptionRectangle);
                }
			}
		}
		/// <summary>
		/// Gets or sets the color schema which is used for the panel.
		/// </summary>
		[Description("ColorScheme of the Panel")]
		[DefaultValue(ColorScheme.Professional)]
		[Browsable(true)]
		[Category("Appearance")]
		public virtual ColorScheme ColorScheme
		{
			get { return this.m_eColorScheme; }
			set
			{
                if (value != this.m_eColorScheme)
                {
                    this.m_eColorScheme = value;
                    this.Invalidate();
                }
			}
		}
		/// <summary>
		/// Gets the height of the panels caption. 
		/// </summary>
		[Description("Height of caption."),
		DefaultValue(25),
		Category("Appearance")]
		public int CaptionHeight
		{
			get { return m_iCaptionHeight; }
		}
		/// <summary>
		/// Gets or sets the font of the text displayed on the caption.
		/// </summary>
		[Description("Gets or sets the font of the text displayed on the caption.")]
		[DefaultValue(typeof(Font),"Microsoft Sans Serif; 8,25pt; style=Bold")]
        [Category("Appearance")]
		public Font CaptionFont
		{
			get { return this.m_captionFont; }
            set
            {
                if (value != null)
                {
                    if (value != this.m_captionFont)
                    {
                        this.m_captionFont = value;
                        this.Invalidate(this.CaptionRectangle);
                    }
                }
            }
		}
		/// <summary>
		/// The foreground color of this component, which is used to display the caption text.
		/// </summary>
		[Description("The foreground color of this component, which is used to display the caption text.")]
		[DefaultValue(typeof(SystemColors), "System.Drawing.SystemColors.ActiveCaptionText")]
		[Category("Appearance")]
		public virtual Color CaptionForeColor
		{
			get { return this.m_captionForeColor; }
            set
            {
                if (value != this.m_captionForeColor)
                {
                    this.m_captionForeColor = value;
                    this.Invalidate(this.CaptionRectangle);
                }
            }
		}
		/// <summary>
		/// Gets or sets the starting color of the gradient used in the caption
		/// </summary>
		[Description("Gets or sets the starting color of the gradient used in the caption")]
		[Category("Appearance")]
		public Color ColorCaptionGradientBegin
		{
			get { return this.m_colorCaptionGradientBegin; }
			set
			{
                if (value != this.m_colorCaptionGradientBegin)
                {
                    this.m_colorCaptionGradientBegin = value;
                    this.Invalidate(this.CaptionRectangle);
                }
			}
		}
		/// <summary>
		/// Gets or sets the middle color of the gradient used in the caption
		/// </summary>
		[Description("Gets or sets the middle color of the gradient used in the caption")]
		[Category("Appearance")]
		public Color ColorCaptionGradientMiddle
		{
			get { return this.m_colorCaptionGradientMiddle; }
			set
			{
                if (value != this.m_colorCaptionGradientMiddle)
                {
                    this.m_colorCaptionGradientMiddle = value;
                    this.Invalidate(this.CaptionRectangle);
                }
			}
		}
		/// <summary>
		/// Gets or sets the end color of the gradient used in the caption
		/// </summary>
		[Description("Gets or sets the end color of the gradient used in the caption")]
		[Category("Appearance")]
		public Color ColorCaptionGradientEnd
		{
			get { return this.m_colorCaptionGradientEnd; }
			set
			{
                if (value != this.m_colorCaptionGradientEnd)
                {
                    this.m_colorCaptionGradientEnd = value;
                    this.Invalidate(this.CaptionRectangle);
                }
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether the control shows a border
		/// </summary>
		[Description("Gets or sets a value indicating whether the control shows a border")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(true)]
		[Browsable(false)]
		[Category("Appearance")]
		public virtual bool ShowBorder
		{
			get { return this.m_bShowBorder; }
			set
			{
                if (value != this.m_bShowBorder)
                {
                    this.m_bShowBorder = value;
                    this.Invalidate();
                }
			}
		}
        internal Size ImageSize
        {
            get { return this.m_imageSize; }
        }
        internal Rectangle CaptionRectangle
        {
            get { return new Rectangle(0,0,this.ClientRectangle.Width,this.CaptionHeight); }
        }
        internal Rectangle ImageRectangle
		{
			get
			{
                if (this.m_imageRectangle == Rectangle.Empty)
                {
                    this.m_imageRectangle = new Rectangle(
                        CaptionSpacing,
                        ImagePaddingTop,
                        this.m_imageSize.Width,
                        this.m_imageSize.Height);
                }
                return this.m_imageRectangle;
			}
		}
		#endregion

		#region MethodsPublic
		#endregion

		#region MethodsProtected
		/// <summary>
		/// Initializes a new instance of the BasePanel class.
		/// </summary>
		protected BasePanel()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.ContainerControl, true);
			this.CaptionFont = new Font("Arial", SystemFonts.MenuFont.SizeInPoints, FontStyle.Bold);
			this.CaptionForeColor = SystemColors.ActiveCaptionText;
			this.PanelStyle = PanelStyle.Default;
            this.m_imageSize = new Size(16, 16);
            this.m_imageRectangle = Rectangle.Empty;
		}
		/// <summary>
		/// Raises the TextChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            this.Invalidate(this.CaptionRectangle);
            base.OnTextChanged(e);
        }
		/// <summary>
		/// Raises the Paint event. 
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			base.OnPaint(e);
		}
		/// <summary>
		/// Draws the specified text string on the specified caption surface; within the specified bounds; and in the specified font, color. 
		/// </summary>
		/// <param name="graphics">The Graphics to draw on.</param>
		/// <param name="iPositionX">The left position of the string</param>
		/// <param name="layoutRectangle">Rectangle structure that specifies the location of the drawn text.</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="fontColor">The color of the string</param>
		/// <param name="strText">String to draw.</param>
        protected static void DrawString(Graphics graphics, Rectangle layoutRectangle, Font font, Color fontColor, string strText, RightToLeft rightToLeft)
		{
            if (graphics == null)
            {
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    Resources.IDS_ArgumentException,
                    typeof(Graphics).Name));
            }
            using (SolidBrush stringBrush = new SolidBrush(fontColor))
			{
				using (StringFormat stringFormat = new StringFormat())
				{
                    if (rightToLeft == RightToLeft.Yes)
                    {
                        stringFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.DirectionRightToLeft;
                    }
                    else
                    {
                        stringFormat.FormatFlags = StringFormatFlags.NoWrap;
                    }
					stringFormat.Trimming = StringTrimming.EllipsisCharacter;
					stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Center;

					graphics.DrawString(strText, font, stringBrush, layoutRectangle, stringFormat);
				}
			}
		}
		/// <summary>
		/// Draws the Chevron Image at the specified location.
		/// </summary>
		/// <param name="graphics">The Graphics to draw on.</param>
		/// <param name="imgChevron">Chevron Image to draw.</param>
		/// <param name="chevronBackColor"></param>
		/// <param name="iPositionX">The left position of the Chevron Image</param>
        protected static void DrawChevron(Graphics graphics, Image imgChevron, Rectangle imageRectangle, Color chevronBackColor)
		{
			if (graphics == null)
			{
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    Resources.IDS_ArgumentException,
                    typeof(Graphics).Name));
			}
			if (imgChevron == null)
			{
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    Resources.IDS_ArgumentException,
                    typeof(Image).Name));
			}

			int chevronPositionX = imageRectangle.Left;
			int chevronPositionY = ImagePaddingTop;

			Rectangle rectangleChevron = new Rectangle(
                chevronPositionX + (imageRectangle.Width / 3) - 2,
				chevronPositionY + 3,
			    9,
			    9);

			using (System.Drawing.Imaging.ImageAttributes imageAttribute = new System.Drawing.Imaging.ImageAttributes())
			{
				imageAttribute.SetColorKey(Color.White, Color.White);
				System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();
                colorMap.OldColor = Color.FromArgb(0, 60, 166);
                colorMap.NewColor = chevronBackColor;
				imageAttribute.SetRemapTable(new System.Drawing.Imaging.ColorMap[] { colorMap });

				using (LinearGradientBrush brushInnerCircle = new LinearGradientBrush(
					new Rectangle(chevronPositionX, chevronPositionY, 16, 16),
					Color.FromArgb(128, Color.White),
					Color.FromArgb(0, Color.White),
					LinearGradientMode.Vertical))
				{
					graphics.FillPath(brushInnerCircle, GetPath(new Rectangle(chevronPositionX, chevronPositionY, 15, 15), 5));
				}
				graphics.DrawImage(imgChevron, rectangleChevron, 0, 0, 9, 9, GraphicsUnit.Pixel, imageAttribute);
			}
		}
		/// <summary>
		/// Draws the specified Image at the specified location. 
		/// </summary>
		/// <param name="graphics">The Graphics to draw on.</param>
		/// <param name="image">Image to draw.</param>
		/// <param name="imageRectangle">Rectangle structure that specifies the location and size of the drawn image.</param>
		/// <param name="iPositionX">The left position of the string behind this image as ref</param>
		protected static void DrawImage(Graphics graphics, Image image, Rectangle imageRectangle)
		{
            if (graphics == null)
            {
                throw new ArgumentNullException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    Resources.IDS_ArgumentException,
                    typeof(Graphics).Name));
            }
            if (image != null)
			{
				graphics.DrawImage(image, imageRectangle);
			}
		}
		/// <summary>
		/// Renders the background of the caption bar
		/// </summary>
		/// <param name="graphics">The Graphics to draw on.</param>
		/// <param name="bounds">Rectangle structure that specifies the location of the caption bar.</param>
		/// <param name="beginColor">The starting color of the gradient used on the caption bar</param>
		/// <param name="middleColor">The middle color of the gradient used on the caption bar</param>
		/// <param name="endColor">The end color of the gradient used on the caption bar</param>
		/// <param name="linearGradientMode">Specifies the type of fill a Pen object uses to fill lines.</param>
		/// <param name="flipHorizontal"></param>
		protected static void RenderDoubleBackgroundGradient(Graphics graphics, Rectangle bounds, Color beginColor, Color middleColor, Color endColor, LinearGradientMode linearGradientMode, bool flipHorizontal)
		{
			RenderDoubleBackgroundGradient(
				graphics,
				bounds,
				beginColor,
				middleColor,
				endColor,
				12,
				12,
				linearGradientMode,
				flipHorizontal);
		}
		/// <summary>
		/// Renders the panels background
		/// </summary>
		/// <param name="graphics">The Graphics to draw on.</param>
		/// <param name="bounds">Rectangle structure that specifies the backgrounds location.</param>
		/// <param name="beginColor">The starting color of the gradient used on the panels background</param>
		/// <param name="endColor">The end color of the gradient used on the panels background</param>
		/// <param name="linearGradientMode">Specifies the type of fill a Pen object uses to fill lines.</param>
		protected static void RenderBackgroundGradient(Graphics graphics, Rectangle bounds, Color beginColor, Color endColor, LinearGradientMode linearGradientMode)
		{
            if (graphics == null)
            {
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    Resources.IDS_ArgumentException,
                    typeof(Graphics).Name));
            }
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, beginColor, endColor, linearGradientMode))
			{
				if (IsZeroWidthOrHeight(bounds))
				{
					return;
				}

				linearGradientBrush.TranslateTransform((float)(bounds.Location.X + bounds.Width), (float)(bounds.Y - bounds.Height));
				graphics.FillRectangle(linearGradientBrush, new Rectangle(Point.Empty, bounds.Size));
			}
		}
		/// <summary>
		/// Gets a GraphicsPath. 
		/// </summary>
		/// <param name="bounds">Rectangle structure that specifies the backgrounds location.</param>
		/// <param name="r">The radius in the graphics path</param>
		/// <returns>the specified graphics path</returns>
        protected static GraphicsPath GetPath(Rectangle bounds, int radius)
		{
			int x = bounds.X;
			int y = bounds.Y;
			int width = bounds.Width;
			int height = bounds.Height;
			GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(x, y, radius, radius, 180, 90);				                    //Upper left corner
            graphicsPath.AddArc(x + width - radius, y, radius, radius, 270, 90);			    //Upper right corner
            graphicsPath.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);//Lower right corner
            graphicsPath.AddArc(x, y + height - radius, radius, radius, 90, 90);			                    //Lower left corner
			graphicsPath.CloseFigure();
			return graphicsPath;
		}
		/// <summary>
		/// Gets a GraphicsPath.
		/// </summary>
		/// <param name="bounds">Rectangle structure that specifies the backgrounds location.</param>
		/// <param name="r">The radius in the graphics path</param>
		/// <returns>the specified graphics path</returns>
        protected virtual GraphicsPath GetBackgroundPath(Rectangle bounds, int radius)
		{
			int x = bounds.X;
			int y = bounds.Y;
			int width = bounds.Width;
			int height = bounds.Height;
			GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(x, y + height, x, y - radius);                 //Left Line
            graphicsPath.AddArc(x, y, radius, radius, 180, 90);                 //Upper left corner
            graphicsPath.AddArc(x + width - radius, y, radius, radius, 270, 90);//Upper right corner
            graphicsPath.AddLine(x + width, y + radius, x + width, y + height); //Right Line
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		#endregion

		#region MethodsPrivate

		private static void RenderDoubleBackgroundGradient(Graphics graphics, Rectangle bounds, Color beginColor, Color middleColor, Color endColor, int firstGradientWidth, int secondGradientWidth, LinearGradientMode mode, bool flipHorizontal)
		{
			if ((bounds.Width != 0) && (bounds.Height != 0))
			{
				Rectangle rectangle1 = bounds;
				Rectangle rectangle2 = bounds;
				bool flag1 = true;
				if (mode == LinearGradientMode.Horizontal)
				{
					if (flipHorizontal)
					{
						Color color1 = endColor;
						endColor = beginColor;
						beginColor = color1;
					}
					rectangle2.Width = firstGradientWidth;
					rectangle1.Width = secondGradientWidth + 1;
					rectangle1.X = bounds.Right - rectangle1.Width;
					flag1 = bounds.Width > (firstGradientWidth + secondGradientWidth);
				}
				else
				{
					rectangle2.Height = firstGradientWidth;
					rectangle1.Height = secondGradientWidth + 1;
					rectangle1.Y = bounds.Bottom - rectangle1.Height;
					flag1 = bounds.Height > (firstGradientWidth + secondGradientWidth);
				}
				if (flag1)
				{
					using (Brush brush1 = new SolidBrush(middleColor))
					{
						graphics.FillRectangle(brush1, bounds);
					}
					using (Brush brush2 = new LinearGradientBrush(rectangle2, beginColor, middleColor, mode))
					{
						graphics.FillRectangle(brush2, rectangle2);
					}
					using (LinearGradientBrush brush3 = new LinearGradientBrush(rectangle1, middleColor, endColor, mode))
					{
						if (mode == LinearGradientMode.Horizontal)
						{
							rectangle1.X++;
							rectangle1.Width--;
						}
						else
						{
							rectangle1.Y++;
							rectangle1.Height--;
						}
						graphics.FillRectangle(brush3, rectangle1);
						return;
					}
				}
				using (Brush brush4 = new LinearGradientBrush(bounds, beginColor, endColor, mode))
				{
					graphics.FillRectangle(brush4, bounds);
				}
			}
		}

		

		private static bool IsZeroWidthOrHeight(Rectangle rectangle)
		{
			if (rectangle.Width != 0)
			{
				return (rectangle.Height == 0);
			}
			return true;
		}

		#endregion
	}
}
