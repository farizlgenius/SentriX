using System;
using Location.Application.Interfaces;
using Location.Contract.Queries;
using SharedKernel.Messaging;

namespace Location.Application.Queries;

public sealed class IsAnyLocationByIdQueryHandler(ILocationRepository repo) : IQueryHandler<IsAnyLocationByIdQuery, bool>
{
      public async Task<bool> HandleAsync(IsAnyLocationByIdQuery query, CancellationToken ct)
      {
            return await repo.IsAnyByIdAsync(query.LocationId, ct);
      }
}
