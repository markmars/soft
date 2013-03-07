using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace MarkMars.PDF
{
    internal class PdfFileObject
    {
        internal long address;
        internal Int32 number, length;
        internal String text;
        internal PdfFile PdfFile;
        MatchEvaluator filterEval;
        internal static PdfFileObject Create(PdfFile PdfFile, Int32 number, long address)
        {
			PdfFileObject pfo = new PdfFileObject();
            pfo.PdfFile = PdfFile;
            pfo.number = number;
            pfo.address = address;
            pfo.GetLenght(PdfFile);
            pfo.LoadText();
            if (pfo.Type == PdfObjectType.Stream)
            {
                pfo = new PdfFileStreamObject(pfo);
            }
			pfo.filterEval=new MatchEvaluator(pfo.FilterEval);

			return pfo;
        }
        
        private void LoadText()
        {
            this.PdfFile.memory.Seek(this.address, SeekOrigin.Begin);
            StringBuilder sb = new StringBuilder();
            for (Int32 index = 0; index < this.length; index++)
            {
                sb.Append((Char)this.PdfFile.memory.ReadByte());
            }

            this.text = sb.ToString();
        }

        private void GetLenght(PdfFile PdfFile)
        {
            Stream stream = PdfFile.memory;
            stream.Seek(this.address, SeekOrigin.Begin);            
            Match m = Regex.Match("", @"endobj\s*");
            Int32 b = 0;
            this.length = 0;
            String word = "";

            while (b != -1)
            {
                b = stream.ReadByte();
                this.length++;
                if (b > 97 && b < 112)
                {
                    Char c = (Char)b;
                    word += c;
                    if (word == "endobj")
                        b = -1;
                }
                else
                {
                    word = "";                    
                }                
            }

            Char c2 = (Char)stream.ReadByte();
            while (Regex.IsMatch(c2.ToString(), @"\s"))
            {
                this.length++;
                c2 = (Char)stream.ReadByte();
            }
        }

        protected PdfObjectType type;
        internal PdfObjectType Type
        {
            get
            {
                if (this.type==PdfObjectType.UnKnown)
                {
                    if (Regex.IsMatch(this.text, @"/Page") & !Regex.IsMatch(this.text, @"/Pages"))
                    {
                        this.type = PdfObjectType.Page;
                        return this.type;
                    }

                    if (Regex.IsMatch(this.text,@"stream"))
                    {
                        this.type = PdfObjectType.Stream;
                        return this.type;
                    }

                    this.type = PdfObjectType.Other;
                }

                return this.type;
            }
        }        
                        
        internal int[] GetArrayNumbers(String arrayName)
        {
            ArrayList ids = new ArrayList();
            String pattern = @"/" + arrayName + @"\s*\[(\s*(?'id'\d+) 0 R\s*)*";
            Match m = Regex.Match(this.text, pattern,RegexOptions.ExplicitCapture);
            foreach (Capture cap in m.Groups["id"].Captures)
            {
                ids.Add(Int32.Parse(cap.Value));
            }

            return ids.ToArray(typeof(Int32)) as Int32[];
        }

        internal ArrayList GetKids()
        {
           ArrayList kids = new ArrayList();
           foreach (Int32 id in this.GetArrayNumbers("Kids"))
            {
                PdfFileObject pfo = PdfFile.LoadObject(id);
                if (pfo.Type==PdfObjectType.Page)
                {
                    kids.Add(pfo);
                }
                else
                {
                    kids.AddRange(pfo.GetKids());
                }
            }
            return kids;
        }
        
        internal void PopulateRelatedObjects(PdfFile PdfFile,Hashtable container)
        {
            if (!container.ContainsKey(this.number))
            {
                container.Add(this.number, this);
                Match m = Regex.Match(this.text, @"(?'parent'(/Parent)*)\s*(?'id'\d+) 0 R[^G]", RegexOptions.ExplicitCapture);
                while (m.Success)
                {
                    Int32 num = Int32.Parse(m.Groups["id"].Value);
                    Boolean notparent = m.Groups["parent"].Length == 0;
                    if (notparent & !container.Contains(num))
                    {
                        PdfFileObject pfo = PdfFile.LoadObject(num);
                        if (pfo != null & !container.Contains(pfo.number))
                        {
                            pfo.PopulateRelatedObjects(PdfFile, container);
                        }
                    }
                    m = m.NextMatch();
                }
            }
        }

        private Hashtable TransformationHash;
        private String FilterEval(Match m)
        {
            Int32 id = Int32.Parse(m.Groups["id"].Value);
            String end = m.Groups["end"].Value;
            if (this.TransformationHash.ContainsKey(id))
            {
                String rest = m.Groups["rest"].Value;

                return (Int32)TransformationHash[id] + rest + end;
            }

            return end;            
        }

		internal PdfFileObject Parent
		{
			get
			{
				return this.PdfFile.LoadObject(this.text,"Parent");
			}
		}

		internal String MediaBoxText
		{
			get
			{
				String pattern=@"/MediaBox\s*\[\s*(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s+(\+|-)?\d+(.\d+)?\s*]";
				return Regex.Match(this.text,pattern).Value;
			}
		}

		internal virtual void Transform(Hashtable TransformationHash)
        {
			if (this.Type==PdfObjectType.Page && this.MediaBoxText=="")
			{
				PdfFileObject parent=this.Parent;
				while (parent!=null)
				{
					String mb=parent.MediaBoxText;
					if (mb=="")
					{
						parent=parent.Parent;
					}
					else
					{
						this.text=Regex.Replace(this.text,@"/Type\s*/Page","/Type /Page\r"+mb);
						parent=null;
					}
				}
			}

            this.TransformationHash = TransformationHash;
            this.text = Regex.Replace(this.text
            , @"(?'id'\d+)(?'rest' 0 (obj|R))(?'end'[^G])", this.filterEval,RegexOptions.ExplicitCapture);
            this.text = Regex.Replace(this.text, @"/Parent\s+(\d+ 0 R)*", "/Parent 2 0 R \r");            
        }

        internal virtual long WriteToStream(Stream Stream)
        {			
            StreamWriter sw = new StreamWriter(Stream,Encoding.ASCII);
            sw.Write(this.text);
            sw.Flush();
            return this.text.Length;
        }
        
    }

    internal class PdfFileObjectNumberComparer : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            PdfFileObject a = x as PdfFileObject;
            PdfFileObject b = y as PdfFileObject;
            return a.number.CompareTo(b.number);
        }

        #endregion
    }
}

