using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.ValueObjects;

public sealed class Sku : ValueObject
{
    public const int MaxLength = 16;
    public const char Separator = '_';
    public const int MaxSeparatorCount = 3;

    private Sku(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Sku> Create(string sku)
    {
        sku = sku.Trim();
        if (string.IsNullOrWhiteSpace(sku))
        {
            return Result<Sku>.Failure(DomainErrors.Sku.Empty);
        }

        if (sku.Length > MaxLength)
        {
            return Result<Sku>.Failure(DomainErrors.Sku.MaxLength);
        }

        var separatorCount = sku.Count(c => c == Separator);

        if (separatorCount != MaxSeparatorCount)
        {
            return Result<Sku>.Failure(DomainErrors.Sku.SeparatorCount);
        }

        return Result<Sku>.Success(new Sku(sku));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}