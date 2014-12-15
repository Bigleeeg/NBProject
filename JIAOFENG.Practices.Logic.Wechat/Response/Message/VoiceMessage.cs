using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat.Response.Message
{
    public class VoiceMessage : BaseMessage
    {
        public Voice Voice { get; set; }
        protected override string ToXmlNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToXmlNode());
            sb.Append("<Voice>");
            sb.AppendFormat(string.Format(CDATAFormat, "MediaId"), this.Voice.MediaId);
            sb.Append("</Voice>");
            return sb.ToString();
        }
    }
}
