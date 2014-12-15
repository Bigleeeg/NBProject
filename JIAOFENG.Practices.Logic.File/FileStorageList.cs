using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.File
{
    public class FileStorageList : List<FileStorage>
    {
        public FilePackage FilePackage { get; private set; }
        public FileStorageList(FilePackage filePackage)
        {
            this.FilePackage = filePackage;
        }
    }
}
