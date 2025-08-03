using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Brighter;

public static class BrighterEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("brighter", CreateAsync)
            .Produces(StatusCodes.Status201Created);
    }

    private static async Task<IResult> CreateAsync([FromServices] ICapPublisher capPublisher,
        [FromServices] DbContext dbContext, CancellationToken cancellationToken)
    {
        var order = new Order { Id = Guid.NewGuid() };
        dbContext.Orders.Add(order);

        using (var trans = dbContext.Database.BeginTransaction(capPublisher, autoCommit: true))
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            capPublisher.Publish("orders", new SubmitOrder { OrderId = order.Id });
        }

        // DepositPost

        // https://brightercommand.gitbook.io/paramore-brighter-documentation/brighter-configuration/brighterbasicconfiguration#configuring-the-service-activator

        return Results.Created();
    }
}
