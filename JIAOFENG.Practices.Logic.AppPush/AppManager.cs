using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using JIAOFENG.Practices.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.AppPush
{
    public static class AppManager
    {
        static AppManager()
        {
            Database = DatabaseFactory.CreateDatabase();
        }
        public static JIAOFENG.Practices.Database.Database Database { get; set; }

        #region search
        public static List<ApppushEntity> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            List<ApppushEntity> entityList = GetPushDal().GetWaitingSendMails(failInterval, failMostNum);

            return entityList;
        }
        #endregion

        #region update
        public static void UpdataMailStatus(int appPushID, PushStatus status)
        {
            GetPushDal().UpdataMailStatus(appPushID, status);
        }
        public static void UpdateSendCount(int appPushID)
        {
            GetPushDal().UpdateSendCount(appPushID);
        }
        #endregion

        #region Push/Add
        public static void Add(string subject, string summary, string body, int creator)
        {
            Add(string.Empty, string.Empty, string.Empty, subject, summary, body, string.Empty, "", creator);
        }
        public static void Add(string clientId, string subject, string summary, string body, int creator)
        {
            Add(clientId, string.Empty, string.Empty, subject, summary, body, string.Empty, "", creator);
        }
        public static void Add(string clientId, string os, string osVersion, string subject, string summary, string body, string url, string logo, int creator)
        {
            ApppushEntity entity = new ApppushEntity();
            entity.ClientID = clientId;
            entity.Client = os;
            entity.ClientVersion = osVersion;
            entity.Subject = subject;
            entity.Summary = summary;
            entity.Body = body;
            entity.Url = url;
            entity.Logo = logo;
            entity.CreateUserName = string.Empty;
            entity.Status = (int)PushStatus.UnSend;
            entity.SendCount = 0; 
            entity.IsBodyHtml = false;
            entity.Priority = 1;
            entity.BodyEncoding = "utf-8";
            entity.CreateBy = creator;
            entity.CreateTime = DateTime.Now;

            GetPushDal().Add(entity);
        }

        #endregion

        #region private method
        internal static IAppPushDal GetPushDal()
        {
            IAppPushDal dal = null;
            switch (Database.DatabaseProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    dal = new MySQLAppPushDal(Database);
                    break;
                case DatabaseProviderType.Oracle:
                    dal = new MySQLAppPushDal(Database);
                    break;
                case DatabaseProviderType.MySQL:
                    dal = new MySQLAppPushDal(Database);
                    break;
                default:
                    dal = new MySQLAppPushDal(Database);
                    break;
            }
            return dal;
        }
        #endregion
    }
}
