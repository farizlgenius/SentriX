using Core.Application.Interfaces;
using Core.Contract.DTOs.Turnstile;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class TurnstileService(
  IMessageBus bus,
  ITurnstileRepository repo
  ) : ITurnstile
{
  public async Task<Guid> CreateAsync(CreateTurnstileDto dto, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(nameof(dto.Name), dto.Name);

    var readerDeviceModuleIdMap = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Lanes.SelectMany(x => x.Readers.Select(r => r.DeviceModuleGuid))));
    var sensorDeviceModuleIdMap = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Lanes.Where(x => x.Sensor != null).Select(x => x.Sensor.DeviceModuleGuid)));

    var d = new Turnstile(
      dto.Name,
      dto.Lanes.Select(x => new Lane(
        x.LaneNo,
        x.Readers.Select(r => new Reader(
          r.SlotNo,
          r.Mode,
          r.Metadata,
          r.Vendor,
          r.ReaderDirection,
          readerDeviceModuleIdMap[r.DeviceModuleGuid]
        )).ToList(),
        x.Sensor == null ? null : new Sensor(
          x.Sensor.SlotNo,
          x.Sensor.Mode,
          x.Sensor.Metadata,
          x.Sensor.Vendor,
          sensorDeviceModuleIdMap[x.Sensor.DeviceModuleGuid]
          )
      )).ToList(),
      locationId
    );

    await repo.AddAsync(d, ct);

    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Turnstile, guid.ToString());

    // Check Relation

    await repo.DeleteAsync(guid, ct);

    return true;
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Door, guid.ToString());

    return await repo.DisableAsync(guid, ct);
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Door, guid.ToString());

    return await repo.EnableAsync(guid, ct);
  }

  public async Task<TurnstileDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<IEnumerable<TurnstileDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(guid));
    return await repo.GetByLocationAsync(locationId, ct);
  }

  public async Task<Pagination<TurnstileDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateTurnstileDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.Turnstile, dto.Guid.ToString());

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(nameof(dto.Name), dto.Name);

    var readerDeviceModuleIdMap = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Lanes.SelectMany(x => x.Readers.Select(r => r.DeviceModuleGuid))));
    var sensorDeviceModuleIdMap = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Lanes.Where(x => x.Sensor != null).Select(x => x.Sensor.DeviceModuleGuid)));

    var d = new Turnstile(
      dto.Guid,
      dto.Name,
      dto.Lanes.Select(x => new Lane(
        x.Guid,
        x.LaneNo,
        x.Readers.Select(r => new Reader(
          r.Guid,
          r.SlotNo,
          r.Mode,
          r.Metadata,
          r.Vendor,
          r.ReaderDirection,
          readerDeviceModuleIdMap[r.DeviceModuleGuid]
        )).ToList(),
        x.Sensor == null ? null : new Sensor(
          x.Sensor.Guid,
          x.Sensor.SlotNo,
          x.Sensor.Mode,
          x.Sensor.Metadata,
          x.Sensor.Vendor,
          sensorDeviceModuleIdMap[x.Sensor.DeviceModuleGuid]
          )
      )).ToList(),
      locationId
    );

    await repo.UpdateAsync(d, ct);

    return d.Guid;

  }
}