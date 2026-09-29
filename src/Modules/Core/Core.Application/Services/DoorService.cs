using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class DoorService(
  IDoorRepository repo, 
  IMessageBus bus,
  IDeviceRepository device,
  IDeviceModuleRepository deviceModule,
  ILocationRepository loc
  ) : IDoor
{
  public async Task<Guid> CreateAsync(CreateDoorDto dto, CancellationToken ct = default)
  {

    //var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));
    var deviceId = await device.GetIdByGuidAsync(dto.DeviceGuid);
    var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid);

    foreach (var reader in dto.Readers)
    {
      if (!await repo.IsAnyGuidAsync(reader.DeviceModuleGuid, ct))
        throw new NotFoundException(EntityType.DeviceModule.ToString(), reader.DeviceModuleGuid.ToString());
    }

    var readerMapModuleId = await deviceModule.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Readers.Select(x => x.DeviceModuleGuid), ct);

    //var readerMapModuleId = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Readers.Select(x => x.DeviceModuleGuid)));

    var sensorMapModuleId = dto.Sensor == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Sensor.DeviceModuleGuid);

    var relayMapModuleId = dto.Relay == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Relay.DeviceModuleGuid);
    var buzzerMapModuleId = dto.Buzzer == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Buzzer.DeviceModuleGuid);
    var rexMapModuleId = dto.Rex == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Rex.DeviceModuleGuid);
    var bgMapModuleId = dto.Bg == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Bg.DeviceModuleGuid);

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(nameof(dto.Name), dto.Name);

    var d = new Door(
      dto.Name,
      dto.Vendor,
      dto.Type,
      deviceId,
      dto.Metadata,
      dto.Readers.Select(x => new Reader(
        x.SlotNo,
        x.Mode,
        x.Metadata,
        x.Vendor,
        x.ReaderDirection,
        readerMapModuleId[x.DeviceModuleGuid]
      )).ToList(),
      dto.Sensor == null ? null : new Sensor(
        dto.Sensor.SlotNo,
        dto.Sensor.Mode,
        dto.Sensor.Metadata,
        dto.Sensor.Vendor,
        sensorMapModuleId
        ),
        dto.Relay == null ? null : new Relay(
          dto.Relay.SlotNo,
          dto.Relay.Mode,
          dto.Relay.Metadata,
          dto.Relay.Vendor,
          relayMapModuleId
        ),
        dto.Buzzer == null ? null : new Buzzer(
          dto.Buzzer.SlotNo,
          dto.Buzzer.Mode,
          dto.Buzzer.Metadata,
          dto.Buzzer.Vendor,
          buzzerMapModuleId
        ),
        dto.Rex == null ? null : new Rex(
          dto.Rex.SlotNo,
          dto.Rex.Mode,
          dto.Rex.Metadata,
          dto.Rex.Vendor,
          rexMapModuleId
        ),
        dto.Bg == null ? null : new BreakGlass(
          dto.Bg.SlotNo,
          dto.Bg.Vendor,
          bgMapModuleId
        ),
        locationId
    );


    // Send command to controller 

    await repo.AddAsync(d, ct);

    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Door.ToString(), guid.ToString());

    // Check relation here 
    if (await repo.IsAnyRelatedEntitiesAsync(guid))
      throw new FoundRelateException();

    await repo.DeleteAsync(guid);

    return true;

  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.Door.ToString());

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Door.ToString(), guid.ToString());

      // Check relate object here
      if (await repo.IsAnyRelatedEntitiesAsync(guid))
        throw new FoundRelateException();
    }

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Door.ToString(), guid.ToString());

    return await repo.DisableAsync(guid, ct);
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Door.ToString(), guid.ToString());

    return await repo.EnableAsync(guid, ct);
  }

  public async Task<DoorDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<IEnumerable<DoorDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetByLocationAsync(guid, ct);
  }

  public async Task<Pagination<DoorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateDoorDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.Door.ToString(), dto.Guid.ToString());

    var deviceId = await device.GetIdByGuidAsync(dto.DeviceGuid);

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    //var readerMapModuleId = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Readers.Select(x => x.DeviceModuleGuid)));

     foreach (var reader in dto.Readers)
    {
      if (!await repo.IsAnyGuidAsync(reader.DeviceModuleGuid, ct))
        throw new NotFoundException(EntityType.DeviceModule.ToString(), reader.DeviceModuleGuid.ToString());
    }

    var readerMapModuleId = await deviceModule.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Readers.Select(x => x.DeviceModuleGuid), ct);

    var sensorMapModuleId = dto.Sensor == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Sensor.DeviceModuleGuid);

    var relayMapModuleId = dto.Relay == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Relay.DeviceModuleGuid);
    var buzzerMapModuleId = dto.Buzzer == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Buzzer.DeviceModuleGuid);
    var rexMapModuleId = dto.Rex == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Rex.DeviceModuleGuid);
    var bgMapModuleId = dto.Bg == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Bg.DeviceModuleGuid);

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(nameof(dto.Name), dto.Name);

    var d = new Door(
      dto.Name,
      dto.Vendor,
      dto.Type,
      deviceId,
      dto.Metadata,
      dto.Readers.Select(x => new Reader(
        x.SlotNo,
        x.Mode,
        x.Metadata,
        x.Vendor,
        x.ReaderDirection,
        readerMapModuleId[x.DeviceModuleGuid]
      )).ToList(),
      dto.Sensor == null ? null : new Sensor(
        dto.Sensor.SlotNo,
        dto.Sensor.Mode,
        dto.Sensor.Metadata,
        dto.Sensor.Vendor,
        sensorMapModuleId
        ),
        dto.Relay == null ? null : new Relay(
          dto.Relay.SlotNo,
          dto.Relay.Mode,
          dto.Relay.Metadata,
          dto.Relay.Vendor,
          relayMapModuleId
        ),
        dto.Buzzer == null ? null : new Buzzer(
          dto.Buzzer.SlotNo,
          dto.Buzzer.Mode,
          dto.Buzzer.Metadata,
          dto.Buzzer.Vendor,
          buzzerMapModuleId
        ),
        dto.Rex == null ? null : new Rex(
          dto.Rex.SlotNo,
          dto.Rex.Mode,
          dto.Rex.Metadata,
          dto.Rex.Vendor,
          rexMapModuleId
        ),
         dto.Bg == null ? null : new BreakGlass(
          dto.Bg.SlotNo,
          dto.Bg.Vendor,
          bgMapModuleId
        ),
        locationId
    );

    await repo.UpdateAsync(d);

    return d.Guid;
  }

      public Task UploadAsync(CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}