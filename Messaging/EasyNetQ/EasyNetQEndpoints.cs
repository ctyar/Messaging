using DotNetCore.CAP;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.EasyNetQ;

public static class EasyNetQEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("easynetq", CreateAsync)
            .Produces(StatusCodes.Status201Created);
    }

    private static async Task<IResult> CreateAsync([FromServices] ICapPublisher capPublisher,
        [FromServices] DbContext dbContext, CancellationToken cancellationToken)
    {
        var order = new Order { Id = Guid.NewGuid() };
        dbContext.Orders.Add(order);

        await dbContext.SaveChangesAsync(cancellationToken);

        var bus = RabbitHutch.CreateBus("host=localhost");
        await bus.PubSub.PublishAsync(new SubmitOrder { OrderId = order.Id }, cancellationToken);

        return Results.Created();
    }
}
