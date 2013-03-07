using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using PXCComLib;
using MarkMars.Common2;
using oWord = Microsoft.Office.Interop.Word;

namespace MarkMars.PDF
{
    /// <summary>
    /// 类：Doc2PDF， 用途：将word文档通过虚拟打印机生成PDF文件。支持多个文档连续生成PDF。
    /// 该类只能初始化一次，不能在多个进程中同时调用。因为是根据创建的虚拟打印机名称来输出PDF的
    /// </summary>
    public class Doc2PDF
    {
        public delegate void FileSavedEventHandler(object sender, EventArgs e);
        public event FileSavedEventHandler OnFileSaved;

        private CPXCControlEx cPXCControlEx;
        private CPXCPrinter cPXCPrinter;
        private Word.ApplicationClass wordApplication = null;

        private String area = String.Empty;
        public String Area
        {
            set
            {
                this.area = value;
            }
        }

        public Doc2PDF()
        {
            if (CommonFunction.WordVersion.Equals("2003"))
            {
                try
                {
                    // 批量打印的时候，创建 COM 组件太快，有可能创建失败，所以失败后要休眠一段时间再创建。
                    this.cPXCControlEx = new CPXCControlExClass();
                }
                catch
                {
                    Thread.Sleep(100);
                    this.cPXCControlEx = new CPXCControlExClass();
                }

                try
                {
                    //这是比较奇怪的现象，Cell打印输出的时候，某些表单打印的时候，好像找到了以前的默认虚拟打印机！！！所以需要删除掉
                    cPXCControlEx.RemoveOrphanPrinters("", "PDF-XChange 4.0");
                    //这是我们创建的临时虚拟打印机名称
                    cPXCControlEx.RemoveOrphanPrinters("", "PDF-XChangeTL4");
                }
                catch
                {
                }

                this.cPXCPrinter = (CPXCPrinter)cPXCControlEx.get_Printer("", "PDF-XChangeTL4", "PCS40-CXUMA-V4D9C-PQFSU-QGUUX-XMWIZ", "PDFX3$Henry$300604_Allnuts#");
                this.cPXCPrinter.OnFileSaved += new _IPXCPrinterEvents_OnFileSavedEventHandler(PDFPrinter_OnFileSaved);
            }
        }

        /// <summary>
        /// 调用 PDFPrintOut 方法，将传入的 WORD 文档生成某目标文件(PDF格式)。
        /// </summary>
        /// <param name="sourceFileName">传入的 WORD 文档。</param>
        /// <param name="destFileName">生成的 PDF 文档名称。</param>
        public void PDFPrintOut(Word.Document wordDocument, String pdfFileFullName)
        {
            CPXCPrinterSetting(pdfFileFullName);

            //2009.06.22,wuyl: 注意：有些参数不能使用System.Type.Missing，否则导致输出PDF的时候不稳定！！！
            object background = true;
            object append = false;
            object range = Word.WdPrintOutRange.wdPrintAllDocument;
            object outputFileName = "";
            object from = System.Type.Missing;
            object to = System.Type.Missing;
            object item = Word.WdPrintOutItem.wdPrintDocumentContent;
            object copies = "1";
            object pages = System.Type.Missing;
            object pageType = Word.WdPrintOutPages.wdPrintAllPages;
            object printToFile = false;
            object collate = false;
            object activePrinterMacGX = System.Type.Missing;
            object manualDuplexPrint = false;
            object printZoomColumn = 0;
            object printZoomRow = 0;
            object printZoomPaperWidth = 0;
            object printZoomPaperHeight = 0;

            try
            {
                wordDocument.PrintOut(ref background, ref append, ref range, ref outputFileName, ref from, ref to, ref item, ref copies,
                    ref pages, ref pageType, ref printToFile, ref collate, ref activePrinterMacGX, ref manualDuplexPrint, ref printZoomColumn,
                    ref printZoomRow, ref printZoomPaperWidth, ref printZoomPaperHeight);
            }
            catch
            {
                MessageBox.Show("PrintOut出现异常！");
                this.KillPDFSave4();
            }
        }

        /// <summary>
        /// 调用PDFPrintOut方法，将传入的word文档生成某目标文件(PDF格式)
        /// </summary>
        /// <param name="sourceFileName">传入的word文档名称</param>
        /// <param name="destFileName">生成的PDF文档名称</param>
        public void PDFPrintOut(String sourceFileName, String destFileName, object objDocument)
        {
            Boolean isOpen = false;

            oWord.ApplicationClass thisApplication = null;
            oWord.Document thisDocument = null;
            if (objDocument != null)
            {
                try
                {
                    thisDocument = (oWord.Document)objDocument;
                }
                catch
                {
                }
            }

            try
            {
                if (thisDocument == null)
                {
                    if (thisApplication == null)
                    {
                        thisApplication = new oWord.ApplicationClass();
                    }                    

                    thisApplication.Visible = false;        //true; // true; //
                    thisApplication.Caption = "";
                    thisApplication.Options.CheckSpellingAsYouType = false;
                    thisApplication.Options.CheckGrammarAsYouType = false;
                    thisApplication.NormalTemplate.Saved = true;

                    Object filename = sourceFileName;
                    Object ConfirmConversions = false;
                    Object ReadOnly = false;
                    Object AddToRecentFiles = false;

                    Object PasswordDocument = System.Type.Missing;
                    Object PasswordTemplate = System.Type.Missing;
                    Object Revert = System.Type.Missing;
                    Object WritePasswordDocument = System.Type.Missing;
                    Object WritePasswordTemplate = System.Type.Missing;
                    Object Format = System.Type.Missing;
                    Object Encoding = System.Type.Missing;
                    Object Visible = System.Type.Missing;
                    Object OpenAndRepair = System.Type.Missing;
                    Object DocumentDirection = System.Type.Missing;
                    Object NoEncodingDialog = System.Type.Missing;
                    Object XMLTransform = System.Type.Missing;

                    //word2007的方式打开
                    try
                    {
                        thisDocument = thisApplication.Documents.Open(ref filename, ref ConfirmConversions, ref ReadOnly, ref AddToRecentFiles, ref PasswordDocument, ref PasswordTemplate,
                            ref Revert, ref WritePasswordDocument, ref WritePasswordTemplate, ref Format, ref Encoding, ref Visible, ref OpenAndRepair, ref DocumentDirection, ref NoEncodingDialog, ref XMLTransform);
                    }
                    catch
                    {
                        //2010-07-16 Youker 有些机器装了Word2007 但是已Word2007的方式打开的时候提示说：无法启动转换器mswrd632。
                        //解决方案：在注册表中新增Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Wordpad项，然后新建键AllowConversion = 1值，类型 DWord。
                        RegistryKey hklm = Registry.LocalMachine;
                        RegistryKey wordpad = hklm.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Wordpad", true);
                        if (wordpad == null)
                        {
                            wordpad = hklm.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Wordpad");
                            wordpad.SetValue("AllowConversion", 1, RegistryValueKind.DWord);
                        }

                        try
                        {
                            thisDocument = thisApplication.Documents.Open(ref filename, ref ConfirmConversions, ref ReadOnly, ref AddToRecentFiles, ref PasswordDocument, ref PasswordTemplate,
                                ref Revert, ref WritePasswordDocument, ref WritePasswordTemplate, ref Format, ref Encoding, ref Visible, ref OpenAndRepair, ref DocumentDirection, ref NoEncodingDialog, ref XMLTransform);
                        }
                        catch
                        {
                            MessageBox.Show("Word2007文件打开失败！");
                        }
                    }

                    isOpen = true;
                }

                if (thisDocument == null)
                {
                    throw new Exception("文件打开异常！");
                }
                
                //*2009.06.22,wuyl: 注意：有些参数不能使用System.Type.Missing，否则导致输出PDF的时候不稳定！！！
                object background = true;
                object append = false;
                object range = oWord.WdPrintOutRange.wdPrintAllDocument;
                object outputFileName = "";  // System.Type.Missing; // destFileName;
                object from = System.Type.Missing;
                object to = System.Type.Missing;
                object item = oWord.WdPrintOutItem.wdPrintDocumentContent;
                object copies = "1";
                object pages = System.Type.Missing;
                object pageType = oWord.WdPrintOutPages.wdPrintAllPages;
                object printToFile = false; // true;
                object collate = false;
                object activePrinterMacGX = System.Type.Missing;
                object manualDuplexPrint = false; //System.Type.Missing;
                object printZoomColumn = 0; //System.Type.Missing;
                object printZoomRow = 0; //System.Type.Missing;
                object printZoomPaperWidth = 0; //System.Type.Missing;
                object printZoomPaperHeight = 0; //System.Type.Missing;
                object fileName = "";  // System.Type.Missing; // destFileName; ""
                //*/

                object NullPara = System.Reflection.Missing.Value;

                if (!String.IsNullOrEmpty(this.area))
                {
                    if (thisApplication == null)
                    {
                        thisApplication = (oWord.ApplicationClass)thisDocument.Application;
                    }

                    switch (this.area)
                    {
                        //case "TY":
                        //    thisDocument.Content.Font.Size = 15;
                        //    thisDocument.Content.Font.Name = "仿宋";
                        //    break;
                        case "RZ":
                        case "DY":
                        case "DZ":
                        case "GY":
                        case "LZ":
                            thisDocument.Content.Font.Size = 12;
                            thisDocument.Content.Font.Name = "宋体";
                            thisDocument.Content.ParagraphFormat.LineSpacingRule = Microsoft.Office.Interop.Word.WdLineSpacing.wdLineSpaceSingle;
                            break;
                        case "TYQD":
                        case "TY":
                            thisDocument.Content.Font.Size = 14;
                            thisDocument.Content.Font.Name = "宋体";
                            thisDocument.Content.ParagraphFormat.LineSpacingRule = Microsoft.Office.Interop.Word.WdLineSpacing.wdLineSpaceSingle;
                            // 2012-05-18 Chenx 修改文档的页边距，只能一节一节的修改，不能全选修改，否则可能会出错。
                            foreach (Word.Section section in thisDocument.Sections)
                            {
                                section.PageSetup.LeftMargin = thisApplication.CentimetersToPoints(2.5F);
                                section.PageSetup.RightMargin = thisApplication.CentimetersToPoints(2);
                                section.PageSetup.TopMargin = thisApplication.CentimetersToPoints(2);
                                section.PageSetup.BottomMargin = thisApplication.CentimetersToPoints(2);
                            }
                            break;
                    }

                    thisDocument.Content.Font.Bold = 0;
                    thisDocument.Content.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                    thisDocument.Content.Font.Italic = 0;
                    thisDocument.Content.Font.Subscript = 0;
                    thisDocument.Save();
                }

                //thisDocument.ExportAsFixedFormat(destFileName, oWord.WdExportFormat.wdExportFormatPDF, false,
                //    oWord.WdExportOptimizeFor.wdExportOptimizeForPrint, oWord.WdExportRange.wdExportAllDocument, 1, 1,
                //    oWord.WdExportItem.wdExportDocumentContent, false, false,
                //    oWord.WdExportCreateBookmarks.wdExportCreateNoBookmarks, true, false, false, ref NullPara);
                // 2011-07-05 Chenx 修改参数“wdExportCreateNoBookmarks”为“wdExportCreateHeadingBookmarks”，允许创建Word书签。
                thisDocument.ExportAsFixedFormat(destFileName, oWord.WdExportFormat.wdExportFormatPDF, false,
                    oWord.WdExportOptimizeFor.wdExportOptimizeForPrint, oWord.WdExportRange.wdExportAllDocument, 1, 1,
                    oWord.WdExportItem.wdExportDocumentContent, false, false,
                    oWord.WdExportCreateBookmarks.wdExportCreateHeadingBookmarks, true, false, false, ref NullPara);
            }
            catch(Exception ex)
            {
                MessageBox.Show("请安装Word转PDF插件！" + Environment.NewLine + ex.Message);
            }

            if (isOpen)
            {
                try
                {
                    //// 缺省参数  
                    object Unknown = Type.Missing;
                    if (thisDocument != null)
                    {
                        thisDocument.Close(ref Unknown, ref Unknown, ref Unknown);
                        thisDocument = null;
                    }

                    if (thisApplication != null)
                    {
                        thisApplication.Quit(ref Unknown, ref Unknown, ref Unknown);
                        thisApplication = null;
                    }
                }
                catch
                {
                }
            }
        }

        private void CreateWordDocument(String sourceFileName, ref Word.Document wordDocument)
        {
            this.wordApplication = new Word.ApplicationClass();
            this.wordApplication.Visible = false;
            this.wordApplication.Caption = "";
            this.wordApplication.Options.CheckSpellingAsYouType = false;
            this.wordApplication.Options.CheckGrammarAsYouType = false;            

            Object filename = sourceFileName;
            Object ConfirmConversions = false;
            Object ReadOnly = false;
            Object AddToRecentFiles = false;

            Object PasswordDocument = System.Type.Missing;
            Object PasswordTemplate = System.Type.Missing;
            Object Revert = System.Type.Missing;
            Object WritePasswordDocument = System.Type.Missing;
            Object WritePasswordTemplate = System.Type.Missing;
            Object Format = System.Type.Missing;
            Object Encoding = System.Type.Missing;
            Object Visible = System.Type.Missing;
            Object OpenAndRepair = System.Type.Missing;
            Object DocumentDirection = System.Type.Missing;
            Object NoEncodingDialog = System.Type.Missing;
            Object XMLTransform = System.Type.Missing;

            //word2000的方式打开,以后的版本自动兼容这个方法
            wordDocument = this.wordApplication.Documents.Open(ref filename, ref ConfirmConversions, ref ReadOnly, ref AddToRecentFiles,
                ref PasswordDocument, ref PasswordTemplate, ref Revert, ref WritePasswordDocument, ref WritePasswordTemplate, ref Format,
                ref Encoding, ref Visible);

            if (!String.IsNullOrEmpty(this.area))
            {
                if (this.wordApplication == null)
                {
                    this.wordApplication = (Word.ApplicationClass)wordDocument.Application;
                }

                switch (this.area)
                {
                    //case "TY":
                    //    wordDocument.Content.Font.Size = 15;
                    //    wordDocument.Content.Font.Name = "仿宋";
                    //    break;
                    case "RZ":
                    case "DY":
                    case "DZ":
                    case "GY":
                    case "LZ":
                        wordDocument.Content.Font.Size = 12;
                        wordDocument.Content.Font.Name = "宋体";
                        wordDocument.Content.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                        break;
                    case "TYQD":
                    case "TY":
                        wordDocument.Content.Font.Size = 14;
                        wordDocument.Content.Font.Name = "宋体";
                        wordDocument.Content.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                        // 2012-05-18 Chenx 修改文档的页边距，只能一节一节的修改，不能全选修改，否则可能会出错。
                        foreach (Word.Section section in wordDocument.Sections)
                        {
                            section.PageSetup.LeftMargin = this.wordApplication.CentimetersToPoints(2.5F);
                            section.PageSetup.RightMargin = this.wordApplication.CentimetersToPoints(2);
                            section.PageSetup.TopMargin = this.wordApplication.CentimetersToPoints(2);
                            section.PageSetup.BottomMargin = this.wordApplication.CentimetersToPoints(2);
                        }
                        break;
                }

                wordDocument.Content.Font.Bold = 0;
                wordDocument.Content.Font.Color = Word.WdColor.wdColorBlack;
                wordDocument.Content.Font.Italic = 0;
                wordDocument.Content.Font.Subscript = 0;
                wordDocument.Save();
            }
        }

        private void KillPDFSave4()
        {
            //可能出现异常：打印完毕之后，再保存PDF文件时，可能pdfsaver4.exe占用了输出的PDF文件，这时需要杀死这个进程
            Process[] processOnComputer = Process.GetProcesses();
            foreach (Process process in processOnComputer)
            {
                if (process.ProcessName.ToLower() == "pdfsaver4")
                {
                    try
                    {
                        //pdfsaver4 有可能还在使用一些资源，导致杀进程失败，所以失败后要休眠一段时间再杀。
                        process.Kill();
                    }
                    catch
                    {
                        Thread.Sleep(100);
                        process.Kill();
                    }
                    break;
                }
            }
        }

        public void CPXCPrinterSetting(String destFileName)
        {
            //this.cPXCPrinter.ResetDefaults();
            this.cPXCPrinter.SetAsDefaultPrinter();

            object printFileName = destFileName;

            this.cPXCPrinter.set_Option("Save.RunApp", "False");
            this.cPXCPrinter.set_Option("Save.File", printFileName);
            this.cPXCPrinter.set_Option("Save.SaveType", "Save");
            this.cPXCPrinter.set_Option("Save.ShowSaveDialog", "False");
            this.cPXCPrinter.set_Option("Save.WhenExists", "Overwrite");
            this.cPXCPrinter.set_Option("Compression.Graphics", "Yes");
            this.cPXCPrinter.set_Option("Compression.Text", "Yes");
            this.cPXCPrinter.set_Option("Compression.ASCII", "No");
            this.cPXCPrinter.set_Option("Compression.Color.Enabled", "Yes");
            this.cPXCPrinter.set_Option("Compression.Color.Method", "Auto");
            this.cPXCPrinter.set_Option("Compression.Indexed.Enabled", "Yes");
            this.cPXCPrinter.set_Option("Compression.Indexed.Method", "Auto");
            this.cPXCPrinter.set_Option("Compression.Mono.Enabled", "Yes");
            this.cPXCPrinter.set_Option("Compression.Mono.Method", "Auto");
            this.cPXCPrinter.set_Option("Fonts.EmbedAll", "Yes");
            this.cPXCPrinter.set_Option("Fonts.WriteToUnicode", "Yes");
        }

        private void PDFPrinter_OnFileSaved(Int32 JobID, String lpszFileName)
        {
            if (this.OnFileSaved != null)
            {
                this.OnFileSaved(this, new EventArgs());
            }
            //文件大的时候会出问题，上下文 0x20fe68 已断开连接。Youker 2009-10-28
            //thisDocument.Close(ref saveChanges, ref originalFormat, ref routeDocument);
        }

        /// <summary>
        /// 打印结束之后，调用本函数释放WordAppliation
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public void Dispose()
        {
            Object saveChanges = false;
            Object originalFormat = System.Type.Missing;
            Object routeDocument = System.Type.Missing;
            if (this.wordApplication != null)
            {
                this.wordApplication.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
                this.wordApplication = null;
            }
            if (this.cPXCPrinter != null)
            {
                this.cPXCPrinter.RestoreDefaultPrinter();
                this.cPXCControlEx = null;
                this.cPXCPrinter = null;
            }

            this.KillPDFSave4();
        }

        /// <summary>
        /// 利用Word生成PDF文档
        /// </summary>
        /// <param name="WordFileFullName">Word文件存放的全路径</param>
        /// <param name="PdfFileFullName">Pdf文件存放的全路径</param>
        public void PrintOutWordToPdf(string wordFileFullName, string pdfFileFullName)
        {
            switch (CommonFunction.WordVersion)
            {
                case "2003":
                    this.PrintOutWordToPdf(wordFileFullName, pdfFileFullName, null);
                    break;
                case "2007":
                    this.PrintOutWord2007ToPdf(wordFileFullName, pdfFileFullName, null);
                    break;
            }
        }        

        /// <summary>
        /// 利用Word 2003生成PDF文档
        /// </summary>
        /// <param name="WordFileFullName">Word文件存放的全路径</param>
        /// <param name="PdfFileFullName">Pdf文件存放的全路径</param>
        public void PrintOutWordToPdf(string wordFileFullName, string pdfFileFullName, Word.Document wordDocument)
        {
            if (wordDocument == null)
            {
                this.CreateWordDocument(wordFileFullName, ref wordDocument);
            }

            this.PDFPrintOut(wordDocument, pdfFileFullName);
        }

        /// <summary>
        /// 利用Word 2007生成PDF文档
        /// </summary>
        /// <param name="WordFileFullName">Word文件存放的全路径</param>
        /// <param name="PdfFileFullName">Pdf文件存放的全路径</param>
        public void PrintOutWord2007ToPdf(string wordFileFullName, string pdfFileFullName, object thisDocument)
        {
            this.PDFPrintOut(wordFileFullName, pdfFileFullName, thisDocument);
        }
    }
}