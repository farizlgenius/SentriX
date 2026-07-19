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
      Task<bool> GetDeviceStatusAsync(string Ip,string Mac,short ComponentId);
      Task<bool> ResetDeviceAsync(string Mac,short ComponentId);
      Task CreateModuleAsync(
            string Mac,
            short ScpId,
            short SioNumber,
            short Model,
            short Address,
            short Port
      );
      
      Task<bool> GetEventStatusAsync(string Mac,short ComponentId);
      Task<bool> SetEventStatusAsync(string Mac,short ComponentId,bool IsEnable);

      Task<string> GetDeviceInformationByMacAsync(string Mac);
      Task<JsonElement> GetDeviceInformationByIpAsync(string Ip,bool? IsFirst);
      Task VerifyDeviceComponentAsync(short ComponentId);

      Task<bool> AsciiCommandAsync(string Mac,short ComponentId,string Command);
      Task<List<IdReportDto>> GetIdReportsAsync();

}
