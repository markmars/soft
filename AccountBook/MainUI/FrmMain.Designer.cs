namespace MainUI
{
    /// <summary>  
    /// 作者：MarkMars  
    /// 时间：2013-2-19 14:50:33  
    /// 公司:  
    /// 版权：2013-2013  
    /// CLR版本：4.0.30319.17929  
    /// 博客地址：http://www.wakealone.com  
    /// Form1说明：本代码版权归MarkMars所有，使用时必须带上MarkMars博客地址  
    /// 唯一标识：92c5e9bc-804e-454c-bf9b-f1e8612127d1  
    /// </summary>  
    partial class FrmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripModule = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusProjectInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelMain = new BaseUI.MMPanel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripModule
            // 
            this.toolStripModule.AutoSize = false;
            this.toolStripModule.BackgroundImage = global::MainUI.Properties.Resources.top_bg_01;
            this.toolStripModule.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripModule.Location = new System.Drawing.Point(0, 0);
            this.toolStripModule.Name = "toolStripModule";
            this.toolStripModule.Size = new System.Drawing.Size(792, 70);
            this.toolStripModule.TabIndex = 0;
            this.toolStripModule.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusProjectInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusProjectInfo
            // 
            this.statusProjectInfo.Name = "statusProjectInfo";
            this.statusProjectInfo.Size = new System.Drawing.Size(131, 17);
            this.statusProjectInfo.Text = "toolStripStatusLabel1";
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 70);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(792, 474);
            this.panelMain.TabIndex = 2;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(240)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStripModule);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.SizeChanged += new System.EventHandler(this.FrmMain_SizeChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripModule;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusProjectInfo;
        private BaseUI.MMPanel panelMain;



    }
}

