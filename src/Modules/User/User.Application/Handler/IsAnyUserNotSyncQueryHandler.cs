using Door.Contract.Queries;
using SharedKernel.Messaging;
using User.Application.Interfaces;

namespace User.Application.Handler;

public sealed class IsAnyUserNotSyncQueryHandler(IUserRepository repo) : IQueryHandler<IsAnyDoorNotSyncQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyDoorNotSyncQuery query, CancellationToken ct)
      {
            return await repo.IsAnyDoorNotSyncAsync(query.LocationId,query.SyncAt);
      }
}