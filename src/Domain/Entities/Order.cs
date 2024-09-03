using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Primitives;
using Ecommerce.Domain.Shared.Result;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

public class Order : AggregateRoot<OrderId>, IAuditableEntity
{
    private readonly HashSet<LineItem> _lineItems = new();

    private Order()
    {
        // Required by EF Core
    }

    private Order(
        OrderId id,
        Guid creatorId,
        decimal price,
        int quantity)
    : base(id)
    {
        CreatorId = creatorId;
        Price = price;
        Quantity = quantity;
    }

    public OrderStatus Status { get; private set; }

    public Guid CreatorId { get; private set; }

    public Money Total => _lineItems.Aggregate(Money.Zero("USD"), (total, item) => total.Add(item.Price));

    public IReadOnlyCollection<LineItem> LineItems => _lineItems;

    public decimal Price { get; private set; }

    public int Quantity { get; private set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public static Result<Order> Create(
        Guid creator,
        decimal price,
        int quantity)
    {
        return new Order(new(Guid.NewGuid()), creator, price, quantity);
    }

    public void AddProduct(Product product, Money price, int quantity)
    {
        _lineItems.Add(new LineItem(
            new(Guid.NewGuid()),
            product.Id,
            Id,
            price,
            quantity));
    }

    public void RemoveProduct(ProductId productId)
    {
        var lineItem = _lineItems.FirstOrDefault(x => x.ProductId == productId);

        if (lineItem is not null)
        {
            _lineItems.Remove(lineItem);
        }
    }

    public void ChangeStatus(OrderStatus status)
    {
        Status = status;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
    }

    public void Complete()
    {
        Status = OrderStatus.Delivered;
    }

    public void Ship()
    {
        Status = OrderStatus.Shipped;
    }

    public void Return()
    {
        Status = OrderStatus.Returned;
    }

    public void Refund()
    {
        Status = OrderStatus.Refunded;
    }
}

public record OrderId(Guid Value);