using System ;
using System.ComponentModel ;
using System.Drawing ;

namespace MarkMars.UI.TrueLoreXPPanel
{
	#region enum StateImageItemTypes
	/// <summary>
	/// Enumeration of standard image states
	/// </summary>
	public enum StateImageItemTypes {
		/// <summary>
		/// Normal (enabled) image
		/// </summary>
		Normal = 0,

		/// <summary>
		/// Hot image (mouse over)
		/// </summary>
		Highlight,

		/// <summary>
		/// Pressed image (mouse down)
		/// </summary>
		Pressed,

		/// <summary>
		/// Disabled image
		/// </summary>
		Disabled
	}
	#endregion enum StateImageItemTypes

	/// <summary>
	/// Provides image index mapping for standard image states
	/// </summary>
	/// <remarks>
	/// <para>This class is <see cref="SerializableAttribute"/> but does not implement 
	/// <see cref="System.Runtime.Serialization.ISerializable"/></para>
	/// </remarks>
#if DEBUG
	// tried using custom type converters and nothing worked correctly. Just use
	// this generic .NET implementation and things work fine
	[TypeConverter(typeof(ExpandableObjectConverter))] 
#endif
	[Serializable]
	public class StateImageItems : ImageItemCollection {
		#region Constructor(s)
		/// <summary>
		/// Create an <c>StateImageItems</c> with all state values = -1
		/// </summary>
		public StateImageItems() : base((int) StateImageItemTypes.Disabled+1) {}
		#endregion Constructor(s)

		#region Properties
		/// <summary>
		/// Get/Set the <i>normal</i> state image index
		/// </summary>
		/// <remarks>Uses custom <see cref="System.Drawing.Design.UITypeEditor"/> that shows a popup of all
        /// images in the <see cref="ImageSet"/> See <see cref="MarkMars.UI.TrueLoreXPPanel.Designers.ImageMapEditor"/>
		/// </remarks>
        [Editor(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.ImageMapEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Category("Image States"),Description("Image index for the normal state"),DefaultValue(-1)]
		public int Normal {
			get {
				return this[(int) StateImageItemTypes.Normal] ;
			}

			set {
				if (value < Undefined) {
					throw new ArgumentException("Invalid image item index. Must be >= -1") ;
				}

				this[(int) StateImageItemTypes.Normal] = value ;
			}
		}

		/// <summary>
		/// Get/Set the <see cref="Image"/> associated with the <i>normal</i> state
		/// </summary>
		[Browsable(false)]
		public Image NormalImage {
			get {
				return Image((int) StateImageItemTypes.Normal) ;
			}
		}

		/// <summary>
		/// Get/Set the <i>highlight</i> state image index
		/// </summary>
		/// <remarks>Uses custom <see cref="System.Drawing.Design.UITypeEditor"/> that shows a popup of all
        /// images in the <see cref="ImageSet"/> See <see cref="MarkMars.UI.TrueLoreXPPanel.Designers.ImageMapEditor"/>
		/// </remarks>
		[Category("Image States"),Description("Image index for the highlight (mouse over) state"),DefaultValue(-1)]
        [Editor(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.ImageMapEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public int Highlight {
			get {
				return this[(int) StateImageItemTypes.Highlight] ;
			}

			set {
				if (value < Undefined) {
					throw new ArgumentException("Invalid image item index. Must be >= -1") ;
				}

				this[(int) StateImageItemTypes.Highlight] = value ;
			}
		}

		/// <summary>
		/// Get/Set the <see cref="Image"/> associated with the <i>highlight</i> state
		/// </summary>
		[Browsable(false)]
		public Image HighlightImage {
			get {
				return Image((int) StateImageItemTypes.Highlight) ;
			}
		}

		/// <summary>
		/// Get/Set the <i>pressed</i> state image index
		/// </summary>
		/// <remarks>Uses custom <see cref="System.Drawing.Design.UITypeEditor"/> that shows a popup of all
        /// images in the <see cref="ImageSet"/> See <see cref="MarkMars.UI.TrueLoreXPPanel.Designers.ImageMapEditor"/>
		/// </remarks>
		[Category("Image States"),Description("Image index for the pressed (mouse down) state"),DefaultValue(-1)]
        [Editor(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.ImageMapEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public int Pressed {
			get {
				return this[(int) StateImageItemTypes.Pressed] ;
			}

			set {
				if (value < Undefined) {
					throw new ArgumentException("Invalid image item index. Must be >= -1") ;
				}

				this[(int) StateImageItemTypes.Pressed] = value ;
			}
		}

		/// <summary>
		/// Get/Set the <see cref="Image"/> associated with the <i>pressed</i> state
		/// </summary>
		[Browsable(false)]
		public Image PressedImage {
			get {
				return Image((int) StateImageItemTypes.Pressed) ;
			}
		}

		/// <summary>
		/// Get/Set the <i>disabled</i> state image index
		/// </summary>
		/// <remarks>Uses custom <see cref="System.Drawing.Design.UITypeEditor"/> that shows a popup of all
        /// images in the <see cref="ImageSet"/>. See <see cref="MarkMars.UI.TrueLoreXPPanel.Designers.ImageMapEditor"/>
		/// </remarks>
		[Category("Image States"),Description("Image index for the disabled state"),DefaultValue(-1)]
        [Editor(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.ImageMapEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public int Disabled {
			get {
				return this[(int) StateImageItemTypes.Disabled] ;
			}

			set {
				if (value < Undefined) {
					throw new ArgumentException("Invalid image item index. Must be >= -1") ;
				}

				this[(int) StateImageItemTypes.Disabled] = value ;
			}
		}

		/// <summary>
		/// Get/Set the <see cref="Image"/> associated with the <i>disabled</i> state
		/// </summary>
		[Browsable(false)]
		public Image DisabledImage {
			get {
				return Image((int) StateImageItemTypes.Disabled) ;
			}
		}
		#endregion Properties

		#region Methods
		/// <summary>
		/// Determine if an image state has a defined image mapping
		/// </summary>
		/// <param name="state">The state</param>
		/// <returns>
		/// <see langword="true"/> if the state has an associated image mapping,
		/// <see langword="false"/> otherwise
		/// </returns>
		public bool IsDefined(StateImageItemTypes state) {
			return base.IsDefined((int) state) ;
		}

		/// <summary>
		/// Determine if an image state has a defined image mapping that maps
		/// to an actual image
		/// </summary>
		/// <param name="state">The state</param>
		/// <returns>
		/// <see langword="true"/> if the state has an associated image mapping 
		/// and valid image, <see langword="false"/> otherwise
		/// </returns>
		public bool IsImage(StateImageItemTypes state) {
			return base.IsImage((int) state) ;
		}
		#endregion Methods
	}
}
