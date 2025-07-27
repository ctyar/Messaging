namespace Messaging;

public record SubmitOrder
{
    public static readonly string QueueName = "order-queue";

    public Guid OrderId { get; init; }
}
