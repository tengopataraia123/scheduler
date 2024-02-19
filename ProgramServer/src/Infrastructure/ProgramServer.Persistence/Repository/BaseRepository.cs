using ProgramServer.Application.Repository;
using ProgramServer.Domain.Common;
using ProgramServer.Persistence.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;

namespace ProgramServer.Persistence.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entity)
        {
            _dbContext.Set<T>().AddRange(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task Delete(Expression<Func<T, bool>> predicate)
        {
            var entities = _dbContext.Set<T>().Where(predicate).ToList(); // Use synchronous ToList
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }


        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }
    }
}
