using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MarkMars.UI
{
    /// <summary>
    /// 承载一个 MarkMars.UI.TrueLoreDataGridViewTextBoxCell 单元格集合。
    /// </summary>
    public class TrueLoreDataGridViewTextBoxColumn : DataGridViewColumn
    {
        private Int32 m_maxInputLength = 32767;

        public TrueLoreDataGridViewTextBoxColumn()
            : base(new TrueLoreDataGridViewTextBoxCell())
        {
        }

        #region 公共属性

        [Category("Behavior"), DefaultValue(32767), Description("可以在编辑文本框中输入的最大字符数。")]
        public Int32 MaxInputLength
        {
            get { return this.m_maxInputLength; }
            set { this.m_maxInputLength = value; }
        }

        #endregion

        #region 重载方法

        public override object Clone()
        {
            TrueLoreDataGridViewTextBoxColumn column = (TrueLoreDataGridViewTextBoxColumn)base.Clone();
            column.ReadOnly = false;
            column.MaxInputLength = this.MaxInputLength;

            return column;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(TrueLoreDataGridViewTextBoxCell)))
                {
                    throw new InvalidCastException("Must be a MarkMars.UI.TrueLoreDataGridViewTextBoxCell!");
                }

                base.CellTemplate = value;
            }
        }

        #endregion
    }
}
