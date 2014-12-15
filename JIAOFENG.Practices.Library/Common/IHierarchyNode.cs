using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.Common
{
    public interface IHierarchyNode
    {
        int ID { get; }
        string DisplayName { get; }
        string Code { get; }
        int? ParentID { get; }
    }
}
