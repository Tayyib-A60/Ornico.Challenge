using Core.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Commands.Interfaces
{
    public interface IDapperCommandRepository<TEntity> : IBaseCommandRepository<TEntity> where TEntity : class
    {
        SqlTransaction BeginTransaction(DataSource dataSource);
        void CommitTransaction(SqlTransaction sqlTransaction);
        void RollBackTransaction(SqlTransaction sqlTransaction);
    }
}
