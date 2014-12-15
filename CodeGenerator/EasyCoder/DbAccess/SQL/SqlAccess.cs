using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyCoder
{
    public class SqlAccess:IDbAccess
    {
        private string connectionString = string.Empty;

        public string ConnectionString
        {
            get
            { 
                if (connectionString == "")
                    connectionString = System.Configuration.ConfigurationSettings.AppSettings[""];

                return connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }

        private System.Data.IDbConnection dbcn;

        public System.Data.IDbConnection DbConnection
        {
            get
            {
                if (this.dbcn == null)
                {
                    this.dbcn = new System.Data.SqlClient.SqlConnection(this.connectionString);
                }
                return this.dbcn;
            }
            set
            {
                this.dbcn = value;
            }
        }

        #region IDbAccess 成员

        public int Execute(string SqlCommand)
        {
            int count = 0;
            System.Data.SqlClient.SqlTransaction tran = null;

            try
            {
                tran = (System.Data.SqlClient.SqlTransaction)this.dbcn.BeginTransaction();

                System.Data.SqlClient.SqlCommand command = (System.Data.SqlClient.SqlCommand)this.dbcn.CreateCommand();

                command.Transaction = tran;

                command.CommandText = SqlCommand;
                command.CommandType = System.Data.CommandType.Text;

                count = command.ExecuteNonQuery();

                tran.Commit();
            }
            catch (System.Exception ex)
            {
                tran.Rollback();
                throw new Exception(ex.Message);
            }

            return count;
        } 

        public System.Data.DataTable Query(string SqlCommand)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            if (this.DbConnection == null)
                throw new Exception("数据库连接资源已释放！");

            using (System.Data.SqlClient.SqlCommand command = (System.Data.SqlClient.SqlCommand)this.dbcn.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = SqlCommand;

                using (System.Data.SqlClient.SqlDataAdapter adapt = new System.Data.SqlClient.SqlDataAdapter(command))
                {
                    adapt.Fill(ds);
                }
            }

            return ds.Tables[0];
        }

        #endregion
    }
}
