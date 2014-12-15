using JIAOFENG.Practices.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.File
{
    public class FilePackage : EntityExtInfo
    {
         #region constructor
        public FilePackage()
        {
            FilePackageName = Guid.NewGuid().ToString();
            FileStorages = new FileStorageList(this);
        }
        public FilePackage(FilePackage entity)
        {
			CopyFrom(entity);
        }
		#endregion
        #region property

        public int FilePackageID { get; set; }

        public string FilePackageName { get; set; }

        public bool IsDelete { get; set; }

        /// <summary>
        /// 子文件集合
        /// </summary>
        public FileStorageList FileStorages { get; private set; }

        #endregion

        public void CopyFrom(FilePackage entity)
        {
            this.FilePackageID = entity.FilePackageID;
            this.FilePackageName = entity.FilePackageName;
            this.CreateTime = entity.CreateTime;
            this.CreateBy = entity.CreateBy;
            this.UpdateTime = entity.UpdateTime;
            this.UpdateBy = entity.UpdateBy;
            this.FileStorages = entity.FileStorages;
        }
        /// <summary>
        /// 保存文件夹及文件
        /// </summary>
        public void Save()
        {
            if (this.FilePackageID > 0)
            {
                FilePackageDal.Update(this);

                List<FileStorage> oldList = FileStorageDal.GetList("FilePackageID", this.FilePackageID);
                IList<FileStorage> addList;
                IList<FileStorage> updateList;
                IList<FileStorage> deleteList;
                this.FileStorages.CompareDiff(oldList, out addList, out updateList, out deleteList);
                foreach (FileStorage file in addList)
                {
                    file.FilePackageID = this.FilePackageID;
                    file.Save();
                }
                foreach (FileStorage file in updateList)
                {
                    file.FilePackageID = this.FilePackageID;
                    file.Save();
                }
                foreach (FileStorage file in deleteList)
                {
                    file.FilePackageID = this.FilePackageID;
                    file.Delete();
                }
            }
            else if (this.FilePackageID == 0)
            {
                this.FilePackageID = FilePackageDal.Add(this).FilePackageID;
                foreach (FileStorage file in this.FileStorages)
                {
                    file.FilePackageID = this.FilePackageID;
                    file.Save();
                }
            }
        }
        /// <summary>
        /// 根据文件夹ID获取文件夹及文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static FilePackage GetEntityById(int id)
        {
            if (id<=0)
            {
                throw new ArgumentOutOfRangeException("id", "找不到相关的文件包");
            }
            var filePackage = FilePackageDal.GetEntityByID(id);
            if (filePackage == null)
                throw new ArgumentOutOfRangeException("id", "找不到相关的文件包");
            var items = FileStorageDal.GetList("FilePackageID", filePackage.FilePackageID);
            filePackage.FileStorages.AddRange(items);
            return filePackage;
        }
        /// <summary>
        /// 根据文件夹Name获取文件夹及文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static FilePackage GetEntityByName(string filePackageName)
        {
            var filePackage = FilePackageDal.GetEntityByColumnValue("FilePackageName", filePackageName);
            if (filePackage == null)
                throw new ArgumentOutOfRangeException("id", "找不到相关的文件包");
            var items = FileStorageDal.GetList("FilePackageID", filePackage.FilePackageID);
            filePackage.FileStorages.AddRange(items);
            return filePackage;
        }
        public static bool Delete(int id)
        {
            return FilePackageDal.Delete(id);
        }
    }
}
