using EasyNetQ;

namespace Messaging.EasyNetQ;

public class WorkerSubscriber : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger<WorkerSubscriber> _logger;

    public WorkerSubscriber(IBus bus, ILogger<WorkerSubscriber> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<SubmitOrder>("my_subscription_id",
            msg => _logger.LogInformation("Received order message: {MessageId}", msg.OrderId),
            stoppingToken
        );
    }
}
