using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MarkMars.Common.ImportExcel
{
	public partial class FormImportExcel : Form
	{
		private OleDbConnection m_Connection = null;

		private DataTable m_dtResult = null;
		public DataTable ResultTable
		{
			get
			{
				return m_dtResult;
			}
		}

        private string[] m_strsResultFields = null;
        private string[] m_strsForceResultFields = null;

		public FormImportExcel(string[] strsResultFields, string[] strsForceResultFields)
		{
			InitializeComponent();

            m_strsResultFields = strsResultFields;
            m_strsForceResultFields = strsForceResultFields;
            foreach (string strField in m_strsResultFields)
            {
                this.dataGridViewMap.Rows.Add();
                DataGridViewCell cell = this.dataGridViewMap.Rows[this.dataGridViewMap.Rows.Count - 1].Cells[0];
                cell.Value = strField;
            }

			groupBox2.Enabled = false;
			groupBox3.Enabled = false;
		}

		private void m_btnBrowser_Click(object sender, EventArgs e)
		{
			OpenFileDialog f = new OpenFileDialog();
			f.Filter = "Excel 文件|*.xls;*.xlsx";
			if (f.ShowDialog() == DialogResult.OK)
			{
				this.m_tbFileName.Text = f.FileName;
				Step1Complete();
			}
		}

		private void Step1Complete()
		{
			string strConnection;
            if (m_tbFileName.Text.EndsWith(".xlsx", StringComparison.CurrentCultureIgnoreCase))
                strConnection = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=NO;IMEX=1;READONLY=TRUE\";",
					m_tbFileName.Text);
            else
                strConnection = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;READONLY=TRUE\"",
                    m_tbFileName.Text);
			try
			{
				m_Connection = new OleDbConnection(strConnection);
				m_Connection.Open();
				Step2Begin();
			}
			catch (Exception e)
			{
				m_Connection = null;
				System.Windows.Forms.MessageBox.Show(e.Message);
			}
		}

		private void Step2Begin()
		{
			groupBox2.Enabled = true;
			groupBox3.Enabled = false;

            LoadExcel2TabControl(m_Connection, this.tabControl1);
            tabControl1_Selected(null, null);
            m_nColumn_RowIndex = -1;
            m_nColumn_TabIndex = -1;
		}

        private bool m_bRemovingTab = false;
        private void LoadExcel2TabControl(OleDbConnection conn, System.Windows.Forms.TabControl tabControl)
        {
            m_bRemovingTab = true;
            while (tabControl.TabPages.Count > 0)
                tabControl.TabPages.RemoveAt(0);
            m_bRemovingTab = false;

            DataTable dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow row in dtSchema.Rows)
            {
                string strTableName = row["TABLE_NAME"].ToString();
                if (strTableName.EndsWith("Print_Titles"))
                    continue;
                TabPage tp = new TabPage(strTableName);
                tabControl.TabPages.Add(tp);
            }
        }

		private void m_cbTables_SelectedIndexChanged(object sender, EventArgs e)
		{
			Step2Complete();
		}

		private void Step2Complete()
		{
			Step3Begin();
		}

		private void Step3Begin()
		{
			groupBox3.Enabled = true;

            this.Excel字段.Items.Clear();
            foreach (DataGridViewCell cell in (this.tabControl1.TabPages[m_nColumn_TabIndex].Controls[0] as DataGridView).Rows[m_nColumn_RowIndex].Cells)
            {
                if (cell.Value == null)
                    continue;
                this.Excel字段.Items.Add(cell.Value.ToString());
            }

            foreach (DataGridViewRow row in this.dataGridViewMap.Rows)
            {
                if (this.Excel字段.Items.Contains(row.Cells[0].Value))
                    row.Cells[1].Value = row.Cells[0].Value;
                else
                    row.Cells[1].Value = null;
            }
		}

		private void m_cbFirstRowAsFieldName_CheckedChanged(object sender, EventArgs e)
		{
			if (m_tbFileName.Text != string.Empty)
				Step1Complete();
		}

		private void m_btnOK_Click(object sender, EventArgs e)
		{
			if (m_Connection == null)
			{
				System.Windows.Forms.MessageBox.Show("请选择一个excel文件");
				return;
			}

            if (m_nColumn_RowIndex == -1 || m_nColumn_TabIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("请选择字段行");
                return;
            }

            //错误检查： dataGridViewMap中Excel字段， 每一个值不能出现多次
            Dictionary<string, object> hsUsedField = new Dictionary<string, object>();
            foreach (DataGridViewRow row in this.dataGridViewMap.Rows)
            {
                if (row.Cells[1].Value == null)
                    continue;
                string strField = row.Cells[1].Value.ToString();
                if (hsUsedField.ContainsKey(strField))
                {
                    System.Windows.Forms.MessageBox.Show("Excel字段 " + strField + " 被多次映射");
                    return;
                }
                hsUsedField.Add(strField, null);
            }

            foreach (string strForceField in m_strsForceResultFields)
            {
                foreach (DataGridViewRow row in this.dataGridViewMap.Rows)
                {
                    string str目标字段 = row.Cells[0].Value.ToString();
                    if (strForceField == str目标字段)
                    {
                        if (row.Cells[1].Value == null)
                        {
                            System.Windows.Forms.MessageBox.Show("目标字段 " + str目标字段 + " 必须被映射");
                            return;
                        }
                    }
                }
            }

            CreateResultTable();

			this.DialogResult = DialogResult.OK;
		}

        private void CreateResultTable()
        {
            DataGridView dgv = this.tabControl1.TabPages[m_nColumn_TabIndex].Controls[0] as DataGridView;
            DataTable dtOriginalSource = dgv.DataSource as DataTable;
            DataTable dtSource = dtOriginalSource.Clone();
            foreach (DataRow row in dtOriginalSource.Rows)
                dtSource.Rows.Add(row.ItemArray);

            Dictionary<string, int> dict目标字段_SourceColumnIndex = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
            foreach (DataGridViewRow row in this.dataGridViewMap.Rows)
            {
                string str目标字段 = row.Cells[0].Value.ToString();

                if (dict目标字段_SourceColumnIndex.ContainsKey(str目标字段))
                    continue;

                string strExcel字段;
                if (row.Cells[1].Value == null)
                    strExcel字段 = null;
                else
                    strExcel字段 = row.Cells[1].Value.ToString();

                if (strExcel字段 == null)
                    dict目标字段_SourceColumnIndex.Add(str目标字段, -1);
                else
                {
                    DataRow rowSource = dtSource.Rows[m_nColumn_RowIndex];
                    for (int i = 0; i < rowSource.ItemArray.Length; i++)
                    {
                        if (strExcel字段 == rowSource.ItemArray[i].ToString())
                        {
                            dict目标字段_SourceColumnIndex.Add(str目标字段, i);
                            break;
                        }
                    }
                    if (!dict目标字段_SourceColumnIndex.ContainsKey(str目标字段))
                        dict目标字段_SourceColumnIndex.Add(str目标字段, -1);
                }
            }

            //把dtSource中没有被映射到的列删了
            for (int i = dtSource.Columns.Count - 1; i >= 0; i--)
            {
                if (dict目标字段_SourceColumnIndex.ContainsValue(i))
                    dtSource.Columns[i].ColumnName = GetKeyByValue(dict目标字段_SourceColumnIndex, i);
                else
                    dtSource.Columns.RemoveAt(i);
            }

            //把dict目标字段_SourceColumnIndex中没有被映射的列加上去
            foreach (string strField in dict目标字段_SourceColumnIndex.Keys)
            {
                if (dict目标字段_SourceColumnIndex[strField] == -1)
                {
                    DataColumn dc = new DataColumn(strField);
                    dtSource.Columns.Add(dc);
                }
            }

            if (this.radioButtonImportAll.Checked)
            {
                for (int i = 0; i <= m_nColumn_RowIndex; i++)
                    dtSource.Rows.RemoveAt(0);
            }
            else
            {
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in dgv.SelectedRows)
                {
                    listRowIndex.Add(row.Index);
                }
                listRowIndex.Sort();

                List<DataRow> listRow = new List<DataRow>();
                foreach (int nIndex in listRowIndex)
                {
                    if (nIndex != m_nColumn_RowIndex)
                    {
                        DataRow rowNew = dtSource.NewRow();
                        rowNew.ItemArray = dtSource.Rows[nIndex].ItemArray;
                        listRow.Add(rowNew);
                    }
                }

                dtSource.Rows.Clear();
                foreach (DataRow row in listRow)
                    dtSource.Rows.Add(row);
            }

            this.m_dtResult = dtSource;
        }

        private string GetKeyByValue(Dictionary<string, int> dictString_Int, int nInt)
        {
            foreach (KeyValuePair<string, int> kvp in dictString_Int)
            {
                if (kvp.Value == nInt)
                    return kvp.Key;
            }
            return null;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (m_bRemovingTab)
                return;

            TabPage tp = tabControl1.SelectedTab;
            if (tp.Controls.Count != 0)
                return;

            DataGridView dgv = new DataGridView();
            dgv.MultiSelect = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersVisible = false;
            dgv.Dock = DockStyle.Fill;
            tp.Controls.Add(dgv);
            DataTable dt = new DataTable();
            string strSQL = string.Format("select * from [{0}]",
                tp.Text);
            OleDbDataAdapter da = new OleDbDataAdapter(strSQL, m_Connection);
            da.Fill(dt);
            dgv.DataSource = dt;
        }

        private int m_nColumn_TabIndex = -1;
        private int m_nColumn_RowIndex = -1;
        private void buttonSetColumn_Click(object sender, EventArgs e)
        {
            if (m_nColumn_TabIndex != -1)
            {
                TabPage tp = this.tabControl1.TabPages[m_nColumn_TabIndex];
                DataGridView dgv = tp.Controls[0] as DataGridView;
                dgv.Rows[m_nColumn_RowIndex].DefaultCellStyle = dgv.DefaultCellStyle;
            }

            m_nColumn_TabIndex = this.tabControl1.SelectedIndex;
            m_nColumn_RowIndex = (this.tabControl1.SelectedTab.Controls[0] as DataGridView).CurrentRow.Index;
            DataGridViewCellStyle st = new DataGridViewCellStyle();
            st.BackColor = Color.Red;
            (this.tabControl1.SelectedTab.Controls[0] as DataGridView).CurrentRow.DefaultCellStyle = st;

            Step2Complete();
        }

        private void dataGridViewMap_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
	}
}
