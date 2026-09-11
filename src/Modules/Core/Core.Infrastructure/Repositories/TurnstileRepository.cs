using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Contract.DTOs.Turnstile;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class TurnstileRepository(CoreDbContext context) : ITurnstileRepository
{
  public async Task AddAsync(Turnstile entity, CancellationToken ct = default)
  {
    await context.Turnstiles.AddAsync(
      new Persistences.Entities.Turnstile(entity)
      , ct);

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Turnstiles
      .Where(x => x.guid == guid)
        .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Turnstile, guid.ToString());

    context.Turnstiles.Remove(entity);

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Turnstiles
      .Where(x => guids.Contains(x.guid))
      .ToArrayAsync();

    context.Turnstiles.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Turnstiles
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Turnstile, guid.ToString());

    en.is_active = false;

    context.Turnstiles.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Turnstiles
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Turnstile, guid.ToString());

    en.is_active = true;

    context.Turnstiles.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<TurnstileDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Turnstiles
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => new TurnstileDto(
        x.guid,
        x.name,
        x.lanes.Select(l => new LaneDto(
          l.guid,
          l.lane_no,
          l.readers.Select(x => new ReaderDto(
            x.guid,
            x.slot_no,
            x.mode,
            x.metadata,
            x.vendor,
            x.reader_direction,
            x.device_module.guid
            )).ToList(),
            l.sensor == null ? null :
            new SensorDto(
              l.sensor.guid,
              l.sensor.slot_no,
              l.sensor.mode,
              l.sensor.metadata,
              l.sensor.vendor,
              l.sensor.device_module.guid
            ),
            x.location.guid,
            x.is_active,
            x.is_default
        )).ToList(),
        x.location.guid,
        x.location.name,
        x.is_active,
        x.is_default
      ))
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Turnstile, guid.ToString());
  }

  public async Task<IEnumerable<TurnstileDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.Turnstiles
      .AsNoTracking()
      .Where(x => x.location_id == locationId)
      .Select(x => new TurnstileDto(
        x.guid,
        x.name,
        x.lanes.Select(l => new LaneDto(
          l.guid,
          l.lane_no,
          l.readers.Select(x => new ReaderDto(
            x.guid,
            x.slot_no,
            x.mode,
            x.metadata,
            x.vendor,
            x.reader_direction,
            x.device_module.guid
            )).ToList(),
            l.sensor == null ? null :
            new SensorDto(
              l.sensor.guid,
              l.sensor.slot_no,
              l.sensor.mode,
              l.sensor.metadata,
              l.sensor.vendor,
              l.sensor.device_module.guid
            ),
            x.location.guid,
            x.is_active,
            x.is_default
        )).ToList(),
        x.location.guid,
        x.location.name,
        x.is_active,
        x.is_default
      ))
      .ToArrayAsync();
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    var res = await context.Turnstiles
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => x.id)
      .FirstOrDefaultAsync();

    if (res == 0)
      throw new NotFoundException(EntityType.Turnstile, guid.ToString());

    return res;
  }

  public async Task<Pagination<TurnstileDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Turnstiles
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
         .Select(x => new TurnstileDto(
        x.guid,
        x.name,
        x.lanes.Select(l => new LaneDto(
          l.guid,
          l.lane_no,
          l.readers.Select(x => new ReaderDto(
            x.guid,
            x.slot_no,
            x.mode,
            x.metadata,
            x.vendor,
            x.reader_direction,
            x.device_module.guid
            )).ToList(),
            l.sensor == null ? null :
            new SensorDto(
              l.sensor.guid,
              l.sensor.slot_no,
              l.sensor.mode,
              l.sensor.metadata,
              l.sensor.vendor,
              l.sensor.device_module.guid
            ),
            x.location.guid,
            x.is_active,
            x.is_default
        )).ToList(),
        x.location.guid,
        x.location.name,
        x.is_active,
        x.is_default
      )).ToListAsync();

    return new Pagination<TurnstileDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    return await context.Turnstiles
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name) && x.location_id == locationId);
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Turnstiles
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid);
  }

  public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Turnstiles
      .AsNoTracking()
      .AnyAsync(x => x.is_default);
  }

  public async Task UpdateAsync(Turnstile entity, CancellationToken ct = default)
  {
    var en = await context.Turnstiles
      .Where(x => x.guid == entity.Guid)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Turnstile, entity.Guid.ToString());

    en.name = entity.Name;
    context.Lanes.RemoveRange(en.lanes);
    en.lanes.Clear();

    en.lanes = entity.Lanes.Select(x => new Persistences.Entities.Lane(x)).ToList();

    context.Turnstiles.Update(en);

    await context.SaveChangesAsync(ct);
  }
}