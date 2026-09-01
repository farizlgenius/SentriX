using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class GroupIdsByGuidsQueryHandler(
  IGroupRepository repo
  ) : IQueryHandler<GroupIdsByGuidsQuery, IEnumerable<int>>
{
      public async Task<IEnumerable<int>> HandleAsync(GroupIdsByGuidsQuery query, CancellationToken ct = default)
      {
            return await repo.GetIdsByGuidsAsync(query.Guids, ct);
      }
}