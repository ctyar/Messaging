using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.MassTransit;

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
