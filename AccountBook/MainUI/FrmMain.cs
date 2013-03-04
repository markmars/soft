using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

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
    public partial class FrmMain : Form
    {
        #region 窗体函数
        public FrmMain()
        {
            InitializeComponent();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.Text = "账目管理系统";
            this.statusProjectInfo.Text = String.Empty;

            this.InitControl("MainUI.FrmMain");
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!MarkMars.Common2.MarkMarsMessageBox.ShowConfirm("确定要退出吗？"))
            {
                e.Cancel = true;
            }
            else
            {
                this.Dispose();
                Application.Exit();
            }
        }
        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            ToolStripItem toolStripModuleBg = this.toolStripModule.Items["ToolModuleBg"];
            if (toolStripModuleBg != null)
            {
                Int32 toolStripWidth = 0;
                foreach (ToolStripItem toolStripItem in this.toolStripModule.Items)
                {
                    toolStripWidth += toolStripItem.Width;
                }

                if (toolStripWidth > this.Width)
                {
                    toolStripModuleBg.Visible = false;
                }
                else
                {
                    toolStripModuleBg.Visible = true;
                }
            }
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 显示MDI子窗体。
        /// </summary>
        /// <param name="formName"></param>
        private Form ShowChildForm(Form childForm)
        {
            if (childForm == null || childForm.IsDisposed)
            {
                MarkMars.Common2.MarkMarsMessageBox.ShowMessage("请实现窗体！");
                return null;
            }

            Form form = null;
            foreach (Object oCtrl in this.panelMain.Controls)
            {
                Form theForm = oCtrl as Form;
                if (theForm.Name == childForm.Name)
                {
                    form = theForm;
                    form.BringToFront();
                    break;
                }
            }

            if (form == null)
            {
                form = childForm;

                form.TopLevel = false;        //设置子窗口不显示为顶级窗口
                form.FormBorderStyle = FormBorderStyle.None;   //设置子窗口的样式，没有上面的标题栏
                panelMain.Controls.Clear();                    //清空Panel里面的控件
                panelMain.Controls.Add(form);                  //加入控件
                form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();
            }
            else
            {
                childForm.Dispose();
                childForm = null;
            }

            return form;
        }
        protected void InitControl(String pageName)
        {
            this.toolStripModule.Items.Clear();

            AccountBook.BLL.Sys sys = new AccountBook.BLL.Sys();
            DataTable dtToolBar = sys.GetToolBarByPage(pageName);
            foreach (DataRow drToolBar in dtToolBar.Rows)
            {
                if (drToolBar["ToolBarType"].ToString() == Convert.ToInt32(AccountBook.Utility.ToolBarType.Button).ToString())
                {
                    ToolStripButton toolStripButton = new ToolStripButton();

                    toolStripButton.Name = drToolBar["ToolBarName"].ToString();
                    toolStripButton.Text = drToolBar["ToolBarText"].ToString();
                    toolStripButton.Click += new EventHandler(toolStripButton_Click);
                    toolStripButton.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    toolStripButton.Tag = drToolBar;
                    toolStripButton.Image = MainUI.Properties.Resources.ResourceManager.GetObject(drToolBar["Icon"].ToString(), MainUI.Properties.Resources.Culture) as Bitmap;
                    toolStripButton.ImageScaling = ToolStripItemImageScaling.None;

                    this.toolStripModule.Items.Add(toolStripButton);
                }
                else if (drToolBar["ToolBarType"].ToString() == Convert.ToInt32(AccountBook.Utility.ToolBarType.DropDownButton).ToString())
                {

                    ToolStripDropDownButton toolStripButton = new ToolStripDropDownButton();

                    toolStripButton.Name = drToolBar["ToolBarName"].ToString();
                    toolStripButton.Text = drToolBar["ToolBarText"].ToString();
                    toolStripButton.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    toolStripButton.Image = MainUI.Properties.Resources.ResourceManager.GetObject(drToolBar["Icon"].ToString(), MainUI.Properties.Resources.Culture) as Bitmap;
                    toolStripButton.ImageScaling = ToolStripItemImageScaling.None;

                    this.toolStripModule.Items.Add(toolStripButton);

                    DataTable dtToolBarItem = sys.GetToolBarByPageAndParent(pageName, drToolBar["ToolBarName"].ToString());
                    foreach (DataRow drToolBarItem in dtToolBarItem.Rows)
                    {
                        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();

                        toolStripMenuItem.BackColor = Color.FromArgb(237, 252, 255);
                        toolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
                        toolStripMenuItem.TextAlign = ContentAlignment.MiddleCenter;

                        toolStripMenuItem.Name = drToolBarItem["ToolBarItemName"].ToString();
                        toolStripMenuItem.Text = drToolBarItem["ToolBarItemText"].ToString();
                        toolStripMenuItem.Click += new EventHandler(toolStripMenuItem_Click);
                        toolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        toolStripMenuItem.Tag = drToolBarItem;
                        toolStripMenuItem.Image = MainUI.Properties.Resources.ResourceManager.GetObject(drToolBarItem["Icon"].ToString(), MainUI.Properties.Resources.Culture) as Bitmap;
                        toolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;

                        toolStripButton.DropDownItems.Add(toolStripMenuItem);
                    }
                }
            }

            ToolStripButton toolStripButtonExit = new ToolStripButton();
            toolStripButtonExit.Name = "Exit";
            toolStripButtonExit.Text = "退 出";
            toolStripButtonExit.Click += new EventHandler(Close_OnClick);
            toolStripButtonExit.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            toolStripButtonExit.Image = MainUI.Properties.Resources.ResourceManager.GetObject("exit", MainUI.Properties.Resources.Culture) as Bitmap;
            toolStripButtonExit.ImageScaling = ToolStripItemImageScaling.None;
            toolStripButtonExit.ForeColor = Color.White;
            toolStripButtonExit.MouseHover += new EventHandler(toolStripButton_MouseOver);
            toolStripButtonExit.MouseLeave += new EventHandler(toolStripButton_MouseLeave);
            this.toolStripModule.Items.Add(toolStripButtonExit);

            ToolStripLabel toolStripLabel = new ToolStripLabel();
            toolStripLabel.Name = "ToolModuleBg";
            toolStripLabel.BackgroundImage = MainUI.Properties.Resources.ResourceManager.GetObject("top_bg_02", MainUI.Properties.Resources.Culture) as Bitmap;
            toolStripLabel.Width = 372;
            toolStripLabel.Height = 69;
            toolStripLabel.Margin = new System.Windows.Forms.Padding(0);
            toolStripLabel.AutoSize = false;
            toolStripLabel.ForeColor = Color.White;
            toolStripLabel.TextAlign = ContentAlignment.MiddleCenter;
            toolStripLabel.Alignment = ToolStripItemAlignment.Right;
            this.toolStripModule.Items.Add(toolStripLabel);
        }
        private void Close_OnClick(object sender, EventArgs e)
        {
            this.Close();
        }
        private void toolStripButton_Click(object sender, EventArgs e)
        {
            DataRow drToolBar = (DataRow)((ToolStripButton)sender).Tag;

            String handler = drToolBar["EventHandler"].ToString();


            if (!String.IsNullOrEmpty(handler) && !handler.ToLower().Equals("none"))
            {
                this.InvokeToolStripButtonHandler(handler, sender);
            }
            else
            {
                Form form = MarkMars.Factory.FormFactory.CreateForm(drToolBar["AssemblyFile"].ToString()
                    , drToolBar["PageFullName"].ToString()
                    , drToolBar["Args"].ToString()
                    , drToolBar["PageName"].ToString()
                    , drToolBar["ToolBarText"].ToString());

                this.ShowChildForm(form);
            }
        }
        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow drModule = (DataRow)((ToolStripMenuItem)sender).Tag;

            String handler = drModule["EventHandler"].ToString();

            if (!String.IsNullOrEmpty(handler) && !handler.ToLower().Equals("none"))
            {
                this.InvokeToolStripButtonHandler(handler, sender);
            }
            else
            {
                Form form = MarkMars.Factory.FormFactory.CreateForm(
                    drModule["AssemblyFile"].ToString()
                    , drModule["PageFullName"].ToString()
                    , drModule["Args"].ToString()
                    , drModule["ModuleName"].ToString()
                    , drModule["ModuleText"].ToString());

                this.ShowChildForm(form);
            }
        }
        private void toolStripButton_MouseOver(object sender, EventArgs e)
        {
            ToolStripButton toolStripButton = (ToolStripButton)sender;

            toolStripButton.ForeColor = Color.Black;
            toolStripButton.BackColor = Color.FromArgb(187, 237, 248);
        }
        private void toolStripButton_MouseLeave(object sender, EventArgs e)
        {
            ToolStripButton toolStripButton = (ToolStripButton)sender;

            toolStripButton.ForeColor = Color.White;
            toolStripButton.BackColor = Color.Transparent;
        }
        /// <summary>
        /// 调用工具栏按钮的处理方法。
        /// </summary>
        /// <param name="handler">按钮处理方法名称。</param>
        private void InvokeToolStripButtonHandler(String handler, object sender)
        {
            try
            {
                MethodInfo method = this.GetType().GetMethod(handler, BindingFlags.Instance | BindingFlags.NonPublic);

                if (method == null)
                {
                    MarkMars.Common2.MarkMarsMessageBox.ShowMessage("未实现方法【" + handler + "】");
                }
                else
                {
                    method.Invoke(this, new Object[] { sender, EventArgs.Empty });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}
