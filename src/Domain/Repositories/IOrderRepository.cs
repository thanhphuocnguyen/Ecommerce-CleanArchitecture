using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId id);

    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default);

    void Insert(Order order);

    void Update(Order order);

    void Remove(Order order);
}