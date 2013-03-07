using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MarkMars.UI
{
    public class TrueLoreDataGridViewComboEditBoxColumn : DataGridViewComboBoxColumn
    {
        public TrueLoreDataGridViewComboEditBoxColumn()
        {
            TrueLoreDataGridViewComboEditBoxCell obj = new TrueLoreDataGridViewComboEditBoxCell();
            this.CellTemplate = obj;
        }
    }
}
