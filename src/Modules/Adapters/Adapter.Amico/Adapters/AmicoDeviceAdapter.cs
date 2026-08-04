using System;
using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;
using Adapter.Amico.Persistences.Entities;
using Device.Contract.DTOs;
using Serilog;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Amico.Adapters;

public sealed class AmicoDeviceAdapter(
      IDeviceCommand command,
      IAmicoRepository repo,
      IMessageBus bus
      ) : IAmicoDeviceAdapter
{




      public Task CreateModuleAsync(
             Guid DeviceGuid,
            Guid ModuleGuid,
            short Model,
            short Address,
            short Port
      )
      {
            throw new NotImplementedException();
      }



      public Task<bool> GetEventStatusAsync(string Mac, short ComponentId)
      {
            throw new NotImplementedException();
      }

      public Task<List<IdReportDto>> GetIdReportsAsync()
      {
            throw new NotImplementedException();
      }

      public async Task<string> GetDeviceInformationByMacAsync(string Mac)
      {

            var session = await command.CheckSessionAsync(Mac);
            var amico = await repo.GetAmicoByMacAsync(Mac);
            
            var info = await command.DeviceInfoAsync(amico.ip,session);

            return JsonHelper.Serialize(info);

      }

      public async Task<JsonElement> GetDeviceInformationByIpAsync(string Ip,bool? IsFirst)
      {


            var res = await command.LoginAsync(Ip,IsFirst);

            var info = await command.DeviceInfoAsync(Ip,res.Session);            

            return JsonHelper.ToJsonElement(info);

      }

      public Task ResetDeviceAsync(Guid Guid)
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

      public Task<bool> AsciiCommandAsync(Guid Guid, string Command)
      {
            throw new NotImplementedException();
      }

      public async Task CreateDeviceAsync(Guid Guid, string Ip, string Mac, short ComponentId,int LocationId)
      {
            var res = await command.LoginAsync(Ip,true);

            // await command.ChangeLogin(Ip,res.Session);

            await repo.AddAsync(
                  Guid,
                  Mac,
                  Ip,
                  res.Session
            );


            await command.VerifyDeviceComponentAsync(Ip,res.Session,LocationId);
      }

      public async Task<bool> GetDeviceStatusAsync(Guid Guid)
      {
            var amico = await repo.GetAmicoByGuidAsync(Guid);

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), amico.mac));


            var res = await command.CheckSessionAsync(amico.mac);

            return true;
      }

      public async Task DeleteDeviceAsync(Guid Guid, string Ip, string Mac, short ComponentId)
      {
            await repo.DeleteAsync(Mac,Ip);
      }
}
