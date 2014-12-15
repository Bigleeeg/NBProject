using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class ComplexButton : Button
    {
        public ComplexButton()
        {
            this.SubButtons = new List<Button>();
        }
        public List<Button> SubButtons { get; set; }
        protected override string ToJsonNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToJsonNode());
            sb.Append("\"sub_button\":[");
            for (int i = 0; i < SubButtons.Count; i++)
            {
                sb.Append(SubButtons[i].ToJson());
                if (i != SubButtons.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");          
            return sb.ToString();
        }
    }
}
