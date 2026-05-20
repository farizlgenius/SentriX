using System;
using Device.Contract.DTOs;
using Device.Domain.Entities;
using SharedKernel.Domain;

namespace Device.Application.Interfaces;

public interface IDeviceRepository
{
      Task<bool> IsAnyWithMacAsync(string macAddress, CancellationToken ct=default);
      Task<DeviceDto> CreateAsync(Domain.Entities.Devices domain, CancellationToken ct = default);
      Task UpdatePortByMacAsync(int componentId, int port, CancellationToken ct = default);
      Task UpdateIpByMacAsync(int componentId, string ip, CancellationToken ct = default);
      Task VerifyDeviceMemoryAllocateStatusAsync(int componentId, string status, CancellationToken ct = default);
      Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);
      Task<List<ModuleDto>> GetModuleByDeviceIdAsync(int id, CancellationToken ct = default);
      Task<bool> IsAnyModuleBySerialNumberAsync(string SerialNumber, CancellationToken ct = default);
      Task<ModuleDto> CreateModuleAsync(Module dto,CancellationToken ct = default);
      Task<int> GetIdByMacAsync(string Mac,CancellationToken ct = default);
      Task UpdateModuleAsync(string Mac,int ModuleId,string SerialNumber,string Fw,short Port,CancellationToken ct = default);
      Task<string> GetMacByIdAsync(int id,CancellationToken ct= default);
      Task<ModuleDto> GetModuleByIdAsync(int id,CancellationToken ct = default);
      Task<DeviceDto> GetByMacAsync(string Mac,CancellationToken ct= default);
      Task<int> GetModuleIdByMacAndAddressAsync(string Mac,int Address,CancellationToken ct =default);
      Task<IEnumerable<OptionDto>> GetOptionByLocationIdAsync(int locationId,CancellationToken ct = default);
      Task<bool> IsAnyModuleByIdAsync(int ModuleId,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceIdAsync(int ModuleId,CancellationToken ct = default);
      Task<string> GetModelByModuleIdAsync(int ModuleId,CancellationToken ct = default);
      Task<int> GetComponentIdByMacAsync(string Mac,CancellationToken ct = default);
      Task<int> GetLowestModuleComponentIdByDeviceIdAsync(int DeviceId,CancellationToken ct = default);
      Task<DeviceDto> GetDeviceByComponentIdAsync(int ComponentId,CancellationToken ct = default);
      Task<short> GetComponentIdByIdAsync(int id,CancellationToken ct = default);

}
