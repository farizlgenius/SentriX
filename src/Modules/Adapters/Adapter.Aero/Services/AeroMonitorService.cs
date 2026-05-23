using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Constants;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using AeroAdapter.Application.Interfaces;
using Events.Contract.Command;
using Output.Contract.DTOs;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class AeroMonitorService(IInputCommand command,IMessageBus bus) : IMonitorAdapter
{
      public async Task CreateUpdateMonitorGroup(
            string Mac,
            short ScpId,
            short MpgNumber,
            string Metadata
      )
      {
           var metadata = JsonSerializer.Deserialize<MpgMetadata>(Metadata);
           if(metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("MpgMetadata"));

           var res = command.ConfigureMonitorPointGroup(
                  Mac,
                  ScpId,
                  MpgNumber,
                  metadata.MpList
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.InputPointSpecification,Mac,ScpId));
      }

      public async Task CreateUpdateMonitorPoint(
            string Mac,
            short ComponentId,
            short DeviceComponentId,
            short ModuleComponentId,
            short InputNo,
            string Metadata
      )
      {
            var metadata = JsonSerializer.Deserialize<MpMetadata>(Metadata);
            if(metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("MpMetadata"));

            var res = command.InputPointSpecification(
                  Mac,
                  DeviceComponentId,
                  ModuleComponentId,
                  InputNo,
                  metadata.SensorMode,
                  metadata.Debounce,
                  metadata.HoldTime
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.InputPointSpecification,Mac,DeviceComponentId));

            
            res = command.MonitorPointConfiguration(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  ModuleComponentId,
                  InputNo,
                  metadata.LogFunction,
                  metadata.LatchMode,
                  metadata.DelayEntry,
                  metadata.DelayExit
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.MonitorPointConfiguration,Mac,DeviceComponentId));
      }

      public async Task DeleteMonitorGroup(
            string Mac,
            short ComponentId,
            short MpgNumber
      )
      {

           var res = command.ConfigureMonitorPointGroup(
                  Mac,
                  ComponentId,
                  MpgNumber,
                  new List<(short Type, short Number)>()
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.InputPointSpecification,Mac,ComponentId));
      }

      public async Task DeleteMonitorPoint(
            string Mac,
            short ComponentId,
            short DeviceComponentId,
            short InputNo,
            string Metadata
      )
      {
            var metadata = JsonSerializer.Deserialize<MpMetadata>(Metadata);
            if(metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("MpMetadata"));

            var res = command.MonitorPointConfiguration(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  -1,
                  InputNo,
                  metadata.LogFunction,
                  metadata.LatchMode,
                  metadata.DelayEntry,
                  metadata.DelayExit
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.MonitorPointConfiguration,Mac,DeviceComponentId));
      }

      public async Task MaskMonitorPoint(
            string Mac,
            short DeviceComponentId,
            short ComponentId,
            bool IsMask
      )
      {
            var res = command.MonitorPointMask(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  IsMask
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.MonitorPointMask,Mac,DeviceComponentId));
      }
}