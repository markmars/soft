using System;
using System.Diagnostics;
using PDFXCviewAxLib;
using MarkMars.Common2;

namespace MarkMars.PDF
{
    public class PDFXCviewer
    {
        private static readonly String PDFXChangePDFViewerRegKey = "PVC20-5K1UW-P0JD6-IKS55-OBAWI-DMBJW";
        private static readonly String PDFXChangePDFViewerDevCode = "PDFX3$Henry$300604_Allnuts#";

        private AxPDFXCviewAxLib.AxCoPDFXCview pdfXCview;
        private Int32 iActiveDocID;

        public PDFXCviewer()
        {
            try
            {
                Process.Start("PDFXCview.exe", "/RegServer");
            }
            catch
            {
            }
        }

        private void ShowErrorMessage(Int32 HResult, String exceptionMsg)
        {
            String strMsg;
            if (HResult != 0)
            {
                try
                {
                    this.pdfXCview.GetTextFromResult(HResult, out strMsg);
                    MarkMarsMessageBox.ShowError(strMsg);
                }
                catch
                {
                    MarkMarsMessageBox.ShowError("error in GetTextFromResult" + Environment.NewLine + exceptionMsg);
                }
            }
        }

        private Boolean TryGetProperty(String sName, out Object dataOut, Int32 flags)
        {
            Boolean bRes = false;
            try
            {
                this.pdfXCview.GetProperty(sName, out dataOut, flags);
                bRes = true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
                dataOut = null;
            }

            return bRes;
        }

        /// <summary>
        /// 打开一个PDF文件
        /// </summary>
        public void OpenPDF(AxPDFXCviewAxLib.AxCoPDFXCview pdfViewer, String pdfFileName, Byte toolType)
        {
            try
            {
                this.pdfXCview = pdfViewer;
                this.pdfXCview.SetDevInfo(PDFXCviewer.PDFXChangePDFViewerRegKey, PDFXCviewer.PDFXChangePDFViewerDevCode);
                this.pdfXCview.SetProperty("International.LocaleID", "$0804", 0);
                this.pdfXCview.OpenDocument(pdfFileName, "", out this.iActiveDocID, 0);

                //系统工具条
                this.SetPDFMainBarOption(0, "Bar", "32924", this.iActiveDocID);      //standard
                this.SetPDFMainBarOption(0, "Bar", "32925", this.iActiveDocID);      //zoom
                this.SetPDFMainBarOption(0, "Bar", "33000", this.iActiveDocID);      //find
                this.SetPDFMainBarOption(0, "Bar", "32929", this.iActiveDocID);      //旋转
                this.SetPDFMainBarOption(0, "Bar", "32908", this.iActiveDocID);          //File Toolbar
                this.SetPDFMainBarOption(2, "Command", "57601", this.iActiveDocID);      //File Toolbar-Open
                this.SetPDFMainBarOption(2, "Command", "57603", this.iActiveDocID);      //File Toolbar-Save
                this.SetPDFMainBarOption(2, "Command", "57612", this.iActiveDocID);      //File Toolbar-SendMail
                this.SetPDFMainBarOption(2, "Command", "32316", this.iActiveDocID);      //File Toolbar-GoBack
                this.SetPDFMainBarOption(2, "Command", "32317", this.iActiveDocID);      //File Toolbar-GoForward
                this.SetPDFMainBarOption(2, "Command", "57643", this.iActiveDocID);      //File Toolbar-Undo
                this.SetPDFMainBarOption(2, "Command", "57644", this.iActiveDocID);      //File Toolbar-Redo
                //this.SetPDFMainBarOption(l_CoPDFXCview, 0, "Bar", "33138", l_ActiveDocID);      //CommentAndMarkup
                //文档工具条
                if (toolType == 2 || toolType == 3 || toolType == 4)
                {
                    this.SetPDFMainBarOption(1, "Pane", "32910", this.iActiveDocID);      //Bookmarks                    
                    //this.SetPDFMainBarOption(0, "Bar", "33138", this.iActiveDocID);      //Bookmarks 

                    if (toolType == 3 || toolType == 4)
                    {
                        this.SetPDFMainBarOption(1, "Pane", "33204", this.iActiveDocID);      //Comments   

                        if (toolType == 3)
                        {
                            this.SetPDFMainBarOption(2, "Command", "33240", this.iActiveDocID);      //Delete Comment
                        }
                    }
                }
                this.SetPDFMainBarOption(1, "Bar", "33263", this.iActiveDocID);      //PagesNavigation
                //隐藏书签的命令
                this.SetPDFMainBarOption(2, "Command", "33416", this.iActiveDocID);      //add Bookmark
                this.SetPDFMainBarOption(2, "Command", "36008", this.iActiveDocID);      //New Bookmark
                this.SetPDFMainBarOption(2, "Command", "33135", this.iActiveDocID);      //add Note
                this.SetPDFMainBarOption(2, "Command", "33136", this.iActiveDocID);      //Delete Selection
                this.SetPDFMainBarOption(2, "Command", "36006", this.iActiveDocID);      //Delete Bookmark
                this.SetPDFMainBarOption(2, "Command", "36007", this.iActiveDocID);      //Bookmark Properties                
                this.SetPDFMainBarOption(2, "Command", "36023", this.iActiveDocID);      //ReName Bookmark
                this.SetPDFMainBarOption(2, "Command", "57634", this.iActiveDocID);      //Copy
                this.SetPDFMainBarOption(2, "Command", "57635", this.iActiveDocID);      //Cut

                this.SetPDFMainBarOption(2, "Command", "57637", this.iActiveDocID);      //Paste
                this.SetPDFMainBarOption(2, "Command", "33270", this.iActiveDocID);      //Document property
                //Main Bars 右键菜单相关命令
                this.SetPDFMainBarOption(2, "Command", "36000", this.iActiveDocID);      //Customize
                this.SetPDFMainBarOption(2, "Command", "33009", this.iActiveDocID);      //main Menu
                this.SetPDFMainBarOption(2, "Command", "32908", this.iActiveDocID);      //File Toolbar
                this.SetPDFMainBarOption(2, "Command", "33138", this.iActiveDocID);      //Comment And Markup toolbar
                this.SetPDFMainBarOption(2, "Command", "36342", this.iActiveDocID);      //Links toolbar
                //this.SetPDFMainBarOption(l_CoPDFXCview, 2, "Command", "32913", l_ActiveDocID);      //Promotion bar
                //this.SetPDFMainBarOption(l_CoPDFXCview, 2, "Command", "33225", l_ActiveDocID);      //Properties toolbar
                this.SetPDFMainBarOption(2, "Command", "36345", this.iActiveDocID);      //Measuring toolbar

                //switch (toolType)
                //{
                //    case 1:
                //        //系统工具条
                //        this.SetPDFMainBarOption(0, "Bar", "32924");      //standard
                //        this.SetPDFMainBarOption(0, "Bar", "32925");      //zoom
                //        this.SetPDFMainBarOption(0, "Bar", "33000");      //find
                //        //文档工具条
                //        this.SetPDFMainBarOption(1, "Bar", "33263");      //PagesNavigation
                //        break;
                //    case 2:
                //        //系统工具条
                //        this.SetPDFMainBarOption(0, "Bar", "32924");      //standard
                //        this.SetPDFMainBarOption(0, "Bar", "32925");      //zoom
                //        this.SetPDFMainBarOption(0, "Bar", "33000");      //find
                //        //文档工具条
                //        this.SetPDFMainBarOption(1, "Bar", "33263");      //PagesNavigation
                //        this.SetPDFMainBarOption(1, "Pane", "32910");      //Bookmarks
                //        OfflineBookmark("36007");
                //        OfflineBookmark("36008");
                //        break;
                //}
                //系统工具条
                //this.SetPDFMainBarOption(0, "Bar", "32924");      //standard
                //this.SetPDFMainBarOption(0, "Bar", "32925");      //zoom
                //this.SetPDFMainBarOption(0, "Bar", "33000");      //find
                //this.SetPDFMainBarOption(0, "Bar", "33138");      //CommentAndMarkup

                //文档工具条
                //this.SetPDFMainBarOption(1, "Pane", "32910");      //Bookmarks
                //this.SetPDFMainBarOption(1, "Bar", "33263");      //PagesNavigation

                //自动填充整个窗口
                // Chenx 有部分机器在查看招标文件后，点击关闭窗口按钮会出错，经过跟踪，发现是下面这个命令引起的。
                //Object dataOut;
                //this.pdfXCview.DoVerb(null, "ExecuteCommand", 32902, out dataOut, 0);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
                return;
            }
        }

        /// <summary>
        /// 设置工具栏
        /// </summary>
        private void SetPDFMainBarOption(Int32 type, String obj, String id, Int32 activeDocId)
        {
            Object dataOut;
            String sVerb = "";
            Boolean fromVDataOut, checkChanged;

            try
            {
                switch (type)
                {
                    case 0:
                        sVerb = "View." + obj + "s[#" + id + "].Visible";
                        break;
                    case 1:
                        sVerb = "Documents[#" + activeDocId.ToString() + "].View." + obj + "s[#" + id + "].Visible";
                        break;
                    case 2:
                        sVerb = "Commands[#" + id + "].State";
                        break;
                }

                if (!TryGetProperty(sVerb, out dataOut, 0)) return;

                fromVDataOut = Convert.ToInt32(dataOut) == 0 ? false : true;

                if (type == 0 || type == 1)
                {
                    checkChanged = (fromVDataOut ^ true);
                }
                else
                {
                    checkChanged = true;
                }

                if (checkChanged)
                {
                    Object dataIn;
                    if (type == 0 || type == 1)
                    {
                        dataIn = 1;
                    }
                    else
                    {
                        dataIn = "Offline";
                    }

                    this.pdfXCview.SetProperty(sVerb, dataIn, 0);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
                return;
            }
        }

        /// <summary>
        /// 关闭文档
        /// </summary>
        public void CloseDocument(AxPDFXCviewAxLib.AxCoPDFXCview axCoPDFCview)
        {
            try
            {
                axCoPDFCview.GetActiveDocument(out this.iActiveDocID);
                if (this.iActiveDocID > -1)
                {
                    axCoPDFCview.CloseDocument(this.iActiveDocID);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
                return;
            }
        }

        public void GoToPage(Int32 pageCount)
        {
            try
            {
                this.pdfXCview.GetActiveDocument(out this.iActiveDocID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
                return;
            }

            if (this.iActiveDocID < 0)
            {
                MarkMarsMessageBox.ShowError("PDF文件没有打开！");
                return;
            }

            Int32 nPage = pageCount - 1;

            String bsVerb = "Pages.Current";
            Object dataIn = nPage;
            try
            {
                this.pdfXCview.SetDocumentProperty(this.iActiveDocID, bsVerb, dataIn, 0);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
            }
        }

        public Int32 GetActiveDocument(AxPDFXCviewAxLib.AxCoPDFXCview axCoPDFCview)
        {
            try
            {
                axCoPDFCview.GetActiveDocument(out this.iActiveDocID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
            }

            return this.iActiveDocID;
        }

        public void SaveActiveDocument()
        {
            try
            {
                this.pdfXCview.GetActiveDocument(out this.iActiveDocID);
                if (this.iActiveDocID < 0)
                {
                    MarkMarsMessageBox.ShowError("PDF文件没有打开！");
                    return;
                }

                this.pdfXCview.SaveDocument(this.iActiveDocID, null, 0, 0);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
            }
        }

        /// <summary>
        /// 显示书签面板。
        /// </summary>
        public void ShowBookmarksPane()
        {
            this.SetPDFMainBarOption(1, "Pane", "32910", this.iActiveDocID);      //Bookmarks  
        }

        /// <summary>
        /// 显示批注面板。
        /// </summary>
        public void ShowCommentsPane()
        {
            this.SetPDFMainBarOption(1, "Pane", "33204", this.iActiveDocID);      //Comments   
        }

        /// <summary>
        /// 显示批注工具栏。
        /// </summary>
        public void ShowCommentsBar()
        {
            this.SetPDFMainBarOption(0, "Bar", "33138", this.iActiveDocID);      //CommentAndMarkup
        }

        /// <summary>
        /// 显示批注工具。
        /// </summary>
        public void ShowStickyNoteTool()
        {
            Object dataIn = 33132;
            Object dataOut;
            this.pdfXCview.DoVerb(null, "ExecuteCommand", dataIn, out dataOut, 0);
        }

        /// <summary>
        /// 显示手型工具。
        /// </summary>
        public void ShowHandTool()
        {
            Object dataIn = 32613;
            Object dataOut;
            this.pdfXCview.DoVerb(null, "ExecuteCommand", dataIn, out dataOut, 0);
        }

        /// <summary>
        /// 将 PDF 指定位置移到显示区顶部。
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MovePointToTop(Int32 pageIndex, Double x, Double y)
        {
            Object dataOut;

            Double[] dataIn = new Double[4];
            dataIn[0] = x;

            //获取页面高度
            this.pdfXCview.DoVerb(String.Format("Documents[0].pages[{0}].Height", pageIndex - 1), "get", null, out dataOut, 0);
            //Word 的坐标原点在左上角，而 PDF 页面的坐标原点在左下角，所以 Y 坐标要反过来。
            dataIn[1] = Convert.ToDouble(dataOut) - y;

            //获取页面显示区左上角的屏幕 X 坐标
            this.pdfXCview.DoVerb("Documents[0].pages.ScreenX", "get", null, out dataOut, 0);
            dataIn[2] = Convert.ToDouble(dataOut) + 5;

            //获取页面显示区左上角的屏幕 Y 坐标
            this.pdfXCview.DoVerb("Documents[0].pages.ScreenY", "get", null, out dataOut, 0);
            dataIn[3] = Convert.ToDouble(dataOut) + 5;

            this.pdfXCview.DoVerb(String.Format("Documents[0].pages[{0}]", pageIndex - 1), "MovePointToScreenPoint", dataIn, out dataOut, 0);
        }

        /// <summary>
        /// 获取整个 PDF 文件中的批注数量。
        /// </summary>
        /// <returns>批注数量。</returns>
        public Int32 GetStickyNoteCount()
        {
            Object dataIn = "";
            Object dataOut;

            //获取页面数量
            this.pdfXCview.DoVerb("Documents[0].Pages.Count", "get", dataIn, out dataOut, 0);

            Int32 pagesCount = Convert.ToInt32(dataOut);
            Int32 stickyNoteCount = 0;
            for (Int32 idx = 0; idx < pagesCount; idx++)
            {
                //获取指定页面的批注数量
                this.pdfXCview.DoVerb(String.Format("Documents[0].Pages[{0}]", idx), "GetAnnotsCount", dataIn, out dataOut, 0);

                Int32 annotsCount = Convert.ToInt32(dataOut);
                for (Int32 idx2 = 0; idx2 < annotsCount; idx2++)
                {
                    dataIn = idx2;

                    //获取指定批注的类型
                    this.pdfXCview.DoVerb(String.Format("Documents[0].Pages[{0}]", idx), "GetAnnotType", dataIn, out dataOut, 0);
                    if (dataOut.ToString() != "0")
                    {
                        stickyNoteCount++;
                    }
                }
            }

            return stickyNoteCount;
        }

        /// <summary>
        /// 将 PDF 文件的每页导成一个图片文件。
        /// </summary>
        /// <param name="destinationFilePath">目标路径。</param>
        public void ExportImages(String destinationPath)
        {
            try
            {
                this.pdfXCview.GetActiveDocument(out this.iActiveDocID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
                return;
            }

            if (this.iActiveDocID < 0)
            {
                MarkMarsMessageBox.ShowError("PDF文件没有打开！");
                return;
            }

            try
            {
                String bsVerb = "Export.Image.RangeType";
                Object vDataIn = "All";
                this.pdfXCview.SetProperty(bsVerb, vDataIn, 0);
            }
            catch (Exception)
            {
            }

            try
            {
                String bsVerb = "Export.Image.Resolution";
                Object vDataIn = 72;
                this.pdfXCview.SetProperty(bsVerb, vDataIn, 0);
            }
            catch (Exception)
            {
            }

            try
            {
                String bsVerb = "Export.Image.Type";
                Object vDataIn = "JPEG";
                this.pdfXCview.SetProperty(bsVerb, vDataIn, 0);
            }
            catch (Exception)
            {
            }

            try
            {
                String bsVerb = "Export.Image.Mode";
                Object vDataIn = "AllToOneMultiImage";
                this.pdfXCview.SetProperty(bsVerb, vDataIn, 0);
            }
            catch (Exception)
            {
            }

            try
            {
                String bsVerb = "Export.Image.Background";
                Object vDataIn = "";
                try
                {
                    this.pdfXCview.GetProperty(bsVerb, out vDataIn, 0);
                }
                catch (Exception)
                {
                }
                finally
                {
                    vDataIn = ((UInt32)vDataIn | 0xFF000000);
                    this.pdfXCview.SetProperty(bsVerb, vDataIn, 0);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                String bsVerb = "Export.Image.RangeReverse";
                Object vDataIn = 0;
                this.pdfXCview.SetProperty(bsVerb, vDataIn, 0);
            }
            catch (Exception)
            {
            }

            try
            {
                String bsVerb = "Export.Image.FolderName";
                Object vDataIn = destinationPath;
                this.pdfXCview.SetProperty(bsVerb, vDataIn, 0);
            }
            catch (Exception)
            {
            }

            try
            {
                this.pdfXCview.ExportDocument(this.iActiveDocID, (Int32)PXCVA_Flags.PXCVA_NoUI);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(System.Runtime.InteropServices.Marshal.GetHRForException(ex), ex.Message);
            }
        }
    }
}
