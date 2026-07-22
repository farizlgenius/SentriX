using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Constants;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Events.Contract.Command;
using Output.Contract.DTOs;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroInputAdapter(
      IInputCommand command,
      IMessageBus bus,
      IAeroRepository repo
      ) : IAeroInputAdapter
{
      public async Task CreateUpdateMonitorGroup(
            Guid Guid,
            Guid DeviceGuid,
            List<(short Type,Guid InputGuid)> Inputs
      )
      {
           var slot = await repo.GetFreeSlotAsync<MpgSlot>(DeviceGuid);
           var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
           var inputsTask = Inputs.Select(async x =>
           {
                  var inputSlot = await repo.GetSlotIdByGuidAsync<MpgSlot>(x.InputGuid);
                 return (x.Type,(short)inputSlot);
           });

           var inputs = (await Task.WhenAll(inputsTask)).ToList();

           var res = command.ConfigureMonitorPointGroup(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  (short)slot,
                  inputs
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.InputPointSpecification,deviceSlot.mac,deviceSlot.slot_id));

            await repo.InsertSlotAsync<MpgSlot>(
                  DeviceGuid,
                  Guid,
                  slot
                  );
      }

      public async Task CreateUpdateMonitorPoint(
            Guid Guid,
            Guid DeviceGuid,
            string metadata,
            Guid ModuleGuid
      )
      {
            
            var meta = JsonHelper.Deserialize<MpMetadata>(metadata);
            if(meta is null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed(nameof(MpMetadata)));

            var slot = await repo.GetFreeSlotAsync<MpgSlot>(DeviceGuid);
           var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
           var moduleSlot = await repo.GetSlotByGuidAsync<SioSlot>(ModuleGuid);

            var res = command.InputPointSpecification(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                   (short)moduleSlot.slot_id,
                  meta.InputNo,
                  meta.SensorMode,
                  meta.Debounce,
                  meta.HoldTime
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.InputPointSpecification,deviceSlot.mac,deviceSlot.slot_id));

            
            res = command.MonitorPointConfiguration(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  (short)slot,
                   (short)moduleSlot.slot_id,
                  meta.InputNo,
                  meta.LogFunction,
                  meta.LatchMode,
                  meta.DelayEntry,
                  meta.DelayExit
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.MonitorPointConfiguration,deviceSlot.mac,deviceSlot.slot_id));

            await repo.InsertSlotAsync<MpSlot>(
                  DeviceGuid,
                  Guid,
                  slot
                  );
      }

      public async Task DeleteMonitorGroup(
            Guid Guid,
            Guid DeviceGuid
      )
      {
            
           var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
           var slot = await repo.GetSlotIdByGuidAsync<MpgSlot>(Guid);


           var res = command.ConfigureMonitorPointGroup(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  (short)slot,
                  new List<(short Type, short Number)>()
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.InputPointSpecification,deviceSlot.mac,deviceSlot.slot_id));

            await repo.EjectSlotAsync<MpgSlot>(
                  Guid,
                  slot
            );
      }

      public async Task DeleteMonitorPoint(
            Guid Guid,
            Guid DeviceGuid,
            string Metadata
      )
      {
            var meta = JsonHelper.Deserialize<MpMetadata>(Metadata);
            if(meta is null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed(nameof(MpMetadata)));
            
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
           var slot = await repo.GetSlotIdByGuidAsync<MpgSlot>(Guid);

            var res = command.MonitorPointConfiguration(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  (short)slot,
                  -1,
                  meta.InputNo,
                  meta.LogFunction,
                  meta.LatchMode,
                  meta.DelayEntry,
                  meta.DelayExit
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.MonitorPointConfiguration,deviceSlot.mac,deviceSlot.slot_id));

            await repo.EjectSlotAsync<MpSlot>(
                  Guid,
                  slot
            );
      }

      public async Task MaskMonitorPoint(
            Guid Guid,
            Guid DeviceGuid,
            bool IsMask
      )
      {
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
           var slot = await repo.GetSlotIdByGuidAsync<MpgSlot>(Guid);
            var res = command.MonitorPointMask(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  (short)slot,
                  IsMask
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if(!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.MonitorPointMask, deviceSlot.mac, deviceSlot.slot_id));
      }
}