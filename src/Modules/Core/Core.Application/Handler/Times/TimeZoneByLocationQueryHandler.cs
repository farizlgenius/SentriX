using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Contract.Queries.Time;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Times;

public sealed class TimeZoneByLocationQueryHandler(ITimeRepository repo) : IQueryHandler<TimeZoneByLocationQuery, IEnumerable<TimeZoneDto>>
{
      public async Task<IEnumerable<TimeZoneDto>> HandleAsync(TimeZoneByLocationQuery query, CancellationToken ct)
      {
            return await repo.GetByLocationAsync(query.guid);
      }
}