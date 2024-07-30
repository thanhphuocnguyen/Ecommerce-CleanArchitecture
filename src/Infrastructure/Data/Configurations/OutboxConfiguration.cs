using Ecommerce.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

public class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.OccurredOn)
            .IsRequired();

        builder.Property(e => e.Type)
            .IsRequired();

        builder.Property(e => e.Content)
            .IsRequired();

        builder.Property(e => e.ProcessedDate)
            .IsRequired(false);

        builder.Property(e => e.Error)
            .IsRequired(false);

        builder.ToTable("OutboxMessages");
    }
}