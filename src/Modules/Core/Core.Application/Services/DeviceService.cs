using Adapter.Abstraction.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class DeviceService(
  IDeviceRepository repo,
  IAdapterFactory adapter,
  IMessageBus bus,
  IComponentMappingRepository com
  ) : IDevice
{
  public async Task<Guid> CreateAsync(CreateDeviceDto dto, CancellationToken ct = default)
  {

    if (!await bus.QueryAsync(new IsAnyLocationByGuidQuery(dto.LocationGuid), ct))
      throw new NotFoundException(EntityType.Location, dto.LocationGuid.ToString());

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid), ct);

    // Check name is duplicate
    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId, ct))
      throw new DuplicateException(EntityType.Device, dto.Name);


    var deviceModules = dto.DeviceModules.Select(x => new DeviceModule(
        x.Name,
        x.SerialNumber,
        x.Firmware,
        x.Mac,
        x.Address,
        x.Port,
        x.Model,
        locationId
        )).ToList();


    // Add Internal

    deviceModules.Add(
       new DeviceModule(
        "Internal",
        dto.SerialNumber,
        dto.Firmware,
        dto.Mac,
        0,
        dto.Port,
        dto.Vendor == SharedKernel.Enums.Vendor.aero ? SharedKernel.Enums.DeviceModuleModel.x1100 : SharedKernel.Enums.DeviceModuleModel.amico,
        locationId
        )
    );



    var d = new Core.Domain.Entities.Device(
      dto.Name,
      dto.SerialNumber,
      dto.Mac,
      dto.Ip,
      dto.Port,
      dto.Firmware,
      dto.Vendor,
      dto.Metadata,
      locationId,
      deviceModules
    );




    // Send Command to device below 


    // Handle how device is create on each device
    switch (dto.Vendor)
    {
      case SharedKernel.Enums.Vendor.amico:
        await repo.AddAsync(d, ct);
        break;
      case SharedKernel.Enums.Vendor.aero:
        if (!await repo.IsAnyMacAsync(dto.Mac))
        {
          await repo.AddAsync(d, ct);
        }
        else
        {
          var guid = await repo.GetGuidByMacAsync(dto.Mac);
          d.SetGuid(guid);
          await repo.UpdateAsync(d, ct);
        }
        break;
      default:
        throw new BadRequestException("Vendor Type Invalid.");

    }


    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Device, guid.ToString());

    // Check reference before 

    await repo.DeleteAsync(guid, ct);

    return true;
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.Company);

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Company, guid.ToString());

      // Check relate object here

    }

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Device, guid.ToString());

    await repo.DisableAsync(guid, ct);
    return true;
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Device, guid.ToString());

    await repo.EnableAsync(guid, ct);
    return true;
  }

  public async Task<DeviceDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }


  public async Task<IEnumerable<DeviceDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(guid), ct);
    return await repo.GetByLocationAsync(locationId, ct);
  }

  public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateDeviceDto dto, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.Device, dto.Guid.ToString());

    if (!await repo.IsAnyMacAsync(dto.Mac, ct))
      throw new DuplicateException(EntityType.Device, dto.Mac);

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    // Check name is duplicate
    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId, ct))
      throw new DuplicateException(EntityType.Device, dto.Name);

    var d = new Device(
      dto.Guid,
      dto.Name,
      dto.SerialNumber,
      dto.Mac,
      dto.Ip,
      dto.Port,
      dto.Firmware,
      dto.Vendor,
      dto.Metadata,
      locationId,
      dto.DeviceModules.Select(x => new DeviceModule(
        x.Name,
        x.SerialNumber,
        x.Firmware,
        x.Mac,
        x.Address,
        x.Port,
        x.Model,
        locationId
        )).ToList()
    );

    await repo.UpdateAsync(d, ct);

    return d.Guid;

  }
}