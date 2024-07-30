using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.Entities;

public class Cart : AggregateRoot<CartId>
{
    private Cart(CartId id, UserId userId)
    : base(id)
    {
        UserId = userId;
    }

    public UserId UserId { get; private set; }

    public static Cart Create(UserId userId)
    {
        return new Cart(new CartId(Guid.NewGuid()), userId);
    }
}

public record CartId(Guid Value);