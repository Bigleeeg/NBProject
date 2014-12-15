using JIAOFENG.Practices.Library.FileIO;
using System.Configuration;
using System.IO;

namespace JIAOFENG.Practices.Logic.File
{
    public class LocalFolderFile : FileStorage
    {
        private static string fileFolder = ConfigurationManager.AppSettings["FileFolder"];
        public LocalFolderFile()
        {
            this.SaveLocationType = ESaveLocationType.WebFolder;
        }
        public override void AddRealFile()
        {
            string physicalFilePath = Path.Combine(fileFolder, this.SavePath);
            FileOperator fileOperator = new FileOperator(physicalFilePath);
            fileOperator.Write(this.Context, 0, this.Context.Length);
        }

        public override void UpdateRealFile()
        {
            string physicalFilePath = Path.Combine(fileFolder, this.SavePath);
            System.IO.File.Delete(physicalFilePath);
            FileOperator fileOperator = new FileOperator(physicalFilePath);
            fileOperator.Write(this.Context, 0, this.Context.Length);
        }
        public override void DeleteRealFile()
        {
            string physicalFilePath = Path.Combine(fileFolder, this.SavePath);
            System.IO.File.Delete(physicalFilePath);
        }
        public override void LoadContent()
        {
            if (this.Context == null)
            {
                string fileFolder = ConfigurationManager.AppSettings["FileFolder"];
                string physicalFilePath = Path.Combine(fileFolder, this.SavePath);
                              
                if (System.IO.File.Exists(physicalFilePath))
                {
                    FileOperator fileOperator = new FileOperator(physicalFilePath);
                    this.Context = fileOperator.ConvertToBytes();
                }
                else
                {
                    LoadLostPicture();
                }
            }            
        }
    }
}
