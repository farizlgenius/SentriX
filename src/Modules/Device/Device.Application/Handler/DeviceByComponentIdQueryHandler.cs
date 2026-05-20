using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class DeviceByComponentIdQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceByComponentIdQuery, DeviceDto>
{
      public async Task<DeviceDto> HandleAsync(DeviceByComponentIdQuery query, CancellationToken ct)
      {
            return await repo.GetDeviceByComponentIdAsync(query.ScpId);
      }
}