using System;
using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Queries;

public class IsAnyModuleBySerialNumberQueryHandler(IDeviceRepository repo) : IQueryHandler<IsAnyModuleBySerialNumberQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyModuleBySerialNumberQuery query, CancellationToken ct)
      {
            return await repo.IsAnyModuleBySerialNumberAsync(query.SerialNumber);
      }
}
