namespace Ecommerce.Domain.Shared;

public class PaginationFilter : BaseFilter
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public string[]? OrderBy { get; set; }

    public string? OrderDirection { get; set; }
}

public static class PaginationFilterExtensions
{
    public static bool HasOrderBy(this PaginationFilter filter) => filter.OrderBy?.Any() is true;
}