using System;
using PXCComLib;
using System.Diagnostics;
using CELL50Lib;                               //这是2000的类库

namespace MarkMars.PDF
{
    /// <summary>
    /// 类：Doc2PDF， 用途：将word文档通过虚拟打印机生成PDF文件。支持多个文档连续生成PDF。
    /// 该类只能初始化一次，不能在多个进程中同时调用。因为是根据创建的虚拟打印机名称来输出PDF的
    /// </summary>
    public class Cell2PDFPrint
    {
        private static CPXCControlEx cPXCControlEx = null;
        private static CPXCPrinter cPXCPrinter = null;
        /// <summary>
        /// 是否已经打印结束
        /// </summary>
        private static bool isPrintEnd = true;
        public static bool IsPrintEnd
        {
            get { return isPrintEnd; }
        }

        #region // begin 初始化系统打印机
        /// <summary>
        /// 初始化系统打印机
        /// </summary>
        public static void CreatNewInstance()
        {
            try
            {
                killPDFSave4();
                cPXCControlEx = new PXCComLib.CPXCControlExClass();
                cPXCControlEx.RemoveOrphanPrinters("", "PDF-XChange 4.0");      //这是比较奇怪的现象，Cell打印输出的时候，某些表单打印的时候，好像找到了以前的默认虚拟打印机！！！所以需要删除掉
                cPXCControlEx.RemoveOrphanPrinters("", "PDF-XChangeTL4");       //这是我们创建的临时虚拟打印机名称
                cPXCPrinter = (CPXCPrinter)cPXCControlEx.get_Printer("", "PDF-XChangeTL4", "PCS40-CXUMA-V4D9C-PQFSU-QGUUX-XMWIZ", "PDFX3$Henry$300604_Allnuts#");
                cPXCPrinter.OnFileSaved += new _IPXCPrinterEvents_OnFileSavedEventHandler(PDFPrinter_OnFileSaved);
                cPXCPrinter.OnStartDoc += new _IPXCPrinterEvents_OnStartDocEventHandler(PDFPrinter_OnStartDoc);
            }
            catch (Exception ex)
            {
                throw new Exception("打印机初始化失败!\n" + ex.Message);
            }
        }
        #endregion // end 初始化系统打印机

        /// <summary>
        /// 调用PDFPrintOut方法，将传入的word文档生成某目标文件(PDF格式)
        /// </summary>
        /// <param name="FCell">Cell对象实例</param>
        /// <param name="outPrintOutFileName">生成的PDF文档名称</param>
        public static void PDFPrintOut(Cell FCell, String outPrintOutFileName)
        {
            try
            {
                FCell.SetPrinter("PDF-XChangeTL4");

                CPXCPrinterSetting(outPrintOutFileName);

                if (FCell != null)
                {
                    FCell.PrintSheet(0, 0);
                }

                FCell = null;
            }
            catch (Exception ex)
            {
                killPDFSave4();

                throw new Exception("打印文件失败！\n" + ex.Message);
            }
        }

        private static void killPDFSave4()
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

        public static void CPXCPrinterSetting(String destFileName)
        {
            isPrintEnd = false;
            cPXCPrinter.ResetDefaults();
            cPXCPrinter.SetAsDefaultPrinter();

            object printFileName = destFileName;

            cPXCPrinter.set_Option("Save.RunApp", "False");       //生成之后不打开PDF文档
            cPXCPrinter.set_Option("Save.File", printFileName);
            cPXCPrinter.set_Option("Save.SaveType", "Save");
            cPXCPrinter.set_Option("Save.ShowSaveDialog", "No");
            cPXCPrinter.set_Option("Save.WhenExists", "Overwrite");
            cPXCPrinter.set_Option("Compression.Graphics", "Yes");
            cPXCPrinter.set_Option("Compression.Text", "Yes");
            cPXCPrinter.set_Option("Compression.ASCII", "No");
            cPXCPrinter.set_Option("Compression.Color.Enabled", "Yes");
            cPXCPrinter.set_Option("Compression.Color.Method", "Auto");
            cPXCPrinter.set_Option("Compression.Indexed.Enabled", "Yes");
            cPXCPrinter.set_Option("Compression.Indexed.Method", "Auto");
            cPXCPrinter.set_Option("Compression.Mono.Enabled", "Yes");
            cPXCPrinter.set_Option("Compression.Mono.Method", "Auto");
            cPXCPrinter.set_Option("Fonts.EmbedAll", "Yes");

            cPXCPrinter.set_Option("Saver.ShowProgress", "No");
        }

        private static void PDFPrinter_OnFileSaved(Int32 JobID, String lpszFileName)
        {
            isPrintEnd = true;
        }

        private static void PDFPrinter_OnStartDoc(Int32 JobID, String lpszFileName, String lpszAppName)
        {
            isPrintEnd = false;
        }

        /// <summary>
        /// 打印结束之后，调用本函数释放WordAppliation
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public static void Dispose()
        {
            while (isPrintEnd == false)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            cPXCPrinter.RestoreDefaultPrinter();
            cPXCControlEx = null;
            cPXCPrinter = null;
        }
    }
}
