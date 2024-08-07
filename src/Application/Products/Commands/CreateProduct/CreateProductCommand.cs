﻿using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    string Sku,
    decimal ComparePrice,
    string Description,
    UserId CreatorId,
    decimal? Discount) : ICommand;