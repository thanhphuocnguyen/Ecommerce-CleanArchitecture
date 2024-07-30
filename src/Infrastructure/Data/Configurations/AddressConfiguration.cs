using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(
            id => id.Value,
            value => new AddressId(value));

        builder.Property(x => x.Street).IsRequired();

        builder.Property(x => x.City).IsRequired();

        builder.Property(x => x.State).IsRequired();

        builder.Property(x => x.Country);

        builder.Property(x => x.ZipCode);

        builder.Property(x => x.IsPrimary).IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(15)
            .IsRequired();
    }
}