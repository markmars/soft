using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BaseUI.Properties;
using System.Reflection;

namespace BaseUI
{
    public partial class BaseForm : Form
    {
        protected ToolStrip ToolBar
        {
            get { return this.toolBar; }
        }
        public BaseForm()
        {
            InitializeComponent();

            this.toolBar.Visible = false;
        }
        protected void InitControl(Form form)
        {
            this.toolBar.Visible = true;
            this.toolBar.Items.Clear();

            ToolStripLabel toolStripLabel = new ToolStripLabel();
            toolStripLabel.Name = "FormTitle";
            toolStripLabel.BackgroundImage = Resources.ResourceManager.GetObject("title_bg", Resources.Culture) as Bitmap;
            toolStripLabel.Width = 174;
            toolStripLabel.Height = 33;
            toolStripLabel.Margin = new System.Windows.Forms.Padding(0);
            toolStripLabel.AutoSize = false;
            toolStripLabel.ForeColor = Color.White;
            toolStripLabel.Font = new Font("宋体", 10f, FontStyle.Bold);
            toolStripLabel.TextAlign = ContentAlignment.MiddleLeft;
            toolStripLabel.Text = (form.Text.Length <= 9 ? "  " : "") + form.Text;
            this.toolBar.Items.Add(toolStripLabel);

            AccountBook.BLL.Sys sys = new AccountBook.BLL.Sys();
            DataTable dtToolBar = sys.GetToolBarByPage(form.Name);
            foreach (DataRow drButton in dtToolBar.Rows)
            {
                ToolStripButton toolStripButton = new ToolStripButton();
                toolStripButton.Name = drButton["ToolBarName"].ToString();
                toolStripButton.Text = drButton["ToolBarName"].ToString();
                toolStripButton.ToolTipText = drButton["Tooltip"].ToString();
                toolStripButton.Image = Resources.ResourceManager.GetObject(drButton["Icon"].ToString(), Resources.Culture) as Bitmap;
                toolStripButton.Click += new EventHandler(toolStripButton_Click);
                toolStripButton.MouseHover += new EventHandler(toolStripButton_MouseOver);
                toolStripButton.MouseLeave += new EventHandler(toolStripButton_MouseLeave);
                toolStripButton.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                toolStripButton.Tag = drButton["EventHandler"].ToString();
                toolStripButton.Alignment = ToolStripItemAlignment.Right;

                this.toolBar.Items.Insert(0, toolStripButton);
            }
        }
        private void toolStripButton_Click(object sender, EventArgs e)
        {
            String handler = ((ToolStripButton)sender).Tag.ToString();
            this.InvokeToolStripButtonHandler(handler);
        }
        private void toolStripButton_MouseOver(object sender, EventArgs e)
        {
            ToolStripButton toolStripButton = (ToolStripButton)sender;

            toolStripButton.ForeColor = Color.Black;
        }
        private void toolStripButton_MouseLeave(object sender, EventArgs e)
        {
            ToolStripButton toolStripButton = (ToolStripButton)sender;

            toolStripButton.ForeColor = Color.White;
        }
        private void InvokeToolStripButtonHandler(String handler)
        {
            try
            {
                MethodInfo method = this.GetType().GetMethod(handler, BindingFlags.Instance | BindingFlags.NonPublic);

                if (method == null)
                {
                    MessageBox.Show("未实现方法【" + handler + "】");
                }
                else
                {
                    method.Invoke(this, new Object[] { null, EventArgs.Empty });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }
        protected void ShowToolStripButton(String toolStripButtonName, Boolean isShow)
        {
            foreach (ToolStripItem toolStripItem in this.toolBar.Items)
            {
                if (toolStripItem.Name == toolStripButtonName)
                {
                    toolStripItem.Visible = isShow;
                    break;
                }
            }
        }
        protected void EnabledToolStripButton(String toolStripButtonName, Boolean enabled)
        {
            foreach (ToolStripItem toolStripItem in this.toolBar.Items)
            {
                if (toolStripItem.Name == toolStripButtonName)
                {
                    toolStripItem.Enabled = enabled;
                    break;
                }
            }
        }
        protected void SetToolStripButtonText(String toolStripButtonName, String text)
        {
            foreach (ToolStripItem toolStripItem in this.toolBar.Items)
            {
                if (toolStripItem.Name == toolStripButtonName)
                {
                    toolStripItem.Text = text;
                    break;
                }
            }
        }
        protected void SetFormTitle(String formTitle)
        {
            if (this.toolBar.Items["FormTitle"] != null)
            {
                this.toolBar.Items["FormTitle"].Text = formTitle;
            }
        }
    }
}
