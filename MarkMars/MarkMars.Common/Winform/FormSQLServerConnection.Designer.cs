namespace MarkMars.Common
{
    partial class FormSQLServerConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSQLServerConnection));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.m_cbWindowsAuthentication = new System.Windows.Forms.CheckBox();
            this.m_tbPassword = new System.Windows.Forms.TextBox();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.m_tbDatabaseName = new System.Windows.Forms.TextBox();
            this.m_tbServerName = new System.Windows.Forms.TextBox();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器名字:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据库名字:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "密码:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "用户名:";
            // 
            // m_cbWindowsAuthentication
            // 
            this.m_cbWindowsAuthentication.AutoSize = true;
            this.m_cbWindowsAuthentication.Location = new System.Drawing.Point(15, 102);
            this.m_cbWindowsAuthentication.Name = "m_cbWindowsAuthentication";
            this.m_cbWindowsAuthentication.Size = new System.Drawing.Size(90, 16);
            this.m_cbWindowsAuthentication.TabIndex = 4;
            this.m_cbWindowsAuthentication.Text = "Windows认证";
            this.m_cbWindowsAuthentication.UseVisualStyleBackColor = true;
            this.m_cbWindowsAuthentication.CheckedChanged += new System.EventHandler(this.m_cbWindowsAuthentication_CheckedChanged);
            // 
            // m_tbPassword
            // 
            this.m_tbPassword.Location = new System.Drawing.Point(96, 78);
            this.m_tbPassword.Name = "m_tbPassword";
            this.m_tbPassword.PasswordChar = '*';
            this.m_tbPassword.Size = new System.Drawing.Size(184, 21);
            this.m_tbPassword.TabIndex = 3;
            // 
            // m_tbUserName
            // 
            this.m_tbUserName.Location = new System.Drawing.Point(96, 56);
            this.m_tbUserName.Name = "m_tbUserName";
            this.m_tbUserName.Size = new System.Drawing.Size(184, 21);
            this.m_tbUserName.TabIndex = 2;
            // 
            // m_tbDatabaseName
            // 
            this.m_tbDatabaseName.Location = new System.Drawing.Point(96, 33);
            this.m_tbDatabaseName.Name = "m_tbDatabaseName";
            this.m_tbDatabaseName.Size = new System.Drawing.Size(184, 21);
            this.m_tbDatabaseName.TabIndex = 1;
            // 
            // m_tbServerName
            // 
            this.m_tbServerName.Location = new System.Drawing.Point(96, 8);
            this.m_tbServerName.Name = "m_tbServerName";
            this.m_tbServerName.Size = new System.Drawing.Size(184, 21);
            this.m_tbServerName.TabIndex = 0;
            // 
            // m_btnOK
            // 
            this.m_btnOK.Location = new System.Drawing.Point(52, 137);
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.Size = new System.Drawing.Size(75, 21);
            this.m_btnOK.TabIndex = 5;
            this.m_btnOK.Text = "确定";
            this.m_btnOK.UseVisualStyleBackColor = true;
            this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(163, 137);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // FormSQLServerConnection
            // 
            this.AcceptButton = this.m_btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(292, 184);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.m_btnOK);
            this.Controls.Add(this.m_tbServerName);
            this.Controls.Add(this.m_tbDatabaseName);
            this.Controls.Add(this.m_tbUserName);
            this.Controls.Add(this.m_tbPassword);
            this.Controls.Add(this.m_cbWindowsAuthentication);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSQLServerConnection";
            this.Text = "设置数据库连接";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox m_cbWindowsAuthentication;
        private System.Windows.Forms.TextBox m_tbPassword;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.TextBox m_tbDatabaseName;
        private System.Windows.Forms.TextBox m_tbServerName;
        private System.Windows.Forms.Button m_btnOK;
		private System.Windows.Forms.Button buttonCancel;
    }
}