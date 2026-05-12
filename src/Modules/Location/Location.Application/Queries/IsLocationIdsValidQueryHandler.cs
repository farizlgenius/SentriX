using System;
using Location.Application.Interfaces;
using Location.Contract.Queries;
using SharedKernel.Messaging;

namespace Location.Application.Queries;

public sealed class IsLocationIdsValidQueryHandler(ILocationRepository repo) : IQueryHandler<IsLocationIdsValidQuery, bool>
{
      public async Task<bool> HandleAsync(IsLocationIdsValidQuery query, CancellationToken ct)
      {
            return await repo.IsLocationIdsValidAsync(query.LocationIds);
      }
}
