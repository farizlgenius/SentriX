using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class LocationIdsByGuidsQueryHandler(ILocationRepository repo) : IQueryHandler<LocationIdsByGuidsQuery, IEnumerable<int>>
{
      public async Task<IEnumerable<int>> HandleAsync(LocationIdsByGuidsQuery query, CancellationToken ct)
      {
            return await repo.GetIdsByGuidsAsync(query.Guids, ct);
      }
}