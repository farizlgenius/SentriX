using System.Data;
using System.Text.Json;
using System.Xml;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Events.Contract.Command;
using Microsoft.Extensions.Logging;
using Output.Contract.DTOs;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class AeroControlService(IAeroRepository repo,IOutputCommand writer,IMessageBus bus) : IControlAdapter
{
      public async Task CreateAsync(
            string Mac,
            short ComponentId,
            short DeviceComponentId,
            short ModuleComponentId,
            short OutputNo,
            short Mode,
            short DefaultPulse
      )
      {
            var res = writer.OutputPointSpecification(
                  Mac,
                  DeviceComponentId,
                  ModuleComponentId,
                  OutputNo,
                  Mode
                  );

            await bus.SendAsync(new AddCommandEvent(res));

            res = writer.ControlPointConfiguration(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  ModuleComponentId,
                  OutputNo,
                  DefaultPulse
            );

             await bus.SendAsync(new AddCommandEvent(res));

             if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification,Mac,DeviceComponentId));
            
      }

      public async Task<IEnumerable<OptionDto>> GetRelayModeAsync()
      {
            return await repo.GetRelayOptionAsync();
      }

      public async Task TriggerOutputAsync(string Mac,short ScpId, short CpId, short Command)
      {
           var res = writer.ControlPointCommand(ScpId,Mac,CpId,Command);
           await bus.SendAsync(new AddCommandEvent(res));
           if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ControlPointCommand,Mac,ScpId));
      }

      public async Task DeleteAsync(
              string Mac,
            short ScpId,
            short CpNumber,
            short OpNumber,
            short DefaultPulse
      )
      {
            var res = writer.DeleteControlPoint(Mac,ScpId,CpNumber,OpNumber,DefaultPulse);
           

            await bus.SendAsync(new AddCommandEvent(res));

             if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ControlPointConfiguration,Mac,ScpId));
      }

      public async Task UpdateAsync(
              string Mac,
            short ComponentId,
            short DeviceComponentId,
            short ModuleComponentId,
            short OutputNo,
            short Mode,
            short DefaultPulse
      )
      {
            
             var res = writer.OutputPointSpecification(
                  Mac,
                  DeviceComponentId,
                  ModuleComponentId,
                  OutputNo,
                  Mode
                  );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification,Mac,DeviceComponentId));

            res = writer.ControlPointConfiguration(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  ModuleComponentId,
                  OutputNo,
                  DefaultPulse
            );

             await bus.SendAsync(new AddCommandEvent(res));

             if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification,Mac,DeviceComponentId));
      }
}