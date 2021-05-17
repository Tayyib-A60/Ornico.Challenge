using Dapper;
using Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Executers
{
    public class Executers : IExecuters
    {
        private readonly IConfiguration _configuration;
        public Executers(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Execute the SQL command passed to the action.
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="task"></param>
        public void ExecuteCommand(string connStr, Action<SqlConnection, SqlTransaction> task)
        {
            using (var conn = new SqlConnection(connStr))
            {
                SqlTransaction _sqlTransaction = null;
                try
                {
                    conn.Open();
                    _sqlTransaction = conn.BeginTransaction();
                    task(conn, _sqlTransaction);
                    _sqlTransaction.Commit();
                }
                catch (SqlException ex)
                {
                    if (_sqlTransaction?.Connection != null)
                    {
                        _sqlTransaction.Rollback();
                    }
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (_sqlTransaction?.Connection != null)
                    {
                        _sqlTransaction.Rollback();
                    }
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Execute an SQL command using the parameters passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connStr"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        public void ExecuteCommand<T>(string connStr, string query, object param)
        {
            using (var conn = new SqlConnection(connStr))
            {
                SqlTransaction _sqlTransaction = null;
                try
                {
                    conn.Open();
                    _sqlTransaction = conn.BeginTransaction();
                    conn.Execute(query, param, transaction: _sqlTransaction);
                    _sqlTransaction.Commit();
                }
                catch (SqlException ex)
                {
                    if (_sqlTransaction?.Connection != null)
                    {
                        _sqlTransaction.Rollback();
                    }
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (_sqlTransaction?.Connection != null)
                    {
                        _sqlTransaction.Rollback();
                    }
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Asynchronously execute an SQL command using the parameters passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connStr"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        public async Task ExecuteCommandAsync<T>(string connStr, string query, object param)
        {
            using (var conn = new SqlConnection(connStr))
            {
                SqlTransaction _sqlTransaction = null;
                try
                {
                    conn.Open();
                    _sqlTransaction = conn.BeginTransaction();
                    await conn.ExecuteAsync(query, param, transaction: _sqlTransaction);
                    _sqlTransaction.Commit();
                }
                catch (SqlException ex)
                {
                    if (_sqlTransaction?.Connection != null)
                    {
                        _sqlTransaction.Rollback();
                    }
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (_sqlTransaction?.Connection != null)
                    {
                        _sqlTransaction.Rollback();
                    }
                    throw ex;
                }
            }
        }



        
        /// <summary>
        /// Execute an SQL query using the parameters passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connStr"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        public IEnumerable<T> ExecuteReader<T>(string connStr, string query, object param)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    IEnumerable<T> responseObject = conn.Query<T>(query, param, commandTimeout: _configuration.GetValue<int>("AppSettings:DatabaseReadTimeout"));
                    return responseObject;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Asynchronously execute an SQL query using the parameters passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connStr"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(string connStr, string query, object param)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    IEnumerable<T> responseObject = await conn.QueryAsync<T>(query, param, commandTimeout: _configuration.GetValue<int>("AppSettings:DatabaseReadTimeout"));
                    return responseObject;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
