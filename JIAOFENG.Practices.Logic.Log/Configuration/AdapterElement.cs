using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using JIAOFENG.Practices.Database;
namespace JIAOFENG.Practices.Logic.Log
{
    public class AdapterElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }
        public MediaType MediaType
        {
            get
            {
                MediaType media;
                if (Enum.TryParse(Name, true, out media))
                {
                    return media;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("name", Name, "Unrecognized MediaType member.");
                }
            }
        }
        public virtual ILogTarget CreateLogTarget()
        {
            ILogTarget ret = null;
            switch (MediaType)
            {
                case MediaType.Diagnostics:
                    ret = new DiagnosticsAdapter();
                    break;
                case MediaType.File:
                    ret = new FileAdapter(LogFileFullName);
                    break;
                case MediaType.Database:
                    JIAOFENG.Practices.Database.Database database = JIAOFENG.Practices.Database.DatabaseFactory.CreateDatabase(ProviderName, ConnectionString);
                    switch ((DatabaseProviderType)Enum.Parse(typeof(DatabaseProviderType), ProviderName))
                    {
                        case DatabaseProviderType.SqlServer:
                            ret = new SqlServerDatabaseAdapter(database);
                            break;
                        case DatabaseProviderType.Oracle:
                            ret = new OracleDatabaseAdapter(database);
                            break;
                        case DatabaseProviderType.MySQL:
                            ret = new MySQLDatabaseAdapter(database);
                            break;
                        default:
                            ret = new SqlServerDatabaseAdapter(database);
                            break;
                    }
                    break;
                case MediaType.EventLog:
                    ret = new EventLogAdapter(EventLogSourceName, EventLogFileName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("name", Name, "Unrecognized MediaType member.");
            }
            ret.Verbose = this.Verbose;
            return ret;
        }
        #region property
        [ConfigurationProperty("providerName", IsRequired = false, DefaultValue = "SqlServer")]
        public string ProviderName
        {
            get { return (string)this["providerName"]; }
        }

        [ConfigurationProperty("connectionString", IsRequired = false, DefaultValue = "")]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
        }

        [ConfigurationProperty("logFileFullName", IsRequired = false, DefaultValue = null)]
        public string LogFileFullName
        {
            get { return (string)this["logFileFullName"]; }
        }

        [ConfigurationProperty("parameter", IsRequired = false, DefaultValue = null)]
        public string Parameter
        {
            get { return (string)this["parameter"]; }
        }

        [ConfigurationProperty("eventLogSourceName", IsRequired = false)]
        public string EventLogSourceName
        {
            get { return this["eventLogSourceName"].ToString(); }
            set { this["eventLogSourceName"] = value; }
        }

        [ConfigurationProperty("eventLogFileName", IsRequired = false)]
        public string EventLogFileName
        {
            get { return this["eventLogFileName"].ToString(); }
            set { this["eventLogFileName"] = value; }
        }

        [ConfigurationProperty("verbose", DefaultValue = false, IsRequired = false)]
        public bool Verbose
        {
            get { return bool.Parse(this["verbose"].ToString()); }
            set { this["verbose"] = value; }
        }

        #endregion
    }
}
