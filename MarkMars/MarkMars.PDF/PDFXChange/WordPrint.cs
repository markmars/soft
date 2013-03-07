using System;
using System.Collections.Generic;
using System.Text;
using oWord = Word;
using PXCComLib;
using System.Diagnostics;
using System.Runtime.InteropServices;                                  //这是2000的类库
using MarkMars.Common2;

namespace MarkMars.PDF
{
    public class WordPrint
    {
        CPXCControlEx cPXCControlEx;
        CPXCPrinter cPXCPrinter;
        oWord._Application thisApplication = null;
        oWord._Document thisDocument = null;
        private bool isPrintEnd;
        public bool IsPrintEnd
        {
            get { return this.isPrintEnd; }
        }

        public WordPrint()
        {
            try
            {
                killPDFSave4();
                try
                {
                    this.cPXCControlEx = new PXCComLib.CPXCControlExClass();
                }
                catch
                {
                    System.Threading.Thread.Sleep(1000);
                    this.cPXCControlEx = new PXCComLib.CPXCControlExClass();
                }
                cPXCControlEx.RemoveOrphanPrinters("", "PDF-XChange 4.0");      //这是比较奇怪的现象，Cell打印输出的时候，某些表单打印的时候，好像找到了以前的默认虚拟打印机！！！所以需要删除掉
                cPXCControlEx.RemoveOrphanPrinters("", "PDF-XChangeTL4");       //这是我们创建的临时虚拟打印机名称
                try
                {
                    this.cPXCPrinter = (CPXCPrinter)cPXCControlEx.get_Printer("",
                        "PDF-XChangeTL4",
                        "PCS40-CXUMA-V4D9C-PQFSU-QGUUX-XMWIZ",
                        "PDFX3$Henry$300604_Allnuts#");
                }
                catch
                {
                    this.cPXCPrinter = (CPXCPrinter)cPXCControlEx.get_Printer("",
                        "PDF-XChange 4.0",
                        "PCS40-CXUMA-V4D9C-PQFSU-QGUUX-XMWIZ",
                        "PDFX3$Henry$300604_Allnuts#");
                }

                this.cPXCPrinter.OnFileSaved += new _IPXCPrinterEvents_OnFileSavedEventHandler(PDFPrinter_OnFileSaved);
                this.cPXCPrinter.OnStartDoc += new _IPXCPrinterEvents_OnStartDocEventHandler(PDFPrinter_OnStartDoc);
            }
            catch (COMException cEx)
            {
                // 获取ComException的异常信息
                this.Dispose();
                throw new Exception("初始化打印机失败！\r\n" + cEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("初始化打印机失败！\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 调用PDFPrintOut方法，将传入的word文档生成某目标文件(PDF格式)
        /// </summary>
        /// <param name="sourceFileName">传入的word文档名称</param>
        /// <param name="destFileName">生成的PDF文档名称</param>
        public void PDFPrintOut(String sourceFileName, String destFileName)
        {
            //killPDFSave4();
            try
            {
                CPXCPrinterSetting(destFileName);

                CreateWordDocument(sourceFileName, ref thisApplication, ref thisDocument);
                /*
                object background = true;
                object append = false;
                object range = oWord.WdPrintOutRange.wdPrintAllDocument;
                object outputFileName = destFileName;
                object from = System.Type.Missing;
                object to = System.Type.Missing;
                object item = oWord.WdPrintOutItem.wdPrintDocumentContent;
                object copies = "1";
                object pages = System.Type.Missing;
                object pageType = oWord.WdPrintOutPages.wdPrintAllPages;
                object printToFile = false; // true;
                object collate = false;
                object activePrinterMacGX = System.Type.Missing;
                object manualDuplexPrint = System.Type.Missing;
                object printZoomColumn = System.Type.Missing;
                object printZoomRow = System.Type.Missing;
                object printZoomPaperWidth = System.Type.Missing;
                object printZoomPaperHeight = System.Type.Missing;
                object fileName = System.Type.Missing; // destFileName;
                //*/
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


                //thisApplication.PrintOut(ref background, ref append, ref range, ref outputFileName, ref from, ref to, ref item, ref copies, ref pages, ref pageType, ref printToFile,
                //  ref collate, ref fileName, ref activePrinterMacGX, ref manualDuplexPrint, ref printZoomColumn, ref printZoomRow, ref printZoomPaperWidth, ref printZoomPaperHeight);
                thisDocument.PrintOut(ref background, ref append, ref range, ref outputFileName, ref from, ref to, ref item, ref copies, ref pages, ref pageType, ref printToFile,
                    ref collate, ref activePrinterMacGX, ref manualDuplexPrint, ref printZoomColumn, ref printZoomRow, ref printZoomPaperWidth, ref printZoomPaperHeight);
                //Int32 printStatus = thisApplication.BackgroundPrintingStatus;
                //while (printStatus > 0)
                //{
                //    printStatus = thisApplication.BackgroundPrintingStatus;
                //}

            }
            catch (COMException cEx)
            {
                // 获取ComException的异常信息
                this.Dispose();
                throw new Exception("打印文件失败！\r\n" + cEx.Message);
            }
            catch (Exception ex)
            {
                // 获取ComException的异常信息
                this.Dispose();
                throw new Exception("打印文件失败！\r\n" + ex.Message);

            }
            //thisDocument.GetType().InvokeMember("PrintOut", System.Reflection.BindingFlags.InvokeMethod, null, thisDocument, new object[] { false, false, oWord.WdPrintOutRange.wdPrintAllDocument, destFileName });

            /*
            Object saveChanges = false;
            Object originalFormat = System.Type.Missing;
            Object routeDocument = System.Type.Missing;
            thisDocument.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            */
        }

        private void killPDFSave4()
        {
            //可能出现异常：打印完毕之后，再保存PDF文件时，可能pdfsaver4.exe占用了输出的PDF文件，这时需要杀死这个进程
            Process[] processOnComputer = Process.GetProcesses();
            foreach (Process process in processOnComputer)
            {
                if (process.ProcessName.ToLower() == "pdfsaver4")
                {
                    process.Kill();
                    break;
                }
            }
        }

        private void CreateWordDocument(String FileName, ref oWord._Application thisApplication, ref oWord._Document thisDocument)
        {
            if (thisApplication == null)
                thisApplication = new oWord.Application();
            thisApplication.Visible = false;        //true; // true; //
            thisApplication.Caption = "";
            thisApplication.Options.CheckSpellingAsYouType = false;
            thisApplication.Options.CheckGrammarAsYouType = false;


            Object filename = FileName;
            Object ConfirmConversions = false;
            Object ReadOnly = true;
            Object AddToRecentFiles = false;
            Object Visible = true;

            Object PasswordDocument = System.Type.Missing;
            Object PasswordTemplate = System.Type.Missing;
            Object Revert = System.Type.Missing;
            Object WritePasswordDocument = System.Type.Missing;
            Object WritePasswordTemplate = System.Type.Missing;
            Object Format = System.Type.Missing;
            Object Encoding = System.Type.Missing;

            Object OpenAndRepair = System.Type.Missing;
            Object DocumentDirection = System.Type.Missing;
            Object NoEncodingDialog = System.Type.Missing;
            Object XMLTransform = System.Type.Missing;

            //word2007的方式打开
            //thisDocument = thisApplication.Documents.Open(ref filename, ref ConfirmConversions, ref ReadOnly, ref AddToRecentFiles, ref PasswordDocument, ref PasswordTemplate,
            //    ref Revert, ref WritePasswordDocument, ref WritePasswordTemplate, ref Format, ref Encoding, ref Visible, ref OpenAndRepair, ref DocumentDirection, ref NoEncodingDialog, ref XMLTransform);
            //word2000的方式打开,以后的版本自动兼容这个方法
            thisDocument = thisApplication.Documents.Open(ref filename,
                ref ConfirmConversions,
                ref ReadOnly,
                ref AddToRecentFiles,
                ref PasswordDocument, ref PasswordTemplate,
                ref Revert, ref WritePasswordDocument, ref WritePasswordTemplate, ref Format, ref Encoding, ref Visible); //, ref OpenAndRepair, ref DocumentDirection, ref NoEncodingDialog, ref XMLTransform);
        }

        public void CPXCPrinterSetting(String destFileName)
        {
            this.cPXCPrinter.ResetDefaults();
            this.cPXCPrinter.SetAsDefaultPrinter();

            object printFileName = destFileName;

            this.cPXCPrinter.set_Option("Save.RunApp", "False");       //生成之后不打开PDF文档
            this.cPXCPrinter.set_Option("Save.File", printFileName);
            this.cPXCPrinter.set_Option("Save.SaveType", "Save");
            this.cPXCPrinter.set_Option("Save.ShowSaveDialog", "No");
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

            //关闭进度条--注意：可能会出现错误，需要反复测试, 特别是连续打印word文档的时候
            this.cPXCPrinter.set_Option("Saver.ShowProgress", "No");
        }

        private void PDFPrinter_OnFileSaved(Int32 JobID, String lpszFileName)
        {
            this.isPrintEnd = true;
        }

        private void PDFPrinter_OnStartDoc(Int32 JobID, String lpszFileName, String lpszAppName)
        {
            this.isPrintEnd = false;
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
            if (this.thisDocument != null)
            {
                saveChanges = oWord.WdSaveOptions.wdDoNotSaveChanges;
                thisDocument.Close(ref saveChanges, ref originalFormat,
                    ref routeDocument);
            }
            if (thisApplication != null)
            {
                saveChanges = false;
                thisApplication.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
            }
            if (this.cPXCPrinter != null)
            {
                this.cPXCPrinter.RestoreDefaultPrinter();
                this.cPXCControlEx = null;
                this.cPXCPrinter = null;
            }
            killPDFSave4();
        }
    }
}
