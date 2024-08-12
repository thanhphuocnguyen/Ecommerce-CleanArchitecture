using System.Linq.Expressions;
using System.Reflection;
using Ardalis.Specification;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Infrastructure.Data.Specifications;

public static class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T> SearchBy<T>(this ISpecificationBuilder<T> builder, BaseFilter filter)
    {
        builder.SearchByKeyword(filter.Keyword)
            .AdvancedSearch(filter.AdvanceSearch)
            .AdvancedFilter(filter.AdvanceFilter);

        return builder;
    }

    public static IOrderedSpecificationBuilder<T> SearchByKeyword<T>(this ISpecificationBuilder<T> builder, string? keyword) =>
        builder.AdvancedSearch(new Search { Keyword = keyword });

    public static IOrderedSpecificationBuilder<T> AdvancedSearch<T>(this ISpecificationBuilder<T> builder, Search? search)
    {
        if (!string.IsNullOrEmpty(search?.Keyword))
        {
            if (search.Fields?.Any() is true)
            {
                foreach (string field in search.Fields)
                {
                    var paramExpr = Expression.Parameter(typeof(T));
                    MemberExpression propertyExpr = Expression.Property(paramExpr, field);
                    builder.AddSearchPropertyByKeyword(propertyExpr, paramExpr, search.Keyword);
                }
            }
            else
            {
                foreach (var property in typeof(T).GetProperties().Where(prop => (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) is { } propertyType
                    && !propertyType.IsEnum
                    && Type.GetTypeCode(propertyType) != TypeCode.Object))
                {
                    var paramExpr = Expression.Parameter(typeof(T));
                    var propertyExpr = Expression.Property(paramExpr, property);
                    builder.AddSearchPropertyByKeyword(propertyExpr, paramExpr, search.Keyword);
                }
            }
        }

        return new OrderedSpecificationBuilder<T>(builder.Specification);
    }

    public static ISpecificationBuilder<T> AdvancedFilter<T>(this ISpecificationBuilder<T> builder, Filter? filter)
    {
        if (filter is not null)
        {
            builder.AdvancedFilter(filter);
        }

        return builder;
    }

    private static void AddSearchPropertyByKeyword<T>(
        this ISpecificationBuilder<T> builder,
        Expression propertyExpr,
        ParameterExpression paramExpr,
        string keyword,
        string operatorSearch = FilterOperator.CONTAINS)
    {
        if (propertyExpr is not MemberExpression memberExpr || memberExpr.Member is not PropertyInfo property)
        {
            throw new ArgumentException("Property expression must be a property access expression");
        }

        string searchTerm = operatorSearch switch
        {
            FilterOperator.STARTSWITH => $"{keyword}%",
            FilterOperator.ENDSWITH => $"%{keyword}",
            FilterOperator.CONTAINS => $"%{keyword}%",
            _ => throw new ArgumentException("Invalid operator")
        };

        Expression selectorExpr =
            property.PropertyType == typeof(string)
                ? propertyExpr
                : Expression.Condition(
                    Expression.Equal(propertyExpr, Expression.Constant(null)),
                    Expression.Constant(null),
                    Expression.Call(propertyExpr, typeof(object).GetMethod("ToString")!));

        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;

        Expression callToLowerMethod = Expression.Call(selectorExpr, toLowerMethod);

        var selector = Expression.Lambda<Func<T, string>>(callToLowerMethod, paramExpr);

        ((List<SearchExpressionInfo<T>>)builder.Specification.SearchCriterias).Add(new SearchExpressionInfo<T>(selector, searchTerm));
    }
}