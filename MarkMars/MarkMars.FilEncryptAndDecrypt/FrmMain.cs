using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MarkMars.FilEncryptAndDecrypt
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private void toolStripButton加密_Click(object sender, EventArgs e)
        {
            this.Text = "加密";
        }

        private void toolStripButton解密_Click(object sender, EventArgs e)
        {
            this.Text = "解密";
        }
        private void FrmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void FrmMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] strsFiles = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string strFile in strsFiles)
                ConvertFile(strFile);
        }
        private void ConvertFile(string strFile)
        {
            if (this.Text == "解密")
            {
                string[] strsExtension = new string[] { ".MMDF" };
                bool bMatch = false;
                foreach (string strExtension in strsExtension)
                {
                    if (strFile.EndsWith(strExtension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        bMatch = true;
                        break;
                    }
                }
                if (!bMatch)
                    return;

                FileInfo fi = new FileInfo(strFile);
                if ((fi.Attributes & FileAttributes.Directory) != 0)
                    return; //忽略目录

                string strNewName = fi.FullName.Replace(".MMDF", ".7z");
                File.Copy(strFile, strNewName);
                FileStream fs = new FileStream(strNewName, FileMode.Open);
                fs.WriteByte(0x37);
                fs.Close();
            }
            if (this.Text == "加密")
            {
                string[] strsExtension = new string[] { ".7z" };
                bool bMatch = false;
                foreach (string strExtension in strsExtension)
                {
                    if (strFile.EndsWith(strExtension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        bMatch = true;
                        break;
                    }
                }
                if (!bMatch)
                    return;

                FileInfo fi = new FileInfo(strFile);
                if ((fi.Attributes & FileAttributes.Directory) != 0)
                    return; //忽略目录

                string strNewName = fi.FullName.Replace(".7z", ".MMDF");
                File.Copy(strFile, strNewName);
                FileStream fs = new FileStream(strNewName, FileMode.Open);
                fs.WriteByte(0x52);
                fs.Close();
            }
        }
    }
}
