using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> OrderByProperty<TEntity>(
        this IQueryable<TEntity> source,
        string? propertyName,
        bool isDescending = true)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            // Fallback default sorting if no property is specified
            return isDescending 
                ? source.OrderByDescending(x => EF.Property<object>(x!, "created_at")) 
                : source.OrderBy(x => EF.Property<object>(x!, "created_at"));
        }

        var type = typeof(TEntity);
        var parameter = Expression.Parameter(type, "x");

        // Search for property matching case-insensitively
        var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
            .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

        if (property is null)
        {
            // If property doesn't exist, fall back to created_at or return unsorted
            return source;
        }

        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var methodName = isDescending ? "OrderByDescending" : "OrderBy";

        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [type, property.PropertyType],
            source.Expression,
            Expression.Quote(orderByExpression)
        );

        return source.Provider.CreateQuery<TEntity>(resultExpression);
    }
}