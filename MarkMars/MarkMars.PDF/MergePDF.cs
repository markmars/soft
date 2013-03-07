using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using MarkMars.Common2;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MarkMars.PDF
{
    public class MergePDF
    {
        private String tempFileName;
        private String userTempPathFullName;
        private Int32 totalCount = 0;
        private Int32 preCount = 1;
        private PdfSplitterMerger psm;
        private String areaType = String.Empty;
        public String AreaType
        {
            set
            {
                this.areaType = value;
            }
        }

        public MergePDF() { }

        public MergePDF(String userTempPathFullName)
        {
            this.userTempPathFullName = userTempPathFullName;
        }

        public Boolean MergePDFFile(String destFileName, String xmlFilePath, String prefix)
        {
            return this.MergePDFFile(destFileName, xmlFilePath, prefix, true, true);
        }

        public Boolean MergePDFFile(String destFileName, String xmlFilePath, String prefix, Boolean isCreateNew, Boolean isNeedAddBookmarks)
        {
            totalCount = 0;
            preCount = 1;

            psm = new PdfSplitterMerger(destFileName);
            try
            {
                if (!File.Exists(xmlFilePath))
                {
                    MarkMarsMessageBox.ShowError("XML文件不存在");
                    return false;
                }

                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilePath);
                XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

                if (!GetMergePDF(xmlNodeList, prefix, xmlNodeList[0].Attributes["Ord"].Value, isCreateNew))
                {
                    return false;
                }

                psm.Finish();
                xmlDoc.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                MarkMarsMessageBox.ShowError("合并PDF失败！" + ex.Message);
                return false;
            }

            if (isNeedAddBookmarks)
            {
                try
                {
                    Bookmarks bm = new Bookmarks();
                    bm.AddBookmarks(xmlFilePath, destFileName, destFileName);
                }
                catch
                {
                    MarkMarsMessageBox.ShowError("增加书签失败！");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 合并PDF文件
        /// </summary>
        private Boolean GetMergePDF(XmlNodeList xmlNodeList, String prefix, String firstOrd, Boolean isCreateNew)
        {
            Boolean isMergeSuccessful = true;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                totalCount += preCount;
                if (xmlNode.HasChildNodes)
                {
                    preCount = 0;
                }
                else
                {
                    tempFileName = this.userTempPathFullName + prefix + "_" + xmlNode.Attributes["Ord"].Value + ".pdf";

                    if (isCreateNew && !File.Exists(tempFileName))
                    {
                        //设置文档大小及边距
                        Document document = new Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
                        PdfWriter.getInstance(document, new FileStream(tempFileName, FileMode.Create));

                        document.Open();

                        
                        float size = 12;
                        switch (this.areaType)
                        {
                            case "TY":
                                size = 14;
                                break;
                            default:
                                size = 12;
                                break;
                        }
                        BaseFont bfChinese = BaseFont.createFont(Environment.GetEnvironmentVariable("WinDir") + "\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        Font fontChinese = new Font(bfChinese, size, 0, new Color(0, 0, 0));
                        document.Add(new Paragraph(xmlNode.Attributes["Sequence"].Value + "、" + (String.IsNullOrEmpty(xmlNode.Attributes["Item"].Value) ? xmlNode.Attributes["Content"].Value : xmlNode.Attributes["Item"].Value), fontChinese));

                        document.Close();
                    }

                    if (File.Exists(tempFileName))
                    {
                        try
                        {
                            psm.MergePDF(tempFileName);
                            preCount = psm.PageCount;
                        }
                        catch (Exception ex)
                        {
                            MarkMarsMessageBox.ShowError(ex.Message);
                            isMergeSuccessful = false;
                            break;
                        }
                    }
                }

                xmlNode.Attributes["PageCount"].Value = totalCount.ToString();

                if (xmlNode.HasChildNodes)
                {
                    isMergeSuccessful = this.GetMergePDF(xmlNode.ChildNodes, prefix, firstOrd, isCreateNew);
                    if (!isMergeSuccessful)
                    {
                        break;
                    }
                }
            }
            return isMergeSuccessful;
        }

        /// <summary>
        /// 检查PDF文件是否有效
        /// </summary>
        public Boolean CheckPdfFileIsValidate(String pdfFileName)
        {
            FileStream fs = File.OpenRead(pdfFileName);
            try
            {                
                PdfFile pdfFile = new PdfFile(fs);
                pdfFile.Load();

                if (pdfFile.PageCount == 0)
                {
                    fs.Close();
                    fs.Dispose();
                    return false;
                }

                fs.Close();
                fs.Dispose();

                return true;
            }
            catch
            {
                fs.Close();
                fs.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 合并HJ投标PDF文件
        /// </summary>
        public Boolean MergePDFFile_HJ(String saveFilePath, String xmlFilePath, Boolean isCreateNew, Boolean isNeedAddBookmarks)
        {
            try
            {
                if (!File.Exists(xmlFilePath))
                {
                    MarkMarsMessageBox.ShowWarning("XML文件不存在");
                    return false;
                }

                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilePath);
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/TechBid/PBNR/PJTM");
                PdfSplitterMerger pdfSM = null;
                String destFileName = String.Empty;
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    if (xmlNode.Attributes["IsImport"].Value.ToLower() == "false")
                    {
                        continue;
                    }

                    totalCount = 0;
                    preCount = 1;
                    destFileName = saveFilePath + xmlNode.Attributes["Guid"].Value + ".pdf";
                    pdfSM = new PdfSplitterMerger(destFileName);
                    foreach (XmlNode childNode in xmlNode.ChildNodes)
                    {
                        tempFileName = this.userTempPathFullName + childNode.Attributes["Guid"].Value + ".pdf";

                        if (isCreateNew && !File.Exists(tempFileName))
                        {
                            //设置文档大小及边距
                            Document document = new Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
                            PdfWriter.getInstance(document, new FileStream(tempFileName, FileMode.Create));

                            document.Open();

                            float size = 14;
                            BaseFont bfChinese = BaseFont.createFont(Environment.GetEnvironmentVariable("WinDir") + "\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            Font fontChinese = new Font(bfChinese, size, 0, new Color(0, 0, 0));
                            document.Add(new Paragraph(childNode.Attributes["Sequence"].Value == String.Empty ? String.Empty : (childNode.Attributes["Sequence"].Value + "、") + childNode.Attributes["XMItem"].Value, fontChinese));

                            document.Close();
                        }

                        if (File.Exists(tempFileName))
                        {
                            pdfSM.MergePDF(tempFileName);
                            preCount = pdfSM.PageCount;
                        }

                        totalCount += preCount;
                        childNode.Attributes["PageCount"].Value = totalCount.ToString();
                    }

                    pdfSM.Finish();

                    if (isNeedAddBookmarks)
                    {
                        try
                        {
                            Bookmarks bookmark = new Bookmarks();
                            bookmark.AddBookmarks_HJ(xmlNode, destFileName, destFileName);
                        }
                        catch(Exception ex)
                        {
                            throw new Exception("增加书签失败！" + Environment.NewLine + ex.Message);
                        }
                    }
                }

                xmlDoc.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                MarkMarsMessageBox.ShowError("合并PDF失败！" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 合并铁路施工投标PDF文件
        /// </summary>
        public Boolean MergePDFFile_BJTL_SG(String destFileName, String xmlFilePath, String prefix, Boolean isNeedAddBookmarks, String bidType)
        {
            totalCount = 0;
            preCount = 1;

            psm = new PdfSplitterMerger(destFileName);
            XmlNodeList xmlNodeList;
            try
            {
                if (!File.Exists(xmlFilePath))
                {
                    MarkMarsMessageBox.ShowError("XML文件不存在");
                    return false;
                }

                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFilePath);
                xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

                if (!GetMergePDF_BJTL_SG(xmlNodeList, prefix, bidType))
                {
                    return false;
                }

                psm.Finish();
                xmlDoc.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                MarkMarsMessageBox.ShowError("合并PDF失败！" + ex.Message);
                return false;
            }

            if (isNeedAddBookmarks)
            {
                try
                {
                    Bookmarks bm = new Bookmarks();
                    bm.AddBookmarks_BJTL_SG(xmlNodeList, destFileName, destFileName, bidType);
                }
                catch
                {
                    MarkMarsMessageBox.ShowError("增加书签失败！");
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 合并PDF文件
        /// </summary>
        private Boolean GetMergePDF_BJTL_SG(XmlNodeList xmlNodeList, String prefix, String bidType)
        {
            Boolean isMergeSuccessful = true;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["Item"].Value == "已标价工程量清单" || xmlNode.Attributes["BidType"].Value != bidType)
                {
                    continue;
                }

                totalCount += preCount;
                if (xmlNode.HasChildNodes)
                {
                    preCount = 0;
                }
                else
                {
                    tempFileName = this.userTempPathFullName + prefix + "_" + xmlNode.Attributes["Ord"].Value + ".pdf";

                    if (!File.Exists(tempFileName))
                    {
                        //设置文档大小及边距
                        Document document = new Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
                        PdfWriter.getInstance(document, new FileStream(tempFileName, FileMode.Create));

                        document.Open();


                        float size = 12;
                        BaseFont bfChinese = BaseFont.createFont(Environment.GetEnvironmentVariable("WinDir") + "\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        Font fontChinese = new Font(bfChinese, size, 0, new Color(0, 0, 0));
                        document.Add(new Paragraph(xmlNode.Attributes["Sequence"].Value + "、" + (String.IsNullOrEmpty(xmlNode.Attributes["Item"].Value) ? xmlNode.Attributes["Content"].Value : xmlNode.Attributes["Item"].Value), fontChinese));

                        document.Close();
                    }

                    if (File.Exists(tempFileName))
                    {
                        try
                        {
                            psm.MergePDF(tempFileName);
                            preCount = psm.PageCount;
                        }
                        catch (Exception ex)
                        {
                            MarkMarsMessageBox.ShowError(ex.Message);
                            isMergeSuccessful = false;
                            break;
                        }
                    }
                }

                xmlNode.Attributes["PageCount"].Value = totalCount.ToString();

                if (xmlNode.HasChildNodes)
                {
                    isMergeSuccessful = this.GetMergePDF_BJTL_SG(xmlNode.ChildNodes, prefix, bidType);
                    if (!isMergeSuccessful)
                    {
                        break;
                    }
                }
            }
            return isMergeSuccessful;
        }
    }
}