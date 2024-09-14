using Ecommerce.Domain.Common.Commands;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    int Stock,
    string Sku,
    decimal ComparePrice,
    string Description,
    Guid CreatorId,
    decimal? Discount) : ICommand;