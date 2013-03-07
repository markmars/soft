using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MarkMars.PDF
{
    public class Bookmarks
    {
        private static readonly Int32 fontNameMaxLength = 100;
        private static readonly Int32 titleMaxLength = 88;
        private static readonly Regex regSpace = new Regex(@"\s+");
        private static readonly Regex regDoubleByteChar = new Regex("[\u0100-\uffff]");

        /// <summary>
        /// 新增书签。
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="pdfFile"></param>
        /// <param name="outFile"></param>
        public void AddBookmarksFromXmlString(String xmlString, String pdfFile, String outFile)
        {
            Int32 flag = 0;
            Int32 m_pdf = 0;

            Int32 res = Declares.PXCp_Init(out m_pdf, Declares.g_RegKey, Declares.g_DevCode);
            res = Declares.PXCp_ReadDocumentW(m_pdf, pdfFile, 0);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            foreach (XmlNode xmlNode in xmlDoc.SelectNodes("TechBid/ZBItem"))
            {
                AddBookmarkNode(xmlNode, m_pdf, flag, 0);

                flag++;
            }

            res = Declares.PXCp_WriteDocumentW(m_pdf, outFile, Declares.PXCp_CreationDisposition.PXCp_CreationDisposition_Overwrite, (int)Declares.PXCp_WriteDocFlag.PXCp_Write_Release);
            m_pdf = 0;
        }

        /// <summary>
        /// 新增书签。
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="pdfFile"></param>
        /// <param name="outFile"></param>
        public void AddBookmarks(String xmlFile, String pdfFile, String outFile)
        {
            Int32 flag = 0;
            Int32 m_pdf = 0;
            
            Int32 res = Declares.PXCp_Init(out m_pdf, Declares.g_RegKey, Declares.g_DevCode);
            res = Declares.PXCp_ReadDocumentW(m_pdf, pdfFile, 0);
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            foreach (XmlNode xmlNode in xmlDoc.SelectNodes("TechBid/ZBItem"))
            {
                AddBookmarkNode(xmlNode, m_pdf, flag, 0);

                flag++;
            }

            res = Declares.PXCp_WriteDocumentW(m_pdf, outFile, Declares.PXCp_CreationDisposition.PXCp_CreationDisposition_Overwrite, (int)Declares.PXCp_WriteDocFlag.PXCp_Write_Release);
            m_pdf = 0;
        }

        /// <summary>
        /// 新增书签节点。
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="m_pdf"></param>
        /// <param name="flag"></param>
        /// <param name="level"></param>
        private void AddBookmarkNode(XmlNode xmlNode, Int32 m_pdf, Int32 flag, Int32 level)
        {
            int res = 0;
            Int32 new_item = 0;

            Declares.PXCp_BMInfo bm_info = new Declares.PXCp_BMInfo();
            Declares.PXCp_BMDestination bm_dest = new Declares.PXCp_BMDestination();

            bm_info.cbSize = Marshal.SizeOf(bm_info);
            bm_info.Mask = (int)Declares.PXCp_BMInfoMask.BMIM_TitleW |
                (int)Declares.PXCp_BMInfoMask.BMIM_Open | (int)Declares.PXCp_BMInfoMask.BMIM_Style |
                (int)Declares.PXCp_BMInfoMask.BMIM_Color | (int)Declares.PXCp_BMInfoMask.BMIM_Destination;

            bm_dest.DestType = Declares.PXC_OutlineDestination.Dest_Page;   //Dest_XYZ;
            bm_dest.Mask = 0;
            bm_dest.Left = Convert.ToDouble(0);
            bm_dest.Top = Convert.ToDouble(0);
            bm_dest.Right = Convert.ToDouble(0);
            bm_dest.Bottom = Convert.ToDouble(0);
            bm_dest.Zoom = Convert.ToDouble(0);
            bm_dest.PageNumber = Convert.ToInt32(xmlNode.Attributes["PageCount"].Value) - 1;

            String title = (String.IsNullOrEmpty(xmlNode.Attributes["Sequence"].Value) ? "" : (xmlNode.Attributes["Sequence"].Value + "、")) + (String.IsNullOrEmpty(xmlNode.Attributes["Item"].Value) ? xmlNode.Attributes["Content"].Value : xmlNode.Attributes["Item"].Value);
            bm_info.TitleW = Marshal.StringToHGlobalUni(title);
            bm_info.LengthOfTitle = title.Length;
            bm_info.bOpen = 1;
            bm_info.Style = Declares.PXC_OutlineStyle.OutlineStyle_Normal;
            bm_info.Color = System.Drawing.Color.Black.ToArgb();
            bm_info.Destination = bm_dest;

            if (level == 0)
            {
                if (flag == 0)
                {
                    res = Declares.PXCp_BMInsertItem(m_pdf, 0, (int)Declares.PXCp_OutlinePos.PBM_ROOT, out new_item, ref bm_info);
                    ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());
                    
                    flag++;
                }
                else
                {
                    res = Declares.PXCp_BMInsertItem(m_pdf, 0, (int)Declares.PXCp_OutlinePos.PBM_LAST, out new_item, ref bm_info);
                    ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());
                }
            }
            else
            {
                res = Declares.PXCp_BMInsertItem(m_pdf, Convert.ToInt32(xmlNode.ParentNode.Attributes["pageId"].Value), (int)Declares.PXCp_OutlinePos.PBM_LAST, out new_item, ref bm_info);
                ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());
            }

            if (xmlNode.HasChildNodes)
            {
                XmlNodeList xmlNodeList = xmlNode.ChildNodes;
                XmlNode tempNode;

                for (int idx = 0; idx <= xmlNodeList.Count - 1; idx++)
                {
                    tempNode = xmlNode.ChildNodes[idx];

                    AddBookmarkNode(tempNode, m_pdf, 1, 1);
                }
            }

            //Marshal.Release(bm_info.TitleW);
        }

        /// <summary>
        /// 新增书签。
        /// </summary>
        private String AddBookmarks(Int32 m_pdf, List<String[]> lstParams)
        {
            Int32 pageNumber = 0;
            String bookmarkName = String.Empty; ;

            try
            {
                foreach (String[] strArr in lstParams)
                {
                    pageNumber = Convert.ToInt32(strArr[0]);
                    bookmarkName = strArr[1];

                    Int32 new_item = 0;

                    Declares.PXCp_BMInfo bm_info = new Declares.PXCp_BMInfo();
                    Declares.PXCp_BMDestination bm_dest = new Declares.PXCp_BMDestination();

                    bm_info.cbSize = Marshal.SizeOf(bm_info);
                    bm_info.Mask = (int)Declares.PXCp_BMInfoMask.BMIM_TitleW |
                        (int)Declares.PXCp_BMInfoMask.BMIM_Open | (int)Declares.PXCp_BMInfoMask.BMIM_Style |
                        (int)Declares.PXCp_BMInfoMask.BMIM_Color | (int)Declares.PXCp_BMInfoMask.BMIM_Destination;

                    bm_dest.DestType = Declares.PXC_OutlineDestination.Dest_Page;   //Dest_XYZ;
                    bm_dest.Mask = 0;
                    bm_dest.Left = Convert.ToDouble(0);
                    bm_dest.Top = Convert.ToDouble(0);
                    bm_dest.Right = Convert.ToDouble(0);
                    bm_dest.Bottom = Convert.ToDouble(0);
                    bm_dest.Zoom = Convert.ToDouble(0);
                    bm_dest.PageNumber = pageNumber - 1;

                    String title = bookmarkName;
                    bm_info.TitleW = Marshal.StringToHGlobalUni(title);
                    bm_info.LengthOfTitle = title.Length;
                    bm_info.bOpen = 1;
                    bm_info.Style = Declares.PXC_OutlineStyle.OutlineStyle_Normal;
                    bm_info.Color = System.Drawing.Color.Black.ToArgb();
                    bm_info.Destination = bm_dest;
                    
                    Declares.PXCp_BMInsertItem(m_pdf, 0, (int)Declares.PXCp_OutlinePos.PBM_LAST, out new_item, ref bm_info);
                }

                m_pdf = 0;

                return String.Empty;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 新增混合型书签。
        /// </summary>
        public String AddCoordinateBookmarks(String xmlFile, String pdfFile, String outFile, List<String[]> lst)
        {
            try
            {
                Int32 pdf = 0;
                String errorMsg = String.Empty;
                Int32 res = Declares.PXCp_Init(out pdf, Declares.g_RegKey, Declares.g_DevCode);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    if (pdf != 0)
                    {
                        Declares.PXCp_ET_Finish(pdf);
                    }

                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                res = Declares.PXCp_ReadDocumentW(pdf, pdfFile, 0);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);

                XmlNode xn = xmlDoc.SelectSingleNode("*");
                Dictionary<Int32, XmlNode> xmlNodes = new Dictionary<Int32, XmlNode>();
                ((XmlElement)xn).SetAttribute("level", "-1");
                ((XmlElement)xn).SetAttribute("flag", "-1");
                xmlNodes.Add(xmlNodes.Count, xn);

                this.AddXmlNodes(xn, ref xmlNodes, 0);
                errorMsg = this.AddBookmarkNode(pdf, xmlNodes);
                if (!String.IsNullOrEmpty(errorMsg))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    return errorMsg;
                }

                errorMsg = AddBookmarks(pdf, lst);
                if (!String.IsNullOrEmpty(errorMsg))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    return errorMsg;
                }

                res = Declares.PXCp_WriteDocumentW(pdf, outFile, Declares.PXCp_CreationDisposition.PXCp_CreationDisposition_Overwrite,
                    (Int32)Declares.PXCp_WriteDocFlag.PXCp_Write_Release);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                Declares.PXCp_ET_Finish(pdf);
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 新增带坐标的书签。
        /// </summary>
        /// <param name="xmlFile">XML 文件名。</param>
        /// <param name="pdfFile">PDF 文件名。</param>
        /// <param name="outFile">增加书签后输出的 PDF 文件名。</param>
        /// <returns>错误信息（空字符串表示没有错误）。</returns>
        public String AddCoordinateBookmarks(String xmlFile, String pdfFile, String outFile)
        {
            try
            {
                Int32 pdf = 0;
                String errorMsg = String.Empty;
                Int32 res = Declares.PXCp_Init(out pdf, Declares.g_RegKey, Declares.g_DevCode);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    if (pdf != 0)
                    {
                        Declares.PXCp_ET_Finish(pdf);
                    }

                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                res = Declares.PXCp_ReadDocumentW(pdf, pdfFile, 0);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);

                XmlNode xn = xmlDoc.SelectSingleNode("*");
                Dictionary<Int32, XmlNode> xmlNodes = new Dictionary<Int32, XmlNode>();
                ((XmlElement)xn).SetAttribute("level", "-1");
                ((XmlElement)xn).SetAttribute("flag", "-1");
                xmlNodes.Add(xmlNodes.Count, xn);

                this.AddXmlNodes(xn, ref xmlNodes, 0);
                errorMsg = this.AddBookmarkNode(pdf, xmlNodes);
                if (!String.IsNullOrEmpty(errorMsg))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    return errorMsg;
                }

                res = Declares.PXCp_WriteDocumentW(pdf, outFile, Declares.PXCp_CreationDisposition.PXCp_CreationDisposition_Overwrite,
                    (Int32)Declares.PXCp_WriteDocFlag.PXCp_Write_Release);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                Declares.PXCp_ET_Finish(pdf);
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 添加指点节点的所有子节点。
        /// </summary>
        /// <param name="xn">节点。</param>
        /// <param name="xmlNodes">XML 节点集合。</param>
        /// <param name="level">节点的级别。</param>
        private void AddXmlNodes(XmlNode xn, ref Dictionary<Int32, XmlNode> xmlNodes, Int32 level)
        {
            Int32 flag = 0;
            foreach (XmlNode xnChild in xn.ChildNodes)
            {
                ((XmlElement)xnChild).SetAttribute("level", level.ToString());
                ((XmlElement)xnChild).SetAttribute("flag", (flag++).ToString());
                xmlNodes.Add(xmlNodes.Count, xnChild);
                this.AddXmlNodes(xnChild, ref xmlNodes, level + 1);
            }
        }

        /// <summary>
        /// 新增书签节点。
        /// </summary>
        /// <param name="pdf">PDF 文件句柄。</param>
        /// <param name="xmlNodes">XML 节点集合。</param>
        /// <returns>错误信息（空字符串表示没有错误）。</returns>
        private String AddBookmarkNode(Int32 pdf, Dictionary<Int32, XmlNode> xmlNodes)
        {
            Dictionary<Int32, String> fontNames = new Dictionary<Int32, String>();
            Dictionary<Int32, String> currentTitleFontNames = new Dictionary<Int32, String>();
            Declares.PXP_TextElement textElement = new Declares.PXP_TextElement();
            String currentTitle = String.Empty;
            String fontName;
            String errorMsg = String.Empty;
            Char[] charArr;
            Boolean isMatched;
            Boolean isMatchedWord;
            Double scaling = 1;
            XmlNode xn;
            Int32 len;
            Int32 pagesCount = 0;
            Int32 fontCount = 0;
            Int32 textElementCount = 0;
            Int32 res = Declares.PXCp_ET_Prepare(pdf);
            if (PXCp_Error.IS_DS_FAILED(res))
            {
                errorMsg = PXCp_Error.GetDSErrorString(res);
                return errorMsg;
            }

            res = Declares.PXCp_GetPagesCount(pdf, out pagesCount);
            if (PXCp_Error.IS_DS_FAILED(res))
            {
                errorMsg = PXCp_Error.GetDSErrorString(res);
                return errorMsg;
            }

            res = Declares.PXCp_ET_GetFontCount(pdf, out fontCount);
            if (PXCp_Error.IS_DS_FAILED(res))
            {
                errorMsg = PXCp_Error.GetDSErrorString(res);
                return errorMsg;
            }

            for (Int32 idx = 0; idx < fontCount; idx++)
            {
                len = Bookmarks.fontNameMaxLength;
                fontName = string.Empty.PadRight(Bookmarks.fontNameMaxLength, ' ');
                res = Declares.PXCp_ET_GetFontName(pdf, idx, fontName, ref len);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }
                else
                {
                    fontNames.Add(idx, fontName.Substring(0, len - 1));
                }
            }

            try
            {
                for (Int32 idx = 0; idx < pagesCount; idx++)
                {
                    res = Declares.PXCp_ET_AnalyzePageContent(pdf, idx);
                    if (PXCp_Error.IS_DS_FAILED(res))
                    {
                        errorMsg = PXCp_Error.GetDSErrorString(res);
                        return errorMsg;
                    }

                    res = Declares.PXCp_ET_GetElementCount(pdf, out textElementCount);
                    textElement.cbSize = Marshal.SizeOf(textElement);
                    currentTitle = String.Empty;
                    currentTitleFontNames.Clear();
                    for (Int32 idx2 = 0; idx2 < textElementCount; idx2++)
                    {
                        textElement.Count = 0;
                        textElement.mask = 0;
                        res = Declares.PXCp_ET_GetElement(pdf, idx2, ref textElement, 0);

                        if (!PXCp_Error.IS_DS_FAILED(res) && textElement.Count > 0)
                        {
                            charArr = new char[textElement.Count];
                            textElement.Characters = new string(charArr);
                            textElement.mask = (Int32)Declares.PXP_TextElementMask.PTEM_Text
                                + (Int32)Declares.PXP_TextElementMask.PTEM_Offsets + (Int32)Declares.PXP_TextElementMask.PTEM_Matrix
                                + (Int32)Declares.PXP_TextElementMask.PTEM_FontInfo + (Int32)Declares.PXP_TextElementMask.PTEM_TextParams;

                            res = Declares.PXCp_ET_GetElement(pdf, idx2, ref textElement, (Int32)Declares.PXP_GetTextElementFlags.GTEF_IgnorePageRotation);
                            if (!PXCp_Error.IS_DS_FAILED(res))
                            {
                                isMatched = false;
                                isMatchedWord = false;
                                currentTitle += Bookmarks.regSpace.Replace(textElement.Characters, "");

                                if (!currentTitleFontNames.ContainsKey(textElement.FontIndex))
                                {
                                    currentTitleFontNames.Add(textElement.FontIndex, fontNames[textElement.FontIndex]);
                                }

                                foreach (Int32 idx3 in xmlNodes.Keys)
                                {
                                    xn = xmlNodes[idx3];

                                    if (xn.Attributes["pageNumber"] == null)
                                    {
                                        if (xn.Attributes["bookMark"] != null && Bookmarks.regSpace.Replace(xn.Attributes["bookMark"].Value, "").IndexOf(currentTitle) != -1)
                                        {
                                            isMatched = true;
                                            if (xmlNodes.ContainsKey(0) && currentTitle == Bookmarks.regSpace.Replace(xn.Attributes["bookMark"].Value, ""))
                                            {
                                                isMatchedWord = true;
                                                xmlNodes.Remove(idx3);
                                                scaling = textElement.FontSize / Convert.ToDouble(xn.Attributes["fontSize"].Value);
                                                break;
                                            }

                                            if (currentTitle == Bookmarks.regSpace.Replace(xn.Attributes["bookMark"].Value, "")
                                                && currentTitleFontNames.ContainsValue(xn.Attributes["fontName"].Value) && (xn.Attributes["fontSize"] == null
                                                || Math.Round(textElement.FontSize / scaling) == Convert.ToDouble(xn.Attributes["fontSize"].Value)))
                                            {
                                                isMatchedWord = true;
                                                ((XmlElement)xn).SetAttribute("pageNumber", (idx + 1).ToString());
                                                this.AddBookmarkNodeDetails(xn, pdf, Convert.ToInt32(xn.Attributes["flag"].Value), Convert.ToInt32(xn.Attributes["level"].Value), textElement.Matrix.f + textElement.FontSize);

                                                if (xn.Attributes["pageId"] != null)
                                                {
                                                    xmlNodes.Remove(idx3);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        isMatched = true;
                                        isMatchedWord = true;
                                        ((XmlElement)xn).SetAttribute("pageNumber", xn.Attributes["pageNumber"].InnerText.Trim());
                                        this.AddBookmarkNodeDetails(xn, pdf, Convert.ToInt32(xn.Attributes["flag"].Value), Convert.ToInt32(xn.Attributes["level"].Value), 0);
                                        xmlNodes.Remove(idx3);
                                        break;
                                    }
                                }

                                // 前缀没有匹配或者已经完全匹配
                                if (!isMatched || isMatchedWord)
                                {
                                    currentTitle = String.Empty;
                                    currentTitleFontNames.Clear();
                                }
                            }
                        }
                    }
                }

                return String.Empty;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 新增书签节点明细。
        /// </summary>
        /// <param name="xmlNode">XML 节点。</param>
        /// <param name="pdf">PDF 文件句柄。</param>
        /// <param name="flag">节点类型标志。</param>
        /// <param name="level">节点级别。</param>
        /// <param name="top">新增的书签指向的 Y 坐标。</param>
        private void AddBookmarkNodeDetails(XmlNode xmlNode, Int32 pdf, Int32 flag, Int32 level, Double top)
        {
            Int32 res = 0;
            Int32 new_item = 0;

            Declares.PXCp_BMInfo bm_info = new Declares.PXCp_BMInfo();
            Declares.PXCp_BMDestination bm_dest = new Declares.PXCp_BMDestination();

            bm_info.cbSize = Marshal.SizeOf(bm_info);
            bm_info.Mask = (Int32)Declares.PXCp_BMInfoMask.BMIM_TitleW |
                (Int32)Declares.PXCp_BMInfoMask.BMIM_Open | (Int32)Declares.PXCp_BMInfoMask.BMIM_Style |
                (Int32)Declares.PXCp_BMInfoMask.BMIM_Color | (Int32)Declares.PXCp_BMInfoMask.BMIM_Destination;

            bm_dest.DestType = Declares.PXC_OutlineDestination.Dest_Y;
            bm_dest.Mask = 0;
            bm_dest.Left = Convert.ToDouble(0);
            bm_dest.Top = top;
            bm_dest.Right = Convert.ToDouble(0);
            bm_dest.Bottom = Convert.ToDouble(0);
            bm_dest.Zoom = Convert.ToDouble(0);
            bm_dest.PageNumber = Convert.ToInt32(xmlNode.Attributes["pageNumber"].Value) - 1;

            String title = xmlNode.Attributes["title"].Value;
            bm_info.TitleW = Marshal.StringToHGlobalUni(title);
            bm_info.LengthOfTitle = title.Length;
            bm_info.bOpen = 1;
            bm_info.Style = Declares.PXC_OutlineStyle.OutlineStyle_Normal;
            bm_info.Color = System.Drawing.Color.Black.ToArgb();
            bm_info.Destination = bm_dest;

            try
            {
                if (level == 0)
                {
                    if (flag == 0)
                    {
                        res = Declares.PXCp_BMInsertItem(pdf, 0, (Int32)Declares.PXCp_OutlinePos.PBM_ROOT, out new_item, ref bm_info);
                        ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());

                        flag++;
                    }
                    else
                    {
                        res = Declares.PXCp_BMInsertItem(pdf, 0, (Int32)Declares.PXCp_OutlinePos.PBM_LAST, out new_item, ref bm_info);
                        ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());
                    }
                }
                else
                {
                    res = Declares.PXCp_BMInsertItem(pdf, Convert.ToInt32(xmlNode.ParentNode.Attributes["pageId"].Value),
                        (Int32)Declares.PXCp_OutlinePos.PBM_LAST, out new_item, ref bm_info);
                    ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());
                }

                //Marshal.Release(bm_info.TitleW);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 打印书签。
        /// </summary>
        /// <param name="pdfFile">PDF 文件名。</param>
        /// <param name="outFile">增加书签后输出的 PDF 文件名。</param>
        /// <returns>错误信息（空字符串表示没有错误）。</returns>
        public String PrintBookmarks(String pdfFile, String outFile)
        {
            try
            {
                Int32 pdf = 0;
                String errorMsg = String.Empty;
                Int32 res = Declares.PXCp_Init(out pdf, Declares.g_RegKey, Declares.g_DevCode);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    if (pdf != 0)
                    {
                        Declares.PXCp_ET_Finish(pdf);
                    }

                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                res = Declares.PXCp_ReadDocumentW(pdf, pdfFile, 0);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                Int32 bmItem = 0;
                res = Declares.PXCp_GetRootBMItem(pdf, out bmItem);
                if (PXCp_Error.IS_DS_FAILED(res))
                {
                    Declares.PXCp_ET_Finish(pdf);
                    errorMsg = PXCp_Error.GetDSErrorString(res);
                    return errorMsg;
                }

                Declares.PXCp_BMInfo bm_info = new Declares.PXCp_BMInfo();
                Declares.PXCp_BMDestination bm_dest = new Declares.PXCp_BMDestination();

                bm_info.cbSize = Marshal.SizeOf(bm_info);
                bm_info.Mask = (Int32)Declares.PXCp_BMInfoMask.BMIM_TitleW |
                    (Int32)Declares.PXCp_BMInfoMask.BMIM_Open | (Int32)Declares.PXCp_BMInfoMask.BMIM_Style |
                    (Int32)Declares.PXCp_BMInfoMask.BMIM_Color | (Int32)Declares.PXCp_BMInfoMask.BMIM_Destination;

                bm_dest.DestType = Declares.PXC_OutlineDestination.Dest_Y;
                bm_dest.Mask = 0;
                bm_dest.Left = Convert.ToDouble(0);
                bm_dest.Top = 0;
                bm_dest.Right = Convert.ToDouble(0);
                bm_dest.Bottom = Convert.ToDouble(0);
                bm_dest.Zoom = Convert.ToDouble(0);
                bm_dest.PageNumber = 1;

                bm_info.bOpen = 1;
                bm_info.Style = Declares.PXC_OutlineStyle.OutlineStyle_Normal;
                bm_info.Color = System.Drawing.Color.Black.ToArgb();
                bm_info.Destination = bm_dest;

                //设置文档大小及边距
                Document document = new Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
                PdfWriter.getInstance(document, new FileStream(outFile, FileMode.Create));
                BaseFont bfChinese = BaseFont.createFont(Environment.GetEnvironmentVariable("WinDir") + "\\Fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fontChinese = new Font(bfChinese, 30, 1, new Color(0, 0, 0));

                document.Open();
                document.Add(new Paragraph("                 目录", fontChinese));
                fontChinese = new Font(bfChinese, 12, 0, new Color(0, 0, 0));
                this.PrintBookmark(pdf, bmItem, bm_info, String.Empty, document, fontChinese);
                document.Close();
                Declares.PXCp_ET_Finish(pdf);
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void PrintBookmark(Int32 pdf, Int32 bmItem, Declares.PXCp_BMInfo bm_info, String prefix, Document document, Font fontChinese)
        {
            String title = String.Empty.PadRight(Bookmarks.titleMaxLength, '\0');
            bm_info.TitleW = Marshal.StringToHGlobalUni(title);
            bm_info.LengthOfTitle = Bookmarks.titleMaxLength;
            Int32 res = Declares.PXCp_BMGetItemInfo(pdf, bmItem, ref bm_info);
            if (PXCp_Error.IS_DS_FAILED(res))
            {
                return;
            }

            //将书签打印到 PDF 文件
            title = Marshal.PtrToStringUni(bm_info.TitleW);
            title = prefix + title;
            String pageNumber = (bm_info.Destination.PageNumber + 1).ToString();
            title += String.Empty.PadRight(Bookmarks.titleMaxLength - Bookmarks.regDoubleByteChar.Replace(title, "**").Length - pageNumber.Length, '.');
            title += pageNumber;
            document.Add(new Paragraph(title, fontChinese));

            //获取子节点
            Int32 pbmItem;
            res = Declares.PXCp_BMGetItem(pdf, bmItem, Declares.PXCp_OutlinePos.PBM_CHILD, out pbmItem);
            if (!PXCp_Error.IS_DS_FAILED(res))
            {
                this.PrintBookmark(pdf, pbmItem, bm_info, prefix + "    ", document, fontChinese);
            }

            //获取下一个兄弟节点
            res = Declares.PXCp_BMGetItem(pdf, bmItem, Declares.PXCp_OutlinePos.PBM_NEXT, out pbmItem);
            if (!PXCp_Error.IS_DS_FAILED(res))
            {
                this.PrintBookmark(pdf, pbmItem, bm_info, prefix, document, fontChinese);
            }
        }

        /// <summary>
        /// 新增书签。
        /// </summary>
        public void AddBookmarks_HJ(XmlNode xmlNode, String pdfFile, String outFile)
        {
            Int32 flag = 0;
            Int32 m_pdf = 0;

            Int32 res = Declares.PXCp_Init(out m_pdf, Declares.g_RegKey, Declares.g_DevCode);
            res = Declares.PXCp_ReadDocumentW(m_pdf, pdfFile, 0);

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                AddBookmarkNode_HJ(childNode, m_pdf, flag, 0);

                flag++;
            }

            res = Declares.PXCp_WriteDocumentW(m_pdf, outFile, Declares.PXCp_CreationDisposition.PXCp_CreationDisposition_Overwrite, (int)Declares.PXCp_WriteDocFlag.PXCp_Write_Release);
            m_pdf = 0;
        }

        /// <summary>
        /// 新增书签节点。
        /// </summary>
        private void AddBookmarkNode_HJ(XmlNode xmlNode, Int32 m_pdf, Int32 flag, Int32 level)
        {
            int res = 0;
            Int32 new_item = 0;

            Declares.PXCp_BMInfo bm_info = new Declares.PXCp_BMInfo();
            Declares.PXCp_BMDestination bm_dest = new Declares.PXCp_BMDestination();

            bm_info.cbSize = Marshal.SizeOf(bm_info);
            bm_info.Mask = (int)Declares.PXCp_BMInfoMask.BMIM_TitleW |
                (int)Declares.PXCp_BMInfoMask.BMIM_Open | (int)Declares.PXCp_BMInfoMask.BMIM_Style |
                (int)Declares.PXCp_BMInfoMask.BMIM_Color | (int)Declares.PXCp_BMInfoMask.BMIM_Destination;

            bm_dest.DestType = Declares.PXC_OutlineDestination.Dest_Page;   //Dest_XYZ;
            bm_dest.Mask = 0;
            bm_dest.Left = Convert.ToDouble(0);
            bm_dest.Top = Convert.ToDouble(0);
            bm_dest.Right = Convert.ToDouble(0);
            bm_dest.Bottom = Convert.ToDouble(0);
            bm_dest.Zoom = Convert.ToDouble(0);
            bm_dest.PageNumber = Convert.ToInt32(xmlNode.Attributes["PageCount"].Value) - 1;

            String title = (String.IsNullOrEmpty(xmlNode.Attributes["Sequence"].Value) ? "" : (xmlNode.Attributes["Sequence"].Value + "、") + xmlNode.Attributes["XMItem"].Value);
            bm_info.TitleW = Marshal.StringToHGlobalUni(title);
            bm_info.LengthOfTitle = title.Length;
            bm_info.bOpen = 1;
            bm_info.Style = Declares.PXC_OutlineStyle.OutlineStyle_Normal;
            bm_info.Color = System.Drawing.Color.Black.ToArgb();
            bm_info.Destination = bm_dest;

            if (level == 0)
            {
                if (flag == 0)
                {
                    res = Declares.PXCp_BMInsertItem(m_pdf, 0, (int)Declares.PXCp_OutlinePos.PBM_ROOT, out new_item, ref bm_info);
                    ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());

                    flag++;
                }
                else
                {
                    res = Declares.PXCp_BMInsertItem(m_pdf, 0, (int)Declares.PXCp_OutlinePos.PBM_LAST, out new_item, ref bm_info);
                    ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());
                }
            }
            else
            {
                res = Declares.PXCp_BMInsertItem(m_pdf, Convert.ToInt32(xmlNode.ParentNode.Attributes["pageId"].Value), (int)Declares.PXCp_OutlinePos.PBM_LAST, out new_item, ref bm_info);
                ((XmlElement)xmlNode).SetAttribute("pageId", new_item.ToString());
            }
        }

        /// <summary>
        /// 新增书签。
        /// </summary>
        public void AddBookmarks_BJTL_SG(XmlNodeList xmlNodeList, String pdfFile, String outFile, String bidType)
        {
            Int32 flag = 0;
            Int32 m_pdf = 0;

            Int32 res = Declares.PXCp_Init(out m_pdf, Declares.g_RegKey, Declares.g_DevCode);
            res = Declares.PXCp_ReadDocumentW(m_pdf, pdfFile, 0);

            foreach (XmlNode childNode in xmlNodeList)
            {
                if (childNode.Attributes["Item"].Value == "已标价工程量清单" || childNode.Attributes["BidType"].Value != bidType) continue;

                AddBookmarkNode(childNode, m_pdf, flag, 0);

                flag++;
            }

            res = Declares.PXCp_WriteDocumentW(m_pdf, outFile, Declares.PXCp_CreationDisposition.PXCp_CreationDisposition_Overwrite, (int)Declares.PXCp_WriteDocFlag.PXCp_Write_Release);
            m_pdf = 0;
        }
    }
}