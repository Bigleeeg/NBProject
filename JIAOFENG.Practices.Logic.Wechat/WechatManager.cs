using JIAOFENG.Practices.Database;
using JIAOFENG.Practices.Logic.Wechat.Response.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public static class WechatManager
    {
        static WechatManager()
        {
            Database = DatabaseFactory.CreateDatabase();
        }
        public static JIAOFENG.Practices.Database.Database Database { get; set; }

        #region search
        public static List<WechatpushEntity> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            List<WechatpushEntity> entityList = GetPushDal().GetWaitingSendMails(failInterval, failMostNum);

            return entityList;
        }
        #endregion

        #region update
        public static void UpdataMailStatus(int wechatPushID, PushStatus status)
        {
            GetPushDal().UpdataMailStatus(wechatPushID, status);
        }
        public static void UpdateSendCount(int wechatPushID)
        {
            GetPushDal().UpdateSendCount(wechatPushID);
        }
        #endregion

        #region Push/Add
        public static void Add(TemplateMessage templateMessage, int creator)
        {
            WechatpushEntity entity = new WechatpushEntity();
            entity.ToUser = templateMessage.ToUser;
            entity.TemplateId = templateMessage.TemplateId;
            entity.Url = templateMessage.Url;
            entity.TopColor = templateMessage.TopColor;
            entity.BodyXml = JIAOFENG.Practices.Library.Utility.XmlHelper.SerializeToXml(templateMessage);
            entity.Status = (int)PushStatus.UnSend;
            entity.SendCount = 0;
            entity.Priority = 1;
            entity.BodyEncoding = "utf-8";
            entity.CreateUserName = string.Empty;
            entity.CreateBy = creator;
            entity.CreateTime = DateTime.Now;

            GetPushDal().Add(entity);
        }

        #endregion

        #region private method
        internal static IWechatPushDal GetPushDal()
        {
            IWechatPushDal dal = null;
            switch (Database.DatabaseProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    dal = new MySQLWechatPushDal(Database);
                    break;
                case DatabaseProviderType.Oracle:
                    dal = new MySQLWechatPushDal(Database);
                    break;
                case DatabaseProviderType.MySQL:
                    dal = new MySQLWechatPushDal(Database);
                    break;
                default:
                    dal = new MySQLWechatPushDal(Database);
                    break;
            }
            return dal;
        }
        #endregion
    }
}
