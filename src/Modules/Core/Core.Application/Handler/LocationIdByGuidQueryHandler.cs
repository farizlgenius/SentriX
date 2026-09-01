using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class LocationIdByGuidQueryHandler(
  ILocationRepository repo
) : IQueryHandler<LocationIdByGuidQuery, int>
{
  public async Task<int> HandleAsync(LocationIdByGuidQuery query, CancellationToken ct)
  {
    return await repo.GetIdByGuidAsync(query.Guid, ct);
  }
}
