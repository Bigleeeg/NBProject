using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class ViewButton : Button
    {
        public string Type { get; set; }
        public string Url { get; set; }
        protected override string ToJsonNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToJsonNode());
            sb.AppendFormat(JsonFormat + ",", "url", this.Url);
            sb.AppendFormat(JsonFormat, "type", this.Type);
            
            return sb.ToString();
        }
    }
}
