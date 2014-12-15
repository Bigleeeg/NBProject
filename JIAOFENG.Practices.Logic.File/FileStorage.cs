using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JIAOFENG.Practices.Library.Common;
using JIAOFENG.Practices.Library.FileIO;

namespace JIAOFENG.Practices.Logic.File
{
    public abstract class FileStorage : EntityExtInfo, IPrimaryKey
    {
        
        
	    #region constructor
        public FileStorage()
        {

        }
        public FileStorage(FileStorage entity)
        {
			CopyFrom(entity);
        }
		#endregion

		#region property

        public int ID
        {
            get
            {
                return this.FileStorageID;
            }
        }
       public int FileStorageID { get; set; }
       public string FileCode { get; set; } 
       public string FileName { get; set; } 

       public string ExtensionName { get; set; } 

       public string FileType { get; set; } 
       public string FileCategory
       {
           get
           {
               if (this.FileType == "application/x-bmp" || this.FileType == "image/gif" ||
                this.FileType == "image/x-icon" || this.FileType == "application/x-img" ||
                this.FileType == "image/jpeg" || this.FileType == "application/x-jpe" ||
                this.FileType == "application/x-jpg" || this.FileType == "image/png" ||
                this.FileType == "application/x-png" || this.FileType == "image/x-png" ||
                this.FileType == "image/tiff" || this.FileType == "image/x-jpeg" || this.FileType == "image/pjpeg")
               {
                   return "CanPreview";
               }
               else if (this.FileType == "application/msword" || this.FileType == "application/pdf" ||
                       this.FileType == "text/plain" || this.FileType == "application/x-rtf" ||
                       this.FileType == "application/vnd.openxmlformats-officedocument.wordprocessingml.template")
               {
                   return "NoPreview";
               }
               else if (this.FileType == "application/-excel" || this.FileType == "application/x-xls" ||
                           this.FileType == "application/x-xlw" || this.FileType == "application/vnd.ms-excel" ||
                           this.FileType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                           this.FileType == "application/vnd.openxmlformats-officedocument.spreadsheetml.template")
               {
                   return "NoPreview";
               }
               else
               {
                   return "NoPreview";
               }
           }           
       }

       public ESaveLocationType SaveLocationType { get; set; } 

       public string SavePath { get; set; } //相对路径，比如abc.docx,\picture\lost.jpg

       public byte[] Context { get; set; } 

       public int FilePackageID { get; set; } 

		#endregion

       public void CopyFrom(FileStorage entity)
		{
          this.FileStorageID = entity.FileStorageID; 
          this.FileName = entity.FileName; 
          this.ExtensionName = entity.ExtensionName; 
          this.FileType = entity.FileType; 
          this.SaveLocationType = entity.SaveLocationType; 
          this.SavePath = entity.SavePath; 
          this.Context = entity.Context; 
          this.CreateTime = entity.CreateTime; 
          this.CreateBy = entity.CreateBy; 
          this.UpdateTime = entity.UpdateTime; 
          this.UpdateBy = entity.UpdateBy; 
          this.FilePackageID = entity.FilePackageID; 

		}
        /// <summary>
        /// 将文件流转化成FileStorage实体
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="context">上下文</param>
        /// <param name="userId">当前用户ID</param>
        /// <returns></returns>
        //public static FileStorage GetFileStorageFromFile(HttpPostedFileBase fileStream, int userId)
        //{
        //    FileStorage model = new FileStorage();
        //    if (fileStream == null)
        //    {
        //        throw new ArgumentNullException("fileStream", "所上传的文件流为空.");
        //    }
        //    model.FileName = Path.GetFileName(fileStream.FileName);
        //    model.ExtensionName = Path.GetExtension(fileStream.FileName);
        //    model.FileType = fileStream.ContentType;

        //    //生成随机唯一Code，用于删除及页面元素定位
        //    model.FileCode = Guid.NewGuid() + DateTime.Now.ToString("hhmmss");

        //    int type = 0;
        //    int.TryParse(ConfigurationManager.AppSettings["SaveLocationType"], out type);
        //    model.SaveLocationType = type;

        //    switch (type)
        //    {
        //        //  网站文件夹（本地相对路径）
        //        case (int)ESaveLocationType.WebFolder:

        //            string visualPath = ConfigurationManager.AppSettings["VisualPath"];
        //            string physicsPath = ConfigurationManager.AppSettings["PhysicsPath"];
        //            string savedPath = physicsPath + visualPath;
        //            string randomFileName = string.Format("{0}_{1}", model.FileCode, model.FileName);
        //            model.SavePath = string.Format("{0}\\{1}", savedPath, randomFileName);
        //            //这里如果保存到服务器的话，可能会产生垃圾数据，因为有可能这个文件作废了，并不打算上传
        //            //所以这里改写成流，并保存在Context里
        //            //fileObj.SaveAs(model.SavePath);
        //            using (BinaryReader br = new BinaryReader(fileStream.InputStream, Encoding.UTF8))
        //            {
        //                var fileBytes = br.ReadBytes((int)fileStream.InputStream.Length);
        //                model.Context = Convert.ToBase64String(fileBytes);
        //                br.Close();
        //            }

        //            break;
        //        //  文件服务器（局域网绝对路径）
        //        case (int)ESaveLocationType.FileServer:
        //            //TO-DO:文件服务器方式
        //            break;
        //        //  本地数据库（放置文件的数据库表）
        //        case (int)ESaveLocationType.LocalDB:

        //            using (BinaryReader br = new BinaryReader(fileStream.InputStream, Encoding.UTF8))
        //            {
        //                var fileBytes = br.ReadBytes((int)fileStream.InputStream.Length);
        //                model.Context = Convert.ToBase64String(fileBytes);
        //                br.Close();
        //            }

        //            break;
        //        //  文件数据库（Mogo数据库）
        //        case (int)ESaveLocationType.FileDB:
        //            //TO-DO:文件数据库方式
        //            break;
        //        default:
        //            throw new ConfigurationErrorsException("文件存储方式配置项未配置或配置错误！");

        //    }

        //    model.CreateBy = userId;
        //    model.UpdateBy = userId;

        //    return model;
        //}

        /// <summary>
        /// 保存
        /// </summary>
       public void Save()
       {
           if (this.FileStorageID > 0)//文件操作是一个特殊的类型，基本上只有新增和删除，没有更新之说（但文件包/夹可以更新）
           {
               //UpdateRealFile();
               //FileStorageDal.Update(this);
           }
           else
           {
               AddRealFile();
               this.FileStorageID = FileStorageDal.Add(this).FileStorageID;
           }
       }
       public void Delete()
       {
           DeleteRealFile();
           FileStorageDal.Delete(this.FileStorageID);
       }
       public abstract void DeleteRealFile();
        public abstract void AddRealFile();

        public abstract void UpdateRealFile();
        public abstract void LoadContent();

        public static FileStorage GetEntityByID(int id)
        {
            return FileStorageDal.GetEntityByID(id);
        }
        protected void LoadLostPicture()
        {
            string lostPicturePath = ConfigurationManager.AppSettings["LostPicturePath"];
            string physicsPath = HttpContext.Current.Server.MapPath(lostPicturePath);
            FileOperator fileOperator = new FileOperator(physicsPath);
            this.Context = fileOperator.ConvertToBytes();
            this.FileType = @"image/x-png";
        }
        /// <summary>
        /// 根据文件ID查询文件是否存在（包括实际文件）
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns></returns>
        //public static bool FileExists(int id)
        //{
        //    var model = FileStorageDal.GetEntityByID(id);
        //    if (model != null)
        //    {
        //        return FileExistsByModel(model);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 查询该实体对应的实际文件是否存在
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        //private static bool FileExistsByModel(FileStorage model)
        //{
        //    switch (model.SaveLocationType)
        //    {
        //        case (int)ESaveLocationType.WebFolder:
        //            //return File.Exists(model.SavePath);

        //        case (int)ESaveLocationType.FileServer:
        //            return false;

        //        case (int)ESaveLocationType.LocalDB:
        //            return !string.IsNullOrWhiteSpace(model.Context);

        //        case (int)ESaveLocationType.MongoDB:
        //            return false;
        //    }
        //    return false;
        //}

        public static bool Exists(int id)
        {
            return FileStorageDal.Exists(id);
        }
        
        /// <summary>
        /// 自定义条件删除文件
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="value">值</param>
        //public static void Delete(string columnName, object value)
        //{
        //    foreach (var item in FileStorageDal.GetList(columnName, value))
        //    {
        //        FileStorage fs = new FileStorage();
        //        fs.CopyFrom(item);
        //        fs.Delete();
        //    }
        //}

        /// <summary>
        /// 根据文件ID删除文件
        /// </summary>
        /// <param name="id">文件ID</param>
        public static void Delete(int id)
        {
            FileStorage fileStorage = FileStorageDal.GetEntityByID(id);
            fileStorage.Delete();
            //var model = FileStorageDal.GetEntityByID(id);
            //switch (model.SaveLocationType)
            //{
            //    case (int)ESaveLocationType.WebFolder:
            //        if (FileExistsByModel(model))
            //        {
            //            File.Delete(model.SavePath);
            //        }
            //        break;

            //    case (int)ESaveLocationType.FileServer:
            //        break;

            //    case (int)ESaveLocationType.LocalDB:
            //        break;

            //    case (int)ESaveLocationType.FileDB:
            //        break;
            //}
            //FileStorageDal.Delete(id);
        }

        /// <summary>
        /// 删除自身
        /// </summary>
        //public void Delete()
        //{
        //    switch (this.SaveLocationType)
        //    {
        //        case (int)ESaveLocationType.WebFolder:
        //            if (FileExistsByModel(this))
        //            {
        //                File.Delete(this.SavePath);
        //            }
        //            break;

        //        case (int)ESaveLocationType.FileServer:
        //            break;

        //        case (int)ESaveLocationType.LocalDB:
        //            break;

        //        case (int)ESaveLocationType.FileDB:
        //            break;
        //    }
        //    FileStorageDal.Delete(this.FileStorageID);
        //}

        //public static FileStorage GetEntityById(int id)
        //{
        //    FileStorage model = new FileStorage();
        //    model.CopyFrom(FileStorageDal.GetEntityByID(id));

        //    //生成随机唯一Code，用于删除及页面元素定位
        //    model.FileCode = Guid.NewGuid() + DateTime.Now.ToString("hhmmss");

        //    TransFileToBase64(model);

        //    return model;
        //}

        /// <summary>
        /// 根据文件ID获取文件流
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        //public static Stream GetFileStreamById(int id)
        //{
        //    var model = FileStorage.GetEntityById(id);
        //    switch (model.SaveLocationType)
        //    {
        //        case (int)ESaveLocationType.WebFolder:
        //            return new FileStream(model.SavePath, FileMode.Open, FileAccess.Read, FileShare.None);

        //        case (int)ESaveLocationType.FileServer:
        //            break;

        //        case (int)ESaveLocationType.LocalDB:
        //            //string visualPath = ConfigurationManager.AppSettings["VisualPath"];
        //            //string savedPath = context.Server.MapPath(visualPath);
        //            //string randomFileName = string.Format("{0}_{1}", Guid.NewGuid(), model.FileName);
        //            //string file = string.Format("{0}\\{1}", savedPath, randomFileName);
        //            //try
        //            //{
        //            //    using (Stream sw = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
        //            //    {
        //            //        byte[] fileBytes = Convert.FromBase64String(model.Context);
        //            //        sw.Write(fileBytes, 0, fileBytes.Length);
        //            //        sw.Close();
        //            //        return new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Delete);
        //            //    }
        //            //}
        //            //finally
        //            //{
        //            //    if (File.Exists(file))
        //            //    {
        //            //        File.Delete(file);
        //            //    }
        //            //}
        //            return new MemoryStream(Convert.FromBase64String(model.Context));
        //        case (int)ESaveLocationType.FileDB:
        //            break;
        //    }
        //    return null;
        //}

        //public static FileStreamResult GetFileStreamResultById(int id)
        //{
        //    var model = FileStorage.GetEntityById(id);
        //    switch (model.SaveLocationType)
        //    {
        //        case (int)ESaveLocationType.WebFolder:
        //            return new FileStreamResult(new FileStream(model.SavePath, FileMode.Open, FileAccess.Read, FileShare.None), model.FileType);

        //        case (int)ESaveLocationType.FileServer:
        //            break;

        //        case (int)ESaveLocationType.LocalDB:
        //            //string visualPath = ConfigurationManager.AppSettings["VisualPath"];
        //            //string savedPath = HttpContext.Current.Server.MapPath(visualPath);
        //            //string randomFileName = string.Format("{0}_{1}", Guid.NewGuid(), model.FileName);
        //            //string file = string.Format("{0}\\{1}", savedPath, randomFileName);
        //            //try
        //            //{
        //            //    using (Stream sw = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
        //            //    {
        //            //        byte[] fileBytes = Convert.FromBase64String(model.Context);
        //            //        sw.Write(fileBytes, 0, fileBytes.Length);
        //            //        sw.Close();
        //            //        return new FileStreamResult(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Delete), model.FileType);
        //            //    }
        //            //}
        //            //finally
        //            //{
        //            //    if (File.Exists(file))
        //            //    {
        //            //        File.Delete(file);
        //            //    }
        //            //}
        //            return new FileStreamResult(new MemoryStream(Convert.FromBase64String(model.Context)), model.FileType);
        //        case (int)ESaveLocationType.FileDB:
        //            break;
        //    }
        //    return null;
        //}

        /// <summary>
        /// 根据文件ID获取该文件的路径
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns></returns>
        public static string GetPathById(int id)
        {
            return FileStorageDal.GetEntityByID(id).SavePath;
        }

        //public static void TransFileToBase64(FileStorage file)
        //{
        //    try
        //    {
        //        switch (file.SaveLocationType)
        //        {
        //            case (int)ESaveLocationType.WebFolder:
        //                using (FileStream fs = new FileStream(file.SavePath, FileMode.Open))
        //                {
        //                    byte[] fileBytes = new byte[fs.Length];
        //                    fs.Read(fileBytes, 0, fileBytes.Length);
        //                    fs.Close();
        //                    file.Context = Convert.ToBase64String(fileBytes);
        //                }
        //                break;
        //            case (int)ESaveLocationType.FileServer:
        //                break;
        //            case (int)ESaveLocationType.LocalDB:
        //                break;
        //            case (int)ESaveLocationType.MongoDB:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //model.Context = "Error:File Lost.";

        //        string lostPicturePath = ConfigurationManager.AppSettings["LostPicturePath"];
        //        string physicsPath = ConfigurationManager.AppSettings["PhysicsPath"];
        //        using (FileStream fs = new FileStream(physicsPath + lostPicturePath, FileMode.Open))
        //        {
        //            byte[] fileBytes = new byte[fs.Length];
        //            fs.Read(fileBytes, 0, fileBytes.Length);
        //            fs.Close();
        //            file.Context = Convert.ToBase64String(fileBytes);
        //        }
        //    }
        //}

        //public static void TransBase64ToFile(FileStorage file)
        //{
        //    switch (file.SaveLocationType)
        //    {
        //        case (int)ESaveLocationType.WebFolder:
        //            using (Stream sw = new FileStream(file.SavePath, FileMode.Create, FileAccess.Write, FileShare.None))
        //            {
        //                byte[] fileBytes = Convert.FromBase64String(file.Context);
        //                sw.Write(fileBytes, 0, fileBytes.Length);
        //                sw.Close();
        //                file.Context = string.Empty;
        //            }
        //            break;
        //        case (int)ESaveLocationType.FileServer:
        //            break;
        //        case (int)ESaveLocationType.LocalDB:
        //            break;
        //        case (int)ESaveLocationType.MongoDB:
        //            break;
        //    }
        //}
    }
}
