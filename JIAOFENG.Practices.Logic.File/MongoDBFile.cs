using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.File
{
    public class MongoDBFile : FileStorage
    {
        public MongoDBFile()
        {
            this.SaveLocationType = ESaveLocationType.MongoDB;
        }
        public override void AddRealFile()
        {

        }

        public override void UpdateRealFile()
        {

        }
        public override void DeleteRealFile()
        {
        }
        public override void LoadContent()
        {
        }
    }
}
