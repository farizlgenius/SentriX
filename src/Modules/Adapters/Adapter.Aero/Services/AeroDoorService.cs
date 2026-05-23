using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Constants;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using AeroAdapter.Application.Interfaces;
using Events.Contract.Command;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;


public sealed class AeroDoorService(IDoorCommand door, IOutputCommand output, IInputCommand input, IMessageBus bus) : IDoorAdapter
{
      public async Task CreateUpdateDoorAsync(
            string Mac,
            short DeviceComponentId,
            string Metadata,
            short FirstComponentId,
            short SecondComponentId = -1
      )
      {

            var metadata = JsonSerializer.Deserialize<DoorMetadata>(Metadata);
            if (metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("DoorMetadata"));

            // Setting Up Reader for both in/out readers

            // Below is Setting Reader In
            if (metadata.ReaderIn.ReaderModuleComponentId > -1)
            {
                  short readerInOsdpFlag = 0x00;
                  if (metadata.ReaderIn.OsdpFlag)
                  {
                        readerInOsdpFlag += metadata.ReaderIn.OsdpBaudrate;
                        readerInOsdpFlag |= metadata.ReaderIn.OsdpDiscover;
                        readerInOsdpFlag |= metadata.ReaderIn.OsdpTracing;
                        readerInOsdpFlag |= (short)(metadata.ReaderIn.OsdpAddress << 5);
                        readerInOsdpFlag |= metadata.ReaderIn.OsdpSecureChannel;
                        metadata.ReaderIn.LedDriveMode = 7;
                  }
                  else
                  {
                        metadata.ReaderIn.LedDriveMode = 1;
                  }

                  var res = door.ReaderSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.ReaderIn.ReaderModuleComponentId,
                        metadata.ReaderIn.ReaderNumber,
                        metadata.ReaderIn.DataFormat,
                        metadata.ReaderIn.KeypadMode,
                        metadata.ReaderIn.LedDriveMode,
                       readerInOsdpFlag
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ReaderSpecification, Mac, DeviceComponentId));
            }


            // Below is Setting Reader Out
            if (metadata.ReaderOut.ReaderModuleComponentId > -1)
            {
                  short readerOutOsdpFlag = 0x00;
                  if (metadata.ReaderOut.OsdpFlag)
                  {
                        readerOutOsdpFlag += metadata.ReaderOut.OsdpBaudrate;
                        readerOutOsdpFlag |= metadata.ReaderOut.OsdpDiscover;
                        readerOutOsdpFlag |= metadata.ReaderOut.OsdpTracing;
                        readerOutOsdpFlag |= (short)(metadata.ReaderOut.OsdpAddress << 5);
                        readerOutOsdpFlag |= metadata.ReaderOut.OsdpSecureChannel;
                        metadata.ReaderOut.LedDriveMode = 7;
                  }
                  else
                  {
                        metadata.ReaderOut.LedDriveMode = 1;
                  }

                  var res = door.ReaderSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.ReaderOut.ReaderModuleComponentId,
                        metadata.ReaderOut.ReaderNumber,
                        metadata.ReaderOut.DataFormat,
                        metadata.ReaderOut.KeypadMode,
                        metadata.ReaderOut.LedDriveMode,
                       readerOutOsdpFlag
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ReaderSpecification, Mac, DeviceComponentId));
            }


            // Output Spec
            if (metadata.Relay.RelayModuleComponentId > -1)
            {
                  var res = output.OutputPointSpecification(
                  Mac,
                  DeviceComponentId,
                  metadata.Relay.RelayModuleComponentId,
                  metadata.Relay.RelayNumber,
                  metadata.Relay.RelayMode
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, Mac, DeviceComponentId));
            }

            if (metadata.Sensor.SensorModuleComponentId > -1)
            {
                  // Input Spec
                  var res = input.InputPointSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.Sensor.SensorModuleComponentId,
                        metadata.Sensor.SensorNumber,
                        metadata.Sensor.SensorMode,
                        metadata.Sensor.Debounce,
                        metadata.Sensor.HoldTime
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, Mac, DeviceComponentId));
            }




            // Set Input Spec for Rex 0 / 1

            if (metadata.Rex.Rex0ModuleComponentId > -1)
            {
                  // Input Spec
                  var res = input.InputPointSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.Rex.Rex0ModuleComponentId,
                        metadata.Rex.Rex0Number,
                        metadata.Rex.Rex0SensorMode,
                        metadata.Rex.Rex0Debounce,
                        metadata.Rex.Rex0HoldTime
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, Mac, DeviceComponentId));
            }

            if (metadata.Rex.Rex1ModuleComponentId > -1)
            {
                  // Input Spec
                  var res = input.InputPointSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.Rex.Rex1ModuleComponentId,
                        metadata.Rex.Rex1Number,
                        metadata.Rex.Rex1SensorMode,
                        metadata.Rex.Rex1Debounce,
                        metadata.Rex.Rex1HoldTime
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, Mac, DeviceComponentId));
            }


            // In Case of Reader is Wiegand 
            if (metadata.ReaderIn.ReaderModuleComponentId > -1)
            {
                  var res = door.AccessControlReaderConfiguration(
                  Mac,
                  DeviceComponentId,
                  FirstComponentId,
                  metadata.AccessConfig,
                  SecondComponentId,
                  metadata.ReaderIn.ReaderModuleComponentId,
                  metadata.ReaderIn.ReaderNumber,
                  metadata.Relay.RelayModuleComponentId,
                  metadata.Relay.RelayNumber,
                  metadata.Relay.RelayMin,
                  metadata.Relay.RelayMax,
                  metadata.Relay.RelayMode,
                  metadata.Sensor.SensorModuleComponentId,
                  metadata.Sensor.SensorNumber,
                  metadata.Sensor.HeldOpenDelay,
                  metadata.Rex.Rex0ModuleComponentId,
                  metadata.Rex.Rex0Number,
                  metadata.Rex.Rex1ModuleComponentId,
                  metadata.Rex.Rex1Number,
                  metadata.Rex.DisableRex0Timezone,
                  metadata.Rex.DisableRex1Timezone,
                  metadata.AltrReader.AltrRdrModuleComponentId,
                  metadata.AltrReader.AltrRdrNumber,
                  metadata.AltrReader.AltrRdrConf,
                  metadata.Antipassback.AntipassbackMode,
                  metadata.Antipassback.AreaIn,
                  metadata.Antipassback.AreaOut,
                  metadata.Spare,
                  metadata.AccessControlFlag,
                  metadata.OfflineMode,
                  metadata.DefaultMode,
                  metadata.LedMode,
                  metadata.ApbDelay,
                  metadata.RelayT2,
                  metadata.HeldOpen2,
                  metadata.RelayFollowerPulse,
                  metadata.RelayFollowerDelay,
                  metadata.ExtendFeatureType,
                  metadata.InteriorPushButtonModuleComponentId,
                  metadata.InteriorPushButtonInputNumber,
                  metadata.InteriorPushButtonLongPress,
                  metadata.InteriorPushButtonOutModuleComponentId,
                  metadata.InteriorPushButtonOutRelayNumber
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, Mac, DeviceComponentId));
            }

            if (metadata.ReaderOut.ReaderModuleComponentId > -1)
            {
                  var res = door.AccessControlReaderConfiguration(
                  Mac,
                  DeviceComponentId,
                  SecondComponentId,
                  2,
                  FirstComponentId,
                  metadata.ReaderIn.ReaderModuleComponentId,
                  metadata.ReaderIn.ReaderNumber,
                  -1,
                  metadata.Relay.RelayNumber,
                  metadata.Relay.RelayMin,
                  metadata.Relay.RelayMax,
                  metadata.Relay.RelayMode,
                  -1,
                  metadata.Sensor.SensorNumber,
                  metadata.Sensor.HeldOpenDelay,
                  -1,
                  metadata.Rex.Rex0Number,
                  -1,
                  metadata.Rex.Rex1Number,
                  metadata.Rex.DisableRex0Timezone,
                  metadata.Rex.DisableRex1Timezone,
                  -1,
                  metadata.AltrReader.AltrRdrNumber,
                  metadata.AltrReader.AltrRdrConf,
                  metadata.Antipassback.AntipassbackMode,
                  metadata.Antipassback.AreaIn,
                  metadata.Antipassback.AreaOut,
                  metadata.Spare,
                  metadata.AccessControlFlag,
                  metadata.OfflineMode,
                  metadata.DefaultMode,
                  metadata.LedMode,
                  metadata.ApbDelay,
                  metadata.RelayT2,
                  metadata.HeldOpen2,
                  metadata.RelayFollowerPulse,
                  metadata.RelayFollowerDelay,
                  metadata.ExtendFeatureType,
                  metadata.InteriorPushButtonModuleComponentId,
                  metadata.InteriorPushButtonInputNumber,
                  metadata.InteriorPushButtonLongPress,
                  metadata.InteriorPushButtonOutModuleComponentId,
                  metadata.InteriorPushButtonOutRelayNumber
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, Mac, DeviceComponentId));
            }



      }

      public async Task DeleteDoorAsync(
            string Mac,
            short DeviceComponentId,
            string Metadata,
            short FirstComponentId,
            short SecondComponentId = -1
      )
      {
            var metadata = JsonSerializer.Deserialize<DoorMetadata>(Metadata);
            if (metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("DoorMetadata"));



            // In Case of Reader is Wiegand 
            if (metadata.ReaderIn.ReaderModuleComponentId > -1)
            {
                  var res = door.AccessControlReaderConfiguration(
                  Mac,
                  DeviceComponentId,
                  FirstComponentId,
                  metadata.AccessConfig,
                  SecondComponentId,
                  -1,
                  metadata.ReaderIn.ReaderNumber,
                  -1,
                  metadata.Relay.RelayNumber,
                  metadata.Relay.RelayMin,
                  metadata.Relay.RelayMax,
                  metadata.Relay.RelayMode,
                  -1,
                  metadata.Sensor.SensorNumber,
                  metadata.Sensor.HeldOpenDelay,
                  -1,
                  metadata.Rex.Rex0Number,
                  -1,
                  metadata.Rex.Rex1Number,
                  metadata.Rex.DisableRex0Timezone,
                  metadata.Rex.DisableRex1Timezone,
                  -1,
                  metadata.AltrReader.AltrRdrNumber,
                  metadata.AltrReader.AltrRdrConf,
                  metadata.Antipassback.AntipassbackMode,
                  metadata.Antipassback.AreaIn,
                  metadata.Antipassback.AreaOut,
                  metadata.Spare,
                  metadata.AccessControlFlag,
                  metadata.OfflineMode,
                  metadata.DefaultMode,
                  metadata.LedMode,
                  metadata.ApbDelay,
                  metadata.RelayT2,
                  metadata.HeldOpen2,
                  metadata.RelayFollowerPulse,
                  metadata.RelayFollowerDelay,
                  metadata.ExtendFeatureType,
                  metadata.InteriorPushButtonModuleComponentId,
                  metadata.InteriorPushButtonInputNumber,
                  metadata.InteriorPushButtonLongPress,
                  metadata.InteriorPushButtonOutModuleComponentId,
                  metadata.InteriorPushButtonOutRelayNumber
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, Mac, DeviceComponentId));
            }

            if (metadata.ReaderOut.ReaderModuleComponentId > -1)
            {
                  var res = door.AccessControlReaderConfiguration(
                  Mac,
                  DeviceComponentId,
                  SecondComponentId,
                  2,
                  FirstComponentId,
                  -1,
                  metadata.ReaderIn.ReaderNumber,
                  -1,
                  metadata.Relay.RelayNumber,
                  metadata.Relay.RelayMin,
                  metadata.Relay.RelayMax,
                  metadata.Relay.RelayMode,
                  -1,
                  metadata.Sensor.SensorNumber,
                  metadata.Sensor.HeldOpenDelay,
                  -1,
                  metadata.Rex.Rex0Number,
                  -1,
                  metadata.Rex.Rex1Number,
                  metadata.Rex.DisableRex0Timezone,
                  metadata.Rex.DisableRex1Timezone,
                  -1,
                  metadata.AltrReader.AltrRdrNumber,
                  metadata.AltrReader.AltrRdrConf,
                  metadata.Antipassback.AntipassbackMode,
                  metadata.Antipassback.AreaIn,
                  metadata.Antipassback.AreaOut,
                  metadata.Spare,
                  metadata.AccessControlFlag,
                  metadata.OfflineMode,
                  metadata.DefaultMode,
                  metadata.LedMode,
                  metadata.ApbDelay,
                  metadata.RelayT2,
                  metadata.HeldOpen2,
                  metadata.RelayFollowerPulse,
                  metadata.RelayFollowerDelay,
                  metadata.ExtendFeatureType,
                  metadata.InteriorPushButtonModuleComponentId,
                  metadata.InteriorPushButtonInputNumber,
                  metadata.InteriorPushButtonLongPress,
                  metadata.InteriorPushButtonOutModuleComponentId,
                  metadata.InteriorPushButtonOutRelayNumber
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, Mac, DeviceComponentId));
            }
      }

      public async Task UpdateDoorAsync(string Mac, short DeviceComponentId, string Metadata, short FirstComponentId, short SecondComponentId = -1)
      {
            var metadata = JsonSerializer.Deserialize<DoorMetadata>(Metadata);
            if (metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("DoorMetadata"));

            // Setting Up Reader for both in/out readers

            // Below is Setting Reader In
            if (metadata.ReaderIn.ReaderModuleComponentId > -1)
            {
                  short readerInOsdpFlag = 0x00;
                  if (metadata.ReaderIn.OsdpFlag)
                  {
                        readerInOsdpFlag += metadata.ReaderIn.OsdpBaudrate;
                        readerInOsdpFlag |= metadata.ReaderIn.OsdpDiscover;
                        readerInOsdpFlag |= metadata.ReaderIn.OsdpTracing;
                        readerInOsdpFlag |= (short)(metadata.ReaderIn.OsdpAddress << 5);
                        readerInOsdpFlag |= metadata.ReaderIn.OsdpSecureChannel;
                        metadata.ReaderIn.LedDriveMode = 7;
                  }
                  else
                  {
                        metadata.ReaderIn.LedDriveMode = 1;
                  }

                  var res = door.ReaderSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.ReaderIn.ReaderModuleComponentId,
                        metadata.ReaderIn.ReaderNumber,
                        metadata.ReaderIn.DataFormat,
                        metadata.ReaderIn.KeypadMode,
                        metadata.ReaderIn.LedDriveMode,
                       readerInOsdpFlag
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ReaderSpecification, Mac, DeviceComponentId));
            }


            // Below is Setting Reader Out
            if (metadata.ReaderOut.ReaderModuleComponentId > -1)
            {
                  short readerOutOsdpFlag = 0x00;
                  if (metadata.ReaderOut.OsdpFlag)
                  {
                        readerOutOsdpFlag += metadata.ReaderOut.OsdpBaudrate;
                        readerOutOsdpFlag |= metadata.ReaderOut.OsdpDiscover;
                        readerOutOsdpFlag |= metadata.ReaderOut.OsdpTracing;
                        readerOutOsdpFlag |= (short)(metadata.ReaderOut.OsdpAddress << 5);
                        readerOutOsdpFlag |= metadata.ReaderOut.OsdpSecureChannel;
                        metadata.ReaderOut.LedDriveMode = 7;
                  }
                  else
                  {
                        metadata.ReaderOut.LedDriveMode = 1;
                  }

                  var res = door.ReaderSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.ReaderOut.ReaderModuleComponentId,
                        metadata.ReaderOut.ReaderNumber,
                        metadata.ReaderOut.DataFormat,
                        metadata.ReaderOut.KeypadMode,
                        metadata.ReaderOut.LedDriveMode,
                       readerOutOsdpFlag
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ReaderSpecification, Mac, DeviceComponentId));
            }


            // Output Spec
            if (metadata.Relay.RelayModuleComponentId > -1)
            {
                  var res = output.OutputPointSpecification(
                  Mac,
                  DeviceComponentId,
                  metadata.Relay.RelayModuleComponentId,
                  metadata.Relay.RelayNumber,
                  metadata.Relay.RelayMode
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, Mac, DeviceComponentId));
            }

            if (metadata.Sensor.SensorModuleComponentId > -1)
            {
                  // Input Spec
                  var res = input.InputPointSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.Sensor.SensorModuleComponentId,
                        metadata.Sensor.SensorNumber,
                        metadata.Sensor.SensorMode,
                        metadata.Sensor.Debounce,
                        metadata.Sensor.HoldTime
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, Mac, DeviceComponentId));
            }




            // Set Input Spec for Rex 0 / 1

            if (metadata.Rex.Rex0ModuleComponentId > -1)
            {
                  // Input Spec
                  var res = input.InputPointSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.Rex.Rex0ModuleComponentId,
                        metadata.Rex.Rex0Number,
                        metadata.Rex.Rex0SensorMode,
                        metadata.Rex.Rex0Debounce,
                        metadata.Rex.Rex0HoldTime
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, Mac, DeviceComponentId));
            }

            if (metadata.Rex.Rex1ModuleComponentId > -1)
            {
                  // Input Spec
                  var res = input.InputPointSpecification(
                        Mac,
                        DeviceComponentId,
                        metadata.Rex.Rex1ModuleComponentId,
                        metadata.Rex.Rex1Number,
                        metadata.Rex.Rex1SensorMode,
                        metadata.Rex.Rex1Debounce,
                        metadata.Rex.Rex1HoldTime
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, Mac, DeviceComponentId));
            }


            // In Case of Reader is Wiegand 
            if (metadata.ReaderIn.ReaderModuleComponentId > -1)
            {
                  var res = door.AccessControlReaderConfiguration(
                  Mac,
                  DeviceComponentId,
                  FirstComponentId,
                  metadata.AccessConfig,
                  SecondComponentId,
                  metadata.ReaderIn.ReaderModuleComponentId,
                  metadata.ReaderIn.ReaderNumber,
                  metadata.Relay.RelayModuleComponentId,
                  metadata.Relay.RelayNumber,
                  metadata.Relay.RelayMin,
                  metadata.Relay.RelayMax,
                  metadata.Relay.RelayMode,
                  metadata.Sensor.SensorModuleComponentId,
                  metadata.Sensor.SensorNumber,
                  metadata.Sensor.HeldOpenDelay,
                  metadata.Rex.Rex0ModuleComponentId,
                  metadata.Rex.Rex0Number,
                  metadata.Rex.Rex1ModuleComponentId,
                  metadata.Rex.Rex1Number,
                  metadata.Rex.DisableRex0Timezone,
                  metadata.Rex.DisableRex1Timezone,
                  metadata.AltrReader.AltrRdrModuleComponentId,
                  metadata.AltrReader.AltrRdrNumber,
                  metadata.AltrReader.AltrRdrConf,
                  metadata.Antipassback.AntipassbackMode,
                  metadata.Antipassback.AreaIn,
                  metadata.Antipassback.AreaOut,
                  metadata.Spare,
                  metadata.AccessControlFlag,
                  metadata.OfflineMode,
                  metadata.DefaultMode,
                  metadata.LedMode,
                  metadata.ApbDelay,
                  metadata.RelayT2,
                  metadata.HeldOpen2,
                  metadata.RelayFollowerPulse,
                  metadata.RelayFollowerDelay,
                  metadata.ExtendFeatureType,
                  metadata.InteriorPushButtonModuleComponentId,
                  metadata.InteriorPushButtonInputNumber,
                  metadata.InteriorPushButtonLongPress,
                  metadata.InteriorPushButtonOutModuleComponentId,
                  metadata.InteriorPushButtonOutRelayNumber
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, Mac, DeviceComponentId));
            }

            if (metadata.ReaderOut.ReaderModuleComponentId > -1)
            {
                  var res = door.AccessControlReaderConfiguration(
                  Mac,
                  DeviceComponentId,
                  SecondComponentId,
                  2,
                  FirstComponentId,
                  metadata.ReaderIn.ReaderModuleComponentId,
                  metadata.ReaderIn.ReaderNumber,
                  -1,
                  metadata.Relay.RelayNumber,
                  metadata.Relay.RelayMin,
                  metadata.Relay.RelayMax,
                  metadata.Relay.RelayMode,
                  -1,
                  metadata.Sensor.SensorNumber,
                  metadata.Sensor.HeldOpenDelay,
                  -1,
                  metadata.Rex.Rex0Number,
                  -1,
                  metadata.Rex.Rex1Number,
                  metadata.Rex.DisableRex0Timezone,
                  metadata.Rex.DisableRex1Timezone,
                  -1,
                  metadata.AltrReader.AltrRdrNumber,
                  metadata.AltrReader.AltrRdrConf,
                  metadata.Antipassback.AntipassbackMode,
                  metadata.Antipassback.AreaIn,
                  metadata.Antipassback.AreaOut,
                  metadata.Spare,
                  metadata.AccessControlFlag,
                  metadata.OfflineMode,
                  metadata.DefaultMode,
                  metadata.LedMode,
                  metadata.ApbDelay,
                  metadata.RelayT2,
                  metadata.HeldOpen2,
                  metadata.RelayFollowerPulse,
                  metadata.RelayFollowerDelay,
                  metadata.ExtendFeatureType,
                  metadata.InteriorPushButtonModuleComponentId,
                  metadata.InteriorPushButtonInputNumber,
                  metadata.InteriorPushButtonLongPress,
                  metadata.InteriorPushButtonOutModuleComponentId,
                  metadata.InteriorPushButtonOutRelayNumber
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  if (!res.IsSend)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, Mac, DeviceComponentId));
            }
      }
}