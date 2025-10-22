using BiddingService.DTOs;
using BiddingService.Models;
using Contracts;

namespace BiddingService.RequestHelper;

public static class MapperExtension
{
    public static BidDto ToDto(this Bid bid)
    {
        return new BidDto
        {
            Id = bid.ID,
            AuctionId = bid.AuctionId,
            Bidder = bid.Bidder,
            Amount = bid.Amount,
            BidStatus = bid.BidStatus.ToString(),
            BidTime = bid.BidTime

        };
    }

    public static List<BidDto> ToDto(this List<Bid> bids)
    {
        var results = bids.Select(x => x.ToDto()).ToList();
        return results;
    }

    public static BidPlaced ToBidPlaced(this Bid bid)
    {
        return new BidPlaced
        {
            Id = bid.ID,
            AuctionId = bid.AuctionId,
            BidStatus = bid.BidStatus.ToString(),
            Bidder = bid.Bidder,
            Amount = bid.Amount,
            BidTime = bid.BidTime,
        };
    }
}
