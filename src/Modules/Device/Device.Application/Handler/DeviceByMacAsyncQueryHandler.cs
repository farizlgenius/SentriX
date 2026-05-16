using System;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class DeviceByMacAsyncQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceByMacAsyncQuery, DeviceDto>
{
      public async Task<DeviceDto> HandleAsync(DeviceByMacAsyncQuery query, CancellationToken ct)
      {
            return await repo.GetByMacAsync(query.Mac);
      }
}
