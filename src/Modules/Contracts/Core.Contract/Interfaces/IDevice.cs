using Core.Contract.DTOs.Device;

namespace Core.Contract.Interfaces;

public interface IDevice : IBase<DeviceDto, CreateDeviceDto, UpdateDeviceDto>
{
      Task<IEnumerable<TempDeviceDto>> GetScanDeviceAsync();
}