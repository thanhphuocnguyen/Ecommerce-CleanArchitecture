using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasConversion(
            cartId => cartId.Value,
            value => new CartId(value));

        builder.HasMany(c => c.CartItems)
            .WithOne()
            .HasForeignKey("CartId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}