using System ;
using System.Drawing ;
using System.Drawing.Drawing2D ;
using System.ComponentModel ;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// Abstract implementaiton of ColorPair
	/// </summary>
	/// <remarks>
	/// <para>This class is <see cref="SerializableAttribute"/> but does not implement 
	/// <see cref="System.Runtime.Serialization.ISerializable"/></para>
	/// </remarks>
	[Serializable]
	public abstract class AbstractColorPair : BaseColorCollection {
		#region enum AbstractColorPairType
		/// <summary>
		/// Generic indices for <see cref="AbstractColorPair"/> entries
		/// </summary>
		public enum AbstractColorPairType {
			/// <summary>
			/// 1st color entry (of 2)
			/// </summary>
			Color1 = 0,

			/// <summary>
			/// 2nd color entry (of 2)
			/// </summary>
			Color2 = 1
		}
		#endregion enum AbstractColorPairType

		#region class ColorChangeEventArgs
		/// <summary>
		/// <see cref="AbstractColorPair.ColorChange"/> event arguments 
		/// </summary>
		public class ColorChangeEventArgs : System.EventArgs {
			/// <summary>
			/// Enumeration/Index of the changed color entry
			/// </summary>
			private readonly AbstractColorPairType colorPairType ;

			/// <summary>
			/// Construct a new ColorChangeEventArgs with the specified
			/// value
			/// </summary>
			/// <param name="colorPairType">The index of the color entry that changed</param>
			public ColorChangeEventArgs(AbstractColorPairType colorPairType) {
				this.colorPairType = colorPairType ;
			}

			/// <summary>
			/// Get the enumeration for the color entry that changed
			/// </summary>
			public AbstractColorPairType ColorPairType {
				get {
					return colorPairType ;
				}
			}

			/// <summary>
			/// <see langword="true"/> if the 1st color entry changed
			/// </summary>
			public bool IsColor1 {
				get {
					return colorPairType == AbstractColorPairType.Color1 ;
				}
			}

			/// <summary>
			/// <see langword="true"/> if the 2nd color entry changed
			/// </summary>
			public bool IsColor2 {
				get {
					return colorPairType == AbstractColorPairType.Color2 ;
				}
			}
		}
		#endregion class ColorChangeEventArgs

		#region Fields
		/// <summary>
		/// ColorChange listeners
		/// </summary>
		[NonSerialized]
		private EventHandler colorChangeListeners = null ;
		#endregion Fields

		#region Constructors
		/// <summary>
		/// Construct a new <c>AbstractColorPair</c> where both entries
		/// are equal to <see cref="Color.Empty"/>
		/// </summary>
		public AbstractColorPair() : this(Color.Empty) {}

		/// <summary>
		/// Construct a new <c>AbstractColorPair</c> where both entries
		/// are equal to the specified <see cref="Color"/>
		/// </summary>
		public AbstractColorPair(Color color) : base(2) {
			this[AbstractColorPairType.Color1] = color ;
			this[AbstractColorPairType.Color2] = color ;
		}

		/// <summary>
		/// Construct a new <c>AbstractColorPair</c> initializing
		/// the entries to the specified <see cref="Color"/> values
		/// </summary>
		public AbstractColorPair(Color color1,Color color2) : base(2) {
			this[AbstractColorPairType.Color1] = color1 ;
			this[AbstractColorPairType.Color2] = color2 ;
		}

		/// <summary>
		/// Create an <c>AbstractColorPair</c> from another 
		/// <c>BaseColorCollection</c>
		/// </summary>
		/// <param name="colors">The other <c>BaseColorCollection</c></param>
		/// <remarks>
		/// Only the 1st and 2nd color are considered. If the <c>colors</c>
		/// collection only contains a single color, both color entries are
		/// equal to that value. If zero colors are specified in colors
		/// then both values are the default, <see cref="Color.Empty"/>
		/// </remarks>
		public AbstractColorPair(BaseColorCollection colors) : base(2) {
			if (colors == null) {
				throw new ArgumentNullException("colors","BaseColorCollection cannot be null") ;
			}

			if (colors.Count > 0) {
				this[AbstractColorPairType.Color1] = colors[0] ;
			}
			
			if (colors.Count > 1) {
				this[AbstractColorPairType.Color2] = colors[1] ;
			} else {
				this[AbstractColorPairType.Color2] = Color1 ;
			}
		}
		#endregion Constructors

		#region Properties
		/// <summary>
		/// Return the 1st color entry 
		/// </summary>
		[Category("ColorPair Properties")]
		[Description("The 1st color of the color pair")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color Color1 {
			get {
				return this[AbstractColorPairType.Color1] ;
			}
			set {
				this[AbstractColorPairType.Color1] = value ;
			}
		}

		/// <summary>
		/// Return the 2nd color entry
		/// </summary>
		[Category("ColorPair Properties")]
		[Description("The 2nd color of the color pair")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color Color2 {
			get {
				return this[AbstractColorPairType.Color2] ;
			}
			set {
				this[AbstractColorPairType.Color2] = value ;
			}
		}

		/// <summary>
		/// <see langword="true"/> if the <see cref="Color1"/> and <see cref="Color2"/>
		/// color values are equivalent (i.e, a solid fill)
		/// </summary>
		[Browsable(false)]
		public override bool IsSolid {
			get {
				return Color1 == Color2 ;
			}
		}

		/// <summary>
		/// <see langword="true"/> if the <see cref="Color1"/> and <see cref="Color1"/>
		/// color values are <see cref="Color.Empty"/>
		/// </summary>
		[Browsable(false)]
		public override bool IsEmpty {
			get {
				return (Color1 == Color2) && (Color1 == Color.Empty) ;
			}
		}

		/// <summary>
		/// <see langword="true"/> if the <see cref="Color1"/> and <see cref="Color1"/>
		/// color values are <see cref="Color.Transparent"/>
		/// </summary>
		[Browsable(false)]
		public override bool IsTransparent {
			get {
				return (Color1 == Color2) && (Color1 == Color.Transparent) ;
			}
		}
		#endregion Properties

		#region Events
		/// <summary>
		/// Add/Remove a <c>ColorChange</c> event listener
		/// </summary>
		public event EventHandler ColorChange {
			add {
				colorChangeListeners += value ;
			}

			remove {
				colorChangeListeners -= value ;
			}
		}
		#endregion Events

		#region Overrides
		/// <summary>
		/// Returns the enumeration type that describes the <c>AbstractColorPair</c>
		/// <see cref="Color"/> values
		/// </summary>
		/// <returns></returns>
		protected virtual Type GetColorItemType() {
			return typeof(AbstractColorPairType) ;
		}

		/// <summary>
		/// Gets the stringified form of the color index
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		protected override String GetColorItemName(int index) {
			Type colorType = GetColorItemType() ;

			// we are expecting an enum type and the index should be valid
			// but you never know...
			if (!colorType.IsEnum || !Enum.IsDefined(colorType,index))
				return "??" ;

			return Enum.GetName(colorType,index) ;
		}

		/// <summary>
		/// Fires a <c>ColorChange</c> event to any listeners
		/// </summary>
		/// <param name="index"></param>
		protected override void OnColorChange(int index) {
			base.OnColorChange(index);

			if (colorChangeListeners != null) {
				colorChangeListeners(this,new ColorChangeEventArgs((AbstractColorPairType) index)) ;
			}
		}
		#endregion Overrides

		#region Implementation
		/// <summary>
		/// Get/Set a color entry using a <see cref="AbstractColorPairType"/>
		/// </summary>
		/// <remarks>
		///	Custom indexer that translates a <see cref="AbstractColorPairType"/>
		/// to the appropriate index for the base class		
		/// </remarks>
		private Color this[AbstractColorPairType colorPairType] {
			get {
				return base[(int) colorPairType] ;
			}

			set {
				base[(int) colorPairType] = value ;
			}
		}
		#endregion Implementation
	}

	/// <summary>
	/// ColorPair is a concrete implementation of <see cref="AbstractColorPair"/> and
	/// describes two related colors
	/// </summary>
	/// <remarks>
    /// <c>ColorPair</c> uses a customized UITypeEditor (<see cref="MarkMars.UI.TrueLoreXPPanel.Designers.ColorPairEditor"/>)
	/// for better IDE integration, as well as a custom <see cref="TypeConverter"/> 
    /// (<see cref="MarkMars.UI.TrueLoreXPPanel.Designers.ColorPairConverter"/>)
	/// <para>This class is <see cref="SerializableAttribute"/> but does not implement 
	/// <see cref="System.Runtime.Serialization.ISerializable"/></para>
	/// </remarks>
#if DEBUG
	[Editor(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.ColorPairEditor), typeof(System.Drawing.Design.UITypeEditor))]
	[TypeConverter(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.ColorPairConverter))] 
#endif
    [Serializable]
	public class ColorPair : AbstractColorPair {
		#region enum ColorPairType
		/// <summary>
		/// Enumeration used for ToString()
		/// <see cref="GetColorItemType()"/>
		/// </summary>
		enum ColorPairType {
			/// <summary>
			/// Foreground is Color1
			/// </summary>
			Foreground = 0,
			/// <summary>
			/// Background is Color2
			/// </summary>
			Background = 1
		}
		#endregion enum ColorPairType

		#region Constructors
		/// <summary>
		/// Create a <c>ColorPair</c> where both <see cref="Color"/> values
		/// are <see cref="Color.Empty"/>
		/// </summary>
		public ColorPair() : base() {}

		/// <summary>
		/// Create a <c>ColorPair</c> where both <see cref="Color"/> values
		/// are equal to the specified value
		/// </summary>
		public ColorPair(Color color) : base(color) {}

		/// <summary>
		/// Create a <c>ColorPair</c> initialized to the specified 
		/// <see cref="Color"/> values
		/// </summary>
		public ColorPair(Color foreground,Color background) : base(foreground,background) {}

		/// <summary>
		/// Create a <c>ColorPair</c> form the specified <see cref="BaseColorCollection"/>
		/// </summary>
		/// <param name="colors">The <see cref="BaseColorCollection"/> to initialize
		/// from</param>
		/// <remarks>
		/// See <see cref="AbstractColorPair(BaseColorCollection)"/> for more
		/// details
		/// </remarks>
		public ColorPair(BaseColorCollection colors) : base(colors) {}
		#endregion Constructors

		#region ICloneable Members
		/// <summary>
		/// Clone this <c>ColorPair</c>
		/// </summary>
		/// <returns>
		/// A clone of this <c>ColorPair</c>
		/// </returns>
		public override object Clone() {
			return new ColorPair(this) ;
		}
		#endregion ICloneable Members

		#region Properties
		/// <summary>
		/// Returns the foreground color (1st color) of the <c>ColorPair</c>
		/// </summary>
		[Category("ColorPair Properties")]
		[Description("The foreground color of the color pair")]
		public Color Foreground {
			get {
				return Color1 ;
			}
			set {
				Color1 = value ;
			}
		}

		/// <summary>
		/// Returns the background color (2nd color) of the <c>ColorPair</c>
		/// </summary>
		[Category("ColorPair Properties")]
		[Description("The background color of the color pair")]
		public Color Background {
			get {
				return Color2 ;
			}
			set {
				Color2 = value ;
			}
		}
		#endregion properties

		#region Designer Support
		/// <summary>
		/// Determine if the foreground color needs to be serialized (designer code
		/// generation)
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the foreground <see cref="Color"/> value
		/// is not equal to the default (<see cref="Color.Empty"/>)
		/// </returns>
		protected bool ShouldSerializeForeground() {
			return Color1 != Color.Empty;
		}

		/// <summary>
		/// Reset the foreground <see cref="Color"/> value to <see cref="Color.Empty"/>
		/// </summary>
		protected void ResetForeground() {
			Color1 = Color.Empty;
		}

		/// <summary>
		/// Determine if the background color needs to be serialized (designer code
		/// generation)
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the background <see cref="Color"/> value
		/// is not equal to the default (<see cref="Color.Empty"/>)
		/// </returns>
		protected bool ShouldSerializeBackground() {
			return Color2 != Color.Empty;
		}

		/// <summary>
		/// Reset the background <see cref="Color"/> value to <see cref="Color.Empty"/>
		/// </summary>
		protected void ResetBackground() {
			Color2 = Color.Empty;
		}
		#endregion Designer Support

		#region Overrides
		/// <summary>
		/// Override to provide our 'custom' enumeration for our color indices
		/// </summary>
		/// <returns>
		/// typeof(ColorPairType)
		/// </returns>
		protected override Type GetColorItemType() {
			return typeof(ColorPairType) ;
		}
		#endregion Overrides
	}
}
