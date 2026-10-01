using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Core.Contract.Commands.Events;
using Core.Contract.DTOs.Door;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class DoorService(
      IDoorRepository door,
      IOutputRepository output,
      IInputRepository input,
      IMessageBus bus
      ) : IDoorAdapter
{
      public async Task Doors
      ( string mac,
            string ip,
            DoorType type,
            short deviceId,
            string metadata,
            List<(
                  short readerNo,
                  ReaderMode readerMode,
                  ReaderDirection readerDirection,
                  string metadata,
                  int deviceModuleId
            )> readers,
            (short outputNo,string metadata,int deviceModuleId)? buzzer,
            (short inputNo,string metadata,int deviceModuleId)? rex,
            (short inputNo,string metadata,int deviceModuleId)? bg,
            (short inputNo,string metadata,int deviceModuleId)? sensor,
            (short outputNo,string metadata,int deviceModuleId)? relay,
            CancellationToken ct = default)
      {
            // Reader Configuation
            foreach(var reader in readers)
            {
                  var meta = JsonHelper.Deserialize<ReaderMetadata>(reader.metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("ReaderMetadata"));

                  short osdpFlag = 0x00;
                  if (reader.readerMode == ReaderMode.Osdp)
                  {
                        osdpFlag += meta.Baudrate;
                        osdpFlag |= meta.AutoDiscover;
                        osdpFlag |= meta.Tracing;
                        osdpFlag |= (short)(meta.Address << 5);
                        osdpFlag |= meta.SecureChannel;
                  }

                  var res = door.ReaderSpecification(
                        mac,
                        deviceId,
                        (short)reader.deviceModuleId,
                        reader.readerNo,
                        osdpFlag
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }

            // Relay Configuration
            if(relay != null)
            {
                  var meta = JsonHelper.Deserialize<OutputMetadata>(relay.Value.metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("OutputMetadata"));

                  var res = output.OutputPointSpecification(
                        mac,
                        deviceId,
                        (short)relay.Value.deviceModuleId,
                        relay.Value.outputNo,
                        meta.OfflineMode,
                        meta.DefaultMode
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }
            
            // Sensor Configuration
            if(sensor != null)
            {
                  var meta = JsonHelper.Deserialize<InputMetadata>(sensor.Value.metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("InputMetadata"));

                  var res = input.InputPointSpecification(
                        mac,
                        deviceId,
                        (short)sensor.Value.deviceModuleId,
                        sensor.Value.inputNo,
                        meta.Mode,
                        meta.Debounce,
                        meta.HoldTime
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }

            // Rex Configuration
            if(rex != null)
            {
                  var meta = JsonHelper.Deserialize<InputMetadata>(rex.Value.metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("InputMetadata"));

                  var res = input.InputPointSpecification(
                        mac,
                        deviceId,
                        (short)rex.Value.deviceModuleId,
                        rex.Value.inputNo,
                        meta.Mode,
                        meta.Debounce,
                        meta.HoldTime
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }

            // Buzzer Configuration
            if(buzzer != null)
            {
                  var meta = JsonHelper.Deserialize<OutputMetadata>(buzzer.Value.metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("OutputMetadata"));

                  var res = output.OutputPointSpecification(
                        mac,
                        deviceId,
                        (short)buzzer.Value.deviceModuleId,
                        buzzer.Value.outputNo,
                        meta.OfflineMode,
                        meta.DefaultMode
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }

            // and the trigger setting here

            // BG Configuration

            // Door Configuration

            
      }

}