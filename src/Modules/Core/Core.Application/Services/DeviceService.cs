using Adapter.Abstraction.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.Interfaces;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class DeviceService(
  IDeviceRepository repo,
  IAdapterFactory adapter
  ) : IDevice
{
  public async Task<DeviceDto> CreateAsync(CreateDeviceDto dto, CancellationToken ct = default)
  {
    var d = new Core.Domain.Entities.Device(
      dto.Name,
      dto.SerialNumber,
      dto.Mac,
      dto.Ip,
      dto.Port,
      dto.Firmware,
      dto.Vendor,
      dto.Metadata,
      dto.LocationGuid
    );

    // Check name is duplicate
    if (await repo.IsAnyByNameAndLocationGuidAsync(d.Name, dto.LocationGuid, ct))
      throw new DuplicateException(EntityType.Device, d.Name);

    // Send Command to device
    await adapter.GetAdapter(dto.Vendor).Device.CreateDeviceAsync();

    await repo.AddAsync(d, ct);

    return new DeviceDto(
      d.Guid,
      d.Name,
      d.SerialNumber,
      d.Mac,
      d.Ip,
      d.Port,
      d.Firmware,
      d.Vendor,
      d.Metadata,
      DateTime.UtcNow,
      DeviceStatus.PENDING,
      d.LocationGuid,
      true,
      false
    );
  }

  public async Task<Guid> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<DeviceDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<DeviceDto> UpdateAsync(UpdateDeviceDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}