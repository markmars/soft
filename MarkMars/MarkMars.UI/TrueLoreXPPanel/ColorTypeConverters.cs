using System ;
using System.ComponentModel ;
using System.Globalization ;
using System.Collections ;
using MarkMars.UI.TrueLoreXPPanel;

namespace MarkMars.UI.TrueLoreXPPanel.Designers
{
	/// <summary>
	/// A custom TypeConvert for GradientColor objects  
	/// </summary>
	internal class GradientColorConverter : ExpandableObjectConverter {
		#region Overrides
		/// <summary>
		/// Provide a <see cref="String"/> representation for the designer property grid
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="culture">globalization info</param>
		/// <param name="value"><see cref="GradientColor"/> to be converted</param>
		/// <param name="destinationType">Expected to be <see cref="GradientColor"/></param>
		/// <returns>
		/// A simple <see cref="String"/> representation when that type is requested
		/// </returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			if (destinationType != typeof(string) || !(value is GradientColor)) {
				return base.ConvertTo(context, culture, value, destinationType);
			}
			return "Gradient Color";
		}

		/// <summary>
		/// Construct a <see cref="GradientColor"/> from the properties in a <see cref="IDictionary"/>
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="propertyValues">The "serialized" values for the <see cref="GradientColor"/></param>
		/// <returns>
		/// A <see cref="GradientColor"/>
		/// </returns>
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
			GradientColor gradientColor = new GradientColor();
			gradientColor.Start = (System.Drawing.Color)propertyValues["Start"];
			gradientColor.End = (System.Drawing.Color)propertyValues["End"];
			return (object)gradientColor;
		}

		/// <summary>
		/// We support CreateInstance
		/// </summary>
		/// <param name="context">designer context</param>
		/// <returns>
		/// <see langword="true"/>
		/// </returns>
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
			return true;
		}
		#endregion
	}

	/// <summary>
	/// A custom TypeConvert for <see cref="ColorPair"/> objects  
	/// </summary>
	internal class ColorPairConverter : ExpandableObjectConverter {
		#region Overrides
		/// <summary>
		/// Provide a <see cref="String"/> representation for the designer property grid
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="culture">globalization info</param>
		/// <param name="value"><see cref="ColorPair"/> to be converted</param>
		/// <param name="destinationType">Expected to be <see cref="ColorPair"/></param>
		/// <returns>
		/// A simple <see cref="String"/> representation when that type is requested
		/// </returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			if (destinationType != typeof(string) || !(value is ColorPair)) {
				return base.ConvertTo(context, culture, value, destinationType);
			}
			return "Color Pair";
		}


		/// <summary>
		/// Construct a <see cref="ColorPair"/> from the properties in a <see cref="IDictionary"/>
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="propertyValues">The "serialized" values for the <see cref="ColorPair"/></param>
		/// <returns>
		/// A <see cref="ColorPair"/>
		/// </returns>
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
			ColorPair colorPair = new ColorPair();
			colorPair.Foreground = (System.Drawing.Color)propertyValues["Foreground"];
			colorPair.Background = (System.Drawing.Color)propertyValues["Background"];
			return (object)colorPair ;
		}

		/// <summary>
		/// We support CreateInstance
		/// </summary>
		/// <param name="context">designer context</param>
		/// <returns>
		/// <see langword="true"/>
		/// </returns>
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
			return true;
		}
		#endregion
	}

	/// <summary>
	/// A custom TypeConvert for <see cref="HSLColor"/> objects  
	/// </summary>
	internal class HSLColorConverter : ExpandableObjectConverter {
		#region Overrides
		/// <summary>
		/// Provide a <see cref="String"/> representation for the designer property grid
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="culture">globalization info</param>
		/// <param name="value"><see cref="HSLColor"/> to be converted</param>
		/// <param name="destinationType">Expected to be <see cref="HSLColor"/></param>
		/// <returns>
		/// A simple <see cref="String"/> representation when that type is requested
		/// </returns>
		/// <remarks>
		/// What would be better is to display the current RGB byte values as R,G,B. E.g, 234,44,128
		/// </remarks>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			if (destinationType != typeof(string) || !(value is HSLColor)) {
				return base.ConvertTo(context, culture, value, destinationType);
			}
			return "HSL Color";
		}

		/// <summary>
		/// Construct a <see cref="HSLColor"/> from the properties in a <see cref="IDictionary"/>
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="propertyValues">The "serialized" values for the <see cref="HSLColor"/></param>
		/// <returns>
		/// A <see cref="HSLColor"/>
		/// </returns>
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
			HSLColor hslColor = new HSLColor();
			hslColor.Hue = (double)propertyValues["Hue"];
			hslColor.Saturation = (double)propertyValues["Saturation"];
			hslColor.Luminance = (double)propertyValues["Luminance"];
			return (object)hslColor ;
		}

		/// <summary>
		/// We support CreateInstance
		/// </summary>
		/// <param name="context">designer context</param>
		/// <returns>
		/// <see langword="true"/>
		/// </returns>
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
			return true;
		}
		#endregion
	}

}
