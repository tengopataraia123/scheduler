using System;
using MainServer.Domain.Common;
using System.Linq.Expressions;

namespace MainServer.Application.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(string relatedProperties = null);
        Task<List<T>> FindList(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> includes = null);
        Task<T> Find(Expression<Func<T, bool>> predicate, string relatedProperties = null);
        Task<T> GetByIdAsync(int id, string relatedProperties = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}

