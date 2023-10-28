using MainServer.Application.Repository;
using MainServer.Domain.Common;
using MainServer.Persistence.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System;

namespace ProgramServer.Persistence.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate, string relatedProperties = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (relatedProperties != null)
                query = query.Include(relatedProperties);

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> FindList(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (includes != null)
                query = includes(query);

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(string relatedProperties = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (relatedProperties != null) query = query.Include(relatedProperties);

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, string relatedProperties = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (relatedProperties != null)
                query = query.Include(relatedProperties);

            return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
