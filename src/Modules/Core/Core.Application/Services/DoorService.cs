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
  IDeviceRepository device,
  IDeviceModuleRepository module,
  ITimeRepository time,
  IOutputRepository output,
  IInputRepository input,
  IAdapterFactory adapter
  ) : BaseService<DoorDto,Domain.Entities.Door>(repo), IDoor
{
  public async Task<bool> ChangeDoorModeAsync(ChangeDoorModeDto dto, CancellationToken ct = default)
  {
    
    var dev = await repo.GetCommandDetailAsync(dto.Guid, ct);

    await adapter.GetAdapter(dev.Vendor).Door.ChangeDoorModeAsync(
      dev.DeviceMac,
      dev.DeviceIp,
      dev.DeviceExternalId,
      dev.ComponentExternalId ?? -1,
      dto.Mode,
      ct
    );

    return true;
  }

  public async Task<Guid> CreateAsync(CreateDoorDto dto, CancellationToken ct = default)
  {
    Output o = null;
    Input i = null;

    // Actually it need to check that 
    var dev = await device.GetCommandDetailAsync(dto.DeviceGuid, ct);

    foreach (var reader in dto.Readers)
    {
      if (!await module.IsAnyGuidAsync(reader.DeviceModuleGuid, ct))
        throw new NotFoundException(EntityType.DEVICE_MODULE.ToString(), reader.DeviceModuleGuid.ToString());
    }

    var readerMapModuleId = await module.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Readers.Select(x => x.DeviceModuleGuid), ct);

    //var readerMapModuleId = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Readers.Select(x => x.DeviceModuleGuid)));

    var sensorMapModuleId = dto.Sensor == null ? 0 : await module.GetIdByGuidAsync(dto.Sensor.DeviceModuleGuid, ct);

    var relayMapModuleId = dto.Relay == null ? 0 : await module.GetIdByGuidAsync(dto.Relay.DeviceModuleGuid, ct);
    var buzzerMapModuleId = dto.Buzzer == null ? (-1,(short)-1) : await module.GetIdAndExternalIdByGuidAsync(dto.Buzzer.DeviceModuleGuid, ct);
    var rexMapModuleId = await module.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Rexes.Select(x => x.DeviceModuleGuid), ct);
    var bgMapModuleId = dto.Bg == null ? (-1,(short)-1) : await module.GetIdAndExternalIdByGuidAsync(dto.Bg.DeviceModuleGuid, ct);

    if (await repo.IsAnyByNameAndLocationGuidAsync(dto.Name, dto.LocationGuid, ct))
      throw new DuplicateException(nameof(dto.Name), dto.Name);

    // Get Component ExternalId
    var doorExternalIds = new List<short>();

    var doorId1 = await repo.GetFreeExeternalIdByMacAsync(dev.DeviceMac, 100, [], ct);
    doorExternalIds.Add(doorId1);

    if (dto.Type == DoorType.Dual)
    {
      var doorId2 = await repo.GetFreeExeternalIdByMacAsync(dev.DeviceMac, 100, [doorId1], ct);
      doorExternalIds.Add(doorId2);
    }

    short BuzzerExternalId = 0;
    short BgExternalId = 0;

    if (dto.Buzzer != null)
    {
      BuzzerExternalId = await output.GetFreeExeternalIdByMacAsync(dev.DeviceMac, 100, [], ct);
    }

    if (dto.Bg != null)
    {
      BgExternalId = await input.GetFreeExeternalIdByMacAsync(dev.DeviceMac, 100, [], ct);
    }

    var d = new Door(
      dto.Name,
      dto.Vendor,
      dto.Type,
      dev.DeviceId,
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
          buzzerMapModuleId.Item1,
          BuzzerExternalId

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
          bgMapModuleId.Item1,
          BgExternalId
        ),
        dev.LocationId ?? 1,
        doorExternalIds.Count > 0 ? doorExternalIds.ElementAt(0) : (short)-1,
        doorExternalIds.Count > 1 ? doorExternalIds.ElementAt(1) : (short)-1
    );

    if(d.Buzzer != null)
    {
      o = new Output(
      $"{dto.Name} - Buzzer",
      d.Buzzer.SlotNo,
      d.Buzzer.Metadata,
      d.Vendor,
      buzzerMapModuleId.Item1,
      d.LocationId,
      BuzzerExternalId
      );
      
    }

    if(d.BG != null)
    {
      i = new Input(
        $"{dto.Name} - Break Glass",
        d.BG.SlotNo,
        d.BG.Metadata,
        d.BG.Vendor,
        bgMapModuleId.Item1,
        d.LocationId,
        BgExternalId
      );
    }


  
    // Send command to controller
    var readers = new List<(short SlotNo, ReaderMode ReaderMode, ReaderDirection ReaderDirection, string Metadata, short ExternalId)>();

    foreach (var x in dto.Readers)
    {
      var externalId = await module.GetExternalIdByGuidAsync(x.DeviceModuleGuid, ct);

      readers.Add((
          (short)x.SlotNo,
          x.Mode,
          x.ReaderDirection,
          x.Metadata,
          externalId
      ));
    }

    var rexes = new List<(short SlotNo, string Metadata, short ExternalId, short TzExternalId)>();

    foreach (var x in dto.Rexes)
    {
      var externalId = await module.GetExternalIdByGuidAsync(x.DeviceModuleGuid, ct);
      var tzExternalId = x.MaskGuid == null ? (short)0 : await time.GetExternalIdByGuidAsync(x.MaskGuid ?? Guid.Empty, ct);

      rexes.Add((
          (short)x.SlotNo,
          x.Metadata,
          externalId,
          tzExternalId
      ));
    }

    if(d.Buzzer != null)
    {
      await adapter.GetAdapter(d.Vendor).Output.CreateControlPointAsync(
        dev.DeviceMac,
        dev.DeviceIp,
        dev.DeviceExternalId,
        buzzerMapModuleId.Item2,
        (short)d.Buzzer.SlotNo,
        d.Buzzer.ExternalId,
        d.Buzzer.Metadata,
        ct
      );
    }

    if(d.BG != null)
    {
      await adapter.GetAdapter(d.Vendor).Input.MonitorPoint(
        dev.DeviceMac,
        dev.DeviceIp,
        ct
      );
    }

  
    await adapter.GetAdapter(d.Vendor).Door.AddDoorsAsync(
      dev.DeviceMac,
      dev.DeviceIp,
      d.Type,
      dev.DeviceExternalId,
      doorExternalIds,
      d.Metadata,
      readers,
      d.Buzzer == null || dto.Buzzer == null ?
      null :
      (
        (short)d.Buzzer.SlotNo,
        d.Buzzer.Metadata,
        await module.GetExternalIdByGuidAsync(dto.Buzzer.DeviceModuleGuid, ct),
        BuzzerExternalId
      ),
      rexes,
      d.BG == null || dto.Bg == null ?
      null :
      (
        (short)d.BG.SlotNo,
        d.BG.Metadata,
        await module.GetExternalIdByGuidAsync(dto.Bg.DeviceModuleGuid, ct),
        BgExternalId
      ),
      d.Sensor == null || dto.Sensor == null ?
      null :
      (
        (short)d.Sensor.SlotNo,
        d.Sensor.Metadata,
        await module.GetExternalIdByGuidAsync(dto.Sensor.DeviceModuleGuid, ct)
      ),
      d.Relay == null || dto.Relay == null ?
      null :
      (
        (short)d.Relay.SlotNo,
        d.Relay.Metadata,
        await module.GetExternalIdByGuidAsync(dto.Relay.DeviceModuleGuid, ct)
      ),
      -1,
      1,
      AntipassBackMode.NONE,
      AntipassBackMode.NONE,
      ct
    );

    // Add Door
    await repo.AddAsync(d, ct);

    // Add Buzzer
    if(o != null)
    {
      await output.AddAsync(o,ct);
    }
    

    // Add Break 
    if(i != null)
    {
      await input.AddAsync(i,ct);
    }
    

    return d.Guid;
  }

  public async Task<Guid> CreateTemplateAsync(CreateTemplateDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();

  }



  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    var door = await repo.GetAsync(guid,ct);
    var dev = await repo.GetCommandDetailAsync(guid,ct);

    // Check relation here 
    if (await repo.IsAnyRelatedEntitiesAsync(guid,ct))
      throw new FoundRelateException();


    //Send Command 
    await adapter.GetAdapter(dev.Vendor).Door.DeleteDoorsAsync(
      dev.DeviceMac,
      dev.DeviceIp,
      door.Type,
      dev.DeviceExternalId,
      [dev.ComponentExternalId ?? -1,dev.AltrComponentExternalId ?? -1],
      door.Buzzer == null ? (short)-1 : await output.GetExternalIdByGuidAsync(door.Buzzer.Guid!.Value,ct),
      door.Bg == null ? (short)-1 : await input.GetExternalIdByGuidAsync(door.Bg.Guid!.Value,ct),
      ct);


    // Delete Door

    await repo.DeleteAsync(guid,ct);

    // Delete Output

    if(door.Buzzer != null)
      await output.DeleteAsync(door.Buzzer.Guid!.Value,ct);

    // Delete Input
    if(door.Bg != null)
      await input.DeleteAsync(door.Bg.Guid!.Value,ct);

    return true;

  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.DOOR.ToString());

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.DOOR.ToString(), guid.ToString());

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
      throw new NotFoundException(EntityType.DOOR.ToString(), guid.ToString());

    return await repo.DisableAsync(guid, ct);
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.DOOR.ToString(), guid.ToString());

    return await repo.EnableAsync(guid, ct);
  }

      public async Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetOptionByLocationAsync(guid,ct);
      }

      public async Task<bool> GetStatusAsync(Guid guid, CancellationToken ct = default)
  {
    var door = await repo.GetAsync(guid, ct);

    var dev = await repo.GetCommandDetailAsync(guid,ct);

    await adapter.GetAdapter(door.Vendor).Door.StatusAsync(
      dev.DeviceMac,
      dev.DeviceIp,
      dev.DeviceExternalId,
      dev.ComponentExternalId ?? -1
      );

    return true;
  }

  public async Task<bool> UnlockAsync(Guid guid, CancellationToken ct = default)
  {
    var door = await repo.GetAsync(guid, ct);
    var dev = await repo.GetCommandDetailAsync(guid,ct);

    await adapter.GetAdapter(door.Vendor).Door.UnlockAsync(
      dev.DeviceMac,
      dev.DeviceIp,
      dev.DeviceExternalId,
      dev.ComponentExternalId ?? -1,
      ct
    );

    return true;
  }

  public async Task<Guid> UpdateAsync(UpdateDoorDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.DOOR.ToString(), dto.Guid.ToString());


    var dev = await device.GetCommandDetailAsync(dto.Guid,ct);

    //var readerMapModuleId = await bus.QueryAsync(new DeviceModuleIdsMapGuidsByGuidsQuery(dto.Readers.Select(x => x.DeviceModuleGuid)));

    foreach (var reader in dto.Readers)
    {
      if (!await repo.IsAnyGuidAsync(reader.DeviceModuleGuid, ct))
        throw new NotFoundException(EntityType.DEVICE_MODULE.ToString(), reader.DeviceModuleGuid.ToString());
    }

    var readerMapModuleId = await module.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Readers.Select(x => x.DeviceModuleGuid), ct);

    var sensorMapModuleId = dto.Sensor == null ? 0 : await module.GetIdByGuidAsync(dto.Sensor.DeviceModuleGuid,ct);

    var relayMapModuleId = dto.Relay == null ? 0 : await module.GetIdByGuidAsync(dto.Relay.DeviceModuleGuid,ct);
    var buzzerMapModuleId = dto.Buzzer == null ? 0 : await module.GetIdByGuidAsync(dto.Buzzer.DeviceModuleGuid,ct);
    var rexMapModuleId = await module.GetDeviceModuleIdsMapGuidsByGuidsAsync(dto.Rexes.Select(x => x.DeviceModuleGuid), ct);
    var bgMapModuleId = dto.Bg == null ? 0 : await module.GetIdByGuidAsync(dto.Bg.DeviceModuleGuid,ct);

    
    short BuzzerExternalId = 0;
    short BgExternalId = 0;

    if (dto.Buzzer != null)
    {
      BuzzerExternalId = await output.GetFreeExeternalIdByMacAsync(dev.DeviceMac, 100, [], ct);
    }

    if (dto.Bg != null)
    {
      BgExternalId = await input.GetFreeExeternalIdByMacAsync(dev.DeviceMac, 100, [], ct);
    }

    var d = new Door(
      dto.Name,
      dto.Vendor,
      dto.Type,
      dev.DeviceId,
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
          buzzerMapModuleId,
          BuzzerExternalId
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
          bgMapModuleId,
          BgExternalId
        ),
        dev.LocationId ?? -1,
        dev.ComponentExternalId ?? -1,
        dev.AltrComponentExternalId ?? -1
    );

    await repo.UpdateAsync(d);

    return d.Guid;
  }

  public Task UploadAsync(CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}