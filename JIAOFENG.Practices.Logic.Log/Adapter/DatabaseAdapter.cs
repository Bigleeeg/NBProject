using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using JIAOFENG.Practices.Database;
using System.Data.Common;
using System.Data.SqlClient;

namespace JIAOFENG.Practices.Logic.Log
{
    public abstract class DatabaseAdapter : LogTarget
    {
        protected JIAOFENG.Practices.Database.Database database = null;
        public DatabaseAdapter(JIAOFENG.Practices.Database.Database db)
        {
            this.database = db;
        }
    }
}
