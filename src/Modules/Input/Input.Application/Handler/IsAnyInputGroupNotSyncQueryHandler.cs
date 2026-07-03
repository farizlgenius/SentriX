using Input.Application.Interfaces;
using Input.Contract.Queries;
using SharedKernel.Messaging;

namespace Input.Application.Handler;

public sealed class IsAnyInputGroupNotSyncQueryHandler(IInputRepository repo) : IQueryHandler<IsAnyInputGroupNotSyncQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyInputGroupNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyInputGroupNotSyncAsync(query.Mac,query.LocationId,query.SyncAt,ct);
      }
}