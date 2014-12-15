using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Database
{
    public class DbParameterFactory
    {
        public static DbParameter CreateDbParameter(DatabaseProviderType provideType, string parameterName, object value)
        {
            switch (provideType)
            {
                case DatabaseProviderType.SqlServer:
                    return new SqlParameter(parameterName, value);
                case DatabaseProviderType.Oracle:
                    return new OracleParameter(parameterName, value);
                case DatabaseProviderType.MySQL:
                    return new MySqlParameter(parameterName, value);
                default:
                    return new SqlParameter(parameterName, value);
            }
        }
    }
}
