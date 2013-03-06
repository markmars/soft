namespace InfoTips
{
    partial class UserControl新闻
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
            this.treeList新闻汇总 = new DevExpress.XtraTreeList.TreeList();
            this.时间 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.标题 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.类型 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
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
            this.barCheckItem11 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem12 = new DevExpress.XtraBars.BarCheckItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.treeList新闻汇总)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl区域)).BeginInit();
            this.panelControl区域.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeList新闻汇总
            // 
            this.treeList新闻汇总.Appearance.CustomizationFormHint.BackColor = System.Drawing.Color.White;
            this.treeList新闻汇总.Appearance.CustomizationFormHint.Options.UseBackColor = true;
            this.treeList新闻汇总.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.treeList新闻汇总.Appearance.OddRow.Options.UseBackColor = true;
            this.treeList新闻汇总.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList新闻汇总.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.时间,
            this.标题,
            this.类型});
            this.treeList新闻汇总.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList新闻汇总.Font = new System.Drawing.Font("Tahoma", 9F);
            this.treeList新闻汇总.HorzScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Never;
            this.treeList新闻汇总.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeList新闻汇总.Location = new System.Drawing.Point(2, 2);
            this.treeList新闻汇总.Name = "treeList新闻汇总";
            this.treeList新闻汇总.OptionsBehavior.Editable = false;
            this.treeList新闻汇总.OptionsLayout.AddNewColumns = false;
            this.treeList新闻汇总.OptionsMenu.EnableColumnMenu = false;
            this.treeList新闻汇总.OptionsMenu.EnableFooterMenu = false;
            this.treeList新闻汇总.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeList新闻汇总.OptionsView.EnableAppearanceOddRow = true;
            this.treeList新闻汇总.OptionsView.ShowFocusedFrame = false;
            this.treeList新闻汇总.OptionsView.ShowIndicator = false;
            this.treeList新闻汇总.OptionsView.ShowRoot = false;
            this.treeList新闻汇总.RowHeight = 21;
            this.treeList新闻汇总.Size = new System.Drawing.Size(1091, 320);
            this.treeList新闻汇总.TabIndex = 3;
            this.treeList新闻汇总.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.Light;
            this.treeList新闻汇总.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.treeList新闻汇总_CustomDrawNodeCell);
            this.treeList新闻汇总.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeList新闻汇总_MouseDoubleClick);
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
            // panelControl区域
            // 
            this.panelControl区域.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelControl区域.Appearance.Options.UseBackColor = true;
            this.panelControl区域.Controls.Add(this.treeList新闻汇总);
            this.panelControl区域.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl区域.Location = new System.Drawing.Point(0, 24);
            this.panelControl区域.Name = "panelControl区域";
            this.panelControl区域.Size = new System.Drawing.Size(1095, 324);
            this.panelControl区域.TabIndex = 6;
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
            this.barCheckItem10,
            this.barCheckItem11,
            this.barCheckItem12});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 16;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem10),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem11),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCheckItem12)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "头条";
            this.barCheckItem1.Checked = true;
            this.barCheckItem1.Description = "1";
            this.barCheckItem1.Id = 0;
            this.barCheckItem1.Name = "barCheckItem1";
            this.barCheckItem1.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=11";
            this.barCheckItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem2
            // 
            this.barCheckItem2.Caption = "指标";
            this.barCheckItem2.Checked = true;
            this.barCheckItem2.Description = "1";
            this.barCheckItem2.Id = 5;
            this.barCheckItem2.Name = "barCheckItem2";
            this.barCheckItem2.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=12";
            this.barCheckItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem3
            // 
            this.barCheckItem3.Caption = "讲话";
            this.barCheckItem3.Checked = true;
            this.barCheckItem3.Description = "1";
            this.barCheckItem3.Id = 6;
            this.barCheckItem3.Name = "barCheckItem3";
            this.barCheckItem3.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=13";
            this.barCheckItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem4
            // 
            this.barCheckItem4.Caption = "播报";
            this.barCheckItem4.Checked = true;
            this.barCheckItem4.Description = "1";
            this.barCheckItem4.Id = 7;
            this.barCheckItem4.Name = "barCheckItem4";
            this.barCheckItem4.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=14";
            this.barCheckItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem5
            // 
            this.barCheckItem5.Caption = "黄金";
            this.barCheckItem5.Checked = true;
            this.barCheckItem5.Description = "1";
            this.barCheckItem5.Id = 8;
            this.barCheckItem5.Name = "barCheckItem5";
            this.barCheckItem5.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=16";
            this.barCheckItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem6
            // 
            this.barCheckItem6.Caption = "原油";
            this.barCheckItem6.Checked = true;
            this.barCheckItem6.Description = "1";
            this.barCheckItem6.Id = 9;
            this.barCheckItem6.Name = "barCheckItem6";
            this.barCheckItem6.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=17";
            this.barCheckItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem7
            // 
            this.barCheckItem7.Caption = "国家政治";
            this.barCheckItem7.Checked = true;
            this.barCheckItem7.Description = "1";
            this.barCheckItem7.Id = 10;
            this.barCheckItem7.Name = "barCheckItem7";
            this.barCheckItem7.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=22";
            this.barCheckItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem8
            // 
            this.barCheckItem8.Caption = "国际争端";
            this.barCheckItem8.Checked = true;
            this.barCheckItem8.Description = "1";
            this.barCheckItem8.Id = 11;
            this.barCheckItem8.Name = "barCheckItem8";
            this.barCheckItem8.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=21";
            this.barCheckItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem9
            // 
            this.barCheckItem9.Caption = "经济争端";
            this.barCheckItem9.Checked = true;
            this.barCheckItem9.Description = "1";
            this.barCheckItem9.Id = 12;
            this.barCheckItem9.Name = "barCheckItem9";
            this.barCheckItem9.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=23";
            this.barCheckItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem10
            // 
            this.barCheckItem10.Caption = "世界灾难";
            this.barCheckItem10.Checked = true;
            this.barCheckItem10.Description = "1";
            this.barCheckItem10.Id = 13;
            this.barCheckItem10.Name = "barCheckItem10";
            this.barCheckItem10.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=24";
            this.barCheckItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem11
            // 
            this.barCheckItem11.Caption = "-reuters-";
            this.barCheckItem11.Checked = true;
            this.barCheckItem11.Description = "1";
            this.barCheckItem11.Id = 14;
            this.barCheckItem11.Name = "barCheckItem11";
            this.barCheckItem11.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=20";
            this.barCheckItem11.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
            // 
            // barCheckItem12
            // 
            this.barCheckItem12.Caption = "-fxstreet-";
            this.barCheckItem12.Checked = true;
            this.barCheckItem12.Description = "1";
            this.barCheckItem12.Id = 15;
            this.barCheckItem12.Name = "barCheckItem12";
            this.barCheckItem12.Tag = "http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=19";
            this.barCheckItem12.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem新闻_ItemClick);
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
            // UserControl新闻
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelControl区域);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UserControl新闻";
            this.Size = new System.Drawing.Size(1095, 348);
            this.Load += new System.EventHandler(this.UserControl新闻_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeList新闻汇总)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl区域)).EndInit();
            this.panelControl区域.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeList新闻汇总;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 时间;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 标题;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 类型;
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
        private DevExpress.XtraBars.BarCheckItem barCheckItem11;
        private DevExpress.XtraBars.BarCheckItem barCheckItem12;
    }
}
