using Core.Application.Interfaces;
using Core.Contract.Queries.Time;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Times;

public sealed class TimeExternalIdMapGuidByGuidRangeQueryHandler(ITimeRepository repo) : IQueryHandler<TimeExternalIdMapGuidByGuidRangeQuery, Dictionary<Guid, short>>
{
      public async Task<Dictionary<Guid, short>> HandleAsync(TimeExternalIdMapGuidByGuidRangeQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdMapGuidByGuidRangeAsync(query.guids,ct);
      }
}