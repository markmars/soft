using System;
using System.Runtime.InteropServices;

namespace MarkMars.PDF
{
	/// <summary>
	/// Summary description for Declares.
	/// </summary>
	public class Declares
	{
		public delegate int Callback_Func(int state, int level, int param);
        public static string g_RegKey = "PCS50-AOKKD-JSUKP-ZS4FA-MBOKW-EF0UY";
        public static string g_DevCode = "PDFX3$Henry$300604_Allnuts#";
		
		// Initialize PDF object
		[DllImport("xcpro40")]
		public static extern int PXCp_Init(out int pObject, string Key, string DevCode);
		// Deinitialize PDF Object
		[DllImport("xcpro40")]
		public static extern int PXCp_Delete(int pObject);
		// Set callback function
		[DllImport("xcpro40")]
		public static extern int PXCp_SetCallBack(int pObject, Callback_Func pProc, int UserData);
		// Read document
		[DllImport("xcpro40")]
		public static extern int PXCp_ReadDocumentW(int pObject, [MarshalAs(UnmanagedType.LPWStr)] string pwFileName, int iFlags);
		// Check the password for encrypted document
		[DllImport("xcpro40")]
		public static extern int PXCp_CheckPassword(int pObject, ref byte pPassword, int PassLen);
		// Continue reading document after checking password
		[DllImport("xcpro40")] public static extern	int  PXCp_FinishReadDocument(int pObject, int Flags);
		// Check if document is encrypted?
		[DllImport("xcpro40")] public static extern	int  PXCp_IsEncrypted(int pObject, out bool bEncrypted);
		// Set Securety
		[DllImport("xcpro40")] public static extern	int	 PXCp_EnableSecurity(int pObject, bool bEnable, [MarshalAs(UnmanagedType.LPStr)] string UserPwd, [MarshalAs(UnmanagedType.LPStr)] string OwnerPwd);
		// Encryption parameters
		[DllImport("xcpro40")] public static extern	int	 PXCp_SetPermissions(int pObject, int enclevel, int permFlags);
		// Write document
		[DllImport("xcpro40")] public static extern	int  PXCp_WriteDocumentW(int pdf, [MarshalAs(UnmanagedType.LPWStr)] string fName, PXCp_CreationDisposition CreationDesposition, int	WriteFlags);
		// Copy selected page ranges and insert them into destination PDF document
		[DllImport("xcpro40")] public static extern	int  PXCp_InsertPagesTo(int pSrcObject, int pDestObject, PXCp_CopyPageRange[] pPageRanges, int RangesCount, int Flags);
		// Permanently delete page
		[DllImport("xcpro40")] public static extern	int  PXCp_RemovePage(int pDocument, int PageNumber);
		// optimize fonts in document
		[DllImport("xcpro40")] public static extern	int  PXCp_OptimizeFonts(int pDoc, int Flags);
		// optimize stream compression
		[DllImport("xcpro40")] public static extern	int  PXCp_OptimizeStreamCompression(int pDoc);
		// remove named destinations
		[DllImport("xcpro40")] public static extern	int  PXCp_OptimizeRemoveNamedDests(int pDoc);

        #region Document level information
		// Get standard document info tag value
		[DllImport("xcpro40")] public static extern	int  PXCp_GetDocumentInfoW(int pDoc, PXC_StdInfoField field, [MarshalAs(UnmanagedType.LPWStr)]string val, ref int bufLen);
		// Get non std doc info tags
		[DllImport("xcpro40")] public static extern	int  PXCp_GetDocumentInfoExW(int pDoc, int index, [MarshalAs(UnmanagedType.LPWStr)] string key, ref int keybufLen, [MarshalAs(UnmanagedType.LPWStr)] string val, ref int valuebufLen);
		// Set Document info
		[DllImport("xcpro40")] public static extern	int  PXCp_SetDocumentInfoW(int pDocument, PXC_StdInfoField field, [MarshalAs(UnmanagedType.LPWStr)] string KeyVal);
		// Set not std Document info
		[DllImport("xcpro40")] public static extern	int  PXCp_SetDocumentInfoExW(int pDocument, [MarshalAs(UnmanagedType.LPWStr)] string KeyName, [MarshalAs(UnmanagedType.LPWStr)] string KeyVal);
		// Get Page Layout
		[DllImport("xcpro40")] public static extern	int	 PXCp_GetPageLayout(int pDocument, out PXC_PageLayout playout);
		// Set Page Layout
		[DllImport("xcpro40")] public static extern	int	 PXCp_SetPageLayout(int pDocument, PXC_PageLayout layout);
		// Get Page Mode
		[DllImport("xcpro40")] public static extern	int	 PXCp_GetPageMode(int pDocument, out PXC_PageMode pmode);
		// Set Page Mode
		[DllImport("xcpro40")] public static extern	int	 PXCp_SetPageMode(int pDocument, PXC_PageMode mode);
		// Get viewer preferences (see. "PXC_ViewerPreferences" for flag values)
		[DllImport("xcpro40")] public static extern	int	 PXCp_GetViewerPreferences(int pDocument, out int pvprefs);
		// Set viewer preferences (see. "PXC_ViewerPreferences" for flag values)
		[DllImport("xcpro40")] public static extern	int	 PXCp_SetViewerPreferences(int pDocument, int vprefs);
		// Get Permissions (enclevel is key length in bits)
		[DllImport("xcpro40")] public static extern	int	 PXCp_GetPermissions(int pDocument, ref int enclevel, ref int permFlags);
		// Get PDF specification version of the document
		[DllImport("xcpro40")] public static extern	int	 PXCp_GetSpecVersion(int pDocument, out PXC_SpecVersion pver);
		// Set PDF specification version of the document
		[DllImport("xcpro40")] public static extern	int	 PXCp_SetSpecVersion(int pDocument, PXC_SpecVersion ver);
        #endregion

        #region Page level information
        // Get pages count
		[DllImport("xcpro40")] public static extern	int	 PXCp_GetPagesCount(int pDocument, out int count);
		// Get page boxs
		[DllImport("xcpro40")] public static extern	int	 PXCp_PageGetBox(int pDocument, int PageNumber, PXC_PageBox pBoxID, out PXC_RectF rect);
		// Set Page boxs
		[DllImport("xcpro40")] public static extern	int	 PXCp_PageSetBox(int pDocument, int PageNumber, PXC_PageBox pBoxID, ref PXC_RectF rect);
		// Get page roatate
		[DllImport("xcpro40")] public static extern	int	 PXCp_PageGetRotate(int pDocument, int PageNumber, out int pangle);
		// Set page rotate
		[DllImport("xcpro40")] public static extern	int	 PXCp_PageSetRotate(int pDocument, int PageNumber, int angle);
		//Page thumbnails
		// Has page thumbnail ?
		[DllImport("xcpro40")] public static extern	int  PXCp_PageHasThumbnail(int pDocument, int PageNumber, out bool bThumbnailPresent);
		// Get page thumbnail (pImage is _XCPage*)
		[DllImport("xcpro40")] public static extern	int  PXCp_PageGetThumbnail(int pDocument, int PageNumber, out int pImage);
		// Set page thumbnail (pImage is _XCPage)
		[DllImport("xcpro40")] public static extern	int  PXCp_PageSetThumbnail(int pDocument, int PageNumber, int pImage, PXCp_ThumbFlag flag);
		// Save page thumbnail into the file
		[DllImport("xcpro40")] public static extern	int  PXCp_PageSaveThumbnailToFile(int pDocument, int PageNumber, [MarshalAs(UnmanagedType.LPWStr)] string FileName, ref PXCp_SaveImageOptions pSaveOptions);
        #endregion

        #region Bookmarks (outlines) functions
        // Get root bookmark item
		[DllImport("xcpro40")] public static extern	int	 PXCp_GetRootBMItem(int pDocument, out int bmItem);
		// Get item (sibling, child, parent)
		[DllImport("xcpro40")] public static extern	int	 PXCp_BMGetItem(int pDocument, int bmItem, PXCp_OutlinePos itemPos, out int pbmItem);
		// Get Bookamrk item info
		[DllImport("xcpro40")] public static extern	int	 PXCp_BMGetItemInfo(int pDocument, int bmItem, ref PXCp_BMInfo pbmItemInfo);
		// Set Bookamrk item info
		[DllImport("xcpro40")] public static extern	int	 PXCp_BMSetItemInfo(int pDocument, int bmItem, ref PXCp_BMInfo pbmItemInfo);
		// Delete Bookmark Item
		[DllImport("xcpro40")] public static extern	int  PXCp_BMDeleteItem(int pDocument, int bmItem);
		// Delete All Bookmark Items
		[DllImport("xcpro40")] public static extern	int  PXCp_BMDeleteAllItems(int pDocument);
		// Insert Bookamrk item, bmInsertAfter could be PBM_ROOT, PBM_FIRST, PBM_LAST or real item handle
		[DllImport("xcpro40")] public static extern	int	 PXCp_BMInsertItem(int pDocument, int bmParent, int bmInsertAfter, out int bmItem, ref PXCp_BMInfo pItemInfo);
		// Move item to other 'position'
		[DllImport("xcpro40")] public static extern	int	 PXCp_BMMoveItem(int pDocument, int bmItem, int bmParent, int bmInsertAfter);
        #endregion

        #region Image obtain functions
        // Get number of all unique images in the document
		[DllImport("xcpro40")] public static extern	int  PXCp_ImageGetCount(int pDocument, out int pImageCnt, bool bForceRecalc);
		// Get _XCPage object for image represented by its id
		[DllImport("xcpro40")] public static extern	int  PXCp_GetDocImageAsXCPage(int pDocument, int ImageID, out int pImage);
		// Save document image into the image file with the specified parameters
		[DllImport("xcpro40")] public static extern	int  PXCp_SaveDocImageIntoFileW(int pDocument, int ImageID, [MarshalAs(UnmanagedType.LPWStr)] string FileName, ref PXCp_SaveImageOptions pSaveOptions);
		// Clean up all image data (free memory used for image operations)
		[DllImport("xcpro40")] public static extern	int  PXCp_ImageClearAllData(int pDocument);
		// Get image number on page
		[DllImport("xcpro40")] public static extern	int  PXCp_ImageGetCountOnPage(int pDocument, int PageNumber, out int pImageCnt);
		// Get image from page by its number
		[DllImport("xcpro40")] public static extern	int  PXCp_ImageGetFromPage(int pDocument, int PageNumber, int ImageOnPageNumber, out int pImageHandle, ref PXC_Matrix pMatrix);
		// Clear information about images from page
		[DllImport("xcpro40")] public static extern	int  PXCp_ImageClearPageData(int pDocument, int PageNumber);
        #endregion

        #region Watermarks
        // Watermarks
		[DllImport("xcpro40")] public static extern	int	 PXCp_AddWatermark(int pDocument, ref PXC_Watermark watermark);
        #endregion

        #region Annotations
        // Annotation manipulations
		[DllImport("xcpro40")] public static extern	int  PXCp_SetAnnotsInfo(int hDocument,  ref PXC_CommonAnnotInfo pInfo);
		// Add text annotation (W)
		[DllImport("xcpro40")] public static extern	int  PXCp_AddTextAnnotationW(int hDocument, int PageNumber, 
									ref PXC_RectF rect, [MarshalAs(UnmanagedType.LPWStr)] string pwszTitle, [MarshalAs(UnmanagedType.LPWStr)] string pwszAnnot, 
									PXC_TextAnnotsType type,  ref PXC_CommonAnnotInfo pInfo);
		// Add text annotation (A)
		[DllImport("xcpro40")] public static extern	int  PXCp_AddTextAnnotationA(int hDocument, int PageNumber, 
									ref PXC_RectF rect, [MarshalAs(UnmanagedType.LPStr)] string pszTitle, [MarshalAs(UnmanagedType.LPStr)] string pszAnnot, 
									PXC_TextAnnotsType type,  ref PXC_CommonAnnotInfo pInfo);

		// Add link 
		[DllImport("xcpro40")] public static extern	int	 PXCp_AddLink(int hDocument, int PageNumber, ref PXC_RectF rect, 
									[MarshalAs(UnmanagedType.LPStr)] string lpszURL,  ref PXC_CommonAnnotInfo pInfo);
		// Add line annotation (W)
		[DllImport("xcpro40")] public static extern	int  PXCp_AddLineAnnotationW(int hDocument, int PageNumber,
									ref PXC_RectF rect, [MarshalAs(UnmanagedType.LPWStr)] string pwszTitle, [MarshalAs(UnmanagedType.LPWStr)] string pwszAnnot,
									ref PXC_PointF pntStart, ref PXC_PointF pntEnd,
									PXC_LineAnnotsType sEndStyle, PXC_LineAnnotsType eEndStyle,
									int cInterior,
									ref PXC_CommonAnnotInfo pInfo);
		// Add line annotation (A)
		[DllImport("xcpro40")] public static extern	int  PXCp_AddLineAnnotationA(int hDocument, int PageNumber,
									ref PXC_RectF rect, [MarshalAs(UnmanagedType.LPStr)] string pszTitle, [MarshalAs(UnmanagedType.LPStr)] string pszAnnot,
									ref PXC_PointF pntStart, ref PXC_PointF pntEnd,
									PXC_LineAnnotsType sEndStyle, PXC_LineAnnotsType eEndStyle,
									int cInterior,
									ref PXC_CommonAnnotInfo pInfo);
		// Add goto annotation
		[DllImport("xcpro40")] public static extern	int	 PXCp_AddGotoAction(int hDocument, int PageNumber, ref PXC_RectF rect, int page, 
									PXC_OutlineDestination mode, double v1, double v2, double v3, double v4,
									ref PXC_CommonAnnotInfo pInfo);
		// Add lunch action (W)
		[DllImport("xcpro40")] public static extern	int  PXCp_AddLaunchActionW(int hDocument, int PageNumber, PXC_RectF rect, 
									[MarshalAs(UnmanagedType.LPWStr)] string lpwszFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpwszParams, 
									PXC_LaunchOperation oper,  ref PXC_CommonAnnotInfo pInfo);
		// Add lunch action (A)
		[DllImport("xcpro40")] public static extern	int  PXCp_AddLaunchActionA(int hDocument, int PageNumber, PXC_RectF rect, 
									[MarshalAs(UnmanagedType.LPStr)] string lpszFileName, [MarshalAs(UnmanagedType.LPStr)] string lpszParams, 
									PXC_LaunchOperation oper, ref PXC_CommonAnnotInfo pInfo);
		// Add 3D annotation (W)
		[DllImport("xcpro40")] public static extern	int  PXCp_Add3DAnnotationW(int hDocument, int PageNumber, PXC_RectF rect, 
									[MarshalAs(UnmanagedType.LPWStr)] string pwszTitle, int dwAnnotOption, 
									IntPtr AltImage, int imFlag, 
									 ref PXC_3DView def_view, int def_view_id, ref byte lpBuf, int nBufSize,
										ref PXC_CommonAnnotInfo pInfo);
		// Add 3D annotation (A)
		[DllImport("xcpro40")] public static extern	int  PXCp_Add3DAnnotationA(int hDocument, int PageNumber, PXC_RectF rect, 
									[MarshalAs(UnmanagedType.LPStr)] string pszTitle, int dwAnnotOption, 
									IntPtr AltImage, int imFlag, 
									 ref PXC_3DView def_view, int def_view_id, ref byte lpBuf, int nBufSize,
										 ref PXC_CommonAnnotInfo pInfo);
        #endregion

        //-- Error descriptions API
		// Get severity description from int
		[DllImport("xcpro40")] public static extern int PXCp_Err_FormatSeverity(int errorcode, string buf, int maxlen);
		// Get facility description from int
		[DllImport("xcpro40")] public static extern int PXCp_Err_FormatFacility(int errorcode, string buf, int maxlen);
		// Get error description from int
		[DllImport("xcpro40")] public static extern int PXCp_Err_FormatErrorCode(int errorcode, string buf, int maxlen);

        #region Text Extractions
        [DllImport("xcpro40")] public static extern int PXCp_ET_AnalyzePageContent(int pDocument, int pageNum);
		[DllImport("xcpro40")] public static extern int PXCp_ET_Finish(int pDocument);
		[DllImport("xcpro40")] public static extern int PXCp_ET_GetCurrentComposeParams(int pDocument, out PXP_TETextComposeOptions pOptions);
		[DllImport("xcpro40")] public static extern int PXCp_ET_GetElement(int pDocument, int index, ref PXP_TextElement pElement, int flags);
		[DllImport("xcpro40")] public static extern int PXCp_ET_GetElementCount(int pDocument, out int count);
		[DllImport("xcpro40")] public static extern int PXCp_ET_GetFontCount(int pDocument, out int count);
		[DllImport("xcpro40")] public static extern int PXCp_ET_GetFontInfo(int pDocument, int index, out PXP_TEFontInfo pinfo);
		[DllImport("xcpro40")] public static extern int PXCp_ET_GetFontName(int pDocument, int index, [MarshalAs(UnmanagedType.LPWStr)] string name,  ref int ln);
		[DllImport("xcpro40")] public static extern int PXCp_ET_GetFontObj(int pDocument, int index, out int pObject);
		[DllImport("xcpro40")] public static extern int PXCp_ET_GetFontStyle(int pDocument, int index, [MarshalAs(UnmanagedType.LPWStr)] string style, ref int ln);
        [DllImport("xcpro40")] public static extern int PXCp_ET_GetPageContentAsTextW(int pDocument, int pagenum, ref PXP_TETextComposeOptions pOptions, out IntPtr pBuffer, ref int ln);
		[DllImport("xcpro40")] public static extern int PXCp_ET_Prepare(int pDocument);
        #endregion

        public enum DPDFVariant_Type
		{
			PVT_EMPTY		= 0x0000,
			PVT_NULL		= 0x0001,
			PVT_BOOL		= 0x0002,
			PVT_INT			= 0x0003,
			PVT_DOUBLE		= 0x0004,
			PVT_NAME		= 0x0005,
			PVT_STRING		= 0x0006,
			PVT_ARRAY		= 0x0007,
			PVT_DICTIONARY	= 0x0008,
			PVT_OBJREF		= 0x0009,
			PVT_TempREF,	// just last, for internal use only
		};

		public enum PXCp_CreationDisposition
		{
			PXCp_CreationDisposition_Skip		=  1,
			PXCp_CreationDisposition_Overwrite	=  2,
		};

		// Write document flags
		public enum PXCp_WriteDocFlag
		{
			PXCp_Write_Release		= 0,
			PXCp_Write_NoRelease	= 1,
		};

		// Page ranges for spilt/merge function
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXCp_CopyPageRange
		{
			public int StartPage;
			public int EndPage;
			public int InsertBefore;
			public int Reserved;
		};

		// Bookmarks retriving constants
		public enum PXCp_OutlinePos
		{
			PBM_CHILD		= 0,	// Retrieves the first child item 
			PBM_NEXT		= 1,	// Retrieves the next sibling item
			PBM_PARENT		= 2,	// Retrieves the parent of the specified item
			PBM_PREVIOUS	= 3,	// Retrieves the previous sibling item
			PBM_ROOT		= 4,	// Retrieves the topmost
			PBM_FIRST		= 5,	// Inserts the item at the beginning of the list.
			PBM_LAST		= 6,	// Inserts the item at the end of the list.
		};

		// Mask of the bookmark item info fields
		public enum PXCp_BMInfoMask
		{
			BMIM_TitleA			= 0x0001,
			BMIM_TitleW			= 0x0002,
			BMIM_Open			= 0x0004,
			BMIM_UserData		= 0x0008,
			BMIM_Style			= 0x0010,
			BMIM_Color			= 0x0020,
			BMIM_Destination	= 0x0040,
		};

		// Destination Flags
		public enum PXCp_BMDestFlag
		{
			BMDF_DestIsURL		= 0x0001,
			BMDF_LeftIsNULL		= 0x0002,
			BMDF_TopIsNULL		= 0x0004,
			BMDF_RightIsNULL	= 0x0008,
			BMDF_BottomIsNULL	= 0x0010,
			BMDF_ZoomIsNULL		= 0x0020,
		};

		// Bookmark destination
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXCp_BMDestination
		{
			public PXC_OutlineDestination DestType;
			public int	Mask;
			public int	PageNumber;
			public double Left;
			public double Top;
			public double Right;
			public double Bottom;
			public double Zoom;
			public IntPtr URL;
			public int LengthOfURL;
		};

		// Bookmark item info
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXCp_BMInfo
		{
			public int cbSize;
			public int Mask;
			public IntPtr TitleW;
			public IntPtr TitleA;
			public int	LengthOfTitle;
			public int bOpen;
			public PXC_OutlineStyle Style;
			public int Color;
			public PXCp_BMDestination Destination;
			public int UserData;
		};

		// Thumbnail set option flags
		public enum PXCp_ThumbFlag
		{
			thf_SetAsIs				= 0x00,	// do not scale, save as is
			thf_Scale				= 0x01, // scale to small rect with page proportions
			thf_KeepProportions		= 0x02, // scale to small rect with image proportions
		};

		public enum FilterType
		{
			ft_Unknown,
			ft_ASCIIHex,
			ft_ASCII85,
			ft_LZW,
			ft_Flate,
			ft_RLE,
			ft_CCITTFax,
			ft_JBIG2,
			ft_DCT,
			ft_JPX,
			ft_Crypt
		};

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXCp_FilterParam
		{
			// --------- Optional parameters for LZWDecode and FlateDecode filters
			// Predictor (def = 1)
			public int Predictor;
			// Colors (Used only if Predictor is greater than 1) The number of interleaved color components
			// per sample. Valid values are 1 to 4 in PDF 1.2 or earlier, and 1 or
			// greater in PDF 1.3 or later. 
			// Default value: 1.
			public int Colors;
			// (Used only if Predictor is greater than 1) The number of bits used to represent
			// each color component in a sample. Valid values are 1, 2, 4, 8, and (in PDF 1.5) 16. 
			// Default value: 1.
			public int Bpp;
			// (Used only if Predictor is greater than 1) The number of samples in each row.
			// Default value: 8.
			public int Columns;
			// (LZWDecode only)
			// Default value: 1.
			public int EarlyChange;
			// --------- Optional parameters for the CCITTFaxDecode filter
			// Default value: 0
			public int K;
			// Default value: false.
			public int EndOfLine;
			// Default value: false.
			public int EncodedByteAlign;
			// Default value: 1728
			public int ccitt_Columns;
			// Default value: 0
			public int Rows;
			// Default value: true.
			public int EndOfBlock;
			// Default value: false.
			public int BlackIs1;
			// Default value: 0
			public int DamagedRowsBeforeError;
			// --------- Optional parameter for the JBIG2Decode filter
			public IntPtr hObject;
			// --------- Optional parameter for the DCTDecode filter
			// The default value of ColorTransform is 1 if the image has three components
			// and 0 otherwise.
			public int ColorTransform;
		};

		// Data for compress filter
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXCp_CompressParam
		{
			// jpeg & j2k parameters:
			public int	Width;
			public int	Height;
			public byte	NumOfComponent;
			public byte	Quality;
			// Flate (zip) parameters:
			public int	CompressionLevel;
			// CCITT parametrs (also 'Widht' and 'Height' are necessary):
			public int	K;
			public int	bEndOfLine;
			public int	bEncodedeLineAlign;
			public int	bEndOfBlocks;
		};

		// Image extraction defenitions
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXCp_SaveImageOptions
		{
			public int fmtID;
			public int imgType;
			public int bConvertToGray;
			public int bDither;	// 0 - disable; 1 - enable
			public int bWriteAlpha;
			public int xDPI;		// 0 - use image's dpi
			public int yDPI;		// 0 - use image's dpi
			public int CompressionMethod;
			public int CompressionLevel;
			public int bAppendToExisting;
		};

		// Format IDs
		public const int PRO_FMT_BMP_ID = 0x424d5020;
		public const int PRO_FMT_PNG_ID	= 0x504e4720;
		public const int PRO_FMT_GIF_ID = 0x47494620;
		public const int PRO_FMT_PGM_ID	= 0x50474d20;
		public const int PRO_FMT_PBM_ID	= 0x50424d20;
		public const int PRO_FMT_PPM_ID	= 0x50504d20;
		public const int PRO_FMT_JP2K_ID = 0x4a50324b;
		public const int PRO_FMT_JPEG_ID = 0x4a504547;
		public const int PRO_FMT_JNG_ID	= 0x4a4e4720;
		public const int PRO_FMT_TGA_ID	= 0x54474120;
		public const int PRO_FMT_TIFF_ID = 0x54494646;
		public const int PRO_FMT_WBMP_ID = 0x57424d50;
		public const int PRO_FMT_PCX_ID	= 0x50435820;
		public const int PRO_FMT_DCX_ID	= 0x44435820;

		// Image types
		public enum PXCp_ImageType
		{
			ImType_bw_1bpp = 1,
			// Indexed
			ImType_index_1bpp,
			ImType_index_2bpp,
			ImType_index_3bpp,
			ImType_index_4bpp,
			ImType_index_5bpp,
			ImType_index_6bpp,
			ImType_index_7bpp,
			ImType_index_8bpp,
			// RGB
			ImType_rgb_15,
			ImType_rgb_16_565,
			ImType_rgb_16_5551,
			ImType_rgb_24bpp,
			ImType_rgb_32bpp,
			ImType_rgb_36bpp,
			ImType_rgb_48bpp,
			// Gray
			ImType_gray_8bpp,
			ImType_gray_12bpp,
			ImType_gray_16bpp,
		};

		// Compression method
		public enum PXCp_ImageCompressionMethod
		{
			ImCompression_None = 0,
			ImCompression_ZIP,
			ImCompression_JPEG,
			ImCompression_LZW,
			ImCompression_RLE,
			ImCompression_CCITT3_1d,
			ImCompression_CCITT3_2d,
			ImCompression_CCITT4,
			ImCompression_CCITT_RLEW,
			ImCompression_JPEG2k,
			// For PxM
			ImCompression_ASCII,
			ImCompression_Binary
		};

		public enum PXCp_ImageFor3dAnnotType
		{
			Im3dAnnot_IXCObject,
			Im3dAnnot_FileName,
		};

		// Callback states
		public enum	PXC_CallbackStates
		{
			PXClb_Start			=	1,
			PXClb_Processing,
			PXClb_Finish,
		};

		public enum	PXC_WriteExFlags
		{
			WEF_ShowSaveDialog		= 0x01,
			WEF_NoWrite				= 0x02,
			WEF_AskForOverwrite		= 0x04,
			WEF_RunApp				= 0x08,
		};

		public enum	PXC_SpecVersion
		{
			SpecVersion10	= 0x10,
			SpecVersion11	= 0x11,
			SpecVersion12	= 0x12,
			SpecVersion13	= 0x13,
			SpecVersion14	= 0x14,
			SpecVersion15	= 0x15,
			SpecVersion16	= 0x16,
		};

		// Line cap styles
		public enum	PXC_LineCap
		{
			LineCap_Butt		= 0,
			LineCap_Round		= 1,
			LineCap_Square		= 2
		};

		// Line join styles
		public enum	PXC_LineJoin
		{
			LineJoin_Miter		= 0,
			LineJoin_Round		= 1,
			LineJoin_Bevel		= 2
		};

		// Fill rules
		public enum	PXC_FillRule
		{
			FillRule_Winding	= 0,
			FillRule_EvenOdd	= 1
		};

		public const int GRADIENT_FILL_RECT_H = 0x00000000;
		public const int GRADIENT_FILL_RECT_V = 0x00000001;
		public const int GRADIENT_FILL_TRIANGLE = 0x00000002;
		public const int GRADIENT_FILL_OP_FLAG = 0x000000ff;
		
		// Gradient Modes
		public enum	PXC_GradientMode
		{
			Gradient_Rect_H		= GRADIENT_FILL_RECT_H,
			Gradient_Rect_V		= GRADIENT_FILL_RECT_V,
			Gradient_Triangle	= GRADIENT_FILL_TRIANGLE
		};

		// Text rendering modes
		public enum	PXC_TextRenderingMode : int
		{
			TextRenderingMode_Fill				= 0x00,
			TextRenderingMode_Stroke			= 0x01,
			TextRenderingMode_FillStroke		= 0x02,
			TextRenderingMode_None				= 0x03,
			TextRenderingMode_Clip_Fill			= TextRenderingMode_Fill		| 0x04,
			TextRenderingMode_Clip_Stroke		= TextRenderingMode_Stroke		| 0x04,
			TextRenderingMode_Clip_FillStroke	= TextRenderingMode_FillStroke	| 0x04,
			TextRenderingMode_Clip				= TextRenderingMode_None		| 0x04
		};
		
		public const int HS_HORIZONTAL = 0;      /* ----- */
		public const int HS_VERTICAL = 1;        /* ||||| */
		public const int HS_FDIAGONAL = 2;       /* \\\\\ */
		public const int HS_BDIAGONAL = 3;       /* ///// */
		public const int HS_CROSS = 4;           /* +++++ */
		public const int HS_DIAGCROSS = 5;       /* xxxxx */

		public enum	PXC_HatchType
		{
			HatchType_Horizontal	= HS_HORIZONTAL,	/* ----- */
			HatchType_Vertical		= HS_VERTICAL,		/* ||||| */
			HatchType_FDiagonal		= HS_FDIAGONAL,		/* \\\\\ */
			HatchType_BDiagonal		= HS_BDIAGONAL,		/* ///// */
			HatchType_Cross			= HS_CROSS,			/* +++++ */
			HatchType_DiagCross		= HS_DIAGCROSS,		/* xxxxx */
		};

		// Transition styles
		public enum	PXC_TransitionStyle
		{
			TransitionStyle_Replace		= 0,
			TransitionStyle_Split		= 1,
			TransitionStyle_Blinds		= 2,
			TransitionStyle_Box			= 3,
			TransitionStyle_Wipe		= 4,
			TransitionStyle_Dissolve	= 5,
			TransitionStyle_Glitter		= 6
		};

		// Transition Options
		public enum	PXC_TransitionOption
		{
			// Dimension (for Split and Blinds styles)
			Transition_Dim_Horizontal				= 0x00,
			Transition_Dim_Vertical					= 0x01,
			// Motion direction (for Split and Box styles)
			Transition_Motion_In					= 0x00,
			Transition_Motion_Out					= 0x02,
			// Direction (for Wipe style)
			Transition_WDir_LeftToRight				= 0,
			Transition_WDir_BottomToTop				= 1,
			Transition_WDir_RightToLeft				= 2,
			Transition_WDir_TopToBottom				= 3,
			// Direction (for Glitter style)
			Transition_GDir_LeftToRight				= Transition_WDir_LeftToRight,
			Transition_GDir_TopToBottom				= Transition_WDir_TopToBottom,
			Transition_GDir_TopLeftToBottomRigth	= 4,
		};

		// Page Mode
		public enum	PXC_PageMode
		{
			PageMode_None		= 0,
			PageMode_Outlines	= 1,
			PageMode_Thumbnails	= 2,
			PageMode_FullScreen	= 3
		};

		// Page Layout
		public enum	PXC_PageLayout
		{
			PageLayout_SinglePage		= 0,
			PageLayout_OneColumn		= 1,
			PageLayout_TwoColumns_Left	= 2,
			PageLayout_TwoColumns_Right	= 3
		};

		// Viewer Preferences
		public enum	PXC_ViewerPreferences
		{
			VP_HideToolbar				= 0x0001,
			VP_HideMenubar				= 0x0002,
			VP_HideWindowUI				= 0x0004,
			VP_FitWindow				= 0x0008,
			VP_CenterWindow				= 0x0010,
			VP_DisplayDocTitle			= 0x0020,
			VP_Direction_R2L			= 0x0040,
			// Full Screen PageMode
			VP_FSPM_None				= 0x0000,	// default
			VP_FSPM_Outlines			= 0x0100,
			VP_FSPM_Tumbnails			= 0x0200,
			VP_FSPM_OC					= 0x0300,
		};

		public enum	PXC_BlendMode
		{
			BlendMode_Normal			= 0,
			BlendMode_Multiply			= 1,
			BlendMode_Screen			= 2,
			BlendMode_Overlay			= 3,
			BlendMode_Darken,
			BlendMode_Lighten,
			BlendMode_ColorDodge,
			BlendMode_ColorBurn,
			BlendMode_HardLight,
			BlendMode_SoftLight,
			BlendMode_Difference,
			BlendMode_Exclusion,
		};

		// Image scaling methods
		public enum	PXC_ScaleMethod
		{
			ScaleImage_Linear = 0,
			ScaleImage_Bilinear = 1,
			ScaleImage_Bicubic = 2,
		};

		public enum	PXC_EmbeddType
		{
			EmbeddType_NeverEmbedd = 1,
			EmbeddType_ForceEmbedd = 2,
		};

		public enum	PXC_MemImageType
		{
			MemType_1bpp		= 1,
			MemType_4bpp		= 2,
			MemType_8bpp		= 3,
			MemType_16bpp		= 4,
			MemType_24bpp		= 5,
			MemType_32bpp		= 6,
			MemType_4RLE		= 7,
			MemType_8RLE		= 8,
		};

		// Annotation flag
		public enum	PXC_AnnotationFlag
		{
			AnnotationFlag_Invisible	= 0x01,
			// spec. >= 1.2
			AnnotationFlag_Hidden		= 0x02,
			AnnotationFlag_Print		= 0x04,
			// spec. >= 1.3
			AnnotationFlag_NoZoom		= 0x08,
			AnnotationFlag_NoRotate		= 0x10,
			AnnotationFlag_NoView		= 0x20,
			AnnotationFlag_ReadOnly		= 0x40,
			// spec. >= 1.4
			AnnotationFlag_Locked		= 0x80
		};

		public enum	PXC_CompressionType
		{
			// Color/Grayscale Images
			ComprType_C_NoCompress		= 0,
			ComprType_C_JPEG			= 0x0001,
			ComprType_C_Deflate			= 0x0002,
			ComprType_C_JPEG_Deflate	= (ComprType_C_JPEG | ComprType_C_Deflate),
			ComprType_C_J2K				= 0x0004,
			ComprType_C_J2K_Deflate		= (ComprType_C_J2K | ComprType_C_Deflate),
			ComprType_C_Auto			= 0xFFFF,
			// Indexed Images
			ComprType_I_NoCompress		= ComprType_C_NoCompress,
			ComprType_I_Deflate			= ComprType_C_Deflate,
			ComprType_I_RunLength		= 0x0008,
			ComprType_I_LZW				= 0x0010,
			ComprType_I_Auto			= ComprType_C_Auto,
			// Monochrome
			ComprType_M_NoCompress		= ComprType_C_NoCompress,
			ComprType_M_Deflate			= ComprType_C_Deflate,
			ComprType_M_RunLength		= ComprType_I_RunLength,
			ComprType_M_CCITT3			= 0x0020,
			ComprType_M_CCITT4			= 0x0040,
			ComprType_M_JBIG2			= 0x0080,
			ComprType_M_Auto			= ComprType_C_Auto,
		};

		// SetInfo types
		public enum	PXC_StdInfoField
		{
			InfoField_Title				= 0,
			InfoField_Subject			= 1,
			InfoField_Author			= 2,
			InfoField_Keywords			= 3,
			InfoField_Creator			= 4,
			InfoField_Producer			= 5,
			InfoField_CreationDate		= 6,
			InfoField_ModDate			= 7
		};

		public enum	PXC_PageBox
		{
			PB_MediaBox					= 0,	// read-only
			PB_CropBox					= 1,
			PB_BleedBox					= 2,
			PB_TrimBox					= 3,
			PB_ArtBox					= 4
		};

		public const int OutlineItem_Root = 0;
		public const int OutlineItem_First = 0;
		public const int OutlineItem_Last = 1;

		public enum	PXC_OutlineStyle
		{
			OutlineStyle_Normal	= 0x0000,
			OutlineStyle_Bold	= 0x0002,
			OutlineStyle_Italic	= 0x0001,
			OutlineStyle_BoldItalic	= (OutlineStyle_Bold | OutlineStyle_Italic),
		};

		// Outline (book mark) destination modes
		public enum	PXC_OutlineDestination
		{
			Dest_Page			= 0,	// keep current display location and zoom
			Dest_XYZ			= 1,	// /XYZ left top zoom (equivalent to above)
			Dest_Fit			= 2,	// /Fit
			Dest_FitH			= 3,	// /FitH top
			Dest_FitV			= 4,	// /FitV left
			Dest_FitR			= 5,	// /FitR left bottom right top
			Dest_FitB			= 6,	// /FitB   (fit bounding box to window) PDF-1.1
			Dest_FitBH			= 7,	// /FitBH top   (fit width of bounding box to window) PDF-1.1
			Dest_FitBV			= 8,	// /FitBV left   (fit height of bounding box to window) PDF-1.1
			Dest_Y				= 9,	// /XYZ null y null
		};

		public enum	PXC_TextPosition
		{
			TextPosition_Top			= 0,
			TextPosition_Baseline		= 1,
			TextPosition_Bottom			= 2,
		};

		public enum	PXC_TextAlign
		{
			TextAlign_Left				= 0x0000,
			TextAlign_Center			= 0x0001,
			TextAlign_Right				= 0x0002,
			TextAlign_Justify			= 0x0003,
			TextAlign_FullJustify		= 0x0007,
			//
			TextAlign_Top				= 0x0000,
			TextAlign_VCenter			= 0x0010,
			TextAlign_Bottom			= 0x0020,
		};

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_TextOptions
		{
			public int cbSize;
			public int fontID;
			public double fontSize;
			//
			public PXC_TextPosition	nTextPosition;
			public PXC_TextAlign nTextAlign;
			public double LineSpacing;
			public double PapaSpacing;
			//
			public double SimItalicAngle;
			public double SimBoldThickness;
		};

		public enum	PXC_DrawTextFlags
		{
			// Aligns
			DTF_Align_Left				= 0x0000,
			DTF_Align_Center			= 0x0001,
			DTF_Align_Right				= 0x0002,
			DTF_Align_Justify			= 0x0003,
			DTF_Align_FullJustify		= 0x0007,
			DTF_Align_HorizontalMask	= 0x000F,
			DTF_Align_Top				= 0x0000,
			DTF_Align_VCenter			= 0x0010,
			DTF_Align_Bottom			= 0x0020,
			DTF_Align_VerticalMask		= 0x00F0,
			//
			DTF_CalcOnly				= 0x1000,
		};

		public enum PXC_DrawTextStructFlags
		{
			DTSF_LineSpace			= 0x0001,
			DTSF_ParagraphSpace		= 0x0002,
			DTSF_ParagraphIndent	= 0x0004,
			DTSF_FontID				= 0x0008,
			DTSF_FontSize			= 0x0010,
			DTSF_NewLine			= 0x0020,
			DTSF_CharSpace			= 0x0040,
			DTSF_WordSpace			= 0x0080,
			DTSF_textScale			= 0x0100,
		};

		public enum PXC_DrawTextNewLineMode
		{
			// new line starts new paragraph
			DTNL_NewParagraph		= 0,
			// below: double new line starts new paragraph, single new line ...
			DTNL_None				= 1,	//	ignored
			DTNL_Space				= 2,	//	converts to space character
			DTNL_SingleSpace		= 3,	//	converts to space character only if it missing between words
		};
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_DrawTextStruct
		{
			public int cbSize;
			public int mask;
			//
			public double endY;
			public int usedChars;
			//
			public double lineSpace;
			public double paraSpace;
			public double paraIndent;
			public int fontID;
			public double fontSize;
			public int newlineMode;
			public double charSpace;
			public double wordSpace;
			public double textScale;
		};
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_PointF
		{
			public double x;
			public double y;
		};
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_RectF
		{
			public double left;
			public double top;
			public double right;
			public double bottom;
		};
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_Matrix
		{
			public double	a;	//			| a b 0 |
			public double	b;	//			| c d 0 |
			public double	c;	//			| e f 1 |
			public double	d;	//
			public double	e;	// dx
			public double	f;	// dy
		};
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_FontInfo
		{
			public int cbSize;
			public double ftmHeight;
			public double ftmAscent;
			public double ftmDescent;
			public double ftmILead;
			public double ftmELead;
			public double fotmAscent;
			public double fotmDescent;
			public double fotmLineGap;
			public double fotmMacAscent;
			public double fotmMacDescent;
			public double fotmMacLineGap;
			public PXC_RectF FfontBox;
		};

		public enum	PXC_WaterPlaceType
		{
			PlaceType_AllPages  = 0,
			PlaceType_FirstPage	= 1,
			PlaceType_LastPage	= 2,
			PlaceType_EvenPages	= 3,
			PlaceType_OddPages	= 4,
			PlaceType_Range		= 5,
		};

		public enum	PXC_WaterPlaceOrder
		{
			PlaceOrder_Background	= 0,
			PlaceOrder_Foreground	= 1,
		};

		public enum	PXC_WaterType
		{
			WaterType_Text		= 0,
			WaterType_Image		= 1,
		};
		
		public const int MAX_PATH = 260;

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_Watermark
		{
			public int m_Size;
			public PXC_WaterType m_Type;			// 0 - text; 1 - image
			// Text Part
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public short[] m_FontName;
			public int m_FontWeight;
			public int m_bItalic;
			public double m_FontSize;
			public PXC_TextRenderingMode m_Mode;
			public double m_LineWidth;
			public int m_FColor;
			public int m_SColor;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public short[] m_Text;
			// Image Part
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PATH)]
			public short[] m_FileName;
			public int m_TransColor;
			public double m_Width;
			public double m_Height;
			public int m_bKeepAspect;
			// Commmon Part
			public int m_Align;
			public double m_XOffset;
			public double m_YOffset;
			//
			public double m_Angle;
			public int m_Opacity;
			// Place Info
			public PXC_WaterPlaceOrder m_PlaceOrder;
			public PXC_WaterPlaceType m_PlaceType;	// WPLACE_XXX value, or page number
			// Ranges
			public int m_NumRanges;
			public IntPtr m_Range;
			//
			public int m_ImagePageNumber;
		};

		// Security
		public enum	PXC_SecurityPermissions : uint
		{
			Permit_Printing								= (1 << 2),
			Permit_Modification							= (1 << 3),
			Permit_Copying_And_TextGraphicsExtractions	= (1 << 4),
			Permit_Add_And_Modify_Annotations			= (1 << 5),
			Permit_FormFilling							= (1 << 8),
			Permit_TextGraphicsExtractions				= (1 << 9),
			Permit_Assemble								= (1 << 10),
			Permit_HighQualityPrinting					= (1 << 11),

			Permit_Nothing								= 0xFFFFF0C0,
			Permit_All									= 0xFFFFFFFC,
		};

		// Annotations
		public enum	PXC_AnnotsFlags
		{
			AF_Invisible	= 0x0001,
			// >= 1.2
			AF_Hidden		= 0x0002,
			AF_Print		= 0x0004,
			// >= 1.3
			AF_NoZoom		= 0x0008,
			AF_NoRotate		= 0x0010,
			AF_NoView		= 0x0020,
			AF_ReadOnly		= 0x0040,
			// >= 1.4
			AF_Locked		= 0x0080,
			// >= 1.5
			AF_ToggleNoView	= 0x0100
		};

		public enum	PXC_AnnotBorderStyle
		{
			ABS_Solid		= 0,			// default
			ABS_Dashed,
			ABS_Bevel,
			ABS_Inset,
			ABS_Underline,
		};

		// Text Annotations
		public enum	PXC_TextAnnotsType
		{
			TAType_Note			= 0,		// default
			TAType_Comment,
			TAType_Key,
			TAType_Help,
			TAType_NewParagraph,
			TAType_Paragraph,
			TAType_Insert,
		};

		public enum	PXC_LineAnnotsType
		{
			LAType_None			= 0,		// default
			LAType_Square,
			LAType_Circle,
			LAType_Diamond,
			LAType_OpenArrow,
			LAType_ClosedArrow,
			// PDF Spec. >= 1.5
			LAType_Butt,
			LAType_ROpenArrow,
			LAType_RClosedArrow,
		};

		public enum	PXC_StampAnnotsType
		{
			SAType_Draft		= 0,		// default
			SAType_Approved,
			SAType_Experimental,
			SAType_NotApproved,
			SAType_AsIs,
			SAType_Expired,
			SAType_NotForPublicRelease,
			SAType_Confidential,
			SAType_Final,
			SAType_Sold,
			SAType_Departmental,
			SAType_ForComment,
			SAType_TopSecret,
			SAType_ForPublicRelease,
		};

		public enum	PXC_FileAttachmentAnnotType
		{
			FAAType_PushPin		= 0,		// default
			FAAType_Graph,
			FAAType_Paperclip,
			FAAType_Tag,
		};

		public enum	PXC_LaunchOperation
		{
			LO_Open				= 0,		// default
			LO_Print
		};

		public enum	PXC_Annot3DOptions
		{
			// Activation
			// Activate On
			ActivateOn_Explicit				= 0x0000,		// default
			ActivateOn_OnPageOpen			= 0x0001,
			ActivateOn_OnPageVisible		= 0x0002,
			// Activation Effect
			ActivationEff_Live				= 0x0000,		// default
			ActivationEff_Loaded			= 0x0010,

			// Deactivation
			// Deactivate On
			DeactivateOn_OnPageInvisible	= 0x0000,		// default
			DeactivateOn_OnPageClose		= 0x0100,
			DeactivateOn_Explicit			= 0x0200,
			// Deactivation Effect
			//
			DeactivationEff_Unloaded		= 0x0000,		// default
			DeactivationEff_Loaded			= 0x1000,
			DeactivationEff_Live			= 0x2000,
		};

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_3DView
		{
			public int m_cbSize;			// size of the structure
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
			public Int16[] m_ExtName;		// External Name
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
			public double[] m_C2W;			// matrix that specifies a position and orientation of the camera
			// in world coordinates
			public double m_CO;				// distance to the center of orbit
			public double m_FOV;				// view of the camera (in degrees)
			public int m_BackColor;
		};

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_AnnotBorder
		{
			public double m_Width;		// def: 1.0
			public PXC_AnnotBorderStyle	m_Type;			// see: PXC_AnnotBorderStyle
			public int m_DashCount;	// def: 0
			public IntPtr m_DashArray;	// def: NULL
		};

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_CommonAnnotInfo
		{
			public double m_Opacity;		// def.: 1.0; spec. >= 1.4
			public int m_Color;
			public int m_Flags;		// see PXC_AnnotsFlags
			public PXC_AnnotBorder m_Border;
		};

		// Text Extraction
		
		public enum PXP_TextElementMask
		{
			PTEM_Text = 0x1,
			PTEM_Offsets = 0x2,
			PTEM_Matrix = 0x4,
			PTEM_FontInfo = 0x8,
			PTEM_TextParams = 0x10,
		}
		
		public enum PXP_GetTextElementFlags
		{
			GTEF_OriginalCodes = 0x1,
			GTEF_OriginalDeltas = 0x2,
			GTEF_IgnorePageRotation = 0x4,
		}

		public enum PXP_TE_FontType
		{
			TEFT_Unknown = 0,
			TEFT_TrueType,
			TEFT_Type1,
			TEFT_Type3,
		}

		public enum PXP_TE_FontQuality
		{
			TEFQ_ToUnicode = 0,
			TEFQ_Encoding,
			TEFQ_BuiltIn,
			TEFQ_Approximated,
			TEFQ_NotKnown = -1,
		}

		public enum PXP_TE_TextComposeMethod
		{
			TETCM_PreserveOrder = 0,
			TETCM_PreservePositions
		}

		public enum PXP_TE_AddSpaces
		{
			TEAS_None = 0,
			TEAS_Single,
			TEAS_Proportional
		}

		public enum PXP_TE_UnecodedCharacters
		{
			TEUC_Remove = 0,
			TEUC_KeepOriginal,
			TEUC_Replace,
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXP_TEFontInfo 
		{
			public int cbSize;
			public int Flags;
			public PXP_TE_FontQuality Quality;
			public PXP_TE_FontType Type;
			public double ItalicAngle;
			public double Ascent;
			public double Descent;
		};
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXP_TETextComposeOptions 
		{
			public int cbSize;
			public int Flags;
			public double MergeDistanceX;
			public double MergeDeltaY;
			public PXP_TE_TextComposeMethod ComposeMethod;
			public PXP_TE_AddSpaces AddSpaces;
			public double MinAddSpaceDistance;
			public PXP_TE_UnecodedCharacters Undecoded;
			[MarshalAs(UnmanagedType.I2)] public char ReplaceBy;
			public short Reserved;
		};
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXP_TextElement 
		{
			public int cbSize;
            public int mask;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Characters;
			public double[] Offsets;
			public int Count;
			public int FontIndex;
			public double FontSize;
			public PXC_Matrix Matrix;
			public double CharSpace;
			public double WordSpace;
			public double Th;
			public double Leading;
			public double Rise;
			public int FillColor;
			public int StrokeColor;
			public PXC_TextRenderingMode RenderingMode;
		};

		// Shades
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PXC_TRIVERTEX
		{
			public double x;
			public double y;
			public int color;
		};

		//
		public static string GetSeparatedString(string str)
		{
			string ret_str = str;
			int len = str.Length;
			int c = len - ((len / 3) * 3);
            for (int i = c; i < len; i += 3)
            {
                if (i > 0)
                {
                    ret_str = ret_str.Insert(i, " ");
                    i++;
                }
            }

			return ret_str;
		}

		public static int GetFileSize(string filename)
		{
			System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Open);
			int len = (int)fs.Length;
			fs.Close();
			return len;
		}
		
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto, Pack = 1)]
		public struct BROWSEINFO
		{
			public uint hwndOwner;
			public uint pidlRoot;
			[MarshalAs(UnmanagedType.LPStr)] 
			public string pszDisplayName;
			[MarshalAs(UnmanagedType.LPStr)] 
			public string lpszTitle;
			public uint ulFlags;
			public uint lpfn;
			public uint lParam;
			public int iImage;
		}

		[DllImport("shell32")]
		public static extern int SHBrowseForFolder(ref BROWSEINFO lpbi);
		[DllImport("shell32")]
		public static extern int SHGetPathFromIDList(int pidl, System.Text.StringBuilder pszPath);

		public static void StringToShortArr(string text, short[] arr)
		{
			for (int i = 0; i < text.Length; i++)
			{
				arr[i] = (short)text[i];
			}
		}

		public static string GetSelectedImage(System.Windows.Forms.OpenFileDialog openFileDlg)
		{
			openFileDlg.Filter = "All image files|*.*";
            
            if (System.Windows.Forms.DialogResult.OK == openFileDlg.ShowDialog())
            {
                return openFileDlg.FileName;
            }

			return "";
		}
	}
}
