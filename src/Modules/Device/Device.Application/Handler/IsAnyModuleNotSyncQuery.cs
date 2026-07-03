using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class IsAnyModuleNotSyncQueryHandler(IDeviceRepository repo) : IQueryHandler<IsAnyModuleNotSyncQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyModuleNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyModuleNotSyncAsync(query.Mac,query.LocationId,query.SyncAt);
      }
}