using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class IntervalRepository(CoreDbContext context) : IIntervalRepository
{
  public async Task AddAsync(Interval entity, CancellationToken ct = default)
  {
    await context.Intervals.AddAsync(
      new Persistences.Entities.Interval(entity)
      , ct
    );

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Intervals
      .Where(x => x.guid == guid)
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Interval, guid.ToString());

    context.Intervals.Remove(entity);

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Intervals
      .Where(x => guids.Contains(x.guid))
      .ToArrayAsync();

    context.Intervals.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Intervals
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync(x => x.guid == guid) ?? throw new NotFoundException(EntityType.TimeZone, guid.ToString());

    entity.is_active = false;

    context.Intervals.Update(entity);
    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Intervals
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync(x => x.guid == guid) ?? throw new NotFoundException(EntityType.TimeZone, guid.ToString());

    entity.is_active = true;

    context.Intervals.Update(entity);
    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<IntervalDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Intervals
      .AsNoTracking()
      .Where(x => x.guid == guid)
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
      ).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Interval, guid.ToString());
  }

  public async Task<IEnumerable<IntervalDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.Intervals
      .AsNoTracking()
      .Where(x => x.location_id == locationId)
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
      ).ToArrayAsync();
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Intervals
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => x.id)
      .FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    return await context.Intervals
      .AsNoTracking()
      .Where(x => guids.Contains(x.guid))
      .Select(x => x.id)
      .ToArrayAsync();
  }

  public async Task<Pagination<IntervalDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Intervals
                  .AsNoTracking()
                  .Where(x => x.location.guid == param.locationGuid)
                  .AsQueryable();

    if (!string.IsNullOrWhiteSpace(param.search))
    {
      if (!string.IsNullOrWhiteSpace(param.search))
      {
        var search = param.search.Trim();

        if (context.Database.IsNpgsql())
        {
          var pattern = $"%{search}%";

          query = query.Where(x =>
              EF.Functions.ILike(x.start_time.ToString(), pattern) ||
              EF.Functions.ILike(x.end_time.ToString(), pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.start_time.ToString().Contains(search) ||
              x.end_time.ToString().Contains(search)
          );
        }

      }
    }


    if (param.startDate != null)
    {
      var startUtc = DateTime.SpecifyKind(param.startDate.Value, DateTimeKind.Utc);
      query = query.Where(x => x.created_at >= startUtc);
    }

    if (param.endDate != null)
    {
      var endUtc = DateTime.SpecifyKind(param.endDate.Value, DateTimeKind.Utc);
      query = query.Where(x => x.created_at <= endUtc);
    }

    var count = await query.CountAsync(ct);

    var res = await query
          .AsNoTracking()
          .OrderByDescending(e => e.created_at)
          .Skip((param.pageNumber - 1) * param.pageSize)
          .Take(param.pageSize)
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
      ).ToListAsync(ct);

    return new Pagination<IntervalDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Intervals
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid);
  }

  public async Task<bool> IsAnySameDataSetAsync(TimeOnly start, TimeOnly end, DayInWeek day, CancellationToken ct = default)
  {
    return await context.Intervals
      .AsNoTracking()
      .AnyAsync(
        x =>
          x.start_time == start &&
          x.end_time == end &&
          x.day.sunday == day.Sunday &&
          x.day.monday == day.Monday &&
          x.day.tuesday == day.Tuesday &&
          x.day.wednesday == day.Wednesday &&
          x.day.thursday == day.Thursday &&
          x.day.friday == day.Friday &&
          x.day.saturday == day.Saturday
      );
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Intervals
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.is_default);
  }

  public async Task UpdateAsync(Interval entity, CancellationToken ct = default)
  {
    var en = await context.Intervals
      .Include(x => x.day)
      .Where(x => x.guid == entity.Guid)
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Interval, entity.Guid.ToString());

    if (en.day is not null)
    {
      en.day.sunday = entity.Days.Sunday;
      en.day.monday = entity.Days.Monday;
      en.day.tuesday = entity.Days.Tuesday;
      en.day.wednesday = entity.Days.Wednesday;
      en.day.thursday = entity.Days.Thursday;
      en.day.friday = entity.Days.Friday;
      en.day.saturday = entity.Days.Saturday;
    }
    en.start_time = entity.StartTime;
    en.end_time = entity.EndTime;
    en.location_id = entity.LocationId;

    context.Intervals.Update(en);

    await context.SaveChangesAsync(ct);
  }
}