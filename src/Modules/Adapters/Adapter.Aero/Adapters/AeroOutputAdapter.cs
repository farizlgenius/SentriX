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

namespace Adapter.Aero.Adapters;

public sealed class AeroOutputAdapter(IAeroRepository repo, IOutputCommand writer, IMessageBus bus) : IAeroOutputAdapter
{
      public async Task CreateAsync(
            Guid Guid,
            Guid DeviceGuid,
            string Metadata,
            Guid ModuleGuid
      )
      {
            var meta = JsonHelper.Deserialize<CpMetadata>(Metadata);
            if (meta is null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed(nameof(CpMetadata)));

            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
            var moduleSlot = await repo.GetSlotIdByGuidAsync<SioSlot>(ModuleGuid);
            var slot = await repo.GetFreeSlotAsync<CpSlot>(DeviceGuid);
            var res = writer.OutputPointSpecification(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                 (short)moduleSlot,
                  meta.OutputNo,
                  OutputHelper.FinalizeOutputMode(meta.DriveMode, meta.OfflineMode)
                  );

            await bus.SendAsync(new AddCommandEvent(res));

            res = writer.ControlPointConfiguration(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  (short)slot,
                 (short)moduleSlot,
                  meta.OutputNo,
                  meta.DefaultPulse
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, deviceSlot.mac, deviceSlot.slot_id));

            await repo.InsertSlotAsync<CpSlot>(
                  DeviceGuid,
                  Guid,
                  slot
            );

      }

      public async Task<IEnumerable<OptionDto>> GetRelayModeAsync()
      {
            return await repo.GetRelayOptionAsync();
      }

      public async Task TriggerOutputAsync(
            Guid Guid,
            Guid DeviceGuid,
            short Command
            )
      {
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
            var slot = await repo.GetSlotIdByGuidAsync<CpSlot>(Guid);
            var res = writer.ControlPointCommand(
             (short)deviceSlot.slot_id,
             deviceSlot.mac,
             (short)slot,
             Command);

            await bus.SendAsync(new AddCommandEvent(res));
            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ControlPointCommand, deviceSlot.mac, deviceSlot.slot_id));
      }

      public async Task DeleteAsync(
            Guid Guid,
            Guid DeviceGuid,
            string Metadata
      )
      {
            var meta = JsonHelper.Deserialize<CpMetadata>(Metadata);
            if(meta is null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed(nameof(CpMetadata)));
                  
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
            var slot = await repo.GetSlotIdByGuidAsync<CpSlot>(Guid);

            var res = writer.DeleteControlPoint(
                  deviceSlot.mac, 
                  (short)deviceSlot.slot_id, 
                  (short)slot, 
                  meta.OutputNo, 
                  meta.DefaultPulse);


            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ControlPointConfiguration, deviceSlot.mac, deviceSlot.slot_id));

            await repo.EjectSlotAsync<CpSlot>(
                  Guid,
                  slot
            );
      }

      public async Task UpdateAsync(
             Guid Guid,
            Guid DeviceGuid,
            string Metadata,
            Guid ModuleGuid
      )
      {
            var meta = JsonHelper.Deserialize<CpMetadata>(Metadata);
            if(meta is null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed(nameof(CpMetadata)));
                  
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
            var slot = await repo.GetSlotIdByGuidAsync<CpSlot>(Guid);
            var moduleSlot = await repo.GetSlotIdByGuidAsync<CpSlot>(ModuleGuid);

            var res = writer.OutputPointSpecification(
                 deviceSlot.mac,
                 (short)deviceSlot.slot_id,
                 (short)moduleSlot,
                 meta.OutputNo,
                 OutputHelper.FinalizeOutputMode(meta.DriveMode, meta.OfflineMode)
                 );

            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification,deviceSlot.mac, deviceSlot.slot_id));

            res = writer.ControlPointConfiguration(
                   deviceSlot.mac,
                 (short)deviceSlot.slot_id,
                 (short)slot,
                 (short)moduleSlot,
                  meta.OutputNo,
                  meta.DefaultPulse
            );

            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification,deviceSlot.mac, deviceSlot.slot_id));
      }

      public async Task CommandOutputAsync(
            Guid Guid,
            Guid DeviceGuid,
            short Command
            )
      {
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
            var slot = await repo.GetSlotIdByGuidAsync<CpSlot>(Guid);
            var res = writer.ControlPointCommand(
                  (short)deviceSlot.slot_id,
                  deviceSlot.mac,
                  (short)slot,
                  Command
                  );

            await bus.SendAsync(new AddCommandEvent(res));

            if (!res.IsSend)
                  throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ControlPointCommand, deviceSlot.mac, deviceSlot.slot_id));
      }
}