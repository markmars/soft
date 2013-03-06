namespace InfoTips
{
    partial class UserControl评论
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
            this.treeList评论中心 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControl区域 = new DevExpress.XtraEditors.PanelControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem2 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem3 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem4 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem5 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem6 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem7 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem8 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem9 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem10 = new DevExpress.XtraBars.BarCheckItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.treeList评论中心)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl区域)).BeginInit();
            this.panelControl区域.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeList评论中心
            // 
            this.treeList评论中心.Appearance.CustomizationFormHint.BackColor = System.Drawing.Color.White;
            this.treeList评论中心.Appearance.CustomizationFormHint.Options.UseBackColor = true;
            this.treeList评论中心.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.treeList评论中心.Appearance.OddRow.Options.UseBackColor = true;
            this.treeList评论中心.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList评论中心.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2,
            this.treeListColumn3});
            this.treeList评论中心.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList评论中心.Location = new System.Drawing.Point(2, 2);
            this.treeList评论中心.Name = "treeList评论中心";
            this.treeList评论中心.OptionsBehavior.Editable = false;
            this.treeList评论中心.OptionsLayout.AddNewColumns = false;
            this.treeList评论中心.OptionsMenu.EnableColumnMenu = false;
            this.treeList评论中心.OptionsMenu.EnableFooterMenu = false;
            this.treeList评论中心.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeList评论中心.OptionsView.EnableAppearanceOddRow = true;
            this.treeList评论中心.OptionsView.ShowFocusedFrame = false;
            this.treeList评论中心.OptionsView.ShowIndicator = false;
            this.treeList评论中心.OptionsView.ShowRoot = false;
            this.treeList评论中心.RowHeight = 21;
            this.treeList评论中心.Size = new System.Drawing.Size(1091, 320);
            this.treeList评论中心.TabIndex = 5;
            this.treeList评论中心.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.Light;
            this.treeList评论中心.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.treeList评论中心_CustomDrawNodeCell);
            this.treeList评论中心.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeList评论中心_MouseDoubleClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn1.Caption = "时间";
            this.treeListColumn1.FieldName = "时间";
            this.treeListColumn1.MinWidth = 50;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowSort = false;
            this.treeListColumn1.OptionsColumn.ReadOnly = true;
            this.treeListColumn1.SortOrder = System.Windows.Forms.SortOrder.Descending;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 50;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.treeListColumn2.AppearanceCell.Options.UseForeColor = true;
            this.treeListColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn2.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn2.Caption = "标题";
            this.treeListColumn2.FieldName = "标题";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.OptionsColumn.AllowEdit = false;
            this.treeListColumn2.OptionsColumn.AllowMove = false;
            this.treeListColumn2.OptionsColumn.AllowMoveToCustomizationForm = false;
            this.treeListColumn2.OptionsColumn.AllowSort = false;
            this.treeListColumn2.OptionsColumn.ReadOnly = true;
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 1;
            this.treeListColumn2.Width = 450;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn3.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.treeListColumn3.Caption = "类型";
            this.treeListColumn3.FieldName = "类型";
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.OptionsColumn.AllowSort = false;
            this.treeListColumn3.Visible = true;
            this.treeListColumn3.VisibleIndex = 2;
            // 
            // panelControl区域
            // 
            this.panelControl区域.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelControl区域.Appearance.Options.UseBackColor = true;
            this.panelControl区域.Controls.Add(this.treeList评论中心);
            this.panelControl区域.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl区域.Location = new System.Drawing.Point(0, 24);
            this.panelControl区域.Name = "panelControl区域";
            this.panelControl区域.Size = new System.Drawing.Size(1095, 324);
            this.panelControl区域.TabIndex = 7;
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
            this.barCheckItem3,
            this.barCheckItem4,
            this.barCheckItem5,
            this.barCheckItem6,
            this.barCheckItem7,
            this.barCheckItem8,
            this.barCheckItem9,
            this.barCheckItem10});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 10;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem4),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem8),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem9),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem10)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "个人汇评";
            this.barCheckItem1.Checked = true;
            this.barCheckItem1.Description = "1";
            this.barCheckItem1.Id = 0;
            this.barCheckItem1.Name = "barCheckItem1";
            this.barCheckItem1.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=35";
            this.barCheckItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem2
            // 
            this.barCheckItem2.Caption = "机构汇评";
            this.barCheckItem2.Checked = true;
            this.barCheckItem2.Description = "1";
            this.barCheckItem2.Id = 1;
            this.barCheckItem2.Name = "barCheckItem2";
            this.barCheckItem2.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=36";
            this.barCheckItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem3
            // 
            this.barCheckItem3.Caption = "银行汇评";
            this.barCheckItem3.Checked = true;
            this.barCheckItem3.Description = "1";
            this.barCheckItem3.Id = 2;
            this.barCheckItem3.Name = "barCheckItem3";
            this.barCheckItem3.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=37";
            this.barCheckItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem4
            // 
            this.barCheckItem4.Caption = "其他汇评";
            this.barCheckItem4.Checked = true;
            this.barCheckItem4.Description = "1";
            this.barCheckItem4.Id = 3;
            this.barCheckItem4.Name = "barCheckItem4";
            this.barCheckItem4.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=38";
            this.barCheckItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem5
            // 
            this.barCheckItem5.Caption = "KITCO";
            this.barCheckItem5.Checked = true;
            this.barCheckItem5.Description = "1";
            this.barCheckItem5.Id = 4;
            this.barCheckItem5.Name = "barCheckItem5";
            this.barCheckItem5.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=30";
            this.barCheckItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem6
            // 
            this.barCheckItem6.Caption = "天交所";
            this.barCheckItem6.Checked = true;
            this.barCheckItem6.Description = "1";
            this.barCheckItem6.Id = 5;
            this.barCheckItem6.Name = "barCheckItem6";
            this.barCheckItem6.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=31";
            this.barCheckItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem7
            // 
            this.barCheckItem7.Caption = "中外银行";
            this.barCheckItem7.Checked = true;
            this.barCheckItem7.Description = "1";
            this.barCheckItem7.Id = 6;
            this.barCheckItem7.Name = "barCheckItem7";
            this.barCheckItem7.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=32";
            this.barCheckItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem8
            // 
            this.barCheckItem8.Caption = "投资机构";
            this.barCheckItem8.Checked = true;
            this.barCheckItem8.Description = "1";
            this.barCheckItem8.Id = 7;
            this.barCheckItem8.Name = "barCheckItem8";
            this.barCheckItem8.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=33";
            this.barCheckItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem9
            // 
            this.barCheckItem9.Caption = "上海黄金交易所";
            this.barCheckItem9.Checked = true;
            this.barCheckItem9.Description = "1";
            this.barCheckItem9.Id = 8;
            this.barCheckItem9.Name = "barCheckItem9";
            this.barCheckItem9.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=34";
            this.barCheckItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
            // 
            // barCheckItem10
            // 
            this.barCheckItem10.Caption = "原油评论";
            this.barCheckItem10.Checked = true;
            this.barCheckItem10.Description = "1";
            this.barCheckItem10.Id = 9;
            this.barCheckItem10.Name = "barCheckItem10";
            this.barCheckItem10.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=29";
            this.barCheckItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem评论中心_ItemClick);
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
            // UserControl评论
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelControl区域);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UserControl评论";
            this.Size = new System.Drawing.Size(1095, 348);
            this.Load += new System.EventHandler(this.UserControl评论_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeList评论中心)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl区域)).EndInit();
            this.panelControl区域.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeList评论中心;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DevExpress.XtraEditors.PanelControl panelControl区域;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem2;
        private DevExpress.XtraBars.BarCheckItem barCheckItem3;
        private DevExpress.XtraBars.BarCheckItem barCheckItem4;
        private DevExpress.XtraBars.BarCheckItem barCheckItem5;
        private DevExpress.XtraBars.BarCheckItem barCheckItem6;
        private DevExpress.XtraBars.BarCheckItem barCheckItem7;
        private DevExpress.XtraBars.BarCheckItem barCheckItem8;
        private DevExpress.XtraBars.BarCheckItem barCheckItem9;
        private DevExpress.XtraBars.BarCheckItem barCheckItem10;
    }
}
