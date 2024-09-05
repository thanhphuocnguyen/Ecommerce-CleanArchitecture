using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.ToTable("LineItems");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                v => v.Value,
                v => new LineItemId(v));

        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.Quantity)
            .HasColumnName("Quantity")
            .HasColumnType("int");

        builder.Property(x => x.OrderId)
            .HasConversion(
                v => v.Value,
                v => new OrderId(v));

        builder.Property(x => x.Price)
            .HasConversion(
                v => v.Amount,
                v => Money.Create(v, "USD").Value);

        builder.HasIndex(x => x.OrderId);
    }
}