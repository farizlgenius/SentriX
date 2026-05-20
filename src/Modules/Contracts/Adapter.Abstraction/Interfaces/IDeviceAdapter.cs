using System;
using Device.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IDeviceAdapter
{
      Task<List<IdReportDto>> GetIdReportsAsync();
      Task CreateDeviceAsync(DeviceDto dto);
      Task<bool> GetDeviceStatusAsync(int ComponentId);
      Task<bool> ResetDeviceAsync(string Mac,short ComponentId);
      Task CreateModuleAsync(
            string Mac,
            short ScpId,
            short SioNumber,
            short Model,
            short Address,
            short Port
      );
      Task<bool> AsciiCommandAsync(string Mac,int ComponentId,string Command);

}
