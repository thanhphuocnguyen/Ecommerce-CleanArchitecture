using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);

    void Insert(Product product);

    void Update(Product product);

    void Remove(Product product);
}