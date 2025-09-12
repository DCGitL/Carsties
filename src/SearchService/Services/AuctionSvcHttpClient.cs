using System;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public interface IAuctionSvcHttpClient
{
    Task<List<Item>?> GetItemsForSearchDb();
}

public class AuctionSvcHttpClient : IAuctionSvcHttpClient
{
    private readonly IHttpClientFactory _httpClient;

    public AuctionSvcHttpClient(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;

    }


    public async Task<List<Item>?> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
        .Sort(x => x.Descending(x => x.UpdatedAt))
        .Project(x => x.UpdatedAt.ToString())
        .ExecuteFirstAsync();
        var client = _httpClient.CreateClient("AuctionClient");
        var response = await client.GetAsync($"/api/auctions?date={lastUpdated}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<Item>>();
            return result;
        }

        Console.WriteLine("No items found");
        return null;



    }

}
