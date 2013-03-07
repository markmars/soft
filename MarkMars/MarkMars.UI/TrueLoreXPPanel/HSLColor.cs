using System ;
using System.Drawing ;
using System.ComponentModel ;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// A simple HSL Color value
	/// </summary>
	/// <remarks>
	/// I wrote this class mostly as an exercise. I like to be able to manipulate colors in
	/// the HSL color-space and in some cases prefer the standard color dialog to the
	/// normal .NET color drop-down. Having said that, this class is not actually used
	/// by <see cref="XPPanel"/> or any other Control/Component in the name space.
	/// <para>
	/// This class includes an implicit <see cref="Color"/> conversion operator
	/// so that it can be used transparently with GDI+ code
	/// </para>
	/// <para>
	/// This class uses a custom <see cref="System.Drawing.Design.UITypeEditor"/> which pops the standard
	/// <see cref="System.Windows.Forms.ColorDialog"/>. It also includes a custom
    /// <see cref="TypeConverter"/>. See <see cref="MarkMars.UI.TrueLoreXPPanel.Designers.HSLColorEditor"/>
    /// and <see cref="MarkMars.UI.TrueLoreXPPanel.Designers.HSLColorConverter"/> respectively.
	/// </para>
	/// <para>This class is <see cref="SerializableAttribute"/> but does not implement 
	/// <see cref="System.Runtime.Serialization.ISerializable"/></para>
	/// </remarks>
	[Serializable]
#if DEBUG
	[Editor(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.HSLColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
	[TypeConverter(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.HSLColorConverter))] 
#endif
    public class HSLColor : ICloneable {
		#region Fields
		/// <summary>
		/// Normalized hue value (0.0 -> 1.0)
		/// </summary>
		private double hue ;

		/// <summary>
		/// Normalized saturation value (0.0 -> 1.0)
		/// </summary>
		private double saturation ;

		/// <summary>
		/// Normalized luminance value (0.0 -> 1.0)
		/// </summary>
		private double luminance ;
		#endregion Fields

		#region Operators
		/// <summary>
		/// Implicitly (without casting) convert an <c>HSLColor</c> to its
		/// corresponding <see cref="Color"/> value. This makes the use
		/// of <c>HSLColor</c> transparent to GDI+ code
		/// </summary>
		/// <param name="hslColor">The <c>HSLColor to convert</c></param>
		/// <returns>
		/// The corresponding <see cref="Color"/> value
		/// </returns>
		public static implicit operator Color(HSLColor hslColor) {
			return hslColor.Color ;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Create a default <c>HSLColor</c> (black)
		/// </summary>
		public HSLColor() : this(0.0d,0.0d,0.0d) {}

		/// <summary>
		/// Create a <c>HSLColor</c> from the specified HSL values
		/// </summary>
		/// <param name="hue">Normalized hue</param>
		/// <param name="saturation">Normalized saturation</param>
		/// <param name="luminance">Normalized saturation</param>
		public HSLColor(double hue,double saturation,double luminance) {
			this.hue = hue ;
			this.saturation = saturation ;
			this.luminance = luminance ;
		}

		/// <summary>
		/// Create an <c>HSLColor</c> from the specified <see cref="Color"/>
		/// value
		/// </summary>
		/// <param name="color">The <see cref="Color"/></param>
		public HSLColor(Color color) : this(new ExtendedColor(color)) {}

		/// <summary>
		/// Create an <c>HSLColor</c> from the specified <see cref="ExtendedColor"/>
		/// </summary>
		/// <param name="color">The <see cref="ExtendedColor"/></param>
		public HSLColor(ExtendedColor color) {
			this.hue = color.Hue ;
			this.saturation = color.Saturation ;
			this.luminance = color.Luminance ;
		}
		#endregion Constructors

		#region ICloneable Members
		/// <summary>
		/// Create a clone of this <c>HSLColor</c>
		/// </summary>
		/// <returns>
		/// A clone of this <c>HSLColor</c>
		/// </returns>
		public object Clone() {
			return new HSLColor(hue,saturation,luminance) ;
		}
		#endregion ICloneable Members

		#region Properties
		/// <summary>
		/// The normalized hue component of the <c>HSLColor</c>
		/// </summary>
		[Category("Color Values"),
		Description("The Hue component of the HSL Color"),DefaultValue(0.0d)]
		public double Hue {
			get {
				return hue ;
			}

			set {
				hue = Math.Min(Math.Max(value,0.0d),1.0d) ;
			}
		}

		/// <summary>
		/// The normalized saturation component of the <c>HSLColor</c>
		/// </summary>
		[Category("Color Values"),
		Description("The Saturation component of the HSL Color"),DefaultValue(0.0d)]
		public double Saturation {
			get {
				return saturation ;
			}

			set {
				saturation = Math.Min(Math.Max(value,0.0d),1.0d) ;
			}
		}

		/// <summary>
		/// The normalized luminance component of the <c>HSLColor</c>
		/// </summary>
		[Category("Color Values"),
		Description("The Luminance component of the HSL Color"),DefaultValue(0.0d)]
		public double Luminance {
			get {
				return luminance ;
			}

			set {
				luminance = Math.Min(Math.Max(value,0.0d),1.0d) ;
			}
		}

		/// <summary>
		/// The <see cref="Color"/> value corresponding to the <c>HSLColor</c>
		/// </summary>
		[Browsable(false)]
		public Color Color {
			get {
				return new ExtendedColor(hue,saturation,luminance).Color ;
			}
		}
		#endregion Properties
	}
}
