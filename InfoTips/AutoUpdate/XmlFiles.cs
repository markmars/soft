using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AutoUpdate
{
    /// <summary>
    /// XmlFiles 的摘要说明。
    /// </summary>
    public class XmlFiles : XmlDocument
    {
        public string XmlFileName { get; set; }

        public XmlFiles(string xmlFile)
        {
            XmlFileName = xmlFile;
            try
            {
                this.Load(xmlFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("加载配置文件失败!" + e.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        /// <summary>
        /// 给定一个节点的xPath表达式并返回一个节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public XmlNode FindNode(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            return xmlNode;
        }
        /// <summary>
        /// 给定一个节点的xPath表达式返回其值
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public string GetNodeValue(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            return xmlNode.InnerText;
        }
        /// <summary>
        /// 给定一个节点的表达式返回此节点下的孩子节点列表
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public XmlNodeList GetNodeList(string xPath)
        {
            XmlNodeList nodeList = this.SelectSingleNode(xPath).ChildNodes;
            return nodeList;
        }
    }
}
