using Door.Application.Interfaces;
using Door.Contract.DTOs;
using Door.Contract.Queries;
using SharedKernel.Messaging;

namespace Door.Application.Handler;

public sealed class IsAnyDoorNotSyncQueryHandler(IDoorRepository repo) : IQueryHandler<IsAnyDoorNotSyncQuery, bool>
{

      public async Task<bool> HandleAsync(IsAnyDoorNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyDoorNotSyncAsync(query.Mac,query.LocationId,query.SyncAt);
      }
}