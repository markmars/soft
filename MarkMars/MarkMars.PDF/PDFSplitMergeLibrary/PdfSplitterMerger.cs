using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MarkMars.PDF
{
    public class PdfSplitterMerger
    {
        private Stream stream;
        long pos = 15;
        private Int32 startNumber = 3;
        private ArrayList pageNumbers, xrefs;
        private Int32 pageCount;
        public Int32 PageCount
        {
            get
            {
                return this.pageCount;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PdfSplitterMerger(String destFileName)
        {
            this.xrefs = new ArrayList();
            this.pageNumbers = new ArrayList();
            this.stream = new FileStream(destFileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(this.stream);
            sw.Write("%PDF-1.4\r");
            sw.Flush();
            Byte[] buffer = new Byte[7];
            buffer[0] = 0x25;
            buffer[1] = 0xE2;
            buffer[2] = 0xE3;
            buffer[3] = 0xCF;
            buffer[4] = 0xD3;
            buffer[5] = 0x0D;
            buffer[6] = 0x0A;
            this.stream.Write(buffer, 0, buffer.Length);
            this.stream.Flush();
        }

        /// <summary>
        /// 开始合并PDF文件
        /// </summary>
        public void MergePDF(String pdfFileName)
        {            
            FileStream fs = File.OpenRead(pdfFileName);
            PdfFile pdfFile = new PdfFile(fs);
            try
            {
                pdfFile.Load();
            }
            catch
            {
                fs.Close();
                fs.Dispose();

                throw new Exception("你导入的PDF文件无效或格式不正确！");
            }

            this.pageCount = pdfFile.PageCount;
            if (this.pageCount == 0)
            {
                fs.Close();
                fs.Dispose();

                throw new Exception("你导入的PDF文件无效或格式不正确！");
            }
            PdfSplitter pdfSplitter = new PdfSplitter();
            Int32[] arrPageNumbers = new Int32[pdfFile.PageCount];
            for (Int32 i = 0; i < arrPageNumbers.Length; i++)
            {
                arrPageNumbers[i] = i;
            }
            pdfSplitter.Load(pdfFile, arrPageNumbers, this.startNumber);
            this.Add(pdfSplitter);
            fs.Close();
            fs.Dispose();
        }

        private void Add(PdfSplitter PdfSplitter)
        {
            foreach (Int32 pageNumber in PdfSplitter.pageNumbers)
            {
                this.pageNumbers.Add(PdfSplitter.transHash[pageNumber]);
            }

            ArrayList sortedObjects = new ArrayList();
            foreach (PdfFileObject pfo in PdfSplitter.sObjects.Values)
            {
                sortedObjects.Add(pfo);
            }

            sortedObjects.Sort(new PdfFileObjectNumberComparer());

            foreach (PdfFileObject pfo in sortedObjects)
            {
                this.xrefs.Add(pos);
                this.pos += pfo.WriteToStream(this.stream);
                this.startNumber++;
            }
        }

        /// <summary>
        /// 完成PDF合并
        /// </summary>
        public void Finish()
        {
            StreamWriter sw = new StreamWriter(this.stream);

            String root = "";
            root = "1 0 obj\r";
            root += "<< \r/Type /Catalog \r";
            root += "/Pages 2 0 R \r";
            root += ">> \r";
            root += "endobj\r";

            xrefs.Insert(0, pos);
            pos += root.Length;
            sw.Write(root);

            String pages = "";
            pages += "2 0 obj \r";
            pages += "<< \r";
            pages += "/Type /Pages \r";
            pages += "/Count " + pageNumbers.Count + " \r";
            pages += "/Kids [ ";
            foreach (Int32 pageIndex in pageNumbers)
            {
                pages += pageIndex + " 0 R ";
            }
            pages += "] \r";
            pages += ">> \r";
            pages += "endobj\r";

            xrefs.Insert(1, pos);
            pos += pages.Length;
            sw.Write(pages);


            sw.Write("xref\r");
            sw.Write("0 " + (this.startNumber) + " \r");
            sw.Write("0000000000 65535 f \r");

            foreach (long xref in this.xrefs)
                sw.Write((xref + 1).ToString("0000000000") + " 00000 n \r");
            sw.Write("trailer\r");
            sw.Write("<<\r");
            sw.Write("/Size " + (this.startNumber) + "\r");
            sw.Write("/Root 1 0 R \r");
            sw.Write(">>\r");
            sw.Write("startxref\r");
            sw.Write((pos + 1) + "\r");
            sw.Write("%%EOF\r");
            sw.Flush();
            sw.Close();
            sw.Dispose();
            this.stream.Close();
            this.stream.Dispose();
        }

        public Int32 GetPageCount(String sourceFileName)
        {
            PdfFile pdfFile = new PdfFile(File.OpenRead(sourceFileName));
            pdfFile.Load();

            return pdfFile.PageCount;
        }
    }
}
