using System;
using System.Text.Json;
using Device.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IDeviceAdapter
{
      Task DeleteDeviceAsync(Guid Guid,string Ip,string Mac,short ComponentId);
      Task CreateDeviceAsync( 
            Guid Guid,
            string Ip,
            string Mac,
            short ComponentId,
            int LocationId
            );
      Task<bool> GetDeviceStatusAsync(string Mac,int ComponentId);
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
      Task<JsonElement> GetDeviceInformationByIpAsync(string Ip,bool? IsFirst);
      Task VerifyDeviceComponentAsync(int ComponentId);

      Task<bool> AsciiCommandAsync(string Mac,int ComponentId,string Command);
      Task<List<IdReportDto>> GetIdReportsAsync();

}
