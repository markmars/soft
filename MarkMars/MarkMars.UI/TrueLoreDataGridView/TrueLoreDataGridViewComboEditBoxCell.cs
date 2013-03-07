using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace MarkMars.UI
{
    public class TrueLoreDataGridViewComboEditBoxCell : DataGridViewComboBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            ComboBox comboBox = (ComboBox)base.DataGridView.EditingControl;

            if (comboBox != null)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox.Validating += new CancelEventHandler(this.comboBox_Validating);
            }
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            if (value != null)
            {
                if (value.ToString().Trim() != string.Empty)
                {
                    if (Items.IndexOf(value) == -1)
                    {
                        Items.Add(value);
                        DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)OwningColumn;
                        col.Items.Add(value);
                    }
                }
            }
            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }

        void comboBox_Validating(object sender, CancelEventArgs e)
        {
            DataGridViewComboBoxEditingControl cbo = (DataGridViewComboBoxEditingControl)sender;
            if (cbo.Text.Trim() == string.Empty) return;

            DataGridView grid = cbo.EditingControlDataGridView;
            object value = cbo.Text;

            // Add value to list if not there
            if (cbo.Items.IndexOf(value) == -1)
            {
                DataGridViewComboBoxColumn cboCol = (DataGridViewComboBoxColumn)grid.Columns[grid.CurrentCell.ColumnIndex];
                // Must add to both the current combobox as well as the template, to avoid duplicate entries
                cbo.Items.Add(value);
                cboCol.Items.Add(value);
                grid.CurrentCell.Value = value;
            }
        }
    }
}
