using SharedKernel.Messaging;
using User.Application.Interfaces;
using User.Contract.DTOs;
using User.Contract.Queries;

namespace User.Application.Handler;

public sealed class UsersByGroupGuidsQueryHandler(IUserRepository repo) : IQueryHandler<UsersByGroupGuidsQuery, IEnumerable<UserDto>>
{
      public async Task<IEnumerable<UserDto>> HandleAsync(UsersByGroupGuidsQuery query, CancellationToken ct)
      {
            return await repo.GetUserByGroupGuidsAsync(query.guids,ct);
      }
}