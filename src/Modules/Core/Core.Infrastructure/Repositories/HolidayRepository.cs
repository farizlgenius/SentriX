using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class HolidayRepository(CoreDbContext context) : IHolidayRepository
{
  public async Task AddAsync(Holiday entity, CancellationToken ct = default)
  {
    await context.Holidays.AddAsync(
      new Persistences.Entities.Holiday(entity)
      , ct
    );

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Holidays
      .Where(x => x.guid == guid)
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Holiday, guid.ToString());

    context.Holidays.Remove(entity);

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Holidays
      .Where(x => guids.Contains(x.guid))
      .ToArrayAsync();

    context.Holidays.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Holidays
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync(x => x.guid == guid) ?? throw new NotFoundException(EntityType.TimeZone, guid.ToString());

    entity.is_active = false;

    context.Holidays.Update(entity);
    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Holidays
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync(x => x.guid == guid) ?? throw new NotFoundException(EntityType.TimeZone, guid.ToString());

    entity.is_active = true;

    context.Holidays.Update(entity);
    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<HolidayDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Holidays
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(
        x => new HolidayDto(
          x.guid,
          x.name,
          x.start,
          x.end,
          x.is_active,
          x.is_default,
          x.location.guid
        )
      ).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Holiday, guid.ToString());
  }

  public async Task<IEnumerable<HolidayDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.Holidays
      .AsNoTracking()
      .Where(x => x.location_id == locationId)
      .Select(
        x => new HolidayDto(
          x.guid,
          x.name,
          x.start,
          x.end,
          x.is_active,
          x.is_default,
          x.location.guid
        )
      ).ToArrayAsync();
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Holidays
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => x.id)
      .FirstOrDefaultAsync();
  }

  public async Task<Pagination<HolidayDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Holidays
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
          .Select(
        x => new HolidayDto(
          x.guid,
          x.name,
          x.start,
          x.end,
          x.is_active,
          x.is_default,
          x.location.guid
        )
      ).ToListAsync();

    return new Pagination<HolidayDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    return await context.Holidays
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name) && x.location_id == locationId);
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Holidays
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid);
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Holidays
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.is_default);
  }

  public async Task UpdateAsync(Holiday entity, CancellationToken ct = default)
  {
    var en = await context.Holidays
      .Where(x => x.guid == entity.Guid)
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Holiday, entity.Guid.ToString());

    en.name = entity.Name;
    en.start = entity.Start;
    en.end = entity.End;
    en.location_id = entity.LocationId;

    context.Holidays.Update(en);

    await context.SaveChangesAsync(ct);
  }
}