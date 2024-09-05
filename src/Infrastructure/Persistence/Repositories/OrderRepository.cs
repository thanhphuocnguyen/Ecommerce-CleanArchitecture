using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

internal class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Insert(Order order)
    {
        _dbContext.Set<Order>().Add(order);
    }

    public void Remove(Order order)
    {
        _dbContext.Set<Order>().Remove(order);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Order>().ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(OrderId id)
    {
        return await _dbContext.Set<Order>().FindAsync(id);
    }

    public void Update(Order order)
    {
        _dbContext.Set<Order>().Update(order);
    }
}