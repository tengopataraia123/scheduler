using System;
using ProgramServer.Domain.Common;
using System.Linq.Expressions;

namespace ProgramServer.Application.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddRange(IEnumerable<T> entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
        Task Delete(Expression<Func<T, bool>> predicate);
        void Save();
    }
}

