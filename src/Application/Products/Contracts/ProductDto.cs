namespace Application.Products.Contracts;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string Category);