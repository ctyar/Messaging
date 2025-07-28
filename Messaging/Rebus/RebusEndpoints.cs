using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;

namespace Messaging.Cap;

public static class RebusEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("rebus", CreateAsync)
            .Produces(StatusCodes.Status201Created);
    }

    private static async Task<IResult> CreateAsync([FromServices] IBus bus,
        [FromServices] DbContext dbContext, CancellationToken cancellationToken)
    {
        var order = new Order { Id = Guid.NewGuid() };
        dbContext.Orders.Add(order);

        await dbContext.SaveChangesAsync(cancellationToken);

        await bus.Send(new SubmitOrder { OrderId = order.Id });

        return Results.Created();
    }
}
