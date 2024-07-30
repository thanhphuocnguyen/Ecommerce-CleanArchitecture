using Ecommerce.Domain.Shared.Primitives;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

public sealed class LineItem : Entity<LineItemId>
{
    internal LineItem(
        LineItemId id,
        ProductId productId,
        OrderId orderId,
        Money price,
        int quantity)
        : base(id)
    {
        ProductId = productId;
        Price = price;
        Quantity = quantity;
        OrderId = orderId;
    }

    public ProductId ProductId { get; private set; }

    public Money Price { get; private set; }

    public int Quantity { get; private set; }

    public OrderId OrderId { get; private set; }
}

public record LineItemId(Guid Value);