using Core.Application.Interfaces;
using Core.Contract.Queries.Device;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Devices;

public sealed class DeviceExternalIdByMacQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceExternalIdByMacQuery, short>
{
      public async Task<short> HandleAsync(DeviceExternalIdByMacQuery query, CancellationToken ct)
      {
           return await repo.GetExternalIdByMacAsync(query.mac,ct);
      }
}