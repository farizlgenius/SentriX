using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class RoleIdByGuidQueryHandler(
  IRoleRepository repo
) : IQueryHandler<RoleIdByGuidQuery, int>
{
  public async Task<int> HandleAsync(RoleIdByGuidQuery query, CancellationToken ct)
  {
    return await repo.GetIdByGuidAsync(query.guid, ct);
  }
}