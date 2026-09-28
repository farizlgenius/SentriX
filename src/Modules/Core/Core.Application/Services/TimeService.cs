using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Contract.Queries.Time;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class TimeService(
  ITimeRepository repo,
  IAdapterFactory adapter,
  IDevice device,
  IComponentMapping com,
  IMessageBus bus) : ITime
{
  public async Task<Guid> CreateAsync(CreateTimeZoneDto dto, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(EntityType.TimeZone, dto.Name);

    var intervalIds = await bus.QueryAsync(new IntervalIdsByGuidsQuery(dto.IntervalGuids));

    var intervalDtos = await bus.QueryAsync(new IntervalByGuidsQuery(dto.IntervalGuids));

    var d = new Domain.Entities.TimeZone(
      dto.Name,
      intervalIds.ToList(),
      locationId
    );

    // Send command to all controller and device
    var devices = await device.GetByLocationAsync(dto.LocationGuid);
    foreach (var device in devices)
    {
      var externalId = await com.GetExternalIdByMacAndEntityAsync(device.Mac, EntityType.Device, ct);
      var tzExternalId = await com.GetFreeIdByEntityAsync(EntityType.TimeZone, 100, null, ct);
      await adapter.GetAdapter(device.Vendor).Time.TimeZone(
        device.Mac,
        device.Ip,
        (short)externalId,
        (short)tzExternalId,
        intervalDtos.ToList(),
        ct);
    }

    await repo.AddAsync(d, ct);

    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.TimeZone, guid.ToString());

    // Check relation

    // Send command

    await repo.DeleteAsync(guid);

    return true;
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.Role);

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Role, guid.ToString());

      // Check relate object here

    }

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Role, guid.ToString());

    return await repo.DisableAsync(guid, ct);
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Role, guid.ToString());

    return await repo.EnableAsync(guid, ct);
  }

  public async Task<TimeZoneDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<IEnumerable<TimeZoneDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetByLocationAsync(guid, ct);
  }

  public async Task<Pagination<TimeZoneDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateTimeZoneDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.TimeZone, dto.Guid.ToString());

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(EntityType.TimeZone, dto.Name);

    var intervalIds = await bus.QueryAsync(new IntervalIdsByGuidsQuery(dto.IntervalGuids));

    var d = new Domain.Entities.TimeZone(
      dto.Guid,
      dto.Name,
      intervalIds.ToList(),
      locationId
    );

    // Send command to controller

    await repo.UpdateAsync(d, ct);

    return d.Guid;
  }

  public async Task UploadAsync(Guid guid,CancellationToken ct = default)
  {
    var d = await device.GetByGuidAsync(guid,ct);
    var tzs = await repo.GetByLocationAsync(d.LocationGuid);

    var deviceExternalId = await com.GetExternalIdByMacAndEntityAsync(d.Mac,EntityType.Device,ct);

    foreach(var tz in tzs)
    {
      var tzExternalId = await com.GetExternalIdByGuidAndEntityAsync(tz.Guid,EntityType.TimeZone);

      await adapter.GetAdapter(d.Vendor).Time.TimeZone(
        d.Mac,
        d.Ip,
        (short)deviceExternalId,
        (short)tzExternalId,
        tz.Intervals,
        ct
      );
    }
  }
}