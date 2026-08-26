using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class RoleGuidByUsernameQueryHandler(IUserRepository repo) : IQueryHandler<RoleGuidByUsernameQuery, Guid>
{
  public async Task<Guid> HandleAsync(RoleGuidByUsernameQuery query, CancellationToken ct)
  {
    return await repo.GetRoleGuidByUsernameAsync(query.username, ct);
  }
}