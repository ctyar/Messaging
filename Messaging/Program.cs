using DotNetCore.CAP;
using MassTransit;
using Messaging.Cap;
using Messaging.MassTransit;

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

        AddMassTransit(builder);
        AddCap(builder);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapSwaggerUI();
        }

        OrderEndpoints.Map(app);
        MassTransitEndpoints.Map(app);
        CapEndpoints.Map(app);

        app.Run();
    }

    private static void AddCap(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICapSubscribe, OrderSubscriber>();

        builder.Services.AddCap(x =>
        {
            x.UseEntityFramework<DbContext>();

            x.UseSqlite(cfg =>
            {
                cfg.ConnectionString = "Data Source=./Database.db";
            });

            x.UseRabbitMQ(o =>
            {
                o.HostName = "localhost";
                o.UserName = "guest";
                o.Password = "guest";
            });
        });
    }

    private static void AddMassTransit(WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<SubmitOrderConsumer>();

            x.AddEntityFrameworkOutbox<DbContext>(o =>
            {
                o.UseSqlServer();
            });

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
    }
}
