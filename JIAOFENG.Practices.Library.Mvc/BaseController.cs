using JIAOFENG.Practices.Library.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JIAOFENG.Practices.Library.Mvc
{
    public class BaseController : Controller
    {
        public string ToJSON(object o)
        {
            return JsonHelper.ToJSON(o);
        }
        public T ToObjectFromJSON<T>(string json)
        {
            return JsonHelper.ToObjectFromJSON<T>(json);
        }
        public JsonResult Json(bool result, string message)
        {
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}
