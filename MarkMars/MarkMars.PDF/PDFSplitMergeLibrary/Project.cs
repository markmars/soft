using System;
using System.Collections;

namespace MarkMars.PDF
{
    internal class Project
	{
		public String Name;
		public ArrayList Parts;
        public Int32 TotalPages
		{
			get
			{
                Int32 tot = 0;
				foreach (ProjectPart pp in this.Parts)
				{
					if (pp.Pages == null)
					{
						tot += pp.MaxPage;
					}
					else
					{
						tot += pp.Pages.Length;
					}
				}
				return tot;
			}
		}

		public Project(String name)
		{
			this.Parts = new ArrayList();
			this.Name = name;
		}
	}
}
