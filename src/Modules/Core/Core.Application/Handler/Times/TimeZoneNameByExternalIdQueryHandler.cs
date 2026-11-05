using Core.Application.Interfaces;
using Core.Contract.Queries.Time;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Times;

public sealed class TimeZoneNameByExternalIdQueryHandler(ITimeRepository repo) : IQueryHandler<TimeZoneNameByExternalIdQuery, string>
{
      public async Task<string> HandleAsync(TimeZoneNameByExternalIdQuery query, CancellationToken ct)
      {
            return await repo.GetNameByExternalIdAsync(query.externalId,ct);
      }
}