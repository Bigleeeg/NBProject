using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat.Response.Message
{
    public class ImageMessage : BaseMessage
    {
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public Image Image { get; set; }
        protected override string ToXmlNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToXmlNode());
            sb.Append("<Image>");
            sb.AppendFormat(string.Format(CDATAFormat, "MediaId"), this.Image.MediaId);
            sb.Append("</Image>");
            return sb.ToString();
        }
    }
}
