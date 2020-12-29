using IMS.Domain.Repository.Standard;
using IMS.Infrastructure.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Repository.Standard
{
    public class GenericRepository<TEntity> : EntityFrameworkRepository<TEntity>, IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly DbSet<TEntity> DbSet;

        protected GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            DbSet = _unitOfWork.Context.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity obj)
        {
            var entryEntity = await DbSet.AddAsync(obj);

            await _unitOfWork.SaveChangesAsync();

            return entryEntity.Entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);

            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task RemoveAsync(object id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null) return;

            Remove(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        public virtual TEntity Remove(TEntity obj)
        {
            DbSet.Remove(obj);
            _unitOfWork.SaveChanges();

            return obj;
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
            _unitOfWork.SaveChanges();
        }

        public virtual async Task UpdateAsync(TEntity obj)
        {
            _unitOfWork.Context.Entry(obj).State = EntityState.Modified;
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual TEntity Update(TEntity obj)
        {
            _unitOfWork.Context.Entry(obj).State = EntityState.Modified;
            _unitOfWork.SaveChanges();

            return obj;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            _unitOfWork.SaveChanges();
        }

        public void Dispose()
        {
            _unitOfWork.Context.Dispose();
            GC.SuppressFinalize(this);
        }

        protected override IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            query = GenerateQueryableWhereExpression(query, filter);
            query = GenerateIncludeProperties(query, includeProperties);

            if (orderBy != null) return orderBy(query);

            return query;
        }

        private IQueryable<TEntity> GenerateQueryableWhereExpression(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter)
        {
            if (filter != null) return query.Where(filter);
            return query;
        }

        private IQueryable<TEntity> GenerateIncludeProperties(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            foreach (var includeProperty in includeProperties)
            {
                var body = includeProperty.Body as MemberExpression;
                query = query.Include(body.Member.Name);
            }

            return query;
        }
    }
}
