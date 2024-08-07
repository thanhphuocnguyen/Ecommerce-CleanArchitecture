﻿using Application.Products.Contracts;
using Ecommerce.Application.Contracts;

namespace Ecommerce.Application.Products.Queries.GetProductById;

public class GetProductByIdQuery : IQuery<ProductDto>
{
    public Guid Id { get; set; }

    public GetProductByIdQuery(Guid id)
    {
        Id = id;
    }
}