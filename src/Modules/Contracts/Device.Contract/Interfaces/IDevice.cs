using System;
using Device.Contract.DTOs;
using SharedKernel.Domain;

namespace Device.Contract.Interfaces;

public interface IDevice
{
      Task<List<IdReportDto>> GetIdReportsAsync();
      Task<IEnumerable<OptionDto>> GetOptionByLocationIdAsync(int locationId,CancellationToken ct = default);
      Task<DeviceDto> CreateAsync(CreateDeviceDto dto,CancellationToken ct=default);
      Task<DeviceStatusDto> GetStatusByIdAsync(int id,CancellationToken ct=default);
      Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct=default);
      Task<BaseResponse> ResetDeviceAsync(int id,CancellationToken ct = default);
      Task<List<ModuleDto>> GetModuleByDeviceIdAsync(int id,CancellationToken ct=default);
      Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto,CancellationToken ct=default);
      Task<BaseResponse> GetModuleStatusByIdAsync(int id,CancellationToken ct = default);
      Task<BaseResponse> AsciiCommandAsync(int deviceId,string command,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceIdAsync(int moduleId,CancellationToken ct =  default);

}
