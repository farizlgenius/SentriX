using System;
using Location.Application.Interfaces;
using Location.Contract.Queries;
using SharedKernel.Messaging;

namespace Location.Application.Queries;

public sealed class LocationNameByIdQueryHandler(ILocationRepository repo) : IQueryHandler<LocationNameByIdQuery, string>
{
      public async Task<string> HandleAsync(LocationNameByIdQuery query, CancellationToken ct)
      {
            return await repo.GetNameByIdAsync(query.LocationId, ct) ?? "";
      }
}
