using JIAOFENG.Practices.Library.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class Token
    {
        public string access_token { get; set; }

        private int _expires_in;
        /// <summary>
        /// 凭证有效期，单位：秒
        /// </summary>
        public int expires_in 
        { 
            get
            {
                return _expires_in;
            }
            set
            {
                _expires_in = value;
                this.DeadLine = DateTime.Now.AddSeconds(value - 60);//提前（60秒/1分钟）过期
            }
        }
        public DateTime DeadLine { get; private set;}
        public bool IsValid
        {
            get
            {
                return DeadLine >= DateTime.Now;
            }            
        }
        public static Token GetClientAccessToken(string appId, string appSecret, string requestUrl)
        {
            //string appId = ConfigurationManager.AppSettings["WechatAppId"];
            //string appSecret = ConfigurationManager.AppSettings["AppSecret"];
            //string requestUrl = ConfigurationManager.AppSettings["ClientAccessTokenUrl"];
            requestUrl = string.Format(requestUrl, appId, appSecret);
            string result = CommonUtil.HttpRequest(requestUrl);
            Token token = JsonHelper.ToObjectFromJSON<Token>(result);
            return token;
        }
    }
}
