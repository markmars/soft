namespace InfoTips
{
    partial class UserControl火线
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem2 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem3 = new DevExpress.XtraBars.BarCheckItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.treeList火线速递 = new DevExpress.XtraTreeList.TreeList();
            this.时间 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.标题 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.类型 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList火线速递)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barCheckItem1,
            this.barCheckItem2,
            this.barCheckItem3});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem3)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "dailyfx";
            this.barCheckItem1.Checked = true;
            this.barCheckItem1.Description = "1";
            this.barCheckItem1.Id = 0;
            this.barCheckItem1.Name = "barCheckItem1";
            this.barCheckItem1.Tag = "http://www.dailyfx.com.hk/livenews/index.html";
            this.barCheckItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem火线速递_ItemClick);
            // 
            // barCheckItem2
            // 
            this.barCheckItem2.Caption = "FX678";
            this.barCheckItem2.Checked = true;
            this.barCheckItem2.Description = "1";
            this.barCheckItem2.Id = 1;
            this.barCheckItem2.Name = "barCheckItem2";
            this.barCheckItem2.Tag = "http://www.fx678.com/news/flash/default.shtml";
            this.barCheckItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem火线速递_ItemClick);
            // 
            // barCheckItem3
            // 
            this.barCheckItem3.Caption = "FX168";
            this.barCheckItem3.Checked = true;
            this.barCheckItem3.Description = "1";
            this.barCheckItem3.Id = 2;
            this.barCheckItem3.Name = "barCheckItem3";
            this.barCheckItem3.Tag = "http://t.news.fx168.com/indexs.shtml";
            this.barCheckItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem火线速递_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1095, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 348);
            this.barDockControlBottom.Size = new System.Drawing.Size(1095, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 324);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1095, 24);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 324);
            // 
            // treeList火线速递
            // 
            this.treeList火线速递.Appearance.CustomizationFormHint.BackColor = System.Drawing.Color.White;
            this.treeList火线速递.Appearance.CustomizationFormHint.Options.UseBackColor = true;
            this.treeList火线速递.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.treeList火线速递.Appearance.OddRow.Options.UseBackColor = true;
            this.treeList火线速递.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList火线速递.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.时间,
            this.标题,
            this.类型});
            this.treeList火线速递.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList火线速递.Font = new System.Drawing.Font("Tahoma", 9F);
            this.treeList火线速递.HorzScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Never;
            this.treeList火线速递.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeList火线速递.Location = new System.Drawing.Point(0, 24);
            this.treeList火线速递.Name = "treeList火线速递";
            this.treeList火线速递.OptionsBehavior.Editable = false;
            this.treeList火线速递.OptionsLayout.AddNewColumns = false;
            this.treeList火线速递.OptionsMenu.EnableColumnMenu = false;
            this.treeList火线速递.OptionsMenu.EnableFooterMenu = false;
            this.treeList火线速递.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeList火线速递.OptionsView.EnableAppearanceOddRow = true;
            this.treeList火线速递.OptionsView.ShowFocusedFrame = false;
            this.treeList火线速递.OptionsView.ShowIndicator = false;
            this.treeList火线速递.OptionsView.ShowRoot = false;
            this.treeList火线速递.RowHeight = 21;
            this.treeList火线速递.Size = new System.Drawing.Size(1095, 324);
            this.treeList火线速递.TabIndex = 4;
            this.treeList火线速递.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.Light;
            this.treeList火线速递.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.treeList火线速递_CustomDrawNodeCell);
            // 
            // 时间
            // 
            this.时间.AppearanceHeader.Options.UseTextOptions = true;
            this.时间.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.时间.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.时间.Caption = "时间";
            this.时间.FieldName = "时间";
            this.时间.MinWidth = 50;
            this.时间.Name = "时间";
            this.时间.OptionsColumn.AllowSort = false;
            this.时间.OptionsColumn.ReadOnly = true;
            this.时间.SortOrder = System.Windows.Forms.SortOrder.Descending;
            this.时间.Visible = true;
            this.时间.VisibleIndex = 0;
            this.时间.Width = 50;
            // 
            // 标题
            // 
            this.标题.AppearanceCell.ForeColor = System.Drawing.Color.Blue;
            this.标题.AppearanceCell.Options.UseForeColor = true;
            this.标题.AppearanceHeader.Options.UseTextOptions = true;
            this.标题.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.标题.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.标题.Caption = "标题";
            this.标题.FieldName = "标题";
            this.标题.Name = "标题";
            this.标题.OptionsColumn.AllowEdit = false;
            this.标题.OptionsColumn.AllowMove = false;
            this.标题.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.标题.OptionsColumn.AllowSort = false;
            this.标题.OptionsColumn.ReadOnly = true;
            this.标题.Visible = true;
            this.标题.VisibleIndex = 1;
            this.标题.Width = 450;
            // 
            // 类型
            // 
            this.类型.AppearanceHeader.Options.UseTextOptions = true;
            this.类型.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.类型.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.类型.Caption = "类型";
            this.类型.FieldName = "类型";
            this.类型.Name = "类型";
            this.类型.OptionsColumn.AllowSort = false;
            this.类型.Visible = true;
            this.类型.VisibleIndex = 2;
            // 
            // UserControl火线
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.treeList火线速递);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UserControl火线";
            this.Size = new System.Drawing.Size(1095, 348);
            this.Load += new System.EventHandler(this.UserControl火线_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList火线速递)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraTreeList.TreeList treeList火线速递;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 时间;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 标题;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 类型;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem2;
        private DevExpress.XtraBars.BarCheckItem barCheckItem3;
    }
}
