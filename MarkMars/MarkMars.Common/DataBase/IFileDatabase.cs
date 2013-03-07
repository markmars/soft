using System;
using System.Collections.Generic;
using System.Text;

namespace MarkMars.Common.Database
{
	public interface IFileDatabase : IDatabase
	{
		string FileName
		{
			get;
		}
	}
}
