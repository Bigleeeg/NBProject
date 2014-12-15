using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class Menu
    {
        public Menu()
        {
            this.Buttons = new List<Button>();
        }
        public List<Button> Buttons { get; set; }
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"button\":[");
            for (int i = 0; i < Buttons.Count; i++)
            {
                sb.Append(Buttons[i].ToJson());
                if (i != Buttons.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]}");
            return sb.ToString();
        }
    }
}
