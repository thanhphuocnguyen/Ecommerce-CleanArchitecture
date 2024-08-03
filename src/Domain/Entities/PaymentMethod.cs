using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.Entities;

public sealed class PaymentMethod : AggregateRoot<PaymentMethodId>, IAuditableEntity
{
    private PaymentMethod()
    {
    }

    private PaymentMethod(
        PaymentMethodId id,
        PaymentTypeId paymentTypeId,
        UserId userId,
        string provider,
        string cardNumber,
        string cardHolderName,
        string cardExpiration,
        string cardSecurityNumber,
        bool isDefault)
    : base(id)
    {
        PaymentTypeId = paymentTypeId;
        UserId = userId;
        Provider = provider;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSecurityNumber;
        IsDefault = isDefault;
    }

    public UserId UserId { get; private set; } = default!;

    public PaymentTypeId PaymentTypeId { get; private set; } = default!;

    public PaymentType PaymentType { get; private set; } = default!;

    // Provider
    public string Provider { get; private set; } = default!;

    public string CardNumber { get; private set; } = string.Empty;

    public string CardHolderName { get; private set; } = string.Empty;

    public string CardExpiration { get; private set; } = string.Empty;

    public string CardSecurityNumber { get; private set; } = string.Empty;

    public bool IsDefault { get; private set; }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public static PaymentMethod Create(
        PaymentTypeId paymentTypeId,
        UserId userId,
        string provider,
        string cardNumber,
        string cardHolderName,
        string cardExpiration,
        string cardSecurityNumber,
        bool isDefault)
    {
        return new PaymentMethod(
            new PaymentMethodId(Guid.NewGuid()),
            paymentTypeId,
            userId,
            provider,
            cardNumber,
            cardHolderName,
            cardExpiration,
            cardSecurityNumber,
            isDefault);
    }
}

public record PaymentMethodId(Guid Value);