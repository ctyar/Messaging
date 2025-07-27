using MassTransit;

namespace Messaging;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerUI();

        builder.Services.AddDbContext<DbContext>();
        using (var context = new DbContext())
        {
            context.Database.EnsureCreated();
        }

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

        OrderEndpoints.Map(app);
        MassTransitEndpoints.Map(app);

        app.Run();
    }
}
