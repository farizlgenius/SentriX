using SharedKernel.Messaging;
using Time.Application.Interfaces;
using Time.Contract.Queries;

namespace Time.Application.Handler;

public sealed class IsAnyTimeZoneNotSyncQueryHandler(ITimeZoneRepository repo) : IQueryHandler<IsAnyTimeZoneNotSyncQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyTimeZoneNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyTimeZoneNotSyncAsync(query.LocationId,query.SyncAt);
      }
}