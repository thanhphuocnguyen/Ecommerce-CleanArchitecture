using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Ardalis.Specification;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Infrastructure.Persistence.Specifications;

public static class SpecificationBuilderExtensions
{
    /// <summary>
    /// Adds search functionality to the specification builder based on the provided filter.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="filter">The filter containing search parameters.</param>
    /// <returns>The specification builder with search functionality added.</returns>
    public static ISpecificationBuilder<T> SearchBy<T>(this ISpecificationBuilder<T> builder, BaseFilter filter)
    {
        builder.SearchByKeyword(filter.Keyword)
            .AdvancedSearch(filter.AdvanceSearch)
            .AdvancedFilter(filter.AdvanceFilter);

        return builder;
    }

    public static ISpecificationBuilder<T> PaginateBy<T>(this ISpecificationBuilder<T> specification, PaginationFilter filter)
    {
        if (filter.Page <= 0)
        {
            filter.Page = 1;
        }

        if (filter.PageSize <= 0)
        {
            filter.PageSize = 10;
        }

        if (filter.Page > 1)
        {
            specification = specification.Skip((filter.Page - 1) * filter.PageSize);
        }

        return specification
            .Take(filter.PageSize)
            .OrderBy(filter.OrderBy);
    }

    public static IOrderedSpecificationBuilder<T> SearchByKeyword<T>(
        this ISpecificationBuilder<T> builder,
        string? keyword) =>
        builder.AdvancedSearch(new Search { Keyword = keyword });

    public static IOrderedSpecificationBuilder<T> AdvancedSearch<T>(
        this ISpecificationBuilder<T> builder,
        Search? search)
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
                foreach (var property in typeof(T).GetProperties().Where(prop =>
                    (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) is { } propertyType
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

    public static IOrderedSpecificationBuilder<T> AdvancedFilter<T>(
        this ISpecificationBuilder<T> builder,
        Filter? filter)
    {
        if (filter is not null)
        {
            var parameter = Expression.Parameter(typeof(T));

            Expression binaryExpressionFilter;

            if (!string.IsNullOrEmpty(filter.Logic))
            {
                if (filter.Filters is null)
                {
                    throw new ArgumentNullException(nameof(filter.Filters));
                }

                binaryExpressionFilter = CreateFilterExpression(filter.Filters, parameter, filter.Logic);
            }
            else
            {
                var filterValid = GetValidFilter(filter);
                binaryExpressionFilter = CreateFilterExpression(
                    filterValid.Field!,
                    filterValid.Operator!,
                    filterValid.Value!,
                    parameter);
            }

            ((List<WhereExpressionInfo<T>>)builder.Specification.WhereExpressions)
                .Add(new WhereExpressionInfo<T>(
                    Expression.Lambda<Func<T, bool>>(binaryExpressionFilter, parameter)));
        }

        return new OrderedSpecificationBuilder<T>(builder.Specification);
    }

    private static Expression CreateFilterExpression(
        IEnumerable<Filter> filters,
        ParameterExpression parameter,
        string logic)
    {
        Expression filterExpression = default!;

        foreach (var filter in filters)
        {
            Expression bExpressionFilter;
            if (!string.IsNullOrEmpty(filter.Logic))
            {
                if (filter.Filters is null)
                {
                    throw new ArgumentNullException(nameof(filter.Filters));
                }

                bExpressionFilter = CreateFilterExpression(filter.Filters, parameter, filter.Logic);
            }
            else
            {
                var filterValid = GetValidFilter(filter);
                bExpressionFilter = CreateFilterExpression(filterValid.Field!, filterValid.Operator!, filterValid.Value!, parameter);
            }

            filterExpression = filterExpression is null ? bExpressionFilter : CombineFilter(logic, filterExpression, bExpressionFilter);
        }

        return filterExpression;
    }

    private static Expression CombineFilter(
        string logic,
        Expression filterExpression,
        Expression bExpressionFilter) =>
        logic switch
        {
            FilterLogic.AND => Expression.AndAlso(filterExpression, bExpressionFilter),
            FilterLogic.OR => Expression.OrElse(filterExpression, bExpressionFilter),
            FilterLogic.NOT => Expression.Not(filterExpression),
            FilterLogic.XOR => Expression.ExclusiveOr(filterExpression, bExpressionFilter),
            _ => throw new ArgumentException("Invalid logic")
        };

    private static Expression CreateFilterExpression(
        string field,
        string @operator,
        object? value,
        ParameterExpression parameter)
    {
        var propertyExpression = GetPropertyExpression(field, parameter);
        var valueExpression = GetValueExpression(field, value, propertyExpression.Type);
        return CreateFilterExpression(propertyExpression, valueExpression, @operator);
    }

    private static Expression CreateFilterExpression(
        Expression propertyExpression,
        Expression valueExpression,
        string @operator)
    {
        if (propertyExpression.Type == typeof(string))
        {
            valueExpression = Expression.Call(valueExpression, "ToLower", null);
            propertyExpression = Expression.Call(propertyExpression, "ToLower", null);
        }

        return @operator switch
        {
            FilterOperator.EQ => Expression.Equal(propertyExpression, valueExpression),
            FilterOperator.NEQ => Expression.NotEqual(propertyExpression, valueExpression),
            FilterOperator.GT => Expression.GreaterThan(propertyExpression, valueExpression),
            FilterOperator.GTE => Expression.GreaterThanOrEqual(propertyExpression, valueExpression),
            FilterOperator.LT => Expression.LessThan(propertyExpression, valueExpression),
            FilterOperator.LTE => Expression.LessThanOrEqual(propertyExpression, valueExpression),
            FilterOperator.CONTAINS => Expression.Call(propertyExpression, "Contains", null, valueExpression),
            FilterOperator.STARTSWITH => Expression.Call(propertyExpression, "StartsWith", null, valueExpression),
            FilterOperator.ENDSWITH => Expression.Call(propertyExpression, "EndsWith", null, valueExpression),
            _ => throw new ArgumentException("Invalid operator")
        };
    }

    private static ConstantExpression GetValueExpression(string field, object? value, Type type)
    {
        if (value is null)
        {
            return Expression.Constant(null, type);
        }

        if (type.IsEnum)
        {
            string? stringEnum = GetStringFromJsonElement(value);

            if (!Enum.TryParse(type, stringEnum, true, out object? valueParsed))
            {
                throw new ArgumentException($"Invalid value for {field}");
            }

            return Expression.Constant(valueParsed, type);
        }

        if (type == typeof(Guid))
        {
            string? stringGuid = GetStringFromJsonElement(value);

            if (!Guid.TryParse(stringGuid, out Guid valueParsed))
            {
                throw new ArgumentException($"Invalid value for {field}");
            }

            return Expression.Constant(valueParsed, type);
        }

        if (type == typeof(string))
        {
            string? text = GetStringFromJsonElement(value);

            return Expression.Constant(text, type);
        }

        return Expression.Constant(ChangeType(((JsonElement)value).GetRawText(), type), type);
    }

    private static dynamic? ChangeType(string v, Type type)
    {
        var t = type;
        if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (v is null)
            {
                return null;
            }

            t = Nullable.GetUnderlyingType(t);
        }

        return Convert.ChangeType(v, t!);
    }

    private static string? GetStringFromJsonElement(object value) => ((JsonElement)value).GetString();

    private static MemberExpression GetPropertyExpression(string field, ParameterExpression parameter)
    {
        Expression propertyExpression = parameter;
        foreach (var member in field.Split('.'))
        {
            propertyExpression = Expression.PropertyOrField(propertyExpression, member);
        }

        return (MemberExpression)propertyExpression;
    }

    private static Filter GetValidFilter(Filter filter)
    {
        if (string.IsNullOrEmpty(filter.Field))
        {
            throw new ArgumentNullException(nameof(filter.Field));
        }

        if (string.IsNullOrEmpty(filter.Operator))
        {
            throw new ArgumentNullException(nameof(filter.Operator));
        }

        return filter;
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

        ((List<SearchExpressionInfo<T>>)builder.Specification.SearchCriterias)
            .Add(new SearchExpressionInfo<T>(selector, searchTerm));
    }

    private static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string[]? orderByFields)
    {
        if (orderByFields is not null)
        {
            foreach (var field in ParseOrderBy(orderByFields))
            {
                var paramExpr = Expression.Parameter(typeof(T));
                Expression propertyExpr = paramExpr;
                foreach (string member in field.Key.Split('.'))
                {
                    propertyExpr = Expression.PropertyOrField(propertyExpr, member);
                }

                var keySelector = Expression.Lambda<Func<T, object?>>(Expression.Convert(propertyExpr, typeof(object)), paramExpr);

                ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(keySelector, field.Value));
            }
        }

        return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
    }

    private static Dictionary<string, OrderTypeEnum> ParseOrderBy(string[] orderByFields) =>
     new(orderByFields.Select((orderByField, index) =>
          {
              string[] fieldParts = orderByField.Split(' ');
              string field = fieldParts[0];
              bool descending = fieldParts.Length > 1 && fieldParts[1].StartsWith("Desc", StringComparison.OrdinalIgnoreCase);
              var orderBy = index == 0
              ? descending ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
              : descending ? OrderTypeEnum.ThenByDescending : OrderTypeEnum.ThenBy;

              return new KeyValuePair<string, OrderTypeEnum>(field, orderBy);
          }));
}