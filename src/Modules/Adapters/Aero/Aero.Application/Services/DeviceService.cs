using System.Text.Json;
using Adapter.Contract.Interfaces;

namespace Aero.Application.Services;

public sealed class DeviceService : IDeviceAdapter
{
      public Task<bool> AsciiCommandAsync(Guid guid, string Command)
      {
            throw new NotImplementedException();
      }

      public Task CreateModuleAsync(Guid DeviceGuid, Guid ModuleGuid, short Model, short Address, short Port)
      {
            throw new NotImplementedException();
      }

      public Task DeleteDeviceAsync(Guid Guid, string Ip, string Mac, short ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task<JsonElement> GetDeviceInformationByIpAsync(string Ip, bool? IsFirst)
      {
            throw new NotImplementedException();
      }

      public Task<string> GetDeviceInformationByMacAsync(string Mac)
      {
            throw new NotImplementedException();
      }

      public Task<bool> GetDeviceStatusAsync(Guid guid)
      {
            throw new NotImplementedException();
      }

      public Task<bool> GetEventStatusAsync(string Mac, short ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task InititalDeviceAsync(int id, string Ip, string Mac)
      {
            throw new NotImplementedException();
      }

      public Task ResetDeviceAsync(Guid guid)
      {
            throw new NotImplementedException();
      }

      public Task<bool> SetEventStatusAsync(string Mac, short ComponentId, bool IsEnable)
      {
            throw new NotImplementedException();
      }

      public Task VerifyDeviceComponentAsync(short ComponentId)
      {
            throw new NotImplementedException();
      }
}