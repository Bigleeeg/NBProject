using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class ClickButton : Button
    {
        public string Type { get; set; }
        public string Key { get; set; }
        protected override string ToJsonNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToJsonNode());
            sb.AppendFormat(JsonFormat + ",", "type", this.Type);
            sb.AppendFormat(JsonFormat, "key", this.Key);           
            return sb.ToString();
        }
    }
}
