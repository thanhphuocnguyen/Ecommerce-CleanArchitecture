using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;

namespace Ecommerce.Infrastructure.Persistence.Initialization;

public class ProductSeeder : ICustomSeeder
{
    private readonly IProductRepository _productRepository;

    public ProductSeeder(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // TODO: Add products to the database
    public async Task InitializeAsync()
    {
        if (await _productRepository.CountAsync() == 0)
        {
            var products = new List<Product>
            {
            };

            // await _productRepository.AddRangeAsync(products);
        }
    }
}