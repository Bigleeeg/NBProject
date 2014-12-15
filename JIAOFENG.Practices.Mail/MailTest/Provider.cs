using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailTest.TestApplication
{
    public class Provider
    {
        public ProviderType ProviderType { get; set; }
        public string ConnectionString { get; set; }
        public Provider(ProviderType providerType, string connectionString)
        {
            this.ProviderType = providerType;
            this.ConnectionString = connectionString;
        }
    }
}
