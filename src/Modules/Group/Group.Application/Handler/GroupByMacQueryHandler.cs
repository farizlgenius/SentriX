using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Contract.Queries;
using SharedKernel.Messaging;

namespace Group.Application.Handler;

public sealed class GroupByMacQueryHandler(IGroupRepository repo) : IQueryHandler<GroupByMacAndDeviceTypeQuery, IEnumerable<GroupDto>>
{
      public async Task<IEnumerable<GroupDto>> HandleAsync(GroupByMacAndDeviceTypeQuery query, CancellationToken ct)
      {
            return await repo.GetGroupByMacAsync(query.Mac,query.Type);
      }
}