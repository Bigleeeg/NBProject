using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class WechatBaseController : JIAOFENG.Practices.Library.Mvc.BaseController
    {
        public string WechatToken 
        { 
            get
            {
                return ConfigurationManager.AppSettings["WechatToken"];
            }
        }
        public Token ClientAccessToken
        {
            get
            {
                Token accessToken = this.HttpContext.Application["ClientAccessToken"] as Token;
                if (accessToken == null || !accessToken.IsValid)
                {
                    string appId = ConfigurationManager.AppSettings["WechatAppId"];
                    string appSecret = ConfigurationManager.AppSettings["AppSecret"];
                    string requestUrl = ConfigurationManager.AppSettings["ClientAccessTokenUrl"];
                    accessToken = Token.GetClientAccessToken(appId, appSecret, requestUrl);
                    ClientAccessToken = accessToken;
                }
                return accessToken;
            }
            set
            {
                this.HttpContext.Application["ClientAccessToken"] = value;
            }
        }
        
        public WechatOauth2Token BaseAccessToken
        {
            get
            {
                return this.HttpContext.Session["BaseAccessToken"] as WechatOauth2Token;
            }
            set
            {
                this.HttpContext.Session["BaseAccessToken"] = value;
            }
        }
    }
}
