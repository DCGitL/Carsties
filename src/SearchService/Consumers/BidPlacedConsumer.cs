using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("--> Consume bid placed");

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
        if (auction != null)
        {
            Console.WriteLine($"==> Auction for bid with auctionId {auction.ID}");
            if (context.Message.BidStatus!.Contains("Accepted") //Accepted
              && (!auction.CurrentHighBid.HasValue || (auction.CurrentHighBid.HasValue && context.Message.Amount > auction.CurrentHighBid.Value)))
            {

                auction.CurrentHighBid = context.Message.Amount;
                await auction.SaveAsync();
                Console.WriteLine($"Bid Placed auction updated auctionId {auction.ID}");

            }
        }

    }



}
