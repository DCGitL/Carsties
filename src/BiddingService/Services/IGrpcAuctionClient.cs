using BiddingService.Models;

namespace BiddingService.Services;

public interface IGrpcAuctionClient
{
    Auction? GetAuction(string id);
}
