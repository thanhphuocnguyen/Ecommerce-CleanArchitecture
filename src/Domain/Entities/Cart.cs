using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.Entities;

public class Cart : AggregateRoot<CartId>
{
    private Cart()
    {
    }

    private Cart(CartId id, Guid userId)
    : base(id)
    {
        Guid = userId;
    }

    public Guid Guid { get; private set; } = default!;

    public ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();

    public void AddProduct(ProductId productId, int quantity)
    {
        var existingItem = CartItems.SingleOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            CartItems.Add(new CartItem(new CartItemId(Guid.NewGuid()), Id, productId, quantity));
        }
    }

    public static Cart Create(Guid userId)
    {
        return new Cart(new CartId(Guid.NewGuid()), userId);
    }
}

public record CartId(Guid Value);