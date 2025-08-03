using DotNetCore.CAP;
using DotNetCore.CAP.Messages;

namespace Messaging.Brighter;

public class BrighterSubscriber : ICapSubscribe
{
    private readonly ILogger<BrighterSubscriber> _logger;

    public BrighterSubscriber(ILogger<BrighterSubscriber> logger)
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
