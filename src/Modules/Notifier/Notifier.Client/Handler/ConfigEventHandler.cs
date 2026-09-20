using Notifier.Contract.Events;
using Notifier.Contract.Interfaces;
using Notifier.Contract.Topics;
using SharedKernel.Messaging;

namespace Notifier.Client.Handler;

public sealed class ConfigEventHandler(INotifier notifier) : IEventHandler<ConfigEvent>
{
      public async Task HandleAsync(ConfigEvent @event, CancellationToken ct)
      {
            await notifier.SendToTopic(DeviceNotifierTopic.CONFIG,@event.config,ct);
      }
}