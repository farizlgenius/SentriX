using Core.Contract.DTOs.Device;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IDeviceRepository : IRepository<DeviceDto, Device>
{
      Task<Guid> GetGuidByMacAsync(string mac, CancellationToken ct = default);
      Task<bool> IsAnyMacAsync(string mac, CancellationToken ct = default);
      Task<int> GetDeviceModuleIdByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<(string, int)> GetNameAndLocationIdByMacAsync(string mac, CancellationToken ct = default);
      Task<DeviceDto> GetByMacAsync(string mac,CancellationToken ct = default);
      Task UpdateConfigurationStatusByMacAsync(string mac, bool isSync,bool isUploaded, CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByVendorAndLocationAsync(Vendor vendor,Guid guid,CancellationToken ct  = default);
      Task<string> GetMacByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<string> GetMacByExternalIdAndVendorAsync(short externalId,Vendor vendor,CancellationToken ct = default);
      Task<DeviceDto> GetByExternalIdAndVendorAsync(short externalId,Vendor vendor,CancellationToken ct = default);
      Task<short> GetExternalIdByMacAsync(string mac,CancellationToken ct = default);
      Task<Guid> GetGuidByExternalAndVendorAsync(short externalId,Vendor vendor,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(Guid guid, CancellationToken ct = default);
     
}