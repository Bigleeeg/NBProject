using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyCoder
{
    /// <summary>
    /// 定义数据库操作访问接口
    /// </summary>
    public interface IDbAccess
    {
        string ConnectionString
        {
            get;
            set;
        }

        int Execute(string SqlCommand);
        System.Data.DataTable Query(string SqlCommand);

        System.Data.IDbConnection DbConnection
        {
            get;
            set;
        }
    }
}
