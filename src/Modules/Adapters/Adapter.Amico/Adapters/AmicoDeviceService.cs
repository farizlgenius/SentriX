using System;
using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;
using Device.Contract.DTOs;
using SharedKernel.Helpers;

namespace Adapter.Amico.Adapters;

public sealed class AmicoDeviceAdapter(IDeviceCommand command,IAmicoRepository repo) : IAmicoDeviceAdapter
{


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

      public async Task<string> GetDeviceInformationByMacAsync(string Mac)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            var res = await command.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await command.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac,news.Session);
            }
            
            var info = await command.DeviceInfoAsync(amico.ip,session);

            return JsonHelper.Serialize(info);

      }

      public async Task<string> GetDeviceInformationByIpAsync(string Ip)
      {
            var res = await command.LoginAsync(Ip);

            var info = await command.DeviceInfoAsync(Ip,res.Session);            

            return JsonHelper.Serialize(info);

      }

      public Task<bool> ResetDeviceAsync(string Mac, short ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> SetEventStatusAsync(string Mac, int ComponentId, bool IsEnable)
      {
            throw new NotImplementedException();
      }

      public Task VerifyDeviceComponentAsync(int ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task<bool> AsciiCommandAsync(string Mac, int ComponentId, string Command)
      {
            throw new NotImplementedException();
      }
}
