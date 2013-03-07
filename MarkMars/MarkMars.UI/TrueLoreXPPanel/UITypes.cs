using System;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// Enumeration used for various panel types to describe how the
	/// background should be painted (or not painted)
	/// </summary>
	public enum BackgroundStyle {
		/// <summary>
		/// No background will be drawn
		/// </summary>
		Transparent,
		/// <summary>
		/// Background will be drawn using <see cref="System.Windows.Forms.Control.BackColor"/>
		/// </summary>
		Solid,
		/// <summary>
		/// Background will be drawn using a gradient defined by the control
		/// </summary>
		Gradient
	}
}
