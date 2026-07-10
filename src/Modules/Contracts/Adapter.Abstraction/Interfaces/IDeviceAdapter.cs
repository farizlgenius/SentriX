using System;
using Device.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IDeviceAdapter
{
      
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
      
      Task<bool> GetEventStatusAsync(string Mac,int ComponentId);
      Task<bool> SetEventStatusAsync(string Mac,int ComponentId,bool IsEnable);

      Task<string> GetDeviceInformationByMacAsync(string Mac);
      Task<string> GetDeviceInformationByIpAsync(string Ip);
      Task VerifyDeviceComponentAsync(int ComponentId);

      Task<bool> AsciiCommandAsync(string Mac,int ComponentId,string Command);
      Task<List<IdReportDto>> GetIdReportsAsync();

}
