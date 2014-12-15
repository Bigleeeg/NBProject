using JIAOFENG.Practices.Library.Utility;
using JIAOFENG.Practices.Logic.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class WechatOauth2Token : Token
    {
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
        public static WechatOauth2Token GetWechatOauth2Token(string code)
        {
            try
            {
                string appId = ConfigurationManager.AppSettings["WechatAppId"];
                string appSecret = ConfigurationManager.AppSettings["AppSecret"];
                string requestUrl = ConfigurationManager.AppSettings["UserAccessTokeUrl"];
                requestUrl = string.Format(requestUrl, appId, appSecret, code);
                string result = CommonUtil.HttpRequest(requestUrl);
                WechatOauth2Token token = JsonHelper.ToObjectFromJSON<WechatOauth2Token>(result);
                return token;
            }
            catch(Exception ex)
            {
                LogManager.LogError("GetWechatOauth2Token", ex.Message, "GetWechatOauth2Token", "GetWechatOauth2Token");
            }
            return null;
        }
        public static WechatOauth2Token RefreshWechatOauth2Token(string refreshToken)
        {
            WechatOauth2Token token = null;
            try
            {               
                string appId = ConfigurationManager.AppSettings["WechatAppId"];
                string requestUrl = ConfigurationManager.AppSettings["RefreshAccessTokeUrl"];
                requestUrl = string.Format(requestUrl, appId, refreshToken);

                string result = CommonUtil.HttpRequest(requestUrl);
                try
                {
                    token = JsonHelper.ToObjectFromJSON<WechatOauth2Token>(result);
                }
                catch(Exception)
                {
                    WechatResult error = JsonHelper.ToObjectFromJSON<WechatResult>(result);
                    LogManager.LogError("RefreshWechatOauth2Token", error.errmsg, "WechatResult.Message", "WechatResult.Message");
                }               
            }
            catch (Exception ex)
            {
                LogManager.LogError("RefreshWechatOauth2Token", ex.Message, "RefreshWechatOauth2Token", "RefreshWechatOauth2Token");
            }
            return token;
        }
        public static string GetUserCode()
        {
            string appId = ConfigurationManager.AppSettings["WechatAppId"];
            string requestUrl = ConfigurationManager.AppSettings["UserCodeUrl"];
            requestUrl = string.Format(requestUrl, appId, "", AccessTokenTypeEnum.snsapi_base.ToString(), 15);
            string result = CommonUtil.HttpRequest(requestUrl);
            Token token = JsonHelper.ToObjectFromJSON<Token>(result);
            return null;
        }
    }
}
