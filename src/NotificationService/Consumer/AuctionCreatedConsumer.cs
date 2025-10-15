using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumer;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<AuctionCreatedConsumer> _logger;

    public AuctionCreatedConsumer(IHubContext<NotificationHub> hubContext, ILogger<AuctionCreatedConsumer> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        _logger.LogInformation("==> auction created messsage received");

        await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
    }
}
