using System;
using Location.Contract.Events;
using Operator.Application.Interfaces;
using SharedKernel.Messaging;

namespace Operator.Application.Handler;

public sealed class LocationDeletedHandler(IOperatorRepository repo) : IEventHandler<LocationDeletedEvent>
{
      public async Task HandleAsync(LocationDeletedEvent @event, CancellationToken ct)
      {
            await repo.RemoveOperatorLocationsAsync(@event.LocationId, ct);
      }
}
