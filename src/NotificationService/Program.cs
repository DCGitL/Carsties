using MassTransit;
using NotificationService.Consumer;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));
    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));

        cfg.ConfigureEndpoints(context);

    });

});
builder.Services.AddSignalR();
var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");


app.Run();
