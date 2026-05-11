using System;
using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Queries;

public class DeviceMacByComponentIdQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceMacByComponentIdQuery, string>
{
      public async Task<string> HandleAsync(DeviceMacByComponentIdQuery query, CancellationToken ct)
      {
            return await repo.GetDeviceMacByComponentIdAsync(query.ComponentId, ct); 
      }
}

