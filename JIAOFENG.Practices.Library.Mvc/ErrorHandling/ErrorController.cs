using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JIAOFENG.Practices.Library.Mvc.ErrorHandling
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// 异常处理首页
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View("Exception");
        }

        /// <summary>
        /// 系统错误[500]
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Exception()
        {
            return View();
        }

        /// <summary>
        /// 没有找到
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult NotFound()
        {
            return View();
        }

        /// <summary>
        /// 禁止访问
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Forbidden()
        {
            return View();
        }

        /// <summary>
        /// 没有授权
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}
