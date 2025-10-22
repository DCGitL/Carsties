using System;
using MongoDB.Entities;

namespace SearchService.Models;

public class Item : Entity
{

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime AuctionEnd { get; set; }
    public required string Seller { get; set; }
    public string? Winner { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public required string Color { get; set; }
    public int Mileage { get; set; }
    public string? ImageUrl { get; set; }
    public string Status { get; set; } = default!;

    public int ReservePrice { get; set; }
    public int? SoldAmount { get; set; }
    public int? CurrentHighBid { get; set; }

}
