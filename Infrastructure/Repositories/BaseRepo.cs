using System.Linq.Expressions;
using Domain.Entities;
using Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    internal DbSet<T> _dbSet;

    public BaseRepo(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Update(T source, T destination)
    {
        _dbSet.Entry(source).CurrentValues.SetValues(destination);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = _dbSet.AsQueryable().Where(predicate);
        return await query.AnyAsync();
    }

    public async Task<T> GetByIdAsync(long Id, string includeProperties = null)
    {
        IQueryable<T> query = _dbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            query = GetQueryWithIncludeProperties(query, includeProperties);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == Id);
    }

    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string includeProperties = null)
    {
        IQueryable<T> query = _dbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            query = GetQueryWithIncludeProperties(query, includeProperties);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    private static IQueryable<T> GetQueryWithIncludeProperties(IQueryable<T> query, string includeProperties)
    {
        var props = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var prop in props)
        {
            query = query.Include(prop.Trim());
        }

        return query;
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
        string includeProperties = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
    {
        var query = _dbSet.AsQueryable();
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            query = GetQueryWithIncludeProperties(query, includeProperties);
        }

        return await query.ToListAsync();
    }

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
    {
        IQueryable<T> query = _dbSet.AsQueryable().Where(predicate);
        return query.CountAsync();
    }
}