using Core.Application.Interfaces;
using Core.Contract.Queries.Time;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Times;

public sealed class TimeExternalIdByGuidQueryHandler(ITimeRepository repo) : IQueryHandler<TimeExternalIdByGuidQuery, short>
{
      public async Task<short> HandleAsync(TimeExternalIdByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdByGuidAsync(query.guid,ct);
      }
}