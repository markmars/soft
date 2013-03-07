using System;
using System.Reflection ;
using System.ComponentModel ;
using System.ComponentModel.Design ;
using System.ComponentModel.Design.Serialization ;

namespace MarkMars.UI.TrueLoreXPPanel.Designers
{
	/// <summary>
	/// XPPanelTypeConverter provides an <see cref="InstanceDescriptor"/> used
	/// for designer code generation. This allows an alternate constructor to
	/// be used that allows the value of <see cref="XPPanel.ExpandedHeight"/> to
	/// be specified prior to other properties (such as <see cref="System.Windows.Forms.Control.Size"/>)
	/// which could interfere with correct sizing
	/// </summary>
	public class XPPanelTypeConverter : TypeConverter {
		/// <summary>
		/// Create an <c>XPPanelTypeConverter</c>
		/// </summary>
		public XPPanelTypeConverter() {}

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
		public override object ConvertTo(
				ITypeDescriptorContext context, 
				System.Globalization.CultureInfo culture, 
				object value, 
				Type destinationType
				) 
		{
			// the designer wants an InstanceDescriptor
			if ((destinationType == typeof(InstanceDescriptor)) && (value is XPPanel)) {
				XPPanel xpPanel = value as XPPanel ;
				// Get our XPPanel(int) constructor
				ConstructorInfo ctorInfo = typeof(XPPanel).GetConstructor(new Type [] { typeof(int) }) ;
				if (ctorInfo != null) {
					// use this constructor and pass in the ExpandedHeight value. Use false to say that
					// initialization is NOT complete
					return new InstanceDescriptor(ctorInfo,new object [] { xpPanel.ExpandedHeight },false) ;
				}
			}
			
			return base.ConvertTo (context, culture, value, destinationType);
		}
	}
}
