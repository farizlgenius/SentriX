using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Contract.Queries;
using SharedKernel.Messaging;

namespace Group.Application.Handler;

public sealed class GroupGuidsByMacQueryHandler(IGroupRepository repo) : IQueryHandler<GroupGuidsByMacQuery, IEnumerable<(Guid guid,short componentId)>>
{
      public async Task<IEnumerable<(Guid guid,short componentId)>> HandleAsync(GroupGuidsByMacQuery query, CancellationToken ct)
      {
            return await repo.GetGroupGuidAndComponentIdsByMacAsync(query.Mac,ct);
      }
}