using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
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
  IComponentMappingRepository com,
  ILocationRepository loc,
  IAdapterFactory adapter
  ) : IDoor
{
  public async Task<Guid> CreateAsync(CreateDoorDto dto, CancellationToken ct = default)
  {

    // Actually it need to check that 
    //var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));
    var deviceId = await device.GetIdByGuidAsync(dto.DeviceGuid);
    var dev = await device.GetAsync(dto.DeviceGuid, ct);
    var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid);

    foreach (var reader in dto.Readers)
    {
      if(!await deviceModule.IsAnyGuidAsync(reader.DeviceModuleGuid, ct))
        throw new NotFoundException(EntityType.DeviceModule.ToString(), reader.DeviceModuleGuid.ToString());
    }

    var readerMapModuleId = await deviceModule.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Readers.Select(x => x.DeviceModuleGuid), ct);

    //var readerMapModuleId = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Readers.Select(x => x.DeviceModuleGuid)));

    var sensorMapModuleId = dto.Sensor == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Sensor.DeviceModuleGuid);

    var relayMapModuleId = dto.Relay == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Relay.DeviceModuleGuid);
    var buzzerMapModuleId = dto.Buzzer == null ? 0 : await deviceModule.GetDeviceModuleIdByGuidAsync(dto.Buzzer.DeviceModuleGuid);
    var rexMapModuleId = await deviceModule.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Rexes.Select(x => x.DeviceModuleGuid), ct);
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
        dto.Sensor.Metadata,
        dto.Sensor.Vendor,
        sensorMapModuleId
        ),
        dto.Relay == null ? null : new Relay(
          dto.Relay.SlotNo,
          dto.Relay.Metadata,
          dto.Relay.Vendor,
          relayMapModuleId
        ),
        dto.Buzzer == null ? null : new Buzzer(
          dto.Buzzer.SlotNo,
          dto.Buzzer.Metadata,
          dto.Buzzer.Vendor,
          buzzerMapModuleId
        ),
        dto.Rexes.Select(x => new Rex(
        x.SlotNo,
        x.Metadata,
        x.Vendor,
        readerMapModuleId[x.DeviceModuleGuid]
      )).ToList(),
        dto.Bg == null ? null : new BreakGlass(
          dto.Bg.SlotNo,
          dto.Bg.Vendor,
          dto.Bg.Metadata,
          bgMapModuleId
        ),
        locationId
    );


    // Get Component ExternalId
    var deviceExternalId = await com.GetExternalIdByMacAndEntityAsync(dev.Mac, EntityType.Device);
    var doorExternalIds = new List<short>();
    
    var doorId1 = await com.GetFreeIdByMacAndEntityAndVendorAsync(dev.Mac,EntityType.Door,dto.Vendor,100);
    doorExternalIds.Add((short)doorId1);

    if(dto.Type == DoorType.Dual)
    {
      var doorId2 = await com.GetFreeIdByMacAndEntityAndVendorAsync(dev.Mac,EntityType.Door,dto.Vendor,100,[doorId1],ct);
      doorExternalIds.Add((short)doorId2);
    }


    // Send command to controller
    var readers = new List<(short SlotNo, ReaderMode ReaderMode, ReaderDirection ReaderDirection, string Metadata, short ExternalId)>();

    foreach (var x in dto.Readers)
    {
        var externalId = (short)await com.GetExternalIdByGuidAndEntityAsync(x.DeviceModuleGuid, EntityType.DeviceModule, ct);
        
        readers.Add((
            (short)x.SlotNo,
            x.Mode,
            x.ReaderDirection,
            x.Metadata,
            externalId
        ));
    }

    var rexes = new List<(short SlotNo, string Metadata, short ExternalId,short TzExternalId)>();

    foreach (var x in dto.Rexes)
    {
        var externalId = (short)await com.GetExternalIdByGuidAndEntityAsync(x.DeviceModuleGuid, EntityType.DeviceModule, ct);
        var tzExternalId = x.MaskGuid == null ? (short)0 : (short)await com.GetExternalIdByGuidAndEntityAsync(x.MaskGuid ?? Guid.Empty,EntityType.TimeZone,ct);
        
        rexes.Add((
            (short)x.SlotNo,
            x.Metadata,
            externalId,
            tzExternalId
        ));
    }

    await adapter.GetAdapter(d.Vendor).Door.DoorsAsync(
      dev.Mac,
      dev.Ip,
      d.Type,
      (short)deviceExternalId,
      doorExternalIds,
      d.Metadata,
      readers,
      d.Buzzer == null || dto.Buzzer == null ? 
      null : 
      (
        (short)d.Buzzer.SlotNo,
        d.Buzzer.Metadata,
        (short)await com.GetExternalIdByGuidAndEntityAsync(dto.Buzzer.DeviceModuleGuid,EntityType.DeviceModule,ct),
        (short)await com.GetFreeIdByMacAndEntityAndVendorAsync(dev.Mac,EntityType.Output,Vendor.aero,100,[],ct)
      ),
      rexes,
      d.BG == null || dto.Bg == null ? 
      null : 
      (
        (short)d.BG.SlotNo,
        d.BG.Metadata,
        (short)await com.GetExternalIdByGuidAndEntityAsync(dto.Bg.DeviceModuleGuid,EntityType.DeviceModule,ct),
        (short)await com.GetFreeIdByMacAndEntityAndVendorAsync(dev.Mac,EntityType.Input,Vendor.aero,100,[],ct)
      ),
      d.Sensor == null || dto.Sensor == null ? 
      null : 
      (
        (short)d.Sensor.SlotNo,
        d.Sensor.Metadata,
        (short)await com.GetExternalIdByGuidAndEntityAsync(dto.Sensor.DeviceModuleGuid,EntityType.DeviceModule,ct)
      ),
      d.Relay == null || dto.Relay == null ? 
      null :
      (
        (short)d.Relay.SlotNo,
        d.Relay.Metadata,
        (short)await com.GetExternalIdByGuidAndEntityAsync(dto.Relay.DeviceModuleGuid,EntityType.DeviceModule,ct)
      ),
      ct
    );

    await com.InsertAsync(
      new ComponentMappping(
        d.Guid,
        EntityType.Door,
        doorExternalIds.Count == 0 ? -1 : doorExternalIds.ElementAt(0),
        string.Empty,
        d.LocationId,
        d.Vendor
      ),
      ct
    );

    if(d.Type == DoorType.Dual)
    {
      await com.InsertAsync(
      new ComponentMappping(
        d.Guid,
        EntityType.Door,
        doorExternalIds.Count <= 1 ? -1 : doorExternalIds.ElementAt(1),
        string.Empty,
        d.LocationId,
        d.Vendor
      ),
      ct
    );
    }

    await repo.AddAsync(d, ct);

    return d.Guid;
  }

  public async Task<Guid> CreateTemplateAsync(CreateTemplateDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
    
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

      public async Task<bool> GetStatusAsync(Guid guid, CancellationToken ct = default)
      {
          var door = await repo.GetAsync(guid,ct);

          var deviceExternalId = (short)await com.GetExternalIdByGuidAndEntityAsync(door.DeviceGuid,EntityType.Device,ct);

          var doorExternalId = (short)await com.GetExternalIdByGuidAndEntityAsync(guid,EntityType.Door);

          var dev = await device.GetAsync(door.DeviceGuid,ct);

          await adapter.GetAdapter(door.Vendor).Door.StatusAsync(
            dev.Mac,
            dev.Ip,
            deviceExternalId,
            doorExternalId
            );

            return true;
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
    var rexMapModuleId = await deviceModule.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Rexes.Select(x => x.DeviceModuleGuid), ct);
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
        dto.Sensor.Metadata,
        dto.Sensor.Vendor,
        sensorMapModuleId
        ),
        dto.Relay == null ? null : new Relay(
          dto.Relay.SlotNo,
          dto.Relay.Metadata,
          dto.Relay.Vendor,
          relayMapModuleId
        ),
        dto.Buzzer == null ? null : new Buzzer(
          dto.Buzzer.SlotNo,
          dto.Buzzer.Metadata,
          dto.Buzzer.Vendor,
          buzzerMapModuleId
        ),
       dto.Rexes.Select(x => new Rex(
        x.SlotNo,
        x.Metadata,
        x.Vendor,
        readerMapModuleId[x.DeviceModuleGuid]
      )).ToList(),
         dto.Bg == null ? null : new BreakGlass(
          dto.Bg.SlotNo,
          dto.Bg.Vendor,
          dto.Bg.Metadata,
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