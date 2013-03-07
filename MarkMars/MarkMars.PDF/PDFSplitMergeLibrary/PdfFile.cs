using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MarkMars.PDF
{
    internal class PdfFile
    {
		internal String trailer;
        internal Stream memory;
        internal Hashtable objects;

        /// <summary>
        /// ¹¹Ôìº¯Êý
        /// </summary>        
        internal PdfFile(Stream InputStream)
        {
            this.memory = InputStream;
        }

        internal void Load()
        {
            long startxref = this.GetStartxref();
            this.trailer = this.ParseTrailer(startxref);
            long[] adds = this.GetAddresses(startxref);
            this.LoadHash(adds);
        }

		private void LoadHash(long[] addresses)
		{
			this.objects = new Hashtable();
			Int32 part = 0;
            Int32 total = addresses.Length;
			foreach (long add in addresses)
			{
				this.memory.Seek(add, SeekOrigin.Begin);
				StreamReader sr = new StreamReader(this.memory);
				String line = sr.ReadLine();
                if (line.Length < 2)
                {
                    line = sr.ReadLine();
                }
				Match m = Regex.Match(line, @"(?'id'\d+)( )+0 obj",RegexOptions.ExplicitCapture);
				if (m.Success)
				{
                    Int32 num = Int32.Parse(m.Groups["id"].Value);
					if (!objects.ContainsKey(num))
					{
						objects.Add(num, PdfFileObject.Create(this,num,add));
					}
				}
				part++;
			}
		}
        
        internal PdfFileObject LoadObject(String text, String key)
        {
            String pattern = @"/"+key+@" (?'id'\d+)";
            Match m = Regex.Match(text, pattern, RegexOptions.ExplicitCapture);
			if (m.Success)
			{
                return this.LoadObject(Int32.Parse(m.Groups["id"].Value));
			}
			return null;
        }

        internal PdfFileObject LoadObject(Int32 number)
        {
            return this.objects[number] as PdfFileObject;
        }

        internal ArrayList PageList
        {
            get
            {
                PdfFileObject root = this.LoadObject(this.trailer, "Root");
                PdfFileObject pages = this.LoadObject(root.text, "Pages");
                return pages.GetKids();
            }
        }

        internal int PageCount
        {
            get
            {
                return this.PageList.Count;
            }
        }

        private long[] GetAddresses(long xref)
        {
            this.memory.Seek(xref, SeekOrigin.Begin);
            ArrayList al = new ArrayList();
            StreamReader sr = new StreamReader(this.memory);
            String line="";
            String prevPattern = @"/Prev \d+";
            Boolean ok = true;
            while (ok)
            {
                if (Regex.IsMatch(line, @"\d{10} 00000 n\s*"))
                {
                    al.Add(long.Parse(line.Substring(0,10)));
                }
                
                line = sr.ReadLine();
                ok = !(line == null || Regex.IsMatch(line, ">>"));
                if (line != null)
                {
                    Match m = Regex.Match(line, prevPattern);
                    if (m.Success)
                    {
                        al.AddRange(this.GetAddresses(long.Parse(m.Value.Substring(6))));
                    }
                }                              
            }

            return al.ToArray(typeof(long)) as long[];
        }   

        private long GetStartxref()
        {
            StreamReader sr = new StreamReader(this.memory);
            this.memory.Seek(this.memory.Length - 100, SeekOrigin.Begin);
            String line="";
            while (!line.StartsWith("startxref"))
            {
                line = sr.ReadLine();
            }

            long startxref = long.Parse(sr.ReadLine());
            if (startxref == -1)
            {
                throw new Exception("Cannot find the startxref");
            }

            return startxref;
        }

        private string ParseTrailer(long xref)
        {
            this.memory.Seek(xref, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(this.memory);
            String line;
            String trailer = "";
            Boolean istrailer = false;
            while ((line = sr.ReadLine()) != "startxref")
            {
                line = line.Trim();
                if (line.StartsWith("trailer"))
                {
                    trailer = "";
                    istrailer = true;
                }
                if (istrailer)
                {
                    trailer += line + "\r";
                }
            }
            if (trailer == "")
                throw new Exception("Cannot find trailer");
            return trailer;
        }
    }
}

