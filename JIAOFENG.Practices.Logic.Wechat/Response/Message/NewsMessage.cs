using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat.Response.Message
{
    public class NewsMessage : BaseMessage
    {
        public List<Article> Articles { get; set; }
    }
}
