using System;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class IdByComponentIdHandler(IDeviceRepository repo) : IQueryHandler<IdByComponentIdQuery, int>
{

      public async Task<int> HandleAsync(IdByComponentIdQuery query, CancellationToken ct)
      {
            return await repo.GetIdByComponentIdAsync((short)query.ComponentId);
      }
}
