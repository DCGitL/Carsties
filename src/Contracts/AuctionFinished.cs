using System;

namespace Contracts;

public class AuctionFinished
{
    public bool ItemSold { get; set; }
    public required string AuctionId { get; set; }
    public string Winner { get; set; } = default!;
    public string Seller { get; set; } = default!;
    public int? Amount { get; set; }

}
