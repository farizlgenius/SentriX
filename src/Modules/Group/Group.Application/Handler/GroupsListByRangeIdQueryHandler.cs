using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Contract.Queries;
using SharedKernel.Messaging;

namespace Group.Application.Handler;

public sealed class GroupsListByRangeIdQueryHandler(IGroupRepository repo) : IQueryHandler<GroupsListByRangeIdQuery, IEnumerable<GroupSplitByMacDto>>
{
      public async Task<IEnumerable<GroupSplitByMacDto>> HandleAsync(GroupsListByRangeIdQuery query, CancellationToken ct)
      {
            return await repo.GetByRangeIdAsync(query.Ids);
      }
}