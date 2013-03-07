using System;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Xml;

namespace MarkMars.Common
{
    public class HTMLFile
    {
        public void WebBrowserNavigate(WebBrowser webBrowser, String htmlFilePath)
        {
            webBrowser.Navigate(htmlFilePath);
        }

        public void Save(WebBrowser webBrowser, String htmlFilePath)
        {
            FileStream fileStream = null;

            try
            {
                HtmlElementCollection elems = webBrowser.Document.GetElementsByTagName("HTML");
                StringBuilder fileContext = new StringBuilder();
                fileContext.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
                fileContext.Append(elems[0].OuterHtml.ToString());
                
                if (File.Exists(htmlFilePath))
                {
                    FileInfo fileInfo = new FileInfo(htmlFilePath);
                    if (fileInfo.Attributes != FileAttributes.Normal)
                    {
                        fileInfo.Attributes = FileAttributes.Normal;
                    }

                    File.Delete(htmlFilePath);
                }

                fileStream = new FileStream(htmlFilePath, FileMode.CreateNew, FileAccess.Write);
                Byte[] bytes = Encoding.UTF8.GetBytes(fileContext.ToString());
                fileStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        #region 绑定数据到WebBrowser
        /// <summary>
        /// 绑定数据到WebBrowser
        /// </summary>
        public static void BindDataToWebBrowser(WebBrowser webBrowser, String inputXmlName)
        {
            HtmlElement htmlElement = null;
            XmlDocument xmlDoc = null;

            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(inputXmlName);
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)//遍历所有子节点     
                {
                    htmlElement = webBrowser.Document.GetElementById(xmlNode.Attributes["id"].Value);
                    if (htmlElement == null)
                    {
                        continue;
                    }

                    switch (xmlNode.Attributes["type"].Value)
                    {
                        case "input":
                        case "text":
                        case "select-one":
                        case "textarea":
                            htmlElement.SetAttribute("value", xmlNode.Attributes["value"].Value.Trim());
                            if (xmlNode.Attributes["style"] != null)
                            {
                                htmlElement.Style = xmlNode.Attributes["style"].Value.ToString();
                            }
                            else
                            {
                                if (xmlNode.Attributes["width"] != null)
                                {
                                    htmlElement.Style += String.Format("; WIDTH: {0}px", xmlNode.Attributes["width"].Value);
                                }

                                if (xmlNode.Attributes["height"] != null)
                                {
                                    htmlElement.Style += String.Format("; HEIGHT: {0}px", xmlNode.Attributes["height"].Value);
                                }
                            }
                            break;
                        case "checkbox":
                            if (xmlNode.Attributes["checked"] != null && xmlNode.Attributes["checked"].Value.ToLower() == "true")
                            {
                                htmlElement.SetAttribute("checked", "checked");
                            }
                            else
                            {
                                htmlElement.SetAttribute("checked", "");
                            }
                            break;
                        case "div":
                            htmlElement.InnerText = xmlNode.Attributes["value"].Value.Trim();
                            if (xmlNode.Attributes["style"] != null)
                            {
                                htmlElement.Style = xmlNode.Attributes["style"].Value.ToString();
                            }
                            else
                            {
                                if (xmlNode.Attributes["width"] != null)
                                {
                                    htmlElement.Style += String.Format("; WIDTH: {0}px", xmlNode.Attributes["width"].Value);
                                }

                                if (xmlNode.Attributes["height"] != null)
                                {
                                    htmlElement.Style += String.Format("; HEIGHT: {0}px", xmlNode.Attributes["height"].Value);
                                }
                            }
                            break;
                        default:
                            if ("span".Equals(htmlElement.GetAttribute("tagName").ToLower()))
                            {
                                htmlElement.SetAttribute("innerText", xmlNode.Attributes["value"].Value.Trim());
                                if (xmlNode.Attributes["style"] != null)
                                {
                                    htmlElement.Style = xmlNode.Attributes["style"].Value.ToString();
                                }
                                else
                                {
                                    if (xmlNode.Attributes["width"] != null)
                                    {
                                        htmlElement.Style += String.Format("; WIDTH: {0}px", xmlNode.Attributes["width"].Value);
                                    }

                                    if (xmlNode.Attributes["height"] != null)
                                    {
                                        htmlElement.Style += String.Format("; HEIGHT: {0}px", xmlNode.Attributes["height"].Value);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
