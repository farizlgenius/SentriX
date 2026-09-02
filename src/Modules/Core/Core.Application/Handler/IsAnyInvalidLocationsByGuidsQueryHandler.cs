using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class IsAnyInvalidLocationsByGuidsQueryHandler(ILocationRepository repo) : IQueryHandler<IsAnyInvalidLocationsByGuidsQuery, IEnumerable<Guid>>
{
  public async Task<IEnumerable<Guid>> HandleAsync(IsAnyInvalidLocationsByGuidsQuery query, CancellationToken ct)
  {
    return await repo.IsAnyInvalidGuidsAsync(query.LocationGuids, ct);
  }
}