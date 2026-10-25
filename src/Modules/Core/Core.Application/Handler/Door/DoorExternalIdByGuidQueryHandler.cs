using Core.Application.Interfaces;
using Core.Contract.Queries.Door;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Door;

public sealed class DoorExternalIdByGuidQueryHandler(IDoorRepository repo) : IQueryHandler<DoorExternalIdRangeByGuidQuery, IEnumerable<short>>
{
      public async Task<IEnumerable<short>> HandleAsync(DoorExternalIdRangeByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdRangeByGuidAsync(query.guid,ct);
      }
}