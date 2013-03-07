namespace MarkMars.FilEncryptAndDecrypt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripButton加密 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton解密 = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton加密,
            this.toolStripButton解密});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(292, 25);
            this.toolBar.TabIndex = 1;
            this.toolBar.Text = "toolStrip1";
            // 
            // toolStripButton加密
            // 
            this.toolStripButton加密.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton加密.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton加密.Image")));
            this.toolStripButton加密.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton加密.Name = "toolStripButton加密";
            this.toolStripButton加密.Size = new System.Drawing.Size(33, 22);
            this.toolStripButton加密.Text = "加密";
            this.toolStripButton加密.Click += new System.EventHandler(this.toolStripButton加密_Click);
            // 
            // toolStripButton解密
            // 
            this.toolStripButton解密.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton解密.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton解密.Image")));
            this.toolStripButton解密.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton解密.Name = "toolStripButton解密";
            this.toolStripButton解密.Size = new System.Drawing.Size(33, 22);
            this.toolStripButton解密.Text = "解密";
            this.toolStripButton解密.Click += new System.EventHandler(this.toolStripButton解密_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "把文件拖进来";
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolBar);
            this.Name = "FrmMain";
            this.Text = "解密";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragEnter);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton toolStripButton加密;
        private System.Windows.Forms.ToolStripButton toolStripButton解密;
        private System.Windows.Forms.Label label1;

    }
}

