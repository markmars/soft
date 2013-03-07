using System;
using System.Collections.Generic;
using System.Text;

namespace MarkMars.Common.Tree
{
	public class AutoID
	{
		private static int m_nNextValidID = 0;
		private int m_nAutoID = m_nNextValidID++;
	}
}
