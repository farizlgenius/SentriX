using System;
using Location.Contract.Events;
using Operator.Application.Interfaces;
using SharedKernel.Messaging;

namespace Operator.Application.Handler;

public sealed class LocationCreatedHandler(IOperatorRepository repo) : IEventHandler<LocationCreatedEvent>
{
      public async Task HandleAsync(LocationCreatedEvent @event, CancellationToken ct)
      {
            await repo.AddOperatorLocationsAsync(1, @event.LocationId,ct);
      }
}
