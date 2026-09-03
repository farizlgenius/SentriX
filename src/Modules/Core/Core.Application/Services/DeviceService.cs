using Adapter.Abstraction.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
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



    var d = new Core.Domain.Entities.Device(
      dto.Name,
      dto.SerialNumber,
      dto.Mac,
      dto.Ip,
      dto.Port,
      dto.Firmware,
      dto.Vendor,
      dto.Metadata,
      locationId
    );


    // Send Command to device
    if (dto.Vendor.Equals(Vendor.AMICO))
    {
      await repo.AddAsync(d, ct);
    }
    else if (dto.Vendor.Equals(Vendor.AERO))
    {
      // var id = await com.get
      // await adapter.GetAdapter(dto.Vendor).Device.InititalDeviceAsync(,d.Ip,d.Mac);
      // Aero get from mac and set guid
      var guid = await repo.GetGuidByMacAsync(dto.Mac);
      d.SetGuid(guid);
      await repo.UpdateAsync(d, ct);
    }


    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    if(!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Device, guid.ToString());

    await repo.DisableAsync(guid, ct);
    return true;
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    if(!await repo.IsAnyGuidAsync(guid, ct))
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
    throw new NotImplementedException();
  }
}