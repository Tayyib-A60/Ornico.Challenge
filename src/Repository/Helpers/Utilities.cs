
using Core.Enums;
using Core.Helpers;
using Repository.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Helpers
{
    public class Utilities : IUtilities
    {
        private readonly IConfiguration _configuration;
        private string _connStr = null;

        public Utilities(IConfiguration configuration)
        {
            _configuration = configuration;

            _connStr = _configuration.GetConnectionString("DbConnectionString");
        }

        public string GetConnectionString(DataSource dataSource)
        {
            switch (dataSource)
            {
                case DataSource.MSDB:
                    return _connStr;

                default:
                    return null;
            }
        }

        public object GetObjectParams<TEntity>(TEntity entity) where TEntity : class
        {
            return GenerateParams(typeof(TEntity).GetProperties(), entity).ConvertToAnonymousObject();
        }

        public Dictionary<string, object> GenerateParams<TEntity>(IEnumerable<System.Reflection.PropertyInfo> listOfProperties, TEntity entity) where TEntity : class
        {
            Dictionary<string, object> objectDictionary = new Dictionary<string, object>();
            foreach (var prop in listOfProperties)
            {
                if (!entity.GetType().IgnoreProperty<Type>(prop.Name))
                {
                    objectDictionary.Add(prop.Name, prop.GetValue(entity));
                }
            }
            return objectDictionary;
        }

        public string GenerateInsertQuery<TEntity>(TEntity entity) where TEntity : class
        {
            string tableName = typeof(TEntity).GetTableName<Type>();

            var insertQuery = new StringBuilder($"INSERT INTO dbo.[{tableName}] ");
            insertQuery.Append("(");
            var properties = GenerateParams(typeof(TEntity).GetProperties(), entity).Keys.ToList();
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });
            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");
            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });
            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");
            return insertQuery.ToString();
        }

        public string GenerateUpdateQuery<TEntity>(TEntity entity) where TEntity : class
        {
            string tableName = typeof(TEntity).GetTableName<Type>();
            var updateQuery = new StringBuilder($"UPDATE dbo.[{tableName}] ");
            updateQuery.Append("SET");
            var properties = GenerateParams(typeof(TEntity).GetProperties(), entity).Keys.ToList();
            foreach (var prop in properties)
            {
                if (prop == "ID") continue;
                updateQuery.Append($" [{prop}] = @{prop},");
            }
            updateQuery.Remove(updateQuery.Length - 1, 1);
            updateQuery.Append($" where ID = @ID");
            return updateQuery.ToString();
        }


        public string GenerateDeleteQuery<TEntity>(TEntity entity) where TEntity : class
        {
            string tableName = typeof(TEntity).GetTableName<Type>();
            var deleteQuery = new StringBuilder($"DELETE FROM dbo.[{tableName}] ");
            deleteQuery.Append($" where ID = @ID");
            return deleteQuery.ToString();
        }

        
        public string GenerateSelectQuery<TEntity>(Dictionary<string, string> criteria) where TEntity : class
        {
            string tableName = typeof(TEntity).GetTableName<Type>();

            var selectQuery = new StringBuilder($"SELECT * FROM dbo.[{tableName}] with (nolock) WHERE ");
            int count = 1;
            foreach (var item in criteria)
            {
                selectQuery.Append($"{item.Key} like '%{item.Value.Trim().Replace(' ','%')}%' ");
                if (criteria.Count > count )
                {
                    selectQuery.Append("AND ");
                }
                count++;
            }
            return $"{selectQuery.ToString()} order by creationdate desc";
        }
    }
}
