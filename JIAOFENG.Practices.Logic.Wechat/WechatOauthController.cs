using JIAOFENG.Practices.Logic.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class WechatOauthController : WechatBaseController
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.BaseAccessToken == null)
            {
                GetBaseAccessToken(filterContext);
                filterContext.Result = new EmptyResult();
            }
            else if(!this.BaseAccessToken.IsValid)
            {
                this.BaseAccessToken = WechatOauth2Token.RefreshWechatOauth2Token(this.BaseAccessToken.refresh_token);
                if (this.BaseAccessToken == null)//如果刷新Token失败，则重新授权
                {
                    GetBaseAccessToken(filterContext);
                }
            }
        }
        protected void GetBaseAccessToken(AuthorizationContext filterContext)
        {
            string appId = ConfigurationManager.AppSettings["WechatAppId"];
            string requestUrl = ConfigurationManager.AppSettings["UserCodeUrl"];
            UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);

            //string controllerName = RouteData.Values["controller"].ToString();
            //string actionName = RouteData.Values["action"].ToString();
            //string currUrl = urlHelper.Action(actionName, controllerName);
            //string currUrlFullPath = Request.Url.AbsoluteUri;
            //string currUrlNormal = "/" + controllerName + "/" + actionName;
            //LogManager.LogDebug("OAuth", currUrl, "currUrl", "currUrl");
            //int index = currUrl.IndexOf(currUrlNormal, StringComparison.OrdinalIgnoreCase);
            //LogManager.LogDebug("OAuth", currUrlNormal, "currUrlNormal", "currUrlNormal");
            //if (index < 0)//省略路径
            //{
            //    currUrl = currUrl.TrimEnd('/') + currUrlNormal;
            //}
            //else
            //{

            //}
            //LogManager.LogDebug("OAuth", currUrlFullPath, "currUrlFullPath", "currUrlFullPath");
            //LogManager.LogDebug("OAuth", currUrl, "currUrl", "currUrl");
            //index = currUrlFullPath.IndexOf(currUrl, StringComparison.OrdinalIgnoreCase);
            //if (index > 0)//全路径
            //{
            //    currUrlFullPath = currUrlFullPath.Substring(0, index);
            //}
            //else
            //{

            //}
            //LogManager.LogDebug("OAuth", currUrlFullPath, "currUrlFullPath", "currUrlFullPath");
            string urlOAuth = urlHelper.Action("OAuth", "Wechat");
            //LogManager.LogDebug("OAuth", urlOAuth, "urlOAuth", "urlOAuth");
            //LogManager.LogDebug("OAuth", currUrlFullPath, "currUrlFullPath", "currUrlFullPath");
            urlOAuth = "http://" + Request.Url.Host + urlOAuth;
            LogManager.LogDebug("OAuth", urlOAuth, "urlOAuth", "urlOAuth");
            requestUrl = string.Format(requestUrl, appId, urlHelper.Encode(urlOAuth), AccessTokenTypeEnum.snsapi_base.ToString(), urlHelper.Encode(Request.Url.AbsoluteUri));
            filterContext.HttpContext.Response.Redirect(requestUrl);           
        }
    }
}
