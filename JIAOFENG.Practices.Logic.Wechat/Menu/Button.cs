using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class Button
    {
        public const string JsonFormat = "\"{0}\":\"{1}\"";
        public string Name { get; set;}
        protected virtual string ToJsonNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(JsonFormat + ",", "name", this.Name);
            return sb.ToString();
        }
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append(ToJsonNode());
            sb.Append("}");
            return sb.ToString();
        }
    }
}
