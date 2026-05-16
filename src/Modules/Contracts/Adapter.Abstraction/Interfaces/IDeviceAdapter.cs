using System;
using Device.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IDeviceAdapter
{
      Task<List<IdReportDto>> GetIdReportsAsync();
      Task CreateDeviceAsync(CreateDeviceDto dto);
      Task<bool> GetDeviceStatusByMacAsync(string mac);
      Task<bool> ResetDeviceAsync(string mac);
      Task CreateModuleAsync(CreateModuleDto dto);

}
