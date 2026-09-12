using Core.Application.Interfaces;
using Core.Contract.DTOs.DeviceModule;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class DeviceModuleService(IDeviceModuleRepository repo, IMessageBus bus) : IDeviceModule
{
  public async Task<Guid> CreateAsync(CreateDeviceModuleDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
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
    throw new NotImplementedException();
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<DeviceModuleDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<DeviceModuleDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(Guid locationGuid, Vendor vendor, CancellationToken ct = default)
  {

    ValidationHelper.Vendor(vendor);

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(locationGuid));

    return await repo.GetByVendorAndLocationAsync(locationId, vendor, ct);

  }

  public async Task<Pagination<DeviceModuleDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Guid> UpdateAsync(UpdateDeviceModuleDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}