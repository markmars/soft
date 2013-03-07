using System ;
using System.Collections ;
using System.ComponentModel ;
using System.Drawing ;
using System.Drawing.Imaging ;
using System.Windows.Forms ;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// <c>ImageCollection</c> provides a collection of <see cref="Image"/> items with
	/// a canonical size and pixel format
	/// </summary>
	/// <remarks>
	/// The canonical <see cref="System.Drawing.Size"/> is defined via the <see cref="Size"/>
	/// property, or the 1st <see cref="Image"/> added to the collection. If the <see cref="Size"/>
	/// property is changed <i>after</i> images are in the collection, all contained
	/// images are resized.
	/// <para>
	/// <c>ImageCollection</c> supports the <see cref="TransparentColor"/> property to
	/// allow transparent images to be created based on a specific color (typically 
	/// <see cref="Color.Magenta"/>). To add images that directly support alpha 
	/// transparency, or for no transparency, set the <see cref="TransparentColor"/>
	/// property to <see cref="Color.Empty"/>
	/// </para>
	/// <para>
	/// For this version of <c>ImageCollection</c> the pixel format is fixed
	/// at <see cref="PixelFormat.Format32bppArgb"/>
	/// </para>
	/// </remarks>
#if DEBUG
	[Editor(typeof(MarkMars.UI.TrueLoreXPPanel.Designers.ImageCollectionEditor),typeof(System.Drawing.Design.UITypeEditor))]
#endif
    [Serializable]
	public class ImageCollection : ICollection, IList {
		#region Fields
		/// <summary>
		/// Collection used to store <see cref="Image"/> items
		/// </summary>
		private ArrayList images = new ArrayList() ;

		/// <summary>
		/// Canonical size of <see cref="Image"/> items
		/// </summary>
		private Size imageSize = Size.Empty ; 

		/// <summary>
		/// <see cref="Color"/> used as a mask to make images transparent
		/// </summary>
		private Color transparentColor = Color.Empty ;
		#endregion Fields

		#region Static Methods
		/// <summary>
		/// Explicit operator (requires cast) to convert an <c>ImageCollection</c>
		/// to a <see cref="System.Windows.Forms.ImageList"/>
		/// </summary>
		/// <param name="imageCollection">The <c>ImageCollection</c> to convert</param>
		/// <returns>
		/// An <see cref="System.Windows.Forms.ImageList"/> representation of the
		/// <c>ImageCollection</c>
		/// </returns>
		public static explicit operator ImageList(ImageCollection imageCollection) {
			return imageCollection.ImageList ;
		}
		#endregion Static Methods

		#region Constructor(s)
		/// <summary>
		/// Create an empty <c>ImageCollection</c>
		/// </summary>
		public ImageCollection() {}

		/// <summary>
		/// Create an empty <c>ImageCollection</c> with the specified
		/// canonical image size
		/// </summary>
		/// <param name="size">Canonical size for images</param>
		public ImageCollection(Size size) {
			imageSize = size ;
		}

		/// <summary>
		/// Create an empty <c>ImageCollection</c> with the 
		/// specified <see cref="Color"/> value as a transparency
		/// mask
		/// </summary>
		/// <param name="transparentColor">Transparent color mask value</param>
		public ImageCollection(Color transparentColor) {
			this.transparentColor = transparentColor ;
		}

		/// <summary>
		/// Create an empty <c>ImageCollection</c> with the specified
		/// canonical image size and <see cref="Color"/> value as a transparency
		/// mask
		/// </summary>
		/// <param name="size">Canonical size for images</param>
		/// <param name="transparentColor">Transparent color mask value</param>
		public ImageCollection(Size size,Color transparentColor) {
			imageSize = size ;
            this.transparentColor = transparentColor;
		}

		/// <summary>
		/// Create an <c>ImageCollection</c> from a 'strip' of bitmap
		/// images
		/// </summary>
		/// <param name="images">The images for the <c>ImageCollection</c></param>
		/// <param name="imageCount">The number of images in the strip</param>
		public ImageCollection(Bitmap images,int imageCount) : this(images,imageCount,Color.Empty) {}

		/// <summary>
		/// Create an <c>ImageCollection</c> from a 'strip' of bitmap
		/// images
		/// </summary>
		/// <param name="images">The images for the <c>ImageCollection</c></param>
		/// <param name="imageCount">The number of images in the strip</param>
		/// <param name="transparentColor">Transparent color mask value</param>
		/// <remarks>
		/// The width of the image strip must be an integral size of <c>imageCount</c>
		/// (i.e., <c>images.Width % imageCount == 0</c>)
		/// </remarks>
		public ImageCollection(Bitmap images,int imageCount,Color transparentColor) {
			if ((images == null) && (imageCount !=0)) {
				throw new ArgumentException("ImageCollection(Bitmap,int): Image count must be 0 when bitmap is null") ;
			}

			if (images != null) {
				if ((images.Width % imageCount) != 0) {
					throw new ArgumentException("ImageCollection(Bitmap,int): Bitmap width is not even multiple of imageCount") ;
				}

				// calculate canonical size
				imageSize.Width = images.Width / imageCount ;
				imageSize.Height = images.Height ;

				Rectangle destRect = new Rectangle(0,0,imageSize.Width,imageSize.Height) ;

				for(int i=0; i < imageCount ; i++) {
					// create individual bitmap for image
					Bitmap b = new Bitmap(imageSize.Width,imageSize.Height,PixelFormat.Format32bppArgb) ;

					// draw the source part of the image into the new bitmap
					using(Graphics g = Graphics.FromImage(b)) {
						g.DrawImage(images,destRect,i*imageSize.Width,0,imageSize.Width,imageSize.Height,GraphicsUnit.Pixel) ;
					}

					// set transparent color mask if appropriate
					if (!transparentColor.IsEmpty) {
						b.MakeTransparent(transparentColor) ;
					}

					// add the bitmap
					this.images.Add(b) ;
				}
			}
		}
		#endregion Constructor(s)

		#region Properties
		/// <summary>
		/// Returns an image 'strip' containing all the images in the collection
		/// </summary>
		/// <remarks>
		/// The images are provided as a <see cref="Bitmap"/> using a single row of <see cref="Count"/> images
		/// </remarks>
		[Browsable(false)]
		public Image Images {
			get {
				if (Count == 0) {
					return null ;
				}

				// create the image strip
				Bitmap b = new Bitmap(imageSize.Width*Count,imageSize.Height) ;

				// draw each image in the collection into the image strip
				using(Graphics g = Graphics.FromImage(b)) {
					for(int i=0; i < Count ; i++) {
						Rectangle destRect = new Rectangle(i*imageSize.Width,0,imageSize.Width,imageSize.Height) ;
						g.DrawImage((Image) images[i],destRect,0,0,imageSize.Width,imageSize.Height,GraphicsUnit.Pixel) ;
					}
				}

				return b ;
			}
		}

		/// <summary>
		/// Get/Set the <see cref="Image"/> at the specified index in the
		/// collection
		/// </summary>
		[Browsable(false)]
		public Image this[int index] {
			get {
				return images[index] as Image ;
			}

			set {
				// TODO: make sure image conforms
				images[index] = value ;
			}
		}

		/// <summary>
		/// Get/Set the canonical image size for images in the <c>ImageCollection</c>
		/// </summary>
		/// <remarks>
		/// If required, each image in the collection is re-drawn at the new size
		/// </remarks>
		public Size Size {
			get {
				return imageSize ;
			}

			set {
				if (!value.Equals(imageSize)) {
					imageSize = value ;

					for(int i=0; i < images.Count ; i++) {
						images[i] = ResizeImage((Image) images[i]) ;
					}
				}
			}
		}

		/// <summary>
		/// Get/Set the transparent color make value
		/// </summary>
		/// <remarks>
		/// If necessary, the transparency mask value for each <see cref="Image"/> is
		/// set to the new value
		/// </remarks>
		public Color TransparentColor {
			get {
				return transparentColor ;
			}

			set {
				if (transparentColor != value) {
					transparentColor = value ;

					for(int i=0; i < images.Count ; i++) {
						Image image = (Image) images[i] ;

						if (image is Bitmap) {
							((Bitmap)image).MakeTransparent(transparentColor) ;
						}
					}
				}
			}
		}

		/// <summary>
		/// Create an <see cref="System.Windows.Forms.ImageList"/> from images
		/// in the collection
		/// </summary>
		public ImageList ImageList {
			get {
				ImageList imageList = new ImageList() ;
				imageList.Images.AddStrip(Images) ;
				return imageList ;
			}
		}
		#endregion Properties

		#region Implementation
		/// <summary>
		/// Make sure the image conforms to the canonical size and transparency mask
		///  as defined by the <c>ImageCollection</c>
		/// </summary>
		/// <param name="image">The image</param>
		/// <returns>
		/// An <see cref="Image"/> that conforms to the canonical size and
		/// transparent color mask for the <c>ImageCollection</c>
		/// </returns>
		internal Image ResizeImage(Image image) {
			if ((image.Size != imageSize) || (image.PixelFormat != PixelFormat.Format32bppArgb)) {
				Bitmap bitmap = new Bitmap(imageSize.Width,imageSize.Height,PixelFormat.Format32bppArgb) ;

				using(Graphics g = Graphics.FromImage(bitmap)) {
					Rectangle destRect = new Rectangle(0,0,bitmap.Width,bitmap.Height) ;
					g.DrawImage(image,destRect,0,0,image.Width,image.Height,GraphicsUnit.Pixel) ;
				}

				if (!transparentColor.IsEmpty) {
					bitmap.MakeTransparent(transparentColor) ;
				}
				
				// Dont do this. It works out badly...
				// image.Dispose() ;

				image = bitmap ;
			}

			return image ;
		}

		private Image ConformImage(object value) {
			if (value == null) {
				throw new ArgumentNullException("Expecting non-null Image/Icon value") ;
			}

			if (value is Icon) {
				Bitmap bitmap = ((Icon)value).ToBitmap();
				bitmap.MakeTransparent();
				value = bitmap ;
			}

			Image image = value as Image ;

			if (image == null) {
				throw new ArgumentException("Image type expected. Got " + value.GetType()) ;
			}

			// If we dont have a canonical size, we do now...
			if (Size == Size.Empty) {
				Size = new Size(image.Size.Width,image.Size.Height) ;
			} else {
				// conform the image
				image = ResizeImage(image) ;
			}

			return image ;
		}

		/// <summary>
		/// Internal routine for adding an <see cref="Image"/> to the
		/// <c>ImageCollection</c>
		/// </summary>
		/// <param name="image">The <see cref="Image"/> to add</param>
		/// <returns>
		/// The index where the <see cref="Image"/> was added to the
		/// <c>ImageCollection</c>
		/// </returns>
		private int AddImage(Image image) {
			// add the image to the internal collection
			return images.Add(ConformImage(image)) ;
		}
		#endregion Implementation

		#region Methods
		/// <summary>
		/// Add an Icon to the <c>ImageCollection</c>
		/// </summary>
		/// <param name="icon">The <see cref="Icon"/> add</param>
		/// <returns>
		/// The index where the <see cref="Icon"/> was added to the
		/// <c>ImageCollection</c>
		/// </returns>
		/// <remarks>
		/// The <see cref="Icon"/> must be converted to a <see cref="Bitmap"/>
		/// prior to being inserted. The <see cref="Icon"/> defines its own
		/// transparency mask so our transparency color mask is irrelavent
		/// </remarks>
		public int Add(Icon icon) {
			// add the image to the internal collection
			return images.Add(ConformImage(icon)) ;
		}

		/// <summary>
		/// Add an <see cref="Image"/> to the <c>ImageCollection</c>
		/// </summary>
		/// <param name="image">The <see cref="Image"/> to add</param>
		/// <returns>
		/// The index where the <see cref="Image"/> was added to the
		/// <c>ImageCollection</c>
		/// </returns>
		public int Add(Image image) {
			return AddImage(image) ;
		}

		/// <summary>
		/// Add an array of <see cref="Image"/> items to the <c>ImageCollection</c>
		/// </summary>
		/// <param name="images">The images to add</param>
		public void AddRange(Image [] images) {
			foreach(Image image in images) {
				AddImage(image) ;
			}
		}

		/// <summary>
		/// Insert an image at the specified point of the <c>ItemCollection</c>
		/// </summary>
		/// <param name="index">The insertion index</param>
		/// <param name="image">The <see cref="Image"/></param>
		public void Insert(int index, Image image) {
			images.Insert(index, ConformImage(image));
		}

		/// <summary>
		/// Swap the position of two images
		/// </summary>
		/// <param name="index1">Index of the 1st image</param>
		/// <param name="index2">Index of the 2nd image</param>
		public void Swap(int index1, int index2) {
			object temp = images[index1] ;
			images[index1] = images[index2] ;
			images[index2] = temp ;
		}
		#endregion Methods

		#region ICollection Members
		/// <summary>
		/// Determine if the <c>ImageCollection</c> is synchronized
		/// </summary>
		/// <remarks>
		/// See <see cref="ICollection.IsSynchronized"/>
		/// </remarks>
		[Browsable(false)]
		public bool IsSynchronized {
			get {
				return images.IsSynchronized ;
			}
		}

		/// <summary>
		/// Get the number of <see cref="Image"/> items in the <c>ImageCollection</c>
		/// </summary>
		/// <remarks>
		/// See <see cref="ICollection.Count"/>
		/// </remarks>
		[ReadOnly(true)]
		public int Count {
			get {
				return images.Count ;
			}
		}

		/// <summary>
		/// Copy the members of the <c>ImageCollection</c> to the specified
		/// <see cref="Array"/>  starting at the specified index
		/// </summary>
		/// <remarks>
		/// See <see cref="ICollection.CopyTo(Array,int)"/>
		/// </remarks>
		public void CopyTo(Array array, int index) {
			images.CopyTo(array,index) ;
		}

		/// <summary>
		/// Get the synhronization root of the <c>ImageCollection</c>
		/// </summary>
		/// <remarks>
		/// See <see cref="ICollection.SyncRoot"/>
		/// </remarks>
		[Browsable(false)]
		public object SyncRoot {
			get {
				return images.SyncRoot ;
			}
		}
		#endregion

		#region IEnumerable Members
		/// <summary>
		/// Get an <see cref="IEnumerator"/> for the items in the <c>ImageCollection</c>
		/// </summary>
		/// <remarks>
		/// See <see cref="IEnumerable.GetEnumerator()"/>
		/// </remarks>
		public IEnumerator GetEnumerator() {
			return images.GetEnumerator() ;
		}
		#endregion

		#region IList Members
		/// <summary>
		/// Determine if the <c>ImageCollection</c> is read-only
		/// </summary>
		[Browsable(false)]
		public bool IsReadOnly {
			get {
				return images.IsReadOnly ;
			}
		}

		/// <summary>
		/// Internal implementation for adding an image to
		/// the <c>ImageCollection</c>
		/// </summary>
		/// <remarks>
		/// Called by any code that casts the <c>ImageCollection</c> to
		/// an <see cref="IList"/>. This is how the <see cref="System.ComponentModel.Design.CollectionEditor"/>
		/// works...
		/// </remarks>
		/// <remarks>
		/// <see cref="IList.this[int]"/>
		/// </remarks>
		object System.Collections.IList.this[int index] {
			get {
				return images[index] ;
			}
			set {
				images[index] = ConformImage(value) ;
			}
		}

		/// <summary>
		/// Remove the <see cref="Image"/> at the specified index
		/// </summary>
		/// <param name="index">The index of the <see cref="Image"/></param>
		/// <remarks>
		/// <see cref="IList.RemoveAt(int)"/>
		/// </remarks>
		public void RemoveAt(int index) {
			images.RemoveAt(index) ;
		}

		/// <summary>
		/// Insert a value at the specified index
		/// </summary>
		/// <param name="index">The insertion index</param>
		/// <param name="value">The value to insert</param>
		/// <remarks>
		/// <see cref="IList.Insert(int,object)"/>
		/// </remarks>
		public void Insert(int index, object value) {
			Insert(index,ConformImage(value)) ;
		}

		/// <summary>
		/// Remove the specified value from the <c>ImageCollection</c>
		/// </summary>
		/// <param name="value">The value to removed</param>
		/// <remarks>
		/// <see cref="IList.Remove(object)"/>
		/// </remarks>
		public void Remove(object value) {
			images.Remove(value) ;
		}

		/// <summary>
		/// Determine if the <c>ImageCollection</c> contains
		/// the specified value
		/// </summary>
		/// <param name="value">A value to search for</param>
		/// <returns>
		/// <see langword="true"/> if the <c>ImageCollection</c> contains
		/// the specified value, <see langword="false"/> otherwise
		/// </returns>
		/// <remarks>
		/// <see cref="IList.Contains(object)"/>
		/// </remarks>
		public bool Contains(object value) {
			return images.Contains(value) ;
		}

		/// <summary>
		/// Clear all images from the <c>ImageCollection</c>
		/// </summary>
		/// <remarks>
		/// <see cref="IList.Clear()"/>
		/// </remarks>
		public void Clear() {
			// Dont do this, it doesn't work out well...
			/*foreach(Image image in images) {
				image.Dispose() ;
			}*/

			images.Clear() ;
		}

		/// <summary>
		/// Determine the index of the specified value
		/// </summary>
		/// <param name="value">A value to search for</param>
		/// <returns>
		/// The index of the value, or -1 if the value is not
		/// a membe of the <c>ImageCollection</c>
		/// </returns>
		/// <remarks>
		/// <see cref="IList.IndexOf(object)"/>
		/// </remarks>
		public int IndexOf(object value) {
			return images.IndexOf(value) ;
		}

		/// <summary>
		/// Add a value to the <c>ImageCollection</c>
		/// </summary>
		/// <param name="value">The value to add</param>
		/// <remarks>
		/// <see cref="IList.IndexOf(object)"/>
		/// </remarks>
		public int Add(object value) {
			return AddImage(ConformImage(value)) ;
		}

		/// <summary>
		/// Determine if the <c>ImageCollection</c> is a fixed size
		/// </summary>
		/// <remarks>
		/// <see cref="IList.IsFixedSize"/>
		/// </remarks>
		[Browsable(false)]
		public bool IsFixedSize {
			get {
				return images.IsFixedSize ;
			}
		}
		#endregion
	}
}
