using System;
using Notifier.Contract.Constants;
using Notifier.Contract.Events;
using Notifier.Contract.Interfaces;
using SharedKernel.Messaging;

namespace Notifier.Client.Handler;

public sealed class ExceptionEventHandler(INotifier notifier) : IEventHandler<ExceptionEvent>
{
      public async Task HandleAsync(ExceptionEvent @event, CancellationToken ct)
      {
            await notifier.SendToTopic(NotifierTopic.EXCEPTION,@event.Message,ct);
      }
}
