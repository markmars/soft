using System ;
using System.ComponentModel ;
using System.Drawing ;
using System.Windows.Forms ;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// Designer <see cref="Component"/> that wraps an <see cref="ImageCollection"/>
	/// </summary>
	/// <remarks>
	/// <para>This class is <see cref="SerializableAttribute"/> but does not implement 
	/// <see cref="System.Runtime.Serialization.ISerializable"/></para>
	/// </remarks>
	[Serializable]
	public class ImageSet : Component {
		#region Fields
		/// <summary>
		/// The <see cref="ImageCollection"/>
		/// </summary>
		private ImageCollection images ;
		#endregion Fields

		#region Constructor(s)
		/// <summary>
		/// Create an empty <c>ImageSet</c>
		/// </summary>
		public ImageSet() {
			images = new ImageCollection() ;
		}

		/// <summary>
		/// Create an empty <c>ImageSet</c> with the specified
		/// canonical <see cref="System.Drawing.Size"/>
		/// </summary>
		public ImageSet(Size size) {
			images = new ImageCollection(size) ;
		}

		/// <summary>
		/// Create an empty <c>ImageSet</c> with the specified
		/// canonical <see cref="System.Drawing.Size"/> and
		/// transparent color mask value
		/// </summary>
		public ImageSet(Size size,Color transparentColor) {
			images = new ImageCollection(size,transparentColor) ;
		}

		/// <summary>
		/// Create an empty <c>ImageSet</c> with the specified
		/// transparent color mask value
		/// </summary>
		public ImageSet(Color transparentColor) {
			images = new ImageCollection(transparentColor) ;
		}

		/// <summary>
		/// Create an <c>ImageSet</c> from the specified <i>image strip</i>
		/// which contains the specified number of images
		/// </summary>
		/// <param name="images">The image strip</param>
		/// <param name="count">Number of images in the strip</param>
		public ImageSet(Bitmap images,int count) {
			this.images = new ImageCollection(images,count) ;
		}

		/// <summary>
		/// Create an <c>ImageSet</c> from the specified <i>image strip</i>
		/// which contains the specified number of images and transparent
		/// color mask value
		/// </summary>
		/// <param name="images">The image strip</param>
		/// <param name="count">Number of images in the strip</param>
		/// <param name="transparentColor">Transparent color mask value</param>
		public ImageSet(Bitmap images,int count,Color transparentColor) {
			this.images = new ImageCollection(images,count,transparentColor) ;
		}
		#endregion Constructor(s)

		#region Properties
		/// <summary>
		/// Get/Set the transparent mask <see cref="Color"/> value
		/// </summary>
		[Description("Transparent color for images in the ImageSet")]
		public Color TransparentColor {
			get {
				return images.TransparentColor ;
			}

			set {
				images.TransparentColor = value ;
			}
		}

		/// <summary>
		/// Get/Set the <see cref="ImageCollection"/>
		/// </summary>
		/// <remarks>
		/// Requires <see cref="DesignerSerializationVisibility.Content"/> for proper
		/// code generation (??)
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.ImageCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public ImageCollection Images {
			get {
				return images ;
			}

			set {
				if (images != value) {
					images = value ;
				}
			}
		}

		/// <summary>
		/// Get an <see cref="ImageList"/> representation of the <c>ImageSet</c>
		/// </summary>
		/// <remarks>
		/// Use <see cref="DesignerSerializationVisibility.Hidden"/> because this is not
		/// a 'real' property
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ImageList ImageList {
			get {
				return images.ImageList ;
			}
		}

		/// <summary>
		/// Return an <i>image strip</i> representation of the <c>ImageSet</c>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Image Preview {
			get {
				return images.Images ;
			}
		}

		/// <summary>
		/// Return the number of images in the <c>ImageSet</c>
		/// </summary>
		[Description("Number of images in the ImageSet")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ReadOnly(true)]
		public int Count {
			get {
				return images.Count ;
			}
		}

		/// <summary>
		/// Return the canonical <see cref="System.Drawing.Size"/> of the
		/// <c>ImageSet</c>
		/// </summary>
		[Description("Dimensions for images in the ImageSet")]
		public Size Size {
			get {
				return images.Size ;
			}

			set {
				images.Size = value ;
			}
		}

		/// <summary>
		/// Return the width of images in the <c>ImageSet</c>
		/// </summary>
		/// <remarks>
		/// Use <see cref="DesignerSerializationVisibility.Hidden"/> because this is not
		/// a 'real' property
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int Width {
			get {
				return images.Size.Width ;
			}
		}

		/// <summary>
		/// Return the height of images in the <c>ImageSet</c>
		/// </summary>
		/// <remarks>
		/// Use <see cref="DesignerSerializationVisibility.Hidden"/> because this is not
		/// a 'real' property
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int Height {
			get {
				return images.Size.Height ;
			}
		}
		#endregion Properties
	}
}
