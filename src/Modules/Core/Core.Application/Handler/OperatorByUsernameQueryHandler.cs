using Core.Application.Interfaces;
using Core.Contract.DTOs.Operator;
using Core.Contract.DTOs.User;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class OperatorByUsernameQueryHandler(IOperatorRepository repo) : IQueryHandler<OperatorByUsernameQuery, OperatorDto>
{
  public async Task<OperatorDto> HandleAsync(OperatorByUsernameQuery query, CancellationToken ct)
  {
    return await repo.GetOperatorByUsernameAsync(query.username, ct);
  }
}