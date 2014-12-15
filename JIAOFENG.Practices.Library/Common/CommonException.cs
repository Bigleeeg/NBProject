using System;
using System.Runtime.Serialization;

namespace JIAOFENG.Practices.Library.Common
{
    /// <summary>
    /// Dashing系统异常基类
    /// </summary>
    [Serializable]
    public class CommonException : Exception, ISerializable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonException()
            : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        public CommonException(string message, string errorCode = "")
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="innerException">内容异常</param>
        public CommonException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="innerException">内容异常</param>
        public CommonException(string message, string errorCode, System.Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        public CommonException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
        public string ErrorCode { get; private set; }
    }
}
