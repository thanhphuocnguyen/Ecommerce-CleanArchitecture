using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(
            id => id.Value,
            value => new PaymentMethodId(value));

        builder.HasOne(x => x.PaymentType)
            .WithMany()
            .HasForeignKey(x => x.PaymentTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CardHolderName).IsRequired();
        builder.Property(x => x.CardNumber).IsRequired();
        builder.Property(x => x.CardExpiration).IsRequired();
        builder.Property(x => x.CardSecurityNumber).IsRequired();
        builder.Property(x => x.Provider).IsRequired();
    }
}