using Ecommerce.Domain.DomainEvents;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Primitives;

namespace Ecommerce.Domain.Entities;

public sealed class User : AggregateRoot<UserId>, IAuditableEntity
{
    private User(
        UserId id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string username,
        string passwordHash)
    : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Username = username;
        PasswordHash = passwordHash;
        _roles.Add(Role.User);
    }

    private List<Address> _addresses = new();
    private List<PaymentMethod> _paymentMethods = new();
    private List<Role> _roles = new();

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public string Username { get; private set; }

    public string PhoneNumber { get; private set; }

    public string PasswordHash { get; private set; }

    public bool EmailConfirmed { get; private set; } = false;

    public bool PhoneNumberConfirmed { get; private set; } = false;

    public string? ProfilePicture { get; private set; }

    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();

    public IReadOnlyList<Address> Addresses => _addresses;

    public IReadOnlyList<PaymentMethod> PaymentMethods => _paymentMethods;

    public Cart Cart { get; private set; } = default!;

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public static Result<User> Create(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string username,
        string password)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<User>(DomainErrors.User.FirstName.Empty);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<User>(DomainErrors.User.LastName.Empty);
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<User>(DomainErrors.User.Email.Empty);
        }

        var userId = new UserId(Guid.NewGuid());
        return Result<User>.Success(
            new User(
                userId,
                firstName,
                lastName,
                email,
                phoneNumber,
                username,
                password));
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;

        AddDomainEvent(new PhoneNumberUpdatedDomainEvent(phoneNumber));
    }

    public Result<Address> AddAddress(
        string street,
        string city,
        string state,
        string country,
        string zipCode,
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            return Result.Failure<Address>(DomainErrors.Address.Street.Empty);
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            return Result.Failure<Address>(DomainErrors.Address.City.Empty);
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            return Result.Failure<Address>(DomainErrors.Address.State.Empty);
        }

        var addressResult = new Address(
            new AddressId(Guid.NewGuid()),
            street,
            city,
            state,
            country,
            zipCode,
            this,
            phoneNumber);

        _addresses.Add(addressResult);

        return addressResult;
    }

    public void UpdateProfilePicture(string profilePicture)
    {
        ProfilePicture = profilePicture;
    }

    public void UpdateUsername(string username)
    {
        Username = username;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
    }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }

    public Result<PaymentMethod> AddPaymentMethod(
        PaymentTypeId paymentTypeId,
        string provider,
        string cardNumber,
        string cardHolderName,
        string cardExpiration,
        string cardSecurityNumber,
        bool isDefault)
    {
        var paymentMethod = PaymentMethod.Create(
            paymentTypeId,
            Id,
            provider,
            cardNumber,
            cardHolderName,
            cardExpiration,
            cardSecurityNumber,
            isDefault);

        _paymentMethods.Add(paymentMethod);

        return paymentMethod;
    }

    public void InitCart()
    {
        Cart = Cart.Create(Id);
    }

    public Result UpdatePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure(DomainErrors.Password.Empty);
        }

        PasswordHash = passwordHash;

        return Result.Success();
    }

    public void RemoveRole(Role role)
    {
        _roles.Remove(role);
    }

    public void AddRole(Role role)
    {
        _roles.Add(role);
    }
}

public record UserId(Guid Value);