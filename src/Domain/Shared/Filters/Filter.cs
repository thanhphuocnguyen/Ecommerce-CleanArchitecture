namespace Ecommerce.Domain.Shared;

public class Filter
{
    public string? Field { get; set; }

    public string? Operator { get; set; }

    public string? Value { get; set; }

    public string? Logic { get; set; }

    public IEnumerable<Filter>? Filters { get; set; }
}

public static class FilterOperator
{
    public const string EQ = "eq";
    public const string NEQ = "neq";
    public const string LT = "lt";
    public const string LTE = "lte";
    public const string GT = "gt";
    public const string GTE = "gte";
    public const string STARTSWITH = "startswith";
    public const string ENDSWITH = "endswith";
    public const string CONTAINS = "contains";
}

public static class FilterLogic
{
    public const string AND = "and";
    public const string OR = "or";
    public const string NOT = "not";
    public const string XOR = "xor";
}