using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace MarkMars.Winform
{
    public class XmlManage
    {
        private string strFile;
        private XmlDocument doc = new XmlDocument();

        public XmlManage(string XmlFile)
        {
            try { doc.Load(XmlFile); }
            catch (System.Exception ex) { throw ex; }
            strFile = XmlFile;
        }
        private void LoadXml()
        {
            doc = new XmlDocument();
            doc.Load(strFile);
        }
        public string GetValue(string key)
        {
            LoadXml();
            XmlNodeList nodeList = doc.GetElementsByTagName(key);
            if (nodeList.Count == 0)
                return "notFound";
            else
            {
                XmlNode xNode = nodeList[0];
                return xNode.InnerText;
            }
        }
        public void SetValue(string key, string value)
        {
            if (GetValue(key) != "notFound")
            {
                LoadXml();
                XmlNodeList nodeList = doc.GetElementsByTagName(key);
                XmlNode xNode = nodeList[0];
                xNode.InnerText = value;
                SaveXml();
            }
        }
        public void AddNode(string key, string value)
        {
            if (GetValue(key) == "notFound")
            {
                LoadXml();
                XmlNodeList nodeList = doc.GetElementsByTagName("RegRoot");
                XmlNode xNode = nodeList[0];
                XmlElement xElement = doc.CreateElement(key);
                xElement.InnerText = value;
                xNode.AppendChild(xElement);
                SaveXml();
            }
        }
        public void DeleteNoteValue(string key)
        {
            if (GetValue(key) != "notFound")
            {
                LoadXml();
                XmlNodeList parentNodeList = doc.GetElementsByTagName("RegRoot");
                XmlNode parentNode = parentNodeList[0];
                XmlNodeList nodeList = doc.GetElementsByTagName(key);
                XmlNode node = nodeList[0];
                parentNode.RemoveChild(node);
                SaveXml();
            }
        }
        private void SaveXml()
        {
            XmlTextWriter xw = new XmlTextWriter(new StreamWriter(strFile));
            xw.Formatting = Formatting.Indented;
            doc.WriteTo(xw);
            xw.Close();
        }
    }
}
