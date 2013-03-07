using System ;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design ;
using System.Windows.Forms ;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;
using MarkMars.UI.TrueLoreXPPanel;

namespace MarkMars.UI.TrueLoreXPPanel.Designers
{
    // We only include the designers in the Debug version of the library. That way the
    // Release version only contains the appropriate redistributables (the Controls and supporting classes)
#if DEBUG
	    /// <summary>
	    /// Designer type editor for the <see cref="GradientColor"/> class
	    /// </summary>
	    /// <remarks>
	    /// Provides a preview of the <see cref="GradientColor"/> in the designer property grid
	    /// </remarks>
	    internal class GradientColorEditor : UITypeEditor {
        #region Overrides
		/// <summary>
		/// Yes we paint the value
		/// </summary>
		/// <param name="context">ignored</param>
		/// <returns><see langword="true"/> as we always show the preview</returns>
		public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
			return true;
		}

		/// <summary>
		/// Draw a preview of the <see cref="GradientColor"/>
		/// </summary>
		/// <param name="e">The paint event args providing the <see cref="Graphics"/> and bounding
		/// rectangle</param>
		public override void PaintValue(PaintValueEventArgs e) {
			GradientColor gradientColor = (GradientColor)e.Value ;
			using ( LinearGradientBrush b = gradientColor.GetBrush(e.Bounds,LinearGradientMode.Horizontal)) 
			{
				e.Graphics.FillRectangle(b, e.Bounds);
			}
		}
        #endregion
	}

	/// <summary>
	/// Designer type editor for the ColorPair class
	/// </summary>
	/// <remarks>
	/// Provides a preview of the <see cref="ColorPair"/> in the designer property grid
	/// </remarks>
	internal class ColorPairEditor : UITypeEditor {
    #region Overrides
		/// <summary>
		/// Yes we paint the value
		/// </summary>
		/// <param name="context">ignored</param>
		/// <returns><see langword="true"/> as we always show the preview</returns>
		public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
			return true;
		}

		/// <summary>
		/// Draw a preview of the <see cref="ColorPair"/>
		/// </summary>
		/// <param name="e">The paint event args providing the <see cref="Graphics"/> and bounding
		/// rectangle</param>
		public override void PaintValue(PaintValueEventArgs e) {
			ColorPair colorPair = (ColorPair)e.Value ;

			using ( SolidBrush b = new SolidBrush(colorPair.Background)) {
				e.Graphics.FillRectangle(b, e.Bounds);
			}

			// Draw the text "ab" using the Foreground/Background values from the ColorPair
			using(SolidBrush b = new SolidBrush(colorPair.Foreground)) {
				using(Font f = new Font("Arial",6)) {
					RectangleF temp = new RectangleF(e.Bounds.Left,e.Bounds.Top,e.Bounds.Height,e.Bounds.Width) ;
					temp.Inflate(-2,-2) ;

					// Set up how we want the text drawn
					StringFormat format = new StringFormat(StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap) ;
					format.Trimming = StringTrimming.EllipsisCharacter ;
					format.Alignment = StringAlignment.Center ;
					format.LineAlignment = StringAlignment.Center ;

					// save the Smoothing mode of the Graphics object so we can restore it
					SmoothingMode saveMode = e.Graphics.SmoothingMode ;
					e.Graphics.SmoothingMode = SmoothingMode.AntiAlias ;
					e.Graphics.DrawString("ab",f,b,temp,format) ;
					e.Graphics.SmoothingMode = saveMode ;
				}
			}
		}
        #endregion
	}

	/// <summary>
	/// Designer type editor for the <see cref="HSLColor"/> class
	/// </summary>
	/// <remarks>
	/// Provides a preview of the <see cref="HSLColor"/> in the designer property grid,
	///  as well as a custom editor (<see cref="System.Windows.Forms.ColorDialog"/>) for 
	///  selecting/defining a <see cref="Color"/> value
	/// </remarks>
	internal class HSLColorEditor : UITypeEditor {
    #region Overrides
		/// <summary>
		/// Yes we paint the value
		/// </summary>
		/// <param name="context">ignored</param>
		/// <returns><see langword="true"/> as we always show the preview</returns>
		public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
			return true;
		}

		/// <summary>
		/// Paint the preview
		/// </summary>
		/// <param name="e">The paint event args providing the <see cref="Graphics"/> and bounding
		/// rectangle</param>
		/// <remarks>
		/// A simple solid fill
		/// </remarks>
		public override void PaintValue(PaintValueEventArgs e) {
			HSLColor hslColor = (HSLColor)e.Value ;

			using ( SolidBrush b = new SolidBrush(hslColor.Color)) {
				e.Graphics.FillRectangle(b, e.Bounds);
			}
		}

		/// <summary>
		/// Tell the designer we use a dialog stype editor
		/// </summary>
		/// <param name="context">The designer context</param>
		/// <returns>
		/// <see cref="UITypeEditorEditStyle.Modal"/> if we have a valid context and instance, otherwise
		/// whatever the base class says
		/// </returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
			if ((context != null) && (context.Instance != null)) {
				return UITypeEditorEditStyle.Modal ;
			}

			return base.GetEditStyle (context);
		}

		/// <summary>
		/// Creates a <see cref="System.Windows.Forms.ColorDialog"/> to allow the user to
		/// select/define a <see cref="Color"/> value
		/// </summary>
		/// <param name="context">The designer context</param>
		/// <param name="provider">The designer service provided</param>
		/// <param name="value">The <see cref="HSLColor"/> value being edited</param>
		/// <returns>
		/// A new <see cref="HSLColor"/> value if the user successfully completes the dialog,
		/// otherwise the original <see cref="HSLColor"/>
		/// </returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
			if ((context != null) && (context.Instance != null) && (provider != null)) {
				IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));

				if (edSvc != null) {
					using(ColorDialog colorDialog = new ColorDialog()) {
						colorDialog.Color = ((HSLColor) value).Color ;
						colorDialog.FullOpen = true;
						if (colorDialog.ShowDialog() == DialogResult.OK) {
							return new HSLColor(colorDialog.Color) ;
						} else {
							return value ;
						}
					}
				}
			}

			return base.EditValue (context, provider, value);
		}
        #endregion
	}
#endif
}
