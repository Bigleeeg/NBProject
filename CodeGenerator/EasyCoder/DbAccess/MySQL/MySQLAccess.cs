using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;


namespace EasyCoder
{
    public class MySQLAccess : IDbAccess
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
                    this.dbcn = new MySqlConnection(this.connectionString);
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
            MySqlTransaction tran = null;

            try
            {
                tran = (MySqlTransaction)this.dbcn.BeginTransaction();

                MySqlCommand command = (MySqlCommand)this.dbcn.CreateCommand(); 

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

            using (MySqlCommand command = (MySqlCommand)this.dbcn.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = SqlCommand;

                using (MySqlDataAdapter adapt = new MySqlDataAdapter(command))
                {
                    adapt.Fill(ds);
                }
            }

            return ds.Tables[0];
        }

        #endregion
    }
}
