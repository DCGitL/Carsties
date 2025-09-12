
using System.Net;
using System.Net.Http.Headers;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Models;
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