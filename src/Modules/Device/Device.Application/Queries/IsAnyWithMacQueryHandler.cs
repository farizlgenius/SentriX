using System;
using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Queries;

public sealed class IsAnyWithMacQueryHandler(IDeviceRepository repo) : IQueryHandler<IsAnyWithMacQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyWithMacQuery query, CancellationToken ct)
      {
            return await repo.IsAnyWithMacAsync(query.MacAddress, ct);
      }
}
