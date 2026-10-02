using Adapter.Contract.Interfaces;
using Aero.Application.Enums;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Aero.Application.Metadata.Door;
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
      ( 
            string mac,
            string ip,
            DoorType type,
            short deviceId,
            List<short> doorId,
            string metadata,
            List<(
                  short readerNo,
                  ReaderMode readerMode,
                  ReaderDirection readerDirection,
                  string metadata,
                  short deviceModuleId
            )> readers,
            (short outputNo,string metadata,short deviceModuleId,short buzzerId)? buzzer,
            List<(short inputNo,string metadata,short deviceModuleId)> rexes,
            (short inputNo,string metadata,short deviceModuleId,short bgId)? bg,
            (short inputNo,string metadata,short deviceModuleId)? sensor,
            (short outputNo,string metadata,short deviceModuleId)? relay,
            CancellationToken ct = default
            )
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
                  var meta = JsonHelper.Deserialize<RelayMetadata>(relay.Value.metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("RelayMetadata"));

                  var res = output.OutputPointSpecification(
                        mac,
                        deviceId,
                        relay.Value.deviceModuleId,
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
                        sensor.Value.deviceModuleId,
                        sensor.Value.inputNo,
                        meta.Mode,
                        meta.Debounce,
                        meta.HoldTime
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }

            // Rex Configuration
            foreach(var rex in rexes)
            {
                  var meta = JsonHelper.Deserialize<InputMetadata>(rex.metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("InputMetadata"));

                  var res = input.InputPointSpecification(
                        mac,
                        deviceId,
                        rex.deviceModuleId,
                        rex.inputNo,
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
                        buzzer.Value.deviceModuleId,
                        buzzer.Value.outputNo,
                        meta.OfflineMode,
                        meta.DefaultMode
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

                  res = output.ControlPointConfiguration(
                        mac,
                        deviceId,
                        buzzer.Value.deviceModuleId,
                        buzzer.Value.buzzerId,
                        buzzer.Value.outputNo,
                        1
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }

            // and the trigger setting here

            // BG Configuration
            if(bg != null)
            {
                  var meta = JsonHelper.Deserialize<BgMetadata>(bg.Value.metadata);
                  if (meta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("BgMetadata"));

                  var res = input.InputPointSpecification(
                        mac,
                        deviceId,
                        (short)bg.Value.deviceModuleId,
                        bg.Value.inputNo,
                        meta.Mode,
                        meta.Debounce,
                        meta.HoldTime
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

                  res = input.MonitorPointConfiguration(
                        mac,
                        deviceId,
                        bg.Value.bgId,
                        bg.Value.deviceModuleId,
                        bg.Value.inputNo,
                        0,
                        0,
                        0,
                        0
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }

            var doorMetadata = JsonHelper.Deserialize<DoorMetadata>(metadata);
                  if (doorMetadata == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("DoorMetadata"));

            

            // Door Configuration
            short spare = 0x00;
            if(doorMetadata.ForceCardPin) spare |= (short)ExtendedAccessControlFlags.ACR_FE_NOPINCARD;
            if(doorMetadata.DoubleCard) spare |= (short)ExtendedAccessControlFlags.ACR_FE_DCARD;
            if(doorMetadata.OutputSelectionTracking) spare |= (short)ExtendedAccessControlFlags.ACR_FE_FLOOR_PIN;
            if(doorMetadata.LockedOverride) spare |= (short)ExtendedAccessControlFlags.ACR_FE_CRD_OVR_EN;
            if(doorMetadata.HostPermission) spare |= (short)ExtendedAccessControlFlags.ACR_FE_HOST_BYPASS; 

            if(type == DoorType.Dual) spare |= (short)ExtendedAccessControlFlags.ACR_FE_LINK_MODE;

            short accessFlag = 0x00;
            if(doorMetadata.DecreaseUseLimit) accessFlag |= (short)AccessControlFlags.ACR_F_DCR;
            if(doorMetadata.RequireUseLimit) accessFlag |= (short)AccessControlFlags.ACR_F_CUL;
            if(doorMetadata.DeniedDuress) accessFlag |= (short)AccessControlFlags.ACR_F_DRSS;
            if(doorMetadata.QuietRex) accessFlag |= (short)AccessControlFlags.ACR_F_QEXIT;
            if(doorMetadata.FilterStatus) accessFlag |= (short)AccessControlFlags.ACR_F_FILTER;
            if(doorMetadata.DoubleCardAccess) accessFlag |= (short)AccessControlFlags.ACR_F_2CARD; 
            if(doorMetadata.HostPermission) accessFlag |= (short)AccessControlFlags.ACR_F_HOST_CBG;
            if(doorMetadata.HostOfflineGrant) accessFlag |= (short)AccessControlFlags.ACR_F_HOST_SFT;

            if(type == DoorType.Dual)
            {

                  var res = door.AccessControlReaderConfiguration(
                        mac,
                        deviceId,
                        doorId.ElementAt(1),
                        2,
                        doorId.ElementAt(0),
                        (short)(readers.Count() == 0 ? -1 : readers.ElementAt(1).deviceModuleId),
                        (short)(readers.Count() == 0 ? -1 : readers.ElementAt(1).readerNo),
                        -1,
                        -1,
                        1,
                        5,
                        0,
                        -1,
                        -1,
                        1,
                        -1,
                        -1,
                        -1,
                        -1,
                        0,
                        0,
                        -1,
                        -1,
                        0,
                        255,
                        0,
                        -1,
                        1,
                        spare,
                        accessFlag,
                        doorMetadata.OfflineMode,
                        doorMetadata.DefaultMode,
                        doorMetadata.DefaultLedMode,
                        0,
                        0
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));

            }

            RelayMetadata? relayMeta = null;
            if (relay != null)
            {
                  relayMeta = JsonHelper.Deserialize<RelayMetadata>(relay.Value.metadata);
                  if (relayMeta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("RelayMetadata"));
            }

            SensorMetadata? sensorMeta = null;
            if (sensor != null)
            {
                  sensorMeta = JsonHelper.Deserialize<SensorMetadata>(sensor.Value.metadata);
                  if (sensorMeta == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("SensorMetadata"));
            }

            

            RexMetadata? rexMeta0 = null;
            RexMetadata? rexMeta1 = null;
            if(rexes.Count() == 1)
            {
                  rexMeta0 = JsonHelper.Deserialize<RexMetadata>(rexes.ElementAt(0).metadata);
                  if (rexMeta0 == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("RexMetadata"));
            }

            if (rexes.Count() == 2)
            {
                  rexMeta1 = JsonHelper.Deserialize<RexMetadata>(rexes.ElementAt(1).metadata);
                  if (rexMeta1 == null)
                        throw new Exception(MessageHelper.Common.DeserializeFailed("RexMetadata"));
            }


            var doorRes = door.AccessControlReaderConfiguration(
                  mac,
                  deviceId,
                  doorId.ElementAt(0),
                  (short)(type == DoorType.Single ? 0 : 1),
                  doorId.ElementAt(1),
                  (short)(readers.Count() == 0 ? -1 : readers.ElementAt(0).deviceModuleId),
                  (short)(readers.Count() == 0 ? -1 : readers.ElementAt(0).readerNo),
                  (short)(relay == null ? -1 : relay.Value.deviceModuleId),
                  (short)(relay == null ? -1 : relay.Value.outputNo),
                  (short)(relay == null || relayMeta == null ? 1 : relayMeta.RelayMin),
                  (short)(relay == null || relayMeta == null ? 5 : relayMeta.RelayMax),
                  (short)(relay == null || relayMeta == null ? 0 : relayMeta.RelayMode),
                  (short)(sensor == null ? -1 : sensor.Value.deviceModuleId),
                  (short)(sensor == null ? -1 : sensor.Value.inputNo),
                  (short)(sensor == null || sensorMeta == null ? 1 : sensorMeta.DcHeld),
                  (short)(rexes.Count() == 0 ? -1 : rexes.ElementAt(0).deviceModuleId),
                  (short)(rexes.Count() == 0 ? -1 : rexes.ElementAt(0).inputNo),
                  (short)(rexes.Count() <= 1 ? -1 : rexes.ElementAt(1).deviceModuleId),
                  (short)(rexes.Count() <= 1 ? -1 : rexes.ElementAt(1).inputNo),
                  (short)(rexMeta0 == null ? 0 : rexMeta0.MaskTime),
                  (short)(rexMeta1 == null ? 0 : rexMeta1.MaskTime),
                  -1,
                  -1,
                  0,
                  255,
                  0,
                  -1,
                  1,
                  spare,
                  accessFlag,
                  doorMetadata.OfflineMode,
                  doorMetadata.DefaultMode,
                  doorMetadata.DefaultLedMode,
                  0,
                  0
            );

            await bus.SendAsync(new AdapterEventCommand(doorRes));



      }

}