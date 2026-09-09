using Core.Contract.DTOs.DeviceModule;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IDeviceModuleRepository : IBaseRepository<DeviceModuleDto, DeviceModule>
{
      Task<int> GetDeviceModuleIdByGuidAsync(Guid guid,CancellationToken ct = default);
}