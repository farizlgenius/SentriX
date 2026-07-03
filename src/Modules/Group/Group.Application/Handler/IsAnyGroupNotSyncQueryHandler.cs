using Group.Application.Interfaces;
using Group.Contract.Queries;
using SharedKernel.Messaging;

namespace Group.Application.Handler;

public sealed class IsAnyGroupNotSyncQueryHandler(IGroupRepository repo) : IQueryHandler<IsAnyGroupNotSyncQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyGroupNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyGroupNotSyncQueryAsync(query.LocationId,query.SyncAt);
      }
}