﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;
using JIAOFENG.Practices.Library.Common;

namespace JIAOFENG.Practices.Database
{
    public class MySQLDatabase : Database
    {

        #region constructor
        public MySQLDatabase(string connectionString)
            : base(connectionString)
        {
            this.DatabaseProviderType = Practices.Database.DatabaseProviderType.MySQL;
        }
        #endregion

        #region ExecuteDataSet
        public override DataSet ExecuteDataSet(CommandType commandType, string commandText, int commandTimeout, params DbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            MySqlCommand cmd = new MySqlCommand();
            DataSet ds = new DataSet();
            ds.Locale = new CultureInfo(string.Empty, false);

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();
                PrepareCommand(cmd, conn, commandType, commandText, commandTimeout, commandParameters);

                //create the DataAdapter & DataSet
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    try
                    {
                        //fill the DataSet using default values for DataTable names, etc.
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        throw new DatabaseExecutionException(ex.Message, ex, cmd);
                    }
                    finally
                    {
                        // detach the SqlParameters from the command object, so they can be usingObjects again.
                        cmd.Parameters.Clear();
                    }                   
                }
            }          

            //return the dataset
            return ds;
        }
        public override DataSet ExecuteDataSet(string spName, params object[] parameterValues)
        {
            //if we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                MySqlParameter[] commandParameters = MySQLParameterCache.GetSpParameterSet(this.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of SqlParameters
                return ExecuteDataSet(CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the Sp without params
            else
            {
                return ExecuteDataSet(CommandType.StoredProcedure, spName);
            }
        }
        /// <summary>
        /// 执行分页数据查询
        /// </summary>
        /// <param name="commandText">sql查询语句</param>
        /// <param name="orderBy">sql语句的排序列</param>
        /// <param name="pageIndex">显示第几页数据</param>
        /// <param name="pageSize">每页记录树</param>
        /// <param name="commandParameters">sql语句的参数</param>
        /// <returns>分页后的table</returns>
        public override PagedTable ExecutePagedTable(string commandText, string orderBy, int pageIndex, int pageSize, params DbParameter[] commandParameters)
        {
            const string PageFormat = @"SELECT * FROM ({0}) T LIMIT {1},{2};SELECT count(1) FROM ({0}) T ";

            string sql = string.Format(PageFormat, commandText, pageSize * (pageIndex - 1), pageSize);
            DataSet ds = ExecuteDataSet(CommandType.Text, sql, commandParameters);
            return new PagedTable(ds.Tables[0], pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
        }
        #endregion

        #region ExecuteNonQuery
        public override int ExecuteNonQuery(CommandType commandType, string commandText, int commandTimeout, params DbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            MySqlCommand cmd = new MySqlCommand();
            int retval;
            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();
                PrepareCommand(cmd, conn, commandType, commandText, commandTimeout, commandParameters);

                try
                {
                    //finally, execute the command.
                    retval = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new DatabaseExecutionException(ex.Message, ex, cmd);
                }
                finally
                {
                    // detach the SqlParameters from the command object, so they can be usingObjects again.
                    cmd.Parameters.Clear();
                }
            }

            return retval;
        }
        public override int ExecuteNonQuery(string spName, params object[] parameterValues)
        {
            //if we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                MySqlParameter[] commandParameters = MySQLParameterCache.GetSpParameterSet(this.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the Sp without params
            else
            {
                return ExecuteNonQuery(CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region ExecuteReader
        public override DbDataReader ExecuteReader(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            MySqlConnection conn = new MySqlConnection(this.ConnectionString);
            MySqlCommand cmd = new MySqlCommand();
            conn.Open();
            PrepareCommand(cmd, conn, commandType, commandText, DEFAULT_COMMAND_TIMEOUT, commandParameters);

            MySqlDataReader ret = null;
            try
            {
                ret = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new DatabaseExecutionException(ex.Message, ex, cmd);
            }
            finally
            {
                // detach the SqlParameters from the command object, so they can be usingObjects again.
                cmd.Parameters.Clear();
            }
            return ret;
        }
        public override DbDataReader ExecuteReader(string spName, params object[] parameterValues)
        {
            //if we receive parameter values, we need to figure out where they go
            if (parameterValues != null && parameterValues.Length > 0)
            {
                MySqlParameter[] commandParameters = MySQLParameterCache.GetSpParameterSet(this.ConnectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the Sp without params
            else
            {
                return ExecuteReader(CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region ExecuteScalar
        public override object ExecuteScalar(CommandType commandType, string commandText, int commandTimeout, params DbParameter[] commandParameters)
        {
            //create a command and prepare it for execution           
            MySqlCommand cmd = new MySqlCommand();
            //return the object
            object ret = null;

            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                conn.Open();
                PrepareCommand(cmd, conn, commandType, commandText, commandTimeout, commandParameters);

                try
                {
                    ret = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new DatabaseExecutionException(ex.Message, ex, cmd);
                }
                finally
                {
                    // detach the SqlParameters from the command object, so they can be usingObjects again.
                    cmd.Parameters.Clear();
                }
            }

            return ret;
        }
        public override object ExecuteScalar(string spName, params object[] parameterValues)
        {
            //if we receive parameter values, we need to figure out where they go
            if (parameterValues != null && parameterValues.Length > 0)
            {
                //pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                MySqlParameter[] commandParameters = MySQLParameterCache.GetSpParameterSet(this.ConnectionString, spName);

                //assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                //call the overload that takes an array of SqlParameters
                return ExecuteScalar(CommandType.StoredProcedure, spName, commandParameters);
            }
            //otherwise we can just call the Sp without params
            else
            {
                return ExecuteScalar(CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region Extend
        /// <summary>
        /// 替换like查询中的通配符。
        /// </summary>
        /// <param name="queryString">查询字符串</param>
        /// <returns></returns>
        public override string ReplacePatternCharacter(string queryString)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                queryString = queryString.Replace(@"\", @"\\\\");
                queryString = queryString.Replace("_", @"\_");
                queryString = queryString.Replace("%", @"\%");
            }

            return queryString;
        }
        #endregion
    }
}
