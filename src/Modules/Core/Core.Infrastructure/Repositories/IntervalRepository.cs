using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class IntervalRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Interval,Domain.Entities.Interval,IntervalDto>(
  context,
  d => new Persistences.Entities.Interval(d),
  (en,dom) =>
  {
    if (en.day is not null)
    {
      en.day.sunday = dom.Days.Sunday;
      en.day.monday = dom.Days.Monday;
      en.day.tuesday = dom.Days.Tuesday;
      en.day.wednesday = dom.Days.Wednesday;
      en.day.thursday = dom.Days.Thursday;
      en.day.friday = dom.Days.Friday;
      en.day.saturday = dom.Days.Saturday;
    }
    en.start_time = dom.StartTime;
    en.end_time = dom.EndTime;
    en.location_id = dom.LocationId;
    en.updated_at = DateTime.UtcNow;
  },
  IntervalProjections.ToDto,
  IntervalProjections.ToCommand
),IIntervalRepository
{




 
  public async Task<IEnumerable<IntervalDto>> GetByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    return await Context.Intervals
     .AsNoTracking()
     .OrderByDescending(x => x.id)
     .Where(x => guids.Contains(x.guid))
     .Select(
 x => new IntervalDto(
   x.guid,
   x.day == null ?
     new DayInWeekDto() :
     new DayInWeekDto(
       x.day.sunday,
       x.day.monday,
       x.day.tuesday,
       x.day.wednesday,
       x.day.thursday,
       x.day.friday,
       x.day.saturday
     ),
   x.start_time,
   x.end_time,
   x.is_active,
   x.is_default
 )
).ToArrayAsync(cancellationToken: ct);
  }

 

  public async Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    return await Context.Intervals
      .AsNoTracking()
      .Where(x => guids.Contains(x.guid))
      .Select(x => x.id)
      .ToArrayAsync(cancellationToken: ct);
  }

    

      public async Task<Pagination<IntervalDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
     return await base.GetPaginationAsync(
      param,
      param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
      [x => x.start_time.ToString(),x => x.end_time.ToString()]
     );
  }





  public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnySameDataSetAsync(TimeOnly start, TimeOnly end, DayInWeek day, CancellationToken ct = default)
  {
    return await Context.Intervals
      .AsNoTracking()
      .AnyAsync(x =>
          x.start_time == start &&
          x.end_time == end &&
          x.day.sunday == day.Sunday &&
          x.day.monday == day.Monday &&
          x.day.tuesday == day.Tuesday &&
          x.day.wednesday == day.Wednesday &&
          x.day.thursday == day.Thursday &&
          x.day.friday == day.Friday &&
          x.day.saturday == day.Saturday
, cancellationToken: ct);
  }

 
}