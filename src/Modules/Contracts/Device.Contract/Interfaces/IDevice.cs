using System.Text.Json;
using Device.Contract.DTOs;
using SharedKernel.Domain;

namespace Device.Contract.Interfaces;

public interface IDevice
{
      Task<List<IdReportDto>> GetIdReportsAsync();
      Task<IEnumerable<OptionDto>> GetOptionByTypeAndLocationIdAsync(int locationId,string type, CancellationToken ct = default);
      Task<DeviceDto> CreateAsync(CreateDeviceDto dto, CancellationToken ct = default);
      Task<DeviceStatusDto> GetStatusByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);
      Task ResetDeviceAsync(Guid guid, CancellationToken ct = default);
      Task<IEnumerable<ModuleDto>> GetModuleByDeviceGuidAsync(Guid guid, CancellationToken ct = default);
      Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto, CancellationToken ct = default);
      Task GetModuleStatusByGuidAsync(Guid guid, CancellationToken ct = default);
      Task AsciiCommandAsync(Guid guid, AeroCommandDto command, CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceGuidAsync(Guid guid, CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetReaderOptionsByModuleGuidAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetInputOptionsByModuleIdAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetRelayOptionsByModuleIdAsync(Guid guid,CancellationToken ct = default);
      Task GetEventStatusAsync(Guid guid,CancellationToken ct = default);
      Task SetEventStatusAsync(SetEventDto dto,CancellationToken ct= default);
      Task<string> GetModuleNameByMacAndComponentIdAsync(string Mac,short ComponentId,CancellationToken ct = default);
      Task UploadDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<JsonElement> GetAmicoDeviceInformationAsync(AmicoStartSessionDto dto);
      Task<DeviceDto> DeleteDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<DeviceDto> GetDeviceByDeviceIdAsync(string DeviceId,CancellationToken ct = default);
      Task<DeviceDto> GetDeviceByGuidAsync(Guid guid,CancellationToken ct = default);


}
