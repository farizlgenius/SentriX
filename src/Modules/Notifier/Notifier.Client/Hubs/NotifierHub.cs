using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Notifier.Client.Hubs;

[Authorize]
public class NotifierHub : Hub
{
      public async Task Subscribe(string topic)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, topic);
         Console.WriteLine($"User {Context.User?.Identity?.Name} joined {topic}");
    }

    public async Task Unsubscribe(string topic)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, topic);
    }

}
