using System;
using Device.Contract.DTOs;
using SharedKernel.Domain;

namespace Device.Contract.Interfaces;

public interface IDevice
{
      Task<List<IdReportDto>> GetIdReportsAsync();
      Task<IEnumerable<OptionDto>> GetOptionByTypeAndLocationIdAsync(int locationId,string type, CancellationToken ct = default);
      Task<DeviceDto> CreateAsync(CreateDeviceDto dto, CancellationToken ct = default);
      Task<DeviceStatusDto> GetStatusByIdAsync(int id, CancellationToken ct = default);
      Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);
      Task<BaseResponse> ResetDeviceAsync(int id, CancellationToken ct = default);
      Task<IEnumerable<ModuleDto>> GetModuleByDeviceIdAsync(int id, CancellationToken ct = default);
      Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto, CancellationToken ct = default);
      Task<BaseResponse> GetModuleStatusByIdAsync(int id, CancellationToken ct = default);
      Task<BaseResponse> AsciiCommandAsync(int deviceId, AeroCommandDto command, CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceIdAsync(int moduleId, CancellationToken ct = default);
      Task<DeviceDto> GetDeviceByComponentIdAsync(int ComponentId, CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetReaderOptionsByModuleIdAsync(int id,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetInputOptionsByModuleIdAsync(int id,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetRelayOptionsByModuleIdAsync(int id,CancellationToken ct = default);
      Task<BaseResponse> GetEventStatusAsync(string type,int id,CancellationToken ct = default);
      Task<BaseResponse> SetEventStatusAsync(SetEventDto dto,CancellationToken ct= default);
      Task<string> GetModuleNameByMacAndComponentIdAsync(string Mac,short ComponentId,CancellationToken ct = default);
      Task<BaseResponse> UploadDeviceAsync(int id,CancellationToken ct = default);

}
