using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class DeviceService(
  IDeviceRepository repo,
  IComponentMapping comm,
  IMessageBus bus,
  ITempDevice temp,
  IAdapterFactory adapter,
  // Below is service that call for get count
  IDeviceModuleRepository module,
  IDoorRepository door,
  IOutputRepository output,
  ITimeRepository time,
  IGroupRepository group,
  IHolidayRepository hol
  ) : IDevice
{
  public async Task<Guid> CreateAsync(CreateDeviceDto dto, CancellationToken ct = default)
  {

    if (!await bus.QueryAsync(new IsAnyLocationByGuidQuery(dto.LocationGuid), ct))
      throw new NotFoundException(EntityType.Location.ToString(), dto.LocationGuid.ToString());

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid), ct);

    // Check name is duplicate
    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId, ct))
      throw new DuplicateException(EntityType.Device.ToString(), dto.Name);


    var deviceModules = dto.DeviceModules.Select(x => new DeviceModule(
        x.Name,
        x.SerialNumber,
        x.Firmware,
        x.Mac,
        x.Address,
        x.Port,
        x.Model,
        AeroModuleModelHelper.nReaderByModel(x.Model),
        AeroModuleModelHelper.nOutputByModel(x.Model),
        AeroModuleModelHelper.nInputByModel(x.Model),
        locationId
        )).ToList();


    // Add Internal

    var model = dto.Vendor == SharedKernel.Enums.Vendor.aero ? SharedKernel.Enums.DeviceModuleModel.x1100 : SharedKernel.Enums.DeviceModuleModel.amico;

    deviceModules.Add(
       new DeviceModule(
        $"{dto.Name} - Internal",
        dto.SerialNumber,
        dto.Firmware,
        dto.Mac,
        0,
        dto.Port,
        model,
        AeroModuleModelHelper.nReaderByModel(model),
        AeroModuleModelHelper.nOutputByModel(model),
        AeroModuleModelHelper.nInputByModel(model),
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
      SharedKernel.Enums.DeviceConfigurationStatus.pending,
      locationId,
      deviceModules
    );

    // Handle how device is create on each device
    switch (dto.Vendor)
    {
      case SharedKernel.Enums.Vendor.amico:
        await repo.AddAsync(d, ct);
        break;
      case SharedKernel.Enums.Vendor.aero:
        await repo.AddAsync(d, ct);
        break;
      default:
        throw new BadRequestException("Vendor Type Invalid.");

    }

    // // Send Command to device below 
    // var exceptionId = temp.TryGetUnavailableId(dto.Mac);

    // var externalId = await comm.GetFreeIdByEntityAndVendorAsync(
    //   EntityType.Device.ToString(),
    //   Vendor.aero,
    //   100, // Limit by License
    //   exceptionId, ct);


    if (d.Vendor == Vendor.aero)
    {

      if (temp.TryGet(d.Mac, out var tempD))
      {
        //await adapter.GetAdapter(d.Vendor).Device.SetExternalIdAsync(d.Mac, d.Ip, tempD.Id, (int)externalId);
        await comm.InsertComponentMappingAsync(
          d.Guid,
          EntityType.Device,
          (short)tempD.Id,
          d.Mac,
          d.Vendor,
          locationId,
          ct
        );

        //await adapter.GetAdapter(d.Vendor).Device.SetExternalIdAsync(d.Mac, d.Ip, tempD.Id, (int)externalId);
        await comm.InsertComponentMappingAsync(
          d.Guid,
          EntityType.DeviceModule,
          0,
          d.Mac,
          d.Vendor,
          locationId,
          ct
        );
      }
      else
      {
        throw new NotFoundException(EntityType.TempDevice.ToString(), d.Mac);
      }

    }



    await adapter.GetAdapter(d.Vendor).Device.InititalDeviceAsync(d.Mac, d.Ip, ct);

    temp.TryRemove(d.Mac);


    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Device.ToString(), guid.ToString());

    // Check reference before

    // Send Comand

    var device = await repo.GetAsync(guid, ct);

    await adapter.GetAdapter(device.Vendor).Device.RemoveDeviceAsync(device.Mac, device.Ip, ct);

    await repo.DeleteAsync(guid, ct);

    await comm.DeleteComponentMappingAsync(guid, ct);

    return true;
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.Company.ToString());

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Company.ToString(), guid.ToString());

      // Check relate object here

    }

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Device.ToString(), guid.ToString());

    await repo.DisableAsync(guid, ct);
    return true;
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Device.ToString(), guid.ToString());

    await repo.EnableAsync(guid, ct);
    return true;
  }

  public async Task<DeviceDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }


  public async Task<IEnumerable<DeviceDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetByLocationAsync(guid, ct);
  }

  public async Task<DeviceDto> GetByMacAsync(string mac, CancellationToken ct = default)
  {
    return await repo.GetByMacAsync(mac, ct);
  }

      public async Task<DeviceComponentDto> GetComponentAsync(Guid guid, CancellationToken ct = default)
      {
        var com = new List<Components>();

        var d = await repo.GetAsync(guid,ct);
          // Module

          // Door

          // Input

          // Output

          // Monitor Group

          // Area

          // TimeZone
          com.Add(
            await time.GetComponentsAsync(d.LocationGuid,d.SyncedAt,ct)
          );

          // AccessGroup

          // Holiday

          // Trigger

          // Procedure

          return new DeviceComponentDto(
            com.All(x => x.isSynced),
            com
          );
      }

      public async Task<object> GetConfigurationAsync(Guid guid, CancellationToken ct = default)
  {
    var device = await repo.GetAsync(guid, ct);
    return adapter.GetAdapter(device.Vendor).Device.GetConfigurationAsync(device.Mac, device.Ip, ct);
  }

  public async Task<bool> GetEventStatusByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    var device = await repo.GetAsync(guid, ct);
    await adapter.GetAdapter(device.Vendor).Device.GetTransactionStatusAsync(device.Mac, device.Ip, ct);
    return true;
  }

      public async Task<IEnumerable<OptionDto>> GetOptionByVendorAndLocationAsync(Vendor vendor, Guid guid, CancellationToken ct = default)
      {
           return await repo.GetOptionByVendorAndLocationAsync(vendor,guid,ct);
      }

      public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<IEnumerable<TempDeviceDto>> GetScanDeviceAsync(CancellationToken ct = default)
  {
    return temp.GetAll().ToArray();
  }

  public async Task<StatusDto> GetStatusAsync(Guid guid, CancellationToken ct = default)
  {
    var device = await repo.GetAsync(guid, ct);
    return new StatusDto(
      device.Guid,
      await adapter.GetAdapter(device.Vendor).Device.GetStatusAsync(device.Mac, device.Ip)
    );
  }

  public async Task<IEnumerable<StatusDto>> GetStatusesAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> ResetAsync(Guid guid, CancellationToken ct = default)
  {
    var device = await repo.GetAsync(guid, ct);
    await adapter.GetAdapter(device.Vendor).Device.ResetAsync(device.Mac, device.Ip, ct);
    return true;
  }

  public async Task<Guid> UpdateAsync(UpdateDeviceDto dto, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.Device.ToString(), dto.Guid.ToString());

    if (!await repo.IsAnyMacAsync(dto.Mac, ct))
      throw new DuplicateException(EntityType.Device.ToString(), dto.Mac);

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    // Check name is duplicate
    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId, ct))
      throw new DuplicateException(EntityType.Device.ToString(), dto.Name);

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
      SharedKernel.Enums.DeviceConfigurationStatus.pending,
      locationId,
      dto.DeviceModules.Select(x => new DeviceModule(
        x.Name,
        x.SerialNumber,
        x.Firmware,
        x.Mac,
        x.Address,
        x.Port,
        x.Model,
         AeroModuleModelHelper.nReaderByModel(x.Model),
        AeroModuleModelHelper.nOutputByModel(x.Model),
        AeroModuleModelHelper.nInputByModel(x.Model),
        locationId
        )).ToList()
    );

    await repo.UpdateAsync(d, ct);

    return d.Guid;

  }

  public async Task UploadAsync(Guid guid, CancellationToken ct = default)
  {
    var d = await repo.GetAsync(guid, ct);

    await adapter.GetAdapter(d.Vendor).Device.UploadAsync(d.Mac, d.Ip, ct);
  }
}