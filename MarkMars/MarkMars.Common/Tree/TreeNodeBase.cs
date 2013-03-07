using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace MarkMars.Common.Tree
{
    public abstract class TreeNodeBase : AutoID
    {
		//父节点
        private TreeNodeBase m_nodeParent;
        public TreeNodeBase ParentNode
        {
            get
            {
                return m_nodeParent;
            }
			set
			{
				m_nodeParent = value;
                SetModified();
			}
        }

		private TreeBase m_treeOwner;
		public TreeBase OwnerTree
		{
			get
			{
				return m_treeOwner;
			}
			set
			{
				Debug.Assert(value != null);
				m_treeOwner = value;
				foreach (TreeNodeBase node in m_listChildNode)
					node.OwnerTree = value;
			}
		}

		private List<TreeNodeBase> m_listChildNode = new List<TreeNodeBase>();
		public System.Collections.ObjectModel.ReadOnlyCollection<TreeNodeBase> ChildNodes
        {
            get
            {
				return m_listChildNode.AsReadOnly();
            }
        }

        internal TreeNodeBase(DataRow row, TreeNodeBase nodeParent, TreeBase treeOwner)
        {
            Debug.Assert(row != null);
			Debug.Assert(treeOwner != null);

            m_nodeParent = nodeParent;
			m_treeOwner = treeOwner;

			m_nDBID = System.Convert.ToInt32(row["id"]);
            m_nUIIndex = System.Convert.ToInt32(row["UIIndex"]);
        }

        internal TreeNodeBase(TreeNodeBase nodeParent, TreeBase treeOwner)
        {
            m_nodeParent = nodeParent;
			m_treeOwner = treeOwner;
        }

        internal virtual void Save()
        {
			foreach (TreeNodeBase node in m_listChildNode)
				node.Save();
        }

        private bool m_bModified = false;
        public bool Modified              //wuyl+, 将原来的这个属性公开，有任何修改都通知本工程有修改！
        {
            get
            {
                return m_bModified;
            }
        }

		public virtual void SetModified()
		{
            m_bModified = true;
		}

        /// <summary>
        /// 数据库表内的id
        /// </summary>
        protected int m_nDBID = -1;
        public int DBID
        {
            get
            {
                return m_nDBID;
            }
        }

        private int? m_nUIIndex = -1;
        public int? UIIndex
        {
            get
            {
                return m_nUIIndex;
            }
            internal set
            {
                if (m_nUIIndex != value)
                {
                    SetModified();
                    m_nUIIndex = value;
                }
            }
        }

        public TreeNodeBase() { }

		public abstract object GetValue(string strPropertyName);
		internal protected abstract void SetValue(string strPropertyName, object oNewValue);
	}
}
