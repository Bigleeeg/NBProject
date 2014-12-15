using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace EasyCoder
{
    public class OracleAccess : IDbAccess
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
                    this.dbcn = new System.Data.OleDb.OleDbConnection(this.connectionString);
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
            System.Data.OleDb.OleDbTransaction tran = null; 
            try
            {
                tran = (System.Data.OleDb.OleDbTransaction)this.dbcn.BeginTransaction();

                System.Data.OleDb.OleDbCommand command = (System.Data.OleDb.OleDbCommand)this.dbcn.CreateCommand();

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

            using (System.Data.OleDb.OleDbCommand command = (System.Data.OleDb.OleDbCommand)this.dbcn.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = SqlCommand;

                using (System.Data.OleDb.OleDbDataAdapter adapt = new System.Data.OleDb.OleDbDataAdapter(command))
                {
                    adapt.Fill(ds);
                }
            }

            return ds.Tables[0];
        }

        public int ExecuteSql(string SQLString, string _oraConn)
        {
            using (OracleConnection connection = new OracleConnection(_oraConn))
            {

                try
                {
                    OracleCommand cmd = new OracleCommand(SQLString, connection);
                    connection.Open(); 
                    int rows = cmd.ExecuteNonQuery();
                    return rows; 
                }
                catch (Exception E)
                {
                    connection.Close();
                    throw new Exception("DbHelperOra.ExecuteSql" + E.Message);
                }
            }
        }

        public DataTable ExecuteDataTableSql(string SQLString, string _oraConn)
        {
            using (OracleConnection connection = new OracleConnection(_oraConn))
            {

                try
                {
                    DataSet dataSet = new DataSet();
                    connection.Open();
                    OracleDataAdapter OraDA = new OracleDataAdapter(SQLString, _oraConn);
                    OraDA.Fill(dataSet, "ds"); 
                    return dataSet.Tables[0];
                }
                catch (Exception E)
                {
                    connection.Close();
                    throw new Exception("DbHelperOra.ExecuteSql" + E.Message);
                }
            }
        }


        #endregion
    }
}
