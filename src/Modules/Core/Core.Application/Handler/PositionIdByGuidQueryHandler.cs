using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class PositionIdByGuidQueryHandler(
  IPositionRepository repo
) : IQueryHandler<PositionIdByGuidQuery, int>
{
  public async Task<int> HandleAsync(PositionIdByGuidQuery query, CancellationToken ct)
  {
    return await repo.GetIdByGuidAsync(query.guid);
  }
}