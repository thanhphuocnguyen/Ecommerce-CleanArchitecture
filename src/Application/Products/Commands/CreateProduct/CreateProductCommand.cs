using Ecommerce.Application.Common.Commands;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    string Sku,
    decimal ComparePrice,
    string Description,
    Guid CreatorId,
    decimal? Discount) : ICommand;