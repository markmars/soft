namespace InfoTips
{
    partial class FormSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetting));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_自动启动 = new System.Windows.Forms.CheckBox();
            this.checkBox_语音提醒 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox语音文件 = new System.Windows.Forms.ComboBox();
            this.comboBox弹窗时间 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.名称 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.时间 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.time = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.采集 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.caiji = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.checkBox_自动弹窗 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.caiji)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_自动启动);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本设置";
            // 
            // checkBox_自动启动
            // 
            this.checkBox_自动启动.AutoSize = true;
            this.checkBox_自动启动.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_自动启动.ForeColor = System.Drawing.Color.Firebrick;
            this.checkBox_自动启动.Location = new System.Drawing.Point(12, 30);
            this.checkBox_自动启动.Name = "checkBox_自动启动";
            this.checkBox_自动启动.Size = new System.Drawing.Size(116, 18);
            this.checkBox_自动启动.TabIndex = 0;
            this.checkBox_自动启动.Text = "开机自动启动";
            this.checkBox_自动启动.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_自动启动.UseVisualStyleBackColor = true;
            // 
            // checkBox_语音提醒
            // 
            this.checkBox_语音提醒.AutoSize = true;
            this.checkBox_语音提醒.Location = new System.Drawing.Point(162, 20);
            this.checkBox_语音提醒.Name = "checkBox_语音提醒";
            this.checkBox_语音提醒.Size = new System.Drawing.Size(72, 16);
            this.checkBox_语音提醒.TabIndex = 5;
            this.checkBox_语音提醒.Text = "语音提醒";
            this.checkBox_语音提醒.UseVisualStyleBackColor = true;
            this.checkBox_语音提醒.CheckedChanged += new System.EventHandler(this.checkBox_语音提醒_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(165, 316);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox语音文件);
            this.groupBox2.Controls.Add(this.comboBox弹窗时间);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.checkBox_自动弹窗);
            this.groupBox2.Controls.Add(this.checkBox_语音提醒);
            this.groupBox2.Location = new System.Drawing.Point(0, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 239);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "提醒设置";
            // 
            // comboBox语音文件
            // 
            this.comboBox语音文件.FormattingEnabled = true;
            this.comboBox语音文件.Items.AddRange(new object[] {
            "Band.wav",
            "News.wav",
            "Notify.wav"});
            this.comboBox语音文件.Location = new System.Drawing.Point(162, 68);
            this.comboBox语音文件.Name = "comboBox语音文件";
            this.comboBox语音文件.Size = new System.Drawing.Size(126, 20);
            this.comboBox语音文件.TabIndex = 18;
            // 
            // comboBox弹窗时间
            // 
            this.comboBox弹窗时间.FormattingEnabled = true;
            this.comboBox弹窗时间.Items.AddRange(new object[] {
            "5秒",
            "10秒",
            "15秒"});
            this.comboBox弹窗时间.Location = new System.Drawing.Point(162, 42);
            this.comboBox弹窗时间.Name = "comboBox弹窗时间";
            this.comboBox弹窗时间.Size = new System.Drawing.Size(126, 20);
            this.comboBox弹窗时间.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "语音提醒文件：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "即时资讯信息窗口显示：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.treeList1);
            this.groupBox3.Location = new System.Drawing.Point(12, 94);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(382, 139);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.名称,
            this.时间,
            this.采集});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(3, 17);
            this.treeList1.Name = "treeList1";
            this.treeList1.BeginUnboundLoad();
            this.treeList1.AppendNode(new object[] {
            "指标",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "原油",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "头条",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "世界灾难",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "经济争端",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "讲话",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "黄金",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "国家政治",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "国际争端",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "播报",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "-reuters-",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "-fxstreet-",
            "15秒",
            true}, -1);
            this.treeList1.AppendNode(new object[] {
            "-DAILYFX-",
            "15秒",
            true}, -1);
            this.treeList1.EndUnboundLoad();
            this.treeList1.OptionsBehavior.PopulateServiceColumns = true;
            this.treeList1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.caiji,
            this.time});
            this.treeList1.Size = new System.Drawing.Size(376, 119);
            this.treeList1.TabIndex = 0;
            // 
            // 名称
            // 
            this.名称.Caption = "名称";
            this.名称.FieldName = "名称";
            this.名称.MinWidth = 34;
            this.名称.Name = "名称";
            this.名称.OptionsColumn.AllowEdit = false;
            this.名称.OptionsColumn.AllowMove = false;
            this.名称.OptionsColumn.AllowSize = false;
            this.名称.OptionsColumn.AllowSort = false;
            this.名称.OptionsColumn.ReadOnly = true;
            this.名称.SortOrder = System.Windows.Forms.SortOrder.Descending;
            this.名称.Visible = true;
            this.名称.VisibleIndex = 0;
            // 
            // 时间
            // 
            this.时间.Caption = "时间（秒）";
            this.时间.ColumnEdit = this.time;
            this.时间.FieldName = "时间";
            this.时间.Name = "时间";
            this.时间.OptionsColumn.AllowMove = false;
            this.时间.SortOrder = System.Windows.Forms.SortOrder.Descending;
            this.时间.Visible = true;
            this.时间.VisibleIndex = 1;
            // 
            // time
            // 
            this.time.AutoHeight = false;
            this.time.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.time.Items.AddRange(new object[] {
            "5秒",
            "10秒",
            "15秒",
            "30秒",
            "1分钟",
            "2分钟",
            "3分钟",
            "5分钟"});
            this.time.Name = "time";
            this.time.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // 采集
            // 
            this.采集.Caption = "采集";
            this.采集.ColumnEdit = this.caiji;
            this.采集.FieldName = "采集";
            this.采集.Name = "采集";
            this.采集.Visible = true;
            this.采集.VisibleIndex = 2;
            // 
            // caiji
            // 
            this.caiji.AutoHeight = false;
            this.caiji.Name = "caiji";
            this.caiji.Tag = true;
            // 
            // checkBox_自动弹窗
            // 
            this.checkBox_自动弹窗.AutoSize = true;
            this.checkBox_自动弹窗.Location = new System.Drawing.Point(12, 20);
            this.checkBox_自动弹窗.Name = "checkBox_自动弹窗";
            this.checkBox_自动弹窗.Size = new System.Drawing.Size(144, 16);
            this.checkBox_自动弹窗.TabIndex = 5;
            this.checkBox_自动弹窗.Text = "即时资讯信息自动弹窗";
            this.checkBox_自动弹窗.UseVisualStyleBackColor = true;
            this.checkBox_自动弹窗.CheckedChanged += new System.EventHandler(this.checkBox_自动弹窗_CheckedChanged);
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 351);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.FormSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.caiji)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_自动启动;
        private System.Windows.Forms.CheckBox checkBox_语音提醒;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_自动弹窗;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox弹窗时间;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox语音文件;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit caiji;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 名称;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 时间;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 采集;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox time;
    }
}