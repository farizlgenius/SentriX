using Group.Application.Interfaces;
using Group.Contract.Queries;
using SharedKernel.Messaging;

namespace Group.Application.Handler;

public sealed class MacsByGroupIdsQueryHandler(IGroupRepository repo) : IQueryHandler<MacsByGroupIdsQuery, IEnumerable<string>>
{
      public async Task<IEnumerable<string>> HandleAsync(MacsByGroupIdsQuery query, CancellationToken ct)
      {
            return await repo.MacsByGroupIdAsync(query.Ids);
      }
}