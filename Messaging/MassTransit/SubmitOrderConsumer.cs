using MassTransit;

namespace Messaging.MassTransit;

public class SubmitOrderConsumer : IConsumer<SubmitOrder>
{
    private readonly ILogger<SubmitOrderConsumer> _logger;

    public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SubmitOrder> context)
    {
        _logger.LogInformation("Received SubmitOrder message for OrderId: {OrderId}", context.Message.OrderId);

        return Task.CompletedTask;
    }
}