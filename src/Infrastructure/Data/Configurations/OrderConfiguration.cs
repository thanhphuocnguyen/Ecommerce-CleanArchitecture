using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(v => v.Value, v => new OrderId(v));

        builder
            .HasOne(x => x.Creator)
            .WithMany();

        builder
            .Property(x => x.Price)
            .IsRequired();
        builder
            .Property(x => x.Quantity)
            .IsRequired();

        builder
            .Property(x => x.Created)
            .IsRequired();

        builder
            .Property(x => x.Status)
            .IsRequired();

        builder
            .Property(x => x.LastModified);

        builder
            .HasMany(x => x.LineItems)
            .WithOne()
            .HasForeignKey(x => x.OrderId);

        builder.HasIndex(x => x.Created);

        builder.HasIndex(x => x.Status);
    }
}