using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories;

internal class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public void Insert(Product product)
    {
        dbContext.Set<Product>().Add(product);
    }

    public void Remove(Product product)
    {
        dbContext.Set<Product>().Remove(product);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Product>().ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Product>().FindAsync(id, cancellationToken);
    }

    public void Update(Product product)
    {
        dbContext.Set<Product>().Update(product);
    }
}