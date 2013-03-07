using System;
using System.Reflection ;
using System.Drawing ;
using System.ComponentModel ;
using System.ComponentModel.Design ;
using System.ComponentModel.Design.Serialization ;

namespace MarkMars.UI.TrueLoreXPPanel.Designers
{
	/// <summary>
	/// ImageSetTypeConverter designed to provide an alternate
	/// constructor for designer code-generation.
	/// </summary>
	/// <remarks>
	/// This class is no longer used. Although the implementation is valid, various
	/// issues with the designer and <see cref="ImageSet"/> prevent this technique
	/// from working (at least out-of-the-box).
	/// </remarks>
	public class ImageSetTypeConverter : TypeConverter {
		/// <summary>
		/// Create an <c>ImageSetTypeConverter</c>
		/// </summary>
		public ImageSetTypeConverter() {}

		/// <summary>
		/// Tell the designer we can convert to <see cref="InstanceDescriptor"/> so
		/// that we can use an alternate constructor during code-generation.
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="destinationType">target conversion type</param>
		/// <returns>
		/// <see langword="true"/> if an <see cref="InstanceDescriptor"/> is requested,
		/// otherwise whatever the base class says
		/// </returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
			if (destinationType == typeof(InstanceDescriptor))
				return true ;

			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// Handle a conversion
		/// </summary>
		/// <param name="context">designer context</param>
		/// <param name="culture">globalization info</param>
		/// <param name="value">The value to be converted</param>
		/// <param name="destinationType">Target type</param>
		/// <returns>
		/// An instance of the <c>destinationType</c>
		/// </returns>
		/// <remarks>
		/// This code specifically handles conversion to a <see cref="InstanceDescriptor"/> so that
		/// the <see cref="ImageSet"/> can be created using a constructor of the form:
		/// <code>
		///		new ImageSet(<see cref="System.Drawing.Size"/>,<see cref="Color"/>) ;
		/// </code>
		/// <para>
		/// Note, the <see cref="InstanceDescriptor"/> is told that the instance may need further
		/// initialization beyond the values provided to the constructor
		/// </para>
		/// </remarks>
		public override object ConvertTo(
			ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, 
			object value, 
			Type destinationType
			) {
			if ((destinationType == typeof(InstanceDescriptor)) && (value is ImageSet)) {
				ImageSet imageSet = value as ImageSet ;
				ConstructorInfo ctorInfo = typeof(ImageSet).GetConstructor(new Type [] { typeof(Size), typeof(Color) }) ;
				if (ctorInfo != null) {
					return new InstanceDescriptor(ctorInfo,new object [] { imageSet.Size, imageSet.TransparentColor },false) ;
				}
			}
			
			return base.ConvertTo (context, culture, value, destinationType);
		}
	}
}
