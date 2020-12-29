using System;
using System.Linq;
using System.Linq.Expressions;

namespace IMS.Infrastructure.Repository.Standard
{
    public abstract class EntityFrameworkRepository<TEntity> where TEntity : class
    {
        protected abstract IQueryable<TEntity> GenerateQuery(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
