using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.ServiceModel;
using JIAOFENG.Practices.Library.Common;

namespace JIAOFENG.Practices.Library.Mvc.ErrorHandling
{
    /// <summary>
    /// Custom Exception Filter，记录错误到系统日志，跳转到指定View。
    /// </summary>
    public class DashingHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 处理错误并保存错误到系统日志
        /// </summary>
        /// <param name="filterContext">ExceptionContext</param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(filterContext.Exception) && !(filterContext.Exception is FaultException))
            {
                return;
            }

            if (filterContext.Exception is FaultException)
            {
                string faultCode = ((FaultException)filterContext.Exception).Code.Name;
                FaultCodeAttribute attribute = ExceptionType.GetCustomAttribute<FaultCodeAttribute>();
                if (attribute != null)
                {
                    if (!faultCode.Equals(attribute.Name)) return;
                }
                else
                {
                    if (!faultCode.Equals(ExceptionType.Name)) return;
                }
            }

            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult 
                { 
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet, 
                    Data = new 
                    { 
                        error = true,
                        message = filterContext.Exception.Message
                    } 
                };
            }
            else
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            
                filterContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
