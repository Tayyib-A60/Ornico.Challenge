using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Executers
{
    public interface IExecuters : IAutoDependencyRepository
    {
        void ExecuteCommand(string connStr, Action<SqlConnection, SqlTransaction> task);
        void ExecuteCommand<T>(string connStr, string query, object param);
        Task ExecuteCommandAsync<T>(string connStr, string query, object param);
        IEnumerable<T> ExecuteReader<T>(string connStr, string query, object param);
        Task<IEnumerable<T>> ExecuteReaderAsync<T>(string connStr, string query, object param);
    }
}
