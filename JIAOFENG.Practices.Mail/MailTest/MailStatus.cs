using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailTest.TestApplication
{
    public enum MailStatus
    {
        //还未发出去
        UnSend = 1,
        //成功发送
        Success = 2,
        //多次尝试也未能发送出去
        Fail = 3
    }
}
