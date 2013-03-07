using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace MarkMars.PDF
{
    internal class ProjectPart
	{
		public String path;
		internal String pages = null;

        public Int32[] Pages
        {
            get
            {
                ArrayList ps = new ArrayList();
                if (this.pages == null || pages.Length == 0)
                {
                    for (int index = 0; index < this.MaxPage; index++)
                    {
                        ps.Add(index);
                    }
                }
                else
                {
                    String[] ss = this.pages.Split(new char[] { ',',' ',';' });
                    foreach (String s in ss)
                        if (Regex.IsMatch(s, @"\d+-\d+"))
                        {
                            Int32 start = Int32.Parse(s.Split(new Char[] { '-' })[0]);
                            Int32 end = Int32.Parse(s.Split(new Char[] { '-' })[1]);
                            if (start > end)
                                return new Int32[] { 0 };
                            while (start <= end)
                            {
                                ps.Add(start-1);
                                start++;
                            }
                        }
                        else
                        {
                            ps.Add(Int32.Parse(s) - 1);
                        }
                }
                return ps.ToArray(typeof(Int32)) as Int32[];
            }           
        }

        public Int32 MaxPage;
        public void Load(String path)
        {
	        this.path = path;
	        PdfFile pf = new PdfFile(File.OpenRead(path));
	        pf.Load();
	        this.MaxPage=pf.PageCount;
        }

        public ProjectPart() { }
	}
}
