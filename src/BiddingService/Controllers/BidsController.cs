
using BiddingService.Models;
using BiddingService.RequestHelper;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BidsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IGrpcAuctionClient _grpcAuctionClient;

    public BidsController(IPublishEndpoint publishEndpoint, IGrpcAuctionClient grpcAuctionClient)
    {
        _publishEndpoint = publishEndpoint;
        _grpcAuctionClient = grpcAuctionClient;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceBid(string auctionId, int amount)
    {
        var auction = await DB.Find<Auction>().OneAsync(auctionId);
        if (auction is null)
        {
            auction = _grpcAuctionClient.GetAuction(auctionId);
            if (auction is null)
            {
                return BadRequest("Cannot accept bid on this auction at this time");
            }
        }

        if (auction.Seller == User?.Identity?.Name)
        {
            return BadRequest("You cannot bid on your own auction");
        }

        var bid = new Bid
        {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = User?.Identity?.Name!
        };

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            var highBid = await DB.Find<Bid>().Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(x => x.Amount))
            .ExecuteFirstAsync();
            if (highBid != null && amount > highBid.Amount || highBid == null)
            {
                bid.BidStatus = amount > auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
            }
            if (highBid != null && bid.Amount <= highBid.Amount)
            {
                bid.BidStatus = BidStatus.TooLow;
            }

        }
        await DB.SaveAsync(bid);

        var placeBid = bid.ToBidPlaced();
        await _publishEndpoint.Publish(placeBid);


        return Ok(bid.ToDto());

    }

    [HttpGet("{auctionId}")]
    public async Task<IActionResult> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>().Match(x => x.AuctionId == auctionId)
        .Sort(x => x.Descending(a => a.BidTime))
        .ExecuteAsync();
        if (bids is null)
        {
            return NotFound($"No bids for auctionId ${auctionId} were found");
        }
        return Ok(bids.ToDto());
    }

}
