using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public abstract class ServiceHandler : IServiceHandler
    {
        public static int ConvertDateTimeToInt(System.DateTime time)
        {
            return CommonUtil.ConvertDateTimeToInt(time);
        }
        public abstract string HandleService(string objReceived);
    }
}
