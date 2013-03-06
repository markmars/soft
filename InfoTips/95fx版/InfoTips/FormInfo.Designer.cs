namespace InfoTips
{
    partial class FormInfo
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label分类 = new System.Windows.Forms.Label();
            this.label时间 = new System.Windows.Forms.Label();
            this.label标题 = new System.Windows.Forms.Label();
            this.myLabel上一页 = new InfoTips.MyLabel();
            this.myLabel下一页 = new InfoTips.MyLabel();
            this.myLabel页面 = new InfoTips.MyLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Image = global::InfoTips.Properties.Resources.close;
            this.label1.Location = new System.Drawing.Point(272, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 24);
            this.label1.TabIndex = 1;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label分类
            // 
            this.label分类.AutoSize = true;
            this.label分类.BackColor = System.Drawing.Color.Transparent;
            this.label分类.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label分类.Location = new System.Drawing.Point(12, 38);
            this.label分类.Name = "label分类";
            this.label分类.Size = new System.Drawing.Size(33, 14);
            this.label分类.TabIndex = 2;
            this.label分类.Text = "分类";
            this.label分类.Click += new System.EventHandler(this.label标题_Click);
            // 
            // label时间
            // 
            this.label时间.AutoSize = true;
            this.label时间.BackColor = System.Drawing.Color.Transparent;
            this.label时间.Location = new System.Drawing.Point(77, 38);
            this.label时间.Name = "label时间";
            this.label时间.Size = new System.Drawing.Size(31, 14);
            this.label时间.TabIndex = 3;
            this.label时间.Text = "时间";
            this.label时间.Click += new System.EventHandler(this.label标题_Click);
            // 
            // label标题
            // 
            this.label标题.BackColor = System.Drawing.Color.Transparent;
            this.label标题.Location = new System.Drawing.Point(12, 70);
            this.label标题.Name = "label标题";
            this.label标题.Size = new System.Drawing.Size(278, 76);
            this.label标题.TabIndex = 4;
            this.label标题.Text = "标题";
            this.label标题.Click += new System.EventHandler(this.label标题_Click);
            // 
            // myLabel上一页
            // 
            this.myLabel上一页.AutoSize = true;
            this.myLabel上一页.BackColor = System.Drawing.Color.Transparent;
            this.myLabel上一页.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.myLabel上一页.Location = new System.Drawing.Point(12, 148);
            this.myLabel上一页.Name = "myLabel上一页";
            this.myLabel上一页.Size = new System.Drawing.Size(43, 14);
            this.myLabel上一页.TabIndex = 5;
            this.myLabel上一页.Text = "上一页";
            this.myLabel上一页.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.myLabel上一页.Click += new System.EventHandler(this.myLabel上一页_Click);
            // 
            // myLabel下一页
            // 
            this.myLabel下一页.AutoSize = true;
            this.myLabel下一页.BackColor = System.Drawing.Color.Transparent;
            this.myLabel下一页.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.myLabel下一页.Location = new System.Drawing.Point(247, 148);
            this.myLabel下一页.Name = "myLabel下一页";
            this.myLabel下一页.Size = new System.Drawing.Size(43, 14);
            this.myLabel下一页.TabIndex = 5;
            this.myLabel下一页.Text = "下一页";
            this.myLabel下一页.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.myLabel下一页.Click += new System.EventHandler(this.myLabel下一页_Click);
            // 
            // myLabel页面
            // 
            this.myLabel页面.AutoSize = true;
            this.myLabel页面.BackColor = System.Drawing.Color.Transparent;
            this.myLabel页面.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.myLabel页面.Location = new System.Drawing.Point(133, 148);
            this.myLabel页面.Name = "myLabel页面";
            this.myLabel页面.Size = new System.Drawing.Size(26, 14);
            this.myLabel页面.TabIndex = 6;
            this.myLabel页面.Text = "1/1";
            this.myLabel页面.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch;
            this.BackgroundImageStore = global::InfoTips.Properties.Resources.backimg;
            this.ClientSize = new System.Drawing.Size(302, 307);
            this.Controls.Add(this.myLabel页面);
            this.Controls.Add(this.myLabel下一页);
            this.Controls.Add(this.myLabel上一页);
            this.Controls.Add(this.label标题);
            this.Controls.Add(this.label时间);
            this.Controls.Add(this.label分类);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInfo";
            this.ShowInTaskbar = false;
            this.Tag = "10000";
            this.Text = "提醒";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextInfo_FormClosing);
            this.Load += new System.EventHandler(this.TextInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label分类;
        private System.Windows.Forms.Label label时间;
        private System.Windows.Forms.Label label标题;
        private MyLabel myLabel上一页;
        private MyLabel myLabel下一页;
        private MyLabel myLabel页面;
        private System.Windows.Forms.Timer timer1;
    }
}