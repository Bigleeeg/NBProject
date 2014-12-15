using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class WechatOauthCode
    {
        private const string UserRejectCode = "authdeny";
        public string code { get; set; }
        public string state { get; set; }
        public bool IsValid
        {
            get
            {
                return !string.Equals(code, UserRejectCode, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
