using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics ;
using System.Drawing ;
using System.Drawing.Drawing2D ;
using System.Drawing.Imaging ;
using System.Windows.Forms ;
using System.Runtime.InteropServices ;

namespace MarkMars.UI.TrueLoreXPPanel
{
	/// <summary>
	/// ImagePanel Control displays a set of source images allowing the
	/// user to select an image using the mouse or keyboard
	/// </summary>
	/// <remarks>
	/// The <c>ImagePanel</c> can be run in two different modes:
	/// <list type="bullet">
	/// <item>Static Control</item>
	/// <item>Popup (like a context menu)</item>
	/// </list>
	/// <para>
	/// Overall functionality is the same but when in popup mode, various actions/events
	/// cause the <c>ImagePanel</c> to be hidden
	/// </para>
	/// <para>
	/// <c>ImagePanel</c> has the following primary properties:
	///	<list type="table">
	///		<listheader>
	///			<term>Property</term>
	///			<description>Purpose</description>
	///		</listheader>
	///		<item>
	///			<term><see cref="Dimensions"/></term>
	///			<description>Determines the row/column arrangement for the image grid</description>
	///		</item>
	///		<item>
	///			<term><see cref="HighlightColor"/></term>
	///			<description></description>
	///		</item>
	///		<item>
	///			<term><see cref="SelectedColor"/></term>
	///			<description></description>
	///		</item>
	///		<item>
	///			<term><see cref="GridColor"/></term>
	///			<description></description>
	///		</item>
	///		<item>
	///			<term><see cref="PanelGridSize"/></term>
	///			<description></description>
	///		</item>
	///		<item>
	///			<term><see cref="AutoSelect"/></term>
	///			<description></description>
	///		</item>
	///		<item>
	///			<term><see cref="Images"/></term>
	///			<description></description>
	///		</item>
	///	</list>
	///	</para>
	///	<para>
	///	<c>ImagePanel</c> has the following primary events:
	///	<list type="table">
	///		<listheader>
	///			<term>Event</term>
	///			<description>Purpose</description>
	///		</listheader>
	///		<item>
	///			<term><see cref="PropertyChange"/></term>
	///			<description></description>
	///		</item>
	///		<item>
	///			<term><see cref="ImageSelected"/></term>
	///			<description></description>
	///		</item>
	///	</list>
	///	</para>
	///	<para>
	///	<c>ImagePanel</c> has the following primary methods:
	///	<list type="table">
	///		<listheader>
	///			<term>Method</term>
	///			<description>Purpose</description>
	///		</listheader>
	///		<item>
	///			<term><see cref="SetImages(Bitmap,int)"/></term>
	///			<description></description>
	///		</item>
	///		<item>
	///			<term><see cref="Popup(int,int,Control)"/></term>
	///			<description></description>
	///		</item>
	///	</list>
	///	</para>
	/// </remarks>
	public class ImagePanel : ScrollableControl {
		#region ImagePanel enums
		/// <summary>
		/// Enumeration providing hint on how <see cref="ImagePanel"/> layout
		/// should appear
		/// </summary>
		public enum PanelSizeHints {
			/// <summary>
			/// Minimize the number of columns
			/// </summary>
			/// <remarks>
			/// <c>ImagePanel</c> will be vertically oriented
			/// </remarks>
			MinimizeColumns=0,
			/// <summary>
			/// Minimize the number of rows
			/// </summary>
			/// <remarks>
			/// <c>ImagePanel</c> will be horizontally oriented
			/// </remarks>
			MinimizeRows=2,

			/// <summary>
			/// Minimize both the number of rows and columns
			/// </summary>
			/// <remarks>
			/// <c>ImagePanel</c> will be a square
			/// </remarks>
			MinimizeBoth
		}

		/// <summary>
		/// Enumeration used to define X/Y coordinates
		/// for Popup mode
		/// </summary>
		public enum ImagePanelPlacement {
			/// <summary>
			/// Coordinates specify Top/Left
			/// </summary>
			TopLeft,

			/// <summary>
			/// Coordinates specify Top/Right
			/// </summary>
			TopRight,

			/// <summary>
			/// Coordinates specify Bottom/Left
			/// </summary>
			BottomLeft,

			/// <summary>
			/// Coordinates specify Bottom/Right
			/// </summary>
			BottomRight
		}

		/// <summary>
		/// Enumeration of property arguments for <see cref="ImagePanel.PropertyChange"/>
		/// </summary>
		public enum ImagePanelProperties {
			/// <summary>
			/// <see cref="ImagePanel.OnBackColorChanged"/>
			/// </summary>
			BackColorProperty,

			/// <summary>
			/// <see cref="ImagePanel.PanelGridSize"/>
			/// </summary>
			PanelGridSizeProperty,

			/// <summary>
			/// <see cref="ImagePanel.GridColor"/>
			/// </summary>
			GridColorProperty,

			/// <summary>
			/// <see cref="ImagePanel.SelectedColor"/>
			/// </summary>
			SelectedColorsProperty,

			/// <summary>
			/// <see cref="ImagePanel.SetImages(Bitmap,int)"/>
			/// </summary>
			ImagesProperty,

			/// <summary>
			/// <see cref="ImagePanel.AutoSelect"/>
			/// </summary>
			AutoSelectProperty,

			/// <summary>
			/// <see cref="ImagePanel.Dimensions"/>
			/// </summary>
			DimensionsProperty,

			/// <summary>
			/// <see cref="ImagePanel.DefaultImage"/>
			/// </summary>
			DefaultImageProperty,

			/// <summary>
			/// <see cref="BounceFactor"/>
			/// </summary>
			BounceFactorProperty,

			/// <summary>
			/// <see cref="MaxBounceRatio"/>
			/// </summary>
			MaxBounceRatioProperty,

			/// <summary>
			/// <see cref="MinBounceRatio"/>
			/// </summary>
			MinBounceRatioProperty
		}
		#endregion ImagePanel enums

		#region class PropertyChangeEventArgs
		/// <summary>
		/// <see cref="XPPanelGroup.PropertyChange"/> event arguments
		/// </summary>
		public class PropertyChangeEventArgs : System.EventArgs {
			/// <summary>
			/// The enumeration for the property that changed
			/// </summary>
			private readonly ImagePanelProperties property ;

			/// <summary>
			/// Create a <c>PropertyChangeEventArgs</c>
			/// </summary>
			/// <param name="property">The enumeration for the property that changed</param>
			public PropertyChangeEventArgs(ImagePanelProperties property) {
				this.property = property ;
			}

			/// <summary>
			/// Get the enumeration for the property that changed
			/// </summary>
			public ImagePanelProperties Property {
				get {
					return property ;
				}
			}
		}
		#endregion class PropertyChangeEventArgs

		#region Win32 Stuff
		/// <summary>
		/// Win32 function that allows us to 'sleep' if there are no application level
		/// events to be fired. This way we dont take up all the CPU cycles while
		/// in a tight loop during popup.
		/// </summary>
		/// <param name="nCount">The number of wait handles</param>
		/// <param name="pHandles">The array of wait handles</param>
		/// <param name="bWaitAll">Wait option</param>
		/// <param name="dwMilliseconds">Wait timeout</param>
		/// <param name="dwWakeMask">Event types mask</param>
		/// <returns>
		/// Various values. Primarily the wait handle that signaled, but could
		/// indicate timeout or abandonment. See Win32 API docs for complete
		/// info.
		/// </returns>
		[DllImport("user32.dll")]
		private static extern int MsgWaitForMultipleObjects(
			int		nCount,			// number of wait handles in the array
			int		pHandles,		// wait handle array
			bool	bWaitAll,		// wait option
			int		dwMilliseconds,	// time-out
			int		dwWakeMask		// input event type(s)
			);
		#endregion Win32 Stuff

		#region Constants
		private static readonly ColorPair DefaultSelectedColors = new ColorPair(Color.White,Color.DodgerBlue) ;
		#endregion Constants

		#region Fields
		/// <summary>
		/// Standard Container Control collection
		/// </summary>
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Source image strip
		/// </summary>
		/// <remarks>
		/// Contains <c>imageCount</c> images
		/// </remarks>
		private Bitmap sourceImages = null ;

		/// <summary>
		/// Number of images in the sourceImage bitmap
		/// </summary>
		private int imageCount = 0 ;

		/// <summary>
		/// Size of source images
		/// </summary>
		private Size imageSize = Size.Empty ;

		/// <summary>
		/// Size of the grid
		/// </summary>
		private Size gridSize = new Size(1,1) ;

		/// <summary>
		/// Pre-drawn, internal representation of grid w/ images
		/// </summary>
		private Bitmap panelImage = null ;

		/// <summary>
		/// Dimensions of columns and rows
		/// </summary>
		private Size dimensions = new Size(5,2) ;

		/// <summary>
		/// Combination of imageSize and gridSize values
		/// </summary>
		private Size imageUnits = Size.Empty ;

		/// <summary>
		/// True if mouse hover automatically selects an image
		/// </summary>
		private bool isAutoSelect = false ;

		/// <summary>
		/// Frame/Background color for selected image
		/// </summary>
		private ColorPair selectedColors = DefaultSelectedColors ;

		/// <summary>
		/// Color of the image grid
		/// </summary>
		private Color gridColor = Color.Black ;

		/// <summary>
		/// The default image to be selected
		/// </summary>
		private int defaultImage = -1 ;

			#region Bouncing
		/// <summary>
		/// Base increment for bouncing
		/// </summary>
		/// <remarks>
		/// See <see cref="BounceFactor"/> 
		/// </remarks>
		private float baseBounceFactorAdjust = .10f ;

		/// <summary>
		/// Maximum image bounce size (120% of org. image)
		/// </summary>
		/// <remarks>
		/// See <see cref="MaxBounceRatio"/> 
		/// </remarks>
		private float maxBounceRatio = 1.2f ;
		
		/// <summary>
		/// Minimum image bounce size (80% of org. image)
		/// </summary>
		/// <remarks>
		/// See <see cref="MinBounceRatio"/> 
		/// </remarks>
		private float minBounceRatio = .80f ;

		/// <summary>
		/// Dynamic bounce factor. Moves between min and max
		/// </summary>
		private float bounceFactor = 1.0f ;

		/// <summary>
		/// Dynamic bounce factor. Goes +/- depending on shrinking/growing
		/// </summary>
		private float bounceFactorAdjust = .10f ;
			#endregion Bouncing

			#region Events
		/// <summary>
		/// Listeners for property change events
		/// </summary>
		private EventHandler propertyChangeListeners = null ;

		/// <summary>
		/// Event handler triggered when an image is selected
		/// </summary>
		private EventHandler imageSelectedListeners = null ;

		/// <summary>
		/// Control to receive focus when a popup completes
		/// </summary>
		private Control focusControl = null ;
			#endregion Events

			#region Dynamic Fields
		/// <summary>
		/// True if the image panel is being shown as a popup
		/// </summary>
		private bool isPopup = false ;

		/// <summary>
		/// When columns are fixed, represents the "actual" number of rows in the
		/// bitmap representation of the image panel (although Rows # of rows are visible)
		/// </summary>
		private int virtualRows = 0 ;

		/// <summary>
		/// Last column selected
		/// </summary>
		private int selectedColumn = -1 ;

		/// <summary>
		/// Last row selected
		/// </summary>
		private int selectedRow = -1 ;

		/// <summary>
		/// True if the mouse is down
		/// </summary>
		private bool isMouseDown = false ;
		private System.Windows.Forms.Timer bounceTimer;

		/// <summary>
		/// True if the mouse is hovering
		/// </summary>
		private bool isMouseHover = false ;
			#endregion Dynamic Fields
		#endregion Fields

		#region Dispose (and related)
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion Dispose (and related)

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.bounceTimer = new System.Windows.Forms.Timer(this.components);
			// 
			// bounceTimer
			// 
			this.bounceTimer.Interval = 100 ;
			this.bounceTimer.Tick += new System.EventHandler(this.bounceTimer_Tick);
		}
		#endregion

		#region Constructor(s)
		/// <summary>
		/// Create an empty <c>ImagePanel</c>
		/// </summary>
		public ImagePanel() {
			//
			// Required for Windows.Forms Class Composition Designer support
			//
			InitializeComponent();

			this.AutoScroll = true ;
			// by default, panel types are not tabstops. We change this, but the
			// user can override using TabStop property
			this.TabStop = true ;
			this.isPopup = false ;
		}

		/// <summary>
		/// Create an <c>ImagePanel</c> with the specified sourc image
		/// and grid dimensions
		/// </summary>
		/// <param name="sourceImages">The source image strip</param>
		/// <param name="imageCount">The number of images in the strip</param>
		/// <param name="rows">Number of rows in the grid</param>
		/// <param name="columns">Number of columns in the grid</param>
		public ImagePanel(Bitmap sourceImages,int imageCount,int rows,int columns) {
			//
			// Required for Windows.Forms Class Composition Designer support
			//
			InitializeComponent();

			this.AutoScroll = true ;
			// by default, panel types are not tabstops. We change this, but the
			// user can override using TabStop property
			this.TabStop = true ;
			this.isPopup = false ;
			this.sourceImages = sourceImages ;
			
			Dimensions = new Size(columns,rows) ;
			this.imageCount = imageCount ;
			this.Width = PanelImage.Width ;
			this.Height = PanelImage.Height ;
		}

		/// <summary>
		/// Create an ImagePanel using the specified <see cref="ImageSet"/> for the
		/// source images
		/// </summary>
		/// <param name="imageSet">Source images</param>
		/// <remarks>
		/// The best dimensions (rows/cols) are calculated using <see cref="PanelSizeHints.MinimizeBoth"/>
		/// which produces the best square. The client rectangle for the <c>ImagePanel</c> is calculated based
		/// upon the dimensions
		/// </remarks>
		public ImagePanel(ImageSet imageSet) {
			if (imageSet == null) {
				throw new ArgumentNullException("imageSet") ;
			}

			//
			// Required for Windows.Forms Class Composition Designer support
			//
			InitializeComponent();

			this.AutoScroll = true ;
			// by default, panel types are not tabstops. We change this, but the
			// user can override using TabStop property
			this.TabStop = true ;
			this.isPopup = false ;
			this.sourceImages = (Bitmap) imageSet.Preview ;
			this.imageCount = imageSet.Count ;
			
			Dimensions = ImagePanel.CalculateBestDimensions(imageSet.Count,PanelSizeHints.MinimizeBoth) ;
			Size bestSize = this.CalculateBestClientSize() ;
			this.Width = bestSize.Width ;
			this.Height = bestSize.Height ;
		}
		#endregion Constructor(s)

		#region Properties
		/// <summary>
		/// Get/Set the <see cref="ColorPair"/> which describes the Frame and
		/// Background <see cref="Color"/> values for selected images
		/// </summary>
		public ColorPair SelectedColors {
			get {
				return selectedColors ;
			}

			set {
				if (selectedColors != value) {
					selectedColors = value ;
					OnPropertyChange(ImagePanelProperties.SelectedColorsProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the image highlight background color used when an
		/// image is pre-selected (mouse over, keyboard) and the frame color
		/// when selected (mouse down, mouse hover)
		/// </summary>
		/// <remarks>
		/// The <c>HighightColor</c> is used for both the frame of the image as
		/// well as the background color. The background color is only visible
		/// when if the image has transparency and the background color of 
		/// the image panel is not equivalent. The background/frame color
		/// used is always determined in conjunction with <see cref="SelectedColor"/>
		/// so that the frame/background colors alternates. (i.e, whats the point
		/// of a frame that is the same color as the background?)
		/// <para>
		/// This property (indirectly) sends a <see cref="PropertyChange"/> event with 
		/// <see cref="ImagePanelProperties.SelectedColorsProperty"/> 
		/// </para>
		/// <para>
		/// <seealso cref="SelectedColor"/>
		/// </para>
		/// </remarks>
		[Category("Appearance"),Description("Background color of image when highlighted"),
		DefaultValue(typeof(System.Drawing.Color),"Color.White")]
		[Obsolete]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color HighlightColor {
			get {
				return selectedColors.Foreground ;
			}

			set {
				if (selectedColors.Foreground != value) {
					selectedColors =  new ColorPair(value,SelectedColor) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the image selected frame/background color used when an
		/// image is selected (mouse down, mouse hover)
		/// </summary>
		/// <remarks>
		/// The <c>SelectedColor</c> is used for both the frame of the image as
		/// well as the background color. The background color is only visible
		/// when if the image has transparency and the background color of 
		/// the image panel is not equivalent. The background/frame color
		/// used is always determined in conjunction with <see cref="HighlightColor"/>
		/// so that the frame/background colors alternates. (i.e, whats the point
		/// of a frame that is the same color as the background?)
		/// <para>
		/// This property (indirectly) sends a <see cref="PropertyChange"/> event with 
		/// <see cref="ImagePanelProperties.SelectedColorsProperty"/> 
		/// </para>
		/// <para>
		/// <seealso cref="HighlightColor"/>
		/// </para>
		/// </remarks>
		[Category("Appearance"),Description("Background color of image when selected"),
		DefaultValue(typeof(System.Drawing.Color),"Color.DodgerBlue")]
		[Obsolete]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color SelectedColor {
			get {
				return selectedColors.Background ;
			}

			set {
				if (selectedColors.Background != value) {
					selectedColors =  new ColorPair(HighlightColor,value) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the <see cref="Color"/> value of the grid lines
		/// </summary>
		/// <para>
		/// This property sends a <see cref="PropertyChange"/> event with 
		/// <see cref="ImagePanelProperties.GridColorProperty"/> 
		/// </para>
		[Category("Appearance"),Description("Color of the image grid"),
		DefaultValue(typeof(System.Drawing.Color),"Color.Black")]
		public Color GridColor {
			get {
				return gridColor ;
			}

			set {
				if (gridColor != value) {
					gridColor =  value ;
					OnPropertyChange(ImagePanelProperties.GridColorProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the line thickness of the <c>ImagePanel</c> grid
		/// </summary>
		/// <remarks>
		/// <para>This method fires a <see cref="PropertyChange"/> event with the
		/// <see cref="ImagePanelProperties.PanelGridSizeProperty"/></para>
		/// </remarks>
		[Category("Appearance"),Description("Size of image grid lines")]
		public Size PanelGridSize {
			get {
				return gridSize ;
			}

			set {
				if (gridSize != value) {
					gridSize = value ;
					OnPropertyChange(ImagePanelProperties.PanelGridSizeProperty) ;
				}
			}
		}

		/// <summary>
		/// Determine if this property should be serialized by designer
		/// code generation
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the property does not equal its default value
		/// </returns>
		protected bool ShouldSerializePanelGridSize() {
			return (gridSize.Width != 1) || (gridSize.Height != 1) ;
		}

		/// <summary>
		/// Reset this property to its default value
		/// </summary>
		protected void ResetPanelGridSize() {
			gridSize.Width =1 ;
			gridSize.Height =1 ;
		}

		/// <summary>
		/// Get/Set the dimensions of the <c>ImagePanel</c> grid
		/// </summary>
		/// <remarks>
		/// The ImagePanel grid is define in terms of rows/columns where 
		/// <see cref="System.Drawing.Size.Width"/> is the number of columns, and
		/// <see cref="System.Drawing.Size.Height"/> is the number of rows
		/// <para>This method fires a <see cref="PropertyChange"/> event with the
		/// <see cref="ImagePanelProperties.DimensionsProperty"/></para>
		/// </remarks>
		[Category("Appearance"),Description("Number of rows/columns in the image grid"),DefaultValue(2)]
		public Size Dimensions {
			get {
				return dimensions ;
			}
			set {
				if (value.Width < 1) {
					throw new ArgumentException("Columns must be >= 1") ;
				}

				if (value.Height < 1) {
					throw new ArgumentException("Rows must be >= 1") ;
				}

				if (dimensions != value) {
					dimensions = value ;
					OnPropertyChange(ImagePanelProperties.DimensionsProperty) ;
				}
			}
		}

		/// <summary>
		/// Determine if this property should be serialized by designer
		/// code generation
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the property does not equal its default value
		/// </returns>
		protected bool ShouldSerializeDimensions() {
			return (dimensions.Height != 2) || (dimensions.Width != 5) ;
		}

		/// <summary>
		/// Reset this property to its default value
		/// </summary>
		protected void ResetDimensions() {
			dimensions.Width = 5 ;
			dimensions.Height = 2 ;
		}


		/// <summary>
		/// Get/Set automatic selection of image on mouse hover
		/// </summary>
		/// <remarks>
		/// <para>This method fires a <see cref="PropertyChange"/> event with the
		/// <see cref="ImagePanelProperties.AutoSelectProperty"/></para>
		/// </remarks>
		[Category("Behaviour"),Description("True if mouse hover automatically selects image"),DefaultValue(false)]
		public bool AutoSelect {
			get {
				return isAutoSelect ;
			}

			set {
				if (value != isAutoSelect) {
					isAutoSelect = value ;
					OnPropertyChange(ImagePanelProperties.AutoSelectProperty) ;
				}
			}
		}

		/// <summary>
		/// Get the <see cref="Bitmap"/> for the source images
		/// </summary>
		[Browsable(false)]
		public Bitmap Images {
			get {
				return sourceImages ;
			}
		}

		/// <summary>
		/// Get/Set the default image selection
		/// </summary>
		/// <remarks>
		/// <para>This method fires a <see cref="PropertyChange"/> event with the
		/// <see cref="ImagePanelProperties.DefaultImageProperty"/></para>
		/// </remarks>
		public int DefaultImage {
			get {
				return defaultImage ;
			}
			set {
				if ((value != defaultImage) && (value < imageCount)) {
					defaultImage = Math.Max(value,-1) ;
					OnPropertyChange(ImagePanelProperties.DefaultImageProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the increment/decrement unit when bouncing an image
		/// </summary>
		/// <remarks>
		/// <c>BounceFactor</c> must be between 0.0 and 1.0 inclusive
		/// </remarks>
		[Category("Behaviour"),Description("Image size increment/decrement during image bounce"),
		DefaultValue(.10f)]
		public float BounceFactor {
			get {
				return baseBounceFactorAdjust ;
			}

			set {
				if ((value < 0.0f) || (value > 1.0)) {
					throw new ArgumentException("BounceFactor must be between 0.0 and 1.0 (inclusive)") ;
				}

				if (baseBounceFactorAdjust != value) {
					baseBounceFactorAdjust = value ;
					OnPropertyChange(ImagePanelProperties.BounceFactorProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the maximum image scaling during bouncing
		/// </summary>
		/// <remarks>
		/// <c>MaxBounceRatio</c> must be &gt;= 1.0
		/// </remarks>
		[Category("Behaviour"),Description("Maximum percentage increase of image during bounce"),
		DefaultValue(1.3f)]
		public float MaxBounceRatio {
			get {
				return maxBounceRatio ;
			}

			set {
				if (value < 1.0f) {
					throw new ArgumentException("MaxBounceRatio must be >= 1.0") ;
				}

				if (maxBounceRatio != value) {
					maxBounceRatio = value ;
					OnPropertyChange(ImagePanelProperties.MaxBounceRatioProperty) ;
				}
			}
		}

		/// <summary>
		/// Get/Set the minimum image scaling during bouncing
		/// </summary>
		/// <remarks>
		/// <c>MinBounceRatio</c> must be &lt;= 1.0
		/// </remarks>
		[Category("Behaviour"),Description("Minimum percentage increase of image during bounce"),
		DefaultValue(0.8f)]
		public float MinBounceRatio {
			get {
				return minBounceRatio ;
			}

			set {
				if (value > 1.0f) {
					throw new ArgumentException("MinBounceRatio must be <= 1.0") ;
				}

				if (minBounceRatio != value) {
					minBounceRatio = value ;
					OnPropertyChange(ImagePanelProperties.MinBounceRatioProperty) ;
				}
			}
		}
		#endregion Properties

		#region Events
		/// <summary>
		/// Add/Remove <c>PropertyChange</c> event listeners
		/// </summary>
		public event EventHandler PropertyChange {
			add {
				propertyChangeListeners += value ;
			}

			remove {
				propertyChangeListeners -= value ;
			}
		}

		/// <summary>
		/// Add/Remove <c>ImageSelected</c> event listeners
		/// </summary>
		public event EventHandler ImageSelected {
			add {
				imageSelectedListeners += value ;
			}

			remove {
				imageSelectedListeners -= value ;
			}
		}
		#endregion Events

		#region Methods
		/// <summary>
		/// Set the source images for the <c>ImagePanel</c>
		/// </summary>
		/// <param name="images">The source image strip</param>
		/// <param name="imageCount">The number of images in the image strip</param>
		/// <remarks>
		/// <para>This method fires a <see cref="PropertyChange"/> event with the
		/// <see cref="ImagePanelProperties.ImagesProperty"/></para>
		/// </remarks>
		public void SetImages(Bitmap images, int imageCount) {
			if ((images != null) && (imageCount <= 0)) {
				throw new ArgumentException("imageCount must be > 0") ;
			}

			sourceImages = images ;
			this.imageCount = imageCount ;

			OnPropertyChange(ImagePanelProperties.ImagesProperty) ;
		}

		/// <summary>
		/// Initializes the source images from the specified <see cref="ImageSet"/>
		/// </summary>
		/// <param name="imageSet">The source image collection</param>
		/// <remarks>
		/// <para>This method (indirectly) fires a <see cref="PropertyChange"/> event with the
		/// <see cref="ImagePanelProperties.ImagesProperty"/></para>
		/// </remarks>
		public void SetImages(ImageSet imageSet) {
			SetImages((Bitmap) imageSet.Preview,imageSet.Count) ;
		}

		///	<summary>
		/// Initializes the source images from the specified <see cref="ImageList"/>
		///	</summary>
		///	<param name="imageList">The source image list</param>
		/// <remarks>
		/// <para>This method (indirectly) fires a <see cref="PropertyChange"/> event with the
		/// <see cref="ImagePanelProperties.ImagesProperty"/></para>
		/// </remarks>
		public void SetImages(ImageList imageList) {
			ImageSet imageSet = new ImageSet(imageList.ImageSize,imageList.TransparentColor) ;

			foreach(Image image in imageList.Images) {
				imageSet.Images.Add(image) ;
			}

			SetImages(imageSet) ;
		}

		/// <summary>
		/// Show the <c>ImagePanel</c> as a popup with its origin
		/// at the specified X,Y coordinates (relative to its parent)
		/// </summary>
		/// <param name="x">Left</param>
		/// <param name="y">Top</param>
		/// <param name="placement">Specifies relative meaning of X/Y coordinates</param>
		/// <param name="focusMe">Control to receive the focus when the
		/// popup is complete (or <see langword="null"/>)</param>
		/// <returns>
		/// The image index selected, or -1 if canceled
		/// </returns>
		public int Popup(int x, int y, ImagePanelPlacement placement,Control focusMe) {
			if (Visible) {
				return -1 ;
			}

			return DoPopup(x,y,placement,focusMe) ;
		}

		/// <summary>
		/// Show the <c>ImagePanel</c> as a popup with its origin
		/// at the specified X,Y coordinates (relative to its parent)
		/// </summary>
		/// <param name="x">Left</param>
		/// <param name="y">Top</param>
		/// <param name="focusMe">Control to receive the focus when the
		/// popup is complete (or <see langword="null"/>)</param>
		/// <returns>
		/// The image index selected, or -1 if canceled
		/// </returns>
		/// <remarks>
		/// X/Y coordinates are <see cref="ImagePanelPlacement.TopLeft"/>
		/// </remarks>
		public int Popup(int x,int y,Control focusMe) {
			if (Visible) {
				return -1 ;
			}

			return DoPopup(x,y,ImagePanelPlacement.TopLeft,focusMe) ;
		}

		/// <summary>
		/// Show the <c>ImagePanel</c> as a popup with its origin
		/// at the specified X,Y coordinates (relative to its parent)
		/// with dimensions suggested by the <see cref="PanelSizeHints"/>
		/// </summary>
		/// <param name="x">Left</param>
		/// <param name="y">Top</param>
		/// <param name="placement">Relative meaning of X/Y coordinates</param>
		/// <param name="panelSizeHint">Hint about panel layout</param>
		/// <param name="focusMe">Control to receive the focus when the
		/// popup is complete (or <see langword="null"/>)</param>
		/// <returns>
		/// The image index selected, or -1 if canceled
		/// </returns>
		public int Popup(int x,int y,ImagePanelPlacement placement,Control focusMe,PanelSizeHints panelSizeHint) {
			if (Visible) {
				return -1 ;
			}

			Dimensions = CalculateBestDimensions(imageCount,panelSizeHint) ;

			return DoPopup(x,y,placement,focusMe) ;
		}

		/// <summary>
		/// Show the <c>ImagePanel</c> as a popup using the specified
		/// client <see cref="Rectangle"/> (relative to its parent)
		/// </summary>
		/// <param name="rect">The client rectangle for the pop-up</param>
		/// <param name="focusMe">Control to receive the focus when the
		/// popup is complete (or <see langword="null"/>)</param>
		/// <returns>
		/// The image index selected, or -1 if canceled
		/// </returns>
		public int Popup(Rectangle rect,Control focusMe) {
			if (Visible) {
				return -1 ;
			}

			// force this to be created
			Bitmap image = PanelImage ;

			int maxCols = Math.Min((rect.Width / imageUnits.Width)+1,8) ;
			int bestCols = 1 ;
			int bestRows = 1 ;

			if (rect.Width > rect.Height) {
				bestRows = Math.Min((imageCount / maxCols)+1,8) ;
			} else {
				bestRows = Math.Min((rect.Height / imageUnits.Height)+1,8) ;
				bestCols = Math.Max(1,Math.Min((imageCount / bestRows),8)) ;
			}

			if ((bestCols * bestRows) > imageCount) {
				Dimensions = new Size(bestCols,bestRows) ;
			} else {
				bestCols = Math.Max(1,Math.Min(((rect.Width-SystemInformation.VerticalScrollBarWidth)/imageUnits.Width),8)) ;
				Dimensions = new Size(bestCols,bestRows) ;
			}

			return DoPopup(rect,focusMe) ;
		}
		#endregion Methods

		#region Implementation
		/// <summary>
		/// Get the number of rows in the <c>ImagePanel</c>
		/// </summary>
		private int Rows {
			get {
				return dimensions.Height ;
			}
		}

		/// <summary>
		/// Get the number of columns in the <c>ImagePanel</c>
		/// </summary>
		private int Columns {
			get {
				return dimensions.Width ;
			}
		}

		/// <summary>
		/// Get the cached <c>ImagePanel</c> <see cref="Bitmap"/>, creating
		/// it if necessary
		/// </summary>
		private Bitmap PanelImage {
			get {
				if (panelImage == null) {
					panelImage = CreateBitmap(sourceImages,imageCount,Rows,Columns) ;
				}

				return panelImage ;
			}
		}

		/// <summary>
		/// Get/Set the selected row coordinate
		/// </summary>
		private int SelectedRow {
			get {
				return selectedRow ;
			}

			set {
				selectedRow = value ;
			}
		}

		/// <summary>
		/// Get/Set the selected column coordinate
		/// </summary>
		private int SelectedColumn {
			get {
				return selectedColumn ;
			}

			set {
				selectedColumn = value ;
			}
		}

		/// <summary>
		/// Determine if the <c>ImagePanel</c> has a selected image
		/// </summary>
		private bool HasSelection {
			get {
				return (SelectedRow != -1) && (SelectedColumn != -1) ;
			}
		}

		/// <summary>
		/// Clear any selection
		/// </summary>
		private void ClearSelection() {
			SelectedColumn = SelectedRow = -1 ;
			Animate(false) ;
			Invalidate() ;
		}

		/// <summary>
		/// Ensure that a given row/column coordinate is fully visible
		/// by scrolling as necessary
		/// </summary>
		/// <param name="row">The row</param>
		/// <param name="column">The column</param>
		private void EnsureVisible(int row, int column) {
			if ((ClientRectangle.Width < PanelImage.Width) ||
				(ClientRectangle.Height < PanelImage.Height)) 
			{
				Rectangle rect = GetPanelFrameRect(row,column) ;
				Rectangle temp = ClientRectangle ;
				temp.Inflate(-1,-1) ;
				temp.Offset(-AutoScrollPosition.X,-AutoScrollPosition.Y) ;

				if (!temp.Contains(rect)) {
					int xOffset = Math.Min((rect.Right - ClientRectangle.Right) + gridSize.Width << 1,PanelImage.Width - ClientRectangle.Width) ;
					int yOffset = Math.Min((rect.Bottom - ClientRectangle.Bottom) + gridSize.Width << 1,PanelImage.Height - ClientRectangle.Height) ;
					AutoScrollPosition = new Point(xOffset,yOffset) ;
				}
			}
		}

		/// <summary>
		/// Set the row/column of the selected image
		/// </summary>
		/// <param name="row">The row index (or -1)</param>
		/// <param name="column">The column index (or -1)</param>
		private void SetSelection(int row,int column) {
			if ((row == -1) || (column==-1)) {
				// no selection
				ClearSelection() ;
			} else if (((row != SelectedRow) || (column != SelectedColumn))) {
				// if its a valid image, set the selection and ensure
				// that the item is fully visible
				if (IsImage(row,column)) {
					SelectedRow = row ;
					SelectedColumn = column ;
					EnsureVisible(row,column) ;
					Animate(Visible && Enabled) ;
				} else {
					// not a valid image, no selection
					SelectedRow = -1 ;
					SelectedColumn = -1 ;
					Animate(false) ;
				}

				Invalidate() ;
			}
		}

		/// <summary>
		/// React to property changes, including firing <see cref="PropertyChange"/> events
		/// </summary>
		/// <param name="imagePanelProperty">Enumeration for the property that changed</param>
		protected virtual void OnPropertyChange(ImagePanelProperties imagePanelProperty) {
			switch(imagePanelProperty) {
				case ImagePanelProperties.BackColorProperty:
				case ImagePanelProperties.PanelGridSizeProperty:
				case ImagePanelProperties.GridColorProperty:
				case ImagePanelProperties.ImagesProperty:
				case ImagePanelProperties.DimensionsProperty:
					if (panelImage != null) {
						panelImage.Dispose() ;
						panelImage = null ;
					}

					if (Visible) {
						Invalidate() ;
					}
					break ;

				case ImagePanelProperties.SelectedColorsProperty:
				case ImagePanelProperties.DefaultImageProperty:
					if (Visible) {
						Invalidate() ;
					}
					break ;

				// Note: We dont do anything if these change dynamically
				// If we are animating they will be picked up and nothing bad
				// will happen
				case ImagePanelProperties.MinBounceRatioProperty:
				case ImagePanelProperties.MaxBounceRatioProperty:
				case ImagePanelProperties.BounceFactorProperty:
				case ImagePanelProperties.AutoSelectProperty:
					break ;
			}

			if (propertyChangeListeners != null) {
				propertyChangeListeners(this,new PropertyChangeEventArgs(imagePanelProperty)) ;
			}
		}

		/// <summary>
		/// Fire an <see cref="ImageSelected"/> event
		/// </summary>
		/// <param name="imageIndex">The image index selected or (-1 for cancel)</param>
		protected virtual void OnImageSelectedEvent(int imageIndex) {
			if (imageSelectedListeners != null) {
				imageSelectedListeners(this,new ImageSelectedEventArgs(imageIndex)) ;
			}

			// selecting an image closes the control when in popup mode
			if (isPopup) {
				this.Hide() ;
			}
		}

		/// <summary>
		/// Given the <see cref="PanelSizeHints"/>, calculate the best dimensions
		/// for a given number of images
		/// </summary>
		/// <param name="imageCount">Number of images</param>
		/// <param name="sizeHints">hints about the layout</param>
		/// <returns>
		/// The suggested Width (cols) and Height (rows) of the panel as type
		///  <see cref="System.Drawing.Size"/>
		/// </returns>
		public static Size CalculateBestDimensions(int imageCount,PanelSizeHints sizeHints) {
			// in this case we will always scroll (vertically)
			if (imageCount > (8*8)) {
				return new Size(8,8) ;
			}

			if (sizeHints == PanelSizeHints.MinimizeColumns) {
				// divide by max rows
				return new Size(Math.Min((imageCount >> 3) + 1,8),Math.Min(8,imageCount)) ;
			} else if (sizeHints == PanelSizeHints.MinimizeRows) {
				return new Size(Math.Min(8,imageCount),Math.Min((imageCount >> 3) + 1,8)) ;
			}

			int adjust1 = 1 ;
			int adjust2 = 0 ;

			do {
				adjust1++ ;
				adjust2 = (imageCount+(adjust1-1)) / adjust1 ;
			} while(Math.Abs(adjust1-adjust2) > 1) ;

			return new Size(Math.Min(Math.Max(adjust1,adjust2),8),Math.Min(Math.Min(adjust1,adjust2),8)) ;
		}

		/// <summary>
		/// Given a rectangle, calculate the best dimensions for the <c>ImagePanel</c>
		/// </summary>
		/// <param name="rect">The bounding rectangle</param>
		/// <returns>
		/// The suggested Width (cols) and Height (rows) of the panel as type
		///  <see cref="System.Drawing.Size"/>
		/// </returns>
		public Size CalculateBestDimensions(Rectangle rect) {
			Size result ; 
			int maxCols = Math.Min((rect.Width / imageUnits.Width)+1,8) ;
			int bestCols = 1 ;
			int bestRows = 1 ;

			if (rect.Width > rect.Height) {
				bestRows = Math.Min((imageCount / maxCols)+1,8) ;
			} else {
				bestRows = Math.Min((rect.Height / imageUnits.Height)+1,8) ;
				bestCols = Math.Max(1,Math.Min((imageCount / bestRows),8)) ;
			}

			if ((bestCols * bestRows) > imageCount) {
				result = new Size(bestCols,bestRows) ;
			} else {
				bestCols = Math.Max(1,Math.Min(((rect.Width-SystemInformation.VerticalScrollBarWidth)/imageUnits.Width),8)) ;
				result = new Size(bestCols,bestRows) ;
			}

			return result ;
		}

		/// <summary>
		/// Calculate the best <see cref="Rectangle"/> given the current
		/// row/column configuration
		/// </summary>
		/// <returns>
		/// A suggested client rectangle
		/// </returns>
		private Size CalculateBestClientSize() {
			if (imageCount > (Columns * Rows)) {
				int width = PanelImage.Width + SystemInformation.VerticalScrollBarWidth ;
				int height = imageUnits.Height * Rows ;
				return new Size(width,height) ;
			}

			return new Size(PanelImage.Width,PanelImage.Height) ;
		}

		/// <summary>
		/// Returns the image <see cref="Rectangle"/> for a given row/column
		/// excluding the frame
		/// </summary>
		/// <param name="row">The row</param>
		/// <param name="column">The column</param>
		/// <returns>
		/// The rectangle for a given row/column (does not include frame of image)
		/// </returns>
		private Rectangle GetPanelImageRect(int row,int column) {
			return new Rectangle(
								(column*imageSize.Width)+((column+1)*gridSize.Width),
								(row*imageSize.Height)+((row+1)*gridSize.Height),
								imageSize.Width,
								imageSize.Height
							) ;
		}

		/// <summary>
		/// Returns the image <see cref="Rectangle"/> for a given row/column
		/// including the frame
		/// </summary>
		/// <param name="row">The row</param>
		/// <param name="column">The column</param>
		/// <returns>
		/// The rectangle for a given row/column including the frame
		/// </returns>
		private Rectangle GetPanelFrameRect(int row,int column) {
			Rectangle frameRect = GetPanelImageRect(row,column) ;
			frameRect.Offset(-((gridSize.Width+1)>>1),-((gridSize.Width+1)>>1)) ;
			frameRect.Height += 1 ;
			frameRect.Width += 1 ;
			return frameRect ;
		}

		/// <summary>
		/// Returns the image <see cref="Rectangle"/> for a given row/column
		/// in the source image
		/// </summary>
		/// <param name="row">The row</param>
		/// <param name="column">The column</param>
		/// <returns>
		/// The rectangle for the image that corresponds to the row/column
		/// </returns>
		private Rectangle GetSourceImageRect(int row,int column) {
			return new Rectangle(
								((row*Columns)+column) * imageSize.Width,
								0,
								imageSize.Width,
								imageSize.Height
							) ;
		}

		/// <summary>
		/// Gets the index of the image for the specified row/column
		/// </summary>
		/// <param name="row">The row</param>
		/// <param name="column">The column</param>
		/// <returns>
		/// The image index for the specified row/column or -1 if
		/// the coordinates do specify valid coordinates
		/// </returns>
		private int GetImageIndex(int row,int column) {
			if ((row >= virtualRows) || (column >= Columns))
				return -1 ;

			int imageIndex = (row*Columns) + column ; 
			return (imageIndex >=0) ? imageIndex : -1 ;
		}

		/// <summary>
		/// Determine if the given row/column coordinates contains
		/// an image
		/// </summary>
		/// <param name="row">The row</param>
		/// <param name="column">The column</param>
		/// <returns>
		/// <see langword="true"/> if the coordinates contain an image
		/// </returns>
		private bool IsImage(int row,int column) {
			int imageIndex = GetImageIndex(row,column) ;
			return (imageIndex >= 0) && (imageIndex < imageCount) ;
		}

		/// <summary>
		/// Create the cached <see cref="Bitmap"/> representation of the
		/// <c>ImagePanel</c>
		/// </summary>
		/// <param name="sourceImages">The source images 'strip'</param>
		/// <param name="imageCount">The number of source images</param>
		/// <param name="rows">The number of rows in the panel grid</param>
		/// <param name="columns">The number of cols in the panel grid</param>
		/// <returns>
		/// The <see cref="Bitmap"/> representation of the <c>ImagePanel</c>
		/// </returns>
		private Bitmap CreateBitmap(Bitmap sourceImages,int imageCount,int rows,int columns) {
			Bitmap bitmap = null ;

			if ((sourceImages != null) && (imageCount > 0)) {
				imageSize = new Size(sourceImages.Width / imageCount,sourceImages.Height) ;
				imageUnits = new Size(imageSize.Width+gridSize.Width,imageSize.Height+gridSize.Height) ;

				virtualRows = ((imageCount-1) / columns) + 1 ;
	
				bitmap = new Bitmap(
					(imageUnits.Width* columns)+gridSize.Width,
					(imageUnits.Height * virtualRows)+gridSize.Height,
					PixelFormat.Format32bppArgb
					) ;

				AutoScrollMinSize = new Size(bitmap.Width,bitmap.Height) ;

				using(Graphics g = Graphics.FromImage(bitmap)) {
					using(SolidBrush b = new SolidBrush(this.BackColor)) {
						g.FillRectangle(b,0,0,bitmap.Width,bitmap.Height) ;
					}

					using(Pen pen = new Pen(GridColor,gridSize.Height)) {
						for(int i=0; i < columns ; i++) {
							for(int j=0 ; j < virtualRows ; j++) {
								if (IsImage(j,i)) {
#if !DEBUG_DRAWING
									g.DrawImage(sourceImages,GetPanelImageRect(j,i),GetSourceImageRect(j,i),GraphicsUnit.Pixel) ;
#endif
									g.DrawRectangle(pen,GetPanelFrameRect(j,i)) ;
								} else {
									break ;
								}
							}
						}
					}
				} 
			} else {
				if ((ClientRectangle.Width == 0) || (ClientRectangle.Height==0))
					return null ;

				bitmap = new Bitmap(
					ClientRectangle.Width,
					ClientRectangle.Height,
					PixelFormat.Format32bppArgb
					) ;

				using(Graphics g = Graphics.FromImage(bitmap)) {
					using(SolidBrush b = new SolidBrush(this.BackColor)) {
						g.FillRectangle(b,0,0,bitmap.Width,bitmap.Height) ;
					}

					using(Pen pen = new Pen(GridColor,gridSize.Width)) {
						g.DrawRectangle(pen,gridSize.Width>>1,gridSize.Height>>1,bitmap.Width-gridSize.Width,bitmap.Height-gridSize.Height) ;
					}
				}
			}

			return bitmap ;
		}

		/// <summary>
		/// Hook for sub-classes. Called when popping up
		/// </summary>
		/// <remarks>
		/// <seealso cref="DoPopup"/>
		/// </remarks>
		protected virtual void OnPopup() {}

		/// <summary>
		/// Popup the <c>ImagePanel</c>
		/// </summary>
		/// <param name="rect">The <see cref="Rectangle"/> for the popup</param>
		/// <param name="focusMe">The control to receive focus when the popup completes</param>
		/// <returns>
		/// The index of the image selected, or -1 if canceled
		/// </returns>
		protected int DoPopup(Rectangle rect,Control focusMe) {
			int result = - 1 ;

			ResetPopup() ;

			this.Left = rect.Left ;
			this.Top = rect.Top ;
			this.Width = rect.Width ;
			this.Height = rect.Height ;

			focusControl = focusMe ;

			OnPopup() ;

			this.BringToFront() ;
			Visible = true ;
			Focus() ;

			while(Visible) {
				Application.DoEvents() ;
				MsgWaitForMultipleObjects(0, 0, true, 250, 255);
			}

			if (IsImage(SelectedRow,SelectedColumn)) {
				result = GetImageIndex(SelectedRow,SelectedColumn) ;
			}

			return result ;
		}

		/// <summary>
		/// Popup the <c>ImagePanel</c> at the specified coordinates
		/// </summary>
		/// <param name="x">The X origin</param>
		/// <param name="y">The Y origin</param>
		/// <param name="placement">Specifies meaning of X/Y coordinates</param>
		/// <param name="focusMe">The control to receive focus when the popup completes</param>
		/// <returns>
		/// The index of the image selected, or -1 if canceled
		/// </returns>
		public int DoPopup(int x,int y,ImagePanelPlacement placement,Control focusMe) {
			Size client = CalculateBestClientSize() ;
			Rectangle popupRect ;

			switch(placement) {
				case ImagePanelPlacement.BottomLeft:
					popupRect = new Rectangle(x,y-client.Height,client.Width,client.Height) ;
					break ;

				case ImagePanelPlacement.BottomRight:
					popupRect = new Rectangle(x-client.Width,y-client.Height,client.Width,client.Height) ;
					break ;

				case ImagePanelPlacement.TopRight:
					popupRect = new Rectangle(x-client.Width,y,client.Width,client.Height) ;
					break ;

				case ImagePanelPlacement.TopLeft:
				default:
					popupRect = new Rectangle(x,y,client.Width,client.Height) ;
					break ;
			}

			return DoPopup(popupRect,focusMe) ;
		}

		/// <summary>
		/// Reset the <c>ImagePanel</c> in preperation for Popup
		/// </summary>
		private void ResetPopup() {
			ClearSelection() ;
			
			AutoScrollPosition = new Point(0,0) ;

			isMouseDown = false ;
			isMouseHover = false ;
			isPopup = true ;
		}
		#endregion Implementation

		#region Overrides
		/// <summary>
		/// Override to supress in designer property grid
		/// </summary>
		/// <remarks>
		/// We always want this value to be <see langword="true"/>
		/// </remarks>
		[Browsable(false)]
		public override bool AutoScroll {
			get {
				return base.AutoScroll;
			}
			set {
				if (value == true) {
					base.AutoScroll = value;
				}
			}
		}


		/// <summary>
		/// Override to forward to our <see cref="PropertyChange"/> event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnBackColorChanged(EventArgs e) {
			base.OnBackColorChanged (e);

			OnPropertyChange(ImagePanelProperties.BackColorProperty) ;
		}

		/// <summary>
		/// Handle keyboard navigation
		/// </summary>
		/// <param name="msg">The <see cref="Message"/></param>
		/// <param name="keyData">The <see cref="Keys"/> data</param>
		/// <returns>
		/// <see langword="true"/> if the message was processed, <see langword="false"/> otherwise
		/// </returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			bool fProcessed = true ;
			int hitCol = SelectedColumn ;
			int hitRow = SelectedRow ;

			switch(keyData) {
				case Keys.Down:
					hitRow = Math.Min(hitRow+1,virtualRows-1) ;
					hitCol = Math.Max(hitCol,0) ;
					break ;
				case Keys.Up:
					hitRow = Math.Max(hitRow-1,0) ;
					hitCol = Math.Max(hitCol,0) ;
					break ;
				case Keys.Left:
					hitCol = Math.Max(hitCol-1,0) ;
					hitRow = Math.Max(hitRow,0) ;
					break ;
				case Keys.Right:
					hitCol = Math.Min(hitCol+1,Columns-1) ;
					hitRow = Math.Max(hitRow,0) ;
					break ;
				case Keys.Escape:
					hitCol = -1 ;
					hitRow = -1 ;

					OnImageSelectedEvent(-1) ;
					break ;
				case Keys.Enter:
				case Keys.Space:
					if (!HasSelection) {
						SelectedRow = 0 ;
						SelectedColumn = 0 ;
					} 

					OnImageSelectedEvent(GetImageIndex(SelectedRow,SelectedColumn)) ;
					break ;
				default:
					fProcessed = false ;
					break ;
			}

			SetSelection(hitRow,hitCol) ;

			return fProcessed ? true : base.ProcessCmdKey (ref msg, keyData);
		}

		/// <summary>
		/// Handle various conditions when the <c>ImagePanel</c>
		/// is shown/hidden
		/// </summary>
		/// <param name="e"><see cref="EventArgs"/></param>
		protected override void OnVisibleChanged(EventArgs e) {
			if (!Visible) {
				if (focusControl!=null) {
					focusControl.Focus() ;
				}
			} else {
				if (DefaultImage != -1) {
					SetSelection(DefaultImage / dimensions.Width,DefaultImage % dimensions.Width) ;
				} else {
					ClearSelection() ;
				}
			}

			base.OnVisibleChanged (e);
		}

		/// <summary>
		/// React to losing the focus
		/// </summary>
		/// <param name="e"><see cref="EventArgs"/></param>
		protected override void OnLostFocus(EventArgs e) {
			if (Visible && isPopup) {
				Hide() ;
			}

			base.OnLostFocus (e);
		}

		/// <summary>
		/// Handle mouse down
		/// </summary>
		/// <param name="e"><see cref="MouseEventArgs"/></param>
		protected override void OnMouseDown(MouseEventArgs e) {
			Rectangle temp = ClientRectangle ;
			temp.Width += VScroll ? SystemInformation.VerticalScrollBarWidth : 0 ;
			temp.Height += HScroll ? SystemInformation.HorizontalScrollBarHeight : 0 ;
			if (temp.Contains(e.X,e.Y)) {
				int hitCol = (e.X-AutoScrollPosition.X) / imageUnits.Width ;
				int hitRow = (e.Y-AutoScrollPosition.Y) / imageUnits.Height ;

				this.SetSelection(hitRow,hitCol) ;
				isMouseDown = true ;
				Invalidate() ;
			} else {
				// This code is useless when the mouse is not captured. It is designed
				// to work like a menu where clicking outside the menu area closes it. When
				// we dont capture the mouse, we never get a mouse move that is outside the
				// client rectangle. Unfourtunately, I had small mouse issues when capturing 
				// so I left it uncaptured
				OnImageSelectedEvent(-1) ;
			}
		}

		/// <summary>
		/// Handle mouse hover for highlight and auto-select
		/// </summary>
		/// <param name="e"><see cref="EventArgs"/></param>
		protected override void OnMouseHover(EventArgs e) {
			if (HasSelection && IsImage(SelectedRow,SelectedColumn)) {
				if (isAutoSelect) {
					OnImageSelectedEvent(GetImageIndex(SelectedRow,SelectedColumn)) ;
				}

				isMouseHover = true ;
				Invalidate() ;
			}

			base.OnMouseHover(e) ;
		}

		/// <summary>
		/// Handle mouse up 
		/// </summary>
		/// <param name="e"><see cref="MouseEventArgs"/></param>
		/// <remarks>
		/// MouseUp triggers <see cref="ImageSelected"/> event if the
		/// mouse down was on a valid image
		/// </remarks>
		protected override void OnMouseUp(MouseEventArgs e) {
			isMouseDown = false ;
			
			if (IsImage(SelectedRow,SelectedColumn)) {
				OnImageSelectedEvent(GetImageIndex(SelectedRow,SelectedColumn)) ;
			} else {
				Invalidate() ;
			}
		}

		/// <summary>
		/// Handle mouse movement for image selection 
		/// </summary>
		/// <param name="e"><see cref="MouseEventArgs"/></param>
		protected override void OnMouseMove(MouseEventArgs e) {
			int hitCol = -1 ;
			int hitRow = -1 ;

			if (imageCount > 0) {
				if (ClientRectangle.Contains(e.X,e.Y)) {
					hitCol = (e.X-AutoScrollPosition.X) / imageUnits.Width ;
					hitRow = (e.Y-AutoScrollPosition.Y) / imageUnits.Height ;
				}

				if ((hitCol != SelectedColumn) || (hitRow != SelectedRow)) {
					if (isMouseHover) {
						isMouseHover = false ;
						// Undocumented. May have unknown side-effects, but does the trick of
						// restarting the mouse hover timer without leaving the control
						base.ResetMouseEventArgs() ;
					}

					SetSelection(hitRow,hitCol) ;
				}
			}

			base.OnMouseMove (e);
		}

		/// <summary>
		/// Grab focus on mouse enter
		/// </summary>
		/// <param name="e"><see cref="EventArgs"/></param>
		protected override void OnMouseEnter(EventArgs e) {
			this.Focus() ;
			base.OnMouseEnter (e);
		}

		/// <summary>
		/// Clear selection when mouse leaves
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>
		/// Note that we don't hide if in popup mode when the mouse leaves
		/// </remarks>
		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave (e);

			if (Visible) {
				ClearSelection() ;
			}
		}
		#endregion Overrides

		#region Paint code
		/// <summary>
		/// Draw the <c>ImagePanel</c>
		/// </summary>
		/// <param name="pevent"><see cref="PaintEventArgs"/></param>
		protected override void OnPaintBackground(PaintEventArgs pevent) {
			Bitmap doubleBuffer = new Bitmap(ClientRectangle.Width,ClientRectangle.Height) ;
			Graphics offscreen = Graphics.FromImage(doubleBuffer) ;
			offscreen.PageUnit = GraphicsUnit.Pixel ;

			using(SolidBrush b = new SolidBrush(BackColor)) {
				offscreen.FillRectangle(b,ClientRectangle) ;
			}

			offscreen.DrawImageUnscaled(PanelImage,AutoScrollPosition.X,AutoScrollPosition.Y) ;

			if (HasSelection && IsImage(SelectedRow,SelectedColumn)) {
				Rectangle imageRect = GetPanelImageRect(SelectedRow,SelectedColumn) ;
				Rectangle frameRect = GetPanelFrameRect(SelectedRow,SelectedColumn) ;
				
				imageRect.Offset(AutoScrollPosition.X,AutoScrollPosition.Y) ;
				frameRect.Offset(AutoScrollPosition.X,AutoScrollPosition.Y) ;

				Color fillColor = SelectedColors.Background ;
				Color frameColor = SelectedColors.Foreground ;

				if (!fillColor.IsEmpty) {
					using(SolidBrush b = new SolidBrush(fillColor)) {
						offscreen.FillRectangle(b,imageRect) ;
					}
				}

				if (!frameColor.IsEmpty) {
					using(Pen pen = new Pen(frameColor,gridSize.Width)) {
						offscreen.DrawRectangle(pen,frameRect) ;
					}
				}

				RectangleF imageRectF = new RectangleF(imageRect.Left,imageRect.Top,imageRect.Width,imageRect.Height) ;

				imageRectF.Offset(
					-((imageRectF.Width * bounceFactor) - imageRectF.Width) / 2,
					-((imageRectF.Height * bounceFactor) - imageRectF.Height) / 2
					) ;

				imageRectF.Width = imageRectF.Width * bounceFactor ;
				imageRectF.Height = imageRectF.Width * bounceFactor ;

#if !DEBUG_DRAWING
				offscreen.DrawImage(sourceImages,imageRectF,GetSourceImageRect(SelectedRow,SelectedColumn),GraphicsUnit.Pixel) ;
#endif
			}

			// pevent.Graphics.PageUnit = GraphicsUnit.Pixel ;
			pevent.Graphics.DrawImage(doubleBuffer,0,0) ;
			offscreen.Dispose() ;
			doubleBuffer.Dispose() ;
		}
		#endregion Paint code

		#region Animation
		private void Animate(bool fAnimationHint) {
			if (fAnimationHint != bounceTimer.Enabled) {
				if (fAnimationHint == false) {
					bounceTimer.Enabled = false ;
					// Make sure image is shown normal
					bounceFactor = 1.0f ;
				} else {
					// we dont animate if either of these conditions is true
					if ((BounceFactor != 0.0f) && ((MinBounceRatio != 1.0) && (MaxBounceRatio != 1.0))) {
						bounceFactor = 1.0f ;
						bounceFactorAdjust = baseBounceFactorAdjust ;
						bounceTimer.Enabled = true ;
					} else {
						// here we wont bounce, but we will show a larger image :)
						bounceFactor = MaxBounceRatio ;
					}
				}
			}
		}

		private void bounceTimer_Tick(object sender, System.EventArgs e) {
			bounceFactor += bounceFactorAdjust ;
			if (bounceFactor >= MaxBounceRatio) {
				bounceFactor = MaxBounceRatio ;
				bounceFactorAdjust = -bounceFactorAdjust ;
			} else if (bounceFactor <= MinBounceRatio) {
				bounceFactor = MinBounceRatio ;
				bounceFactorAdjust = -bounceFactorAdjust ;
			}
			Invalidate() ;
		}
		#endregion Animation
	}

	#region class ImageSelectedEventArgs
	/// <summary>
	/// <see cref="ImagePanel.ImageSelected"/> event arguments
	/// </summary>
	public class ImageSelectedEventArgs : System.EventArgs {
		/// <summary>
		/// The image index selected (or -1 if canceled)
		/// </summary>
		private readonly int imageIndex ;

		/// <summary>
		/// Create a <c>ImageSelectedEventArgs</c> with the specified
		/// image index
		/// </summary>
		/// <param name="imageIndex">The index of the image selected or -1 if canceled</param>
		public ImageSelectedEventArgs(int imageIndex) {
			this.imageIndex = imageIndex ;
		}

		/// <summary>
		/// Get the selected image index
		/// </summary>
		public int ImageIndex {
			get {
				return imageIndex ;
			}
		}
	}
	#endregion class ImageSelectedEventArgs
}
