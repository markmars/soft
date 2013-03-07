using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using PXCComLib;

namespace MarkMars.PDF
{
    /// <summary>
    /// 类：PDFXPrinter， 用途：创建一个虚拟打印机，并设置为默认的打印机。
    /// 在调用应用程序的打印时，先调用该类设置某个虚拟打印机。
    /// 该类只能初始化一次，不能在多个进程中同时调用。因为是根据创建的虚拟打印机名称来输出PDF的
    /// </summary>
    public class PDFXPrinter
    {
        public delegate void FileSavedEventHandler(object sender, EventArgs e);
        public event FileSavedEventHandler OnFileSaved;

        private CPXCControlEx cPXCControlEx;
        private CPXCPrinter cPXCPrinter;

        public PDFXPrinter()
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
                cPXCControlEx.RemoveOrphanPrinters("", "PDF-XChange 4.0");      //这是比较奇怪的现象，Cell打印输出的时候，某些表单打印的时候，好像找到了以前的默认虚拟打印机！！！所以需要删除掉
                cPXCControlEx.RemoveOrphanPrinters("", "PDF-XChangeTL4");
            }
            catch
            {
            }
            this.cPXCPrinter = (CPXCPrinter)cPXCControlEx.get_Printer("", "PDF-XChangeTL4", "PCS40-CXUMA-V4D9C-PQFSU-QGUUX-XMWIZ", "PDFX3$Henry$300604_Allnuts#");
            this.cPXCPrinter.OnFileSaved += new _IPXCPrinterEvents_OnFileSavedEventHandler(PDFPrinter_OnFileSaved);
            this.cPXCPrinter.OnError += new _IPXCPrinterEvents_OnErrorEventHandler(PDFPrinter_OnError);
        }

        /// <summary>
        /// 在主程序执行打印时，先设置打印机的环境。这里重点是传入参数，即生成的PDF文件的存放位置（完整的文件名称，包含路径）
        /// </summary>
        /// <param name="destFileName">生成的PDF文件的文件名称，包含路径</param>
        public void CPXCPrinterSetting(String destFileName)
        {
            //this.cPXCPrinter.ResetDefaults();
            this.cPXCPrinter.SetAsDefaultPrinter();

            object printFileName = destFileName;

            //this.cPXCPrinter.set_Option("Paper.LayoutType", "Normal");

            this.cPXCPrinter.set_Option("Save.RunApp", "False");       //生成之后不打开PDF文档
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

            //关闭进度条--注意：可能会出现错误，需要反复测试, 特别是连续打印word文档的时候
            //this.cPXCPrinter.set_Option("Saver.ShowProgress", "No");
        }

        /// <summary>
        /// 打印机生成PDF文件完毕时的回调事件
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="lpszFileName"></param>
        private void PDFPrinter_OnFileSaved(Int32 JobID, String lpszFileName)
        {
            this.cPXCPrinter.RestoreDefaultPrinter();
            if (this.OnFileSaved != null)
            {
                this.OnFileSaved(this, new EventArgs());
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

        private void PDFPrinter_OnError(Int32 JobID, Int32 dwErrorCode)
        {
            MessageBox.Show(dwErrorCode.ToString());
            this.KillPDFSave4();
        }

        /// <summary>
        /// 打印结束之后，调用本函数释放打印机对象
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public void Dispose()
        {
            //this.cPXCPrinter.RestoreDefaultPrinter();
            this.cPXCControlEx = null;
            this.cPXCPrinter = null;
            this.KillPDFSave4();
        }
    }
}