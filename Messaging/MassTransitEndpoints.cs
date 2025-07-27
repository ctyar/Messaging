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

    private static async Task<IResult> CreateAsync(int orderId, [FromServices] ISendEndpointProvider sendEndpointProvider,
        [FromServices] DbContext dbContext, CancellationToken cancellationToken)
    {
        dbContext.Orders.Add(new Order { Id = orderId });
        await dbContext.SaveChangesAsync(cancellationToken);

        var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-queue"));

        await endpoint.Send(new SubmitOrder { OrderId = orderId }, cancellationToken);

        return Results.Created();
    }
}

public record SubmitOrder
{
    public int OrderId { get; init; }
}
