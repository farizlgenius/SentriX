using Input.Application.Interfaces;
using Input.Contract.Queries;
using SharedKernel.Messaging;

namespace Input.Application.Handler;

public sealed class IsAnyInputNotSyncQueryHandler(IInputRepository repo) : IQueryHandler<IsAnyInputNotSyncQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyInputNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyInputNotSyncAsync(query.Mac,query.LocationId,query.SyncAt,ct);
      }
}