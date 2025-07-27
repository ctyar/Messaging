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

    private static async Task<IResult> CreateAsync([FromServices] ISendEndpointProvider sendEndpointProvider,
        [FromServices] DbContext dbContext, CancellationToken cancellationToken)
    {
        var order = new Order { Id = Guid.NewGuid() };
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{SubmitOrder.QueueName}"));

        await endpoint.Send(new SubmitOrder { OrderId = order.Id }, cancellationToken);

        return Results.Created();
    }
}

public record SubmitOrder
{
    public static readonly string QueueName = "order-queue";

    public Guid OrderId { get; init; }
}
