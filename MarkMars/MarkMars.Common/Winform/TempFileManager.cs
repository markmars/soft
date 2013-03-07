using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MarkMars.Common
{
	public class TempFileManager
	{
		private static string m_strFolder = null;

		static TempFileManager()
		{
			m_strFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Utility.AssemblyTitle);
			Directory.CreateDirectory(m_strFolder);

			DirectoryInfo di = new DirectoryInfo(m_strFolder);
			int nErrorCount = 0;
			DeleteFolder(di, false, ref nErrorCount);

			if (nErrorCount > 10)
			{
				System.Windows.Forms.MessageBox.Show("请手工删除" + m_strFolder + "目录以后再运行软件");
			}

			Directory.CreateDirectory(m_strFolder);
		}

		private static void DeleteFolder(DirectoryInfo di, bool bIncludeSelf, ref int nErrorCount)
		{
			FileInfo[] fis = di.GetFiles();
			foreach (FileInfo fi in fis)
			{
				try
				{
					fi.Delete();
				}
				catch
				{
					nErrorCount++;
				}
			}

			DirectoryInfo[] dis = di.GetDirectories();
			foreach (DirectoryInfo di2 in dis)
				DeleteFolder(di2, true, ref nErrorCount);

			if (bIncludeSelf)
			{
				try
				{
					di.Delete(true);
				}
				catch
				{
					nErrorCount++;
				}
			}
		}

		public static string GetNewFileName()
		{
			return m_strFolder + "\\" + Guid.NewGuid().ToString() + ".tmp";
		}

		public static string GetNewFolderPath()
		{
			return m_strFolder + "\\" + Guid.NewGuid().ToString();
		}
	}
}
