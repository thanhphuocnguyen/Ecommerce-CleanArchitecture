using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Shared.Primitives;
using Ecommerce.Domain.Shared.Result;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities;

public sealed class Product : Entity<ProductId>
{
    private Product()
    {
    }

    private Product(
        ProductId id,
        string name,
        Money price,
        int stock,
        Sku sku,
        Money discount,
        string description,
        Money comparePrice,
        Guid creatorId)
    : base(id)
    {
        Name = name;
        Price = price;
        Sku = sku;
        Discount = discount;
        Description = description;
        ComparePrice = comparePrice;
        CreatorId = creatorId;
        Stock = stock;
    }

    public string Name { get; private set; } = null!;

    public Money Price { get; private set; } = null!;

    public Sku Sku { get; private set; } = null!;

    public Money Discount { get; private set; } = null!;

    public Money ComparePrice { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public ProductStatus Status { get; private set; }

    public bool IsAvailable { get; private set; }

    public bool IsOnSale => Discount.Amount > 0;

    public int Stock { get; private set; }

    public bool IsOutOfStock => Stock == 0;

    public Guid CreatorId { get; private set; }

    public void SetStock(int stock)
    {
        Stock = stock;
    }

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }

    public static Result<Product> Create(
        string name,
        decimal price,
        int stock,
        string sku,
        string currency,
        string description,
        decimal comparePrice,
        Guid creatorId,
        decimal discount = 0)
    {
        var skuResult = Sku.Create(sku);
        if (skuResult.IsFailure)
        {
            return Result<Product>.Failure(skuResult.Error!);
        }

        var priceResult = Money.Create(price, currency);

        if (priceResult.IsFailure)
        {
            return Result<Product>.Failure(priceResult.Error!);
        }

        var discountResult = Money.Create(discount, currency);

        if (discountResult.IsFailure)
        {
            return Result<Product>.Failure(discountResult.Error!);
        }

        var comparePriceResult = Money.Create(comparePrice, currency);

        if (comparePriceResult.IsFailure)
        {
            return Result<Product>.Failure(comparePriceResult.Error!);
        }

        var product = Result<Product>.Success(
            new Product(
                new ProductId(Guid.NewGuid()),
                name,
                priceResult.Value,
                stock,
                skuResult.Value,
                discountResult.Value,
                description,
                comparePriceResult.Value,
                creatorId));

        return product;
    }
}

public record ProductId(Guid Value);