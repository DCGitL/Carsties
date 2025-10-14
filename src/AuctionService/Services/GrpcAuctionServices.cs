using System;
using AuctionService.Data;
using AuctionService.protos;
using Grpc.Core;

namespace AuctionService.Services;

public class GrpcAuctionServices : GrpcAuction.GrpcAuctionBase
{
    private readonly AuctionDbContext _auctionDbContext;

    public GrpcAuctionServices(AuctionDbContext auctionDbContext)
    {
        _auctionDbContext = auctionDbContext;
    }

    public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request, ServerCallContext context)
    {
        Console.WriteLine("==> Receive Grpc request for auction");
        var auction = await _auctionDbContext.Auctions.FindAsync(Guid.Parse(request.Id))
        ?? throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

        var response = new GrpcAuctionResponse
        {
            Auction = new GrpcAuctionModel
            {
                AuctionEnd = auction.AuctionEnd.ToString(),
                Id = auction.Id.ToString(),
                ReservePrice = auction.ReservePrice,
                Seller = auction.Seller,

            }
        };

        return response;

    }

}
