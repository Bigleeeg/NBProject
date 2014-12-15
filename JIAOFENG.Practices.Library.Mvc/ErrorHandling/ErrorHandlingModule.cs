using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JIAOFENG.Practices.Library.Mvc.ErrorHandling
{
    /// <summary>
    /// 异常处理的Module
    /// </summary>
    public class ErrorHandlingModule : IHttpModule
    {
        public void Dispose()
        {
            
        }
        private void OnBeginRequest(object sender, EventArgs e)  
        {  
            //HttpApplication http = sender as HttpApplication;  
            //http.Response.Write("请求开始了");  
        }  
        private void OnEndRequest(object sender, EventArgs e)  
        {  
            //HttpApplication http = sender as HttpApplication;  
            //http.Response.Write("请求结束了");  
        }  

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context">HttpApplication</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += this.OnBeginRequest;
            context.EndRequest += this.OnEndRequest;  

            context.Error += delegate (object sender, EventArgs args)
            {
                var httpContext = ((HttpApplication)sender).Context;
                var errorRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
                Func<string, string> extract = delegate(string key)
                {
                    if (errorRouteData != null)
                    {
                        if (errorRouteData.Values[key] != null && !String.IsNullOrEmpty(errorRouteData.Values[key].ToString()))
                        {
                            return errorRouteData.Values[key].ToString();
                        }
                    }
                    return " ";
                };

                var errorController = extract("controller");
                var errorAction = extract("action");
                var exception = httpContext.Server.GetLastError();
                var controller = new ErrorController();
                var routeData = new RouteData();

                if (exception != null)
                {
                    //ILog logger = ObjectContainer.ResolveService<ILogFactory>().GetLogger(exception.GetType());
                    //logger.Error(exception.Message, exception);
                }

                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = exception is HttpException ? ((HttpException)exception).GetHttpCode() : 500;
                httpContext.Response.TrySkipIisCustomErrors = true;

                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = DecideAction(exception);
                controller.ViewData.Model = new HandleErrorInfo(exception, errorController, errorAction);
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            };
        }

        private string DecideAction(Exception exception)
        {
            const string DefaultAction = "Exception";

            var mapping = new Dictionary<int, string> 
                                {
                                    {404,"NotFound"},
                                    {403,"Forbidden"} ,
                                    {401,"Unauthorized"} 
                                };

            var httpException = exception as HttpException;
            if (httpException == null) return DefaultAction;
            int statusCode = httpException.GetHttpCode();
            if (mapping.Keys.Contains<int>(statusCode)) return mapping[statusCode];

            return DefaultAction;
        }
    }
}