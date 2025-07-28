using EasyNetQ;

namespace Messaging.EasyNetQ;

public class WorkerSubscriber : BackgroundService
{
    private readonly ILogger<WorkerSubscriber> _logger;

    public WorkerSubscriber(ILogger<WorkerSubscriber> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var c = new ConnectionConfiguration();
        c.Hosts.Add(new HostConfiguration
        {
            Host = "localhost",
        });

        var bus = RabbitHutch.CreateBus(c, o => { });

        await bus.PubSub.SubscribeAsync<SubmitOrder>("my_subscription_id",
            msg => _logger.LogInformation("Received order message: {MessageId}", msg.OrderId),
            stoppingToken
        );
    }
}
