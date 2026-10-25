using Core.Application.Interfaces;
using Core.Contract.Queries.Group;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Group;

public sealed class GroupExternalIdByGuidQueryHandler(IGroupRepository repo) : IQueryHandler<GroupExternalIdByGuidQuery, short>
{
      public async Task<short> HandleAsync(GroupExternalIdByGuidQuery query, CancellationToken ct)
      {
           return await repo.GetExternalIdByGuidAsync(query.guid,ct);
      }
}