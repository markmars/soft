using System;
using System.Drawing ;
using System.Reflection ;
using System.ComponentModel ;
using System.ComponentModel.Design ;
using System.ComponentModel.Design.Serialization ;

namespace MarkMars.UI.TrueLoreXPPanel.Designers
{
	/// <summary>
	/// TextElementTypeConverterTypeConverter provides an <see cref="InstanceDescriptor"/> used
	/// for designer code generation. This allows an alternate constructor to
	/// be specified, and in due to the current design, is required to allow TextElements to 
	/// be serializedusing Code Generation.
	/// </summary>
	/// <remarks>
	/// The constructor selected is necessary to work around other 'weird' designer issues.
	/// </remarks>
	public class TextElementTypeConverter : TypeConverter {
		/// <summary>
		/// Create an <c>TextElementTypeConverter</c>
		/// </summary>
		public TextElementTypeConverter() {}

		/// <summary>
		/// Signal that we can convert to an <see cref="InstanceDescriptor"/> (if asked...)
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="destinationType">Target type</param>
		/// <returns>
		/// <see langword="true"/> if the designer asks for an <see cref="InstanceDescriptor"/>,
		/// otherwise whatever the base class says
		/// </returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
			if (destinationType == typeof(InstanceDescriptor))
				return true ;

			return base.CanConvertTo (context, destinationType);
		}

		/// <summary>
		/// Convert to an <see cref="InstanceDescriptor"/> if requested
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="culture">globalization</param>
		/// <param name="value">the instance to convert</param>
		/// <param name="destinationType">The target type</param>
		/// <returns>
		/// An <see cref="InstanceDescriptor"/> if requested, otherwise whatever
		/// the base class returns
		/// </returns>
		/// <remarks>
		/// The constructor selected is necessary to work around 'weird' designer issues.
		/// </remarks>
		public override object ConvertTo(
			ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, 
			object value, 
			Type destinationType
			) {
			// the designer wants an InstanceDescriptor
			if ((destinationType == typeof(InstanceDescriptor)) && (value is TextElement)) {
				TextElement textElement = value as TextElement ;
				// Get our TextElement(String prefix,String text,Image image) constructor
				ConstructorInfo ctorInfo = typeof(TextElement).GetConstructor(new Type [] { typeof(String),typeof(String), typeof(Image) } ) ;
				if (ctorInfo != null) {
					return new InstanceDescriptor(ctorInfo,new object [] {textElement.Prefix,textElement.Text, textElement.CustomImage },false) ;
				}
			}
			
			return base.ConvertTo (context, culture, value, destinationType);
		}
	}
}
