using SharedKernel.Messaging;
using Time.Application.Interfaces;
using Time.Contract.Queries;

namespace Time.Application.Handler;

public sealed class TimeZoneCountByLocationIdQueryHandler(ITimeZoneRepository repo) : IQueryHandler<TimeZoneCountByLocationIdQuery, int>
{
      public async Task<int> HandleAsync(TimeZoneCountByLocationIdQuery query, CancellationToken ct)
      {
            return await repo.CountTimeZoneByLocationIdAsync(query.LocationId,ct);
      }
}