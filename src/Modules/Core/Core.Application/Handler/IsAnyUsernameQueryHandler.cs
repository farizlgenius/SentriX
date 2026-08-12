using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class IsAnyUsernameQueryHandler(IOperatorRepository repo) : IQueryHandler<IsAnyUsernameQuery, bool>
{
  public async Task<bool> HandleAsync(IsAnyUsernameQuery query, CancellationToken ct)
  {
    return await repo.IsAnyUsernameAsync(query.username, ct);
  }
}