using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace MarkMars.Common
{
    public partial class FormSQLServerConnection : Form
    {
		public string Server
        {
            get
            {
                return m_tbServerName.Text;
            }
        }

		public string Database
        {
            get
            {
                return m_tbDatabaseName.Text;
            }
        }

		public string User
        {
            get
            {
                return m_tbUserName.Text;
            }
        }

		public string Password
        {
            get
            {
                return m_tbPassword.Text;
            }
        }

		public bool IntegratedSecurity
        {
            get
            {
                return m_cbWindowsAuthentication.Checked;
            }
        }

        public FormSQLServerConnection(string strServerName, string strDatabaseName, string strUserName, string strPassword, bool bIntegratedSecurity)
        {
            InitializeComponent();

            m_tbServerName.Text = strServerName;
            m_tbDatabaseName.Text = strDatabaseName;
            m_tbUserName.Text = strUserName;
            m_tbPassword.Text = strPassword;
            m_cbWindowsAuthentication.Checked = bIntegratedSecurity;
        }

		public FormSQLServerConnection()
		{
			InitializeComponent();
		}

        private void m_btnOK_Click(object sender, EventArgs e)
        {
            try
            {
				string strConnection = "data source = " + m_tbServerName.Text + ";initial catalog = " + m_tbDatabaseName.Text;
				if (m_cbWindowsAuthentication.Checked)
				{
					strConnection += ";Integrated Security = SSPI";
				}
				else
				{
					strConnection += ";user id = ";
					strConnection += m_tbUserName.Text;
					strConnection += ";password = ";
					strConnection += m_tbPassword.Text;
				}
				SqlConnection conn = new SqlConnection(strConnection);
				conn.Open();
				conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败:" + ex.Message);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

		private void m_cbWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
		{
			if (m_cbWindowsAuthentication.Checked)
			{
				m_tbUserName.Enabled = false;
				m_tbPassword.Enabled = false;
			}
			else
			{
				m_tbUserName.Enabled = true;
				m_tbPassword.Enabled = true;
			}
		}
    }
}
