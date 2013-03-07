using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Data;
using System.Text;
using System.Globalization;

namespace MarkMars.UI
{
    public class XmlFile
    {
        #region 绑定Xml文件到树形控件
        /// <summary>
        /// 绑定Xml文件到树形控件。
        /// </summary>
        /// <param name="treeView">树形控件。</param>
        /// <param name="xmlFilePath">Xml文件。</param>
        /// <param name="pbFF">评标方法。</param>
        public void BindXml2TreeView(TreeView treeView, String xmlFilePath, Int32 pbFF)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            //TreeNodeCollection treeNodeCollection = treeView.Nodes;
            //switch (treeView.Name)
            //{
            //    case "tvPrecept":
            //        treeNodeCollection = treeView.Nodes.Add("tnPrecept", "方案标").Nodes;                    
            //        break;
            //    case "tvQualify":
            //        treeNodeCollection = treeView.Nodes.Add("tnQualify", "资信标").Nodes;
            //        break;
            //    case "tvTBLetter":
            //        treeNodeCollection = treeView.Nodes.Add("tnTBLetter", "投标函格式").Nodes;
            //        break;
            //}
            this.BindXmlNode2TreeNode(xmlNodeList, treeView.Nodes, pbFF);
            //this.BindXmlNode2TreeNode(xmlNodeList, treeNodeCollection, pbFF);

            treeView.EndUpdate();
        }

        /// <summary>
        /// 绑定Xml节点到树形控件节点。
        /// </summary>
        /// <param name="xmlNodeList">Xml节点集合。</param>
        /// <param name="treeNodeCollection">树形控件节点对象。</param>
        /// <param name="pbFF">评标方法。</param>
        private void BindXmlNode2TreeNode(XmlNodeList xmlNodeList, TreeNodeCollection treeNodeCollection, Int32 pbFF)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                TreeNode newTreeNode = new TreeNode();
                if (xmlNode.Name.ToLower() == "projinfo")
                {
                    continue;
                }

                String strValue = this.TransferToHtmlEntity(xmlNode.Attributes[0].Value);
                if ((pbFF == 1 && strValue == "m3") || (pbFF == 2 && strValue == "m2"))
                {
                    continue;
                }


                String strText = xmlNode.Name.ToLower().Equals("zbitem") == true ? ((String.IsNullOrEmpty(xmlNode.Attributes[0].Value) ?
                    "" : (this.TransferToHtmlEntity(xmlNode.Attributes[0].Value.TrimEnd(new Char[]{'、'})) + "、")) +
                    (String.IsNullOrEmpty(xmlNode.Attributes[1].Value) ? this.TransferToHtmlEntity(xmlNode.Attributes[2].Value) : 
                    this.TransferToHtmlEntity(xmlNode.Attributes[1].Value))) : this.TransferToHtmlEntity(xmlNode.Attributes[1].Value);
                newTreeNode.Text = strText;
                newTreeNode.Tag = xmlNode;
                if (xmlNode.HasChildNodes)
                {
                    this.BindXmlNode2TreeNode(xmlNode.ChildNodes, newTreeNode.Nodes, pbFF);
                }

                treeNodeCollection.Add(newTreeNode);
            }
        }

        /// <summary>
        /// 绑定Xml文件到树形控件。
        /// </summary>
        /// <param name="treeView">树形控件。</param>
        /// <param name="xmlFilePath">Xml文件。</param>
        /// <param name="pbFF">评标方法。</param>
        public void BindXml2TreeView(TreeView treeView, String xmlFilePath, ImageList imageList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            treeView.ImageList = imageList;
            this.BindXmlNode2TreeNode(xmlNodeList, treeView.Nodes, imageList);
            
            treeView.EndUpdate();
        }

        /// <summary>
        /// 绑定Xml节点到树形控件节点。
        /// </summary>
        /// <param name="xmlNodeList">Xml节点集合。</param>
        /// <param name="treeNodeCollection">树形控件节点对象。</param>
        /// <param name="pbFF">评标方法。</param>
        private void BindXmlNode2TreeNode(XmlNodeList xmlNodeList, TreeNodeCollection treeNodeCollection, ImageList imageList)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                TreeNode newTreeNode = new TreeNode();
                if (xmlNode.Name.ToLower() == "projinfo")
                {
                    continue;
                }

                String strValue = this.TransferToHtmlEntity(xmlNode.Attributes[0].Value);

                String strText = xmlNode.Name.ToLower().Equals("zbitem") == true ? ((String.IsNullOrEmpty(xmlNode.Attributes[0].Value) ?
                    "" : (this.TransferToHtmlEntity(xmlNode.Attributes[0].Value.TrimEnd(new Char[] { '、' })) + "、")) +
                    (String.IsNullOrEmpty(xmlNode.Attributes[1].Value) ? this.TransferToHtmlEntity(xmlNode.Attributes[2].Value) :
                    this.TransferToHtmlEntity(xmlNode.Attributes[1].Value))) : this.TransferToHtmlEntity(xmlNode.Attributes[1].Value);
                newTreeNode.Text = strText;
                newTreeNode.Tag = xmlNode;
                if (xmlNode.HasChildNodes)
                {
                    newTreeNode.ImageIndex = 0;
                    if (newTreeNode.IsExpanded)
                    {
                        newTreeNode.SelectedImageIndex = 2;
                    }
                    else
                    {
                        newTreeNode.SelectedImageIndex = 0;
                    }
                    this.BindXmlNode2TreeNode(xmlNode.ChildNodes, newTreeNode.Nodes, imageList);
                }
                else
                {
                    newTreeNode.ImageIndex = 1;
                    newTreeNode.SelectedImageIndex = 1;
                }

                treeNodeCollection.Add(newTreeNode);
            }
        }

        /// <summary>
        /// 绑定XML文件中指定节点到树形控件
        /// </summary>        
        public void BindAssignXmlNode2TreeView(String xmlFilePath, TreeView treeView, ArrayList treeNodeIds)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            TreeNodeCollection treeNodeCollection = treeView.Nodes;

            this.BindAssignXmlNode2TreeNode(xmlNodeList, treeNodeCollection, treeNodeIds);

            treeView.EndUpdate();
        }

        private void BindAssignXmlNode2TreeNode(XmlNodeList xmlNodeList, TreeNodeCollection treeNodeCollection, ArrayList treeNodeIds)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                TreeNode newTreeNode = new TreeNode();
                if (xmlNode.Name.ToLower() == "projinfo")
                {
                    continue;
                }

                if (!treeNodeIds.Contains(xmlNode.Attributes["id"].Value))
                {
                    String strText = xmlNode.Attributes[1].Value;
                    newTreeNode.Text = strText;
                    newTreeNode.Tag = xmlNode;
                    if (xmlNode.HasChildNodes)
                    {
                        this.BindAssignXmlNode2TreeNode(xmlNode.ChildNodes, newTreeNode.Nodes, treeNodeIds);
                    }

                    treeNodeCollection.Add(newTreeNode);
                }
            }
        }

        /// <summary>
        /// 绑定XML文件中指定节点到树形控件
        /// </summary>        
        public void BindAssignXmlNode2TreeView(String xmlFilePath, TreeView treeView, ArrayList treeNodeIds, ImageList imageList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            TreeNodeCollection treeNodeCollection = treeView.Nodes;
            treeView.ImageList = imageList;
            this.BindAssignXmlNode2TreeNode(xmlNodeList, treeNodeCollection, treeNodeIds, imageList);

            treeView.EndUpdate();
        }

        private void BindAssignXmlNode2TreeNode(XmlNodeList xmlNodeList, TreeNodeCollection treeNodeCollection, ArrayList treeNodeIds, ImageList imageList)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                TreeNode newTreeNode = new TreeNode();
                if (xmlNode.Name.ToLower() == "projinfo")
                {
                    continue;
                }

                if (!treeNodeIds.Contains(xmlNode.Attributes["id"].Value))
                {
                    String strText = xmlNode.Attributes[1].Value;
                    newTreeNode.Text = strText;
                    newTreeNode.Tag = xmlNode;
                    if (xmlNode.HasChildNodes)
                    {
                        newTreeNode.ImageIndex = 0;
                        if (newTreeNode.IsExpanded)
                        {
                            newTreeNode.SelectedImageIndex = 2;
                        }
                        else
                        {
                            newTreeNode.SelectedImageIndex = 0;
                        }

                        this.BindAssignXmlNode2TreeNode(xmlNode.ChildNodes, newTreeNode.Nodes, treeNodeIds, imageList);
                    }
                    else
                    {
                        newTreeNode.ImageIndex = 1;
                        newTreeNode.SelectedImageIndex = 1;
                    }

                    treeNodeCollection.Add(newTreeNode);
                }
            }
        }

        /// <summary>
        /// 绑定Xml文件到树形控件。
        /// </summary>
        /// <param name="treeView">树形控件。</param>
        /// <param name="list">列表。</param>
        public void BindList2TreeView(TreeView treeView, List<CompareInfo> list)
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            foreach (CompareInfo compareInfo in list)
            {
                TreeNode newTreeNode = new TreeNode();
                newTreeNode.Text = compareInfo.Content;
                newTreeNode.Tag = compareInfo.Id;

                treeView.Nodes.Add(newTreeNode);
            }

            treeView.EndUpdate();
        }
        #endregion

        #region 绑定Xml文件到树形TreeGridView控件
        /// <summary>
        /// 绑定Xml文件到树形DataGridView控件。
        /// </summary>
        /// <param name="treeGridView">树形控件。</param>
        /// <param name="xmlFilePath">Xml文件。</param>
        public void BindXml2TreeGridView(TreeGridView treeGridView, String xmlFilePath, Boolean isExpand, EvalType evalType)
        {
            if (!File.Exists(xmlFilePath))
            {
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            treeGridView.Nodes.Clear();
            this.BindXmlNode2TreeGridNode(xmlDocument.DocumentElement.ChildNodes, treeGridView.Nodes, evalType);
            if (isExpand)
            {
                foreach (TreeGridNode tgn in treeGridView.Nodes)
                {
                    if (!tgn.IsExpanded)
                    {
                        treeGridView.ExpandNode(tgn);
                    }
                }
            }
        }

        /// <summary>
        /// 绑定Xml节点到树形控件节点。
        /// </summary>
        /// <param name="xmlNodeList">Xml节点集合。</param>
        /// <param name="treeGridNodeCollection">树形控件节点对象。</param>
        private void BindXmlNode2TreeGridNode(XmlNodeList xmlNodeList, TreeGridNodeCollection treeGridNodeCollection, EvalType evalType)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                TreeGridNode newTreeGridNode = null;

                switch (evalType)
                {
                    case EvalType.TechBidEval:
                        newTreeGridNode = treeGridNodeCollection.Add(this.TransferToHtmlEntity(xmlNode.Attributes["Sequence"].Value), this.TransferToHtmlEntity(xmlNode.Attributes["Item"].Value),
                        this.TransferToHtmlEntity(xmlNode.Attributes["Content"].Value), xmlNode.Attributes["Score"].Value, Convert.ToBoolean(xmlNode.Attributes["Qualified"].Value));
                        break;
                    case EvalType.PreceptEval:
                        newTreeGridNode = treeGridNodeCollection.Add(this.TransferToHtmlEntity(xmlNode.Attributes["Sequence"].Value), this.TransferToHtmlEntity(xmlNode.Attributes["Item"].Value),
                        this.TransferToHtmlEntity(xmlNode.Attributes["Content"].Value), xmlNode.Attributes["Score"].Value, this.TransferToHtmlEntity(xmlNode.Attributes["VoteValue"].Value),
                        this.TransferToHtmlEntity(xmlNode.Attributes["Standard"].Value), Convert.ToBoolean(xmlNode.Attributes["Qualified"].Value),
                        xmlNode.Attributes["PageCount"].Value, xmlNode.Attributes["Ord"].Value);
                        break;
                    case EvalType.OtherEval:
                        newTreeGridNode = treeGridNodeCollection.Add(this.TransferToHtmlEntity(xmlNode.Attributes["Sequence"].Value), this.TransferToHtmlEntity(xmlNode.Attributes["Item"].Value),
                        this.TransferToHtmlEntity(xmlNode.Attributes["Content"].Value), xmlNode.Attributes["Score"].Value);
                        break;
                    case EvalType.SynthesisEval:
                        newTreeGridNode = treeGridNodeCollection.Add(this.TransferToHtmlEntity(xmlNode.Attributes["Sequence"].Value), this.TransferToHtmlEntity(xmlNode.Attributes["Item"].Value),
                        this.TransferToHtmlEntity(xmlNode.Attributes["Content"].Value), xmlNode.Attributes["Score"].Value, this.TransferToHtmlEntity(xmlNode.Attributes["VoteValue"].Value),
                        this.TransferToHtmlEntity(xmlNode.Attributes["Standard"].Value), Convert.ToBoolean(xmlNode.Attributes["Qualified"].Value),
                        xmlNode.Attributes["PageCount"].Value, xmlNode.Attributes["Ord"].Value, Convert.ToBoolean(xmlNode.Attributes["IsDecItem"].Value));
                        break;
                    case EvalType.DesignEval:
                        newTreeGridNode = treeGridNodeCollection.Add(this.TransferToHtmlEntity(xmlNode.Attributes["Sequence"].Value), this.TransferToHtmlEntity(xmlNode.Attributes["Item"].Value),
                        this.TransferToHtmlEntity(xmlNode.Attributes["Content"].Value), xmlNode.Attributes["Score"].Value, xmlNode.Attributes["ReasonableScore"].Value,
                        xmlNode.Attributes["PageCount"].Value, xmlNode.Attributes["Ord"].Value);
                        break;
                    case EvalType.AgenciesEval:
                        newTreeGridNode = treeGridNodeCollection.Add(this.TransferToHtmlEntity(xmlNode.Attributes["Sequence"].Value), this.TransferToHtmlEntity(xmlNode.Attributes["Item"].Value),
                        this.TransferToHtmlEntity(xmlNode.Attributes["Content"].Value), xmlNode.Attributes["Score"].Value, Convert.ToBoolean(xmlNode.Attributes["Qualified"].Value),
                        xmlNode.Attributes["PageCount"].Value, xmlNode.Attributes["Ord"].Value);
                        break;
                }
                newTreeGridNode.Tag = xmlNode;

                if (xmlNode.HasChildNodes)
                {
                    this.BindXmlNode2TreeGridNode(xmlNode.ChildNodes, newTreeGridNode.Nodes, evalType);
                }
            }
        }

        public void BindXml2TreeGridView(TreeGridView treeGridView, String xmlFilePath, Boolean isExpand)
        {
            if (!File.Exists(xmlFilePath))
            {
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            treeGridView.Nodes.Clear();

            Dictionary<String, Type> lstColumns = new Dictionary<String, Type>();
            for (Int32 i = 0; i < treeGridView.Columns.Count; i++)
            {
                if (treeGridView.Columns[i].GetType() == typeof(DataGridViewButtonColumn))
                {
                    continue;
                }

                lstColumns.Add(treeGridView.Columns[i].Name.Split('_')[1], treeGridView.Columns[i].GetType());
            }

            this.BindXmlNode2TreeGridNode(xmlDocument.DocumentElement.ChildNodes, treeGridView.Nodes, lstColumns);
            if (isExpand)
            {
                foreach (TreeGridNode tgn in treeGridView.Nodes)
                {
                    if (!tgn.IsExpanded)
                    {
                        treeGridView.ExpandNode(tgn);
                    }

                    if (tgn.HasChildren)
                    {
                        foreach (TreeGridNode childNode in tgn.Nodes)
                        {
                            if (!childNode.IsExpanded)
                            {
                                childNode.Expand();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绑定Xml节点到树形控件节点。
        /// </summary>
        /// <param name="xmlNodeList">Xml节点集合。</param>
        /// <param name="treeGridNodeCollection">树形控件节点对象。</param>
        private void BindXmlNode2TreeGridNode(XmlNodeList xmlNodeList, TreeGridNodeCollection treeGridNodeCollection, Dictionary<String, Type> lstColumns)
        {
            String attValue = String.Empty;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                TreeGridNode newTreeGridNode = null;
                Object[] treeNodes = new Object[lstColumns.Count];
                Int32 i = 0;
                foreach(String strKey in lstColumns.Keys)
                {
                    if (lstColumns[strKey] == typeof(DataGridViewCheckBoxColumn))
                    {
                        if (xmlNode.Attributes[strKey] != null)
                        {
                            attValue = this.TransferToHtmlEntity(xmlNode.Attributes[strKey].Value);
                        }
                        else
                        {
                            if (strKey.ToUpper() == "ISOPEN")
                            {
                                attValue = "True";
                            }
                            else
                            {
                                attValue = "False";
                            }
                        }

                        if (attValue == "True" || attValue == "False")
                        {
                            treeNodes.SetValue(attValue, i);
                        }
                        else
                        {
                            treeNodes.SetValue("False", i);
                        }
                    }
                    else
                    {
                        attValue = xmlNode.Attributes[strKey] != null ? this.TransferToHtmlEntity(xmlNode.Attributes[strKey].Value) : String.Empty;
                        treeNodes.SetValue(attValue, i);
                    }

                    i++;
                }


                //for (Int32 i = 0; i < lstColumns.Count; i++)
                //{
                //    if (xmlNode.Attributes[lstColumns.] != null)
                //    {
                //        attValue = this.TransferToHtmlEntity(xmlNode.Attributes[lstColumns[i]].Value);

                //        if (lstColumns[i] == "Qualified")
                //        {
                //            if (attValue == "True" || attValue == "False")
                //            {
                //                treeNodes.SetValue(attValue, i);
                //            }
                //            else
                //            {
                //                treeNodes.SetValue("False", i);
                //            }
                //        }
                //        else
                //        {
                //            treeNodes.SetValue(attValue, i);
                //        }
                //    }
                //    else
                //    {
                //        if (lstColumns[i] == "Qualified")
                //        { 
                //        }
                //        else
                //        {
                //            treeNodes.SetValue(String.Empty, i);
                //        }
                //    }
                //}
                newTreeGridNode = treeGridNodeCollection.Add(treeNodes);

                if (xmlNode.HasChildNodes)
                {
                    this.BindXmlNode2TreeGridNode(xmlNode.ChildNodes, newTreeGridNode.Nodes, lstColumns);
                }
            }
        }

        /// <summary>
        /// (郑州施工)绑定Xml节点到树形控件节点。
        /// </summary>
        public void BindXml2TreeGridView_ZZ_SG(TreeGridView treeGridView, String xmlFilePath, Boolean isExpand)
        {
            if (!File.Exists(xmlFilePath))
            {
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            treeGridView.Nodes.Clear();

            String columnName = "colTech_IsOpen";
            Dictionary<String, Type> lstColumns = new Dictionary<String, Type>();
            for (Int32 i = 0; i < treeGridView.Columns.Count; i++)
            {
                if (treeGridView.Columns[i].GetType() == typeof(DataGridViewButtonColumn))
                {
                    continue;
                }

                lstColumns.Add(treeGridView.Columns[i].Name.Split('_')[1], treeGridView.Columns[i].GetType());
                if (treeGridView.Columns[i].Name.Split('_')[1] == "IsOpen")
                {
                    columnName = treeGridView.Columns[i].Name;
                }
            }

            this.BindXmlNode2TreeGridNode(xmlDocument.DocumentElement.ChildNodes, treeGridView.Nodes, lstColumns);

            DataGridViewRow dgvr = null;
            foreach (TreeGridNode treeNode in treeGridView.Nodes)
            {
                if (treeNode.HasChildren)
                {
                    dgvr = treeGridView.Rows[treeNode.RowIndex];
                    dgvr.Cells[columnName] = new DataGridViewTextBoxCell();
                    dgvr.Cells[columnName].Style.BackColor = System.Drawing.SystemColors.Window;
                    dgvr.Cells[columnName].Value = String.Empty;
                    dgvr.ReadOnly = true;
                }
            }

            if (isExpand)
            {
                foreach (TreeGridNode tgn in treeGridView.Nodes)
                {
                    if (!tgn.IsExpanded)
                    {
                        treeGridView.ExpandNode(tgn);
                    }
                }
            }
        }
        #endregion

        #region 绑定Xml文件到一个对象中
        public void BindXml2Object(String xmlFilePath, Object objectInfo)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("TechBid").ChildNodes;
            PropertyInfo pi = null;
            Object propertyValue = null;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                pi = objectInfo.GetType().GetProperty(xmlNode.Name);
                if (pi != null)
                {
                    propertyValue = Convert.ChangeType(xmlNode.InnerText, pi.PropertyType);
                    pi.SetValue(objectInfo, propertyValue, null);
                }
            }
        }

        public void BindXml2Object(String xmlFilePath, Object objectInfo, String rootName)
        {
            if (!File.Exists(xmlFilePath))
            {
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode(rootName).ChildNodes;
            PropertyInfo pi = null;
            Object propertyValue = null;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                pi = objectInfo.GetType().GetProperty(xmlNode.Name);
                propertyValue = Convert.ChangeType(xmlNode.InnerText, pi.PropertyType);
                pi.SetValue(objectInfo, propertyValue, null);
            }
        }
        #endregion

        #region 保存Xml文件
        #region 保存类实例对象到Xml文件
        /// <summary>
        /// 保存类实例对象到Xml文件。
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="xmlNodeValue"></param>
        public void SaveXml(String xmlFilePath, object xmlNodeValue)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");

            XmlNode rootNode = xmlDoc.SelectSingleNode("TechBid");

            PropertyInfo[] lstPropertyInfo = xmlNodeValue.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in lstPropertyInfo)
            {
                XmlElement nodeElement = xmlDoc.CreateElement(propertyInfo.Name);
                object nodeValue = propertyInfo.GetValue(xmlNodeValue, null);
                nodeElement.InnerText = nodeValue == null ? "" : this.TransferToSafeXmlString(nodeValue.ToString());
                rootNode.AppendChild(nodeElement);
            }

            xmlDoc.Save(xmlFilePath);
        }
        #endregion

        #region 保存字符串到Xml文件
        public void SaveStringXml(String xmlFilePath, Dictionary<String, String> dictionary)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");

            XmlNode rootNode = xmlDoc.SelectSingleNode("TechBid");

            foreach (var item in dictionary)
            {
                XmlElement nodeElement = xmlDoc.CreateElement(item.Key);
                nodeElement.InnerText = this.TranferToXmlEntity(item.Value);
                rootNode.AppendChild(nodeElement);
            }

            xmlDoc.Save(xmlFilePath);
        }

        public void InsertStringXml(String xmlFilePath, Dictionary<String, String> dictionary)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode;
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                xmlDoc.Load(xmlFilePath);
                rootNode = xmlDoc.SelectSingleNode("TechBid");
                foreach (var item in dictionary)
                {
                    XmlNode xn = xmlDoc.SelectSingleNode("TechBid/" + item.Key);
                    if (xn != null)
                    {
                        rootNode.RemoveChild(xn);
                    }

                    XmlElement nodeElement = xmlDoc.CreateElement(item.Key);
                    nodeElement.InnerText = this.TranferToXmlEntity(item.Value);
                    rootNode.AppendChild(nodeElement);                    
                }
            }
            else
            {                
                xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");
                rootNode = xmlDoc.SelectSingleNode("TechBid");

                foreach (var item in dictionary)
                {
                    XmlElement nodeElement = xmlDoc.CreateElement(item.Key);
                    nodeElement.InnerText = this.TranferToXmlEntity(item.Value);
                    rootNode.AppendChild(nodeElement);
                }
            }

            xmlDoc.Save(xmlFilePath);
        }
        #endregion

        #region 保存TreeGridView中的数据到Xml文件中
        /// <summary>
        /// 保存TreeGridView中的数据到Xml文件中。
        /// </summary>
        /// <param name="treeGridView">TreeGridView控件。</param>
        /// <param name="xmlFilePath">Xml文件。</param>
        public void SaveTreeGridView2Xml(TreeGridView treeGridView, String xmlFilePath, EvalType evalType)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");

            XmlNode rootNode = xmlDoc.SelectSingleNode("TechBid");

            Int32 ord = 1;

            WriteXmlNode(rootNode, treeGridView.Nodes, xmlDoc, evalType, ref ord);

            xmlDoc.Save(xmlFilePath);
        }

        /// <summary>
        /// 把树形控件写入到Xml节点。
        /// </summary>
        /// <param name="xmlNode">Xml节点。</param>
        /// <param name="treeGridNodeCollection">树形节点集合。</param>
        private void WriteXmlNode(XmlNode xmlNode, TreeGridNodeCollection treeGridNodeCollection, XmlDocument xmlDoc, EvalType evalType, ref Int32 ord)
        {
            foreach (TreeGridNode treeGridNode in treeGridNodeCollection)
            {
                XmlElement nodeElement = xmlDoc.CreateElement("ZBItem");

                nodeElement.SetAttribute("Sequence", this.TranferToXmlEntity(treeGridNode.Cells[0].FormattedValue.ToString()));
                nodeElement.SetAttribute("Item", this.TranferToXmlEntity(treeGridNode.Cells[1].FormattedValue.ToString()));
                nodeElement.SetAttribute("Content", this.TranferToXmlEntity(treeGridNode.Cells[2].FormattedValue.ToString()));
                nodeElement.SetAttribute("Score", treeGridNode.Cells[3].FormattedValue.ToString());
                nodeElement.SetAttribute("Ord", ord.ToString());
                nodeElement.SetAttribute("PageCount", "");
                ord++;
                switch (evalType)
                {
                    case EvalType.TechBidEval:
                        nodeElement.SetAttribute("Standard", treeGridNode.Tag == null ? "" : this.TranferToXmlEntity(((XmlNode)treeGridNode.Tag).Attributes["Standard"].Value));
                        nodeElement.SetAttribute("Qualified", this.TranferToXmlEntity(treeGridNode.Cells[4].FormattedValue.ToString()));
                        break;
                    case EvalType.PreceptEval:
                        nodeElement.SetAttribute("Standard", this.TranferToXmlEntity(treeGridNode.Cells[5].FormattedValue.ToString()));
                        nodeElement.SetAttribute("Qualified", this.TranferToXmlEntity(treeGridNode.Cells[6].FormattedValue.ToString()));
                        nodeElement.SetAttribute("VoteValue", this.TranferToXmlEntity(treeGridNode.Cells[4].FormattedValue.ToString()));
                        break;
                    case EvalType.OtherEval:
                        break;
                    case EvalType.SynthesisEval:
                        nodeElement.SetAttribute("VoteValue", this.TranferToXmlEntity(treeGridNode.Cells[4].FormattedValue.ToString()));
                        nodeElement.SetAttribute("Standard", this.TranferToXmlEntity(treeGridNode.Cells[5].FormattedValue.ToString()));
                        nodeElement.SetAttribute("Qualified", this.TranferToXmlEntity(treeGridNode.Cells[6].FormattedValue.ToString()));
                        nodeElement.SetAttribute("IsDecItem", treeGridNode.Cells[9].FormattedValue.ToString());
                        break;
                }

                xmlNode.AppendChild(nodeElement);

                if (treeGridNode.HasChildren)
                {
                    //2012-03-29 Youker 如果节点收缩状态，保存会出错
                    treeGridNode.Expand();

                    this.WriteXmlNode(nodeElement, treeGridNode.Nodes, xmlDoc, evalType, ref ord);
                }
            }
        }

        /// <summary>
        /// 保存TreeGridView中的数据到Xml文件中。
        /// </summary>
        /// <param name="treeGridView">TreeGridView控件。</param>
        /// <param name="xmlFilePath">Xml文件。</param>
        public void SaveTreeGridView2Xml(TreeGridView treeGridView, String xmlFilePath)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");

            XmlNode rootNode = xmlDoc.SelectSingleNode("TechBid");

            Int32 ord = 1;

            WriteXmlNode(rootNode, treeGridView.Nodes, xmlDoc, ref ord);

            xmlDoc.Save(xmlFilePath);
        }

        /// <summary>
        /// 把树形控件写入到Xml节点。
        /// </summary>
        /// <param name="xmlNode">Xml节点。</param>
        /// <param name="treeGridNodeCollection">树形节点集合。</param>
        private void WriteXmlNode(XmlNode xmlNode, TreeGridNodeCollection treeGridNodeCollection, XmlDocument xmlDoc, ref Int32 ord)
        {
            foreach (TreeGridNode treeGridNode in treeGridNodeCollection)
            {
                XmlElement nodeElement = xmlDoc.CreateElement("ZBItem");

                foreach (DataGridViewCell dgvCell in treeGridNode.Cells)
                {
                    if (dgvCell.OwningColumn.GetType() == typeof(DataGridViewButtonColumn))
                    {
                        continue;
                    }

                    String attribute = dgvCell.OwningColumn.Name.Split('_')[1];
                    if (attribute == "IsOpen")
                    {
                        if (dgvCell.Value == null)
                        {
                            nodeElement.SetAttribute(attribute, "True");
                        }
                        else
                        {
                            nodeElement.SetAttribute(attribute, dgvCell.Value.ToString());
                        }
                    }
                    else
                    {
                        if (dgvCell.Value == null) dgvCell.Value = String.Empty;

                        nodeElement.SetAttribute(attribute, (attribute == "Ord" ? ord.ToString() : this.TranferToXmlEntity(dgvCell.Value.ToString())));
                    }
                }
                ord++;

                xmlNode.AppendChild(nodeElement);

                if (treeGridNode.HasChildren)
                {
                    this.WriteXmlNode(nodeElement, treeGridNode.Nodes, xmlDoc, ref ord);
                }
            }
        }
        #endregion

        #region 保存TreeView中的数据到Xml文件中
        /// <summary>
        /// 保存TreeView中的数据到Xml文件中。
        /// </summary>
        /// <param name="treeView">TreeView控件。</param>
        /// <param name="xmlFilePath">Xml文件。</param>
        public void SaveTreeView2Xml(TreeView treeView, String xmlFilePath, EvalType evalType)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");

            XmlNode rootNode = xmlDoc.SelectSingleNode("TechBid");

            this.WriteTree2XmlNode(rootNode, treeView.Nodes[0].Nodes, xmlDoc, evalType);

            xmlDoc.Save(xmlFilePath);
        }

        /// <summary>
        /// 把树形控件写入到Xml节点。
        /// </summary>
        /// <param name="xmlNode">Xml节点。</param>
        /// <param name="treeNodeCollection">树形节点集合。</param>
        private void WriteTree2XmlNode(XmlNode xmlNode, TreeNodeCollection treeNodeCollection, XmlDocument xmlDoc, EvalType evalType)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                XmlElement nodeElement = xmlDoc.CreateElement("ZBItem");
                XmlNode xmlTreeNode = (XmlNode)treeNode.Tag;

                nodeElement.SetAttribute("Sequence", this.TranferToXmlEntity(xmlTreeNode.Attributes["Sequence"].Value));
                nodeElement.SetAttribute("Item", this.TranferToXmlEntity(xmlTreeNode.Attributes["Item"].Value));
                nodeElement.SetAttribute("Content", this.TranferToXmlEntity(xmlTreeNode.Attributes["Content"].Value));
                nodeElement.SetAttribute("Score", xmlTreeNode.Attributes["Score"].Value);
                nodeElement.SetAttribute("PageCount", xmlTreeNode.Attributes["PageCount"].Value);
                nodeElement.SetAttribute("Ord", xmlTreeNode.Attributes["Ord"].Value);

                switch (evalType)
                {
                    case EvalType.TechBidEval:
                        nodeElement.SetAttribute("Standard", this.TranferToXmlEntity(xmlTreeNode.Attributes["Standard"].Value));
                        nodeElement.SetAttribute("Qualified", this.TranferToXmlEntity(xmlTreeNode.Attributes["Qualified"].Value));
                        break;
                    case EvalType.PreceptEval:
                        nodeElement.SetAttribute("VoteValue", this.TranferToXmlEntity(xmlTreeNode.Attributes["VoteValue"].Value));
                        break;
                    case EvalType.OtherEval:
                        break;
                }
                xmlNode.AppendChild(nodeElement);

                if (treeNode.Nodes.Count > 0)
                {
                    this.WriteTree2XmlNode(nodeElement, treeNode.Nodes, xmlDoc, evalType);
                }
            }
        }
        #endregion

        #region 保存HTML文档中Input的值到Xml
        public void SaveHtml2Xml(String xmlFilePath, HtmlDocument htmlDocument, Int32 inputCount)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");

            XmlNode rootNode = xmlDoc.SelectSingleNode("TechBid");

            XmlElement nodeElement = null;
            for (Int32 index = 0; index <= inputCount; index++)
            {
                nodeElement = this.CreateXmlElement("input" + index.ToString(), htmlDocument, xmlDoc);
                if (nodeElement != null)
                {
                    rootNode.AppendChild(nodeElement);
                }

                nodeElement = this.CreateXmlElement("checkbox" + index.ToString(), htmlDocument, xmlDoc);
                if (nodeElement != null)
                {
                    rootNode.AppendChild(nodeElement);
                }
            }

            xmlDoc.Save(xmlFilePath);
        }

        private XmlElement CreateXmlElement(String controlId, HtmlDocument htmlDocument, XmlDocument xmlDoc)
        {
            Regex regWidth = new Regex(@"WIDTH:\s*(\d+)px;?\s*", RegexOptions.IgnoreCase);
            Regex regHeight = new Regex(@"HEIGHT:\s*(\d+)px;?\s*", RegexOptions.IgnoreCase);
            XmlElement nodeElement = null;
            HtmlElement htmlElement = htmlDocument.GetElementById(controlId);
            if (htmlElement != null)
            {
                nodeElement = xmlDoc.CreateElement("input");
                nodeElement.SetAttribute("id", htmlElement.GetAttribute("id"));

                String type = htmlElement.GetAttribute("type");
                nodeElement.SetAttribute("type", type);

                switch (type)
                {
                    case "checkbox":
                        nodeElement.SetAttribute("checked", htmlElement.GetAttribute("checked"));
                        break;
                    case "textarea":
                    case "text":
                        nodeElement.SetAttribute("value", this.TranferToXmlEntity(htmlElement.GetAttribute("value")));

                        if (regWidth.Matches(htmlElement.Style).Count > 0)
                        {
                            nodeElement.SetAttribute("width", regWidth.Match(htmlElement.Style).Groups[1].Value);
                        }

                        if (regHeight.Matches(htmlElement.Style).Count > 0)
                        {
                            nodeElement.SetAttribute("height", regHeight.Match(htmlElement.Style).Groups[1].Value);
                        }
                        break;
                }

                if (!String.IsNullOrEmpty(htmlElement.GetAttribute("name")))
                {
                    nodeElement.SetAttribute("name", htmlElement.GetAttribute("name"));
                }
            }

            return nodeElement;
        }
        #endregion

        #region 保存评审方式到XML文件
        public void SaveEvalWay2Xml(String xmlFilePath, String evalWayName, String passedCount)
        {
            if (!File.Exists(xmlFilePath))
            {
                return;
            }
            else
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            XmlElement xmlElement = xmlDocument.DocumentElement;

            xmlElement.SetAttribute("EvalwayName", evalWayName);
            xmlElement.SetAttribute("PassedCount", passedCount);

            xmlDocument.Save(xmlFilePath);
        }
        #endregion

        #region 保存类实例对象到Xml文件RootName
        /// <summary>
        /// 保存类实例对象到Xml文件。
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="xmlNodeValue"></param>
        public void SaveXml(String xmlFilePath, object xmlNodeValue, String rootName)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><" + rootName + "></" + rootName + ">");

            XmlNode rootNode = xmlDoc.SelectSingleNode(rootName);

            PropertyInfo[] lstPropertyInfo = xmlNodeValue.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in lstPropertyInfo)
            {
                XmlElement nodeElement = xmlDoc.CreateElement(propertyInfo.Name);
                object nodeValue = propertyInfo.GetValue(xmlNodeValue, null);
                nodeElement.InnerText = nodeValue == null ? "" : this.TransferToSafeXmlString(nodeValue.ToString());
                rootNode.AppendChild(nodeElement);
            }

            xmlDoc.Save(xmlFilePath);
        }
        #endregion
        #endregion

        #region 比较两个Xml文件异同
        public List<CompareInfo> XmlCompare(String xmlFilePath1, String xmlFilePath2)
        {
            return this.XmlCompare(xmlFilePath1, xmlFilePath2, false);
        }

        public List<CompareInfo> XmlCompare(String xmlFilePath1, String xmlFilePath2, Boolean isRecordingPosition)
        {
            List<CompareInfo> list = new List<CompareInfo>();
            CompareInfo compareInfo = null;

            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.Load(xmlFilePath1);

            XmlDocument xmlDoc2 = new XmlDocument();
            xmlDoc2.Load(xmlFilePath2);

            XmlNode root1 = xmlDoc1.SelectSingleNode("TechBid");
            XmlNode root2 = xmlDoc2.SelectSingleNode("TechBid");

            XmlNodeList xmlNodeList = root1.ChildNodes;

            XmlNode tempNode = null;
            String tempId = null;
            String tempType = null;
            String tempValue = null;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                tempId = xmlNode.Attributes["id"].Value.ToString();
                tempType = xmlNode.Attributes["type"].Value.ToString();
                if (tempType == "checkbox")
                { 
                    tempValue = xmlNode.Attributes["checked"].Value.ToString();
                }
                else
                {
                    tempValue = xmlNode.Attributes["value"] == null ? String.Empty : xmlNode.Attributes["value"].Value.ToString();
                }                 

                tempNode = null;
                tempNode = xmlDoc2.SelectSingleNode("descendant::input[@id='" + tempId + "']");
                if (tempNode == null)
                {
                    compareInfo = new CompareInfo();
                    compareInfo.Id = tempId;
                    compareInfo.Content = "删除【" + tempId + "】";
                    if (isRecordingPosition)
                    {
                        if (xmlNode.Attributes["PosPage"] != null && xmlNode.Attributes["PosX"] != null && xmlNode.Attributes["PosY"] != null)
                        {
                            compareInfo.PosPage = Convert.ToInt32(xmlNode.Attributes["PosPage"].Value);
                            compareInfo.PosX = Convert.ToDouble(xmlNode.Attributes["PosX"].Value);
                            compareInfo.PosY = Convert.ToDouble(xmlNode.Attributes["PosY"].Value);
                            list.Add(compareInfo);
                        }
                    }
                    else
                    {
                        list.Add(compareInfo);
                    }
                }
            }

            xmlNodeList = root2.ChildNodes;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                tempId = xmlNode.Attributes["id"].Value.ToString();
                tempType = xmlNode.Attributes["type"].Value.ToString();
                if (tempType == "checkbox")
                {
                    tempValue = xmlNode.Attributes["checked"].Value.ToString();
                }
                else
                {
                    tempValue = xmlNode.Attributes["value"] == null ? String.Empty : this.TransferToHtmlEntity(xmlNode.Attributes["value"].Value.ToString());
                }

                tempNode = null;
                tempNode = xmlDoc1.SelectSingleNode("descendant::input[@id='" + tempId + "']");
                if (tempNode == null)
                {
                    compareInfo = new CompareInfo();
                    compareInfo.Id = tempId;
                    compareInfo.Content = "新增【" + tempId + "】";
                    if (isRecordingPosition)
                    {
                        if (xmlNode.Attributes["PosPage"] != null && xmlNode.Attributes["PosX"] != null && xmlNode.Attributes["PosY"] != null)
                        {
                            compareInfo.PosPage2 = Convert.ToInt32(xmlNode.Attributes["PosPage"].Value);
                            compareInfo.PosX2 = Convert.ToDouble(xmlNode.Attributes["PosX"].Value);
                            compareInfo.PosY2 = Convert.ToDouble(xmlNode.Attributes["PosY"].Value);
                            list.Add(compareInfo);
                        }
                    }
                    else
                    {
                        list.Add(compareInfo);
                    }
                }
                else
                {
                    String tempValue2 = String.Empty;
                    if (tempType == "checkbox")
                    {
                        tempValue2 = tempNode.Attributes["checked"].Value.ToString();
                    }
                    else
                    {
                        tempValue2 = tempNode.Attributes["value"] == null ? String.Empty : this.TransferToHtmlEntity(tempNode.Attributes["value"].Value.ToString());
                    }

                    if (tempValue != tempValue2)
                    {
                        compareInfo = new CompareInfo();
                        compareInfo.Id = tempId;
                        compareInfo.Content = "修改【" + tempValue2 + "-->" + tempValue + "】";
                        if (isRecordingPosition)
                        {
                            if (tempNode.Attributes["PosPage"] != null && tempNode.Attributes["PosX"] != null && tempNode.Attributes["PosY"] != null
                                && xmlNode.Attributes["PosPage"] != null && xmlNode.Attributes["PosX"] != null && xmlNode.Attributes["PosY"] != null)
                            {
                                compareInfo.PosPage = Convert.ToInt32(tempNode.Attributes["PosPage"].Value);
                                compareInfo.PosX = Convert.ToDouble(tempNode.Attributes["PosX"].Value);
                                compareInfo.PosY = Convert.ToDouble(tempNode.Attributes["PosY"].Value);
                                compareInfo.PosPage2 = Convert.ToInt32(xmlNode.Attributes["PosPage"].Value);
                                compareInfo.PosX2 = Convert.ToDouble(xmlNode.Attributes["PosX"].Value);
                                compareInfo.PosY2 = Convert.ToDouble(xmlNode.Attributes["PosY"].Value);
                                list.Add(compareInfo);
                            }
                        }
                        else
                        {
                            list.Add(compareInfo);
                        }
                    }
                }
            }

            return list;
        }
        #endregion

        #region 判断XML模版有无数据
        public Boolean IsExistsModuleData(String xmlFilePath)
        {
            Boolean isExists = false;

            if (!File.Exists(xmlFilePath))
            {
                return isExists;
            }

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFilePath);
            }
            catch
            {
                return isExists;
            }

            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

            if (xmlNodeList.Count > 0)
            {
                isExists = true;
            }

            return isExists;
        }
        #endregion

        #region 获取满分合计值
        public Decimal GetTotalScore(String xmlFilePath)
        {
            if (!File.Exists(xmlFilePath))
            {
                return 0;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);

            Decimal totalScore = 0;
            this.CalculationTotalScore(xmlDocument.DocumentElement.ChildNodes, ref totalScore);

            return totalScore;
        }

        private void CalculationTotalScore(XmlNodeList xmlNodeList, ref Decimal totalScore)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    totalScore += Convert.ToDecimal(xmlNode.Attributes["Score"].Value);
                }
                catch
                {
                    totalScore += 0;
                }
            }
        }
        #endregion

        #region 获取技术标评审方式和合格标准
        public String[] GetEvalWayAndPassedCount(String xmlFilePath)
        {
            if (!File.Exists(xmlFilePath))
            {
                return new String[] { "", "" };
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            XmlElement xe = xmlDocument.DocumentElement;
            String evalWayName = xe.Attributes["EvalwayName"] == null ? "" : xe.Attributes["EvalwayName"].Value;
            String passedCount = xe.Attributes["PassedCount"] == null ? "" : xe.Attributes["PassedCount"].Value;

            return new String[] { evalWayName, passedCount };
        }
        #endregion

        #region 根据节点名获取节点文本
        public String GetNodeTextByName(String xmlFilePath, String nodeName)
        {
            if (!File.Exists(xmlFilePath))
            {
                return String.Empty;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            XmlElement xe = xmlDocument.DocumentElement;

            return xe.SelectSingleNode(nodeName).InnerText;
        }
        #endregion

        #region 保存数组到Xml文件
        public void SaveArray2Xml(String xmlFilePath, List<String> lstFileName)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");

            XmlNode rootNode = xmlDoc.SelectSingleNode("TechBid");

            foreach (String pdfFileName in lstFileName)
            {
                XmlElement nodeElement = xmlDoc.CreateElement("Item");
                nodeElement.InnerText = pdfFileName;
                rootNode.AppendChild(nodeElement);
            }

            xmlDoc.Save(xmlFilePath);
        }
        #endregion

        #region 移除xml文件上的指定节点
        public void RemoveXmlNode(String xmlFilePath, ArrayList xmlNodeIds)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            RemoveXmlNodeById(xmlDoc.DocumentElement.ChildNodes, xmlNodeIds);

            xmlDoc.Save(xmlFilePath);
        }

        private void RemoveXmlNodeById(XmlNodeList xmlNodeList, ArrayList xmlNodeIds)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                TreeNode newTreeNode = new TreeNode();
                if (xmlNode.Name.ToLower() == "projinfo")
                {
                    continue;
                }

                if (xmlNodeIds.Contains(xmlNode.Attributes["id"].Value))
                {
                    xmlNode.Attributes["fontName"].Value = "";                    
                }

                if (xmlNode.HasChildNodes)
                {
                    this.RemoveXmlNodeById(xmlNode.ChildNodes, xmlNodeIds);
                }
            }
        }
        #endregion

        #region 去掉字符串中特殊字符
        /// <summary>
        /// 去掉字符串中特殊字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public String TranferToXmlEntity(String str)
        {
            if (str == null)
            {
                return null;
            }

            String new_str = str;

            Regex regex = new Regex("&");
            new_str = regex.Replace(new_str, "&amp;");

            regex = new Regex("<");
            new_str = regex.Replace(new_str, "&lt;");

            regex = new Regex(">");
            new_str = regex.Replace(new_str, "&gt;");

            regex = new Regex("'");
            new_str = regex.Replace(new_str, "&apos;");

            regex = new Regex("\"");
            new_str = regex.Replace(new_str, "&quot;");

            return new_str;
        }

        public String TransferToHtmlEntity(String str)
        {
            if (str == null)
            {
                return null;
            }

            String new_str = str;

            Regex regex = new Regex("&amp;");
            new_str = regex.Replace(new_str, "&");

            regex = new Regex("&lt;");
            new_str = regex.Replace(new_str, "<");

            regex = new Regex("&gt;");
            new_str = regex.Replace(new_str, ">");

            regex = new Regex("&apos;");
            new_str = regex.Replace(new_str, "'");

            regex = new Regex("&quot;");
            new_str = regex.Replace(new_str, "\"");

            return new_str;
        }

        /// <summary>
        /// XML标准规定的无效字节为：
        /// 0x00 - 0x08
        /// 0x0b - 0x0c
        /// 0x0e - 0x1f
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public String TransferToSafeXmlString(String str)
        {
            Regex reg = new Regex("[\\x00-\\x08\\x0b-\\x0c\\x0e-\\x1f]");

            return reg.Replace(str, "");
        }
        #endregion

        #region 将一个DataTable以xml方式存入指定的文件中
        /// <summary>
        /// 将一个DataTable以xml方式存入指定的文件中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public static void SaveDataTableToXml(DataTable dt, string filePath)
        {
            //创建文件夹
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.WriteXml(filePath);
        }
        #endregion

        #region 从一个指定的文件中读取DataTable
        /// <summary>
        /// 从一个指定的文件中读取DataTable
        /// </summary>
        /// <param name="filePath"></param>
        public static DataTable ReadDataTableFromXml(string filePath)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(filePath);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 根据子节点的名称取得所有相同的列表
        /// <summary>
        /// 根据子节点的名称取得所有相同的列表
        /// </summary>
        public static List<XmlNode> GetChildXmlNodeListByChildName(XmlNode xmlNode, String childName)
        {
            List<XmlNode> resultList = new List<XmlNode>();
            // 循环取得节点
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childName.Equals(childNode.Name))
                {
                    resultList.Add(childNode);
                }
            }

            return resultList;
        }
        #endregion

        #region 绑定Xml节点到树形控件节点。
        public static void XmlNode2TreeNode(XmlNodeList xmlNodes, TreeNodeCollection treeNodes)
        {
            foreach (XmlNode lNode in xmlNodes)
            {
                if (lNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                TreeNode newTreeNode = new TreeNode();
                string strText = lNode.Attributes[1].Value;
                newTreeNode.Text = strText;  // lNode.Attributes["title"].Value;
                newTreeNode.Tag = lNode;
                if (lNode.HasChildNodes)
                {
                    XmlNode2TreeNode(lNode.ChildNodes, newTreeNode.Nodes);
                }
                treeNodes.Add(newTreeNode);
            }
        }
        #endregion        

        #region 绑定xml数据到DataGridView控件上
        public static void BindXml2DataGridView(DataGridView dataGridView, String xmlFilePath)
        {
            if (!File.Exists(xmlFilePath))
            {
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
            dataGridView.Rows.Clear();

            DataGridViewRow newDataGridViewRow = null;
            Int32 idx = -1;
            String attriName = String.Empty;
            String colName = String.Empty;
            foreach (XmlNode xn in xmlDocument.DocumentElement.ChildNodes)
            {
                idx = dataGridView.Rows.Add();
                newDataGridViewRow = dataGridView.Rows[idx];
                foreach (DataGridViewCell dgvCell in newDataGridViewRow.Cells)
                {
                    colName = dgvCell.OwningColumn.Name;
                    attriName = colName.Split(new Char[] { '_' })[1];
                    if (xn.Attributes[attriName] != null)
                    {
                        dgvCell.Value = xn.Attributes[attriName].Value;
                    }
                }
            }
        }
        #endregion

        #region 保存DataGridViewRow数据到Xml文件
        public static void SaveDataGridView2Xml(DataGridView dataGridView, String xmlFilePath)
        {
            if (File.Exists(xmlFilePath))
            {
                FileInfo fileInfo = new FileInfo(xmlFilePath);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }

                File.Delete(xmlFilePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><TechBid></TechBid>");

            XmlNode rootNode = xmlDoc.SelectSingleNode("TechBid");

            XmlElement nodeElement = null;
            String colName = String.Empty;
            foreach (DataGridViewRow dgvr in dataGridView.Rows)
            {
                nodeElement = xmlDoc.CreateElement("ZBItem");

                foreach (DataGridViewCell dgvCell in dgvr.Cells)
                {
                    colName = dgvCell.OwningColumn.Name;
                    nodeElement.SetAttribute(colName.Split(new Char[] { '_' })[1], CommonFunction.ObjectToString(dgvCell.Value));
                }

                rootNode.AppendChild(nodeElement);
            }

            xmlDoc.Save(xmlFilePath);
        }
        #endregion 
    }
}
