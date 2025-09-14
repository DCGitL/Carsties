
using System.Net;
using System.Net.Http.Headers;
using AutoMapper;
using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService.Consumers;
using SearchService.Data;
using SearchService.RequestHelpers;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var configuration = builder.Configuration;
builder.Services.AddScoped<IAuctionSvcHttpClient, AuctionSvcHttpClient>();
builder.Services.AddHttpClient("AuctionClient", options =>
{
    var baseUrl = configuration.GetSection("AuctionServiceClient:BaseUrl").Value;
    if (string.IsNullOrEmpty(baseUrl))
        throw new InvalidOperationException("Missing SearchService:BaseUrl in appsetting.json file");

    options.BaseAddress = new Uri(baseUrl);
    options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

}).AddPolicyHandler(GetPolicy());
var mappconfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfiles());
});
IMapper mapper = mappconfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    x.AddConsumersFromNamespaceContaining<AuctionUpdatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("updated", false));
    x.AddConsumersFromNamespaceContaining<AuctionDeletedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("deleted", false));
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", 5672, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("search-auction-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);

        });
        cfg.ConfigureEndpoints(context); //automatically configure endpoints

    });

});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("internal");
}



app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {

        Console.WriteLine(e);
    }
});



app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
 => HttpPolicyExtensions.HandleTransientHttpError()
 .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
 .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));