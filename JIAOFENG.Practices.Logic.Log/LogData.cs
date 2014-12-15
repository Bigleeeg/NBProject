using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Logic.Log
{
    public class LogData
    {
        public int LogID { get; set; }

        public string Title { get; set; }
        public string Action { get; set; }
        public string TargetCode { get; set; }
        public string TargetName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int CreateBy { get; set; }
        public string CreateUserName { get; set; }
        public DateTime CreateTime { get; set; }

        public int EventTypeID { get; set; }
        public string EventTypeName { get; set; }        
    }
}
