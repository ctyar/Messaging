using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Messaging;

public static class MassTransitEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("masstransit", CreateAsync)
            .Produces(StatusCodes.Status201Created);
    }

    private static async Task<IResult> CreateAsync([FromServices] IPublishEndpoint publishEndpoint,
        [FromServices] DbContext dbContext, CancellationToken cancellationToken)
    {
        var order = new Order { Id = Guid.NewGuid() };
        dbContext.Orders.Add(order);

        await publishEndpoint.Publish(new SubmitOrder { OrderId = order.Id }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Created();
    }
}

public record SubmitOrder
{
    public static readonly string QueueName = "order-queue";

    public Guid OrderId { get; init; }
}
