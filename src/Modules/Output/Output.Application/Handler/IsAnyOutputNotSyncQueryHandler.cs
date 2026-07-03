using Output.Application.Interfaces;
using Output.Contract.Queries;
using SharedKernel.Messaging;

namespace Output.Application.Handler;

public sealed class IsAnyOutputNotSyncQueryHandler(IOutputRepository repo) : IQueryHandler<IsAnyOutputNotSyncQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyOutputNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyOutputNotSyncAsync(query.Mac,query.LocationId,query.SyncAt,ct);
      }
}