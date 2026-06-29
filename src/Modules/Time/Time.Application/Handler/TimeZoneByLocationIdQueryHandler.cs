using SharedKernel.Messaging;
using Time.Application.Interfaces;
using Time.Contract.DTOs;
using Time.Contract.Queries;

namespace Time.Application.Handler;

public sealed class TimeZoneByLocationIdQueryHandler(ITimezoneRepository repo) : IQueryHandler<TimeZoneByLocationIdQuery, IEnumerable<TimezoneDto>>
{
      public async Task<IEnumerable<TimezoneDto>> HandleAsync(TimeZoneByLocationIdQuery query, CancellationToken ct)
      {
            return await repo.GetTimeZoneByLocationIdAsync(query.LocationId);
      }
}