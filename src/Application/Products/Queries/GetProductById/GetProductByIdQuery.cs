using Application.Products.Contracts;
using Ecommerce.Domain.Common.Queries;

namespace Ecommerce.Domain.Products.Queries.GetProductById;

public class GetProductByIdQuery : IQuery<ProductDto>
{
    public Guid Id { get; set; }

    public GetProductByIdQuery(Guid id)
    {
        Id = id;
    }
}