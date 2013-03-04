namespace BaseUI
{
    partial class BaseForm
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
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.AutoSize = false;
            this.toolBar.BackgroundImage = global::BaseUI.Properties.Resources.top_bg;
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(792, 36);
            this.toolBar.TabIndex = 0;
            this.toolBar.Text = "toolStrip1";
            // 
            // BaseForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(240)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.toolBar);
            this.Name = "BaseForm";
            this.Text = "BaseForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolBar;
    }
}
