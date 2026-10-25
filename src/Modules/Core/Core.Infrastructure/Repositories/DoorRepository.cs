using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DoorRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Door, Domain.Entities.Door, DoorDto>(
  context,
  d => new Persistences.Entities.Door(d),
  (en, dom) =>
  {
    en.name = dom.Name;
    en.vendor = dom.Vendor;
    en.type = dom.Type;
    en.metadata = dom.Metadata;
    en.sensor = dom.Sensor == null ? null : new Persistences.Entities.Sensor(dom.Sensor);
    en.relay = dom.Relay == null ? null : new Persistences.Entities.Relay(dom.Relay);
    en.buzzer = dom.Buzzer == null ? null : new Persistences.Entities.Buzzer(dom.Buzzer);

    // 1 : N Handler

    context.Readers.RemoveRange(en.readers);
    en.readers.Clear();

    foreach (var reader in dom.Readers)
    {
      en.readers.Add(
        new Persistences.Entities.Reader(reader)
      );
    }

    context.Rexes.RemoveRange(en.rexes);
    en.rexes.Clear();

    foreach (var rex in dom.Rexes)
    {
      en.rexes.Add(
        new Persistences.Entities.Rex(rex)
      );
    }
  },
  DoorProjections.ToDto,
  DoorProjections.ToCommand
), IDoorRepository
{

  public async Task<Guid> GetGuidByExternalIdAndDeviceExternalIdAndVendorAsync(short externalId, short deviceExternalId, Vendor vendor, CancellationToken ct = default)
      {
           return await Context.Doors
            .AsNoTracking()
            .Where(x => x.external_id == externalId && x.device.external_id == externalId && x.device.vendor == vendor)
            .Select(x => x.guid)
            .FirstOrDefaultAsync(ct);
      }


  public async Task<bool> CheckRelationAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

      public async Task<string> GetNameByExternalIdAndMacAndVendorQuery(short externalId, string mac, Vendor vendor,CancellationToken ct= default)
      {
           return await Context.Doors
            .AsNoTracking()
            .Where(x => x.external_id == externalId && x.device.mac == mac && x.vendor == vendor)
            .Select(x => x.name)
            .FirstOrDefaultAsync() ?? string.Empty;
      }

      public async Task<IEnumerable<DoorDto>> GetByMacAsync(string mac, CancellationToken ct = default)
  {
    return await Context.Doors
      .AsNoTracking()
      .Where(x => x.device.mac == mac)
      .Select(x => new DoorDto(
  x.guid,
  x.name,
  x.vendor,
  x.type,
   x.device.guid,
  x.device.name,
  x.metadata,
  x.readers.Select(
    r => new ReaderDto(
      r.slot_no,
      r.mode,
      r.metadata,
      r.vendor,
      r.reader_direction,
      r.device_module.guid
    )
  ).ToList(),
  x.buzzer == null ? null : new BuzzerDto(
    x.buzzer.slot_no,
    x.buzzer.metadata,
    x.buzzer.vendor,
    x.buzzer.device_module.guid
  ),
  x.rexes.Select(x => new RexDto(
    x.slot_no,
    x.metadata,
    x.vendor,
    x.device_module.guid,
     x.timezone == null ? default : x.timezone.guid
  )).ToList(),
  x.sensor == null ? null : new SensorDto(
    x.sensor.slot_no,
    x.sensor.metadata,
    x.sensor.vendor,
    x.sensor.device_module.guid
  ),
  x.relay == null ? null : new RelayDto(
    x.relay.slot_no,
    x.relay.metadata,
    x.relay.vendor,
    x.relay.device_module.guid
  ),
   x.bg == null ? null : new BGDto(
    x.bg.slot_no,
    x.metadata,
    x.bg.vendor,
    x.bg.device_module.guid
  ),
  x.location.guid,
  x.location.name,
  x.is_active,
  x.is_default
)).ToArrayAsync(ct);
  }

  public async Task<IEnumerable<(Guid guid, int id, string mac, string ip, Vendor vendor,short external_id)>> GetDetailsByGuidAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var res = await Context.Doors
      .AsNoTracking()
      .Where(x => guids.Contains(x.guid))
      .Select(x => new
      {
        x.guid,
        x.id,
        x.device.mac,
        x.device.ip,
        x.device.vendor,
        x.device.external_id
      }).ToArrayAsync(cancellationToken: ct);

    return [.. res.Select(x => (x.guid, x.id, x.mac, x.ip, x.vendor,x.external_id ?? -1))];
  }

  public async Task<Dictionary<Guid, int>> GetDoorIdsMapGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    return await Context.Doors
      .AsNoTracking()
      .Where(x => guids.Contains(x.guid))
      .ToDictionaryAsync(x => x.guid, x => x.id, ct);
  }

      public async Task<IEnumerable<short>> GetExternalIdRangeByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var result = await Context.Doors
                .AsNoTracking()
                .Where(x => x.guid == guid)
                .Select(x => new { x.external_id, x.altr_external_id })
                .FirstOrDefaultAsync();

            return result is not null 
                    ? new[] { result.external_id!.Value, result.altr_external_id!.Value } 
                    : Array.Empty<short>(); // Replace string with your column's data type
      }

      public async Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default)
      {
            var existingIds = await Context.Doors
                  .AsNoTracking()
                  .Where(x => x.device.mac == mac)
                  .Select(x => x.external_id!.Value)
                  .ToArrayAsync(ct);

            var existIdsHash = existingIds.ToHashSet();

            if (exception != null)
            {
                  existIdsHash.UnionWith(exception);
            }

            for (short id = 1; id < max; id++)
            {
                  if (!existIdsHash.Contains(id))
                        return id;
            }

            throw new ExceedException(typeof(Persistences.Entities.Door).Name);
      }

      public async Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    return await Context.Doors
     .AsNoTracking()
     .Where(x => guids.Contains(x.guid))
     .Select(x => x.id)
     .ToArrayAsync(cancellationToken: ct);
  }


  public async Task<Pagination<DoorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await base.GetPaginationAsync(
      param,
      param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name],
                  ct
    );
  }




  public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    return false;
  }
}
