using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Contract.Queries;
using SharedKernel.Messaging;

namespace Group.Application.Handler;

public sealed class GroupIdListByMacQueryHandler(IGroupRepository repo) : IQueryHandler<GroupIdListByMacQuery, IEnumerable<(int id,short componentId)>>
{
      public async Task<IEnumerable<(int id,short componentId)>> HandleAsync(GroupIdListByMacQuery query, CancellationToken ct)
      {
            return await repo.GetGroupIdAndComponentIdListByMacAsync(query.Mac,ct);
      }
}