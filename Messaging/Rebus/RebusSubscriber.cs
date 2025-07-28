using Rebus.Handlers;

namespace Messaging.Rebus;

public class RebusSubscriber : IHandleMessages<SubmitOrder>
{
    private readonly ILogger<RebusSubscriber> _logger;

    public RebusSubscriber(ILogger<RebusSubscriber> logger)
    {
        _logger = logger;
    }

    public Task Handle(SubmitOrder submitOrder)
    {
        _logger.LogInformation("Rebus subscriber received message with {OrderId}", submitOrder.OrderId);

        return Task.CompletedTask;
    }
}
