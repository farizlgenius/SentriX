using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class HashedByUsernameQueryHandler(IUserRepository repo) : IQueryHandler<HashedByUsernameQuery, string>
{
  public async Task<string> HandleAsync(HashedByUsernameQuery query, CancellationToken ct)
  {
    return await repo.GetHashByUsernameAsync(query.username, ct);
  }
}