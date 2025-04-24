using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Application.Helpers;

public static class FilterHelper
{
    public static  IEnumerable<T> ApplyFilter<T>(this IQueryable<T> query, 
        Filter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.PropertyName) && !string.IsNullOrWhiteSpace(filter.Value))
        {
            query = query.Where(GetFilterExpressions<T>(filter)); 
        }
        
        if (filter?.Sorts != null && (filter?.Sorts?.Any() ?? false))
        {
            var firstSort = filter.Sorts.First();
            var orderedQuery = firstSort.IsAscending
                ? query.OrderBy(GetSortExpression<T>(firstSort))
                : query.OrderByDescending(GetSortExpression<T>(firstSort));

            foreach (var sort in filter.Sorts.Skip(1))
            {
                orderedQuery = sort.IsAscending
                    ? orderedQuery.ThenBy(GetSortExpression<T>(sort))
                    : orderedQuery.ThenByDescending(GetSortExpression<T>(sort));
            }

            query = orderedQuery;
        }

        return query.Skip((filter.Page - 1) * filter.Size)
            .Take(filter.Size);
    }
   
    private static Expression<Func<T, object>> GetSortExpression<T>(Sort sort)
    {
        var prop = Expression.Parameter(typeof(T));
        var property = Expression.PropertyOrField(prop, sort.PropertyName);
        return Expression.Lambda<Func<T, object>>(property, prop);
    }
    private static Expression<Func<T, bool>> GetFilterExpressions<T>(Filter filter)
{
  // We Want Generate X=>X.FirstName == "Jon"

  // Here We create X=>
  var paramter = Expression.Parameter(typeof(T));
  // Here We create X.FirstName
  var propName = Expression.PropertyOrField(paramter, filter.PropertyName);
  // Here We create "Jon" Constant
  var targetType = propName.Type;
  if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
      targetType = Nullable.GetUnderlyingType(targetType);
  var constExpression = Expression.Constant(Convert.ChangeType(filter.Value, targetType), propName.Type);
  Expression filterExpression;

  switch (filter.Operation)
  {
      case Operator.Eq:
          filterExpression = Expression.Equal(propName, constExpression);
          break;
      case Operator.GtOrEq:
          filterExpression = Expression.GreaterThanOrEqual(propName, constExpression);
          break;
      case Operator.LtorEq:
          filterExpression = Expression.LessThanOrEqual(propName, constExpression);
          break;
      case Operator.Gt:
          filterExpression = Expression.GreaterThan(propName, constExpression);
          break;
      case Operator.Lt:
          filterExpression = Expression.LessThan(propName, constExpression);
          break;
      case Operator.NotEq:
          filterExpression = Expression.NotEqual(propName, constExpression);
          break;
      case Operator.Conatains:
          if (filter.Value.GetType() != typeof(string))
              throw new InvalidFilterCriteriaException();
          var containsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
          filterExpression = Expression.Call(propName, containsMethodInfo, constExpression);
          break;
      default:
          throw new InvalidOperationException();
  }

  return Expression.Lambda<Func<T, bool>>(filterExpression, paramter);
}
}

public class Filter
{
    public string? PropertyName { get; set; }
    public Operator? Operation { get; set; }  = Operator.Conatains;
    public string? Value { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 20;

    public List<Sort>? Sorts { get; set; }
}

public enum Operator
{
    Eq = 0,
    GtOrEq = 1,
    LtorEq = 2,
    Gt = 3,
    Lt = 4,
    NotEq = 5,
    Conatains = 6,
}

public class Sort
{
    public string PropertyName { get; set; }
    public bool IsAscending { get; set; }
}
/*public async Task<IEnumerable<T>> ApplyFilter<T>(this IQueryable<T> query,
       QueryCriteria queryCriteria)
   {
       foreach (var filter in queryCriteria.Filters)
       {
           query = query.Where(GetFilterExpressions<T>(filter));
       }

       foreach (var sort in queryCriteria.Sorts)
       {
           query = sort.IsAscending ?
               query.OrderBy(GetSortExpression<T>(sort)) :
               query.OrderByDescending(GetSortExpression<T>(sort));
       }

       return await query.Skip(queryCriteria.Skip)
           .Take(queryCriteria.Take)
           .ToListAsync();
   }*/

