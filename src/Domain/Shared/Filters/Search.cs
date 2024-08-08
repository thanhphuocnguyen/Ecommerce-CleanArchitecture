namespace Ecommerce.Domain.Shared;

public class Search
{
    public List<string> Fields { get; set; } = new List<string>();

    public string? Term { get; set; }
}