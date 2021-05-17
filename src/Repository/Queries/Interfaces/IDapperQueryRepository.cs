using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Queries.Interfaces
{
    public interface IDapperQueryRepository<TEntity> : IBaseQueryRepository<TEntity> where TEntity : class
    {

    }
}
