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

namespace Adapter.Aero.Adapters;

public sealed class AeroInputAdapter(IInputCommand command,IMessageBus bus) : IAeroInputAdapter
{
      public async Task CreateUpdateMonitorGroup(
            string Mac,
            short ScpId,
            short MpgNumber,
            List<(short Type, short Number)> Inputs
      )
      {

           var res = command.ConfigureMonitorPointGroup(
                  Mac,
                  ScpId,
                  MpgNumber,
                  Inputs
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
            short SensorMode,
            short Debounce,
            short HoldTime,
            short LogFunction,
            short LatchMode,
            short DelayEntry,
            short DelayExit
      )
      {

            var res = command.InputPointSpecification(
                  Mac,
                  DeviceComponentId,
                  ModuleComponentId,
                  InputNo,
                  SensorMode,
                  Debounce,
                  HoldTime
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
                  LogFunction,
                  LatchMode,
                  DelayEntry,
                  DelayExit
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
            short SensorMode,
            short Debounce,
            short HoldTime,
            short LogFunction,
            short LatchMode,
            short DelayEntry,
            short DelayExit
      )
      {

            var res = command.MonitorPointConfiguration(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  -1,
                  InputNo,
                  LogFunction,
                  LatchMode,
                  DelayEntry,
                  DelayExit
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