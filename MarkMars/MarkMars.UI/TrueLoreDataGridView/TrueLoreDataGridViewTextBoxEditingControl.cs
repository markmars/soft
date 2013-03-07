using System;
using System.Windows.Forms;

namespace MarkMars.UI
{
    /// <summary>
    /// MarkMars.UI.TrueLoreDataGridView 中的文本框控件。
    /// </summary>
    public class TrueLoreDataGridViewTextBoxEditingControl : TextBox, IDataGridViewEditingControl
    {
        private DataGridView m_dataGridView = null;
        private Int32 m_rowIndex = -1;
        private Boolean m_valueChanged = false;

        #region IDataGridViewEditingControl 成员

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;

            if (dataGridViewCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight || dataGridViewCellStyle.Alignment == DataGridViewContentAlignment.TopRight
                || dataGridViewCellStyle.Alignment == DataGridViewContentAlignment.BottomRight)
            {
                this.TextAlign = HorizontalAlignment.Right;
            }
            else
            {
                this.TextAlign = HorizontalAlignment.Left;
            }
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return this.m_dataGridView;
            }
            set
            {
                this.m_dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value == null)
                {
                    this.Text = String.Empty;
                }
                else
                {
                    this.Text = value.ToString();
                }
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return this.m_rowIndex;
            }
            set
            {
                this.m_rowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return this.m_valueChanged;
            }
            set
            {
                this.m_valueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.Escape:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            selectAll = true;
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion

        #region 重载方法

        protected override void OnTextChanged(EventArgs e)
        {
            this.m_valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(e);
        }

        //protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        //{
        //    this.ValidateInput();
        //    //Boolean isValidInput = this.ValidateInput();
        //    //e.Cancel = !isValidInput;

        //    //base.OnValidating(e);
        //}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                TrueLoreDataGridView tldgv = this.m_dataGridView as TrueLoreDataGridView;
                if (tldgv != null && tldgv.ReadOnly2)
                {
                    e.Handled = true;
                    return;
                }

                this.Text = String.Empty;
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 || e.KeyChar == 9 || e.KeyChar == 27 || e.KeyChar == 38 || e.KeyChar == 40) //Enter or Tab or Esc or Up or Down
            {
                e.Handled = true;
                return;
            }

            base.OnKeyPress(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            TrueLoreDataGridView tldgv = this.m_dataGridView as TrueLoreDataGridView;
            if (tldgv != null)
            {
                TrueLoreDataGridViewTextBoxEditingControl ctl = this.m_dataGridView.EditingControl as TrueLoreDataGridViewTextBoxEditingControl;
                if (ctl != null)
                {
                    ctl.ReadOnly = tldgv.ReadOnly2;
                }
            }

            this.SelectAll();
            base.OnEnter(e);
        }

        //protected override void OnLeave(EventArgs e)
        //{
        //    InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
        //    base.OnLeave(e);
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.Focused && keyData == (Keys.Control | Keys.C))
            {
                this.Copy();
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #endregion
    }
}
