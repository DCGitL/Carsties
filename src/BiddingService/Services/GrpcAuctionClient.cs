using System;
using AuctionService.protos;
using BiddingService.Models;
using Grpc.Net.Client;
using MongoDB.Entities;

namespace BiddingService.Services;

public class GrpcAuctionClient : IGrpcAuctionClient
{
    private readonly ILogger<GrpcAuctionClient> _logger;
    private readonly IConfiguration _configuration;

    public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Auction? GetAuction(string id)
    {
        _logger.LogInformation("Calling GRPC Service");
        var channel = GrpcChannel.ForAddress(_configuration["GrpcAuction"]!);
        var client = new GrpcAuction.GrpcAuctionClient(channel);
        var request = new GetAuctionRequest
        {
            Id = id
        };

        try
        {
            var result = client.GetAuction(request);

            var auction = new Auction
            {
                ID = result.Auction.Id,
                Seller = result.Auction.Seller,
                AuctionEnd = DateTime.Parse(result.Auction.AuctionEnd),
                ReservePrice = result.Auction.ReservePrice,
            };
            return auction;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Could not get auction from GRPC server");
            return null;
        }


    }

}
