using System;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;

namespace MarkMars.UI
{
    public class DataGridViewRadioButtonCell : DataGridViewComboBoxCell, IDataGridViewEditingCell
    {
        /// <summary>
        /// Convenient enumeration using privately for calculating preferred cell sizes.
        /// </summary>
        private enum DataGridViewRadioButtonFreeDimension
        {
            Both,
            Height,
            Width
        }

        // 4 pixels of margin on the left and right of error icons
        private const byte DATAGRIDVIEWRADIOBUTTONCELL_iconMarginWidth = 4;
        // 4 pixels of margin on the top and bottom of error icons
        private const byte DATAGRIDVIEWRADIOBUTTONCELL_iconMarginHeight = 4;
        // all icons are 12 pixels wide by default
        private const byte DATAGRIDVIEWRADIOBUTTONCELL_iconsWidth = 12;
        // all icons are 11 pixels tall by default
        private const byte DATAGRIDVIEWRADIOBUTTONCELL_iconsHeight = 11;

        // default value of MaxDisplayedItems property
        internal const int DATAGRIDVIEWRADIOBUTTONCELL_defaultMaxDisplayedItems = 3; 
        // blank pixels around each radio button entry
        private const byte DATAGRIDVIEWRADIOBUTTONCELL_margin = 2;

        // codes used for the mouseLocationCode static variable:
        private const int DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric = -3;
        private const int DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationBottomScrollButton = -2; // mouse is over bottom scroll button
        private const int DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationTopScrollButton = -1;    // mouse is over top scroll button

        private DataGridViewRadioButtonCellLayout layout;   // represents the current layout information of the cell

        private static int mouseLocationCode = DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric; 
                                               // -3 no particular location
                                               // -2 mouse over bottom scroll button
                                               // -1 mouse over top scroll button
                                               // 0-N mouse over radio button glyph
        
        private PropertyDescriptor displayMemberProperty;   // Property descriptor for the DisplayMember property
        private PropertyDescriptor valueMemberProperty;     // Property descriptor for the ValueMember property
        private CurrencyManager dataManager;                // Currency manager for the cell's DataSource
        private int maxDisplayedItems;                      // Maximum number of radio buttons displayed by the cell
        private int selectedItemIndex;                      // Index of the currently selected radio button entry
        private int focusedItemIndex;                       // Index of the focused radio button entry
        private int pressedItemIndex;                       // Index of the currently pressed radio button entry
        private bool dataSourceInitializedHookedUp;         // Indicates whether the DataSource's Initialized event is listened to
        private bool valueChanged;                          // Stores whether the cell's value was changed since it became the current cell
        private bool handledKeyDown;                        // Indicates whether the cell handled the key down notification
        private bool mouseUpHooked;                         // Indicates whether the cell listens to the grid's MouseUp event
        
        /// <summary>
        /// DataGridViewRadioButtonCell class constructor.
        /// </summary>
        public DataGridViewRadioButtonCell()
        {
            this.maxDisplayedItems = DATAGRIDVIEWRADIOBUTTONCELL_defaultMaxDisplayedItems;
            this.layout = new DataGridViewRadioButtonCellLayout();
            this.selectedItemIndex = -1;
            this.focusedItemIndex = -1;
            this.pressedItemIndex = -1;
        }

        // Implementation of the IDataGridViewEditingCell interface starts here.

        /// <summary>
        /// Represents the cell's formatted value
        /// </summary>
        public virtual object EditingCellFormattedValue
        {
            get
            {
                return GetEditingCellFormattedValue(DataGridViewDataErrorContexts.Formatting);
            }
            set
            {
                if (this.FormattedValueType == null)
                {
                    throw new ArgumentException("FormattedValueType property of a cell cannot be null.");
                }
                if (value == null || !this.FormattedValueType.IsAssignableFrom(value.GetType()))
                {
                    // Assigned formatted value may not be of the good type, in cases where the app
                    // is feeding wrong values to the cell in virtual / databound mode.
                    throw new ArgumentException("The value provided for the DataGridViewRadioButtonCell has the wrong type.");
                }

                // Try to locate the item that corresponds to the 'value' provided.
                for (int itemIndex = 0; itemIndex < this.Items.Count; itemIndex++)
                {
                    object item = this.Items[itemIndex];
                    object displayValue = GetItemDisplayValue(item);
                    if (value.Equals(displayValue))
                    {
                        // 'value' was found. It becomes the new selected item.
                        this.selectedItemIndex = itemIndex;
                        return;
                    }
                }

                string strValue = value as string;
                if (strValue == string.Empty)
                {
                    // Special case the empty string situation - reset the selected item
                    this.selectedItemIndex = -1;
                    return;
                }

                // 'value' could not be matched against an item in the Items collection.
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Keeps track of whether the cell's value has changed or not.
        /// </summary>
        public virtual bool EditingCellValueChanged
        {
            get
            {
                return this.valueChanged;
            }
            set
            {
                this.valueChanged = value;
            }
        }

        /// <summary>
        /// Returns the current formatted value of the cell
        /// </summary>
        public virtual object GetEditingCellFormattedValue(DataGridViewDataErrorContexts context)
        {
            if (this.FormattedValueType == null)
            {
                throw new InvalidOperationException("FormattedValueType property of a cell cannot be null.");
            }
            if (this.selectedItemIndex == -1)
            {
                return null;
            }
            object item = this.Items[this.selectedItemIndex];
            object displayValue = GetItemDisplayValue(item);
            // Making sure the returned value has an acceptable type
            if (this.FormattedValueType.IsAssignableFrom(displayValue.GetType()))
            {
                return displayValue;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Called by the grid when the cell enters editing mode. 
        /// </summary>
        public virtual void PrepareEditingCellForEdit(bool selectAll)
        {
            // This cell type has nothing to do here.
        }

        // Implementation of the IDataGridViewEditingCell interface stops here.

        /// <summary>
        /// Stores the CurrencyManager associated to the cell's DataSource
        /// </summary>
        private CurrencyManager DataManager
        {
            get
            {
                CurrencyManager cm = this.dataManager;
                if (cm == null && this.DataSource != null && this.DataGridView != null && 
                    this.DataGridView.BindingContext != null && !(this.DataSource == Convert.DBNull))
                {
                    ISupportInitializeNotification dsInit = this.DataSource as ISupportInitializeNotification;
                    if (dsInit != null && !dsInit.IsInitialized)
                    {
                        // The datasource is not ready yet. Attaching to its Initialized event to be notified
                        // when it's finally ready
                        if (!this.dataSourceInitializedHookedUp)
                        {
                            dsInit.Initialized += new EventHandler(DataSource_Initialized);
                            this.dataSourceInitializedHookedUp = true;
                        }
                    }
                    else
                    {
                        cm = (CurrencyManager)this.DataGridView.BindingContext[this.DataSource];
                        this.DataManager = cm;
                    }
                }
                return cm;
            }
            set
            {
                this.dataManager = value;
            }
        }

        /// <summary>
        /// Overrides the DataGridViewComboBox's implementation of the DataSource property to 
        /// initialize the displayMemberProperty and valueMemberProperty members.
        /// </summary>
        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                if (this.DataSource != value)
                {
                    // Invalidate the currency manager
                    this.DataManager = null;

                    ISupportInitializeNotification dsInit = this.DataSource as ISupportInitializeNotification;
                    if (dsInit != null && this.dataSourceInitializedHookedUp)
                    {
                        // If we previously hooked the datasource's ISupportInitializeNotification
                        // Initialized event, then unhook it now (we don't always hook this event,
                        // only if we needed to because the datasource was previously uninitialized)
                        dsInit.Initialized -= new EventHandler(DataSource_Initialized);
                        this.dataSourceInitializedHookedUp = false;
                    }

                    base.DataSource = value;

                    // Update the displayMemberProperty and valueMemberProperty members.
                    try
                    {
                        InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
                    }
                    catch
                    {
                        Debug.Assert(this.DisplayMember != null && this.DisplayMember.Length > 0);
                        InitializeDisplayMemberPropertyDescriptor(null);
                    }

                    try
                    {
                        InitializeValueMemberPropertyDescriptor(this.ValueMember);
                    }
                    catch
                    {
                        Debug.Assert(this.ValueMember != null && this.ValueMember.Length > 0);
                        InitializeValueMemberPropertyDescriptor(null);
                    }

                    if (value == null)
                    {
                        InitializeDisplayMemberPropertyDescriptor(null);
                        InitializeValueMemberPropertyDescriptor(null);
                    }
                }
            }
        }

        /// <summary>
        /// Overrides the DataGridViewComboBox's implementation of the DisplayMember property to
        /// update the displayMemberProperty member.
        /// </summary>
        public override string DisplayMember
        {
            get
            {
                return base.DisplayMember;
            }
            set
            {
                base.DisplayMember = value;
                InitializeDisplayMemberPropertyDescriptor(value);
            }
        }

        /// <summary>
        /// Overrides the base implementation to replace the 'complex editing experience'
        /// with a 'simple editing experience'.
        /// </summary>
        public override Type EditType
        {
            get
            {
                // Return null since no editing control is used for the editing experience.
                return null;
            }
        }

        /// <summary>
        /// Custom property that represents the maximum number of radio buttons shown by the cell.
        /// </summary>
        [
            DefaultValue(DATAGRIDVIEWRADIOBUTTONCELL_defaultMaxDisplayedItems)
        ]
        public int MaxDisplayedItems
        {
            get
            {
                return this.maxDisplayedItems;
            }
            set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("MaxDisplayedItems");
                }
                this.maxDisplayedItems = value;

                if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
                {
                    if (this.RowIndex == -1)
                    {
                        // Invalidate and autosize column
                        this.DataGridView.InvalidateColumn(this.ColumnIndex);

                        // TODO: Add code to autosize the cell's column, the rows, the column headers 
                        // and the row headers depending on their autosize settings.
                        // The DataGridView control does not expose a public method that takes care of this.
                    }
                    else
                    {
                        // The DataGridView control exposes a public method called UpdateCellValue
                        // that invalidates the cell so that it gets repainted and also triggers all
                        // the necessary autosizing: the cell's column and/or row, the column headers
                        // and the row headers are autosized depending on their autosize settings.
                        this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                    }
                }
            }
        }

        /// <summary>
        /// Called internally by the DataGridViewRadioButtonColumn class to avoid the invalidation
        /// done by the MaxDisplayedItems setter above (for performance reasons).
        /// </summary>
        internal int MaxDisplayedItemsInternal
        {
            set
            {
                Debug.Assert(value >= 1 && value <= 100);
                this.maxDisplayedItems = value;
            }
        }

        /// <summary>
        /// Utility function that returns the standard thickness (in pixels) of the four borders of the cell.
        /// </summary>
        private Rectangle StandardBorderWidths
        {
            get
            {
                if (this.DataGridView != null)
                {
                    DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle(), dgvabsEffective;
                    dgvabsEffective = AdjustCellBorderStyle(this.DataGridView.AdvancedCellBorderStyle,
                        dataGridViewAdvancedBorderStylePlaceholder,
                        false /*singleVerticalBorderAdded*/,
                        false /*singleHorizontalBorderAdded*/,
                        false /*isFirstDisplayedColumn*/,
                        false /*isFirstDisplayedRow*/);
                    return BorderWidths(dgvabsEffective);
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }

        /// <summary>
        /// Overrides the DataGridViewComboBox's implementation of the ValueMember property to
        /// update the valueMemberProperty member.
        /// </summary>
        public override string ValueMember
        {
            get
            {
                return base.ValueMember;
            }
            set
            {
                base.ValueMember = value;
                InitializeValueMemberPropertyDescriptor(value);
            }
        }

        /// <summary>
        /// Utility function that returns the cell state inherited from the owning row and column.
        /// </summary>
        private DataGridViewElementStates CellStateFromColumnRowStates(DataGridViewElementStates rowState)
        {
            Debug.Assert(this.DataGridView != null);
            Debug.Assert(this.ColumnIndex >= 0);
            DataGridViewElementStates orFlags = DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected;
            DataGridViewElementStates andFlags = DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible;
            DataGridViewElementStates cellState = (this.OwningColumn.State & orFlags);
            cellState |= (rowState & orFlags);
            cellState |= ((this.OwningColumn.State & andFlags) & (rowState & andFlags));
            return cellState;
        }

        /// <summary>
        /// Custom implementation of the Clone method to copy over the special properties of the cell.
        /// </summary>
        public override object Clone()
        {
            DataGridViewRadioButtonCell dataGridViewCell = base.Clone() as DataGridViewRadioButtonCell;
            if (dataGridViewCell != null)
            {
                dataGridViewCell.MaxDisplayedItems = this.MaxDisplayedItems;
            }
            return dataGridViewCell;
        }

        /// <summary>
        /// Computes the layout of the cell and optionally paints it.
        /// </summary>
        private void ComputeLayout(Graphics graphics,
                                   Rectangle clipBounds,
                                   Rectangle cellBounds,
                                   int rowIndex,
                                   DataGridViewElementStates cellState,
                                   object formattedValue,
                                   string errorText,
                                   DataGridViewCellStyle cellStyle,
                                   DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                   DataGridViewPaintParts paintParts,
                                   bool paint)
        {
            if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.Border))
            {
                // Paint the borders first
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            }

            // Discard the space taken up by the borders.
            Rectangle borderWidths = BorderWidths(advancedBorderStyle);
            Rectangle valBounds = cellBounds;
            valBounds.Offset(borderWidths.X, borderWidths.Y);
            valBounds.Width -= borderWidths.Right;
            valBounds.Height -= borderWidths.Bottom;

            SolidBrush backgroundBrush = null;
            try
            {
                Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
                bool cellCurrent = ptCurrentCell.X == this.ColumnIndex && ptCurrentCell.Y == rowIndex;
                bool cellSelected = (cellState & DataGridViewElementStates.Selected) != 0;
                bool mouseOverCell = cellBounds.Contains(this.DataGridView.PointToClient(Control.MousePosition));

                if (DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.SelectionBackground) && cellSelected)
                {
                    backgroundBrush = new SolidBrush(cellStyle.SelectionBackColor);
                }
                else
                {
                    backgroundBrush = new SolidBrush(cellStyle.BackColor);
                }

                if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.Background) && backgroundBrush.Color.A == 255)
                {
                    Rectangle backgroundRect = valBounds;
                    backgroundRect.Intersect(clipBounds);
                    graphics.FillRectangle(backgroundBrush, backgroundRect);
                }

                // Discard the space taken up by the padding area.
                if (cellStyle.Padding != Padding.Empty)
                {
                    valBounds.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
                    valBounds.Width -= cellStyle.Padding.Horizontal;
                    valBounds.Height -= cellStyle.Padding.Vertical;
                }

                Rectangle errorBounds = valBounds;
                Rectangle scrollBounds = valBounds;

                this.layout.ScrollingNeeded = GetScrollingNeeded(graphics, rowIndex, cellStyle, valBounds.Size);

                if (this.layout.ScrollingNeeded)
                {
                    this.layout.ScrollButtonsSize = ScrollBarRenderer.GetSizeBoxSize(graphics, ScrollBarState.Normal);
                    // Discard the space required for displaying the 2 scroll buttons
                    valBounds.Width -= this.layout.ScrollButtonsSize.Width;
                }

                valBounds.Inflate(-DATAGRIDVIEWRADIOBUTTONCELL_margin, -DATAGRIDVIEWRADIOBUTTONCELL_margin);

                // Layout / paint the radio buttons themselves
                this.layout.RadioButtonsSize = RadioButtonRenderer.GetGlyphSize(graphics, RadioButtonState.CheckedNormal);
                this.layout.DisplayedItemsCount = 0;
                this.layout.TotallyDisplayedItemsCount = 0;
                if (valBounds.Width > 0 && valBounds.Height > 0)
                {
                    this.layout.FirstDisplayedItemLocation = new Point(valBounds.Left + DATAGRIDVIEWRADIOBUTTONCELL_margin, valBounds.Top + DATAGRIDVIEWRADIOBUTTONCELL_margin);
                    int textHeight = cellStyle.Font.Height;
                    int itemIndex = this.layout.FirstDisplayedItemIndex;
                    Rectangle radiosBounds = valBounds;
                    while (itemIndex < this.Items.Count &&
                           itemIndex < this.layout.FirstDisplayedItemIndex + this.maxDisplayedItems &&                           
                           radiosBounds.Height > 0)
                    {
                        if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.ContentBackground))
                        {
                            Rectangle itemRect = radiosBounds;
                            itemRect.Intersect(clipBounds);
                            if (!itemRect.IsEmpty)
                            {
                                bool itemReadOnly = (cellState & DataGridViewElementStates.ReadOnly) != 0;
                                bool itemSelected = false;
                                if (formattedValue != null)
                                {
                                    object displayValue = GetItemDisplayValue(this.Items[itemIndex]);
                                    if (formattedValue.Equals(displayValue))
                                    {
                                        itemSelected = true;
                                    }
                                }
                                PaintItem(graphics, 
                                          radiosBounds, 
                                          rowIndex, 
                                          itemIndex, 
                                          cellStyle,
                                          itemReadOnly,
                                          itemSelected, 
                                          mouseOverCell,
                                          cellCurrent && this.focusedItemIndex == itemIndex && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.Focus));
                            }
                        }
                        itemIndex++;
                        radiosBounds.Y += textHeight + DATAGRIDVIEWRADIOBUTTONCELL_margin;
                        radiosBounds.Height -= (textHeight + DATAGRIDVIEWRADIOBUTTONCELL_margin);
                        if (radiosBounds.Height >= 0)
                        {
                            this.layout.TotallyDisplayedItemsCount++;
                        }
                        this.layout.DisplayedItemsCount++;
                    }
                    this.layout.ContentBounds = new Rectangle(this.layout.FirstDisplayedItemLocation, new Size(this.layout.RadioButtonsSize.Width, this.layout.DisplayedItemsCount * (textHeight + DATAGRIDVIEWRADIOBUTTONCELL_margin)));
                }
                else
                {
                    this.layout.FirstDisplayedItemLocation = new Point(-1, -1);
                    this.layout.ContentBounds = Rectangle.Empty;
                }

                if (this.layout.ScrollingNeeded)
                {
                    // Layout / paint the 2 scroll buttons
                    Rectangle rectArrow = new Rectangle(scrollBounds.Right - this.layout.ScrollButtonsSize.Width,
                                                        scrollBounds.Top,
                                                        this.layout.ScrollButtonsSize.Width,
                                                        this.layout.ScrollButtonsSize.Height);
                    this.layout.UpButtonLocation = rectArrow.Location;
                    if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.ContentBackground))
                    {
                        ScrollBarRenderer.DrawArrowButton(graphics, rectArrow, GetScrollBarArrowButtonState(true, mouseOverCell ? mouseLocationCode : DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric, this.layout.FirstDisplayedItemIndex > 0 /*enabled*/));
                    }
                    rectArrow.Y = scrollBounds.Bottom - this.layout.ScrollButtonsSize.Height;
                    this.layout.DownButtonLocation = rectArrow.Location;
                    if (paint && DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.ContentBackground))
                    {
                        ScrollBarRenderer.DrawArrowButton(graphics, rectArrow, GetScrollBarArrowButtonState(false, mouseOverCell ? mouseLocationCode : DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric, this.layout.FirstDisplayedItemIndex + this.layout.TotallyDisplayedItemsCount < this.Items.Count /*enabled*/));
                    }
                }

                // Finally paint the potential error icon
                if (paint && 
                    DataGridViewRadioButtonCell.PartPainted(paintParts, DataGridViewPaintParts.ErrorIcon) &&
                    !(cellCurrent && this.DataGridView.IsCurrentCellInEditMode) && 
                    this.DataGridView.ShowCellErrors)
                {
                    PaintErrorIcon(graphics, clipBounds, errorBounds, errorText);
                }
            }
            finally
            {
                if (backgroundBrush != null)
                {
                    backgroundBrush.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns whether calling the OnContentClick method would force the owning row to be unshared.
        /// </summary>
        protected override bool ContentClickUnsharesRow(DataGridViewCellEventArgs e)
        {
            Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
            return ptCurrentCell.X == this.ColumnIndex &&
                   ptCurrentCell.Y == e.RowIndex &&
                   this.DataGridView.IsCurrentCellInEditMode;
        }

        /// <summary>
        /// Raised when the owning grid gets a MouseUp notification
        /// </summary>
        private void DataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            // Unhook the event handler
            this.DataGridView.MouseUp -= new MouseEventHandler(DataGridView_MouseUp);
            this.mouseUpHooked = false;
            // Reset the pressed item index. Since the mouse was released, no item can be pressed anymore.
            this.pressedItemIndex = -1;
        }

        /// <summary>
        /// Raised when the cell's DataSource is initialized.
        /// </summary>
        private void DataSource_Initialized(object sender, EventArgs e)
        {
            Debug.Assert(sender == this.DataSource);
            Debug.Assert(this.DataSource is ISupportInitializeNotification);
            Debug.Assert(this.dataSourceInitializedHookedUp);

            ISupportInitializeNotification dsInit = this.DataSource as ISupportInitializeNotification;
            // Unhook the Initialized event.
            if (dsInit != null)
            {
                dsInit.Initialized -= new EventHandler(DataSource_Initialized);
            }

            // The wait is over: the DataSource is initialized.
            this.dataSourceInitializedHookedUp = false;

            // Check the DisplayMember and ValueMember values - will throw if values don't match existing fields.
            InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
            InitializeValueMemberPropertyDescriptor(this.ValueMember);
        }

        /// <summary>
        /// Returns whether calling the OnEnter method would force the owning row to be unshared.
        /// </summary>
        protected override bool EnterUnsharesRow(int rowIndex, bool throughMouseClick)
        {
            return this.focusedItemIndex == -1;
        }

        /// <summary>
        /// Custom implementation of the GetContentBounds method which delegates most of the work to the ComputeLayout function.
        /// </summary>
        protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
        {
            if (this.DataGridView == null || rowIndex < 0 || this.OwningColumn == null)
            {
                return Rectangle.Empty;
            }

            // First determine the effective border style of this cell.
            bool singleVerticalBorderAdded = !this.DataGridView.RowHeadersVisible && this.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            bool singleHorizontalBorderAdded = !this.DataGridView.ColumnHeadersVisible && this.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();

            Debug.Assert(rowIndex > -1 && this.OwningColumn != null);

            DataGridViewAdvancedBorderStyle dgvabsEffective = AdjustCellBorderStyle(this.DataGridView.AdvancedCellBorderStyle,
                dataGridViewAdvancedBorderStylePlaceholder,
                singleVerticalBorderAdded,
                singleHorizontalBorderAdded,
                rowIndex == this.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Displayed) /*isFirstDisplayedRow*/,
                this.ColumnIndex == this.DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Displayed).Index /*isFirstDisplayedColumn*/);

            // Next determine the state of this cell.
            DataGridViewElementStates rowState = this.DataGridView.Rows.GetRowState(rowIndex);
            DataGridViewElementStates cellState = CellStateFromColumnRowStates(rowState);
            cellState |= this.State;

            // Then the bounds of this cell.
            Rectangle cellBounds = new Rectangle(new Point(0, 0), GetSize(rowIndex));

            // Finally compute the layout of the cell and return the resulting content bounds.
            ComputeLayout(graphics,
                cellBounds,
                cellBounds,
                rowIndex,
                cellState,
                null /*formattedValue*/,            // contentBounds is independent of formattedValue
                null /*errorText*/,                 // contentBounds is independent of errorText
                cellStyle,
                dgvabsEffective,
                DataGridViewPaintParts.ContentForeground,
                false /*paint*/);

            return this.layout.ContentBounds;
        }

        /// <summary>
        /// Utility function that converts a constraintSize provided to GetPreferredSize into a 
        /// DataGridViewRadioButtonFreeDimension enum value.
        /// </summary>
        private static DataGridViewRadioButtonFreeDimension GetFreeDimensionFromConstraint(Size constraintSize)
        {
            if (constraintSize.Width < 0 || constraintSize.Height < 0)
            {
                throw new ArgumentException("InvalidArgument=Value of '" + constraintSize.ToString() + "' is not valid for 'constraintSize'.");
            }
            if (constraintSize.Width == 0)
            {
                if (constraintSize.Height == 0)
                {
                    return DataGridViewRadioButtonFreeDimension.Both;
                }
                else
                {
                    return DataGridViewRadioButtonFreeDimension.Width;
                }
            }
            else
            {
                if (constraintSize.Height == 0)
                {
                    return DataGridViewRadioButtonFreeDimension.Height;
                }
                else
                {
                    throw new ArgumentException("InvalidArgument=Value of '" + constraintSize.ToString() + "' is not valid for 'constraintSize'.");
                }
            }
        }

        /// <summary>
        /// Utility function that returns the display value of an item given the 
        /// display/value property descriptors and display/value property names.
        /// </summary>
        private object GetItemDisplayValue(object item)
        {
            Debug.Assert(item != null);
            bool displayValueSet = false;
            object displayValue = null;
            if (this.displayMemberProperty != null)
            {
                displayValue = this.displayMemberProperty.GetValue(item);
                displayValueSet = true;
            }
            else if (this.valueMemberProperty != null)
            {
                displayValue = this.valueMemberProperty.GetValue(item);
                displayValueSet = true;
            }
            else if (!string.IsNullOrEmpty(this.DisplayMember))
            {
                PropertyDescriptor propDesc = TypeDescriptor.GetProperties(item).Find(this.DisplayMember, true /*caseInsensitive*/);
                if (propDesc != null)
                {
                    displayValue = propDesc.GetValue(item);
                    displayValueSet = true;
                }
            }
            else if (!string.IsNullOrEmpty(this.ValueMember))
            {
                PropertyDescriptor propDesc = TypeDescriptor.GetProperties(item).Find(this.ValueMember, true /*caseInsensitive*/);
                if (propDesc != null)
                {
                    displayValue = propDesc.GetValue(item);
                    displayValueSet = true;
                }
            }
            if (!displayValueSet)
            {
                displayValue = item;
            }
            return displayValue;
        }

        /// <summary>
        /// Utility function that returns the value of an item given the 
        /// display/value property descriptors and display/value property names.
        /// </summary>
        private object GetItemValue(object item)
        {
            bool valueSet = false;
            object value = null;
            if (this.valueMemberProperty != null)
            {
                value = this.valueMemberProperty.GetValue(item);
                valueSet = true;
            }
            else if (this.displayMemberProperty != null)
            {
                value = this.displayMemberProperty.GetValue(item);
                valueSet = true;
            }
            else if (!string.IsNullOrEmpty(this.ValueMember))
            {
                PropertyDescriptor propDesc = TypeDescriptor.GetProperties(item).Find(this.ValueMember, true /*caseInsensitive*/);
                if (propDesc != null)
                {
                    value = propDesc.GetValue(item);
                    valueSet = true;
                }
            }
            if (!valueSet && !string.IsNullOrEmpty(this.DisplayMember))
            {
                PropertyDescriptor propDesc = TypeDescriptor.GetProperties(item).Find(this.DisplayMember, true /*caseInsensitive*/);
                if (propDesc != null)
                {
                    value = propDesc.GetValue(item);
                    valueSet = true;
                }
            }
            if (!valueSet)
            {
                value = item;
            }
            return value;
        }

        /// <summary>
        /// Returns the code identifying the part of the cell which is underneath the mouse pointer.
        /// </summary>
        private int GetMouseLocationCode(Graphics graphics, int rowIndex, DataGridViewCellStyle cellStyle, int mouseX, int mouseY)
        {
            // First determine this cell's effective border style.
            bool singleVerticalBorderAdded = !this.DataGridView.RowHeadersVisible && this.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            bool singleHorizontalBorderAdded = !this.DataGridView.ColumnHeadersVisible && this.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            bool isFirstDisplayedColumn = this.ColumnIndex == this.DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Displayed).Index;
            bool isFirstDisplayedRow = rowIndex == this.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Displayed);
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle(), dataGridViewAdvancedBorderStyleEffective;
            dataGridViewAdvancedBorderStyleEffective = AdjustCellBorderStyle(this.DataGridView.AdvancedCellBorderStyle,
                                                                             dataGridViewAdvancedBorderStylePlaceholder,
                                                                             singleVerticalBorderAdded,
                                                                             singleHorizontalBorderAdded,
                                                                             isFirstDisplayedColumn,
                                                                             isFirstDisplayedRow);
            // Then its size.
            Rectangle cellBounds = this.DataGridView.GetCellDisplayRectangle(this.ColumnIndex, rowIndex, false /*cutOverflow*/);
            Debug.Assert(GetSize(rowIndex) == cellBounds.Size);

            // Recompute the layout of the cell.
            ComputeLayout(graphics,
                          cellBounds,
                          cellBounds,
                          rowIndex,
                          DataGridViewElementStates.None,
                          null /*formattedValue*/,
                          null /*errorText*/,
                          cellStyle,
                          dataGridViewAdvancedBorderStyleEffective,
                          DataGridViewPaintParts.None,
                          false /*paint*/);

            // Deduce the cell part beneath the mouse pointer.
            Point mousePosition = this.DataGridView.PointToClient(Control.MousePosition);
            Rectangle rect;
            if (this.layout.ScrollingNeeded)
            {
                // Is the mouse over the bottom scroll button?
                rect = new Rectangle(this.layout.DownButtonLocation, this.layout.ScrollButtonsSize);
                if (rect.Contains(mousePosition))
                {
                    return DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationBottomScrollButton;
                }
                // Is the mouse over the upper scroll button?
                rect = new Rectangle(this.layout.UpButtonLocation, this.layout.ScrollButtonsSize);
                if (rect.Contains(mousePosition))
                {
                    return DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationTopScrollButton;
                }
            }
            if (this.layout.DisplayedItemsCount > 0)
            {
                Point radioButtonLocation = this.layout.FirstDisplayedItemLocation;
                int textHeight = cellStyle.Font.Height;
                int itemIndex = this.layout.FirstDisplayedItemIndex;
                Rectangle radioButtonBounds = new Rectangle(radioButtonLocation, this.layout.RadioButtonsSize);
                while (itemIndex < this.Items.Count &&
                       itemIndex < this.layout.FirstDisplayedItemIndex + this.maxDisplayedItems &&
                       itemIndex - this.layout.FirstDisplayedItemIndex < this.layout.DisplayedItemsCount)
                {
                    if (radioButtonBounds.Contains(mousePosition))
                    {
                        // The mouse is over a radio button
                        return itemIndex - this.layout.FirstDisplayedItemIndex;
                    }
                    itemIndex++;
                    radioButtonBounds.Y += textHeight + DATAGRIDVIEWRADIOBUTTONCELL_margin;
                }
            }
            return DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric;
        }

        /// <summary>
        /// Returns a ScrollBarArrowButtonState state given the current mouse location.
        /// </summary>
        private ScrollBarArrowButtonState GetScrollBarArrowButtonState(bool upButton, int mouseLocationCode, bool enabled)
        {
            if (!enabled)
            {
                if (upButton)
                {
                    return ScrollBarArrowButtonState.UpDisabled;
                }
                else
                {
                    return ScrollBarArrowButtonState.DownDisabled;
                }
            }
            if (mouseLocationCode == DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationTopScrollButton)
            {
                // Mouse is over upper button
                if (Control.MouseButtons == MouseButtons.Left)
                {
                    if (upButton)
                    {
                        return ScrollBarArrowButtonState.UpPressed;
                    }
                    else
                    {
                        return ScrollBarArrowButtonState.DownNormal;
                    }
                }
                else
                {
                    if (upButton)
                    {
                        return ScrollBarArrowButtonState.UpHot;
                    }
                    else
                    {
                        return ScrollBarArrowButtonState.DownNormal;
                    }
                }
            }
            else if (mouseLocationCode == DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationBottomScrollButton)
            {
                // Mouse is over bottom button
                if (Control.MouseButtons == MouseButtons.Left)
                {
                    if (upButton)
                    {
                        return ScrollBarArrowButtonState.UpNormal;
                    }
                    else
                    {
                        return ScrollBarArrowButtonState.DownPressed;
                    }
                }
                else
                {
                    if (upButton)
                    {
                        return ScrollBarArrowButtonState.UpNormal;
                    }
                    else
                    {
                        return ScrollBarArrowButtonState.DownHot;
                    }
                }
            }

            else if (upButton)
            {
                return ScrollBarArrowButtonState.UpNormal;
            }
            else
            {
                return ScrollBarArrowButtonState.DownNormal;
            }
        }

        /// <summary>
        /// Custom implementation of the GetPreferredSize method.
        /// </summary>
        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (this.DataGridView == null)
            {
                return new Size(-1, -1);
            }
            DataGridViewRadioButtonFreeDimension freeDimension = DataGridViewRadioButtonCell.GetFreeDimensionFromConstraint(constraintSize);
            Rectangle borderWidthsRect = this.StandardBorderWidths;
            int borderAndPaddingWidths = borderWidthsRect.Left + borderWidthsRect.Width + cellStyle.Padding.Horizontal;
            int borderAndPaddingHeights = borderWidthsRect.Top + borderWidthsRect.Height + cellStyle.Padding.Vertical;
            int preferredHeight = 0, preferredWidth = 0;
            // Assuming here that all radio button states use the same size.
            Size radioButtonGlyphSize = RadioButtonRenderer.GetGlyphSize(graphics, RadioButtonState.CheckedNormal);

            if (freeDimension != DataGridViewRadioButtonFreeDimension.Width)
            {
                preferredHeight = Math.Min(this.Items.Count, this.MaxDisplayedItems) * (Math.Max(cellStyle.Font.Height, radioButtonGlyphSize.Height) + DATAGRIDVIEWRADIOBUTTONCELL_margin) + DATAGRIDVIEWRADIOBUTTONCELL_margin;
                preferredHeight += 2 * DATAGRIDVIEWRADIOBUTTONCELL_margin + borderAndPaddingHeights;
            }
           
            if (freeDimension != DataGridViewRadioButtonFreeDimension.Height)
            {
                TextFormatFlags flags = TextFormatFlags.Top | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPrefix;

                if (this.Items.Count > 0)
                {
                    // Figure out the width of the longest entry
                    int maxPreferredItemWidth = -1, preferredItemWidth;
                    foreach (object item in this.Items)
                    {
                        string formattedValue = GetFormattedValue(GetItemValue(item), rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize) as string;
                        if (formattedValue != null)
                        {
                            preferredItemWidth = DataGridViewCell.MeasureTextSize(graphics, formattedValue, cellStyle.Font, flags).Width;
                        }
                        else
                        {
                            preferredItemWidth = DataGridViewCell.MeasureTextSize(graphics, " ", cellStyle.Font, flags).Width;
                        }
                        if (preferredItemWidth > maxPreferredItemWidth)
                        {
                            maxPreferredItemWidth = preferredItemWidth;
                        }
                    }
                    preferredWidth = maxPreferredItemWidth;
                }

                if (freeDimension == DataGridViewRadioButtonFreeDimension.Width)
                {
                    Size contentSize = new Size(Int32.MaxValue, constraintSize.Height - borderAndPaddingHeights);
                    if (GetScrollingNeeded(graphics, rowIndex, cellStyle, contentSize))
                    {
                        // Accommodate the scrolling buttons
                        preferredWidth += ScrollBarRenderer.GetSizeBoxSize(graphics, ScrollBarState.Normal).Width;
                    }
                }

                preferredWidth += radioButtonGlyphSize.Width + 5 * DATAGRIDVIEWRADIOBUTTONCELL_margin + borderAndPaddingWidths;
            }

            if (this.DataGridView.ShowCellErrors)
            {
                // Making sure that there is enough room for the potential error icon
                if (freeDimension != DataGridViewRadioButtonFreeDimension.Height)
                {
                    preferredWidth = Math.Max(preferredWidth,
                                              borderAndPaddingWidths + DATAGRIDVIEWRADIOBUTTONCELL_iconMarginWidth * 2 + DATAGRIDVIEWRADIOBUTTONCELL_iconsWidth);
                }
                if (freeDimension != DataGridViewRadioButtonFreeDimension.Width)
                {
                    preferredHeight = Math.Max(preferredHeight,
                                               borderAndPaddingHeights + DATAGRIDVIEWRADIOBUTTONCELL_iconMarginHeight * 2 + DATAGRIDVIEWRADIOBUTTONCELL_iconsHeight);
                }
            }

            return new Size(preferredWidth, preferredHeight);
        }

        /// <summary>
        /// Helper function that determines if scrolling buttons should be displayed
        /// </summary>
        private bool GetScrollingNeeded(Graphics graphics, int rowIndex, DataGridViewCellStyle cellStyle, Size contentSize)
        {
            if (this.Items.Count <= 1)
            {
                return false;
            }

            if (this.MaxDisplayedItems >= this.Items.Count && 
                this.Items.Count * (cellStyle.Font.Height + DATAGRIDVIEWRADIOBUTTONCELL_margin) + DATAGRIDVIEWRADIOBUTTONCELL_margin <= contentSize.Height /*- borderAndPaddingHeights*/)
            {
                // There is enough vertical room to display all the radio buttons
                return false;
            }

            // Is there enough room to display the scroll buttons?
            Size sizeBoxSize = ScrollBarRenderer.GetSizeBoxSize(graphics, ScrollBarState.Normal);
            if (2 * DATAGRIDVIEWRADIOBUTTONCELL_margin + sizeBoxSize.Width > contentSize.Width ||
                2 * sizeBoxSize.Height > contentSize.Height)
            {
                // There isn't enough room to show the scroll buttons.
                return false;
            }

            // Scroll buttons are required and there is enough room for them.
            return true;
        }

        /// <summary>
        /// Helper function that sets the displayMemberProperty member based on the DataSource and the provided displayMember field name
        /// </summary>
        private void InitializeDisplayMemberPropertyDescriptor(string displayMember)
        {
            if (this.DataManager != null)
            {
                if (String.IsNullOrEmpty(displayMember))
                {
                    this.displayMemberProperty = null;
                }
                else
                {
                    BindingMemberInfo displayBindingMember = new BindingMemberInfo(displayMember);
                    // make the DataManager point to the sublist inside this.DataSource
                    this.DataManager = this.DataGridView.BindingContext[this.DataSource, displayBindingMember.BindingPath] as CurrencyManager;

                    PropertyDescriptorCollection props = this.DataManager.GetItemProperties();
                    PropertyDescriptor displayMemberProperty = props.Find(displayBindingMember.BindingField, true);
                    if (displayMemberProperty == null)
                    {
                        throw new ArgumentException("Field called '" + displayMember + "' does not exist.");
                    }
                    else
                    {
                        this.displayMemberProperty = displayMemberProperty;
                    }
                }
            }
        }

        /// <summary>
        /// Helper function that sets the valueMemberProperty member based on the DataSource and the provided valueMember field name
        /// </summary>
        private void InitializeValueMemberPropertyDescriptor(string valueMember)
        {
            if (this.DataManager != null)
            {
                if (String.IsNullOrEmpty(valueMember))
                {
                    this.valueMemberProperty = null;
                }
                else
                {
                    BindingMemberInfo valueBindingMember = new BindingMemberInfo(valueMember);
                    // make the DataManager point to the sublist inside this.DataSource
                    this.DataManager = this.DataGridView.BindingContext[this.DataSource, valueBindingMember.BindingPath] as CurrencyManager;

                    PropertyDescriptorCollection props = this.DataManager.GetItemProperties();
                    PropertyDescriptor valueMemberProperty = props.Find(valueBindingMember.BindingField, true);
                    if (valueMemberProperty == null)
                    {
                        throw new ArgumentException("Field called '" + valueMember + "' does not exist.");
                    }
                    else
                    {
                        this.valueMemberProperty = valueMemberProperty;
                    }
                }
            }
        }

        /// <summary>
        /// Helper function that invalidates the entire area of an entry
        /// </summary>
        private void InvalidateItem(int itemIndex, int rowIndex)
        {
            if (itemIndex >= this.layout.FirstDisplayedItemIndex &&
                itemIndex < this.layout.FirstDisplayedItemIndex + this.layout.DisplayedItemsCount)
            {
                DataGridViewCellStyle cellStyle = GetInheritedStyle(null, rowIndex, false /* includeColors */);
                Point radioButtonLocation = this.layout.FirstDisplayedItemLocation;
                int textHeight = cellStyle.Font.Height;
                radioButtonLocation.Y += (textHeight + DATAGRIDVIEWRADIOBUTTONCELL_margin) * (itemIndex - this.layout.FirstDisplayedItemIndex);
                Size cellSize = GetSize(rowIndex);
                this.DataGridView.Invalidate(new Rectangle(radioButtonLocation.X, radioButtonLocation.Y, cellSize.Width, Math.Max(textHeight + DATAGRIDVIEWRADIOBUTTONCELL_margin, this.layout.RadioButtonsSize.Height)));
            }
        }

        /// <summary>
        /// Helper function that invalidates the glyph section of an entry
        /// </summary>
        private void InvalidateRadioGlyph(int itemIndex, DataGridViewCellStyle cellStyle)
        {
            if (itemIndex >= this.layout.FirstDisplayedItemIndex &&
                itemIndex < this.layout.FirstDisplayedItemIndex + this.layout.DisplayedItemsCount)
            {
                Point radioButtonLocation = this.layout.FirstDisplayedItemLocation;
                int textHeight = cellStyle.Font.Height;
                radioButtonLocation.Y += (textHeight + DATAGRIDVIEWRADIOBUTTONCELL_margin) * (itemIndex - this.layout.FirstDisplayedItemIndex);
                this.DataGridView.Invalidate(new Rectangle(radioButtonLocation, this.layout.RadioButtonsSize));
            }
        }

        /// <summary>
        /// Returns whether calling the OnKeyDown method would force the owning row to be unshared.
        /// </summary>
        protected override bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex)
        {
            if (!e.Alt && !e.Control && !e.Shift)
            {
                if (this.handledKeyDown)
                {
                    return true;
                }
                if (e.KeyCode == Keys.Down && this.focusedItemIndex < this.Items.Count - 1)
                {
                    return true;
                }
                else if (e.KeyCode == Keys.Up && this.focusedItemIndex > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether calling the OnKeyUp method would force the owning row to be unshared.
        /// </summary>
        protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
        {
            if (!e.Alt && !e.Control && !e.Shift)
            {
                if (e.KeyCode == Keys.Down && this.focusedItemIndex < this.Items.Count - 1 && this.handledKeyDown)
                {
                    return true;
                }
                else if (e.KeyCode == Keys.Up && this.focusedItemIndex > 0 && this.handledKeyDown)
                {
                    return true;
                }
            }
            if (this.handledKeyDown)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether calling the OnMouseDown method would force the owning row to be unshared.
        /// </summary>
        protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
        {
            if (this.DataGridView == null)
            {
                return false;
            }
            if (e.Button == MouseButtons.Left)
            {
                int mouseLocationCode = GetMouseLocationCode(this.DataGridView.CreateGraphics(),
                                                             e.RowIndex,
                                                             GetInheritedStyle(null, e.RowIndex, false /* includeColors */),
                                                             e.X,
                                                             e.Y);
                switch (mouseLocationCode)
                {
                    case DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric:
                        break;
                    case DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationBottomScrollButton:
                        if (this.layout.FirstDisplayedItemIndex + this.layout.DisplayedItemsCount < this.Items.Count)
                        {
                            return true;
                        }
                        break;
                    case DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationTopScrollButton:
                        if (this.layout.FirstDisplayedItemIndex > 0)
                        {
                            return true;
                        }
                        break;
                    default:
                        if (this.pressedItemIndex != mouseLocationCode + this.layout.FirstDisplayedItemIndex)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether calling the OnMouseLeave method would force the owning row to be unshared.
        /// </summary>
        protected override bool MouseLeaveUnsharesRow(int rowIndex)
        {
            return this.pressedItemIndex != -1 && !this.mouseUpHooked;
        }

        /// <summary>
        /// Returns whether calling the OnMouseUp method would force the owning row to be unshared.
        /// </summary>
        protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
        {
             return e.Button == MouseButtons.Left && this.pressedItemIndex != -1;
        }

        /// <summary>
        /// Method that declares the cell dirty and notifies the grid of the value change.
        /// </summary>
        private void NotifyDataGridViewOfValueChange()
        {
            this.valueChanged = true;
            Debug.Assert(this.DataGridView != null);
            this.DataGridView.NotifyCurrentCellDirty(true);
        }

        /// <summary>
        /// Potentially updates the selected item and notifies the grid of the change.
        /// </summary>
        protected override void OnContentClick(DataGridViewCellEventArgs e)
        {
            if (this.DataGridView == null)
            {
                return;
            }
            Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
            if (ptCurrentCell.X == this.ColumnIndex &&
                ptCurrentCell.Y == e.RowIndex &&
                this.DataGridView.IsCurrentCellInEditMode)
            {
                if (mouseLocationCode >= 0 && 
                    UpdateFormattedValue(this.layout.FirstDisplayedItemIndex + mouseLocationCode, e.RowIndex))
                {
                    NotifyDataGridViewOfValueChange();
                }
            }
        }

        /// <summary>
        /// Updates the property descriptors when the cell gets attached to the grid.
        /// </summary>
        protected override void OnDataGridViewChanged()
        {
            if (this.DataGridView != null)
            {
                // Will throw an error if DataGridView is set and a member is invalid
                InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
                InitializeValueMemberPropertyDescriptor(this.ValueMember);
            }
            base.OnDataGridViewChanged();
        }

        /// <summary>
        /// Makes sure that there is a focused item when the cell becomes the current one.
        /// </summary>
        protected override void OnEnter(int rowIndex, bool throughMouseClick)
        {
            if (this.focusedItemIndex == -1)
            {
                this.focusedItemIndex = this.layout.FirstDisplayedItemIndex;
            }
            base.OnEnter(rowIndex, throughMouseClick);
        }

        /// <summary>
        /// Handles the KeyDown notification when it can result in a value change.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e, int rowIndex)
        {
            if (this.DataGridView == null)
            {
                return;
            }
            if (!e.Alt && !e.Control && !e.Shift)
            {
                if (this.handledKeyDown)
                {
                    this.handledKeyDown = false;
                }
                if (e.KeyCode == Keys.Down && this.focusedItemIndex < this.Items.Count - 1)
                {
                    this.handledKeyDown = true;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Up && this.focusedItemIndex > 0)
                {
                    this.handledKeyDown = true;
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles the KeyUp notification to change the cell's value.
        /// </summary>
        protected override void OnKeyUp(KeyEventArgs e, int rowIndex)
        {
            if (this.DataGridView == null)
            {
                return;
            }
            if (!e.Alt && !e.Control && !e.Shift && this.handledKeyDown)
            {
                if (e.KeyCode == Keys.Down && this.focusedItemIndex < this.Items.Count - 1)
                {
                    // Handle the Down Arrow key
                    if (UpdateFormattedValue(this.focusedItemIndex+1, rowIndex))
                    {
                        NotifyDataGridViewOfValueChange();
                    }
                    else if (this.selectedItemIndex == this.focusedItemIndex+1)
                    {
                        this.focusedItemIndex++;
                    }
                    if (this.focusedItemIndex >= this.layout.FirstDisplayedItemIndex + this.layout.TotallyDisplayedItemsCount)
                    {
                        this.layout.FirstDisplayedItemIndex++;
                    }
                    while (this.focusedItemIndex < this.layout.FirstDisplayedItemIndex)
                    {
                        this.layout.FirstDisplayedItemIndex--;
                    }
                    this.DataGridView.InvalidateCell(this.ColumnIndex, rowIndex);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Up && this.focusedItemIndex > 0)
                {
                    // Handle the Up Arrow key
                    if (UpdateFormattedValue(this.focusedItemIndex - 1, rowIndex))
                    {
                        NotifyDataGridViewOfValueChange();
                    }
                    else if (this.selectedItemIndex == this.focusedItemIndex - 1)
                    {
                        this.focusedItemIndex--;
                    }
                    if (this.focusedItemIndex < this.layout.FirstDisplayedItemIndex)
                    {
                        this.layout.FirstDisplayedItemIndex--;
                    }
                    while (this.focusedItemIndex >= this.layout.FirstDisplayedItemIndex + this.layout.TotallyDisplayedItemsCount)
                    {
                        this.layout.FirstDisplayedItemIndex++;
                    }
                    this.DataGridView.InvalidateCell(this.ColumnIndex, rowIndex);
                    e.Handled = true;
                }
            }
            // Always reset the flag that indicates if the KeyDown was handled.
            if (this.handledKeyDown)
            {
                this.handledKeyDown = false;
            }
        }

        /// <summary>
        /// Custom implementation of the MouseDown notification to update the cell's value or scroll the entries.
        /// </summary>
        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (this.DataGridView == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                int mouseLocationCode = GetMouseLocationCode(this.DataGridView.CreateGraphics(),
                                                             e.RowIndex,
                                                             GetInheritedStyle(null, e.RowIndex, false /* includeColors */),
                                                             e.X,
                                                             e.Y);
                switch (mouseLocationCode)
                {
                    case DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric:
                        break;
                    case DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationBottomScrollButton:
                        if (this.layout.FirstDisplayedItemIndex + this.layout.TotallyDisplayedItemsCount < this.Items.Count)
                        {
                            // Scroll the entries down.
                            this.layout.FirstDisplayedItemIndex++;
                            this.DataGridView.Invalidate(new Rectangle(this.layout.DownButtonLocation, this.layout.ScrollButtonsSize));
                        }
                        break;
                    case DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationTopScrollButton:
                        if (this.layout.FirstDisplayedItemIndex > 0)
                        {
                            // Scroll the entries up.
                            this.layout.FirstDisplayedItemIndex--;
                            this.DataGridView.Invalidate(new Rectangle(this.layout.UpButtonLocation, this.layout.ScrollButtonsSize));
                        }
                        break;
                    default:
                        if (this.pressedItemIndex != mouseLocationCode + this.layout.FirstDisplayedItemIndex)
                        {
                            // Update the value of the cell.
                            InvalidateItem(this.pressedItemIndex, e.RowIndex);
                            this.pressedItemIndex = mouseLocationCode + this.layout.FirstDisplayedItemIndex;
                            InvalidateItem(this.pressedItemIndex, e.RowIndex);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Makes sure the radio button gets hot when the mouse gets over it
        /// </summary>
        protected override void OnMouseEnter(int rowIndex)
        {
            if (this.DataGridView == null)
            {
                return;
            }

            if (this.pressedItemIndex != -1)
            {
                InvalidateRadioGlyph(this.pressedItemIndex, GetInheritedStyle(null, rowIndex, false /* includeColors */));
            }
        }

        /// <summary>
        /// Invalidates part of the cell as needed
        /// </summary>
        protected override void OnMouseLeave(int rowIndex)
        {
            if (this.DataGridView == null)
            {
                return;
            }

            int oldMouseLocationCode = mouseLocationCode;
            if (oldMouseLocationCode != DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric)
            {
                mouseLocationCode = DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric;
                if (oldMouseLocationCode == DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationTopScrollButton && this.layout.FirstDisplayedItemIndex > 0)
                {
                    this.DataGridView.Invalidate(new Rectangle(this.layout.UpButtonLocation, this.layout.ScrollButtonsSize));
                }
                else if (oldMouseLocationCode == DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationBottomScrollButton && this.layout.FirstDisplayedItemIndex + this.layout.DisplayedItemsCount < this.Items.Count)
                {
                    this.DataGridView.Invalidate(new Rectangle(this.layout.DownButtonLocation, this.layout.ScrollButtonsSize));
                }
                else if (oldMouseLocationCode >= 0)
                {
                    InvalidateRadioGlyph(oldMouseLocationCode + this.layout.FirstDisplayedItemIndex, GetInheritedStyle(null, rowIndex, false /* includeColors */));
                }
            }

            if (this.pressedItemIndex != -1)
            {
                if (!this.mouseUpHooked)
                {
                    // Hookup the grid's MouseUp event so that this.pressedItemIndex can be reset when the user releases the mouse button.
                    this.DataGridView.MouseUp += new MouseEventHandler(DataGridView_MouseUp);
                    this.mouseUpHooked = true;
                }
                InvalidateRadioGlyph(this.pressedItemIndex, GetInheritedStyle(null, rowIndex, false /* includeColors */));
            }
        }

        /// <summary>
        /// Invalidates part of the cell as needed
        /// </summary>
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            if (this.DataGridView == null)
            {
                return;
            }

            DataGridViewCellStyle cellStyle = GetInheritedStyle(null, e.RowIndex, false /* includeColors */);
            int oldMouseLocationCode = mouseLocationCode;
            mouseLocationCode = GetMouseLocationCode(this.DataGridView.CreateGraphics(), 
                                                     e.RowIndex, 
                                                     cellStyle, 
                                                     e.X, 
                                                     e.Y);
            if (oldMouseLocationCode != mouseLocationCode)
            {
                if ((oldMouseLocationCode == DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationTopScrollButton || mouseLocationCode == DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationTopScrollButton) && this.layout.FirstDisplayedItemIndex > 0)
                {
                    this.DataGridView.Invalidate(new Rectangle(this.layout.UpButtonLocation, this.layout.ScrollButtonsSize));
                }
                else if ((oldMouseLocationCode == DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationBottomScrollButton || mouseLocationCode == DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationBottomScrollButton) && this.layout.FirstDisplayedItemIndex + this.layout.DisplayedItemsCount < this.Items.Count)
                {
                    this.DataGridView.Invalidate(new Rectangle(this.layout.DownButtonLocation, this.layout.ScrollButtonsSize));
                }
                else
                {
                    if ((this.DataGridView.Rows.SharedRow(e.RowIndex).Cells[e.ColumnIndex].GetInheritedState(e.RowIndex) & DataGridViewElementStates.ReadOnly) != 0)
                    {
                        return;
                    }
                    if (oldMouseLocationCode >= 0)
                    {
                        InvalidateRadioGlyph(oldMouseLocationCode + this.layout.FirstDisplayedItemIndex, cellStyle);
                    }
                    if (mouseLocationCode >= 0)
                    {
                        InvalidateRadioGlyph(mouseLocationCode + this.layout.FirstDisplayedItemIndex, cellStyle);
                    }
                }
            }
        }

        /// <summary>
        /// Invalidates the potential pressed radio button. 
        /// </summary>
        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (this.DataGridView == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Left && this.pressedItemIndex != -1)
            {
                InvalidateItem(this.pressedItemIndex, e.RowIndex);
                this.pressedItemIndex = -1;
            }
        }

        /// <summary>
        /// Paints the entire cell.
        /// </summary>
        protected override void Paint(Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates cellState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            ComputeLayout(graphics,
                          clipBounds,
                          cellBounds,
                          rowIndex,
                          cellState,
                          formattedValue,
                          errorText,
                          cellStyle,
                          advancedBorderStyle,
                          paintParts,
                          true  /*paint*/);
        }

        /// <summary>
        /// Paints a single item.
        /// </summary>
        private void PaintItem(Graphics graphics, 
                               Rectangle radiosBounds, 
                               int rowIndex,
                               int itemIndex, 
                               DataGridViewCellStyle cellStyle,
                               bool itemReadOnly,
                               bool itemSelected,
                               bool mouseOverCell,
                               bool paintFocus)
        {
            object itemFormattedValue = GetFormattedValue(GetItemValue(this.Items[itemIndex]),
                                                    rowIndex,
                                                    ref cellStyle,
                                                    null /*valueTypeConverter*/,
                                                    null /*formattedValueTypeConverter*/,
                                                    DataGridViewDataErrorContexts.Display);
            string itemFormattedText = itemFormattedValue as string;
            if (string.IsNullOrEmpty(itemFormattedText))
            {
                return;
            }
            else
            {
                //Paint the glyph & caption
                Point glyphLocation = new Point(radiosBounds.Left + DATAGRIDVIEWRADIOBUTTONCELL_margin, radiosBounds.Top + DATAGRIDVIEWRADIOBUTTONCELL_margin);
                TextFormatFlags flags = TextFormatFlags.Top | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPrefix;
                Rectangle textBounds = new Rectangle(radiosBounds.Left + 2 * DATAGRIDVIEWRADIOBUTTONCELL_margin + this.layout.RadioButtonsSize.Width, radiosBounds.Top + DATAGRIDVIEWRADIOBUTTONCELL_margin, radiosBounds.Width - (2 * DATAGRIDVIEWRADIOBUTTONCELL_margin + this.layout.RadioButtonsSize.Width), cellStyle.Font.Height + 1 /*radiosBounds.Height - 2 * DATAGRIDVIEWRADIOBUTTONCELL_margin*/);
                int localMouseLocationCode = mouseOverCell ? mouseLocationCode : DATAGRIDVIEWRADIOBUTTONCELL_mouseLocationGeneric;
                using (Region clipRegion = graphics.Clip)
                {
                    graphics.SetClip(radiosBounds);
                    RadioButtonState radioButtonState;
                    if (itemSelected)
                    {
                        if (itemReadOnly)
                        {
                            radioButtonState = RadioButtonState.CheckedDisabled;
                        }
                        else
                        {
                            if (mouseOverCell && this.pressedItemIndex == itemIndex)
                            {
                                if (localMouseLocationCode + this.layout.FirstDisplayedItemIndex == this.pressedItemIndex)
                                {
                                    radioButtonState = RadioButtonState.CheckedPressed;
                                }
                                else
                                {
                                    radioButtonState = RadioButtonState.CheckedHot;
                                }
                            }
                            else
                            {
                                if (localMouseLocationCode + this.layout.FirstDisplayedItemIndex == itemIndex && this.pressedItemIndex == -1)
                                {
                                    radioButtonState = RadioButtonState.CheckedHot;
                                }
                                else
                                {
                                    radioButtonState = RadioButtonState.CheckedNormal;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (itemReadOnly)
                        {
                            radioButtonState = RadioButtonState.UncheckedDisabled;
                        }
                        else
                        {
                            if (mouseOverCell && this.pressedItemIndex == itemIndex)
                            {
                                if (localMouseLocationCode + this.layout.FirstDisplayedItemIndex == this.pressedItemIndex)
                                {
                                    radioButtonState = RadioButtonState.UncheckedPressed;
                                }
                                else
                                {
                                    radioButtonState = RadioButtonState.UncheckedHot;
                                }
                            }
                            else
                            {
                                if (localMouseLocationCode + this.layout.FirstDisplayedItemIndex == itemIndex && this.pressedItemIndex == -1)
                                {
                                    radioButtonState = RadioButtonState.UncheckedHot;
                                }
                                else
                                {
                                    radioButtonState = RadioButtonState.UncheckedNormal;
                                }
                            }
                        }
                    }
                    // Note: The cell should only show the focus rectangle when this.DataGridView.ShowFocusCues is true. However that property is
                    // protected and can't be accessed directly. A custom grid derived from DataGridView could expose this notion publicly.
                    RadioButtonRenderer.DrawRadioButton(graphics, 
                                                        glyphLocation, 
                                                        textBounds, 
                                                        itemFormattedText, 
                                                        cellStyle.Font, 
                                                        flags, 
                                                        paintFocus && /* this.DataGridView.ShowFocusCues && */ this.DataGridView.Focused,
                                                        radioButtonState);
                    graphics.Clip = clipRegion;
                } 
            }
        }

        /// <summary>
        /// Helper function that indicates if a paintPart needs to be painted.
        /// </summary>
        private static bool PartPainted(DataGridViewPaintParts paintParts, DataGridViewPaintParts paintPart)
        {
            return (paintParts & paintPart) != 0;
        }

        /// <summary>
        /// Custom implementation that follows the standard representation of cell types.
        /// </summary>
        public override string ToString()
        {
            return "DataGridViewRadioButtonCell { ColumnIndex=" + this.ColumnIndex.ToString(CultureInfo.CurrentCulture) + ", RowIndex=" + this.RowIndex.ToString(CultureInfo.CurrentCulture) + " }";
        }

        /// <summary>
        /// Returns true if the provided item successfully became the selected item.
        /// </summary>
        private bool UpdateFormattedValue(int newSelectedItemIndex, int rowIndex)
        {
            if (this.FormattedValueType == null || newSelectedItemIndex == this.selectedItemIndex)
            {
                return false;
            }
            IDataGridViewEditingCell editingCell = (IDataGridViewEditingCell)this;
            Debug.Assert(newSelectedItemIndex >= 0);
            Debug.Assert(newSelectedItemIndex < this.Items.Count);
            object item = this.Items[newSelectedItemIndex];
            object displayValue = GetItemDisplayValue(item);
            if (this.FormattedValueType.IsAssignableFrom(displayValue.GetType()))
            {
                editingCell.EditingCellFormattedValue = displayValue;
                this.focusedItemIndex = this.selectedItemIndex;
                this.DataGridView.InvalidateCell(this.ColumnIndex, rowIndex);
            }
            return true;
        }
    }
}
