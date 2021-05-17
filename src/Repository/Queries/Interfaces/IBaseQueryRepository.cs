using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace  Repository.Queries.Interfaces
{
    public interface IBaseQueryRepository<TEntity> where TEntity : class
    {

        IQueryable<TEntity> GetBy(Dictionary<string, string> criteria);

        Task<IQueryable<TEntity>> GetByAsync(Dictionary<string, string> criteria);

    }
}
