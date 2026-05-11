using System;

namespace Notifier.Contract.Interfaces;

public interface INotifier
{
    Task SendToTopic(string topic, object payload, CancellationToken ct=default);
    Task SendToUser(string userId, object payload, CancellationToken ct=default);
}
