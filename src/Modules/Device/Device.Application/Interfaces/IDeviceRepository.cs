using System;
using Device.Contract.DTOs;
using Device.Domain.Entities;
using SharedKernel.Domain;

namespace Device.Application.Interfaces;

public interface IDeviceRepository
{
      Task<bool> IsAnyWithMacAsync(string macAddress, CancellationToken ct=default);
      Task AddAsync(Domain.Entities.Devices domain, CancellationToken ct = default);
      Task UpdatePortByComponentIdAsync(int componentId, int port, CancellationToken ct = default);
      Task UpdateIpByComponentIdAsync(int componentId, string ip, CancellationToken ct = default);
      Task VerifyDeviceMemoryAllocateStatusAsync(int componentId, string status, CancellationToken ct = default);
      Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);
      Task<IEnumerable<ModuleDto>> GetModuleByDeviceGuidAsync(Guid guid, CancellationToken ct = default);
      Task<bool> IsAnyModuleBySerialNumberAsync(string SerialNumber, CancellationToken ct = default);
      Task AddModuleAsync(Module dto,CancellationToken ct = default);
      Task<int> GetIdByMacAsync(string Mac,CancellationToken ct = default);
      Task UpdateModuleAsync(string Mac,int ModuleId,string SerialNumber,string Fw,short Port,CancellationToken ct = default);
      Task<string> GetMacByIdAsync(int id,CancellationToken ct= default);
      Task<ModuleDto> GetModuleByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<DeviceDto> GetByMacAsync(string Mac,CancellationToken ct= default);
      Task<int> GetModuleIdByMacAndAddressAsync(string Mac,int Address,CancellationToken ct =default);

      Task<IEnumerable<OptionDto>> GetOptionByLocationIdTypeAeroAsync(int locationId,string type,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByLocationIdTypeAmicoAsync(int locationId,string type,CancellationToken ct = default);

      Task<bool> IsAnyModuleByIdAsync(int ModuleId,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceGuidAsync(Guid DeviceGuid,CancellationToken ct = default);
      Task<string> GetModelByModuleIdAsync(int ModuleId,CancellationToken ct = default);
      Task<int> GetLowestModuleComponentIdByDeviceGuidAsync(Guid DeviceGuid,CancellationToken ct = default);
      Task<DeviceDto> GetDeviceByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<string> GetMacByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<(string Mac, string Type,short ComponentId)> GetMacAndTypeAndComponentIdByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<IEnumerable<(string Mac,short ComponentId,string Type)>> MacAndComponentIdListAsync(int LocationId,CancellationToken ct = default);
      Task<bool> AddReaderAsync(Reader domain,CancellationToken ct = default);
      Task<bool> DeleteReaderAsync(Guid Guid,CancellationToken ct = default);

      Task<bool> AddInputAsync(Device.Domain.Entities.Input domain,CancellationToken ct = default);
      Task<bool> DeleteInputAsync(Guid Guid,CancellationToken ct = default);
      Task<bool> AddRelayAsync(Relay domain,CancellationToken ct = default);
      Task<bool> DeleteRelayAsync(Guid Guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetReaderOptionsByModuleGuidAsync(Guid guid ,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetRelayOptionsByModuleIdAsync(Guid guid ,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetInputOptionsByModuleIdAsync(Guid guid ,CancellationToken ct = default);
      Task<int> GetIdByComponentIdAsync(short ComponentId,CancellationToken ct = default);
      Task<string> GetModuleNameByMacAndComponentIdAsync(string Mac,short ComponentId,CancellationToken ct = default);
      Task<bool> UploadDeviceAsync(int id ,CancellationToken ct = default);
      Task<IEnumerable<DeviceDto>> GetDeviceByLocationIdAsync(int LocationId,CancellationToken ct = default);
      Task<bool> IsAnyModuleNotSyncAsync(string Mac,int LocationId,DateTime SyncAt,CancellationToken ct = default);
      Task SetDeviceSyncStatusAsync(string Mac,string Status,CancellationToken ct = default);
      Task UpdateSyncTimeAsync(string Mac,CancellationToken ct = default);
      Task<(string Name,int LocationId)> GetNameAndLocationIdByMacAsync(string Mac,CancellationToken ct = default);
      Task<bool> IsAnyByGuidAsync(Guid guid,CancellationToken ct = default);
      Task DeleteAsync(Guid guid,CancellationToken ct = default);
      Task<DeviceDto> GetDeviceByDeviceIdAsync(string DeviceId,CancellationToken ct = default);

}
