namespace Ecommerce.Domain.Shared;

public class BaseFilter
{
    public Search? AdvanceSearch { get; set; }

    public string? Keyword { get; set; }

    public Filter? AdvanceFilter { get; set; }
}