using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EasyCoder
{
    public interface ICreator
    {
        void CreateAllCode(string connectionString,string dbName, string tablename, string namespacename,string ownerName);

        DataSet GetDataInfo(List<string> tablenames, string namespacename);

        void CreateModel(DataSet ds, string namespacename);

        void CreateDal(DataSet ds, string namespacename);

        void CreateLogic(DataSet ds, string namespacename);
        
        //string GetFieldDefine(DataTable dt);

        //string GetReadDefineAll(DataTable dt);

        //string GetReadDefineByID(DataTable dt);

        //string GetDeleteDefine(DataTable dt);

        //string GetUpdateDefine(DataTable dt);

        //string GetAddDefine(DataTable dt);  

        //string GetFieldType(string fileType);

    }
}
