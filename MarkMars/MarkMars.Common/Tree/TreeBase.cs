using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Data;

namespace MarkMars.Common.Tree
{
    /// <summary>
    /// 这是有父子关系的grid的数据的基类
    /// </summary>
    public abstract class TreeBase
    {
        public TreeBase()
		{
		}

        /// <summary>
        /// 实际的加载逻辑在这里,每个子类自己实现
        /// </summary>
        protected abstract void FillData_Inner();

        public virtual void Save()
        {
            if (m_listNode == null) 
                return;

			foreach (TreeNodeBase node in m_listNode)
				node.Save();
        }

		public System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeBase> GetSameLevelNodes(TreeNodeBase node)
		{
			Debug.Assert(node != null);

			TreeNodeBase treenode = node as TreeNodeBase;
			if (treenode.ParentNode == null)
				return this.Nodes;
			else
				return treenode.ParentNode.ChildNodes;
		}

		/// <summary>
		/// 节点
		/// </summary>
		protected List<TreeNodeBase> m_listNode = null;

		public System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeBase> Nodes
		{
			get
			{
				return m_listNode.AsReadOnly();
			}
		}

		/// <summary>
		/// 是否已经填入了数据
		/// </summary>
		protected bool m_bHasData = false;

        /// <summary>
        /// 填充数据
        /// </summary>
		protected void FillData()
		{
			if (!m_bHasData)
			{
				FillData_Inner();
				m_bHasData = true;
			}
		}

		protected abstract TreeNodeBase CreateNode(DataRow row, TreeNodeBase nodeParent, TreeBase treeOwner);

		public List<TreeNodeBase> GetBottomNode()
		{
			List<TreeNodeBase> listNode = new List<TreeNodeBase>();

			foreach (TreeNodeBase node in this.Nodes)
				GetBottomNode(node, listNode);

			return listNode;
		}

		private void GetBottomNode(TreeNodeBase node, List<TreeNodeBase> listNode)
		{
			if (node.ChildNodes.Count > 0)
			{
				foreach (TreeNodeBase nodeChild in node.ChildNodes)
					GetBottomNode(nodeChild, listNode);
			}
			else
				listNode.Add(node);
		}
    }
}
