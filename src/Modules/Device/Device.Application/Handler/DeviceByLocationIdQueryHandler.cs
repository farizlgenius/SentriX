using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class DeviceByLocationIdQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceByLocationIdQuery, IEnumerable<DeviceDto>>
{
      public async Task<IEnumerable<DeviceDto>> HandleAsync(DeviceByLocationIdQuery query, CancellationToken ct)
      {
            return await repo.GetDeviceByLocationIdAsync(query.LocationId,ct);
      }
}