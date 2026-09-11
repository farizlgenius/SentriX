using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class DoorService(IDoorRepository repo, IMessageBus bus) : IDoor
{
  public async Task<Guid> CreateAsync(CreateDoorDto dto, CancellationToken ct = default)
  {

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));
    var deviceModuleId = await bus.QueryAsync(new DeviceModuleIdByGuidQuery(dto.DeviceModuleGuid));

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(nameof(dto.Name), dto.Name);

    var d = new Door(
      dto.Name,
      dto.Vendor,
      dto.Type,
      dto.Metadata,
      dto.Readers.Select(x => new Reader(
        x.SlotNo,
        x.Mode,
        x.Metadata,
        x.Vendor,
        x.ReaderDirection
      )).ToList(),
      dto.Sensor == null ? null : new Sensor(
        dto.Sensor.SlotNo,
        dto.Sensor.Mode,
        dto.Sensor.Metadata,
        dto.Sensor.Vendor
        ),
        dto.Relay == null ? null : new Relay(
          dto.Relay.SlotNo,
          dto.Relay.Mode,
          dto.Relay.Metadata,
          dto.Relay.Vendor
        ),
        dto.Buzzer == null ? null : new Buzzer(
          dto.Buzzer.SlotNo,
          dto.Buzzer.Mode,
          dto.Buzzer.Metadata,
          dto.Buzzer.Vendor
        ),
        dto.Rex == null ? null : new Rex(
          dto.Rex.SlotNo,
          dto.Rex.Mode,
          dto.Rex.Metadata,
          dto.Rex.Vendor
        ),
       deviceModuleId,
        locationId
    );


    // Send command to controller 

    await repo.AddAsync(d, ct);

    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Door, guid.ToString());

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
      throw new NotFoundException(EntityType.Door);

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Door, guid.ToString());

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

  public async Task<DoorDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<IEnumerable<DoorDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(guid));
    return await repo.GetByLocationAsync(locationId, ct);
  }

  public async Task<Pagination<DoorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateDoorDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.Door, dto.Guid.ToString());

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));
    var deviceModuleId = await bus.QueryAsync(new DeviceModuleIdByGuidQuery(dto.DeviceModuleGuid));

    var d = new Door(
      dto.Name,
      dto.Vendor,
      dto.Type,
      dto.Metadata,
      dto.Readers.Select(x => new Reader(
        x.Guid,
        x.SlotNo,
        x.Mode,
        x.Metadata,
        x.Vendor,
        x.ReaderDirection
      )).ToList(),
      dto.Sensor == null ? null : new Sensor(
        dto.Sensor.Guid,
        dto.Sensor.SlotNo,
        dto.Sensor.Mode,
        dto.Sensor.Metadata,
        dto.Sensor.Vendor
        ),
        dto.Relay == null ? null : new Relay(
          dto.Relay.Guid,
          dto.Relay.SlotNo,
          dto.Relay.Mode,
          dto.Relay.Metadata,
          dto.Relay.Vendor
        ),
        dto.Buzzer == null ? null : new Buzzer(
          dto.Buzzer.Guid,
          dto.Buzzer.SlotNo,
          dto.Buzzer.Mode,
          dto.Buzzer.Metadata,
          dto.Buzzer.Vendor
        ),
        dto.Rex == null ? null : new Rex(
          dto.Rex.Guid,
          dto.Rex.SlotNo,
          dto.Rex.Mode,
          dto.Rex.Metadata,
          dto.Rex.Vendor
        ),
       deviceModuleId,
        locationId
    );

    await repo.UpdateAsync(d);

    return d.Guid;
  }
}