using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Messaging;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerUI();

        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapSwaggerUI();
        }

        app.MapPost("/produce", async (int orderId, [FromServices] ISendEndpointProvider sendEndpointProvider) =>
        {
            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-queue"));

            await endpoint.Send(new SubmitOrder { OrderId = orderId });

            return Results.Created();
        });

        app.Run();
    }

    public record SubmitOrder
    {
        public int OrderId { get; init; }
    }
}
