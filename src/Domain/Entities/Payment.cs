using Ecommerce.Domain.Shared.Primitives;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

public sealed class Payment : AggregateRoot<PaymentId>, IAuditableEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Payment()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        // Required by EF Core
    }

    private Payment(PaymentId id, Guid orderId, Money amount)
    : base(id)
    {
        OrderId = orderId;
        Amount = amount;
    }

    public Guid OrderId { get; private set; }

    public Money Amount { get; private set; }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public static Payment Create(Guid orderId, Money amount)
    {
        return new Payment(
            new PaymentId(Guid.NewGuid()),
            orderId,
            amount);
    }
}

public record PaymentId(Guid Value);