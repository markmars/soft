using System ;
using System.Drawing ;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// ExtendedColor allows colors to be defined/manipulated using HSL
	/// color values, and provides an implicit conversion to <see cref="Color"/>
	/// to make its use mostly transparent
	/// </summary>
	/// <remarks>
	/// The .NET Framework exposes HSL components of <see cref="Color"/> values in
	/// a read-only manner. This class allows those values to be written.
	/// <para>This class is <see cref="SerializableAttribute"/> but does not implement 
	/// <see cref="System.Runtime.Serialization.ISerializable"/></para>
	/// </remarks>
	[Serializable]
	public class ExtendedColor : ICloneable {
		#region Constants
		/// <summary>
		/// Hues are generally described as a circle, ergo 360.0d
		/// </summary>
		public const double HueMaxValue = 360.0d ;

		/// <summary>
		/// Saturation values are normalized to the range 0.0 -> 1.0
		/// </summary>
		public const double SaturationMaxValue = 1.0d ;

		/// <summary>
		/// Brightness/Luminance values are normalized to the range 0.0 -> 1.0
		/// </summary>
		public const double BrightnessMaxValue = 1.0d ;

		/// <summary>
		/// Maximum value for an RGB component (0 -> 255)
		/// </summary>
		public const int RGBMaxValue = Byte.MaxValue ;
		#endregion Constants

		#region Fields
		/// <summary>
		/// The current <see cref="Color"/> value of the <c>ExtendedColor</c>
		/// </summary>
		private Color color ;
		#endregion Fields

		#region Static Methods
		/// <summary>
		/// Implicitly (without casting) converts an <c>ExtendedColor</c>
		/// to a <see cref="Color"/> value
		/// </summary>
		/// <param name="extendedColor">The <c>ExtendedColor</c> to convert</param>
		/// <returns>
		/// The <see cref="Color"/> value for the <c>ExtendedColor</c>
		/// </returns>
		public static implicit operator Color(ExtendedColor extendedColor) {
			return extendedColor.color ;
		}

		/// <summary>
		/// Converts an intermediate Hue value to a normalized 
		/// RGB value (0.0 -> 1.0)
		/// </summary>
		/// <param name="m1">Intermediate value 1</param>
		/// <param name="m2">Intermediate value 2</param>
		/// <param name="hue">The normalized hue value</param>
		/// <returns></returns>
		public static double HueToRGB(double m1, double m2, double hue ) {
			if (hue < 0.0d) hue += 1.0d ;
			if (hue > 1.0d) hue -= 1.0d ;

			if ((6.0*hue) < 1.0d) 
				return (m1+(m2-m1)*hue*6.0d) ;
			else if ((2.0*hue) < 1.0d  ) 
				return m2 ;
			if ((3.0d*hue) < 2.0d) 
				return (m1+(m2-m1)*((2.0d/3.0d)-hue)*6.0d) ;

			return m1 ;
		}

		/// <summary>
		/// Convert an HSL definition to its RGB equivalent
		/// </summary>
		/// <param name="hue">Normalized hue</param>
		/// <param name="saturation">Normalized saturation</param>
		/// <param name="luminance">Normalized luminance</param>
		/// <returns>
		/// The equivalent <see cref="Color"/> value for the specified HSL definition
		/// </returns>
		public static Color HSLtoRGB(double hue,double saturation,double luminance) {
			double red ;
			double green ; 
			double blue ;
			double m1, m2;

			if (saturation==0) { 
				red = green = blue = luminance ;
			} else {
				if (luminance <= 0.5d) {
					m2 = luminance*(1.0d+saturation) ;
				} else {
					m2 = luminance+saturation-luminance*saturation ;
				}

				m1 = (2.0d * luminance) - m2 ;

				red = HueToRGB(m1,m2,hue + (1.0d/3.0d));
				green = HueToRGB(m1,m2,hue);
				blue = HueToRGB(m1,m2,hue - (1.0d/3.0d));
			}

			// Create a Color value from the red, greren, and blue values by converting the normalized values
			// into the standard 'byte' value space (0-255)
			return Color.FromArgb(
				(int)Math.Round(red*(double)RGBMaxValue),
				(int)Math.Round(green*(double)RGBMaxValue),
				(int) Math.Round(blue*(double)RGBMaxValue)
				);
		}	
		#endregion Static Methods

		#region Constructors
		/// <summary>
		/// Create an <c>ExtendedColor</c> from the specified <see cref="Color"/> value
		/// </summary>
		/// <param name="color">The color</param>
		public ExtendedColor(Color color) {
			this.color = color ;
		}

		/// <summary>
		/// Create an <c>ExtendedColor</c> from another <c>ExtendedColor</c>
		/// </summary>
		/// <param name="other">The other <c>ExtendedColor</c></param>
		public ExtendedColor(ExtendedColor other) {
			this.color = other.color ;
		}

		/// <summary>
		/// Create an <c>ExtendedColor</c> from the specified <see cref="Color"/> value
		/// and specified saturation level
		/// </summary>
		/// <param name="color">The color</param>
		/// <param name="saturation">The new saturation value for the color</param>
		public ExtendedColor(Color color,double saturation) {
			this.color = color ;
			Saturation = saturation ;
		}

		/// <summary>
		/// Create an <c>ExtendedColor</c> from the specified RGB values
		/// </summary>
		/// <param name="red">The Red component</param>
		/// <param name="green">The Green component</param>
		/// <param name="blue">The Blue component</param>
		public ExtendedColor(byte red, byte green, byte blue) {
			this.color = Color.FromArgb(red,green,blue) ;
		}

		/// <summary>
		/// Create an <c>ExtendedColor</c> from the specified HSL values
		/// </summary>
		/// <param name="hue">The hue component</param>
		/// <param name="saturation">The saturation component</param>
		/// <param name="luminance">The luminance component</param>
		public ExtendedColor(double hue, double saturation, double luminance) {
			this.color = HSLtoRGB(hue,saturation,luminance) ;
		}
		#endregion Constructors

		#region IClonable Members
		/// <summary>
		/// Helper with correct return value
		/// </summary>
		/// <returns>
		/// A clone of this <c>ExtendedColor</c>
		/// </returns>
		public ExtendedColor CloneExtendedColor() {
			return new ExtendedColor(this) ;
		}

		/// <summary>
		/// Clone this <c>ExtendedColor</c>
		/// </summary>
		/// <returns>
		/// A clone of this <c>ExtendedColor</c>
		/// </returns>
		public Object Clone() {
			return CloneExtendedColor() ;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get/Set the <see cref="Color"/> value of the <c>ExtendedColor</c>
		/// </summary>
		public Color Color {
			get {
				return color ;
			}

			set {
				color = value ;
			}
		}

		/// <summary>
		/// Get/Set the Red component of the <c>ExtendedColor</c>
		/// </summary>
		public byte Red {
			get {
				return color.R ;
			}

			set {
				color = Color.FromArgb(value,color.G,color.B) ;
			}
		}

		/// <summary>
		/// Get/Set the Green component of the <c>ExtendedColor</c>
		/// </summary>
		public byte Green {
			get {
				return color.G ; 
			}

			set {
				color = Color.FromArgb(color.R,value,color.B) ;
			}
		}

		/// <summary>
		/// Get/Set the Blue component of the <c>ExtendedColor</c>
		/// </summary>
		public byte Blue {
			get {
				return color.B ; 
			}

			set {
				color = Color.FromArgb(color.R,color.G,value) ;
			}
		}

		/// <summary>
		/// Get/Set the Alpha component of the <c>ExtendedColor</c>
		/// </summary>
		public byte Alpha {
			get {
				return color.A ;
			}

			set {
				color = Color.FromArgb(value,color) ;
			}
		}

		/// <summary>
		/// Convert a normalized hue value to its common non-normalized form
		/// (0 -> 360)
		/// </summary>
		/// <param name="hue">The normalized hue value</param>
		/// <returns>
		/// The equivalent hue value in degrees
		/// </returns>
		public static int NonNormalizedHue(double hue) {
			return (int) (Math.Min(Math.Max(0.0d,hue),1.0d) * HueMaxValue) ;
		}

		/// <summary>
		/// Get/Set the hue component of the <c>ExtendedColor</c>
		/// </summary>
		/// <remarks>
		/// This value is always specified using the normalized form (0.0 -> 1.0)
		/// </remarks>
		public double Hue {
			get {
				return color.GetHue() / HueMaxValue ;
			}

			set {
				color = HSLtoRGB(
					value,
					Saturation,
					Luminance
					);
			}
		}

		/// <summary>
		/// Get/Set the hue component of the <c>ExtendedColor</c> in its
		/// non-normalized form
		/// </summary>
		public double HueNonNormalized {
			get {
				return Hue * HueMaxValue ;
			}

			set {
				color = HSLtoRGB(
					value / HueMaxValue,
					Saturation,
					Luminance
					);
			}
		}

		/// <summary>
		/// Get/Set the saturation component of the <c>ExtendedColor</c>
		/// </summary>
		public double Saturation {
			get {
				return (double) color.GetSaturation() ;
			}

			set {
				color = HSLtoRGB(
					Hue,
					value, 
					Luminance
					);
			}
		}

		/// <summary>
		/// Get/Set the luminance component of the <c>ExtendedColor</c>
		/// </summary>
		/// <remarks>
		/// Internally the .NET Framework refers to this value as Brightness
		/// (<see cref="System.Drawing.Color.GetBrightness()"/>)
		/// </remarks>
		public double Luminance {
			get {
				return (double) color.GetBrightness() ;
			}

			set {
				color = HSLtoRGB(
					Hue ,
					Saturation,
					value
					);
			}
		}

		/// <summary>
		/// <see langword="true"/> if the <c>ExtendedColor</c> is <see cref="System.Drawing.Color.Empty"/>
		/// </summary>
		public bool IsEmpty {
			get {
				return color.IsEmpty ;
			}
		}

		/// <summary>
		/// <see langword="true"/> if the <c>ExtendedColor</c> is <see cref="System.Drawing.Color.Transparent"/>
		/// </summary>
		public bool IsTransparent {
			get {
				return color == Color.Transparent ;
			}
		}
		#endregion Properties

		#region Methods
		/// <summary>
		/// Modify the current hue component of the <c>ExtendedColor</c> by
		/// a percentage
		/// </summary>
		/// <param name="hueAdjustment">The percentage</param>
		public void AdjustHue(double hueAdjustment) {
			hueAdjustment = Math.Min(Math.Max(hueAdjustment,0.0d),1.0d) ;
			Hue = (Hue * hueAdjustment) ;
		}

		/// <summary>
		/// Modify the current saturation component of the <c>ExtendedColor</c> by
		/// a percentage
		/// </summary>
		/// <param name="saturationAdjustment">The percentage</param>
		public void AdjustSaturation(double saturationAdjustment) {
			saturationAdjustment = Math.Min(Math.Max(saturationAdjustment,0.0d),1.0d) ;
			Saturation = Saturation * saturationAdjustment ;
		}

		/// <summary>
		/// Modify the current luminance component of the <c>ExtendedColor</c> by
		/// a percentage
		/// </summary>
		/// <param name="luminanceAdjustment">The percentage</param>
		public void AdjustLuminance(double luminanceAdjustment) {
			luminanceAdjustment = Math.Min(Math.Max(luminanceAdjustment,0.0d),1.0d) ;
			Luminance = Luminance * luminanceAdjustment ;
		}
		#endregion Methods
	}
}
