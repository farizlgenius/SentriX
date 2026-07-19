using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Contract.Queries;
using SharedKernel.Messaging;

namespace Group.Application.Handler;

public sealed class GroupsListByRangeGuidQueryHandler(IGroupRepository repo) : IQueryHandler<GroupsListByRangeGuidQuery, IEnumerable<GroupSplitByMacDto>>
{
      public async Task<IEnumerable<GroupSplitByMacDto>> HandleAsync(GroupsListByRangeGuidQuery query, CancellationToken ct)
      {
            return await repo.GetByRangeGuidAsync(query.Guids);
      }
}