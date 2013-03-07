namespace MarkMars.Common.ImportExcel
{
	partial class FormImportExcel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportExcel));
            this.m_tbFileName = new System.Windows.Forms.TextBox();
            this.m_btnBrowser = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonImportSelection = new System.Windows.Forms.RadioButton();
            this.radioButtonImportAll = new System.Windows.Forms.RadioButton();
            this.buttonSetColumn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridViewMap = new System.Windows.Forms.DataGridView();
            this.目标字段 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Excel字段 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMap)).BeginInit();
            this.SuspendLayout();
            // 
            // m_tbFileName
            // 
            this.m_tbFileName.Location = new System.Drawing.Point(6, 30);
            this.m_tbFileName.Name = "m_tbFileName";
            this.m_tbFileName.ReadOnly = true;
            this.m_tbFileName.Size = new System.Drawing.Size(788, 21);
            this.m_tbFileName.TabIndex = 0;
            // 
            // m_btnBrowser
            // 
            this.m_btnBrowser.Location = new System.Drawing.Point(800, 28);
            this.m_btnBrowser.Name = "m_btnBrowser";
            this.m_btnBrowser.Size = new System.Drawing.Size(61, 21);
            this.m_btnBrowser.TabIndex = 2;
            this.m_btnBrowser.Text = "浏览...";
            this.m_btnBrowser.UseVisualStyleBackColor = true;
            this.m_btnBrowser.Click += new System.EventHandler(this.m_btnBrowser_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_btnBrowser);
            this.groupBox1.Controls.Add(this.m_tbFileName);
            this.groupBox1.Location = new System.Drawing.Point(15, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(991, 70);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "第一步: 选择excel文件";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonImportSelection);
            this.groupBox2.Controls.Add(this.radioButtonImportAll);
            this.groupBox2.Controls.Add(this.buttonSetColumn);
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Location = new System.Drawing.Point(15, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(991, 342);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "第二步: 选择表和字段名";
            // 
            // radioButtonImportSelection
            // 
            this.radioButtonImportSelection.AutoSize = true;
            this.radioButtonImportSelection.Location = new System.Drawing.Point(825, 161);
            this.radioButtonImportSelection.Name = "radioButtonImportSelection";
            this.radioButtonImportSelection.Size = new System.Drawing.Size(95, 16);
            this.radioButtonImportSelection.TabIndex = 10;
            this.radioButtonImportSelection.Text = "导入选中的行";
            this.radioButtonImportSelection.UseVisualStyleBackColor = true;
            // 
            // radioButtonImportAll
            // 
            this.radioButtonImportAll.AutoSize = true;
            this.radioButtonImportAll.Checked = true;
            this.radioButtonImportAll.Location = new System.Drawing.Point(825, 130);
            this.radioButtonImportAll.Name = "radioButtonImportAll";
            this.radioButtonImportAll.Size = new System.Drawing.Size(155, 16);
            this.radioButtonImportAll.TabIndex = 9;
            this.radioButtonImportAll.TabStop = true;
            this.radioButtonImportAll.Text = "导入当前行下面的所有行";
            this.radioButtonImportAll.UseVisualStyleBackColor = true;
            // 
            // buttonSetColumn
            // 
            this.buttonSetColumn.Location = new System.Drawing.Point(825, 38);
            this.buttonSetColumn.Name = "buttonSetColumn";
            this.buttonSetColumn.Size = new System.Drawing.Size(156, 64);
            this.buttonSetColumn.TabIndex = 8;
            this.buttonSetColumn.Text = "当前行就是字段名";
            this.buttonSetColumn.UseVisualStyleBackColor = true;
            this.buttonSetColumn.Click += new System.EventHandler(this.buttonSetColumn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 18);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(813, 318);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(805, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(805, 292);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridViewMap);
            this.groupBox3.Location = new System.Drawing.Point(15, 434);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(351, 202);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "第三步: 字段映射";
            // 
            // dataGridViewMap
            // 
            this.dataGridViewMap.AllowUserToAddRows = false;
            this.dataGridViewMap.AllowUserToDeleteRows = false;
            this.dataGridViewMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.目标字段,
            this.Excel字段});
            this.dataGridViewMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMap.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewMap.Location = new System.Drawing.Point(3, 17);
            this.dataGridViewMap.MultiSelect = false;
            this.dataGridViewMap.Name = "dataGridViewMap";
            this.dataGridViewMap.ShowCellErrors = false;
            this.dataGridViewMap.ShowRowErrors = false;
            this.dataGridViewMap.Size = new System.Drawing.Size(345, 182);
            this.dataGridViewMap.TabIndex = 0;
            this.dataGridViewMap.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewMap_DataError);
            // 
            // 目标字段
            // 
            this.目标字段.HeaderText = "目标字段";
            this.目标字段.Name = "目标字段";
            this.目标字段.ReadOnly = true;
            // 
            // Excel字段
            // 
            this.Excel字段.HeaderText = "Excel字段";
            this.Excel字段.Name = "Excel字段";
            // 
            // m_btnOK
            // 
            this.m_btnOK.Location = new System.Drawing.Point(593, 522);
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.Size = new System.Drawing.Size(75, 21);
            this.m_btnOK.TabIndex = 6;
            this.m_btnOK.Text = "确定";
            this.m_btnOK.UseVisualStyleBackColor = true;
            this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_btnCancel.Location = new System.Drawing.Point(593, 576);
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(75, 21);
            this.m_btnCancel.TabIndex = 7;
            this.m_btnCancel.Text = "取消";
            this.m_btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormImportExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 647);
            this.Controls.Add(this.m_btnCancel);
            this.Controls.Add(this.m_btnOK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImportExcel";
            this.Text = "导入Excel文件";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMap)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox m_tbFileName;
		private System.Windows.Forms.Button m_btnBrowser;
		private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button m_btnOK;
		private System.Windows.Forms.Button m_btnCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RadioButton radioButtonImportSelection;
        private System.Windows.Forms.RadioButton radioButtonImportAll;
        private System.Windows.Forms.Button buttonSetColumn;
        private System.Windows.Forms.DataGridView dataGridViewMap;
        private System.Windows.Forms.DataGridViewTextBoxColumn 目标字段;
        private System.Windows.Forms.DataGridViewComboBoxColumn Excel字段;
	}
}