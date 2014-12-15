using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.File
{
    public class LocalDBFile : FileStorage
    {
        public LocalDBFile()
        {
            this.SaveLocationType = ESaveLocationType.LocalDB;
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
