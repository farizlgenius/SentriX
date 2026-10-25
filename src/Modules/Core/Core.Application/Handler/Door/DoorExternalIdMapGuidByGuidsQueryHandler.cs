using Core.Application.Interfaces;
using Core.Contract.Queries.Door;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Door;

public sealed class DoorExternalIdMapGuidByGuidsQueryHandler(IDoorRepository repo) : IQueryHandler<DoorExternalIdMapGuidByGuidRangeQuery, Dictionary<Guid, short>>
{
      public async Task<Dictionary<Guid, short>> HandleAsync(DoorExternalIdMapGuidByGuidRangeQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdMapGuidByGuidRangeAsync(query.guids,ct);
      }
}