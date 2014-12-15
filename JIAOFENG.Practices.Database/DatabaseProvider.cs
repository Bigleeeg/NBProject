using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Database
{
    public class DatabaseProvider
    {
        public DatabaseProviderType ProviderType { get; set; }
        public string ConnectionString { get; set; }
        public DatabaseProvider(DatabaseProviderType providerType, string connectionString)
        {
            this.ProviderType = providerType;
            this.ConnectionString = connectionString;
        }
    }
}
