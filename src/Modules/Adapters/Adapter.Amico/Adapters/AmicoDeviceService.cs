using System;
using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;
using Device.Contract.DTOs;
using SharedKernel.Helpers;

namespace Adapter.Amico.Adapters;

public sealed class AmicoDeviceAdapter(IDeviceCommand command) : IAmicoDeviceAdapter
{
      public Task<bool> AsciiCommandAsync(string Mac, int ComponentId, string Command)
      {
            throw new NotImplementedException();
      }

      public async Task CreateDeviceAsync(string Mac, short ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task CreateModuleAsync(string Mac, short ScpId, short SioNumber, short Model, short Address, short Port)
      {
            throw new NotImplementedException();
      }

      public Task<bool> GetDeviceStatusAsync(int ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> GetEventStatusAsync(string Mac, int ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task<List<IdReportDto>> GetIdReportsAsync()
      {
            throw new NotImplementedException();
      }

      public async Task<string> GetDeviceInformationAsync(string ip, string login, string password)
      {
            // Login
            var logRes = await command.LoginAsync(ip,login,password);
            
            var info = await command.DeviceInfoAsync(ip,logRes.Session);

            await command.LogoutAsync(ip,logRes.Session);

            return JSONHelper.Serialize(info);

      }

      public Task<bool> ResetDeviceAsync(string Mac, short ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> SetEventStatusAsync(string Mac, int ComponentId, bool IsEnable)
      {
            throw new NotImplementedException();
      }

}
