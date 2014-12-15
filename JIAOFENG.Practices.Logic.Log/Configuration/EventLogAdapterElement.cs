using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Log
{
    /// <summary>
    /// 所有的实现都在AdapterElement，此子类没用上
    /// </summary>
    public class EventLogAdapterElement : AdapterElement
    {
        public override ILogTarget CreateLogTarget()
        {
            return new EventLogAdapter(EventLogSourceName, EventLogFileName);
        }

        [ConfigurationProperty("eventLogSourceName", IsRequired = true)]
        public string EventLogSourceName
        {
            get { return this["eventLogSourceName"].ToString(); }
            set { this["eventLogSourceName"] = value; }
        }

        [ConfigurationProperty("eventLogFileName", IsRequired = true)]
        public string EventLogFileName
        {
            get { return this["eventLogFileName"].ToString(); }
            set { this["eventLogFileName"] = value; }
        }
    }
}
