using Core.Application.Interfaces;
using Core.Contract.Queries.Device;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Devices;

public sealed class IsAnyDeviceExeternalIdQueryHandler(IDeviceRepository repo) : IQueryHandler<IsAnyDeviceExeternalIdQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyDeviceExeternalIdQuery query, CancellationToken ct)
      {
           return await repo.IsAnyDeviceExeternalIdAsync(query.externalId,ct);
      }
}