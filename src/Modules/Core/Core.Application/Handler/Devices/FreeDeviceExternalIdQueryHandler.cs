using Core.Application.Interfaces;
using Core.Contract.Queries.Device;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Devices;

public sealed class FreeDeviceExternalIdQueryHandler(IDeviceRepository repo) : IQueryHandler<FreeDeviceExternalIdQuery, short>
{
      public async Task<short> HandleAsync(FreeDeviceExternalIdQuery query, CancellationToken ct)
      {
            return await repo.GetFreeExternalIdAsync(100,[],ct);
      }
}