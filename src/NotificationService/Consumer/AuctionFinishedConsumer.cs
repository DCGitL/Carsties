using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumer;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<AuctionFinishedConsumer> _logger;

    public AuctionFinishedConsumer(IHubContext<NotificationHub> hubContext, ILogger<AuctionFinishedConsumer> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        _logger.LogInformation("==> auction finished messsage received");

        await _hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
    }
}
