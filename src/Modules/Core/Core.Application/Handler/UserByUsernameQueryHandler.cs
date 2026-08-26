using Core.Application.Interfaces;

using Core.Contract.DTOs.User;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class UserByUsernameQueryHandler(IUserRepository repo) : IQueryHandler<UserByUsernameQuery, UserDto>
{
  public async Task<UserDto> HandleAsync(UserByUsernameQuery query, CancellationToken ct)
  {
    return await repo.GetByUsernameAsync(query.username, ct);
  }
}