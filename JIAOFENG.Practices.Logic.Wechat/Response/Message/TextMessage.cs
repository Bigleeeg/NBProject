using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat.Response.Message
{
    public class TextMessage : BaseMessage
    {
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }
        protected override string ToXmlNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToXmlNode());
            sb.AppendFormat(string.Format(CDATAFormat, "Content"), this.Content);
            return sb.ToString();
        }
    }
}
