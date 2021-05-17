using Core.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Repository.Helpers
{
    public interface IUtilities : IAutoDependencyRepository
    {
        string GetConnectionString(DataSource dataSource);
        object GetObjectParams<TEntity>(TEntity entity) where TEntity : class;

        Dictionary<string, object> GenerateParams<TEntity>(IEnumerable<PropertyInfo> listOfProperties, TEntity entity) where TEntity : class;

        string GenerateInsertQuery<TEntity>(TEntity entity) where TEntity : class;

        string GenerateUpdateQuery<TEntity>(TEntity entity) where TEntity : class;

        string GenerateSelectQuery<TEntity>(Dictionary<string, string> criteria) where TEntity : class;
        string GenerateDeleteQuery<TEntity>(TEntity entity) where TEntity : class;
    }
}
