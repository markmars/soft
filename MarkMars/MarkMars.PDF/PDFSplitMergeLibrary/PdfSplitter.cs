using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace MarkMars.PDF
{
	internal class PdfSplitter
    {
        internal Hashtable sObjects;
        internal ArrayList pageNumbers;
        internal Hashtable  transHash;
        internal PdfFile PdfFile;

        /// <summary>
        /// ¹¹Ôìº¯Êý
        /// </summary>
        internal PdfSplitter() { }

        internal void Load(PdfFile PdfFile, Int32[] PageNumbers, Int32 startNumber)
		{
			this.PdfFile = PdfFile;
			this.pageNumbers = new ArrayList();
			this.sObjects = new Hashtable();
            Int32 part = 0;
            Int32 total = PageNumbers.Length;
            foreach (Int32 PageNumber in PageNumbers)
			{
				PdfFileObject page = PdfFile.PageList[PageNumber] as PdfFileObject;
				page.PopulateRelatedObjects(PdfFile, this.sObjects);
				this.pageNumbers.Add(page.number);
				part++;
			}

			this.transHash = this.CalcTransHash(startNumber);
			foreach (PdfFileObject pfo in this.sObjects.Values)
			{
				pfo.Transform(transHash);
			}
		}

        private Hashtable CalcTransHash(Int32 startNumber)
        {
            Hashtable ht = new Hashtable();
            ArrayList al = new ArrayList();
            foreach (PdfFileObject pfo in this.sObjects.Values)
            {
                al.Add(pfo);
            }

            al.Sort(new PdfFileObjectNumberComparer());
            Int32 number = startNumber;
            foreach (PdfFileObject pfo in al)
            {
                ht.Add(pfo.number, number);
                number++;
            }

            return ht;
        }
    }
}

