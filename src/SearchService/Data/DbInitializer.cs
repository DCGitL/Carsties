using System;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{

    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings
             .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
        .Key(x => x.Make, KeyType.Text)
        .Key(x => x.Model, KeyType.Text)
        .Key(x => x.Color, KeyType.Text)
        .CreateAsync();

        var count = await DB.CountAsync<Item>();
        using var scope = app.Services.CreateScope();
        var clientService = scope.ServiceProvider.GetRequiredService<IAuctionSvcHttpClient>();
        var items = await clientService.GetItemsForSearchDb();

        Console.WriteLine(items?.Count + " Items return from the auction service");
        if (items?.Count > 0)
        {
            await DB.SaveAsync(items);
        }

    }

}
