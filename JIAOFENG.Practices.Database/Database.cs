using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using JIAOFENG.Practices.Library.Common;

namespace JIAOFENG.Practices.Database
{
    public abstract class Database
    {
        #region private data fields
        //The default time (in seconds) to wait for a SQL command to execute is 60 seconds.
        protected const int DEFAULT_COMMAND_TIMEOUT = 60;
        #endregion

        #region constructor
        public Database(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString", "ConnectionString should be Initialized.");
            }
            this.ConnectionString = connectionString;
        }

        #endregion
        #region property
        public DatabaseProviderType DatabaseProviderType { get; set; }
        public string ConnectionString { get; set; }
        #endregion

        #region private utility methods

        protected void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            foreach (DbParameter p in commandParameters)
            {
                if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        protected int AssignParameterValues(DbParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                return 0;
            }

            int numberOfParameters = commandParameters.Length;
            if (numberOfParameters != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }


            for (int i = 0; i < commandParameters.Length; i++)
            {
                commandParameters[i].Value = parameterValues[i];
            }
            return numberOfParameters;
        }
        protected void PrepareCommand(DbCommand command, DbConnection conn, CommandType commandType, string commandText, int commandTimeout, DbParameter[] commandParameters)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            //associate the connection with the command
            command.Connection = conn;

            //set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            //set the timeout for the sqlcommand
            command.CommandTimeout = commandTimeout;

            //set the command type
            command.CommandType = commandType;

            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }
        #endregion private utility methods & constructors

        #region ExecuteNonQuery
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(commandType, commandText, (DbParameter[])null);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            //pass through the call providing 30 as the default for CommandTimeout
            return (ExecuteNonQuery(commandType, commandText, DEFAULT_COMMAND_TIMEOUT, commandParameters));
        }
        public int ExecuteNonQuery(CommandType commandType, string commandText, List<DbParameter> commandParameters)
        {
            //pass through the call providing 30 as the default for CommandTimeout
            return (ExecuteNonQuery(commandType, commandText, DEFAULT_COMMAND_TIMEOUT, commandParameters.ToArray()));
        }

        public abstract int ExecuteNonQuery(CommandType commandType, string commandText, int commandTimeout, params DbParameter[] commandParameters);

        public abstract int ExecuteNonQuery(string spName, params object[] parameterValues);

        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// Overloaded. Executes a Transact-SQL statement and returns a DataSet. 
        /// </summary>
        /// <permission cref="System.Security.PermissionSet">Public</permission>
        /// <example>
        /// <code>
        /// DataSet ds = ExecuteDataSet("Select * From Orders");
        /// </code>
        /// </example>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(CommandType.Text, commandText);
        }

        /// <summary>
        /// Overloaded. Executes a Transact-SQL statement and returns a DataSet. 
        /// </summary>
        /// <permission cref="System.Security.PermissionSet">Public</permission>
        /// <example>
        /// <code>
        /// DataSet ds = ExecuteDataSet(CommandType.StoredProcedure, "GetOrders");
        /// </code>
        /// </example>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of SqlParameters
            return ExecuteDataSet(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// Overloaded. Executes a Transact-SQL statement and returns a DataSet. 
        /// </summary>
        /// <remarks>
        /// Uses provided parameter values.
        /// 
        /// This method will query the database to discover the parameters for the stored procedure 
        /// (the first time each stored procedure is called), and assign the values based on parameter order.
        /// 
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// </remarks>
        /// <permission cref="System.Security.PermissionSet">Public</permission>
        /// <example>
        /// <code>
        /// DataSet ds = ExecuteDataSet(CommandType.StoredProcedure, "GetOrders", "Beijing", 36);
        /// </code>
        /// </example>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            return ExecuteDataSet(commandType, commandText, DEFAULT_COMMAND_TIMEOUT, commandParameters);
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public abstract DataSet ExecuteDataSet(CommandType commandType, string commandText, int commandTimeout, params DbParameter[] commandParameters);

        public abstract DataSet ExecuteDataSet(string spName, params object[] parameterValues);

        /// <summary>
        /// 执行分页数据查询
        /// </summary>
        /// <param name="commandText">sql查询语句</param>
        /// <param name="orderBy">sql语句的排序列</param>
        /// <param name="pageIndex">显示第几页数据</param>
        /// <param name="pageSize">每页记录树</param>
        /// <param name="commandParameters">sql语句的参数</param>
        /// <returns>分页后的table</returns>
        public abstract PagedTable ExecutePagedTable(string commandText, string orderBy, int pageIndex, int pageSize, params DbParameter[] commandParameters);
       
        /// <summary>
        /// 调用存储过程返回分页数据。
        /// 要求存储过程返回两个DataTable，第一个为分页后的数据页，第二个为总的记录数（单行单列）
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public PagedTable ExecutePagedTable(string spName, int pageIndex, int pageSize, params DbParameter[] commandParameters)
        {
            DataSet ds = ExecuteDataSet(CommandType.StoredProcedure, spName, commandParameters);
            return new PagedTable(ds.Tables[0], pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
        }

        #region CET PagedTable
        /// <summary>
        /// 带CET表达式的分页数据查询
        /// 仅适用于MS SQL Server
        /// </summary>
        /// <param name="cteExpression"></param>
        /// <param name="commandText"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public virtual PagedTable ExecutePagedTable(string cteExpression, string commandText, string orderBy, int pageIndex, int pageSize, params DbParameter[] commandParameters)
        {
            throw new NotImplementedException("数据库类型不支持当前查询");
        }
        #endregion

        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(CommandType.Text, commandText);
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(commandType, commandText, (DbParameter[])null);
        }

        public object ExecuteScalar(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            return (ExecuteScalar(commandType, commandText, DEFAULT_COMMAND_TIMEOUT, commandParameters));
        }

        public abstract object ExecuteScalar(CommandType commandType, string commandText, int commandTimeout, params DbParameter[] commandParameters);

        public object ExecuteScalar(string commandText, List<DbParameter> commandParameters)
        {
            return ExecuteScalar(CommandType.Text, commandText, commandParameters.ToArray());
        }
        public abstract object ExecuteScalar(string spName, params object[] parameterValues);

        #endregion

        #region ExecuteReader
        public abstract DbDataReader ExecuteReader(string spName, params object[] parameterValues);

        public DbDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(CommandType.Text, commandText);
        }

        public DbDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of SqlParameters
            return ExecuteReader(commandType, commandText, (DbParameter[])null);
        }

        public abstract DbDataReader ExecuteReader(CommandType commandType, string commandText, params DbParameter[] commandParameters);
        #endregion

        #region Extend
        public int GetMaxID(string idColumnName, string tableName, string where)
        {
            string sql = string.Format("select max({0}) from {1} {2}", idColumnName, tableName, string.IsNullOrWhiteSpace(where) ? string.Empty : "where " + where);
            return Convert.ToInt32(ExecuteScalar(sql));
        }
        /// <summary>
        /// 替换like查询中的通配符。
        /// </summary>
        /// <param name="queryString">查询字符串</param>
        /// <returns></returns>
        public abstract string ReplacePatternCharacter(string queryString);
        #endregion
    }
}
