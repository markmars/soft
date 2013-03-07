using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MarkMars.UI
{
    /// <summary>
    /// 显示 MarkMars.UI.TrueLoreDataGridView 控件中可编辑文本信息。
    /// </summary>
    public class TrueLoreDataGridViewTextBoxCell : DataGridViewTextBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            TrueLoreDataGridViewTextBoxEditingControl ctl = this.DataGridView.EditingControl as TrueLoreDataGridViewTextBoxEditingControl;
            if (ctl != null)
            {
                switch (dataGridViewCellStyle.Alignment)
                {
                    case DataGridViewContentAlignment.BottomCenter:
                    case DataGridViewContentAlignment.MiddleCenter:
                    case DataGridViewContentAlignment.TopCenter:
                        ctl.TextAlign = HorizontalAlignment.Center;
                        break;
                    case DataGridViewContentAlignment.BottomRight:
                    case DataGridViewContentAlignment.MiddleRight:
                    case DataGridViewContentAlignment.TopRight:
                        ctl.TextAlign = HorizontalAlignment.Right;
                        break;
                    default:
                        ctl.TextAlign = HorizontalAlignment.Left;
                        break;
                }

                TrueLoreDataGridView tldgv = this.DataGridView as TrueLoreDataGridView;
                if (tldgv != null)
                {
                    TrueLoreDataGridViewTextBoxColumn column = this.OwningColumn as TrueLoreDataGridViewTextBoxColumn;
                    if (column != null)
                    {
                        ctl.ReadOnly = tldgv.ReadOnly2;
                        ctl.MaxLength = column.MaxInputLength;
                    }
                }

                ctl.BorderStyle = BorderStyle.None;
                ctl.EditingControlRowIndex = rowIndex;
                ctl.Text = initialFormattedValue.ToString();
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(TrueLoreDataGridViewTextBoxEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(String);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return String.Empty;
            }
        }

        protected override bool SetValue(int rowIndex, object value)
        {
            if (this.OwningColumn != null)
            {
                Type valueType = this.OwningColumn.ValueType;

                if ((value == null || value.ToString() == String.Empty) && (valueType == typeof(Byte)
                    || valueType == typeof(Int16) || valueType == typeof(Int32) || valueType == typeof(Int64)
                    || valueType == typeof(UInt16) || valueType == typeof(UInt32) || valueType == typeof(UInt64)
                     || valueType == typeof(Single) || valueType == typeof(Double) || valueType == typeof(Decimal)
                     || valueType == typeof(DateTime)))
                {
                    value = DBNull.Value;
                }
            }

            return base.SetValue(rowIndex, value);
        }

        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
        {
            if (formattedValue == null || formattedValue.ToString().Trim() == String.Empty)
            {
                return String.Empty;
            }

            return base.ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
        }
    }
}
