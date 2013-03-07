using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using MarkMars.Common.Winform;

namespace MarkMars.Common.Database
{
	public class SQLServerConnectionSetting
	{
		private static Dictionary<string, SQLServerConnectionSetting> m_dictName_Instance = new Dictionary<string,SQLServerConnectionSetting>(StringComparer.CurrentCultureIgnoreCase);
		public static SQLServerConnectionSetting GetInstance(string strName)
		{
            if (string.IsNullOrEmpty(strName))
                return GetInstance(MarkMars.Common.Utility.AssemblyTitle);

            if (m_dictName_Instance.ContainsKey(strName))
                return m_dictName_Instance[strName];
            else
            {
                m_dictName_Instance.Add(strName, new SQLServerConnectionSetting(strName));
                return m_dictName_Instance[strName];
            }
		}

        private string m_strName = null;

		private SQLServerConnectionSetting(string strName)
		{
            m_strName = strName;
			Load();
		}

		private void Load()
		{
            RegistryKey rkRoot = Registry.CurrentUser.OpenSubKey("Software\\筑龙\\" + m_strName, false);
			if (rkRoot == null)
				return;

			Server = rkRoot.GetValue("Server", string.Empty).ToString();
			Database = rkRoot.GetValue("Database", string.Empty).ToString();
			User = rkRoot.GetValue("User", string.Empty).ToString();

			try
			{
				IntegratedSecurity = (int)rkRoot.GetValue("IntegratedSecurity", 0) != 0;
				Password = EncryptDecrypt.DecryptString(rkRoot.GetValue("Password", string.Empty).ToString());
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show("读取数据库配置出错:" + e.Message);
			}
		}

		public void Save()
		{
			RegistryKey rkRoot = Registry.CurrentUser.OpenSubKey("Software", true);
			Debug.Assert(rkRoot != null);

			RegistryKey rkCompany = rkRoot.CreateSubKey("筑龙");
			Debug.Assert(rkCompany != null);
            RegistryKey rkProduct = rkCompany.CreateSubKey(m_strName);
			Debug.Assert(rkProduct != null);
			rkProduct.SetValue("Server", Server);
			rkProduct.SetValue("Database", Database);
			rkProduct.SetValue("User", User);
			rkProduct.SetValue("Password", EncryptDecrypt.EncryptString(Password));
			rkProduct.SetValue("IntegratedSecurity", IntegratedSecurity ? 1 : 0);
		}

		public string Server
		{
			get;
			set;
		}

		public string Database
		{
			get;
			set;
		}

		public string User
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public bool IntegratedSecurity
		{
			get;
			set;
		}
	}
}
