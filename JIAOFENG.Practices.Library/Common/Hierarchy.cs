using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.Common
{
    public class HierarchyNode<T> : IHierarchyNode where T : IHierarchyNode
    {
        public HierarchyNode(T entity)
        {
            this.Entity = entity;
        }
        public int ID { get { return this.Entity.ID; } }
        public string DisplayName { get { return this.Entity.DisplayName; } }
        public string Code { get { return this.Entity.Code; } }

        public bool Selected { get; set; }
        //private bool selected = false;
        //public bool Selected 
        //{ 
        //    get
        //    {
        //        return selected;
        //    }
        //    set
        //    {               
        //        this.selected = value;
        //        if (this.selected == true)
        //        {
        //            if (this.Parent != null)
        //            {
        //                this.Parent.Selected = value;
        //            }
        //        }
        //        else
        //        {
        //            this.Children.SetSelected(value);
        //        }
        //    }
        //}
        public int? ParentID { get { return this.Entity.ParentID; } }
        //public HierarchyNode<T> Parent { get; set; }
        public HierarchyList<T> Children { get; set; }

        public T Entity { get; protected set; }
    }

    public class HierarchyList<T> : List<HierarchyNode<T>> where T : IHierarchyNode
    {
        #region Property
        public HierarchyList<T> AllNodesWithDescendant
        {
            get
            {
                HierarchyList<T> list = new HierarchyList<T>();
                foreach (HierarchyNode<T> node in this)
                {
                    AppendModelListWithDescendant(list, node);
                }
                return list;
            }
        }
        #endregion

        #region Method
        public bool Contains(string code)
        {
            foreach (HierarchyNode<T> node in this)
            {
                if (node.Code == code)
                {
                    return true;
                }
            }
            return false;
        }
        public bool ContainsWithDescendant(string code)
        {
            foreach (HierarchyNode<T> node in this.AllNodesWithDescendant)
            {
                if (node.Code == code)
                {
                    return true;
                }
            }
            return false;
        }

        public HierarchyNode<T> FindWithDescendant(string code)
        {
            foreach (HierarchyNode<T> node in this.AllNodesWithDescendant)
            {
                if (node.Code == code)
                {
                    return node;
                }
            }
            return null;
        }
        public HierarchyNode<T> FindWithDescendant(int id)
        {
            foreach (HierarchyNode<T> node in this.AllNodesWithDescendant)
            {
                if (node.ID == id)
                {
                    return node;
                }
            }
            return null;
        }
        
        private void AppendModelListWithDescendant(HierarchyList<T> list, HierarchyNode<T> node)
        {
            list.Add(node);
            foreach (HierarchyNode<T> child in node.Children)
            {
                AppendModelListWithDescendant(list, child);
            }
        }
        public static HierarchyList<T> ConvertToHierarchyList(List<T> modelList)
        {
            HierarchyList<T> roots = new HierarchyList<T>();
            foreach (T model in modelList.Where(m => m.ParentID.HasValue == false || m.ParentID.Value == 0))
            {
                HierarchyNode<T> node = new HierarchyNode<T>(model);
                GetSetChildList(node, modelList);
                roots.Add(node);
            }
            return roots;
        }
        private static void GetSetChildList(HierarchyNode<T> parent, List<T> modelList)
        {
            HierarchyList<T> childs = new HierarchyList<T>();
            foreach (T model in modelList.Where(m => m.ParentID == parent.ID))
            {
                HierarchyNode<T> node = new HierarchyNode<T>(model);
                //node.Parent = parent;
                GetSetChildList(node, modelList);
                childs.Add(node);
            }
            parent.Children = childs;
        }

        public void SetSelected(bool isSelected)
        {
            foreach (HierarchyNode<T> node in this)
            {
                node.Selected = isSelected;
            }
        }
        #endregion
    }
}
