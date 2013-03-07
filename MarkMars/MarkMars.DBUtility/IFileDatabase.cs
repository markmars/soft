using System;
using System.Collections.Generic;
using System.Text;

namespace MarkMars.DBUtility
{
    internal interface IFileDatabase : IDatabase
	{
		string FileName
		{
			get;
		}
	}
}
