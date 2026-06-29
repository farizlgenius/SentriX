using SharedKernel.Messaging;
using User.Application.Interfaces;
using User.Contract.DTOs;
using User.Contract.Queries;

namespace User.Application.Handler;

public sealed class CredentialByGroupListQueryHandler(IUserRepository repo) : IQueryHandler<CredentialByGroupListQuery, IEnumerable<CredentialDto>>
{
      public async Task<IEnumerable<CredentialDto>> HandleAsync(CredentialByGroupListQuery query, CancellationToken ct)
      {
            return await repo.GetCredentialByGroupListAsync(query.Groups,ct);
      }
}