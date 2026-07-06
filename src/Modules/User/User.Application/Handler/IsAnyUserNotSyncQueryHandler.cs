using Door.Contract.Queries;
using Group.Contract.Queries;
using SharedKernel.Messaging;
using User.Application.Interfaces;
using User.Contract.Queries;

namespace User.Application.Handler;

public sealed class IsAnyUserNotSyncQueryHandler(IUserRepository repo,IMessageBus bus) : IQueryHandler<IsAnyUserNotSyncQuery, bool>
{

      public async Task<bool> HandleAsync(IsAnyUserNotSyncQuery query, CancellationToken ct)
      {
            var res = await bus.QueryAsync(new GroupIdListByMacQuery(query.Mac));
            return await repo.IsAnyUserNotSyncAsync(res.Select(x => x.id).ToArray(),query.LocationId,query.SyncAt);
      }
}