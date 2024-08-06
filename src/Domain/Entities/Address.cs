using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Primitives;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

public sealed class Address : Entity<AddressId>
{
    internal Address(
        AddressId id,
        string street,
        string city,
        string state,
        string country,
        string zipCode,
        UserId userId,
        string phoneNumber)
    : base(id)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
        UserId = userId;
        PhoneNumber = phoneNumber;
    }

    private Address()
    {
    }

    public string Street { get; private set; } = null!;

    public string City { get; private set; } = null!;

    public string State { get; private set; } = null!;

    public string? Country { get; private set; }

    public string? ZipCode { get; private set; }

    public bool IsPrimary { get; private set; }

    public UserId UserId { get; private set; } = null!;

    public string PhoneNumber { get; private set; } = null!;

    public Result Update(
        string street,
        string city,
        string state,
        string country,
        string zipCode,
        string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;

        return Result.Success();
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }
}

public record AddressId(Guid Value);