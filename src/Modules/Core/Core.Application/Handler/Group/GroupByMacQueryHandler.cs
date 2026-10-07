using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Contract.DTOs.Group;
using Core.Contract.Queries.Group;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Door;

public sealed class GroupByMacQueryHandler(IGroupRepository repo) : IQueryHandler<GroupByMacQuery, IEnumerable<GroupDto>>
{
      public async Task<IEnumerable<GroupDto>> HandleAsync(GroupByMacQuery query, CancellationToken ct)
      {
            return await repo.GetByMacAsync(query.mac,ct);
      }
}