using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace JIAOFENG.Practices.Logic.Log
{
    /// <summary>
    /// 所有的实现都在AdapterElement，此子类没用上
    /// </summary>
    public class FileAdapterElement : AdapterElement
    {
        public override ILogTarget CreateLogTarget()
        {
            return new FileAdapter(LogFilePath);
        }

        [ConfigurationProperty("logFilePath", IsRequired = true)]
        public string LogFilePath
        {
            get { return this["logFilePath"].ToString(); }
            set { this["logFilePath"] = value; }
        }
    }
}
