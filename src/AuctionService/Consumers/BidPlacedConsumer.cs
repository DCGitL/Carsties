using System;
using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly AuctionDbContext _dbContext;

    public BidPlacedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {

        Console.WriteLine("--> consuming BidPlaced");
        var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));
        if (auction != null)
        {
            if (auction.CurrentHighBid == null
            || context.Message.BidStatus!.Contains("Accepted")
            && (!auction.CurrentHighBid.HasValue || (auction.CurrentHighBid.HasValue && context.Message.Amount > auction.CurrentHighBid.Value)))
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Bid place auction updated auctionId: {auction.Id}");

            }

        }
        else
        {
            Console.WriteLine("No bid auction available to update");
        }

    }
}
