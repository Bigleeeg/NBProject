using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JIAOFENG.Practices.Logic.File
{
    public static class FileStorageFactory
    {
        public static FileStorage CreateFileStorage(HttpPostedFileBase postedFile)
        {
            if (postedFile == null)
            {
                throw new ArgumentNullException("postedFile", "所上传的文件流为空.");
            }

            FileStorage fileStorage;
            string fileLocationType = ConfigurationManager.AppSettings["SaveLocationType"];
            ESaveLocationType saveLocationType = (ESaveLocationType)Enum.Parse(typeof(ESaveLocationType), fileLocationType);
            switch (saveLocationType)
            {
                case  ESaveLocationType.WebFolder:
                    fileStorage = new LocalFolderFile();
                    
                    break;
                case ESaveLocationType.FileServer:
                    fileStorage = new FileServerFile();
                    break;
                case ESaveLocationType.LocalDB:
                    fileStorage = new LocalDBFile();
                    break;
                case ESaveLocationType.MongoDB:
                    fileStorage = new MongoDBFile();
                    break;
                default:
                    fileStorage = new LocalDBFile();
                    break;
            }
            fileStorage.FileCode = Guid.NewGuid().ToString();
            fileStorage.FileName = Path.GetFileName(postedFile.FileName);
            fileStorage.ExtensionName = Path.GetExtension(postedFile.FileName);
            fileStorage.SavePath = fileStorage.FileCode + fileStorage.ExtensionName;
            fileStorage.FileType = postedFile.ContentType;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream, Encoding.UTF8))
            {
                fileStorage.Context = br.ReadBytes((int)postedFile.InputStream.Length);
            }
            return fileStorage;
        }
    }
}
