using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumer;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<BidPlacedConsumer> _logger;

    public BidPlacedConsumer(IHubContext<NotificationHub> hubContext, ILogger<BidPlacedConsumer> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        _logger.LogInformation("==> bid placed messsage received");

        await _hubContext.Clients.All.SendAsync("Bidplaced", context.Message);
    }
}
