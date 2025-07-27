using DotNetCore.CAP;
using DotNetCore.CAP.Messages;

namespace Messaging.Cap;

public class OrderSubscriber : ICapSubscribe
{
    private readonly ILogger<OrderSubscriber> _logger;

    public OrderSubscriber(ILogger<OrderSubscriber> logger)
    {
        _logger = logger;
    }

    [CapSubscribe("orders")]
    public Task ProcessAsync(Message message, CancellationToken cancellationToken)
    {
        // TODO: Message.Value is always null
        if (message.Value is null)
        {
            return Task.CompletedTask;
        }

        var SubmitOrder = (SubmitOrder)message.Value!;

        _logger.LogInformation("Received order message: {MessageId}", SubmitOrder.OrderId);

        return Task.CompletedTask;
    }
}
