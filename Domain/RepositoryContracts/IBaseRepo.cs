using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.RepositoryContracts;

public interface IBaseRepo<T> where T : class
{
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T source, T destination);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(long Id, string includeProperties = null);
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string includeProperties = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, string includeProperties = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
}