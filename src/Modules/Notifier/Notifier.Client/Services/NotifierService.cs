using System;
using Microsoft.AspNetCore.SignalR;
using Notifier.Client.Hubs;
using Notifier.Contract.Interfaces;

namespace Notifier.Client.Services;

public sealed class NotifierService : INotifier
{
    private readonly IHubContext<NotifierHub> _hub;

    public NotifierService(IHubContext<NotifierHub> hub)
    {
        _hub = hub;
    }

    public async Task SendToTopic(string topic, object payload, CancellationToken ct = default)
    {
        await _hub.Clients.Group(topic)
            .SendAsync(topic, payload, ct);

        // await _hub.Clients.All.SendAsync(topic,payload);

    }

    public async Task SendToUser(string userId, object payload, CancellationToken ct = default)
    {
        await _hub.Clients.Group($"user-{userId}")
            .SendAsync("private", payload, ct);
    }
}