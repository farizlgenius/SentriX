using SharedKernel.Messaging;
using Time.Application.Interfaces;
using Time.Contract.DTOs;
using Time.Contract.Queries;

namespace Time.Application.Handler;

public sealed class TimeZoneByLocationIdQueryHandler(ITimeZoneRepository repo) : IQueryHandler<TimeZoneByLocationIdQuery, IEnumerable<TimeZoneDto>>
{
      public async Task<IEnumerable<TimeZoneDto>> HandleAsync(TimeZoneByLocationIdQuery query, CancellationToken ct)
      {
            return await repo.GetTimeZoneByLocationIdAsync(query.LocationId);
      }
}