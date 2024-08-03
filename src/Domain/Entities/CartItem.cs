using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.Entities;

public class CartItem : Entity<CartItemId>
{
    internal CartItem(CartItemId id, CartId cartId, ProductId productId, int quantity)
    : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        CartId = cartId;
    }

    public ProductId ProductId { get; private set; } = default!;

    public Product Product { get; private set; } = default!;

    public CartId CartId { get; private set; } = default!;

    public int Quantity { get; private set; }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }
}

public record CartItemId(Guid Value);