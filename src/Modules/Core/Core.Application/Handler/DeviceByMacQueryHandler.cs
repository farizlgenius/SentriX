using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class DeviceByMacQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceByMacQuery, DeviceDto>
{
      public async Task<DeviceDto> HandleAsync(DeviceByMacQuery query, CancellationToken ct)
      {
            return await repo.GetByMacAsync(query.mac,ct);
      }
}