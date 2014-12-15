using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat.Response.Message
{
    public class VideoMessage : BaseMessage
    {
        public Video Video { get; set; }
        protected override string ToXmlNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToXmlNode());
            sb.Append("<Video>");
            sb.AppendFormat(string.Format(CDATAFormat, "MediaId"), this.Video.MediaId);
            sb.AppendFormat(string.Format(CDATAFormat, "ThumbMediaId"), this.Video.ThumbMediaId);
            sb.Append("</Video>");
            return sb.ToString();
        }
    }
}
