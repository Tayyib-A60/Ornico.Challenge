
using Core.Enums;
using Repository.Executers;
using Repository.Extensions;
using Repository.Helpers;
using Repository.Queries.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Queries.Implementations
{
    public class DapperQueryRepository<TEntity> : IDapperQueryRepository<TEntity> where TEntity : class
    {
        private readonly IConfiguration _configuration;
        private readonly IExecuters _executers;
        private readonly IUtilities _utilities;
        private readonly string _connStr;

        public DapperQueryRepository(
            IConfiguration configuration,
            IExecuters executers,
            IUtilities utilities)
        {
            _configuration = configuration;
            _executers = executers;
            _connStr = _configuration.GetConnectionString("DbConnectionString");
            _utilities = utilities;
        }

        public IQueryable<TEntity> GetBy(Dictionary<string, string> criteria)
        {
            string query = _utilities.GenerateSelectQuery<TEntity>(criteria);

            var entityObject = _executers.ExecuteReader<TEntity>(_connStr, query, null);
            return entityObject.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetByAsync(Dictionary<string, string> criteria)
        {
            string query = _utilities.GenerateSelectQuery<TEntity>(criteria);

            var entityObject = await _executers.ExecuteReaderAsync<TEntity>(_connStr, query, null);
            return entityObject.AsQueryable();
        }

    }
}
