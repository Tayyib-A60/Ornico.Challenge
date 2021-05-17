using Dapper;
using Core.Enums;
using Repository.Commands.Interfaces;
using Repository.Executers;
using Repository.Extensions;
using Repository.Helpers;
using Repository.Queries.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Repository.Commands.Implementations
{
    public class DapperCommandRepository<TEntity> : IDapperCommandRepository<TEntity> where TEntity : class
    {
        private readonly IConfiguration _configuration;
        private readonly IExecuters _executers;
        private readonly IUtilities _utilities;
        private readonly string _connStr;

        public DapperCommandRepository(IConfiguration configuration, IExecuters executers, IUtilities utilities)
        {
            _configuration = configuration;
            _executers = executers;
            _connStr = _configuration.GetConnectionString("DbConnectionString");
            _utilities = utilities;
        }

        public void Add(TEntity entity)
        {
            _executers.ExecuteCommand(_connStr, (conn, sqlTrx) => {
                conn.Query<TEntity>(_utilities.GenerateInsertQuery(entity), _utilities.GetObjectParams(entity), transaction: sqlTrx);
            });
        }

        public async Task AddAsync(TEntity entity)
        {
            await _executers.ExecuteCommandAsync<TEntity>(_connStr, _utilities.GenerateInsertQuery(entity), _utilities.GetObjectParams(entity));
        }

        public void Update(TEntity entity)
        {
            _executers.ExecuteCommand(_connStr, (conn, sqlTrx) => {
                conn.Query<TEntity>(_utilities.GenerateUpdateQuery(entity), _utilities.GetObjectParams(entity), transaction: sqlTrx);
            });
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _executers.ExecuteCommandAsync<TEntity>(_connStr, _utilities.GenerateUpdateQuery(entity), _utilities.GetObjectParams(entity));
        }

        
        public SqlTransaction BeginTransaction(DataSource dataSource)
        {
            try
            {
                SqlTransaction sqlTransaction = null;
                SqlConnection conn;
                switch (dataSource)
                {
                    case DataSource.MSDB:
                        conn = new SqlConnection(_connStr);
                        conn.Open();
                        sqlTransaction = conn.BeginTransaction();
                        break;
                    default:
                        break;
                }
                return sqlTransaction;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CommitTransaction(SqlTransaction sqlTransaction)
        {
            try
            {
                if (sqlTransaction?.Connection != null)
                {
                    using (SqlConnection conn = sqlTransaction.Connection)
                    {
                        sqlTransaction.Commit();
                        conn.Close();
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RollBackTransaction(SqlTransaction sqlTransaction)
        {
            try
            {
                if (sqlTransaction?.Connection != null)
                {
                    using (SqlConnection conn = sqlTransaction.Connection)
                    {
                        sqlTransaction.Rollback();
                        conn.Close();
                    }
                }
                sqlTransaction = null;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(Guid id)
        {
            string tableName = typeof(TEntity).GetTableName<Type>();

             _executers.ExecuteCommand<TEntity>(_connStr, CustomQueries.DeleteByID.Replace("#table", tableName), new { @ID = id });

        }

        public async Task DeleteAsync(Guid id)
        {
            string tableName = typeof(TEntity).GetTableName<Type>();

            await _executers.ExecuteCommandAsync<TEntity>(_connStr, CustomQueries.DeleteByID.Replace("#table", tableName), new { @ID = id });
        }
    }
}
