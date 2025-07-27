using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Messaging;

public static class OrderEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("orders", ListAsync)
            .Produces<List<Order>>(StatusCodes.Status200OK);
    }

    private static async Task<List<Order>> ListAsync([FromServices] DbContext dbContext, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders.ToListAsync(cancellationToken);

        return orders;
    }
}
