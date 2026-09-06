using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class TimeRepository(CoreDbContext context) : ITimeRepository
{
  public async Task AddAsync(Domain.Entities.TimeZone entity, CancellationToken ct = default)
  {
    await context.TimeZones.AddAsync(
      new Persistences.Entities.TimeZone(entity)
      ,
      ct
    );

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.TimeZones
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync(x => x.guid == guid) ?? throw new NotFoundException(EntityType.TimeZone, guid.ToString());

    context.TimeZones.Remove(entity);

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.TimeZones
      .Where(x => guids.Contains(x.guid))
      .ToArrayAsync();

    context.TimeZones.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.TimeZones
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync(x => x.guid == guid) ?? throw new NotFoundException(EntityType.TimeZone, guid.ToString());

    entity.is_active = false;

    context.TimeZones.Update(entity);
    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.TimeZones
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync(x => x.guid == guid) ?? throw new NotFoundException(EntityType.TimeZone, guid.ToString());

    entity.is_active = true;

    context.TimeZones.Update(entity);
    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<TimeZoneDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.TimeZones
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => new TimeZoneDto(
        x.guid,
        x.name,
        x.timezone_intervals.Select(
          x => new IntervalDto(
            x.interval.guid,
            x.interval.day == null ?
            new DayInWeekDto() :
            new DayInWeekDto(
              x.interval.day.sunday,
              x.interval.day.monday,
              x.interval.day.tuesday,
              x.interval.day.wednesday,
              x.interval.day.thursday,
              x.interval.day.friday,
              x.interval.day.saturday
            ),
            x.interval.start_time,
            x.interval.end_time,
            x.interval.is_active,
            x.interval.is_default
          )
        ).ToList(),
        x.location.guid,
        x.is_active,
        x.is_default
      )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.TimeZone, guid.ToString());
  }

  public async Task<IEnumerable<TimeZoneDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.TimeZones
      .AsNoTracking()
      .Where(x => x.location_id == locationId)
      .Select(x => new TimeZoneDto(
        x.guid,
        x.name,
        x.timezone_intervals.Select(
          x => new IntervalDto(
            x.interval.guid,
            x.interval.day == null ?
            new DayInWeekDto() :
            new DayInWeekDto(
              x.interval.day.sunday,
              x.interval.day.monday,
              x.interval.day.tuesday,
              x.interval.day.wednesday,
              x.interval.day.thursday,
              x.interval.day.friday,
              x.interval.day.saturday
            ),
            x.interval.start_time,
            x.interval.end_time,
            x.interval.is_active,
            x.interval.is_default
          )
        ).ToList(),
        x.location.guid,
        x.is_active,
        x.is_default
      )).ToArrayAsync();
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.TimeZones
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => x.id)
      .FirstOrDefaultAsync();
  }

  public async Task<Pagination<TimeZoneDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.TimeZones
                  .Where(x => x.location.guid == param.locationGuid)
                  .AsNoTracking()
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
              EF.Functions.ILike(x.name, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.name.Contains(search)
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

    var count = await query.CountAsync();

    var res = await query
          .AsNoTracking()
          .OrderByDescending(e => e.created_at)
          .Skip((param.pageNumber - 1) * param.pageSize)
          .Take(param.pageSize)
          .Select(x => new TimeZoneDto(
        x.guid,
        x.name,
        x.timezone_intervals.Select(
          x => new IntervalDto(
            x.interval.guid,
            x.interval.day == null ?
            new DayInWeekDto() :
            new DayInWeekDto(
              x.interval.day.sunday,
              x.interval.day.monday,
              x.interval.day.tuesday,
              x.interval.day.wednesday,
              x.interval.day.thursday,
              x.interval.day.friday,
              x.interval.day.saturday
            ),
            x.interval.start_time,
            x.interval.end_time,
            x.interval.is_active,
            x.interval.is_default
          )
        ).ToList(),
        x.location.guid,
        x.is_active,
        x.is_default
      )).ToListAsync();

    return new Pagination<TimeZoneDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    return await context.TimeZones
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name) && x.location_id == locationId);
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.TimeZones
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid);
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.TimeZones
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.is_default);
  }

  public async Task UpdateAsync(Domain.Entities.TimeZone entity, CancellationToken ct = default)
  {

    try
    {
      await context.Database.BeginTransactionAsync(ct);

      var en = await context.TimeZones
      .Where(x => x.guid == entity.Guid)
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.TimeZone, entity.Guid.ToString());

      var existingInterval = await context.TimeZoneIntervals.Where(x => x.timezone_id == en.id).ToArrayAsync();

      var removes = new List<Persistences.Entities.TimeZoneInterval>();

      var existingIds = new List<int>();

      foreach (var exiting in existingInterval)
      {
        if (!entity.IntervalIds.Contains(exiting.id))
        {
          removes.Add(exiting);
          continue;
        }

        existingIds.Add(exiting.id);

      }

      context.TimeZoneIntervals.RemoveRange(removes);

      var newInterval = entity.IntervalIds
      .Where(x => !existingIds.Contains(x))
      .Select(x => new Persistences.Entities.TimeZoneInterval(0, x))
      .ToList();

      await context.TimeZoneIntervals.AddRangeAsync(newInterval, ct);

      en.name = entity.Name;
      en.location_id = entity.LocationId;

      await context.SaveChangesAsync(ct);

      await context.Database.CommitTransactionAsync(ct);

    }
    catch
    {
      await context.Database.RollbackTransactionAsync(ct);
      throw;
    }

  }
}