using System;
using Device.Contract.Events;
using Notifier.Contract.Constants;
using Notifier.Contract.Interfaces;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class IdReportUpdatedEventHandler(INotifier notifier) : IEventHandler<IdReportUpdatedEvent>
{
      public async Task HandleAsync(IdReportUpdatedEvent @event, CancellationToken ct)
      {
            await notifier.SendToTopic(NotifierTopic.IDREPORT, @event, ct);
      }
}
      