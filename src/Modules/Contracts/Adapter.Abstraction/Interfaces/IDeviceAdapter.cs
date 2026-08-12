using System;
using System.Text.Json;

namespace Adapter.Abstraction.Interfaces;

public interface IDeviceAdapter
{

      Task CreateDeviceAsync(
            int Guid,
            string Ip,
            string Mac,
            short ScpId
            );

      Task DeleteDeviceAsync(Guid Guid, string Ip, string Mac, short ComponentId);
      Task<bool> GetDeviceStatusAsync(Guid guid);
      Task ResetDeviceAsync(Guid guid);
      Task CreateModuleAsync(
            Guid DeviceGuid,
            Guid ModuleGuid,
            short Model,
            short Address,
            short Port
            );

      Task<bool> GetEventStatusAsync(string Mac, short ComponentId);
      Task<bool> SetEventStatusAsync(string Mac, short ComponentId, bool IsEnable);

      Task<string> GetDeviceInformationByMacAsync(string Mac);
      Task<JsonElement> GetDeviceInformationByIpAsync(string Ip, bool? IsFirst);
      Task VerifyDeviceComponentAsync(short ComponentId);

      Task<bool> AsciiCommandAsync(Guid guid, string Command);


      // Task<List<IdReportDto>> GetIdReportsAsync();

}
