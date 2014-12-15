using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.AppPush
{
    public class AppPushResult
    {
        public string result { get; set; }
        public string contentId { get; set; }
        public bool IsOk 
        {
            get
            {
                return string.Equals(result, "ok");
            }
        }
    }
}
