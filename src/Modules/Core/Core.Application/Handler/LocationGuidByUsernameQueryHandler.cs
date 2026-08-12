using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class LocationGuidByUsernameQueryHandler(IOperatorRepository repo) : IQueryHandler<LocationGuidByUsernameQuery, IEnumerable<Guid>>
{
  public async Task<IEnumerable<Guid>> HandleAsync(LocationGuidByUsernameQuery query, CancellationToken ct)
  {
    return await repo.GetLocationGuidByUsernameAsync(query.username, ct);
  }
}