using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Cap;

public static class CapEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("cap", CreateAsync)
            .Produces(StatusCodes.Status201Created);
    }

    private static async Task<IResult> CreateAsync([FromServices] ICapPublisher capPublisher,
        [FromServices] DbContext dbContext, CancellationToken cancellationToken)
    {
        var order = new Order { Id = Guid.NewGuid() };
        dbContext.Orders.Add(order);

        using (var trans = dbContext.Database.BeginTransaction(capPublisher, autoCommit: true))
        {
            //await dbContext.SaveChangesAsync(cancellationToken);

            capPublisher.Publish("orders", new SubmitOrder { OrderId = order.Id });
        }

        return Results.Created();
    }
}
