using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class IsAnyLocationByGuidQueryHandler(ILocationRepository repo) : IQueryHandler<IsAnyLocationByGuidQuery, bool>
{


  public async Task<bool> HandleAsync(IsAnyLocationByGuidQuery query, CancellationToken ct = default)
  {
    return await repo.IsAnyGuidAsync(query.LocationGuid);
  }
}