namespace Ecommerce.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public DateTimeOffset OccurredOn { get; set; }

    public required string Type { get; set; }

    public required string Content { get; set; }

    public DateTimeOffset? ProcessedDate { get; set; }

    public string? Error { get; set; }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembersAttribute]
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