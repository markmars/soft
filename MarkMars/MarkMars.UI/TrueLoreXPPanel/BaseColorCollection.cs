using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// <c>BaseColorCollection</c> provides a simple, array-based container for
	/// a group of <see cref="Color"/> instances
	/// </summary>
	/// <remarks>
	/// <para>This class is <see cref="SerializableAttribute"/> but does not implement 
	/// <see cref="System.Runtime.Serialization.ISerializable"/></para>
	/// <para>This class provides an implementation for <see cref="ICollection"/> but is <b>not</b> defined as 
	/// such because it causes issues with the IDE designer</para>
	/// </remarks>
	[Serializable]
	public class BaseColorCollection : ICloneable /*: ICollection*/ 
    {
		#region Fields
		/// <summary>
		/// Where each <see cref="Color"/> is stored
		/// </summary>
		private Color [] colors = null ;
		#endregion Fields

		#region Constructor(s)
		/// <summary>
		/// Construct a new <c>BaseColorCollection</c> that holds the specified
		/// number of <see cref="Color"/> items
		/// </summary>
		/// <param name="numColors">The number of colors to be stored in the collection</param>
		public BaseColorCollection(int numColors) {
			if (numColors <= 0) {
				throw new ArgumentException("The number of colors in the collection must be > 0") ;
			}

			colors = new Color[numColors] ;
		}

		/// <summary>
		/// Copy construct a new <c>BaseColorCollection</c>
		/// </summary>
		/// <param name="other">The BaseColorCollection to copy</param>
		public BaseColorCollection(BaseColorCollection other) {
			if (other == null) {
				throw new ArgumentNullException("other","Cannot copy construct a null reference") ;
			}

			this.colors = (Color []) other.colors.Clone() ;
		}
		#endregion Constructor(s)

		#region ICloneable Members
		/// <summary>
		/// Clone this <c>BaseColorCollection</c>
		/// </summary>
		/// <returns>
		/// A clone of this <c>BaseColorCollection</c>
		/// </returns>
		public virtual object Clone() {
			return new BaseColorCollection(this) ;
		}
		#endregion ICloneable Members

		#region Properties
		/// <summary>
		/// Get/Set a <see cref="Color"/> at the specified index
		/// </summary>
		/// <exception cref="IndexOutOfRangeException">If <c>index</c> is not within the bounds of
		/// the collection</exception>
		[Browsable(false)]
		public Color this[int index] {
			get {
				if ((index < 0) || (index >= colors.Length)) {
					throw new IndexOutOfRangeException("Color index (" + index + ") out-of-range") ;
				}

				return colors[index] ;
			}

			set {
				if ((index < 0) || (index >= colors.Length)) {
					throw new IndexOutOfRangeException("Color index (" + index + ") out-of-range") ;
				}

				if (colors[index] != value) {
					colors[index] = value ;
					OnColorChange(index) ;
				}
			}
		}

		/// <summary>
		/// Determine if all the <see cref="Color"/> items in the collection are
		/// equivalent to <see cref="Color.Empty"/>
		/// </summary>
		/// A color collection is <i>Empty</i> if it does not have any colors defined, 
		/// or all its colors are <see cref="Color.Empty"/>
		[Browsable(false)] 
		public virtual bool IsEmpty {
			get {
				foreach(Color color in colors) {
					if (color != Color.Empty)
						return false ;
				}

				return true ;
			}
		}

		/// <summary>
		/// Determine if all the <see cref="Color"/> items in the collection are
		/// equivalent to <see cref="Color.Empty"/>
		/// </summary>
		/// <remarks>
		/// A color collection is <i>Transparent</i> if it has at least one color, and
		/// all its colors are <see cref="Color.Transparent"/>
		/// </remarks>
		[Browsable(false)] 
		public virtual bool IsTransparent {
			get {
				if (colors.Length == 0)
					return false ;

				foreach(Color color in colors) {
					if (color != Color.Transparent)
						return false ;
				}

				return true ;
			}
		}

		/// <summary>
		/// Determine if all the <see cref="Color"/> items in the collection are
		/// equivalent to <see cref="Color.Empty"/>
		/// </summary>
		/// <remarks>
		/// A color collection is <i>solid</i> if it has at least one defined color
		/// (i.e., not <see cref="Color.Empty"/> || <see cref="Color.Transparent"/>)
		/// and all other defined colors are equivalent
		/// </remarks>
		[Browsable(false)] 
		public virtual bool IsSolid {
			get {
				if (colors.Length == 0)
					return false ;

				Color baseColor = colors[0] ;

				if (baseColor.IsEmpty || (baseColor == Color.Transparent)) 
					return true ;

				for(int i=1; i < colors.Length ; i++) {
					if (colors[i] != baseColor)
						return false ;
				}

				return true ;
			}
		}
		#endregion Properties

		#region Overrides
		/// <summary>
		/// Determine if two <c>BaseColorCollections</c> are equivalent
		/// </summary>
		/// <param name="obj">The other <c>BaseColorCollection</c></param>
		/// <returns>
		/// <see langword="true"/> if the two collections are equal, <see langword="false"/>
		/// otherwise
		/// </returns>
		public override bool Equals(object obj) {
			if ((obj == null) || !(obj is BaseColorCollection))
				return false ;

			BaseColorCollection other = obj as BaseColorCollection ;
			
			if (other == this)
				return true ;

			if (Count != other.Count)
				return false ;

			for(int i=0; i < Count ; i++) {
				if (colors[i] != other.colors[i])
					return false ;
			}

			return true ;
		}

		/// <summary>
		/// Overridden to avoid warning when we override <see cref="Equals(Object)"/>
		/// </summary>
		/// <returns>
		/// The hashcode for the <c>BaseColorCollection</c>
		/// </returns>
		public override int GetHashCode() {
			return base.GetHashCode ();
		}

		/// <summary>
		/// Custom method for converting a <see cref="Color"/> value to a string
		/// </summary>
		/// <param name="color">The color to stringify</param>
		/// <returns>
		/// String representation of the <see cref="Color"/> value.
		/// </returns>
		/// <remarks>
		/// <seealso cref="Color.ToString()"/>
		/// </remarks>
		protected String ToColorString(Color color) {
			String result = String.Empty ;

			if (color.IsNamedColor) {
				result = color.Name ;
			} else if (color.A == Byte.MaxValue) {
				result = String.Format("RGB({0},{1},{2})",color.R,color.G,color.B) ;
			} else {
				result = String.Format("ARGB({0},{1},{2},{3})",color.A,color.R,color.G,color.B) ;
			}

			return result ;
		}

		/// <summary>
		/// For sub-classes to override and provide an index specific name
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		protected virtual String GetColorItemName(int index) {
			return "" ;
		}

		/// <summary>
		/// Provide a reasonable string representation for the <see cref="Color"/> values in the
		/// collection
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			String result = "Colors{" ;

			for(int i=0; i < Count ; i++) {
				if (i != 0) {
					result += "," ;
				}

				result += GetColorItemName(i) ;
				result += ToColorString(colors[i]) ;
			}
			
			return result + "}" ;
		}

		#endregion Overrides

		#region Implementation
		/// <summary>
		/// Hook for derived classes to provide ColorChange events
		/// </summary>
		/// <param name="index">The color index that changed</param>
		/// <remarks>
		/// <c>BaseColorCollection</c> does not directly provide ColorChange
		/// events. This hook exists so that sub-classes may provide it if
		/// desired
		/// </remarks>
		protected virtual void OnColorChange(int index) {}
		#endregion Implementation

		#region ICollection Members
		/// <summary>
		/// BaseColorCollection is not synchronized
		/// </summary>
		[Browsable(false)]
		public bool IsSynchronized {
			get {
				return false;
			}
		}

		/// <summary>
		/// Return the number of items in the collection
		/// </summary>
		[Browsable(false)]
		public int Count {
			get {
				return colors.Length ;
			}
		}

		/// <summary>
		/// Copy the contents of the <c>BaseColorCollection</c> to the specified <see cref="Array"/> starting
		/// at the specified index
		/// </summary>
		/// <param name="array">The destination array</param>
		/// <param name="index">The initial index within the destination array</param>
		public void CopyTo(Array array, int index) {
			colors.CopyTo(array,index) ;
		}

		/// <summary>
		/// Return the synchronization root of the <c>BaseColorCollection</c>
		/// </summary>
		[Browsable(false)]
		public object SyncRoot {
			get {
				return colors ;
			}
		}
		#endregion

		#region IEnumerable Members
		/// <summary>
		/// Return an <see cref="IEnumerator"/> for the items in the <c>BaseColorCollection</c>
		/// </summary>
		/// <returns>
		/// An <see cref="IEnumerator"/> for the <c>colors</c> array
		/// </returns>
		public IEnumerator GetEnumerator() {
			return colors.GetEnumerator() ;
		}
		#endregion
	}
}
