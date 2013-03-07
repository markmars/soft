using System;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace MarkMars.UI
{
    public class PropertyGridEx : PropertyGrid
    {
        private Int32 splitterWidth = 180;

        public PropertyGridEx() : base() 
        { 
        }

        public void SetParent(Form form)
        {
            // Catch null arguments
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }

            // Set this property to intercept all events
            form.KeyPreview = true;

            // Listen for keydown event
            form.KeyDown += new KeyEventHandler(this.Form_KeyDown);
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Exit if cursor not in control
            if (!this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
            {
                return;
            }

            if (!this.Visible)
            {
                return;
            }

            // Handle tab key
            if (e.KeyCode != Keys.Tab && e.KeyCode != Keys.Enter
                && e.KeyCode != Keys.Up && e.KeyCode != Keys.Down) 
            { 
                return; 
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            
            // Get selected griditem
            GridItem gridItem = this.SelectedGridItem;
            if (gridItem == null) 
            { 
                return; 
            }

            // Create a collection all visible child griditems in propertygrid
            GridItem root = gridItem;
            while (root.GridItemType != GridItemType.Root)
            {
                root = root.Parent;
            }

            List<GridItem> gridItems = new List<GridItem>();
            this.FindItems(root, gridItems);

            // Get position of selected griditem in collection
            int index = gridItems.IndexOf(gridItem);

            if (e.KeyCode == Keys.Up)
            {
                if (index < 0)
                {
                    return;
                }
                else if (index == 0)
                {
                    index = gridItems.Count;
                }

                // Select next griditem in collection
                this.SelectedGridItem = gridItems[--index];                
            }
            else
            {
                if (index == gridItems.Count - 1)
                {
                    index = -1;
                }

                // Select next griditem in collection
                if (index < gridItems.Count)
                {
                    this.SelectedGridItem = gridItems[++index];
                }
            }
        }

        private void FindItems(GridItem item, List<GridItem> gridItems)
        {
            switch (item.GridItemType)
            {
                case GridItemType.Root:
                case GridItemType.Category:
                    foreach (GridItem i in item.GridItems)
                    {
                        this.FindItems(i, gridItems);
                    }
                    break;
                case GridItemType.Property:
                    gridItems.Add(item);
                    if (item.Expanded)
                    {
                        foreach (GridItem i in item.GridItems)
                        {
                            this.FindItems(i, gridItems);
                        }
                    }
                    break;
                case GridItemType.ArrayValue:
                    break;
            }
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            //Int32 width = 180;
            Control propertyGridView = this.Controls[2];
            Type propertyGridViewType = propertyGridView.GetType();

            propertyGridViewType.InvokeMember("MoveSplitterTo", 
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, 
                null, propertyGridView, new object[] { this.splitterWidth });

            base.OnLayout(e);
        }

        // Chenx+ 2011-07-12 设置属性是否可见。
        public void SetPropertyVisibility(object obj, string propertyName, bool visible)
        {
            Type type = typeof(System.ComponentModel.BrowsableAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;            
            FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
            fld.SetValue(attrs[type], visible);
        }

        // Chenx+ 2011-07-12 设置属性是否只读。
        public void SetPropertyReadOnly(object obj, string propertyName, bool readOnly)
        {
            Type type = typeof(System.ComponentModel.ReadOnlyAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("isReadOnly", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);
            fld.SetValue(attrs[type], readOnly);
        }

        [BrowsableAttribute(true), DefaultValue(180), Description("分隔线靠近左边的宽度")]
        public Int32 SplitterWidth
        {
            set
            {
                this.splitterWidth = value;
            }
        }
    }
}
