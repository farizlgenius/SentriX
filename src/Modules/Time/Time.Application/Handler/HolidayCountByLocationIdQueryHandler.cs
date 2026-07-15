

using SharedKernel.Messaging;
using Time.Application.Interfaces;
using Time.Contract.Queries;

namespace Time.Application.Handler;

public sealed class HolidayCountByLocationIdQueryHandler(IHolidayRepository repo) : IQueryHandler<HolidayCountByLocationIdQuery, int>
{
      public async Task<int> HandleAsync(HolidayCountByLocationIdQuery query, CancellationToken ct)
      {
            return await repo.CountHolidayByLocationIdAsync(query.LocationId);
      }
}