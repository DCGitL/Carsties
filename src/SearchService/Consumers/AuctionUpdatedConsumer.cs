using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine($"--> Consuming Aution updated with id {context.Message.Id}");



        var result = await DB.Update<Item>()
          .Match(x => x.ID == context.Message.Id)
          .Modify(x => x.Model, context.Message.Model)
          .Modify(x => x.Make, context.Message.Make)
          .Modify(x => x.Year, context.Message.Year)
          .Modify(x => x.Color, context.Message.Color)
          .Modify(x => x.Mileage, context.Message.Mileage)
          .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(AuctionUpdated), "Problem updating auction");

    }
}
