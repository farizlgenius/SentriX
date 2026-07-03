using Setting.Application.Interfaces;
using Setting.Contract.Queries;
using SharedKernel.Messaging;

namespace Setting.Application.Handler;

public sealed class IsAnyCardFormatNotAsyncQueryHandler(ICfmtRepository repo) : IQueryHandler<IsAnyCardFormatNotSyncQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyCardFormatNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyCardFormatNotSyncAsync(query.LocationId,query.SyncAt);
      }
}