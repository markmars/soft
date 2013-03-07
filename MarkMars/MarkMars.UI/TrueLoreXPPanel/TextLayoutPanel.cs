using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D ;
using System.Windows.Forms;
using System.Drawing.Imaging ;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// TextLayoutPanel
	/// </summary>
	public class TextLayoutPanel : Panel {
		#region enum TextLayoutPanelProperty
		/// <summary>
		/// Emumeration of properties that trigger internal property change events
		/// </summary>
		public enum TextLayoutPanelProperty {
			/// <summary>
			/// See <see cref="TextLayoutPanel.Elements"/> property
			/// </summary>
			ElementsProperty,
			/// <summary>
			/// See <see cref="TextLayoutPanel.PanelGradient"/> property
			/// </summary>
			PanelGradientProperty,
			/// <summary>
			/// See <see cref="TextLayoutPanel.BorderMargin"/> property
			/// </summary>
			BorderMarginProperty,
			/// <summary>
			/// See <see cref="TextLayoutPanel.TextSpacing"/> property
			/// </summary>
			TextSpacingProperty,
			/// <summary>
			/// See <see cref="TextLayoutPanel.ImageSize"/> property
			/// </summary>
			ImageSizeProperty,
			/// <summary>
			/// See <see cref="TextLayoutPanel.AutoSize"/> property
			/// </summary>
			AutoSizeProperty,
			/// <summary>
			/// See <see cref="TextLayoutPanel.BackgroundStyle"/> property
			/// </summary>
			BackgroundStyleProperty,
			/// <summary>
			/// See <see cref="Control.BackColor"/> property
			/// </summary>
			BackColorProperty,
			/// <summary>
			/// See <see cref="Control.ForeColor"/> property
			/// </summary>
			ForeColorProperty,
			/// <summary>
			/// See <see cref="Control.Font"/> property
			/// </summary>
			FontProperty
		}
		#endregion enum TextLayoutPanelProperty

		#region Constants
		/// <summary>
		/// Default value for <see cref="PanelGradient"/>
		/// </summary>
		public static readonly GradientColor DefaultPanelGradient = new GradientColor(Color.CornflowerBlue) ;

		/// <summary>
		/// Default value for <see cref="BorderMargin"/>
		/// </summary>
		public static readonly Size DefaultBorderMargin = new Size(4,4) ;

		/// <summary>
		/// Default value for <see cref="TextSpacing"/>
		/// </summary>
		public const int DefaultSpacing = 5 ;
		#endregion Constants

		#region Static Methods
		/// <summary>
		/// Standard gray-scale conversion for images
		/// </summary>
		/// <remarks>
		/// This is a very generic definition and is defined as <see langword="state"/> so
		/// that it can be shared by all instances of <c>TextLayoutPanel</c>
		/// </remarks>
		private static System.Drawing.Imaging.ImageAttributes grayScaleAttributes ;
		
		/// <summary>
		/// Returns the standard grayscale matrix for image transformations
		/// </summary>
		[Browsable(false)]
		public static ImageAttributes GrayScaleAttributes {
			get {
				// prevent two of these from being created concurrently by locking the class
				lock(typeof(TextLayoutPanel)) {
					if (grayScaleAttributes == null) {
						ColorMatrix matrix = new ColorMatrix();
						matrix.Matrix00 = 1/3f;
						matrix.Matrix01 = 1/3f;
						matrix.Matrix02 = 1/3f;
						matrix.Matrix10 = 1/3f;
						matrix.Matrix11 = 1/3f;
						matrix.Matrix12 = 1/3f;
						matrix.Matrix20 = 1/3f;
						matrix.Matrix21 = 1/3f;
						matrix.Matrix22 = 1/3f;
						
						grayScaleAttributes = new ImageAttributes();

						grayScaleAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default,ColorAdjustType.Bitmap);
					}
				}

				return grayScaleAttributes ;
			}
		}
		#endregion Static Methods

		#region Fields
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// The <see cref="GradientColor"/> for the background of the <c>TextLayoutPanel</c>
		/// </summary>
		private GradientColor panelGradient = new GradientColor(DefaultPanelGradient) ;

		/// <summary>
		/// <c>TextElement</c> collection
		/// </summary>
		private TextElementCollection textElementCollection = null ;

		/// <summary>
		/// Controls left/top/right spacing of <see cref="TextLayoutPanel"/> controls within the <c>TextLayoutPanel</c>
		/// </summary>
		private Size borderMargin = new Size(DefaultBorderMargin.Width,DefaultBorderMargin.Height) ;

		/// <summary>
		/// Controls the Y spacing between <see cref="TextElement"/> items in the <c>TextLayoutPanel</c>
		/// </summary>
		private int textSpacing = DefaultSpacing ;

		/// <summary>
		/// Tracks when we need to recalculate our layout
		/// </summary>
		private bool isLayoutDirty = true ;

		/// <summary>
		/// Force images into this size (or no restriction if Size.Empty)
		/// </summary>
		private Size imageSize = Size.Empty ;

		/// <summary>
		/// Gradient brush used to draw the background
		/// </summary>
		private LinearGradientBrush backgroundBrush = null ;

		/// <summary>
		/// <see langword="true"/> if the panel auto-sizes itself based upon content
		/// </summary>
		private bool autoSize = false ;

		/// <summary>
		/// Default background style is not to have one (i.e, Transparent)
		/// </summary>
		private BackgroundStyle backgroundStyle = BackgroundStyle.Transparent ;

		/// <summary>
		/// Used to indicate that redrawing of TextElements is suspended 
		/// (used when large #'s of updates occur as a single transaction)
		/// </summary>
		private int redrawSuspended = 0 ;
		#endregion Fields

		#region Constructor(s)
		/// <summary>
		/// Create a <c>TextLayoutPanel</c>
		/// </summary>
		/// <remarks>
		/// By default the <see cref="Control.Font"/> is set to 'Microsoft Verdana, 8.5'
		/// </remarks>
		public TextLayoutPanel() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Use these control styles for smoother drawing and transparency support
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.ContainerControl, true);
			SetStyle(ControlStyles.Selectable,true) ;

			Elements = new TextElementCollection() ;
			Font = new Font("Microsoft Verdana", 8.5f) ;
		}		
		#endregion Constructor(s)

		#region Dispose
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}
		#endregion Dispose

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Properties
		/// <summary>
		/// <see langword="true"/> if the <c>ItemLayoutPanel</c> has one or more child controls
		/// </summary>
		[Browsable(false)]
		public bool HasElements {
			get {
				return textElementCollection.Count > 0 ;
			}
		}

		/// <summary>
		/// Get the <see cref="ArrayList"/> of control items
		/// </summary>
		/// <remarks>
		/// The controls in the <c>Items</c> property are in appearance order
		/// <para>
		/// You should NOT directly modify the contents of this member
		/// </para>
		/// </remarks>
		[Category("Appearance"), Description("Background Style for TextLayoutPanel")]
		public BackgroundStyle BackgroundStyle {
			get {
				return backgroundStyle ;
			}

			set {
				if (backgroundStyle != value) {
					backgroundStyle = value ;
					OnPropertyChange(TextLayoutPanelProperty.BackgroundStyleProperty) ;
				}
			}
		}

		/// <summary>
		/// The <c>TextElement</c> Collection
		/// </summary>
		[Category("Elements"), Description("Text Elements Collection")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TextElementCollection Elements {
			get {
				return textElementCollection ;
			}

			set {
				if (textElementCollection != value) {
					if (textElementCollection != null) {
						// remove listener
						textElementCollection.Change -= new EventHandler(textElementCollection_Change) ;
                        textElementCollection.ElementChange -= new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(textElementCollection_ElementChange);
					}

					if (value == null) {
						value = new TextElementCollection() ;
					}

					textElementCollection = value ;

					// add listener
					textElementCollection.Change += new EventHandler(textElementCollection_Change);
                    textElementCollection.ElementChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(textElementCollection_ElementChange);

					// notify
					OnPropertyChange(TextLayoutPanelProperty.ElementsProperty) ;
				}
			}
		}

		/// <summary>
		/// If we have any elements then serialize them
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if we have any defined TextElement(s)
		/// </returns>
		protected bool ShouldSerializeElements() {
			return textElementCollection.Count > 0 ;
		}

		/// <summary>
		/// Reset the property to its default value
		/// </summary>
		/// <remarks>
		/// Called by the IDE designer
		/// </remarks>
		protected void ResetElements() {
			textElementCollection.Clear() ;
		}

		/// <summary>
		/// <see cref="GradientColor"/> used to draw the background of the <c>TextLayoutPanel</c>
		/// </summary>
		/// <remarks>
		/// <para>Fires a PropertyChange event w/ <see cref="TextLayoutPanelProperty.PanelGradientProperty"/> argument</para>
		/// </remarks>
		[Category("Appearance"),
		Description("Gradient color for the background of the TextLayoutPanel")]
		public GradientColor PanelGradient {
			get {
				return panelGradient ;
			}

			set {
				if (!panelGradient.Equals(value)) {
					if (value == null) {
						panelGradient = new GradientColor(SystemColors.Control) ;
					}

					panelGradient = value ;
					OnPropertyChange(TextLayoutPanelProperty.PanelGradientProperty) ;
				}
			}
		}

		/// <summary>
		/// Determine if this property should be serialized
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the proeprty does not equal the default value
		/// </returns>
		protected bool ShouldSerializePanelGradient() {
			return panelGradient != DefaultPanelGradient ;
		}

		/// <summary>
		/// Reset the property to its default value
		/// </summary>
		/// <remarks>
		/// Called by the IDE designer
		/// </remarks>
		protected void ResetPanelGradient() {
			PanelGradient = DefaultPanelGradient ;
		}

		/// <summary>
		/// Sets the left/right/top margins for <see cref="TextElement"/> instance within the <c>TextLayoutPanel</c>
		/// </summary>
		/// <remarks>
		/// <para>Fires a PropertyChange event w/ <see cref="TextLayoutPanelProperty.BorderMarginProperty"/> argument</para>
		/// </remarks>
		[Category("Appearance"),
		Description("X/Y margins for TextElement's in the TextLayoutPanel")]
		public Size BorderMargin {
			get {
				return borderMargin ;
			}
		
			set {
				if (!borderMargin.Equals(value)) {
					borderMargin = value ;
					OnPropertyChange(TextLayoutPanelProperty.BorderMarginProperty) ;
				}
			}
		}

		/// <summary>
		/// Determine if this property should be serialized
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the proeprty does not equal the default value
		/// </returns>
		protected bool ShouldSerializeBorderMargin() {
			return borderMargin != DefaultBorderMargin ;
		}

		/// <summary>
		/// Reset the property to its default value
		/// </summary>
		/// <remarks>
		/// Called by the IDE designer
		/// </remarks>
		protected void ResetBorderMargin() {
			BorderMargin = DefaultBorderMargin ;
		}

		/// <summary>
		/// Y spacing between <see cref="TextElement"/> instances within the <c>TextLayoutPanel</c>
		/// </summary>
		/// <remarks>
		/// Default value for this property is <see cref="DefaultSpacing"/>
		/// <para>Fires a PropertyChange event w/ <see cref="TextLayoutPanelProperty.TextSpacingProperty"/> argument</para>
		/// </remarks>
		[Category("Appearance"),
		Description("Y spacing between Text Elements in the TextLayoutPanel"),
		DefaultValue(DefaultSpacing)]
		public int TextSpacing {
			get {
				return textSpacing ;
			}

			set {
				if (textSpacing != value) {
					textSpacing = value ;
					OnPropertyChange(TextLayoutPanelProperty.TextSpacingProperty) ;
				}
			}
		}

		/// <summary>
		/// If non-empty, restricts the display size of images for <see cref="TextElement"/>'s
		/// </summary>
		[Category("Appearance"),Description("The display size of the TextElement images in the TextLayoutPanel")]
		[DefaultValue("Empty")]
		public Size ImageSize {
			get {
				return imageSize ;
			}

			set {
				if (imageSize != value) {
					imageSize = value ;
					OnPropertyChange(TextLayoutPanelProperty.ImageSizeProperty) ;
				}
			}
		}
		#endregion Properties

		#region Methods
		/// <summary>
		/// Disable redrawing of <see cref="TextElement"/> contents
		/// </summary>
		/// <remarks>
		/// This method is 'reference' counted. Each call to this method
		/// must be matched by a call to <see cref="EnableRedraw()"/>
		/// </remarks>
		public void DisableRedraw() {
			redrawSuspended++ ;
		}

		/// <summary>
		/// Re-enable drawing of <see cref="TextElement"/> contents
		/// </summary>
		/// <remarks>
		/// This method should be called once for each call to <see cref="DisableRedraw()"/>
		/// </remarks>
		public void EnableRedraw() {
			if (redrawSuspended == 0)
				return ;

			if (--redrawSuspended == 0) {
				Invalidate() ;
			}
		}

		/// <summary>
		/// <see langword="true"/> if redrawing of <see cref="TextElement"/> content is disabled
		/// </summary>
		[Browsable(false)]
		public bool IsRedrawSuspended {
			get {
				return (redrawSuspended > 0) ;
			}
		}

		/// <summary>
		/// Get/Set whether this <c>TextLayoutPanel</c> automatically resizes itself (and
		/// possibly its parent) to match its contents
		/// </summary>
		[Category("Behavior"), Description("True if the panel resizes based on content")]
		[DefaultValue(false)]
		public bool AutoSize {
			get {
				return autoSize ;
			}

			set {
				if (autoSize != value) {
					autoSize = value ;
					OnPropertyChange(TextLayoutPanelProperty.AutoSizeProperty) ;
				}
			}
		}
		#endregion Methods

		#region Implementation
		/// <summary>
		/// Handles internal property change events
		/// </summary>
		/// <param name="property">The enumeration for the property that changed</param>
		/// <remarks>
		/// These events are NOT exposed to listeners. You must derive and override 
		/// to get this style of functionaltiy
		/// </remarks>
		protected virtual void OnPropertyChange(TextLayoutPanelProperty property) {
			switch(property) {
				case TextLayoutPanelProperty.ElementsProperty:
				case TextLayoutPanelProperty.BorderMarginProperty:
				case TextLayoutPanelProperty.TextSpacingProperty:
				case TextLayoutPanelProperty.ImageSizeProperty:
				case TextLayoutPanelProperty.FontProperty:
					isLayoutDirty = true ;
					break ;

				case TextLayoutPanelProperty.BackColorProperty:
				case TextLayoutPanelProperty.PanelGradientProperty:
				case TextLayoutPanelProperty.BackgroundStyleProperty:
					BackgroundBrush = null ;
					break ;

				case TextLayoutPanelProperty.ForeColorProperty:
				case TextLayoutPanelProperty.AutoSizeProperty:
					break ;
			}

			if (!IsRedrawSuspended) {
				Invalidate() ;
			}
		}
		#endregion Implementation

		#region Cached Drawing Properties
		/// <summary>
		/// The <see cref="LinearGradientBrush"/> used to draw the background
		/// </summary>
		/// <remarks>
        /// When the <see cref="BackgroundStyle"/> is <see cref="MarkMars.UI.TrueLoreXPPanel.BackgroundStyle.Solid"/>
		/// the brush is created with the <see cref="Control.BackColor"/> value, otherwise
        /// if <see cref="BackgroundStyle"/> is <see cref="MarkMars.UI.TrueLoreXPPanel.BackgroundStyle.Gradient"/>
		/// we use the <see cref="PanelGradient"/> color values.
		/// 
        /// In the case of <see cref="MarkMars.UI.TrueLoreXPPanel.BackgroundStyle.Transparent"/> the result is <b>undefined</b>
		/// </remarks>
		protected LinearGradientBrush BackgroundBrush {
			get {
				if (backgroundBrush == null) {
					if (BackgroundStyle == BackgroundStyle.Gradient) {
						backgroundBrush = new LinearGradientBrush(
							new Rectangle(0,0,Width,Height),
							PanelGradient.Start,
							PanelGradient.End,
							LinearGradientMode.Horizontal
							) ;
					} else {
						backgroundBrush = new LinearGradientBrush(
							new Rectangle(0,0,Width,Height),
							BackColor,
							BackColor,
							LinearGradientMode.Horizontal
							) ;
					}
				}

				return backgroundBrush ;
			}

			set {
				if (backgroundBrush != value) {
					if (backgroundBrush != null) {
						backgroundBrush.Dispose() ;
					}

					backgroundBrush = value ;
				}
			}
		}
		#endregion Cached Drawing Properties

		#region Paint Code
		/// <summary>
		/// Measure the specified text to establish the <c>vertical extents</c> of its
		/// bounding box. This way we can support multi-line items
		/// </summary>
		/// <param name="g">The <see cref="Graphics"/> object being used</param>
		/// <param name="width">The horizontal extents/limits</param>
		/// <param name="font">The font of the text</param>
		/// <param name="stringFormat">The <see cref="StringFormat"/> options</param>
		/// <param name="text">The actual text</param>
		/// <returns></returns>
		protected Size GetTextSize(Graphics g,String text,Font font,int width,StringFormat stringFormat) {
			return g.MeasureString(text,font,width,stringFormat).ToSize() ;
		}

		/// <summary>
		/// Get the <see cref="System.Drawing.Font"/> used by a <see cref="TextElement"/>
		/// </summary>
		/// <param name="textElement">The TextElement being drawn/measured</param>
		/// <returns>
		/// The <see cref="TextElement.Font"/> value, or if <see langword="null"/> the
		/// <see cref="Font"/> of this <c>TextLayoutPanel</c>
		/// </returns>
		private Font GetElementFont(ITextElement textElement) {
			return (textElement.Font != null) ? textElement.Font : Font ;
		}

		/// <summary>
		/// Get the <see cref="System.Drawing.Color"/> used by a <see cref="TextElement"/>
		/// </summary>
		/// <param name="textElement">The TextElement being drawn/measured</param>
		/// <returns>
		/// The <see cref="TextElement.Font"/> value, or if <see langword="null"/> the
		/// <see cref="Font"/> of this <c>TextLayoutPanel</c>
		/// </returns>
		private Color GetElementTextColor(ITextElement textElement) {
			return (textElement.TextColor != Color.Empty) ? textElement.TextColor : ForeColor ;
		}

		/// <summary>
		/// Set the height of the <c>TextLayoutPanel</c> and if applicable, the height 
		/// of parent <see cref="XPPanel"/>'s panel area to match our height (with 
		/// a little margin)
		/// </summary>
		/// <param name="bestHeight">The best height as determined by the layout engine</param>
		private void SetBestHeight(int bestHeight) {
			this.Height = bestHeight ;
		
			if (!DesignMode) {
				XPPanel panel = Parent as XPPanel ;
				if (panel != null) {
					panel.PanelHeight = bestHeight + 12 ;
				}
			}
		}

		/// <summary>
		/// Recalculate the layout of all <see cref="TextElement"/> content
		/// </summary>
		/// <param name="graphics">The graphics to be used for drawing</param>
		/// <returns>
		/// The array of <see cref="TextElement"/> instances
		/// </returns>
		protected virtual ITextElement [] RecalculateLayout(Graphics graphics) {
			// content
			ITextElement [] elements = Elements.ToArray() ;

			StringFormat textFormat = new StringFormat() ;
			textFormat.Trimming = StringTrimming.Word ;
			textFormat.FormatFlags = StringFormatFlags.FitBlackBox ;
			
			// offset of 1st element
			int yOffset = BorderMargin.Height ;

			foreach(ITextElement textElement in elements) {
				// not shown
				if (!textElement.Visible)
					continue ;

				// custom format flags
				textFormat.Alignment = textElement.HorzAlign ;
				textFormat.LineAlignment = textElement.VertAlign ;

				// apply proceeding element kerning
				yOffset += textElement.SpacingAdjustment.Width ;

				// "width" used by image (if any)
				Size bestImageSize = Size.Empty ;


				if (textElement.Image != null) {
					// initial image rect
					Rectangle imageRect = new Rectangle(
						BorderMargin.Width+textElement.Indent,
						yOffset,
						textElement.Image.Width,
						textElement.Image.Height
						) ;

					// does this panel override the image dimensions?
					if (ImageSize != Size.Empty) {
						imageRect.Width = ImageSize.Width ;
						imageRect.Height = ImageSize.Height ;
					}

					// cache the image rect
					textElement["TextLayoutPanel.ImageRect"] = imageRect ;

					// width plus reusable spacing
					bestImageSize = new Size(imageRect.Width + 4,imageRect.Height) ;
				} else {
					// cache the image rect
					textElement["TextLayoutPanel.ImageRect"] = Rectangle.Empty ;
				}

				// measure the text, primarily looking for height extents
				Size textSize = GetTextSize(
					graphics,
					textElement.ToString(),
					GetElementFont(textElement),
					Width - (BorderMargin.Width<<1) - bestImageSize.Width - textElement.Indent,
					textFormat
					) ;

				int bestHeight = textSize.Height ;
				
				// height the element is max of text/image
				if (textElement.Image != null) {
					bestHeight = Math.Max(bestHeight,bestImageSize.Height) ;
				}

				// cache the element layout rect
				textElement["TextLayoutPanel.ElementRect"] = new Rectangle(
					BorderMargin.Width + bestImageSize.Width + textElement.Indent,
					yOffset,
					Width-(BorderMargin.Width<<1) - bestImageSize.Width - textElement.Indent,
					bestHeight
					) ;

				// adjust the offset of the next element based on the height and succeeding element kerning
				// (plus the normal text spacing)
				yOffset += bestHeight + TextSpacing + textElement.SpacingAdjustment.Height ;
			}

			// If we are in auto-size mode, adjust our height (and possibly our parents)
			if (AutoSize) {
				SetBestHeight(yOffset) ;
			}

			// up-to-date
			isLayoutDirty = false ;

			return elements ;
		}

		/// <summary>
		/// Paint the background of the <c>TextLayoutPanel</c> using the gradient color for the panel and place
		/// all the visible <c>TextElement</c> content in their appropriate position
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaint(PaintEventArgs pevent) {
			// draw background (if necessary)
			if (BackgroundStyle != BackgroundStyle.Transparent) {
				pevent.Graphics.FillRectangle(BackgroundBrush,pevent.ClipRectangle) ;
			}

			// don't draw elements if suspended
			if (IsRedrawSuspended) 
				return ;

			ITextElement [] elements = null ;

			// recalculate the layout if necessary
			if (isLayoutDirty) {
				elements = RecalculateLayout(pevent.Graphics) ;
			} else {
				elements = Elements.ToArray() ;
			}

			// nothing to do...
			if ((elements == null) || (elements.Length == 0))
				return ;

			// Default text formatting
			StringFormat textFormat = new StringFormat() ;
			textFormat.Trimming = StringTrimming.Word ;
			textFormat.FormatFlags = StringFormatFlags.FitBlackBox ;

			foreach(ITextElement textElement in elements) {
				// not shown
				if (!textElement.Visible)
					continue ;

				// custom text formatting
				textFormat.Alignment = textElement.HorzAlign ;
				textFormat.LineAlignment = textElement.VertAlign ;

				// cached layout values
				Rectangle elementRect = (Rectangle) textElement["TextLayoutPanel.ElementRect"] ;
				Rectangle imageRect = (Rectangle) textElement["TextLayoutPanel.ImageRect"] ;

				// do we need to draw a background for the element?
				if ((textElement.BackColor != Color.Transparent) && !textElement.BackColor.IsEmpty) {
					Rectangle itemRect ;
					
					// combine the image and text rect since together the represent the entire area
					if (textElement.Image != null) {
						itemRect = Rectangle.Union(elementRect,imageRect) ;
					} else {
						itemRect = elementRect ;
					}

					// fill the background
					using(SolidBrush brush=new SolidBrush(textElement.BackColor)) {
						pevent.Graphics.FillRectangle(brush,itemRect) ;
					}
				}

				// draw the image (scaling it as necessary)
				if (textElement.Image != null) {
					if (Enabled) {
						pevent.Graphics.DrawImage(textElement.Image,imageRect) ;
					} else {
						// draw it gray'd out
						pevent.Graphics.DrawImage(
							textElement.Image,
							imageRect,
							0,0,textElement.Image.Width,textElement.Image.Height,
							GraphicsUnit.Pixel,
							GrayScaleAttributes
							) ;
					}
				}

				if (Enabled) {
					// draw text element using its text color
					using(SolidBrush elementBrush = new SolidBrush(GetElementTextColor(textElement))) {
						pevent.Graphics.DrawString(
							textElement.ToString(),
							GetElementFont(textElement),
							elementBrush,
							elementRect,
							textFormat
							) ;
					}
				} else {
					// draw text element in disabled fashion
					ControlPaint.DrawStringDisabled(
						pevent.Graphics,
						textElement.ToString(),
						GetElementFont(textElement),
						GetElementTextColor(textElement),
						elementRect,
						textFormat
						);
				}
			}
		}
		#endregion Paint Code

		#region Overrides
		/// <summary>
		/// Layout is dirty if the size of the control changes
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged (e);
			isLayoutDirty = true ;
			Invalidate() ;
		}

		/// <summary>
		/// The <see cref="BackgroundBrush"/> may need to be updated
		/// </summary>
		/// <param name="e"></param>
		protected override void OnBackColorChanged(EventArgs e) {
			base.OnBackColorChanged (e);
			OnPropertyChange(TextLayoutPanelProperty.BackColorProperty) ;
		}

		/// <summary>
		/// The layout is dirty since 1 or more text elements may be
		/// using our 'default' <see cref="Font"/>
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFontChanged(EventArgs e) {
			base.OnFontChanged (e);
			OnPropertyChange(TextLayoutPanelProperty.FontProperty) ;
		}

		/// <summary>
		/// Need to redraw since 1 or more text elements may be
		/// using our 'default' <see cref="Control.ForeColor"/>
		/// </summary>
		/// <param name="e"><see cref="System.EventArgs.Empty"/></param>
		protected override void OnForeColorChanged(EventArgs e) {
			base.OnForeColorChanged (e);
			OnPropertyChange(TextLayoutPanelProperty.ForeColorProperty) ;
		}
		#endregion Overrides

		#region Event Handlers
		/// <summary>
		/// The contents of the <see cref="Elements"/> collection has changed in an unspecified
		/// way (added, removed, moved, etc...)
		/// </summary>
		/// <param name="sender">The <see cref="TextElementCollection"/> that changed</param>
		/// <param name="e"><see cref="System.EventArgs.Empty"/></param>
		private void textElementCollection_Change(object sender, EventArgs e) {
			OnPropertyChange(TextLayoutPanelProperty.ElementsProperty) ;
		}

		/// <summary>
		/// The attributes of a <see cref="TextElement"/> within the <see cref="Elements"/> collection 
		/// has changed
		/// </summary>
		/// <param name="sender">The <see cref="TextElementCollection"/> that changed</param>
		/// <param name="e"><see cref="TextElementPropertyChangeEventArgs"/> specifying the <see cref="TextElement"/>
		/// and the property that changed</param>
        private void textElementCollection_ElementChange(object sender, MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventArgs e)
        {
			switch(e.Property) {
				// all these invalidate the layout
				case TextElementProperty.FontProperty:
				case TextElementProperty.HorizontalAlignmentProperty:
				case TextElementProperty.VerticalAlignmentProperty:
				case TextElementProperty.StyleProperty:
				case TextElementProperty.VisibleProperty:
				case TextElementProperty.SpacingAdjustmentProperty:
				case TextElementProperty.ImageSetProperty:
				case TextElementProperty.ImageProperty:
				case TextElementProperty.TextProperty:
				case TextElementProperty.PrefixProperty:
				case TextElementProperty.IndentProperty:
					isLayoutDirty = true ;
					break ;

				// all these force a redraw
				case TextElementProperty.TextColorProperty:
				case TextElementProperty.BackColorProperty:
				case TextElementProperty.OtherProperty:
					break ;
			}

			if (!IsRedrawSuspended) {
				Invalidate() ;
			}
		}
		#endregion Event Handlers
	}

	#region class TextElementCollection
	/// <summary>
	/// A collection of <see cref="TextElement"/> instances
	/// </summary>
	[Serializable]
	public class TextElementCollection : CollectionBase {
		#region Fields
			#region Events
		/// <summary>
		/// Listeners to <see cref="Change"/> events
		/// </summary>
		[NonSerialized]
		private EventHandler changeListeners = null ;
		/// <summary>
		/// Listeners to <see cref="ElementChange"/> events
		/// </summary>
		[NonSerialized]
		private TextElementPropertyChangeEventHandler elementChangeListeners = null ;
			#endregion Events
		#endregion Fields

		#region Constructor(s)
		/// <summary>
		/// Create an empty <see cref="TextElement"/> collection
		/// </summary>
		public TextElementCollection() {}
		#endregion Constructor(s)

		#region Events
		/// <summary>
		/// Register/Unregister for changes to the <c>TextElementCollection</c>
		/// </summary>
		public event EventHandler Change {
			add {
				changeListeners += value ;
			}

			remove {
				changeListeners -= value ;
			}
		}

		/// <summary>
		/// Register/Unregister for changes to the <c>TextElement</c> instances
		/// in the <c>TextElementCollection</c>
		/// </summary>
		public event TextElementPropertyChangeEventHandler ElementChange {
			add {
				elementChangeListeners += value ;
			}

			remove {
				elementChangeListeners -= value ;
			}
		}
		#endregion Events

		#region Properties
		/// <summary>
		/// Get/Set the <c>TextElement</c> at the specified index
		/// </summary>
		public TextElement this[int index] {
			get {
				return (TextElement) InnerList[index] ;
			}

			set {
				InnerList[index] = value ;
			}
		}
		#endregion Properties

		#region Implementation
		/// <summary>
		/// Fire a <see cref="Change"/> event
		/// </summary>
		protected virtual void OnChangeEvent() {
			if (changeListeners != null) {
				changeListeners(this,System.EventArgs.Empty) ;
			}
		}

		/// <summary>
		/// Fire a <see cref="ElementChange"/> event
		/// </summary>
		protected virtual void OnElementChangeEvent(TextElementPropertyChangeEventArgs eventArgs) {
			if (elementChangeListeners != null) {
				elementChangeListeners(this,eventArgs) ;
			}
		}
		#endregion Implementation

		#region Methods
		/// <summary>
		/// Insert a <see cref="TextElement"/> at the specified location
		/// </summary>
		/// <param name="index">The index</param>
		/// <param name="textElement">The <see cref="TextElement"/></param>
		public void Insert(int index,ITextElement textElement) {
			if (textElement != null) {
				List.Insert(index,textElement) ;
				// listen to its PropertyChange events
                textElement.PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextElementCollection_PropertyChange);
				// notify our listeners we changed
				OnChangeEvent() ;
			}
		}

		/// <summary>
		/// Add a <see cref="TextElement"/> to the collection
		/// </summary>
		/// <param name="textElement">The <see cref="TextElement"/></param>
		/// <returns>
		/// The index of the inserted item
		/// </returns>
		public int Add(ITextElement textElement) {
			if (textElement == null)
				return -1 ;

			int result = base.InnerList.Add(textElement) ;

			if (result != -1) {
				// listen to its PropertyChange events
                textElement.PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextElementCollection_PropertyChange);
				// notify our listeners we changed
				OnChangeEvent() ;
			}

			return result ;
		}

		/// <summary>
		/// Add 'N' <see cref="TextElement"/> items to the collection
		/// </summary>
		/// <param name="textElements">The <see cref="TextElement"/>'s to be added</param>
		public void AddRange(ITextElement [] textElements) {
			foreach(ITextElement textElement in textElements) {
				if (textElement != null) {
                    textElement.PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextElementCollection_PropertyChange);
					base.InnerList.Add(textElement) ;
				}
			}

			OnChangeEvent() ;
		}

		/// <summary>
		/// Reorder a <see cref="TextElement"/> within the collection
		/// </summary>
		/// <param name="currIndex">The current index of the <see cref="TextElement"/></param>
		/// <param name="newIndex">The new index of the <see cref="TextElement"/></param>
		public void MoveTo(int currIndex,int newIndex) {
			if ((currIndex >= 0) && (currIndex < Count) && (currIndex != newIndex)) {
				ITextElement textElement = this[currIndex] ;
				this.InnerList.RemoveAt(currIndex) ;
				this.InnerList.Insert(newIndex,textElement) ;
				OnChangeEvent() ;
			}
		}

		/// <summary>
		/// Reorder a <see cref="TextElement"/> within the collection
		/// </summary>
		/// <param name="newIndex">The new index of the <see cref="TextElement"/></param>
		/// <param name="element">The <see cref="TextElement"/> to move</param>
		public void MoveTo(int newIndex,TextElement element) {
			int indexOf = InnerList.IndexOf(element) ;

			if ((indexOf != -1) && (indexOf != newIndex)) {
				this.InnerList.RemoveAt(indexOf) ;
				this.InnerList.Insert(newIndex,element) ;
				OnChangeEvent() ;
			}
		}

		/// <summary>
		/// Hide all the <see cref="TextElement"/> items within the collection
		/// </summary>
		public void HideAll() {
			ShowAll(false) ;
		}

		/// <summary>
		/// Show all the <see cref="TextElement"/> items within the collection
		/// </summary>
		public void ShowAll() {
			ShowAll(true) ;
		}

		/// <summary>
		/// ShowHide all the <see cref="TextElement"/> items within the collection
		/// </summary>
		/// <param name="showAllElements"><see langword="true"/> to show all the elements, <see langword="false"/> to hide</param>
		public void ShowAll(bool showAllElements) {
			foreach(ITextElement textElement in InnerList) {
				textElement.Visible = showAllElements ;
			}

			OnChangeEvent() ;
		}

		/// <summary>
		/// Copy the <c>TextElementCollection</c> to an array
		/// </summary>
		/// <returns>The array of <see cref="TextElement"/> items in the collection</returns>
		public ITextElement [] ToArray() {
			return (ITextElement []) InnerList.ToArray(typeof(ITextElement)) ;
		}

		/// <summary>
		/// Copy the <c>TextElementCollection</c> to an array
		/// </summary>
		public void CopyTo(int index,System.Array array,int arrayIndex,int count) {
			InnerList.CopyTo(index,array,arrayIndex,count) ;
		}

		/// <summary>
		/// Get the index of the specified <see cref="TextElement"/>
		/// </summary>
		/// <param name="textElement">The <c>TextElement</c> to find</param>
		/// <returns>
		/// The index of the <see cref="TextElement"/> within the <c>TextElementCollection</c>
		/// or -1 if it is not a member
		/// </returns>
		public int IndexOf(ITextElement textElement) {
			return InnerList.IndexOf(textElement) ;
		}

		/// <summary>
		/// Get the index of the specified <see cref="TextElement"/>
		/// </summary>
		/// <param name="textElement">The <c>TextElement</c> to find</param>
		/// <returns>
		/// <see langword="true"/> if the <see cref="TextElement"/> is a member
		/// of the <c>TextElementCollection</c>
		/// </returns>
		public bool Contains(ITextElement textElement) {
			return InnerList.Contains(textElement) ;
		}
		#endregion Methods

		#region Overrides
		/// <summary>
		/// Called when a <see cref="TextElement"/> is inserted by the designer
		/// </summary>
		/// <param name="index">Index where item was inserted</param>
		/// <param name="value">The item inserted (<see cref="TextElement"/>)</param>
		protected override void OnInsertComplete(int index, object value) {
			base.OnInsertComplete (index, value);
            ((ITextElement)value).PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextElementCollection_PropertyChange);
			OnChangeEvent() ;
		}

		/// <summary>
		/// Called before the <c>TextElementCollection</c> is cleared by the designer
		/// </summary>
		protected override void OnClear() {
			foreach(ITextElement textElement in InnerList) {
                textElement.PropertyChange -= new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextElementCollection_PropertyChange);
			}

			base.OnClear ();
		}

		/// <summary>
		/// Called after the <c>TextElementCollection</c> is cleared by the designer
		/// </summary>
		protected override void OnClearComplete() {
			base.OnClearComplete ();
			OnChangeEvent() ;
		}

		/// <summary>
		/// Called when a <see cref="TextElement"/> is 'set' by the designer
		/// </summary>
		/// <param name="index">Index where item was set</param>
		/// <param name="newValue">The new item value (<see cref="TextElement"/>)</param>
		/// <param name="oldValue">The old item value (<see cref="TextElement"/>)</param>
		protected override void OnSetComplete(int index, object oldValue, object newValue) {
			base.OnSetComplete (index, oldValue, newValue);

			if (oldValue != null) {
                ((ITextElement)oldValue).PropertyChange -= new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextElementCollection_PropertyChange);
			}

			if (newValue != null) {
                ((ITextElement)newValue).PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextElementCollection_PropertyChange);
			}

			OnChangeEvent() ;
		}

		/// <summary>
		/// Called when a <see cref="TextElement"/> is removed by the designer
		/// </summary>
		/// <param name="index">Index where item was set</param>
		/// <param name="oldValue">The old item value (<see cref="TextElement"/>)</param>
		protected override void OnRemoveComplete(int index, object oldValue) {
			if (oldValue != null) {
                ((ITextElement)oldValue).PropertyChange -= new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextElementCollection_PropertyChange);
			}
		}
		#endregion Overrides

		#region Event Handlers
		/// <summary>
		/// Handle <see cref="TextElement"/> changes so they can be forwarded if neccessary
		/// </summary>
		/// <param name="sender">The <see cref="TextElement"/></param>
		/// <param name="e"><see cref="TextElementPropertyChangeEventArgs"/> describing the property change</param>
        private void TextElementCollection_PropertyChange(object sender, MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventArgs e)
        {
			OnElementChangeEvent(e) ;
		}
		#endregion Event Handlers
	}
	#endregion class TextElementCollection

	#region enum TextElementProperty
	/// <summary>
	/// Shared property enumeration for <see cref="TextStyle"/> and <see cref="TextElement"/>
	/// </summary>
	/// <remarks>
	/// This "sharing" is for historical reasons
	/// </remarks>
	public enum TextElementProperty {
		/// <summary>
		/// Font property
		/// </summary>
		FontProperty,
		/// <summary>
		/// TextColor property
		/// </summary>
		TextColorProperty,
		/// <summary>
		/// BackColor property
		/// </summary>
		BackColorProperty,
		/// <summary>
		/// Horizontal Alignment Property
		/// </summary>
		HorizontalAlignmentProperty,
		/// <summary>
		/// Vertical Alignment Property
		/// </summary>
		VerticalAlignmentProperty,
		/// <summary>
		/// See <see cref="TextStyle.SpacingAdjustment"/>
		/// </summary>
		SpacingAdjustmentProperty,
		/// <summary>
		/// See <see cref="TextStyle.ImageSet"/>
		/// </summary>
		ImageSetProperty,
		/// <summary>
		/// See <see cref="TextElement.Image"/>
		/// </summary>
		ImageProperty,
		/// <summary>
		/// <see cref="TextElement.Text"/>
		/// </summary>
		TextProperty,
		/// <summary>
		/// <see cref="TextElement.Prefix"/>
		/// </summary>
		PrefixProperty,
		/// <summary>
		/// <see cref="TextStyle.Indent"/>
		/// </summary>
		IndentProperty,
		/// <summary>
		/// <see cref="TextElement.TextStyle"/>
		/// </summary>
		StyleProperty,
		/// <summary>
		/// <see cref="TextElement.Visible"/>
		/// </summary>
		VisibleProperty,
		/// <summary>
		/// Any other property not specifically described
		/// </summary>
		OtherProperty
	}
	#endregion enum TextElementProperty

	#region TextElementPropertyChangeEventArgs (and related)
	/// <summary>
	/// Delegate signature for <see cref="TextElementProperty"/> style property 
	/// change event arguments
	/// </summary>
	public delegate void TextElementPropertyChangeEventHandler(object sender,TextElementPropertyChangeEventArgs e) ;

	/// <summary>
	/// Represents a <see cref="TextElementProperty"/> property change event
	/// </summary>
	public class TextElementPropertyChangeEventArgs : System.EventArgs {
		/// <summary>
		/// The property that changed
		/// </summary>
		private readonly TextElementProperty property ;

		/// <summary>
		/// Create a <c>TextElementPropertyChangeEventArgs</c>
		/// </summary>
		/// <param name="property"></param>
		public TextElementPropertyChangeEventArgs(TextElementProperty property) {
			this.property = property ;
		}

		/// <summary>
		/// Get the <see cref="TextElementProperty"/> that changed
		/// </summary>
		public TextElementProperty Property {
			get {
				return property ;
			}
		}
	}
	#endregion TextElementPropertyChangeEventArgs (and related)

	#region interface ITextElement
	/// <summary>
	/// Generic interface for a 'Text Element'
	/// </summary>
	public interface ITextElement {
		#region Properties
		/// <summary>
		/// Get the <see cref="System.Drawing.Font"/> of the <c>ITextElement</c>
		/// </summary>
		Font Font { get ; }

		/// <summary>
		/// Get the foreground <see cref="System.Drawing.Color"/> of the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// If this value is <see cref="Color.Empty"/> then the <see cref="Control.ForeColor"/> of
		/// the parent/owner is used</remarks>
		Color TextColor { get ; }

		/// <summary>
		/// Get the background <see cref="System.Drawing.Color"/> of the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// If this value is <see cref="Color.Empty"/> then the <see cref="Control.BackColor"/> of
		/// the parent/owner is used
		/// </remarks>
		Color BackColor { get ; }

		/// <summary>
		/// Get the 'label' text content
		/// </summary>
		/// <remarks>
		/// Use of the <c>Prefix</c> property results in output such as:
		/// <code>
		///	{Prefix}: {Text}
		/// </code>
		/// </remarks>
		String Prefix { get ; }

		/// <summary>
		/// Get the text content
		/// </summary>
		String Text { get ; }

		/// <summary>
		/// Get an <see cref="Image"/> associated with the <c>ITextElement</c>
		/// </summary>
		Image Image { get ; }

		/// <summary>
		/// Get the horizontal alignment associated with the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// The default value is <see cref="StringAlignment.Near"/>
		/// </remarks>
		StringAlignment HorzAlign { get ; }

		/// <summary>
		/// Get the vertical alignment associated with the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// The default value is <see cref="StringAlignment.Near"/>
		/// </remarks>
		StringAlignment VertAlign { get ; }

		/// <summary>
		/// Get the <see cref="TextStyle.SpacingAdjustment"/>
		/// associated with the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// Spacing adjustment effects the relative distance between this
		/// <c>ITextElement</c> and it's two nearest peer's
		/// </remarks>
		Size SpacingAdjustment { get ; }

		/// <summary>
		/// Get the left indent (in pixels) for the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// The default value for this property is zero (0)
		/// </remarks>
		int Indent { get ; }

		/// <summary>
		/// Get the shwo/hide value for the <c>ITextElement</c>
		/// </summary>
		bool Visible { get ; set ;}

		/// <summary>
		/// Get/Set an arbitrary property for a <c>ITextElement</c>
		/// </summary>
		Object this[String key] { get ; set ; }
		#endregion Properties

		#region Events
		/// <summary>
		/// Register/Unregister for property change events
		/// </summary>
		event TextElementPropertyChangeEventHandler PropertyChange ;
		#endregion Events
	}
	#endregion interface ITextElement

	/// <summary>
	/// Default implementation of <see cref="ITextElement"/>
	/// </summary>
	[Serializable]
    [TypeConverter(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.TextElementTypeConverter))]
	[DefaultProperty("Text"), DefaultEvent("PropertyChange")]
	public class TextElement : ITextElement {
		#region Fields
		/// <summary>
		/// Name value for the Text Element
		/// </summary>
		/// <remarks>
		/// Only used for {name}: {value} elements
		/// </remarks>
		private String prefix = String.Empty ;

		/// <summary>
		/// Text for the Text Element
		/// </summary>
		private String text = String.Empty ;

		/// <summary>
		/// Image associated with the element
		/// </summary>
		private Image image = null ;

		/// <summary>
		/// Maps into TextStyle.ImageSet
		/// </summary>
		private int imageIndex = -1 ;

		/// <summary>
		/// <see langword="true"/> if the <c>TextElement</c> is Visible
		/// </summary>
		private bool visible = true ;

		/// <summary>
		/// 
		/// </summary>
		[NonSerialized]
		private TextStyle textStyle = null ;

		/// <summary>
		/// Allows arbitrary properties to be associatd with TextElement
		/// </summary>
		[NonSerialized]
		private Hashtable properties ;

			#region Events
		[NonSerialized]
		private TextElementPropertyChangeEventHandler propertyChangeListeners = null ;
			#endregion Events
		#endregion Fields

		#region Constructor(s)
		/// <summary>
		/// Construct a <c>TextElement</c> with default properties
		/// </summary>
		public TextElement() : this(String.Empty) {}

		/// <summary>
		/// Construct a <c>TextElement</c> with the specified <see cref="TextElement.Text"/>
		/// </summary>
		public TextElement(String text) {
			this.text = text ;
		}

		/// <summary>
		/// Construct a <c>TextElement</c> using the specified object for the 
		/// <see cref="TextElement.Text"/>
		/// </summary>
		public TextElement(Object element) : this(element.ToString()) {}

		/// <summary>
		/// Construct a <c>TextElement</c> with the specified <see cref="Prefix"/> and <see cref="TextElement.Text"/>
		/// </summary>
		public TextElement(String prefix,Object element) : this(prefix,element.ToString(),(Image) null) {}

		/// <summary>
		/// Construct a <c>TextElement</c> with the specified <see cref="Prefix"/> and <see cref="TextElement.Text"/>
		/// </summary>
		public TextElement(String prefix,String text) : this(prefix,text,(Image) null) {}

		/// <summary>
		/// Construct a <c>TextElement</c> with the specified <see cref="Prefix"/>,
		/// <see cref="TextElement.Text"/>, and <see cref="Image"/>
		/// </summary>
		public TextElement(String prefix,String text,Image image) {
			this.prefix = prefix ;
			this.text = text ;
			this.image = image ;
		}

		/// <summary>
		/// Construct a <c>TextElement</c> with the specified <see cref="TextStyle"/>
		/// </summary>
		public TextElement(TextStyle textStyle) {
			TextStyle = textStyle ;
		}
		#endregion Constructor(s)

		#region Events
		/// <summary>
		/// Register/Unregister a property change listener
		/// </summary>
		public event TextElementPropertyChangeEventHandler PropertyChange {
			add {
				propertyChangeListeners += value ;
			}

			remove {
				propertyChangeListeners -= value ;
			}
		}
		#endregion Events

		#region Properties
		/// <summary>
		/// Get the <see cref="System.Drawing.Font"/> of the <c>TextElement</c>
		/// </summary>
		/// <remarks>
		/// If no <see cref="TextStyle"/> is associated with this <c>TextElement</c> this 
		/// property returns the default value (<see langword="null"/>)
		/// </remarks>
		[Category("Appearance"),Description("Font of the TextElement")]
		public Font Font {
			get {
				return (textStyle != null) ? textStyle.Font : null ;
			}
		}

		/// <summary>
		/// Get the foreground <see cref="System.Drawing.Color"/> of the <c>TextElement</c>
		/// </summary>
		/// <remarks>
		/// If no <see cref="TextStyle"/> is associated with this <c>TextElement</c> this 
		/// property returns the default value (<see cref="Color.Empty"/>)
		/// </remarks>
		[Category("Appearance"),Description("ForeColor of the TextElement")]
		public Color TextColor {
			get {
				return (textStyle != null) ? textStyle.TextColor : Color.Empty ;
			}
		}

		/// <summary>
		/// Get the background <see cref="System.Drawing.Color"/> of the <c>TextElement</c>
		/// </summary>
		/// <remarks>
		/// If no <see cref="TextStyle"/> is associated with this <c>TextElement</c> this 
		/// property returns the default value (<see cref="Color.Empty"/>)
		/// </remarks>
		[Category("Appearance"),Description("BackColor of the TextElement")]
		public Color BackColor {
			get {
				return (textStyle != null) ? textStyle.BackColor : Color.Empty ;
			}
		}

		/// <summary>
		/// Get the vertical alignment associated with the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// If no <see cref="TextStyle"/> is associated with this <c>TextElement</c> this 
		/// property returns the default value (<see cref="StringAlignment.Near"/>)
		/// </remarks>
		[Category("Appearance"),Description("Horizontal Alignment of the TextElement")]
		public StringAlignment HorzAlign {
			get {
				return (textStyle != null) ? textStyle.HorzAlign : StringAlignment.Near ;
			}
		}

		/// <summary>
		/// Get the vertical alignment associated with the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// If no <see cref="TextStyle"/> is associated with this <c>TextElement</c> this 
		/// property returns the default value (<see cref="StringAlignment.Near"/>)
		/// </remarks>
		[Category("Appearance"),Description("Vertical Alignment of the TextElement")]
		public StringAlignment VertAlign {
			get {
				return (textStyle != null) ? textStyle.VertAlign : StringAlignment.Near ;
			}
		}

		/// <summary>
        /// Get the <see cref="MarkMars.UI.TrueLoreXPPanel.TextStyle.SpacingAdjustment"/>
		/// associated with the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// If no <see cref="TextStyle"/> is associated with this <c>TextElement</c> this 
		/// property returns the default value (<see cref="Size.Empty"/>)
		/// </remarks>
		[Category("Appearance"),Description("Pre/Post Kerning adjustment fo the TextElement")]
		public Size SpacingAdjustment {
			get {
				return (textStyle != null) ? textStyle.SpacingAdjustment : Size.Empty ;
			}
		}

		/// <summary>
		/// Get the left indent (in pixels) for the <c>ITextElement</c>
		/// </summary>
		/// <remarks>
		/// If no <see cref="TextStyle"/> is associated with this <c>TextElement</c> this 
		/// property returns the default value (0)
		/// </remarks>
		[Category("Appearance"),Description("Left Indent (in pixels) of the TextElement")]
		public int Indent {
			get {
				return (textStyle != null) ? textStyle.Indent : 0 ;
			}
		}

		/// <summary>
		/// Get/Set the Prefix string
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.PrefixProperty"/>
		/// </remarks>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category("Appearance"),Description("Label text for TextElement")]
		public String Prefix {
			get {
				return prefix ;
			}

			set {
				if (prefix != value) {
					prefix = value ;
					OnPropertyChange(TextElementProperty.PrefixProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the text content string
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.TextProperty"/>
		/// </remarks>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category("Appearance"),Description("Text of the TextElement")]
		public String Text {
			get {
				return text ;
			}

			set {
				if (text != value) {
					text = value ;
					OnPropertyChange(TextElementProperty.TextProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set a custom <see cref="Image"/> associated with the <c>TextElement</c>
		/// </summary>
		/// <remarks>
		/// The <c>Image</c> property (if non-<see langword="null"/>) overrides the <see cref="ImageIndex"/> 
		/// property in regards to displaying an associated image. 
		/// <para>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.ImageProperty"/>
		/// </para>
		/// </remarks>
		[Browsable(true)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category("Appearance"),Description("Custom Image for the TextElement")]
		public Image Image {
			get {
				return ((textStyle != null) && (imageIndex != -1)) ? textStyle.GetImage(imageIndex) : image ;
			}

			set {
				if (image != value) {
					image = value ;
					OnPropertyChange(TextElementProperty.ImageProperty) ;
				}
			}
		}


		/// <summary>
		/// A routine to get the custom <see cref="Image"/> directly 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Image CustomImage {
			get {
				return image ;
			}
		}

		/// <summary>
        /// Get/Set the <c>ImageIndex</c> that maps to the <see cref="MarkMars.UI.TrueLoreXPPanel.TextStyle.ImageSet"/>
		/// defined by the <see cref="TextStyle"/>
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.OtherProperty"/>
		/// </remarks>
		[Browsable(true)]
		[DefaultValue(-1)]
		[Category("Appearance"),Description("ImageIndex (into TextStyle.ImageSet) for the TextElement")]
		public int ImageIndex {
			get {
				return imageIndex ;
			}

			set {
				if (imageIndex != value) {
					imageIndex = value ;
					OnPropertyChange(TextElementProperty.OtherProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the <see cref="TextStyle"/> associated with this <c>TextElement</c>
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.StyleProperty"/>
		/// </remarks>
		[Browsable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Category("Appearance"),Description("TextStyle defining formatting of the TextElement")]
		public TextStyle TextStyle {
			get {
				return textStyle ;
			}

			set {
				if (textStyle != value) {
					if (textStyle != null) {
						textStyle.PropertyChange -= new TextElementPropertyChangeEventHandler(textStyle_PropertyChange);
					}

					textStyle = value ;

					if (textStyle != null) {
						textStyle.PropertyChange += new TextElementPropertyChangeEventHandler(textStyle_PropertyChange);
					}

					OnPropertyChange(TextElementProperty.StyleProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the visibility of this <c>TextElement</c>
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.VisibleProperty"/>
		/// </remarks>
		[Browsable(true)]
		[DefaultValue(true)]
		[Category("Behavior"),Description("true if the TextElement is visible")]
		public bool Visible {
			get {
				return visible ;
			}

			set {
				if (visible != value) {
					visible = value ;
					OnPropertyChange(TextElementProperty.VisibleProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set arbitrary properties on the <c>TextElement</c>
		/// </summary>
		/// <remarks>
		/// Can be used to store application context information specific to a <c>TextElement</c>
		/// <para>
		/// This mechanism is used by <see cref="TextLayoutPanel"/> to store dynamic rectangle 
		/// information for the layout of the <c>TextElement</c>
		/// </para>
		/// </remarks>
		[Browsable(false)]
		public Object this[String key] {
			get {
				return Properties[key] ;
			}

			set {
				Properties[key] = value ;
			}
		}
		#endregion Properties

		#region Overrides
		/// <summary>
		/// Convert the <c>TextElement</c> to it's presentation/content form
		/// </summary>
		/// <returns>
		/// <c>{<see cref="Prefix"/>}: {<see cref="Text"/>}</c>, where Prefix is optional
		/// </returns>
		public override string ToString() {
			String result = String.Empty ;

			if ((Prefix != null) && (Prefix != String.Empty)) {
				result += Prefix + ": " ;
			}

			return result + Text.ToString() ;
		}

		#endregion Overrides

		#region Implementation
		/// <summary>
		/// Get the arbitrary properties Hashtable, creating it if necessary
		/// </summary>
		protected Hashtable Properties {
			get {
				lock(this) {
					if (properties == null) {
						properties = new Hashtable() ;
					}
				}

				return properties ;
			}
		}

		/// <summary>
		/// Fire a <see cref="PropertyChange"/> event to listeners
		/// </summary>
		/// <param name="property">The property that changed</param>
		protected virtual void OnPropertyChange(TextElementProperty property) {
			if (propertyChangeListeners != null) {
				propertyChangeListeners(this,new TextElementPropertyChangeEventArgs(property)) ;
			}
		}
		#endregion Implementation

		#region Event Handlers
		/// <summary>
        /// Propagate <see cref="MarkMars.UI.TrueLoreXPPanel.TextStyle.PropertyChange"/> events to our owner/parent/listeners
		/// </summary>
		/// <param name="sender">The <see cref="TextStyle"/> that changed</param>
		/// <param name="e">The property that changed</param>
		private void textStyle_PropertyChange(object sender, TextElementPropertyChangeEventArgs e) {
			if (propertyChangeListeners != null) {
				propertyChangeListeners(this,e) ;
			}
		}
		#endregion Event Handlers
	}

	/// <summary>
	/// Defines the visualization of a <see cref="TextElement"/>
	/// </summary>
	[DefaultProperty("TextColor"), DefaultEvent("PropertyChange")]
	public class TextStyle : Component {
		#region Fields
		/// <summary>
		/// Font for the style
		/// </summary>
		/// <remarks>
		/// If <see langword="null"/> then implies use of the "parent" font
		/// </remarks>
		private Font font = null ;

		/// <summary>
		/// Foreground Color for the style
		/// </summary>
		/// <remarks>
		/// If <see cref="Color.Empty"/> implies use of the "parent" 
		/// <see cref="Control.ForeColor"/>
		/// </remarks>
		private Color textColor = Color.Empty ;

		/// <summary>
		/// Background Color for the style
		/// </summary>
		/// <remarks>
		/// If <see cref="Color.Empty"/> then implies use of the 
		/// "parent" <see cref="Control.BackColor"/>
		/// </remarks>
		private Color backColor = Color.Transparent ;

		/// <summary>
		/// Horizontal Alignment of the element within its bounding rectangle
		/// </summary>
		private StringAlignment horzAlignment = StringAlignment.Near ;

		/// <summary>
		/// Vertical Alignment of an element within its bounding rectangle
		/// </summary>
		private StringAlignment vertAlignment = StringAlignment.Near ;

		/// <summary>
		/// +/- value to alter default spacing of style of parent
		/// </summary>
		private Size spacingAdjust = Size.Empty ;

		/// <summary>
		/// ImageSet associated with the style
		/// </summary>
		private ImageSet imageSet = null ;

		/// <summary>
		/// A description for the style (not used by code)
		/// </summary>
		private String description = String.Empty ;

		/// <summary>
		/// Left indent (in pixels) for the style
		/// </summary>
		private int indent = 0 ;

		#region Events
		/// <summary>
		/// <see cref="PropertyChange"/>
		/// </summary>
		[NonSerialized]
		private TextElementPropertyChangeEventHandler propertyChangeListeners = null ;
		#endregion Events
		#endregion Fields

		#region Constructor(s)
		/// <summary>
		/// Create a <see cref="TextStyle"/> with defaults
		/// </summary>
		public TextStyle() {}
		#endregion Constructor(s)

		#region Events
		/// <summary>
		/// Register/Unregister for <see cref="PropertyChange"/> events
		/// </summary>
		public event TextElementPropertyChangeEventHandler PropertyChange {
			add {
				propertyChangeListeners += value ;
			}

			remove {
				propertyChangeListeners -= value ;
			}
		}
		#endregion Events

		#region Properties
		/// <summary>
		/// Just a description string. Might be helpful if you have 50 of these things
		/// </summary>
		/// <remarks>
		/// This property has no effect on functionality
		/// </remarks>
		[Category("Misc"),Description("For informational purposes only")]
		public String Description {
			get {
				return description ;
			}

			set {
				description = value ;
			}
		}

		/// <summary>
		/// Get/Set the <see cref="System.Drawing.Font"/> for the <c>TextStyle</c>
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.FontProperty"/>
		/// </remarks>
		[Category("Appearance"),Description("Font of the TextStyle")]
		public Font Font {
			get {
				return font ;
			}

			set {
				if (font != value) {
					font = value ;
					OnPropertyChange(TextElementProperty.FontProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the Foreground <see cref="Color"/> of the <c>TextStyle</c> 
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.TextColorProperty"/>
		/// </remarks>
		[Category("Appearance"),Description("ForeColor of the TextStyle")]
		public Color TextColor {
			get {
				return textColor ;
			}

			set {
				if (textColor != value) {
					textColor = value ;
					OnPropertyChange(TextElementProperty.TextColorProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the Background <see cref="Color"/> of the <c>TextStyle</c> 
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.BackColorProperty"/>
		/// </remarks>
		[Category("Appearance"),Description("BackColor of the TextStyle")]
		public Color BackColor {
			get {
				return backColor ;
			}

			set {
				if (backColor != value) {
					backColor = value ;
					OnPropertyChange(TextElementProperty.BackColorProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the horizontal alignment of the <c>TextStyle</c> 
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.HorizontalAlignmentProperty"/>
		/// </remarks>
		[Category("Appearance"),Description("Horizontal Alignment of the TextStyle")]
		public StringAlignment HorzAlign {
			get {
				return horzAlignment ;
			}

			set {
				if (horzAlignment != value) {
					horzAlignment = value ;
					OnPropertyChange(TextElementProperty.HorizontalAlignmentProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the vertical alignment of the <c>TextStyle</c> 
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.VerticalAlignmentProperty"/>
		/// </remarks>
		[Category("Appearance"),Description("Vertical Alignment of the TextStyle")]
		public StringAlignment VertAlign {
			get {
				return vertAlignment ;
			}

			set {
				if (vertAlignment != value) {
					vertAlignment = value ;
					OnPropertyChange(TextElementProperty.VerticalAlignmentProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the <see cref="ImageSet"/> associated with the <c>TextStyle</c>
		/// </summary>
		/// <remarks>
		/// <see cref="TextElement.ImageIndex"/> is used to associate a <see cref="TextElement"/>
		/// with an <see cref="Image"/> in the <see cref="ImageSet"/>. Note that the <see cref="TextElement.Image"/> 
		/// property can override the <see cref="TextElement.ImageIndex"/> property 
		/// <para>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.ImageSetProperty"/>
		/// </para>
		/// </remarks>
		[Category("Appearance"),Description("Images of the TextStyle")]
		public ImageSet ImageSet {
			get {
				return imageSet ;
			}

			set {
				if (imageSet != value) {
					imageSet = value ;
					OnPropertyChange(TextElementProperty.ImageSetProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the Y-spacing adjustment for the <c>TextStyle</c> 
		/// </summary>
		/// <remarks>
		/// The <see cref="Size.Width"/> values specifies the +/- offset from the
		/// proceeding element, while the <see cref="Size.Height"/> specifies the
		/// +/- offset from the succeeding element
		/// <para>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.SpacingAdjustmentProperty"/>
		/// </para>
		/// </remarks>
		[Category("Appearance"),Description("Pre/Post Kerning adjustment fo the TextElement")]
		public Size SpacingAdjustment {
			get {
				return spacingAdjust ;
			}

			set {
				if (spacingAdjust != value) {
					spacingAdjust = value ;
					OnPropertyChange(TextElementProperty.SpacingAdjustmentProperty) ;
				}
			}
		}

		/// <summary>
		/// Number of "pixels" to indent element (from left)
		/// </summary>
		/// <remarks>
		/// Settting this value fires a <see cref="PropertyChange"/> <see langword="event"/>
		/// with the argument of <see cref="TextElementProperty.IndentProperty"/>
		/// </remarks>
		[Category("Appearance"),Description("Left Indent (in pixels) of the TextElement")]
		public int Indent {
			get {
				return indent ;
			}

			set {
				if (indent != value) {
					indent = value ;
					OnPropertyChange(TextElementProperty.IndentProperty) ;
				}
			}
		}
		#endregion Properties

		#region Methods
		/// <summary>
		/// Look up an image given an index
		/// </summary>
		/// <param name="index">The index into the <see cref="ImageSet"/></param>
		/// <returns>
		/// The <see cref="Image"/> or <see langword="null"/> if it cannot be found
		/// </returns>
		public Image GetImage(int index) {
			if ((imageSet != null) && (index >= 0) && (index < imageSet.Count))
				return imageSet.Images[index] ;

			return null ;
		}
		#endregion Methods

		#region Implementation
		/// <summary>
		/// Fire <see cref="PropertyChange"/> events to listeners
		/// </summary>
		/// <param name="property"></param>
		protected virtual void OnPropertyChange(TextElementProperty property) {
			if (propertyChangeListeners != null) {
				propertyChangeListeners(this,new TextElementPropertyChangeEventArgs(property)) ;
			}
		}
		#endregion Implementation
	}

	//	-----------------------------------------------------------------------
	//	The following classes are NOT used. They may be used in a future
	//	update
	//	-----------------------------------------------------------------------

#if NOTUSED
	#region class TextStyleCollection
	[Serializable]
	public class TextStyleCollection : CollectionBase {
		[NonSerialized]
		private EventHandler changeListeners = null ;
		[NonSerialized]
		private TextElementPropertyChangeEventHandler elementChangeListeners = null ;

		public TextStyleCollection() {}

		public event EventHandler Change {
			add {
				changeListeners += value ;
			}

			remove {
				changeListeners -= value ;
			}
		}

		public event TextElementPropertyChangeEventHandler ElementChange {
			add {
				elementChangeListeners += value ;
			}

			remove {
				elementChangeListeners -= value ;
			}
		}

		public TextStyle this[int index] {
			get {
				return (TextStyle) InnerList[index] ;
			}

			set {
				InnerList[index] = value ;
			}
		}

		protected virtual void OnChangeEvent() {
			if (changeListeners != null) {
				changeListeners(this,System.EventArgs.Empty) ;
			}
		}

		protected virtual void OnElementChangeEvent(TextElementPropertyChangeEventArgs eventArgs) {
			if (elementChangeListeners != null) {
				elementChangeListeners(this,eventArgs) ;
			}
		}

		public void Insert(int index,TextStyle textStyle) {
			if (textStyle != null) {
				List.Insert(index,textStyle) ;
				textStyle.PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextStyleCollection_PropertyChange);
				OnChangeEvent() ;
			}
		}

		public int Add(TextStyle textStyle) {
			if (textStyle == null)
				return -1 ;

			int result = base.InnerList.Add(textStyle) ;

			if (result != -1) {
				textStyle.PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextStyleCollection_PropertyChange);
				OnChangeEvent() ;
			}

			return result ;
		}

		public void AddRange(TextStyle [] textStyles) {
			foreach(TextStyle textStyle in textStyles) {
				if (textStyle != null) {
					textStyle.PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextStyleCollection_PropertyChange);
					base.InnerList.Add(textStyle) ;
				}
			}

			OnChangeEvent() ;
			
		}

		public TextStyle [] ToArray() {
			return (TextStyle []) InnerList.ToArray(typeof(TextStyle)) ;
		}

		public void CopyTo(int index,System.Array array,int arrayIndex,int count) {
			InnerList.CopyTo(index,array,arrayIndex,count) ;
		}

		protected override void OnInsertComplete(int index, object value) {
			base.OnInsertComplete (index, value);
			((TextStyle) value).PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextStyleCollection_PropertyChange);
			OnChangeEvent() ;
		}

		protected override void OnClear() {
			foreach(TextStyle textElement in InnerList) {
				textElement.PropertyChange -= new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextStyleCollection_PropertyChange);
			}

			base.OnClear ();
		}

		protected override void OnClearComplete() {
			base.OnClearComplete ();
			OnChangeEvent() ;
		}

		protected override void OnSetComplete(int index, object oldValue, object newValue) {
			base.OnSetComplete (index, oldValue, newValue);

			if (oldValue != null) {
				((TextStyle) oldValue).PropertyChange -= new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextStyleCollection_PropertyChange);
			}

			if (newValue != null) {
				((TextStyle) newValue).PropertyChange += new MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventHandler(TextStyleCollection_PropertyChange);
			}

			OnChangeEvent() ;
		}

		private void TextStyleCollection_PropertyChange(object sender, MarkMars.UI.TrueLoreXPPanel.TextElementPropertyChangeEventArgs e) {
			OnElementChangeEvent(e) ;
		}
	}
    #endregion class TextStyleCollection

    #region class ElementKerning
	/// <summary>
	/// Class used to specify relative location of an element in relation
	/// to preceding and succeeding elements
	/// </summary>
	[Serializable]
	public class ElementKerning {
		public static ElementKerning Empty = new ElementKerning(0.0f,0.0f) ;

		private float preAdjust = 0.0f ;
		private float postAdjust = 0.0f ;

		public ElementKerning() {}

		public ElementKerning(float preAdjust,float postAdjust) {
			this.preAdjust = preAdjust ;
			this.postAdjust = postAdjust ;
		}

		public float PreAdjust {
			get {
				return preAdjust ;
			}
		}

		public float PostAdjust {
			get {
				return preAdjust ;
			}
		}
	}
    #endregion class ElementKerning
#endif
}
