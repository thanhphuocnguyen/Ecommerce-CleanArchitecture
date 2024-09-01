namespace Ecommerce.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public DateTimeOffset OccurredOn { get; set; }

    public string Type { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTimeOffset? ProcessedDate { get; set; }

    public string? Error { get; set; }

    public OutboxMessage(Guid id, DateTime occurredOn, string type, string content)
    {
        Id = id;
        OccurredOn = occurredOn;
        Type = type;
        Content = content;
    }

    private OutboxMessage()
    {
    }
}