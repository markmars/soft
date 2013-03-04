namespace IPDL
{
    partial class FrmSetting
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
            this.btn_导入 = new System.Windows.Forms.Button();
            this.btn_导出 = new System.Windows.Forms.Button();
            this.btn_删除验证 = new System.Windows.Forms.Button();
            this.btn_修改验证 = new System.Windows.Forms.Button();
            this.btn_添加验证 = new System.Windows.Forms.Button();
            this.listView_YanZhengZiYuan = new System.Windows.Forms.ListView();
            this.资源名称 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.验证资源网址 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txt_并发线程数目 = new System.Windows.Forms.TextBox();
            this.txt_验证超时时间 = new System.Windows.Forms.TextBox();
            this.txt_连接超时时间 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_导入);
            this.groupBox1.Controls.Add(this.btn_导出);
            this.groupBox1.Controls.Add(this.btn_删除验证);
            this.groupBox1.Controls.Add(this.btn_修改验证);
            this.groupBox1.Controls.Add(this.btn_添加验证);
            this.groupBox1.Controls.Add(this.listView_YanZhengZiYuan);
            this.groupBox1.Controls.Add(this.txt_并发线程数目);
            this.groupBox1.Controls.Add(this.txt_验证超时时间);
            this.groupBox1.Controls.Add(this.txt_连接超时时间);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(629, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "验证参数";
            // 
            // btn_导入
            // 
            this.btn_导入.Location = new System.Drawing.Point(548, 142);
            this.btn_导入.Name = "btn_导入";
            this.btn_导入.Size = new System.Drawing.Size(75, 23);
            this.btn_导入.TabIndex = 3;
            this.btn_导入.Text = "导入";
            this.btn_导入.UseVisualStyleBackColor = true;
            this.btn_导入.Click += new System.EventHandler(this.btn_导入_Click);
            // 
            // btn_导出
            // 
            this.btn_导出.Location = new System.Drawing.Point(548, 171);
            this.btn_导出.Name = "btn_导出";
            this.btn_导出.Size = new System.Drawing.Size(75, 23);
            this.btn_导出.TabIndex = 3;
            this.btn_导出.Text = "导出";
            this.btn_导出.UseVisualStyleBackColor = true;
            this.btn_导出.Click += new System.EventHandler(this.btn_导出_Click);
            // 
            // btn_删除验证
            // 
            this.btn_删除验证.Location = new System.Drawing.Point(548, 100);
            this.btn_删除验证.Name = "btn_删除验证";
            this.btn_删除验证.Size = new System.Drawing.Size(75, 23);
            this.btn_删除验证.TabIndex = 3;
            this.btn_删除验证.Text = "删除验证";
            this.btn_删除验证.UseVisualStyleBackColor = true;
            this.btn_删除验证.Click += new System.EventHandler(this.btn_删除验证_Click);
            // 
            // btn_修改验证
            // 
            this.btn_修改验证.Location = new System.Drawing.Point(548, 71);
            this.btn_修改验证.Name = "btn_修改验证";
            this.btn_修改验证.Size = new System.Drawing.Size(75, 23);
            this.btn_修改验证.TabIndex = 3;
            this.btn_修改验证.Text = "修改验证";
            this.btn_修改验证.UseVisualStyleBackColor = true;
            this.btn_修改验证.Click += new System.EventHandler(this.btn_修改验证_Click);
            // 
            // btn_添加验证
            // 
            this.btn_添加验证.Location = new System.Drawing.Point(548, 42);
            this.btn_添加验证.Name = "btn_添加验证";
            this.btn_添加验证.Size = new System.Drawing.Size(75, 23);
            this.btn_添加验证.TabIndex = 3;
            this.btn_添加验证.Text = "添加验证";
            this.btn_添加验证.UseVisualStyleBackColor = true;
            this.btn_添加验证.Click += new System.EventHandler(this.btn_添加验证_Click);
            // 
            // listView_YanZhengZiYuan
            // 
            this.listView_YanZhengZiYuan.AllowDrop = true;
            this.listView_YanZhengZiYuan.CheckBoxes = true;
            this.listView_YanZhengZiYuan.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.资源名称,
            this.验证资源网址});
            this.listView_YanZhengZiYuan.FullRowSelect = true;
            this.listView_YanZhengZiYuan.GridLines = true;
            this.listView_YanZhengZiYuan.Location = new System.Drawing.Point(17, 42);
            this.listView_YanZhengZiYuan.Name = "listView_YanZhengZiYuan";
            this.listView_YanZhengZiYuan.Size = new System.Drawing.Size(519, 152);
            this.listView_YanZhengZiYuan.TabIndex = 2;
            this.listView_YanZhengZiYuan.UseCompatibleStateImageBehavior = false;
            this.listView_YanZhengZiYuan.View = System.Windows.Forms.View.Details;
            // 
            // 资源名称
            // 
            this.资源名称.Text = "[验证]资源名称";
            this.资源名称.Width = 150;
            // 
            // 验证资源网址
            // 
            this.验证资源网址.Text = "验证资源网址";
            this.验证资源网址.Width = 360;
            // 
            // txt_并发线程数目
            // 
            this.txt_并发线程数目.Location = new System.Drawing.Point(448, 18);
            this.txt_并发线程数目.Name = "txt_并发线程数目";
            this.txt_并发线程数目.Size = new System.Drawing.Size(45, 21);
            this.txt_并发线程数目.TabIndex = 1;
            // 
            // txt_验证超时时间
            // 
            this.txt_验证超时时间.Location = new System.Drawing.Point(279, 18);
            this.txt_验证超时时间.Name = "txt_验证超时时间";
            this.txt_验证超时时间.Size = new System.Drawing.Size(45, 21);
            this.txt_验证超时时间.TabIndex = 1;
            // 
            // txt_连接超时时间
            // 
            this.txt_连接超时时间.Location = new System.Drawing.Point(110, 18);
            this.txt_连接超时时间.Name = "txt_连接超时时间";
            this.txt_连接超时时间.Size = new System.Drawing.Size(45, 21);
            this.txt_连接超时时间.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(499, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "个";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(330, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "秒";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "秒";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(353, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "并发线程数目：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "验证超时时间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "连接超时时间：";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 218);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(629, 143);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(195, 377);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(100, 23);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Location = new System.Drawing.Point(336, 377);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(100, 23);
            this.btn_Cancle.TabIndex = 1;
            this.btn_Cancle.Text = "取消";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // FrmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 422);
            this.Controls.Add(this.btn_Cancle);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "FrmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "参数设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.TextBox txt_连接超时时间;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_并发线程数目;
        private System.Windows.Forms.TextBox txt_验证超时时间;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listView_YanZhengZiYuan;
        private System.Windows.Forms.Button btn_导入;
        private System.Windows.Forms.Button btn_导出;
        private System.Windows.Forms.Button btn_删除验证;
        private System.Windows.Forms.Button btn_修改验证;
        private System.Windows.Forms.Button btn_添加验证;
        private System.Windows.Forms.ColumnHeader 资源名称;
        private System.Windows.Forms.ColumnHeader 验证资源网址;
    }
}