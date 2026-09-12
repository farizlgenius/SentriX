using Core.Contract.DTOs.DeviceModule;
using Core.Domain.Entities;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IDeviceModuleRepository : IBaseRepository<DeviceModuleDto, DeviceModule>
{
      Task<int> GetDeviceModuleIdByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<Dictionary<Guid, int>> GetDeviceModuleIdsMapGuidsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
      Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(int locationId, Vendor vedor, CancellationToken ct = default);
}