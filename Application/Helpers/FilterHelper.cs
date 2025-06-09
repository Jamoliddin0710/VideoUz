using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Application.Helpers;

public static class FilterHelper
{
    public static IEnumerable<T> ApplyFilter<T>(this IQueryable<T> query,
        Filter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.PropertyName) && !string.IsNullOrWhiteSpace(filter.Value))
        {
            query = query.Where(GetFilterExpressions<T>(filter));
        }


        if (!string.IsNullOrWhiteSpace(filter.Value) && string.IsNullOrWhiteSpace(filter.PropertyName))
        {
            var properties = (typeof(T).GetProperties()).Where(a => a.PropertyType == typeof(string));
            Expression<Func<T, bool>> combinedExpression = null;

            foreach (var property in properties)
            {
                var expression = GetFilterExpressionsAll<T>(filter, property.Name);

                if (combinedExpression == null)
                    combinedExpression = expression;
                else
                    combinedExpression = OrElse(combinedExpression, expression);
            }

            if (combinedExpression != null)
            {
                query = query.Where(combinedExpression);
            }
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

    private static Expression<Func<T, bool>> OrElse<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var left = Expression.Invoke(expr1, parameter);
        var right = Expression.Invoke(expr2, parameter);

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(left, right), parameter);
    }

    private static Expression<Func<T, object>> GetSortExpression<T>(Sort sort)
    {
        var prop = Expression.Parameter(typeof(T));
        var property = Expression.PropertyOrField(prop, sort.PropertyName);
        return Expression.Lambda<Func<T, object>>(property, prop);
    }

    private static Expression<Func<T, bool>> GetFilterExpressionsAll<T>(Filter filter, string propertyName)
    {
        var paramter = Expression.Parameter(typeof(T));

        var propName = Expression.PropertyOrField(paramter, propertyName);
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
                var containsMethodInfo =
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
                filterExpression = Expression.Call(propName, containsMethodInfo, constExpression);
                break;
            default:
                throw new InvalidOperationException();
        }

        return Expression.Lambda<Func<T, bool>>(filterExpression, paramter);
    }

    private static Expression<Func<T, bool>> GetFilterExpressions<T>(Filter filter)
    {
        var paramter = Expression.Parameter(typeof(T));
        var propName = Expression.PropertyOrField(paramter, filter.PropertyName);
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
                var containsMethodInfo =
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
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
    public Operator? Operation { get; set; } = Operator.Conatains;
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