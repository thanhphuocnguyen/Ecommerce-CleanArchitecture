using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(
            productId => productId.Value,
            value => new ProductId(value));

        builder
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(p => p.Price, price =>
        {
            price
                .Property(p => p.Amount)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            price.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });

        builder.Property(p => p.Status).HasConversion<string>();

        builder.Property(p => p.Description).HasMaxLength(500);

        builder.Property(p => p.Sku).HasConversion(
            sku => sku.Value,
            value => Sku.Create(value).Value);

        builder.OwnsOne(p => p.Discount, discount =>
        {
            discount.Property(p => p.Amount).HasColumnName("Discount").HasColumnType("decimal(18,2)").IsRequired();
            discount.Property(p => p.Currency).HasColumnName("DiscountCurrency").HasMaxLength(3).IsRequired();
        });

        builder.Property(p => p.IsAvailable).IsRequired();

        builder.OwnsOne(cp => cp.ComparePrice, cp =>
        {
            cp.Property(p => p.Amount).HasColumnName("Price").HasColumnType("decimal(18,2)").IsRequired();
            cp.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.Stock).IsRequired();

        builder.HasIndex(p => p.Sku).IsUnique();
    }
}