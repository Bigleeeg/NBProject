using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.Common
{
    public class ReturnResult
    {
        public ReturnResult(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
        }
        public ReturnResult(bool result, ResultCode code)
        {
            this.Result = result;
            this.Code = code;
        }
        public ReturnResult(bool result, ResultCode code, string message)
        {
            this.Result = result;
            this.Message = message;
            this.Code = code;
        }
        public ReturnResult()
        {

        }
        public bool Result { get; set; }//结果，成功或失败
        public ResultCode Code { get; set; }//返回码
        public string Message { get; set; }
        public string Other { get; set; }

        public enum ResultCode
        {
            UnKnown = -1,//未设定，未知
            Success = 0, //成功
            SessionTimeout = 10, //Session超时
            AuthenticationFailure = 20,//身份认证失败
            ExecuteError = 30,//操作执行错误
            SystemError = 100 //系统内部错误
        }
    }
}
