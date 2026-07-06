using System;
using Device.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IDeviceAdapter
{
      Task<List<IdReportDto>> GetIdReportsAsync();
      Task CreateDeviceAsync( string Mac,
            short ComponentId);
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
      Task<bool> GetEventStatusAsync(string Mac,int ComponentId);
      Task<bool> SetEventStatusAsync(string Mac,int ComponentId,bool IsEnable);
      // Amico
      Task<string> GetDeviceInformationAsync(string ip,string login,string password);
      Task VerifyDeviceComponentAsync(int ComponentId);

}
