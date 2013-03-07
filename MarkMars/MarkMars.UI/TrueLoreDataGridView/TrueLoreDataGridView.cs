using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MarkMars.UI
{
    public class TrueLoreDataGridView : DataGridView
    {
        private Boolean m_readOnly2 = false;
        private Boolean[] m_columnsReadOnly = null;
        private Color headerBackColor = Color.White;
        private Color headerForeColor = Color.Black;
        private Color alternatingRowBackColor = Color.WhiteSmoke;
        private Color selectedRowBackColor = Color.LightSkyBlue;

        #region 公共属性
        [Category("Data"), DefaultValue(null), Description("控件的数据源。")]
        public new Object DataSource
        {
            get { return base.DataSource; }
            set
            {
                try
                {
                    base.DataSource = value;

                    foreach (DataGridViewColumn col in this.Columns)
                    {
                        if (col.ValueType == typeof(Int16) || col.ValueType == typeof(Int32) || col.ValueType == typeof(Int64)
                            || col.ValueType == typeof(Byte) || col.ValueType == typeof(UInt16) || col.ValueType == typeof(UInt32)
                            || col.ValueType == typeof(Single) || col.ValueType == typeof(Double) || col.ValueType == typeof(Decimal))
                        {
                            if (!(col is DataGridViewComboBoxColumn || col is DataGridViewCheckBoxColumn))
                            {
                                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }
                        }
                        else if (col.ValueType == typeof(Boolean))
                        {
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                        else if (col.ValueType == typeof(String))
                        {
                            col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 获取或者设置是否只读（设置为只读后， MarkMars.UI.TrueLoreDataGridViewTextBoxColumn 列中的文本框内容仍然可以选择）。
        /// </summary>
        [ToolboxItem(false)]
        public Boolean ReadOnly2
        {
            get { return this.m_readOnly2; }
            set
            {
                this.m_readOnly2 = value;
                this.ReadOnly = value;

                Boolean isExists = false;
                for (Int32 idx = 0; idx < this.Columns.Count; idx++)
                {
                    if (this.Columns[idx] is TrueLoreDataGridViewTextBoxColumn)
                    {
                        isExists = true;
                        break;
                    }
                }

                if (!isExists)
                {
                    return;
                }

                this.ReadOnly = false;

                if (value)
                {
                    if (this.m_columnsReadOnly == null)
                    {
                        this.m_columnsReadOnly = new Boolean[this.Columns.Count];
                    }

                    DataGridViewColumn dgvc = null;
                    for (Int32 idx = 0; idx < this.Columns.Count; idx++)
                    {
                        dgvc = this.Columns[idx];
                        this.m_columnsReadOnly[idx] = dgvc.ReadOnly;

                        if (dgvc is TrueLoreDataGridViewTextBoxColumn)
                        {
                            dgvc.ReadOnly = false;
                        }
                        else
                        {
                            dgvc.ReadOnly = true;
                        }
                    }
                }
                else
                {
                    if (this.m_columnsReadOnly != null)
                    {
                        for (Int32 idx = 0; idx < this.Columns.Count; idx++)
                        {
                            this.Columns[idx].ReadOnly = this.m_columnsReadOnly[idx];
                        }
                    }
                }
            }
        }

        public Color HeaderBackColor
        {
            get { return this.headerBackColor; }
            set { this.headerBackColor = value; }
        }

        public Color HeaderForeColor
        {
            get { return this.headerForeColor; }
            set { this.headerForeColor = value; }
        }

        public Color AlternatingRowBackColor
        {
            get { return this.alternatingRowBackColor; }
            set { this.alternatingRowBackColor = value; }
        }

        public Color SelectedRowBackColor
        {
            get { return this.selectedRowBackColor; }
            set { this.selectedRowBackColor = value; }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 初始化控件。
        /// </summary>
        public void Initialize()
        {
            this.Initialize(true);
        }

        /// <summary>
        /// 初始化控件。
        /// </summary>
        /// <param name="readOnly">是否只读。</param>
        public void Initialize(Boolean readOnly)
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            headerStyle.Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
            headerStyle.BackColor = this.headerBackColor;
            headerStyle.ForeColor = this.headerForeColor;

            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle();
            rowStyle.BackColor = Color.White;

            DataGridViewCellStyle alternatingRowStyle = new DataGridViewCellStyle();
            alternatingRowStyle.BackColor = this.alternatingRowBackColor;

            if (readOnly)
            {
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.MultiSelect = false;
            }

            this.AllowUserToAddRows = !readOnly;
            this.AllowUserToDeleteRows = !readOnly;
            this.ReadOnly2 = readOnly;
            this.AutoGenerateColumns = false;
            //this.EditMode = DataGridViewEditMode.EditOnEnter;
            this.BackgroundColor = Color.White;
            this.RowHeadersWidth = 30;
            //2010-06-12 Youker 默认设置列表左边第一列不显示
            this.RowHeadersVisible = false;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ColumnHeadersHeight = 30;
            this.ColumnHeadersDefaultCellStyle = headerStyle;
            this.RowsDefaultCellStyle = rowStyle;
            this.AlternatingRowsDefaultCellStyle = alternatingRowStyle;

            this.DefaultCellStyle.SelectionBackColor = this.selectedRowBackColor;

            //2010-09-13 dengt 单元格内容自动换行，并且自动调整高度。
            this.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        #endregion

        #region 重载方法
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != DBNull.Value)
            {
                if (this.Columns[e.ColumnIndex].ValueType == typeof(DateTime))
                {
                    if (String.IsNullOrEmpty(this.Columns[e.ColumnIndex].DefaultCellStyle.Format))
                    {
                        e.Value = TypeHelper.DateTimeToLongString(Convert.ToDateTime(e.Value));
                        e.FormattingApplied = true;
                    }
                }
            }

            base.OnCellFormatting(e);
        }
        #endregion
    }
}
