using System;
using Device.Contract.DTOs;
using Device.Domain.Entities;
using SharedKernel.Domain;

namespace Device.Application.Interfaces;

public interface IDeviceRepository
{
      Task<bool> IsAnyWithMacAsync(string macAddress, CancellationToken ct=default);
      Task<DeviceDto> CreateAsync(Domain.Entities.Devices domain, CancellationToken ct = default);
      Task UpdatePortByMacAsync(string mac, int port, CancellationToken ct = default);
      Task UpdateIpByMacAsync(string mac, string ip, CancellationToken ct = default);
      Task VerifyDeviceMemoryAllocateStatusAsync(string mac, string status, CancellationToken ct = default);
      Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);
      Task<List<ModuleDto>> GetModuleByDeviceIdAsync(int id, CancellationToken ct = default);
      Task<bool> IsAnyModuleBySerialNumberAsync(string SerialNumber, CancellationToken ct = default);
      Task<ModuleDto> CreateModuleAsync(Module dto,CancellationToken ct = default);
      Task<int> GetIdByMacAsync(string Mac,CancellationToken ct = default);
      Task UpdateModuleAsync(string Mac,int ModuleId,string SerialNumber,string Fw,int Port,CancellationToken ct = default);
      Task<string> GetMacByIdAsync(int id,CancellationToken ct= default);
      Task<ModuleDto> GetModuleByIdAsync(int id,CancellationToken ct = default);
      Task<DeviceDto> GetByMacAsync(string Mac,CancellationToken ct= default);

}
