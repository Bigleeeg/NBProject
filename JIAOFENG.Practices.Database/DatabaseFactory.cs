using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Database
{
    public static class DatabaseFactory
    {
        public static Database CreateDatabase()
        {
            return CreateDatabase("DBMain");
        }
        public static Database CreateDatabase(DatabaseProviderType provideType, string connectionString)
        {
            switch (provideType)
            {
                case DatabaseProviderType.SqlServer:
                    return new SqlDatabase(connectionString);
                case DatabaseProviderType.Oracle:
                    return new OracleDatabase(connectionString);
                case DatabaseProviderType.MySQL:
                    return new MySQLDatabase(connectionString);
                default:
                    return new SqlDatabase(connectionString);
            }
        }
        public static Database CreateDatabase(string providerName, string connectionString)
        {
            DatabaseProviderType provideType = (DatabaseProviderType)Enum.Parse(typeof(DatabaseProviderType), providerName);
            return CreateDatabase(provideType, connectionString);  
        }
        public static Database CreateDatabase(string connectionStringKey)
        {
            if (string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings[connectionStringKey].ProviderName))
            {
                throw new ArgumentNullException("对于\"" + connectionStringKey + "\"数据库连接配置项的providerName需要配置，可选名称为SqlServer、Oracle或者MySQL。");
            }
            return CreateDatabase(ConfigurationManager.ConnectionStrings[connectionStringKey].ProviderName, ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString);
        }
    }
}
