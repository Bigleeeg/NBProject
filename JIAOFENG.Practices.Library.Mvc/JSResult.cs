using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JIAOFENG.Practices.Library.Mvc
{
    public class JSResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            const string format = "<script type='text/javascript'>{0}</script>";
            HttpResponseBase response = context.HttpContext.Response;
            response.Write(string.Format(format, this.Script));
        }
        public string Script { get; set; }
    }
}
