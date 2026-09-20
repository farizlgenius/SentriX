using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DoorRepository(CoreDbContext context) : IDoorRepository
{
  public async Task AddAsync(Door entity, CancellationToken ct = default)
  {
    await context.Doors.AddAsync(
      new Persistences.Entities.Door(entity)
      , ct);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> CheckRelationAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Doors
    .OrderByDescending(x => x.id)
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Door, guid.ToString());

    // Check releation

    context.Doors.Remove(entity);

    await context.SaveChangesAsync(ct);

  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Doors
      .Where(x => guids.Contains(x.guid))
      .ToArrayAsync();

    context.Doors.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Doors
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Door, guid.ToString());

    en.is_active = false;

    context.Doors.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Doors
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Door, guid.ToString());

    en.is_active = true;

    context.Doors.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<DoorDto> GetAsync(Guid guid, CancellationToken ct = default)
  {

    return await context.Doors
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => new DoorDto(
        x.guid,
        x.name,
        x.vendor,
        x.type,
        x.metadata,
        x.readers.Select(
          r => new ReaderDto(
            r.guid,
            r.slot_no,
            r.mode,
            r.metadata,
            r.vendor,
            r.reader_direction,
            r.device_module.guid
          )
        ).ToList(),
        x.buzzer == null ? null : new BuzzerDto(
          x.buzzer.guid,
          x.buzzer.slot_no,
          x.buzzer.mode,
          x.buzzer.metadata,
          x.buzzer.vendor,
          x.buzzer.device_module.guid
        ),
        x.rex == null ? null : new RexDto(
          x.rex.guid,
          x.rex.slot_no,
          x.rex.mode,
          x.rex.metadata,
          x.rex.vendor,
          x.rex.device_module.guid
        ),
        x.sensor == null ? null : new SensorDto(
          x.sensor.guid,
          x.sensor.slot_no,
          x.sensor.mode,
          x.sensor.metadata,
          x.sensor.vendor,
          x.sensor.device_module.guid
        ),
        x.relay == null ? null : new RelayDto(
          x.relay.guid,
          x.relay.slot_no,
          x.relay.mode,
          x.relay.metadata,
          x.relay.vendor,
          x.relay.device_module.guid
        ),
        x.location.guid,
        x.location.name,
        x.is_active,
        x.is_default
      )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Door, guid.ToString());
  }

  public async Task<IEnumerable<DoorDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.Doors
      .AsNoTracking()
      .Where(x => x.location_id == locationId)
      .Select(x => new DoorDto(
        x.guid,
        x.name,
        x.vendor,
        x.type,
        x.metadata,
        x.readers.Select(
          r => new ReaderDto(
            r.guid,
            r.slot_no,
            r.mode,
            r.metadata,
            r.vendor,
            r.reader_direction,
            r.device_module.guid
          )
        ).ToList(),
        x.buzzer == null ? null : new BuzzerDto(
          x.buzzer.guid,
          x.buzzer.slot_no,
          x.buzzer.mode,
          x.buzzer.metadata,
          x.buzzer.vendor,
          x.buzzer.device_module.guid
        ),
        x.rex == null ? null : new RexDto(
          x.rex.guid,
          x.rex.slot_no,
          x.rex.mode,
          x.rex.metadata,
          x.rex.vendor,
          x.rex.device_module.guid
        ),
        x.sensor == null ? null : new SensorDto(
          x.sensor.guid,
          x.sensor.slot_no,
          x.sensor.mode,
          x.sensor.metadata,
          x.sensor.vendor,
          x.sensor.device_module.guid
        ),
        x.relay == null ? null : new RelayDto(
          x.relay.guid,
          x.relay.slot_no,
          x.relay.mode,
          x.relay.metadata,
          x.relay.vendor,
          x.relay.device_module.guid
        ),
        x.location.guid,
        x.location.name,
        x.is_active,
        x.is_default
      )).ToArrayAsync(ct);
  }

  public async Task<Dictionary<Guid, int>> GetDoorIdsMapGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    return await context.Doors
      .AsNoTracking()
      .Where(x => guids.Contains(x.guid))
      .ToDictionaryAsync(x => x.guid, x => x.id, ct);
  }

      public Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    var res = await context.Doors
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => x.id)
      .FirstOrDefaultAsync();

    if (res == 0)
      throw new NotFoundException(EntityType.Door, guid.ToString());

    return res;
  }

  public async Task<Pagination<DoorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Doors
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
         .Select(x => new DoorDto(
                      x.guid,
                      x.name,
                      x.vendor,
        x.type,
                      x.metadata,
                      x.readers.Select(
                        r => new ReaderDto(
                          r.guid,
                          r.slot_no,
                          r.mode,
                          r.metadata,
                          r.vendor,
                          r.reader_direction,
                          r.device_module.guid
                        )
                      ).ToList(),
                      x.buzzer == null ? null : new BuzzerDto(
                        x.buzzer.guid,
                        x.buzzer.slot_no,
                        x.buzzer.mode,
                        x.buzzer.metadata,
                        x.buzzer.vendor,
                        x.buzzer.device_module.guid
                      ),
                      x.rex == null ? null : new RexDto(
                        x.rex.guid,
                        x.rex.slot_no,
                        x.rex.mode,
                        x.rex.metadata,
                        x.rex.vendor,
                        x.rex.device_module.guid
                      ),
                      x.sensor == null ? null : new SensorDto(
                        x.sensor.guid,
                        x.sensor.slot_no,
                        x.sensor.mode,
                        x.sensor.metadata,
                        x.sensor.vendor,
                        x.sensor.device_module.guid
                      ),
                      x.relay == null ? null : new RelayDto(
                        x.relay.guid,
                        x.relay.slot_no,
                        x.relay.mode,
                        x.relay.metadata,
                        x.relay.vendor,
                        x.relay.device_module.guid
                      ),
                      x.location.guid,
                      x.location.name,
                      x.is_active,
                      x.is_default
                    )).ToListAsync();

    return new Pagination<DoorDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    return await context.Doors
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name) && x.location_id == locationId);

  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Doors
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid);
  }

  public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Doors
      .AsNoTracking()
      .AnyAsync(x => x.is_default);
  }

  public async Task UpdateAsync(Door entity, CancellationToken ct = default)
  {
    var en = await context.Doors
      .AsNoTracking()
      .Where(x => x.guid == entity.Guid)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Door, entity.Guid.ToString());

    en.name = entity.Name;
    en.vendor = entity.Vendor;
    en.type = entity.Type;
    en.metadata = entity.Metadta;
    en.sensor = entity.Sensor == null ? null : new Persistences.Entities.Sensor(entity.Sensor);
    en.relay = entity.Relay == null ? null : new Persistences.Entities.Relay(entity.Relay);
    en.buzzer = entity.Buzzer == null ? null : new Persistences.Entities.Buzzer(entity.Buzzer);
    en.rex = entity.Rex == null ? null : new Persistences.Entities.Rex(entity.Rex);

    // 1 : N Handler

    context.Readers.RemoveRange(en.readers);
    en.readers.Clear();

    foreach (var reader in entity.Readers)
    {
      en.readers.Add(
        new Persistences.Entities.Reader(reader)
      );
    }


    context.Doors.Update(en);

    await context.SaveChangesAsync(ct);
  }
}