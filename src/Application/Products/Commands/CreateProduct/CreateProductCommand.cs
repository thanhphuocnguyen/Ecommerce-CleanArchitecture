using Ecommerce.Application.Contracts;

namespace Ecommerce.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    string Sku,
    decimal ComparePrice,
    string Description,
    decimal? Discount) : ICommand
{
}