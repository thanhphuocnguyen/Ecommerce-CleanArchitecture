using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.ValueObjects;

public class Money : ValueObject
{
    public const int MaxCurrencyLength = 3;

    public static Money Zero(string currency) => new Money(0, currency);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Money> Create(decimal amount, string currency)
    {
        if (amount < 0)
        {
            return Result<Money>.Failure(DomainErrors.Money.Invalid);
        }

        if (string.IsNullOrWhiteSpace(currency) || currency.Length > MaxCurrencyLength)
        {
            return Result<Money>.Failure(DomainErrors.Money.Invalid);
        }

        return Result<Money>.Success(new Money(amount, currency));
    }

    public decimal Amount { get; private set; }

    public string Currency { get; private set; }

    public Money Add(Money money)
    {
        if (Currency != money.Currency)
        {
            throw new InvalidOperationException("Cannot add money with different currencies.");
        }

        return new Money(Amount + money.Amount, Currency);
    }
}