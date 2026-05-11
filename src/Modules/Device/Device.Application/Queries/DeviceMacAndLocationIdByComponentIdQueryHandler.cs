using System;
using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Queries;


public class DeviceMacAndLocationIdByComponentIdQueryHandler(IDeviceRepository repo) : IQueryHandler<DeviceMacAndLocationIdByComponentIdQuery, (string Mac, int LocationId)>
{
      public async Task<(string Mac, int LocationId)> HandleAsync(DeviceMacAndLocationIdByComponentIdQuery query, CancellationToken ct)
      {
            return await repo.GetDeviceMacAndLocationIdByComponentIdAsync(query.ComponentId, ct);
      }
}

