using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Mail
{
    //internal enum AttachmentType
    //{
    //    Send = 1, //发送邮件附件
    //    Receive = 2//接收邮件附件
    //}
    public enum OutMailStatus
    {
        UnSend = 1,//待发送/尚未发送出去

        Success = 2,//成功发送

        Fail = 3//多次发送失败，不再尝试
    }
    internal enum InMailStatus
    {
        UnDealwith = 1,
        Finished
    }
}
