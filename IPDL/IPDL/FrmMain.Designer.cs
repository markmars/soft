namespace IPDL
{
    partial class FrmMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_删除资源 = new System.Windows.Forms.Button();
            this.btn_修改资源 = new System.Windows.Forms.Button();
            this.btn_添加资源 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_修改选中 = new System.Windows.Forms.Button();
            this.btn_添加代理 = new System.Windows.Forms.Button();
            this.btn_验证选中 = new System.Windows.Forms.Button();
            this.btn_下载代理资源 = new System.Windows.Forms.Button();
            this.btn_导出选定 = new System.Windows.Forms.Button();
            this.btn_粘贴 = new System.Windows.Forms.Button();
            this.btn_清理 = new System.Windows.Forms.Button();
            this.btn_导入 = new System.Windows.Forms.Button();
            this.btn_删除 = new System.Windows.Forms.Button();
            this.btn_设置 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_代理状态 = new System.Windows.Forms.Label();
            this.btn_设置代理 = new System.Windows.Forms.Button();
            this.btn_取消代理 = new System.Windows.Forms.Button();
            this.btn_验证全部 = new System.Windows.Forms.Button();
            this.listView_Source = new System.Windows.Forms.ListView();
            this.名称 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.网页地址 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView_Info = new System.Windows.Forms.ListView();
            this.类型 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.服务器地址 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.端口 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.时间 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.备注 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_删除资源);
            this.groupBox1.Controls.Add(this.btn_修改资源);
            this.groupBox1.Controls.Add(this.btn_添加资源);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_修改选中);
            this.groupBox1.Controls.Add(this.btn_添加代理);
            this.groupBox1.Controls.Add(this.btn_验证选中);
            this.groupBox1.Controls.Add(this.btn_下载代理资源);
            this.groupBox1.Controls.Add(this.btn_导出选定);
            this.groupBox1.Controls.Add(this.btn_粘贴);
            this.groupBox1.Controls.Add(this.btn_清理);
            this.groupBox1.Controls.Add(this.btn_导入);
            this.groupBox1.Controls.Add(this.btn_删除);
            this.groupBox1.Controls.Add(this.btn_设置);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label_代理状态);
            this.groupBox1.Controls.Add(this.btn_设置代理);
            this.groupBox1.Controls.Add(this.btn_取消代理);
            this.groupBox1.Controls.Add(this.btn_验证全部);
            this.groupBox1.Controls.Add(this.listView_Source);
            this.groupBox1.Controls.Add(this.listView_Info);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(659, 445);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // btn_删除资源
            // 
            this.btn_删除资源.Location = new System.Drawing.Point(579, 394);
            this.btn_删除资源.Name = "btn_删除资源";
            this.btn_删除资源.Size = new System.Drawing.Size(75, 23);
            this.btn_删除资源.TabIndex = 13;
            this.btn_删除资源.Text = "删除资源";
            this.btn_删除资源.UseVisualStyleBackColor = true;
            this.btn_删除资源.Click += new System.EventHandler(this.btn_删除资源_Click);
            // 
            // btn_修改资源
            // 
            this.btn_修改资源.Location = new System.Drawing.Point(579, 365);
            this.btn_修改资源.Name = "btn_修改资源";
            this.btn_修改资源.Size = new System.Drawing.Size(75, 23);
            this.btn_修改资源.TabIndex = 13;
            this.btn_修改资源.Text = "修改资源";
            this.btn_修改资源.UseVisualStyleBackColor = true;
            this.btn_修改资源.Click += new System.EventHandler(this.btn_修改资源_Click);
            // 
            // btn_添加资源
            // 
            this.btn_添加资源.Location = new System.Drawing.Point(578, 336);
            this.btn_添加资源.Name = "btn_添加资源";
            this.btn_添加资源.Size = new System.Drawing.Size(75, 23);
            this.btn_添加资源.TabIndex = 13;
            this.btn_添加资源.Text = "添加资源";
            this.btn_添加资源.UseVisualStyleBackColor = true;
            this.btn_添加资源.Click += new System.EventHandler(this.btn_添加资源_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "获取代理网址：";
            // 
            // btn_修改选中
            // 
            this.btn_修改选中.Location = new System.Drawing.Point(578, 187);
            this.btn_修改选中.Name = "btn_修改选中";
            this.btn_修改选中.Size = new System.Drawing.Size(75, 23);
            this.btn_修改选中.TabIndex = 11;
            this.btn_修改选中.Text = "修改选中";
            this.btn_修改选中.UseVisualStyleBackColor = true;
            this.btn_修改选中.Click += new System.EventHandler(this.btn_修改选中_Click);
            // 
            // btn_添加代理
            // 
            this.btn_添加代理.Location = new System.Drawing.Point(578, 158);
            this.btn_添加代理.Name = "btn_添加代理";
            this.btn_添加代理.Size = new System.Drawing.Size(75, 23);
            this.btn_添加代理.TabIndex = 11;
            this.btn_添加代理.Text = "添加代理";
            this.btn_添加代理.UseVisualStyleBackColor = true;
            this.btn_添加代理.Click += new System.EventHandler(this.btn_添加代理_Click);
            // 
            // btn_验证选中
            // 
            this.btn_验证选中.Location = new System.Drawing.Point(578, 71);
            this.btn_验证选中.Name = "btn_验证选中";
            this.btn_验证选中.Size = new System.Drawing.Size(75, 23);
            this.btn_验证选中.TabIndex = 11;
            this.btn_验证选中.Text = "验证选中";
            this.btn_验证选中.UseVisualStyleBackColor = true;
            this.btn_验证选中.Click += new System.EventHandler(this.btn_验证选中_Click);
            // 
            // btn_下载代理资源
            // 
            this.btn_下载代理资源.Location = new System.Drawing.Point(443, 307);
            this.btn_下载代理资源.Name = "btn_下载代理资源";
            this.btn_下载代理资源.Size = new System.Drawing.Size(117, 23);
            this.btn_下载代理资源.TabIndex = 11;
            this.btn_下载代理资源.Text = "▲下载代理资源";
            this.btn_下载代理资源.UseVisualStyleBackColor = true;
            this.btn_下载代理资源.Click += new System.EventHandler(this.btn_下载代理资源_Click);
            // 
            // btn_导出选定
            // 
            this.btn_导出选定.Location = new System.Drawing.Point(579, 274);
            this.btn_导出选定.Name = "btn_导出选定";
            this.btn_导出选定.Size = new System.Drawing.Size(75, 23);
            this.btn_导出选定.TabIndex = 11;
            this.btn_导出选定.Text = "导出选定";
            this.btn_导出选定.UseVisualStyleBackColor = true;
            this.btn_导出选定.Click += new System.EventHandler(this.btn_导出选定_Click);
            // 
            // btn_粘贴
            // 
            this.btn_粘贴.Location = new System.Drawing.Point(616, 245);
            this.btn_粘贴.Name = "btn_粘贴";
            this.btn_粘贴.Size = new System.Drawing.Size(37, 23);
            this.btn_粘贴.TabIndex = 11;
            this.btn_粘贴.Text = "粘贴";
            this.btn_粘贴.UseVisualStyleBackColor = true;
            this.btn_粘贴.Click += new System.EventHandler(this.btn_粘贴_Click);
            // 
            // btn_清理
            // 
            this.btn_清理.Location = new System.Drawing.Point(616, 216);
            this.btn_清理.Name = "btn_清理";
            this.btn_清理.Size = new System.Drawing.Size(37, 23);
            this.btn_清理.TabIndex = 11;
            this.btn_清理.Text = "清理";
            this.btn_清理.UseVisualStyleBackColor = true;
            this.btn_清理.Click += new System.EventHandler(this.btn_清理_Click);
            // 
            // btn_导入
            // 
            this.btn_导入.Location = new System.Drawing.Point(578, 245);
            this.btn_导入.Name = "btn_导入";
            this.btn_导入.Size = new System.Drawing.Size(37, 23);
            this.btn_导入.TabIndex = 11;
            this.btn_导入.Text = "导入";
            this.btn_导入.UseVisualStyleBackColor = true;
            this.btn_导入.Click += new System.EventHandler(this.btn_导入_Click);
            // 
            // btn_删除
            // 
            this.btn_删除.Location = new System.Drawing.Point(578, 216);
            this.btn_删除.Name = "btn_删除";
            this.btn_删除.Size = new System.Drawing.Size(37, 23);
            this.btn_删除.TabIndex = 11;
            this.btn_删除.Text = "删除";
            this.btn_删除.UseVisualStyleBackColor = true;
            this.btn_删除.Click += new System.EventHandler(this.btn_删除_Click);
            // 
            // btn_设置
            // 
            this.btn_设置.Location = new System.Drawing.Point(579, 12);
            this.btn_设置.Name = "btn_设置";
            this.btn_设置.Size = new System.Drawing.Size(75, 23);
            this.btn_设置.TabIndex = 10;
            this.btn_设置.Text = "【设置】";
            this.btn_设置.UseVisualStyleBackColor = true;
            this.btn_设置.Click += new System.EventHandler(this.btn_设置_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.Location = new System.Drawing.Point(77, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(483, 21);
            this.textBox1.TabIndex = 9;
            // 
            // label_代理状态
            // 
            this.label_代理状态.AutoSize = true;
            this.label_代理状态.Location = new System.Drawing.Point(6, 17);
            this.label_代理状态.Name = "label_代理状态";
            this.label_代理状态.Size = new System.Drawing.Size(65, 12);
            this.label_代理状态.TabIndex = 8;
            this.label_代理状态.Text = "代理状态：";
            // 
            // btn_设置代理
            // 
            this.btn_设置代理.Location = new System.Drawing.Point(578, 100);
            this.btn_设置代理.Name = "btn_设置代理";
            this.btn_设置代理.Size = new System.Drawing.Size(75, 23);
            this.btn_设置代理.TabIndex = 7;
            this.btn_设置代理.Text = "设置代理";
            this.btn_设置代理.UseVisualStyleBackColor = true;
            this.btn_设置代理.Click += new System.EventHandler(this.btn_设置代理_Click);
            // 
            // btn_取消代理
            // 
            this.btn_取消代理.Location = new System.Drawing.Point(579, 129);
            this.btn_取消代理.Name = "btn_取消代理";
            this.btn_取消代理.Size = new System.Drawing.Size(75, 23);
            this.btn_取消代理.TabIndex = 6;
            this.btn_取消代理.Text = "取消代理";
            this.btn_取消代理.UseVisualStyleBackColor = true;
            this.btn_取消代理.Click += new System.EventHandler(this.btn_取消代理_Click);
            // 
            // btn_验证全部
            // 
            this.btn_验证全部.Location = new System.Drawing.Point(578, 42);
            this.btn_验证全部.Name = "btn_验证全部";
            this.btn_验证全部.Size = new System.Drawing.Size(75, 23);
            this.btn_验证全部.TabIndex = 5;
            this.btn_验证全部.Text = "验证全部";
            this.btn_验证全部.UseVisualStyleBackColor = true;
            this.btn_验证全部.Click += new System.EventHandler(this.btn_验证全部_Click);
            // 
            // listView_Source
            // 
            this.listView_Source.BackColor = System.Drawing.SystemColors.Window;
            this.listView_Source.CheckBoxes = true;
            this.listView_Source.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.名称,
            this.网页地址});
            this.listView_Source.FullRowSelect = true;
            this.listView_Source.GridLines = true;
            this.listView_Source.Location = new System.Drawing.Point(8, 336);
            this.listView_Source.Name = "listView_Source";
            this.listView_Source.Size = new System.Drawing.Size(552, 103);
            this.listView_Source.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView_Source.TabIndex = 4;
            this.listView_Source.UseCompatibleStateImageBehavior = false;
            this.listView_Source.View = System.Windows.Forms.View.Details;
            this.listView_Source.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_Source_MouseDoubleClick);
            // 
            // 名称
            // 
            this.名称.Text = "名称";
            this.名称.Width = 200;
            // 
            // 网页地址
            // 
            this.网页地址.Text = "网页地址";
            this.网页地址.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.网页地址.Width = 345;
            // 
            // listView_Info
            // 
            this.listView_Info.BackColor = System.Drawing.SystemColors.Window;
            this.listView_Info.CheckBoxes = true;
            this.listView_Info.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.类型,
            this.服务器地址,
            this.端口,
            this.时间,
            this.备注});
            this.listView_Info.FullRowSelect = true;
            this.listView_Info.GridLines = true;
            this.listView_Info.Location = new System.Drawing.Point(8, 41);
            this.listView_Info.Name = "listView_Info";
            this.listView_Info.Size = new System.Drawing.Size(552, 256);
            this.listView_Info.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.listView_Info.TabIndex = 4;
            this.listView_Info.UseCompatibleStateImageBehavior = false;
            this.listView_Info.View = System.Windows.Forms.View.Details;
            this.listView_Info.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_Info_MouseDoubleClick);
            // 
            // 类型
            // 
            this.类型.Text = "[常用]类型";
            this.类型.Width = 90;
            // 
            // 服务器地址
            // 
            this.服务器地址.Text = "服务器地址";
            this.服务器地址.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.服务器地址.Width = 110;
            // 
            // 端口
            // 
            this.端口.Text = "端口";
            this.端口.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.端口.Width = 75;
            // 
            // 时间
            // 
            this.时间.Text = "时间状态(ms)";
            this.时间.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.时间.Width = 90;
            // 
            // 备注
            // 
            this.备注.Text = "-备注-";
            this.备注.Width = 170;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 452);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_设置代理;
        private System.Windows.Forms.Button btn_取消代理;
        private System.Windows.Forms.Button btn_验证全部;
        private System.Windows.Forms.ListView listView_Info;
        private System.Windows.Forms.ColumnHeader 服务器地址;
        private System.Windows.Forms.ColumnHeader 类型;
        private System.Windows.Forms.ColumnHeader 端口;
        private System.Windows.Forms.ColumnHeader 时间;
        private System.Windows.Forms.Label label_代理状态;
        private System.Windows.Forms.Button btn_设置;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_删除;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_修改选中;
        private System.Windows.Forms.Button btn_添加代理;
        private System.Windows.Forms.Button btn_验证选中;
        private System.Windows.Forms.Button btn_下载代理资源;
        private System.Windows.Forms.Button btn_导出选定;
        private System.Windows.Forms.Button btn_粘贴;
        private System.Windows.Forms.Button btn_清理;
        private System.Windows.Forms.Button btn_导入;
        private System.Windows.Forms.ListView listView_Source;
        private System.Windows.Forms.ColumnHeader 名称;
        private System.Windows.Forms.ColumnHeader 网页地址;
        private System.Windows.Forms.ColumnHeader 备注;
        private System.Windows.Forms.Button btn_删除资源;
        private System.Windows.Forms.Button btn_修改资源;
        private System.Windows.Forms.Button btn_添加资源;

    }
}

