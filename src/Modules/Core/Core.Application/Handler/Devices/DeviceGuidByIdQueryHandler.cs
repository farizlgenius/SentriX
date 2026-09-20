using Core.Application.Interfaces;
using Core.Contract.Queries.Device;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Devices;

public sealed class DeviceGuidByIdQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceGuidByIdQuery, Guid>
{
      public async Task<Guid> HandleAsync(DeviceGuidByIdQuery query, CancellationToken ct)
      {
            return await repo.GetGuidByIdAsync(query.id,ct);
      }
}