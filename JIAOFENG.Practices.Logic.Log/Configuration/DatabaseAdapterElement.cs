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
    public class DatabaseAdapterElement : AdapterElement
    {
        public override ILogTarget CreateLogTarget()
        {
            JIAOFENG.Practices.Database.Database database;
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                database = JIAOFENG.Practices.Database.DatabaseFactory.CreateDatabase();
            }
            else
            {
                database = JIAOFENG.Practices.Database.DatabaseFactory.CreateDatabase(ProviderName, ConnectionString);
            }
            return new MySQLDatabaseAdapter(database);
        }

        [ConfigurationProperty("providerName", IsRequired = false)]
        public string ProviderName
        {
            get { return this["providerName"].ToString(); }
            set { this["providerName"] = value; }
        }

        [ConfigurationProperty("connectionString", IsRequired = false)]
        public string ConnectionString
        {
            get { return this["connectionString"].ToString(); }
            set { this["connectionString"] = value; }
        }
    }
}
