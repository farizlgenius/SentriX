using Core.Contract.DTOs.Device;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IDeviceRepository : IBaseRepository<DeviceDto, Device>
{
      Task<Guid> GetGuidByMacAsync(string mac, CancellationToken ct = default);
      Task<bool> IsAnyMacAsync(string mac, CancellationToken ct = default);
      Task<int> GetDeviceModuleIdByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<(string, int)> GetNameAndLocationIdByMacAsync(string mac, CancellationToken ct = default);
      Task<DeviceDto> GetByMacAsync(string mac,CancellationToken ct = default);
      Task UpdateConfigurationStatusByMacAsync(string mac,bool isSync,CancellationToken ct = default);
     
}