namespace InfoTips
{
    partial class UserControl日历
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeList财经日历 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn5 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn6 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn7 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn8 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn9 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn10 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControl区域 = new DevExpress.XtraEditors.PanelControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem2 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem3 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem4 = new DevExpress.XtraBars.BarCheckItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barCheckItem5 = new DevExpress.XtraBars.BarCheckItem();
            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList财经日历)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl区域)).BeginInit();
            this.panelControl区域.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.ContextMenuStrip = this.contextMenuStrip1;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(2, 2);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(1091, 302);
            this.webBrowser.TabIndex = 10;
            this.webBrowser.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // treeList财经日历
            // 
            this.treeList财经日历.Appearance.CustomizationFormHint.BackColor = System.Drawing.Color.White;
            this.treeList财经日历.Appearance.CustomizationFormHint.Options.UseBackColor = true;
            this.treeList财经日历.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.treeList财经日历.Appearance.OddRow.Options.UseBackColor = true;
            this.treeList财经日历.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList财经日历.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn5,
            this.treeListColumn6,
            this.treeListColumn4,
            this.treeListColumn7,
            this.treeListColumn8,
            this.treeListColumn9,
            this.treeListColumn10});
            this.treeList财经日历.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList财经日历.Location = new System.Drawing.Point(2, 2);
            this.treeList财经日历.Name = "treeList财经日历";
            this.treeList财经日历.OptionsBehavior.Editable = false;
            this.treeList财经日历.OptionsLayout.AddNewColumns = false;
            this.treeList财经日历.OptionsMenu.EnableColumnMenu = false;
            this.treeList财经日历.OptionsMenu.EnableFooterMenu = false;
            this.treeList财经日历.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeList财经日历.OptionsView.EnableAppearanceOddRow = true;
            this.treeList财经日历.OptionsView.ShowFocusedFrame = false;
            this.treeList财经日历.OptionsView.ShowIndentAsRowStyle = true;
            this.treeList财经日历.OptionsView.ShowIndicator = false;
            this.treeList财经日历.OptionsView.ShowRoot = false;
            this.treeList财经日历.RowHeight = 21;
            this.treeList财经日历.Size = new System.Drawing.Size(1091, 302);
            this.treeList财经日历.TabIndex = 9;
            this.treeList财经日历.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.Light;
            this.treeList财经日历.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.treeList财经日历_CustomDrawNodeCell);
            // 
            // treeListColumn5
            // 
            this.treeListColumn5.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeListColumn5.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.treeListColumn5.AppearanceCell.Options.UseFont = true;
            this.treeListColumn5.AppearanceCell.Options.UseForeColor = true;
            this.treeListColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn5.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn5.Caption = "时间";
            this.treeListColumn5.FieldName = "时间";
            this.treeListColumn5.Name = "treeListColumn5";
            this.treeListColumn5.OptionsColumn.AllowEdit = false;
            this.treeListColumn5.OptionsColumn.AllowMove = false;
            this.treeListColumn5.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn5.OptionsColumn.ReadOnly = true;
            this.treeListColumn5.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.treeListColumn5.Visible = true;
            this.treeListColumn5.VisibleIndex = 0;
            // 
            // treeListColumn6
            // 
            this.treeListColumn6.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeListColumn6.AppearanceCell.Options.UseFont = true;
            this.treeListColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn6.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn6.Caption = "国家";
            this.treeListColumn6.FieldName = "国家";
            this.treeListColumn6.Name = "treeListColumn6";
            this.treeListColumn6.OptionsColumn.AllowEdit = false;
            this.treeListColumn6.OptionsColumn.AllowMove = false;
            this.treeListColumn6.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn6.OptionsColumn.AllowSort = false;
            this.treeListColumn6.OptionsColumn.ReadOnly = true;
            this.treeListColumn6.Width = 100;
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeListColumn4.AppearanceCell.Options.UseFont = true;
            this.treeListColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn4.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn4.Caption = "标题";
            this.treeListColumn4.FieldName = "标题";
            this.treeListColumn4.Name = "treeListColumn4";
            this.treeListColumn4.OptionsColumn.AllowEdit = false;
            this.treeListColumn4.OptionsColumn.AllowMove = false;
            this.treeListColumn4.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn4.OptionsColumn.AllowSort = false;
            this.treeListColumn4.OptionsColumn.ReadOnly = true;
            this.treeListColumn4.Visible = true;
            this.treeListColumn4.VisibleIndex = 1;
            this.treeListColumn4.Width = 200;
            // 
            // treeListColumn7
            // 
            this.treeListColumn7.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeListColumn7.AppearanceCell.Options.UseFont = true;
            this.treeListColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn7.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn7.Caption = "重要性";
            this.treeListColumn7.FieldName = "重要性";
            this.treeListColumn7.Name = "treeListColumn7";
            this.treeListColumn7.OptionsColumn.AllowEdit = false;
            this.treeListColumn7.OptionsColumn.AllowMove = false;
            this.treeListColumn7.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn7.OptionsColumn.ReadOnly = true;
            this.treeListColumn7.Visible = true;
            this.treeListColumn7.VisibleIndex = 2;
            this.treeListColumn7.Width = 100;
            // 
            // treeListColumn8
            // 
            this.treeListColumn8.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeListColumn8.AppearanceCell.Options.UseFont = true;
            this.treeListColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn8.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn8.Caption = "前值";
            this.treeListColumn8.FieldName = "前值";
            this.treeListColumn8.Name = "treeListColumn8";
            this.treeListColumn8.OptionsColumn.AllowEdit = false;
            this.treeListColumn8.OptionsColumn.AllowMove = false;
            this.treeListColumn8.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn8.OptionsColumn.AllowSort = false;
            this.treeListColumn8.OptionsColumn.ReadOnly = true;
            this.treeListColumn8.Visible = true;
            this.treeListColumn8.VisibleIndex = 3;
            // 
            // treeListColumn9
            // 
            this.treeListColumn9.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeListColumn9.AppearanceCell.Options.UseFont = true;
            this.treeListColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn9.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn9.Caption = "预测值";
            this.treeListColumn9.FieldName = "预测值";
            this.treeListColumn9.Name = "treeListColumn9";
            this.treeListColumn9.OptionsColumn.AllowEdit = false;
            this.treeListColumn9.OptionsColumn.AllowMove = false;
            this.treeListColumn9.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn9.OptionsColumn.AllowSort = false;
            this.treeListColumn9.OptionsColumn.ReadOnly = true;
            this.treeListColumn9.Visible = true;
            this.treeListColumn9.VisibleIndex = 4;
            // 
            // treeListColumn10
            // 
            this.treeListColumn10.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeListColumn10.AppearanceCell.Options.UseFont = true;
            this.treeListColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn10.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn10.Caption = "结果";
            this.treeListColumn10.FieldName = "结果";
            this.treeListColumn10.Name = "treeListColumn10";
            this.treeListColumn10.OptionsColumn.AllowEdit = false;
            this.treeListColumn10.OptionsColumn.AllowMove = false;
            this.treeListColumn10.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn10.OptionsColumn.AllowSort = false;
            this.treeListColumn10.OptionsColumn.ReadOnly = true;
            this.treeListColumn10.Visible = true;
            this.treeListColumn10.VisibleIndex = 5;
            this.treeListColumn10.Width = 150;
            // 
            // panelControl区域
            // 
            this.panelControl区域.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelControl区域.Appearance.Options.UseBackColor = true;
            this.panelControl区域.Controls.Add(this.treeList财经日历);
            this.panelControl区域.Controls.Add(this.webBrowser);
            this.panelControl区域.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl区域.Location = new System.Drawing.Point(0, 42);
            this.panelControl区域.Name = "panelControl区域";
            this.panelControl区域.Size = new System.Drawing.Size(1095, 306);
            this.panelControl区域.TabIndex = 11;
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
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barSubItem1,
            this.barCheckItem1,
            this.barCheckItem2,
            this.barCheckItem3,
            this.barCheckItem4,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barButtonItem8,
            this.barButtonItem9,
            this.barCheckItem5,
            this.barButtonItem10});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 21;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "星期一";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Tag = "0";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日期_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "星期二";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.Tag = "1";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日期_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "星期三";
            this.barButtonItem3.Id = 3;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.Tag = "2";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日期_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "星期四";
            this.barButtonItem4.Id = 4;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.Tag = "3";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日期_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "星期五";
            this.barButtonItem5.Id = 5;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.Tag = "4";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日期_ItemClick);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "日历来源";
            this.barSubItem1.Glyph = global::InfoTips.Properties.Resources.cjlr1;
            this.barSubItem1.Id = 6;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem4)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "INVESTING";
            this.barCheckItem1.Checked = true;
            this.barCheckItem1.Id = 7;
            this.barCheckItem1.Name = "barCheckItem1";
            this.barCheckItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日历来源_ItemClick);
            // 
            // barCheckItem2
            // 
            this.barCheckItem2.Caption = "DAILYFX中文";
            this.barCheckItem2.Checked = true;
            this.barCheckItem2.Id = 8;
            this.barCheckItem2.Name = "barCheckItem2";
            this.barCheckItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日历来源_ItemClick);
            // 
            // barCheckItem3
            // 
            this.barCheckItem3.Caption = "FX168日历";
            this.barCheckItem3.Checked = true;
            this.barCheckItem3.Id = 9;
            this.barCheckItem3.Name = "barCheckItem3";
            this.barCheckItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日历来源_ItemClick);
            // 
            // barCheckItem4
            // 
            this.barCheckItem4.Caption = "FX168周历";
            this.barCheckItem4.Checked = true;
            this.barCheckItem4.Id = 10;
            this.barCheckItem4.Name = "barCheckItem4";
            this.barCheckItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日历来源_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "INVESTING";
            this.barButtonItem6.Id = 15;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem财经日历_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "DAILYFX中文";
            this.barButtonItem7.Id = 16;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem财经日历_ItemClick);
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "FX168日历";
            this.barButtonItem8.Id = 17;
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem财经日历_ItemClick);
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "FX168周历";
            this.barButtonItem9.Id = 18;
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem财经日历_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1095, 42);
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
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 42);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 306);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1095, 42);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 306);
            // 
            // barCheckItem5
            // 
            this.barCheckItem5.Caption = "DAILYFX英文";
            this.barCheckItem5.Checked = true;
            this.barCheckItem5.Id = 19;
            this.barCheckItem5.Name = "barCheckItem5";
            this.barCheckItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem日历来源_ItemClick);
            // 
            // barButtonItem10
            // 
            this.barButtonItem10.Caption = "DAILYFX英文";
            this.barButtonItem10.Id = 20;
            this.barButtonItem10.Name = "barButtonItem10";
            this.barButtonItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem财经日历_ItemClick);
            // 
            // UserControl日历
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelControl区域);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UserControl日历";
            this.Size = new System.Drawing.Size(1095, 348);
            this.Load += new System.EventHandler(this.UserControl日历_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList财经日历)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl区域)).EndInit();
            this.panelControl区域.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeList财经日历;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn5;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn6;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn7;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn8;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn9;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn10;
        private System.Windows.Forms.WebBrowser webBrowser;
        private DevExpress.XtraEditors.PanelControl panelControl区域;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem2;
        private DevExpress.XtraBars.BarCheckItem barCheckItem3;
        private DevExpress.XtraBars.BarCheckItem barCheckItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private DevExpress.XtraBars.BarCheckItem barCheckItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
    }
}
