using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.Entities;

public class PaymentType : Entity<PaymentTypeId>
{
    internal PaymentType(PaymentTypeId id, string name)
    : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; } = default!;
}

public record PaymentTypeId(Guid Value);