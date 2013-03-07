using System;
using System.Runtime.InteropServices;

namespace MarkMars.PDF
{
    /// <summary>
    /// Summary description for PDFXC_Funcs.
    /// </summary>
    public class PDFXC_Funcs
    {
        public delegate bool CALLBACK_FUNC(int state, int level, int param);
        #region "Document operations"
        [DllImport("pxclib40")]
        public static extern int PXC_EnableLinkAnalyzer(int pdf, bool bEnable);
        [DllImport("pxclib40")]
        public static extern int PXC_EnableSecurity(int pdf, bool bEnable, [MarshalAs(UnmanagedType.LPStr)]string UserPwd,
            [MarshalAs(UnmanagedType.LPStr)]string OwnerPwd);
        [DllImport("pxclib40")]
        public static extern int PXC_GetCompression(int pdf, ref bool bText, ref bool bAscii, ref PXC_CompressionType cColor,
            ref int jpegQual, ref PXC_CompressionType cIndexed, ref PXC_CompressionType cMono);
        [DllImport("pxclib40")]
        public static extern int PXC_NewDocument(out int pdf, [MarshalAs(UnmanagedType.LPStr)]string key,
            [MarshalAs(UnmanagedType.LPStr)]string devCode);
        [DllImport("pxclib40")]
        public static extern int PXC_ReleaseDocument(int pdf);
        [DllImport("pxclib40")]
        public static extern int PXC_SetCallback(int pdf, CALLBACK_FUNC clbFn, int clbParam);
        [DllImport("pxclib40")]
        public static extern int PXC_SetCompression(int pdf, bool bText, bool bAscii, PXC_CompressionType cColor,
            int jpegQual, PXC_CompressionType cIndexed, PXC_CompressionType cMono);
        [DllImport("pxclib40")]
        public static extern int PXC_SetDocumentInfoA(int pdf, PXC_StdInfoField field, [MarshalAs(UnmanagedType.LPStr)]string val);
        [DllImport("pxclib40")]
        public static extern int PXC_SetDocumentInfoExA(int pdf, [MarshalAs(UnmanagedType.LPStr)]string key,
            [MarshalAs(UnmanagedType.LPStr)]string val);
        [DllImport("pxclib40")]
        public static extern int PXC_SetDocumentInfoExW(int pdf, [MarshalAs(UnmanagedType.LPWStr)]string key,
            [MarshalAs(UnmanagedType.LPWStr)]string val);
        [DllImport("pxclib40")]
        public static extern int PXC_SetDocumentInfoW(int pdf, PXC_StdInfoField field, [MarshalAs(UnmanagedType.LPWStr)]string val);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPermissions(int pdf, int enclevel, int permFlags);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPermissions128(int pdf, int bComments, int bCopyExtract, int Changing, int Printing);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPermissions40(int pdf, int bComments, int bCopyExtract, int Changing, int Printing);
        [DllImport("pxclib40")]
        public static extern int PXC_SetSpecVersion(int pdf, PXC_SpecVersion ver, bool bCompatMode);
        [DllImport("pxclib40")]
        public static extern int PXC_SetViewerPreferences(int pdf, int vprefs);
        [DllImport("pxclib40")]
        public static extern int PXC_WriteDocumentA(int pdf, [MarshalAs(UnmanagedType.LPStr)]string fName);
        [DllImport("pxclib40")]
        public static extern int PXC_WriteDocumentExA(int pdf, [MarshalAs(UnmanagedType.LPStr)]string lpszFileName,
            int dwFileNameLen, int dwFlags, [MarshalAs(UnmanagedType.LPStr)]string appname);
        [DllImport("pxclib40")]
        public static extern int PXC_WriteDocumentW(int pdf, [MarshalAs(UnmanagedType.LPWStr)]string fName);
        [DllImport("pxclib40")]
        public static extern int PXC_WriteDocumentExW(int pdf, [MarshalAs(UnmanagedType.LPWStr)]string lpszFileName,
            int dwFileNameLen, int dwFlags, [MarshalAs(UnmanagedType.LPWStr)]string appname);
        #endregion
        #region "Pages operations"
        [DllImport("pxclib40")]
        public static extern int PXC_AddPage(int pdf, double width, double height, out int page);
        [DllImport("pxclib40")]
        public static extern int PXC_GetPage(int pdf, int index, out int page);
        [DllImport("pxclib40")]
        public static extern int PXC_GetPageIndex(int pdf, int page, out int index);
        [DllImport("pxclib40")]
        public static extern int PXC_GetPageRotation(int page, out int angle);
        [DllImport("pxclib40")]
        public static extern int PXC_GetPagesCount(int pdf, out int count);
        [DllImport("pxclib40")]
        public static extern int PXC_GetPageSize(int page, out double width, out double height);
        [DllImport("pxclib40")]
        public static extern int PXC_InsertPage(int pdf, int index, double width, double height, out int page);
        [DllImport("pxclib40")]
        public static extern int PXC_RemovePage(int pdf, int index);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPageDuration(int page, int miliseconds);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPageLayout(int pdf, PXC_PageLayout layout);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPageMode(int pdf, PXC_PageMode mode);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPageRotation(int page, int angle);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPageTransition(int page, PXC_TransitionStyle style, int miliseconds,
            int v1, int v2);

        #endregion
        #region "Coordinate system"
        [DllImport("pxclib40")]
        public static extern int PXC_CS_Get(int content, PXC_Matrix m);
        [DllImport("pxclib40")]
        public static extern int PXC_CS_Rotate(int content, double phi);
        [DllImport("pxclib40")]
        public static extern int PXC_CS_Scale(int content, double sx, double sy);
        [DllImport("pxclib40")]
        public static extern int PXC_CS_Skew(int content, double alpha, double beta);
        [DllImport("pxclib40")]
        public static extern int PXC_CS_Translate(int content, double dx, double dy);
        [DllImport("pxclib40")]
        public static extern int PXC_CS_Concat(int content, ref PXC_Matrix m);
        #endregion
        #region "Text and Fonts"
        [DllImport("pxclib40")]
        public static extern int PXC_AddFontA(int pdf, int dwWeight, bool bItalic,
            [MarshalAs(UnmanagedType.LPStr)]string fName, out int font);
        [DllImport("pxclib40")]
        public static extern int PXC_AddFontFromFileA(int pdf, [MarshalAs(UnmanagedType.LPStr)]string fName,
            [MarshalAs(UnmanagedType.LPStr)]string aName, out int font);
        [DllImport("pxclib40")]
        public static extern int PXC_AddFontFromFileW(int pdf, [MarshalAs(UnmanagedType.LPWStr)]string fName,
            [MarshalAs(UnmanagedType.LPWStr)]string aName, out int font);
        [DllImport("pxclib40")]
        public static extern int PXC_AddFontW(int pdf, int dwWeight, bool bItalic,
            [MarshalAs(UnmanagedType.LPWStr)]string fName, out int font);
        [DllImport("pxclib40")]
        public static extern int PXC_ClearNoEmbeddList(int pdf);
        [DllImport("pxclib40")]
        public static extern int PXC_GetFontInfo(int content, out PXC_FontInfo fInfo);
        [DllImport("pxclib40")]
        public static extern int PXC_SetEmbeddingOptions(int pdf, bool bAllowEmbedding, bool bForceEmbedding,
            bool bToUnicode);
        [DllImport("pxclib40")]
        public static extern int PXC_SetFontEmbeddA(int pdf, [MarshalAs(UnmanagedType.LPStr)]string lpszFontName,
            PXC_EmbeddType bEmbedd);
        [DllImport("pxclib40")]
        public static extern int PXC_SetFontEmbeddW(int pdf, [MarshalAs(UnmanagedType.LPWStr)]string lpszFontName,
            PXC_EmbeddType bEmbedd);
        [DllImport("pxclib40")]
        public static extern int PXC_GetStringWidthW(int content, [MarshalAs(UnmanagedType.LPWStr)]string lpwszText,
            int cbLen, out double width);
        [DllImport("pxclib40")]
        public static extern int PXC_GetTextOptions(int content, out PXC_TextOptions options);
        [DllImport("pxclib40")]
        public static extern int PXC_SetCharSpacing(int content, double cs, out double oldcs);
        [DllImport("pxclib40")]
        public static extern int PXC_SetCurrentFont(int content, int fontID, double fSize);
        [DllImport("pxclib40")]
        public static extern int PXC_GetCurrentFont(int content, out int fontID, out double fSize);
        [DllImport("pxclib40")]
        public static extern int PXC_DrawTextExW(int content, ref PXC_RectF rect, [MarshalAs(UnmanagedType.LPWStr)]string str,
            int sPos, int len, int flags, ref PXC_DrawTextStruct lpOptions);
        [DllImport("pxclib40")]
        public static extern int PXC_GetStringWidthA(int content, [MarshalAs(UnmanagedType.LPWStr)]string lpszText,
            int cbLen, out double width);
        [DllImport("pxclib40")]
        public static extern int PXC_SetTextOptions(int content, ref PXC_TextOptions options);
        [DllImport("pxclib40")]
        public static extern int PXC_SetTextRise(int content, double rise, ref double oldrise);
        [DllImport("pxclib40")]
        public static extern int PXC_SetTextRMode(int content, PXC_TextRenderingMode mode, out PXC_TextRenderingMode oldmode);
        [DllImport("pxclib40")]
        public static extern int PXC_SetTextScaling(int content, double scale, ref double oldscale);
        [DllImport("pxclib40")]
        public static extern int PXC_SetWordSpacing(int content, double ws, ref double oldws);
        [DllImport("pxclib40")]
        public static extern int PXC_TCS_Get(int content, out PXC_Matrix m);
        [DllImport("pxclib40")]
        public static extern int PXC_TCS_Transform(int content, ref PXC_Matrix m);
        [DllImport("pxclib40")]
        public static extern int PXC_TextOutA(int content, ref PXC_PointF origin,
            [MarshalAs(UnmanagedType.LPStr)]string lpszText, int cbLen);
        [DllImport("pxclib40")]
        public static extern int PXC_TextOutW(int content, ref PXC_PointF origin,
            [MarshalAs(UnmanagedType.LPWStr)]string lpszText, int cbLen);
        #endregion
        #region "Images"
        [DllImport("pxclib40")]
        public static extern int PXC_AddEnhMetafile(int pdf, IntPtr metafile, out int image);
        [DllImport("pxclib40")]
        public static extern int PXC_AddImageA(int pdf, [MarshalAs(UnmanagedType.LPStr)]string filename,
            out int image);
        [DllImport("pxclib40")]
        public static extern int PXC_AddImageExA(int pdf, [MarshalAs(UnmanagedType.LPStr)]string filename,
            int[] image, int pages);
        [DllImport("pxclib40")]
        public static extern int PXC_AddImageExW(int pdf, [MarshalAs(UnmanagedType.LPWStr)]string filename,
            int[] image, int pages);
        [DllImport("pxclib40")]
        public static extern int PXC_AddImageFromHBITMAP(int pdf, IntPtr hbm, IntPtr hpal, out int img);
        [DllImport("pxclib40")]
        public static extern int PXC_AddImageFromImageXChangePage(int pdf, IntPtr page, bool bClone, out int img);
        [DllImport("pxclib40")]
        public static extern int PXC_AddImageW(int pdf, [MarshalAs(UnmanagedType.LPWStr)]string filename,
            out int image);
        [DllImport("pxclib40")]
        public static extern int PXC_AddStdMetafile(int pdf, IntPtr metafile, out int image);
        [DllImport("pxclib40")]
        public static extern int PXC_CloseImage(int pdf, int image);
        [DllImport("pxclib40")]
        public static extern int PXC_CropImage(int pdf, int image, ref RECT croprect);
        [DllImport("pxclib40")]
        public static extern int PXC_GetImageColors(int pdf, int image);
        [DllImport("pxclib40")]
        public static extern int PXC_GetImageDimension(int pdf, int image, out double width, out double height);
        [DllImport("pxclib40")]
        public static extern int PXC_GetImageDPI(int pdf, int image, out int xdpi, out int ydpi);
        [DllImport("pxclib40")]
        public static extern int PXC_MakeImageGrayscale(int pdf, int image);
        [DllImport("pxclib40")]
        public static extern int PXC_MarkImageAsMask(int pdf, int image, bool bMask);
        [DllImport("pxclib40")]
        public static extern int PXC_PlaceImage(int content, int image, double x, double y,
            double width, double height);
        [DllImport("pxclib40")]
        public static extern int PXC_ReduceImageColors(int pdf, int image, int depth, bool bGrayscale,
            bool dither, bool optimal);
        [DllImport("pxclib40")]
        public static extern int PXC_ScaleImage(int pdf, int image, int width, int height, bool bProp,
            int method);
        [DllImport("pxclib40")]
        public static extern int PXC_SetImageMask(int pdf, int image, int mask);
        [DllImport("pxclib40")]
        public static extern int PXC_SetImageTransColor(int pdf, int image, int color);
        [DllImport("pxclib40")]
        public static extern int PXC_AddImagePattern(int pdf, int image);
        [DllImport("pxclib40")]
        public static extern int PXC_AddHatchPatternEx(int pdf, PXC_HatchType type, double w, double h,
            int fore, int back);
        [DllImport("pxclib40")]
        public static extern int PXC_AddHatchPattern(int pdf, PXC_HatchType type);
        #endregion
        #region "Drawing"
        [DllImport("pxclib40")]
        public static extern int PXC_Arc(int content, ref PXC_PointF center, double r, double alpha, double beta);
        [DllImport("pxclib40")]
        public static extern int PXC_ArcN(int content, ref PXC_PointF center, double r, double alpha, double beta);
        [DllImport("pxclib40")]
        public static extern int PXC_Chord(int content, ref PXC_RectF rect, double alpha, double beta);
        [DllImport("pxclib40")]
        public static extern int PXC_ChordEx(int content, ref PXC_RectF rect, ref PXC_PointF pnt1, ref PXC_PointF pnt2);
        [DllImport("pxclib40")]
        public static extern int PXC_Circle(int content, ref PXC_PointF center, double r);
        [DllImport("pxclib40")]
        public static extern int PXC_ClipPath(int content, PXC_FillRule mode);
        [DllImport("pxclib40")]
        public static extern int PXC_ClosePath(int content);
        [DllImport("pxclib40")]
        public static extern int PXC_CurveTo(int content, ref PXC_PointF pnt1, ref PXC_PointF pnt2, ref PXC_PointF pnt3);
        [DllImport("pxclib40")]
        public static extern int PXC_Ellipse(int content, ref PXC_RectF rect);
        [DllImport("pxclib40")]
        public static extern int PXC_EllipseArc(int content, ref PXC_RectF rect, double alpha, double beta);
        [DllImport("pxclib40")]
        public static extern int PXC_EllipseArcEx(int content, ref PXC_RectF rect, ref PXC_PointF pnt1, ref PXC_PointF pnt2);
        [DllImport("pxclib40")]
        public static extern int PXC_EndPath(int content);
        [DllImport("pxclib40")]
        public static extern int PXC_LineTo(int content, double x, double y);
        [DllImport("pxclib40")]
        public static extern int PXC_MoveTo(int content, double x, double y);
        [DllImport("pxclib40")]
        public static extern int PXC_Pie(int content, ref PXC_RectF rect, double alpha, double beta);
        [DllImport("pxclib40")]
        public static extern int PXC_PieEx(int content, ref PXC_RectF rect, ref PXC_PointF pnt1, ref PXC_PointF pnt2);
        [DllImport("pxclib40")]
        public static extern int PXC_PolyCurve(int content, PXC_PointF[] points, int pntCount);
        [DllImport("pxclib40")]
        public static extern int PXC_Polygon(int content, PXC_PointF[] points, int pntCount, bool bMove);
        [DllImport("pxclib40")]
        public static extern int PXC_Rect(int content, double left, double top, double right, double bottom);
        [DllImport("pxclib40")]
        public static extern int PXC_RoundRect(int content, double left, double top, double right, double bottom,
            double ew, double eh);
        [DllImport("pxclib40")]
        public static extern int PXC_GetLineInfo(int content, out double width, out PXC_LineJoin join, out PXC_LineCap cap,
            out double mlimit);
        [DllImport("pxclib40")]
        public static extern int PXC_GradientFill(int content, PXC_TRIVERTEX[] pVertex, int dwNumVertex,
            IntPtr pMesh, int dwNumMesh, PXC_GradientMode dwMode);
        [DllImport("pxclib40")]
        public static extern int PXC_ApplyPattern(int content, int patID, bool bForStroke, int patColor);
        [DllImport("pxclib40")]
        public static extern int PXC_FillPath(int content, bool bClose, bool bStroke, PXC_FillRule mode);
        [DllImport("pxclib40")]
        public static extern int PXC_SetBlendMode(int content, PXC_BlendMode bMode);
        [DllImport("pxclib40")]
        public static extern int PXC_SetDash(int content, double b, double w, double offs);
        [DllImport("pxclib40")]
        public static extern int PXC_SetDrawingColor(int content, int color);
        [DllImport("pxclib40")]
        public static extern int PXC_SetDrawingGray(int content, byte gLevel);
        [DllImport("pxclib40")]
        public static extern int PXC_SetFillColor(int content, int color);
        [DllImport("pxclib40")]
        public static extern int PXC_SetFillGray(int content, byte gLevel);
        [DllImport("pxclib40")]
        public static extern int PXC_SetFlat(int content, double flat_tolerance);
        [DllImport("pxclib40")]
        public static extern int PXC_SetLineCap(int content, PXC_LineCap cap);
        [DllImport("pxclib40")]
        public static extern int PXC_SetLineJoin(int content, PXC_LineJoin join);
        [DllImport("pxclib40")]
        public static extern int PXC_SetLineWidth(int content, double width);
        [DllImport("pxclib40")]
        public static extern int PXC_SetMiterLimit(int content, double mlimit);
        [DllImport("pxclib40")]
        public static extern int PXC_SetPolyDash(int content, double[] darray, int arCount, double offs);
        [DllImport("pxclib40")]
        public static extern int PXC_SetStrokeAdjust(int content, bool bAdjust);
        [DllImport("pxclib40")]
        public static extern int PXC_SetStrokeColor(int content, int color);
        [DllImport("pxclib40")]
        public static extern int PXC_SetStrokeGray(int content, byte gLevel);
        [DllImport("pxclib40")]
        public static extern int PXC_SetTransparency(int content, byte tFill, byte tStroke);
        [DllImport("pxclib40")]
        public static extern int PXC_StrokePath(int content, bool bClose);
        [DllImport("pxclib40")]
        public static extern int PXC_NoDash(int content);
        [DllImport("pxclib40")]
        public static extern int PXC_GetContentDC(int content, IntPtr refDC, ref RECT drawRect, ref PXC_RectF crect,
            ref IntPtr cdc);
        [DllImport("pxclib40")]
        public static extern int PXC_SaveState(int content);
        [DllImport("pxclib40")]
        public static extern int PXC_GetStateLevel(int content, out int level);
        [DllImport("pxclib40")]
        public static extern int PXC_ReleaseContentDC(int content, bool bCancel);
        [DllImport("pxclib40")]
        public static extern int PXC_RestoreState(int content);
        #endregion
        #region "PDF Viewing/Display functions"
        [DllImport("pxclib40")]
        public static extern int PXC_AddGotoAction(int content, ref PXC_RectF rect, int page, PXC_OutlineDestination mode,
            double v1, double v2, double v3, double v4, ref PXC_CommonAnnotInfo pInfo);
        [DllImport("pxclib40")]
        public static extern int PXC_AddLaunchActionA(int content, ref PXC_RectF rect,
            [MarshalAs(UnmanagedType.LPStr)]string lpszFileName, [MarshalAs(UnmanagedType.LPStr)]string lpszParams,
            PXC_LaunchOperation oper, ref PXC_CommonAnnotInfo pInfo);
        [DllImport("pxclib40")]
        public static extern int PXC_AddLaunchActionW(int content, ref PXC_RectF rect,
            [MarshalAs(UnmanagedType.LPWStr)]string lpwszFileName, [MarshalAs(UnmanagedType.LPWStr)]string lpwszParams,
            PXC_LaunchOperation oper, ref PXC_CommonAnnotInfo pInfo);
        [DllImport("pxclib40")]
        public static extern int PXC_AddLineAnnotation(int content, ref PXC_PointF pntStart, ref PXC_PointF pntEnd,
            PXC_LineAnnotsType sEndStyle, PXC_LineAnnotsType eEndStyle, int cInterior, ref PXC_CommonAnnotInfo pInfo);
        [DllImport("pxclib40")]
        public static extern int PXC_AddLink(int content, ref PXC_RectF rect, [MarshalAs(UnmanagedType.LPStr)]string lpszURL,
            ref PXC_CommonAnnotInfo pInfo);
        [DllImport("pxclib40")]
        public static extern IntPtr PXC_AddOutlineEntryW(int pdf, IntPtr parent, IntPtr after, bool open, int page,
            [MarshalAs(UnmanagedType.LPWStr)]string title, PXC_OutlineDestination mode, double v1, double v2,
            double v3, double v4, int color, int flags);
        [DllImport("pxclib40")]
        public static extern int PXC_AddTextAnnotationA(int content, ref PXC_RectF rect,
            [MarshalAs(UnmanagedType.LPStr)]string pszTitle, [MarshalAs(UnmanagedType.LPStr)]string pszAnnot,
            PXC_TextAnnotsType type, ref PXC_CommonAnnotInfo pInfo);
        [DllImport("pxclib40")]
        public static extern int PXC_AddTextAnnotationW(int content, ref PXC_RectF rect,
            [MarshalAs(UnmanagedType.LPWStr)]string pszTitle, [MarshalAs(UnmanagedType.LPWStr)]string pszAnnot,
            PXC_TextAnnotsType type, ref PXC_CommonAnnotInfo pInfo);
        [DllImport("pxclib40")]
        public static extern int PXC_AddWatermark(int pdf, ref PXC_Watermark watermark);
        [DllImport("pxclib40")]
        public static extern int PXC_SetAnnotsInfo(int pdf, ref PXC_CommonAnnotInfo pInfo);
        #endregion
        #region "Error handling"
        [DllImport("pxclib40")]
        public static extern int PXC_Err_FormatSeverity(int errorcode, byte[] buf, int maxlen);
        [DllImport("pxclib40")]
        public static extern int PXC_Err_FormatFacility(int errorcode, byte[] buf, int maxlen);
        [DllImport("pxclib40")]
        public static extern int PXC_Err_FormatErrorCode(int errorcode, byte[] buf, int maxlen);
        #endregion
        #region "Structures and constants"
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct GRADIENT_TRIANGLE
        {
            public int Vertex1;
            public int Vertex2;
            public int Vertex3;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct GRADIENT_RECT
        {
            public int UpperLeft;
            public int LowerRight;
        }

        public const int MAX_PATH = 260;
        public const int DS_OK = 0;
        // Callback states
        public enum PXC_CallbackStates
        {
            PXClb_Start = 1,
            PXClb_Processing,
            PXClb_Finish,
        }
        public enum PXC_WriteExFlags
        {
            WEF_ShowSaveDialog = 0x01,
            WEF_NoWrite = 0x02,
            WEF_AskForOverwrite = 0x04,
            WEF_RunApp = 0x08,
        }
        public enum PXC_SpecVersion
        {
            SpecVersion10 = 0x10,
            SpecVersion11 = 0x11,
            SpecVersion12 = 0x12,
            SpecVersion13 = 0x13,
            SpecVersion14 = 0x14,
            SpecVersion15 = 0x15,
        }
        // Line cap styles
        public enum PXC_LineCap
        {
            LineCap_Butt = 0,
            LineCap_Round = 1,
            LineCap_Square = 2
        }
        // Line join styles
        public enum PXC_LineJoin
        {
            LineJoin_Miter = 0,
            LineJoin_Round = 1,
            LineJoin_Bevel = 2
        };

        // Fill rules
        public enum PXC_FillRule
        {
            FillRule_Winding = 0,
            FillRule_EvenOdd = 1
        }

        // Gradient Modes
        public enum PXC_GradientMode
        {
            Gradient_Rect_H = 0,
            Gradient_Rect_V = 1,
            Gradient_Triangle = 2
        }

        // Text rendering modes
        public enum PXC_TextRenderingMode
        {
            TextRenderingMode_Fill = 0x00,
            TextRenderingMode_Stroke = 0x01,
            TextRenderingMode_FillStroke = 0x02,
            TextRenderingMode_None = 0x03,
            TextRenderingMode_Clip_Fill = TextRenderingMode_Fill | 0x04,
            TextRenderingMode_Clip_Stroke = TextRenderingMode_Stroke | 0x04,
            TextRenderingMode_Clip_FillStroke = TextRenderingMode_FillStroke | 0x04,
            TextRenderingMode_Clip = TextRenderingMode_None | 0x04
        }

        public enum PXC_HatchType
        {
            HatchType_Horizontal = 0,	/* ----- */
            HatchType_Vertical = 1,		/* ||||| */
            HatchType_FDiagonal = 2,		/* \\\\\ */
            HatchType_BDiagonal = 3,		/* ///// */
            HatchType_Cross = 4,			/* +++++ */
            HatchType_DiagCross = 5,		/* xxxxx */
        }

        // Transition styles
        public enum PXC_TransitionStyle
        {
            TransitionStyle_Replace = 0,
            TransitionStyle_Split = 1,
            TransitionStyle_Blinds = 2,
            TransitionStyle_Box = 3,
            TransitionStyle_Wipe = 4,
            TransitionStyle_Dissolve = 5,
            TransitionStyle_Glitter = 6
        }

        // Transition Options
        public enum PXC_TransitionOption
        {
            // Dimention (for Split and Blinds styles)
            Transition_Dim_Horizontal = 0x00,
            Transition_Dim_Vertical = 0x01,
            // Motion direction (for Split and Box styles)
            Transition_Motion_In = 0x00,
            Transition_Motion_Out = 0x02,
            // Direction (for Wipe style)
            Transition_WDir_LeftToRight = 0,
            Transition_WDir_BottomToTop = 1,
            Transition_WDir_RightToLeft = 2,
            Transition_WDir_TopToBottom = 3,
            // Direction (for Glitter style)
            Transition_GDir_LeftToRight = Transition_WDir_LeftToRight,
            Transition_GDir_TopToBottom = Transition_WDir_TopToBottom,
            Transition_GDir_TopLeftToBottomRigth = 4,
        }

        // Page Mode
        public enum PXC_PageMode
        {
            PageMode_None = 0,
            PageMode_Outlines = 1,
            PageMode_Thumbnails = 2,
            PageMode_FullScreen = 3
        }

        // Page Layout
        public enum PXC_PageLayout
        {
            PageLayout_SinglePage = 0,
            PageLayout_OneColumn = 1,
            PageLayout_TwoColumns_Left = 2,
            PageLayout_TwoColumns_Right = 3
        };

        // Viewer Preferences
        public enum PXC_ViewerPreferences
        {
            VP_HideToolbar = 0x0001,
            VP_HideMenubar = 0x0002,
            VP_HideWindowUI = 0x0004,
            VP_FitWindow = 0x0008,
            VP_CenterWindow = 0x0010,
            VP_DisplayDocTitle = 0x0020,
            VP_Direction_R2L = 0x0040,
            // Full Screen PageMode
            VP_FSPM_None = 0x0000,	// default
            VP_FSPM_Outlines = 0x0100,
            VP_FSPM_Tumbnails = 0x0200,
            VP_FSPM_OC = 0x0300,
        }

        public enum PXC_BlendMode
        {
            BlendMode_Normal = 0,
            BlendMode_Multiply = 1,
            BlendMode_Screen = 2,
            BlendMode_Overlay = 3,
            BlendMode_Darken,
            BlendMode_Lighten,
            BlendMode_ColorDodge,
            BlendMode_ColorBurn,
            BlendMode_HardLight,
            BlendMode_SoftLight,
            BlendMode_Difference,
            BlendMode_Exclusion,
        }

        // Image scaling methods
        public enum PXC_ScaleMethod
        {
            ScaleImage_Linear = 0,
            ScaleImage_Bilinear = 1,
            ScaleImage_Bicubic = 2,
        }

        public enum PXC_EmbeddType
        {
            EmbeddType_NeverEmbedd = 1,
            EmbeddType_ForceEmbedd = 2,
        }

        // Annotation flag
        public enum PXC_AnnotationFlag
        {
            AnnotationFlag_Invisible = 0x01,
            // spec. >= 1.2
            AnnotationFlag_Hidden = 0x02,
            AnnotationFlag_Print = 0x04,
            // spec. >= 1.3
            AnnotationFlag_NoZoom = 0x08,
            AnnotationFlag_NoRotate = 0x10,
            AnnotationFlag_NoView = 0x20,
            AnnotationFlag_ReadOnly = 0x40,
            // spec. >= 1.4
            AnnotationFlag_Locked = 0x80
        }

        public enum PXC_CompressionType
        {
            // Color/Grayscale Images
            ComprType_C_NoCompress = 0,
            ComprType_C_JPEG = 0x0001,
            ComprType_C_Deflate = 0x0002,
            ComprType_C_JPEG_Deflate = (ComprType_C_JPEG | ComprType_C_Deflate),
            ComprType_C_J2K = 0x0004,
            ComprType_C_J2K_Deflate = (ComprType_C_J2K | ComprType_C_Deflate),
            ComprType_C_Auto = 0xFFFF,
            // Indexed Images
            ComprType_I_NoCompress = ComprType_C_NoCompress,
            ComprType_I_Deflate = ComprType_C_Deflate,
            ComprType_I_RunLength = 0x0008,
            ComprType_I_LZW = 0x0010,
            ComprType_I_Auto = ComprType_C_Auto,
            // Monochrome
            ComprType_M_NoCompress = ComprType_C_NoCompress,
            ComprType_M_Deflate = ComprType_C_Deflate,
            ComprType_M_RunLength = ComprType_I_RunLength,
            ComprType_M_CCITT3 = 0x0020,
            ComprType_M_CCITT4 = 0x0040,
            ComprType_M_JBIG2 = 0x0080,
            ComprType_M_Auto = ComprType_C_Auto,
        }

        // SetInfo types
        public enum PXC_StdInfoField
        {
            InfoField_Title = 0,
            InfoField_Subject = 1,
            InfoField_Author = 2,
            InfoField_Keywords = 3,
            InfoField_Creator = 4,
            InfoField_Producer = 5
        }

        public const int OutlineItem_Root = 0;
        public const int OutlineItem_First = 0;
        public const int OutlineItem_Last = 1;

        public enum PXC_OutlineStyle
        {
            OutlineStyle_Normal = 0x0000,
            OutlineStyle_Bold = 0x0002,
            OutlineStyle_Italic = 0x0001,
            OutlineStyle_BoldItalic = (OutlineStyle_Bold | OutlineStyle_Italic),
        }

        // Outline (book mark) destination modes
        public enum PXC_OutlineDestination
        {
            Dest_Page = 0,	// keep current display location and zoom
            Dest_XYZ = 1,	// /XYZ left top zoom (equivalent to above)
            Dest_Fit = 2,	// /Fit
            Dest_FitH = 3,	// /FitH top
            Dest_FitV = 4,	// /FitV left
            Dest_FitR = 5,	// /FitR left bottom right top
            Dest_FitB = 6,	// /FitB   (fit bounding box to window) PDF-1.1
            Dest_FitBH = 7,	// /FitBH top   (fit width of bounding box to window) PDF-1.1
            Dest_FitBV = 8,	// /FitBV left   (fit height of bounding box to window) PDF-1.1
            Dest_Y = 9,	// /XYZ null y null
        }

        public enum PXC_TextPosition
        {
            TextPosition_Top = 0,
            TextPosition_Baseline = 1,
            TextPosition_Bottom = 2,
        }

        public enum PXC_TextAlign
        {
            TextAlign_Left = 0x0000,
            TextAlign_Center = 0x0001,
            TextAlign_Right = 0x0002,
            TextAlign_Justify = 0x0003,
            TextAlign_FullJustify = 0x0007,
            //
            TextAlign_Top = 0x0000,
            TextAlign_VCenter = 0x0010,
            TextAlign_Bottom = 0x0020,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PXC_TextOptions
        {
            public int cbSize;
            public int fontID;
            public double fontSize;
            //
            public PXC_TextPosition nTextPosition;
            public PXC_TextAlign nTextAlign;
            public double LineSpacing;
            public double PapaSpacing;
            //
            public double SimItalicAngle;
            public double SimBoldThickness;
        }

        public enum PXC_DrawTextFlags
        {
            // Aligns
            DTF_Align_Left = 0x0000,
            DTF_Align_Center = 0x0001,
            DTF_Align_Right = 0x0002,
            DTF_Align_Justify = 0x0003,
            DTF_Align_FullJustify = 0x0007,
            DTF_Align_HorizontalMask = 0x000F,
            DTF_Align_Top = 0x0000,
            DTF_Align_VCenter = 0x0010,
            DTF_Align_Bottom = 0x0020,
            DTF_Align_VerticalMask = 0x00F0,
            //
            DTF_CalcOnly = 0x1000,
        }

        public enum PXC_DrawTextStructFlags
        {
            DTSF_LineSpace = 0x0001,
            DTSF_ParagraphSpace = 0x0002,
            DTSF_ParagraphIndent = 0x0004,
            DTSF_FontID = 0x0008,
            DTSF_FontSize = 0x0010,
            DTSF_NewLine = 0x0020,
            DTSF_CharSpace = 0x0040,
            DTSF_WordSpace = 0x0080,
            DTSF_textScale = 0x0100,
        }

        public enum PXC_DrawTextNewLineMode
        {
            // new line starts new paragraph
            DTNL_NewParagraph = 0,
            // below: double new line starts new paragraph, single new line ...
            DTNL_None = 1,	//	ignored
            DTNL_Space = 2,	//	converts to space character
            DTNL_SingleSpace = 3,	//	converts to space character only if it missing between words
        }

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
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PXC_PointF
        {
            public double x;
            public double y;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PXC_RectF
        {
            public double left;
            public double top;
            public double right;
            public double bottom;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PXC_Matrix
        {
            public double a;	//			| a b 0 |
            public double b;	//			| c d 0 |
            public double c;	//			| e f 1 |
            public double d;	//
            public double e;	// dx
            public double f;	// dy
        }

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
            public PXC_RectF fontBox;
        }

        public enum PXC_WaterPlaceType
        {
            PlaceType_AllPages = 0,
            PlaceType_FirstPage = 1,
            PlaceType_LastPage = 2,
            PlaceType_EvenPages = 3,
            PlaceType_OddPages = 4,
            PlaceType_Range = 5,
        }

        public enum PXC_WaterPlaceOrder
        {
            PlaceOrder_Background = 0,
            PlaceOrder_Foreground = 1,
        }

        public enum PXC_WaterType
        {
            WaterType_Text = 0,
            WaterType_Image = 1,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PXC_Watermark
        {
            public int m_Size;
            public PXC_WaterType m_Type;			// 0 - text; 1 - image
            // Text Part
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public Int16[] m_FontName;
            public int m_FontWeight;
            public bool m_bItalic;
            public double m_FontSize;
            public PXC_TextRenderingMode m_Mode;
            public double m_LineWidth;
            public int m_FColor;
            public int m_SColor;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public Int16[] m_Text;
            // Image Part
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PATH)]
            public Int16[] m_FileName;
            public int m_TransColor;
            public double m_Width;
            public double m_Height;
            public bool m_bKeepAspect;
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
            public int[] m_Range;
        }

        // Security
        public enum PXC_SecurityPermissions
        {
            Permit_Printing = (1 << 2),
            Permit_Modification = (1 << 3),
            Permit_Copying_And_TextGraphicsExtractions = (1 << 4),
            Permit_Add_And_Modify_Annotations = (1 << 5),
            Permit_FormFilling = (1 << 8),
            Permit_TextGraphicsExtractions = (1 << 9),
            Permit_Assemble = (1 << 10),
            Permit_HighQualityPrinting = (1 << 11),
            Permit_Nothing = unchecked((int)0xFFFFF0C0),
            Permit_All = unchecked((int)0xFFFFFFFC),

        }

        // Annotations

        public enum PXC_AnnotsFlags
        {
            AF_Invisible = 0x0001,
            // >= 1.2
            AF_Hidden = 0x0002,
            AF_Print = 0x0004,
            // >= 1.3
            AF_NoZoom = 0x0008,
            AF_NoRotate = 0x0010,
            AF_NoView = 0x0020,
            AF_ReadOnly = 0x0040,
            // >= 1.4
            AF_Locked = 0x0080,
            // >= 1.5
            AF_ToggleNoView = 0x0100
        }

        public enum PXC_AnnotBorderStyle
        {
            ABS_Solid = 0,			// default
            ABS_Dashed,
            ABS_Bevel,
            ABS_Inset,
            ABS_Underline,
        }

        // Text Annotations
        public enum PXC_TextAnnotsType
        {
            TAType_Note = 0,		// default
            TAType_Comment,
            TAType_Key,
            TAType_Help,
            TAType_NewParagraph,
            TAType_Paragraph,
            TAType_Insert,
        }

        public enum PXC_LineAnnotsType
        {
            LAType_None = 0,		// default
            LAType_Square,
            LAType_Circle,
            LAType_Diamond,
            LAType_OpenArrow,
            LAType_ClosedArrow,
            // PDF Spec. >= 1.5
            LAType_Butt,
            LAType_ROpenArrow,
            LAType_RClosedArrow,
        }

        public enum PXC_StampAnnotsType
        {
            SAType_Draft = 0,		// default
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
        }

        public enum PXC_FileAttachmentAnnotType
        {
            FAAType_PushPin = 0,		// default
            FAAType_Graph,
            FAAType_Paperclip,
            FAAType_Tag,
        }

        public enum PXC_LaunchOperation
        {
            LO_Open = 0,		// default
            LO_Print
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PXC_AnnotBorder
        {
            public double m_Width;		// def: 1.0
            public PXC_AnnotBorderStyle m_Type;			// see: PXC_AnnotBorderStyle
            public int m_DashCount;	// def: 0
            public double[] m_DashArray;	// def: NULL
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PXC_CommonAnnotInfo
        {
            public double m_Opacity;		// def.: 1.0; spec. >= 1.4
            public int m_Color;
            public int m_Flags;		// see PXC_AnnotsFlags
            public PXC_AnnotBorder m_Border;
        }

        // Shades
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PXC_TRIVERTEX
        {
            public double x;
            public double y;
            public int color;
        }
        #endregion
    }
}
