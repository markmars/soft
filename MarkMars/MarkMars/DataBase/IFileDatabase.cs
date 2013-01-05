using System;
using System.Collections.Generic;
using System.Text;

namespace MarkMars.Database
{
	public interface IFileDatabase : IDatabase
	{
		string FileName
		{
			get;
		}
	}
}
