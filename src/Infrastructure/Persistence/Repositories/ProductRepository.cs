using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

internal class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public void Insert(Product product)
    {
        dbContext.Products.Add(product);
    }

    public void Remove(Product product)
    {
        dbContext.Products.Remove(product);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.FindAsync(id, cancellationToken);
    }

    public void Update(Product product)
    {
        dbContext.Products.Update(product);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Products.CountAsync();
    }
}