// using System.Text.Json;
// using Adapter.Abstraction.Interfaces;
// using Adapter.Aero.Constants;
// using Adapter.Aero.Helpers;
// using Adapter.Aero.Interfaces;
// using Adapter.Aero.Model.Metadata;
// using Adapter.Aero.Persistences.Entities;
// using AeroAdapter.Application.Interfaces;
// using Door.Contract.DTOs;
// using Events.Contract.Command;
// using SharedKernel.Helpers;
// using SharedKernel.Messaging;

// namespace Adapter.Aero.Adapters;


// public sealed class AeroDoorAdapter(
//       IDoorCommand door, 
//       IOutputCommand output, 
//       IInputCommand input, 
//       IMessageBus bus,
//       IAeroRepository repo
//       ) : IAeroDoorAdapter
// {
//       public async Task CreateAsync(
//             Guid DeviceGuid,
//             Guid DoorGuid,
//             string Metadata
//       )
//       {
//             short FirstSlot = -1;
//             short SecondSlot = -1;
//             var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
//             var metadata = JsonHelper.Deserialize<DoorMetadata>(Metadata);
//             if (metadata == null)
//                   throw new Exception(MessageHelper.Common.DeserializeFailed("DoorMetadata"));

//             // Setting Up Reader for both in/out readers

//             // Below is Setting Reader In
//             if (metadata.ReaderIn.ReaderModuleComponentId > -1)
//             {
//                   FirstSlot = (short)await repo.GetFreeSlotAsync<AcrSlot>(DeviceGuid);
//                   short readerInOsdpFlag = 0x00;
//                   if (metadata.ReaderIn.OsdpFlag)
//                   {
//                         readerInOsdpFlag += metadata.ReaderIn.OsdpBaudrate;
//                         readerInOsdpFlag |= metadata.ReaderIn.OsdpDiscover;
//                         readerInOsdpFlag |= metadata.ReaderIn.OsdpTracing;
//                         readerInOsdpFlag |= (short)(metadata.ReaderIn.OsdpAddress << 5);
//                         readerInOsdpFlag |= metadata.ReaderIn.OsdpSecureChannel;
//                         metadata.ReaderIn.LedDriveMode = 7;
//                   }
//                   else
//                   {
//                         metadata.ReaderIn.LedDriveMode = 1;
//                   }

//                   var res = door.ReaderSpecification(
//                         deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.ReaderIn.ReaderModuleComponentId,
//                         metadata.ReaderIn.ReaderNumber,
//                         metadata.ReaderIn.DataFormat == -1 ? (short)0x01 : metadata.ReaderIn.DataFormat,
//                         metadata.ReaderIn.KeypadMode,
//                         metadata.ReaderIn.LedDriveMode,
//                        readerInOsdpFlag
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ReaderSpecification, deviceSlot.mac,deviceSlot.slot_id));
//             }


//             // Below is Setting Reader Out
//             if (metadata.ReaderOut.ReaderModuleComponentId > -1)
//             {
//                   SecondSlot = (short)await repo.GetFreeSlotAsync<AcrSlot>(DeviceGuid,FirstSlot);
//                   short readerOutOsdpFlag = 0x00;
//                   if (metadata.ReaderOut.OsdpFlag)
//                   {
//                         readerOutOsdpFlag += metadata.ReaderOut.OsdpBaudrate;
//                         readerOutOsdpFlag |= metadata.ReaderOut.OsdpDiscover;
//                         readerOutOsdpFlag |= metadata.ReaderOut.OsdpTracing;
//                         readerOutOsdpFlag |= (short)(metadata.ReaderOut.OsdpAddress << 5);
//                         readerOutOsdpFlag |= metadata.ReaderOut.OsdpSecureChannel;
//                         metadata.ReaderOut.LedDriveMode = 7;
//                   }
//                   else
//                   {
//                         metadata.ReaderOut.LedDriveMode = 1;
//                   }

//                   var res = door.ReaderSpecification(
//                         deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.ReaderOut.ReaderModuleComponentId,
//                         metadata.ReaderOut.ReaderNumber,
//                          metadata.ReaderOut.DataFormat == -1 ? (short)0x01 : metadata.ReaderOut.DataFormat,
//                         metadata.ReaderOut.KeypadMode,
//                         metadata.ReaderOut.LedDriveMode,
//                        readerOutOsdpFlag
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ReaderSpecification, deviceSlot.mac,deviceSlot.slot_id));
//             }


//             // Output Spec
//             if (metadata.Relay.RelayModuleComponentId > -1)
//             {
//                   var res = output.OutputPointSpecification(
//                   deviceSlot.mac,
//                   (short)deviceSlot.slot_id,
//                   metadata.Relay.RelayModuleComponentId,
//                   metadata.Relay.RelayNumber,
//                   OutputHelper.FinalizeOutputMode(metadata.Relay.DriveMode,metadata.Relay.OfflineMode)
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, deviceSlot.mac,deviceSlot.slot_id));
//             }

//             if (metadata.Sensor.SensorModuleComponentId > -1)
//             {
//                   // Input Spec
//                   var res = input.InputPointSpecification(
//                         deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.Sensor.SensorModuleComponentId,
//                         metadata.Sensor.SensorNumber,
//                         metadata.Sensor.SensorMode,
//                         metadata.Sensor.Debounce,
//                         metadata.Sensor.HoldTime
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification,deviceSlot.mac,deviceSlot.slot_id));
//             }




//             // Set Input Spec for Rex 0 / 1

//             if (metadata.Rex.Rex0ModuleComponentId > -1)
//             {
//                   // Input Spec
//                   var res = input.InputPointSpecification(
//                         deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.Rex.Rex0ModuleComponentId,
//                         metadata.Rex.Rex0Number,
//                         metadata.Rex.Rex0SensorMode,
//                         metadata.Rex.Rex0Debounce,
//                         metadata.Rex.Rex0HoldTime
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, deviceSlot.mac,deviceSlot.slot_id));
//             }

//             if (metadata.Rex.Rex1ModuleComponentId > -1)
//             {
//                   // Input Spec
//                   var res = input.InputPointSpecification(
//                         deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.Rex.Rex1ModuleComponentId,
//                         metadata.Rex.Rex1Number,
//                         metadata.Rex.Rex1SensorMode,
//                         metadata.Rex.Rex1Debounce,
//                         metadata.Rex.Rex1HoldTime
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification,deviceSlot.mac,deviceSlot.slot_id));
//             }


//             // In Case of Reader is Wiegand 
//             if (metadata.ReaderIn.ReaderModuleComponentId > -1)
//             {
//                   var res = door.AccessControlReaderConfiguration(
//                  deviceSlot.mac,
//                   (short)deviceSlot.slot_id,
//                   FirstSlot,
//                   metadata.AccessConfig,
//                   SecondSlot,
//                   metadata.ReaderIn.ReaderModuleComponentId,
//                   metadata.ReaderIn.ReaderNumber,
//                   metadata.Relay.RelayModuleComponentId,
//                   metadata.Relay.RelayNumber,
//                   metadata.Relay.RelayMin,
//                   metadata.Relay.RelayMax,
//                   metadata.Relay.DriveMode,
//                   metadata.Sensor.SensorModuleComponentId,
//                   metadata.Sensor.SensorNumber,
//                   metadata.Sensor.HeldOpenDelay,
//                   metadata.Rex.Rex0ModuleComponentId,
//                   metadata.Rex.Rex0Number,
//                   metadata.Rex.Rex1ModuleComponentId,
//                   metadata.Rex.Rex1Number,
//                   metadata.Rex.DisableRex0Timezone,
//                   metadata.Rex.DisableRex1Timezone,
//                   metadata.AltrReader.AltrRdrModuleComponentId,
//                   metadata.AltrReader.AltrRdrNumber,
//                   metadata.AltrReader.AltrRdrConf,
//                   metadata.Antipassback.AntipassbackMode,
//                   metadata.Antipassback.AreaIn,
//                   metadata.Antipassback.AreaOut,
//                   metadata.Spare,
//                   metadata.AccessControlFlag,
//                   metadata.OfflineMode,
//                   metadata.DefaultMode,
//                   metadata.LedMode,
//                   metadata.ApbDelay,
//                   metadata.RelayT2,
//                   metadata.HeldOpen2,
//                   metadata.RelayFollowerPulse,
//                   metadata.RelayFollowerDelay,
//                   metadata.ExtendFeatureType,
//                   metadata.InteriorPushButtonModuleComponentId,
//                   metadata.InteriorPushButtonInputNumber,
//                   metadata.InteriorPushButtonLongPress,
//                   metadata.InteriorPushButtonOutModuleComponentId,
//                   metadata.InteriorPushButtonOutRelayNumber
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, deviceSlot.mac,deviceSlot.slot_id));

//                   await repo.InsertSlotAsync<AcrSlot>(DeviceGuid,DoorGuid,FirstSlot);
//             }

//             if (metadata.ReaderOut.ReaderModuleComponentId > -1)
//             {
//                   var res = door.AccessControlReaderConfiguration(
//                   deviceSlot.mac,
//                   (short)deviceSlot.slot_id,
//                   FirstSlot,
//                   2,
//                   SecondSlot,
//                   metadata.ReaderOut.ReaderModuleComponentId,
//                   metadata.ReaderOut.ReaderNumber,
//                   -1,
//                   -1,
//                   metadata.Relay.RelayMin,
//                   metadata.Relay.RelayMax,
//                   metadata.Relay.DriveMode,
//                   -1,
//                   -1,
//                   metadata.Sensor.HeldOpenDelay,
//                   -1,
//                   -1,
//                   -1,
//                   -1,
//                   -1,
//                   -1,
//                   -1,
//                   metadata.AltrReader.AltrRdrNumber,
//                   metadata.AltrReader.AltrRdrConf,
//                   metadata.Antipassback.AntipassbackMode,
//                   metadata.Antipassback.AreaIn,
//                   metadata.Antipassback.AreaOut,
//                   metadata.Spare,
//                   metadata.AccessControlFlag,
//                   metadata.OfflineMode,
//                   metadata.DefaultMode,
//                   metadata.LedMode,
//                   metadata.ApbDelay,
//                   metadata.RelayT2,
//                   metadata.HeldOpen2,
//                   metadata.RelayFollowerPulse,
//                   metadata.RelayFollowerDelay,
//                   metadata.ExtendFeatureType,
//                   metadata.InteriorPushButtonModuleComponentId,
//                   metadata.InteriorPushButtonInputNumber,
//                   metadata.InteriorPushButtonLongPress,
//                   metadata.InteriorPushButtonOutModuleComponentId,
//                   metadata.InteriorPushButtonOutRelayNumber
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, deviceSlot.mac,deviceSlot.slot_id));

//                   await repo.InsertSlotAsync<AcrSlot>(DeviceGuid,DoorGuid,SecondSlot);
//             }

            

//       }

      
//       public async Task DeleteAsync(
//             Guid DeviceGuid,
//             Guid DoorGuid,
//             string Metadata
//       )
//       {
//             short FirstSlot = -1;
//             short SecondSlot = -1;
//             var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
//             var doorSlot = await repo.GetSlotIdsByGuidAsync<AcrSlot>(DoorGuid);

//             if(doorSlot.Count() > 0)
//                   FirstSlot = (short)doorSlot.ElementAt(0);
            
//              if(doorSlot.Count() > 1)
//                   SecondSlot = (short)doorSlot.ElementAt(1);
            
            
//             var metadata = JsonSerializer.Deserialize<DoorMetadata>(Metadata);
//             if (metadata == null)
//                   throw new Exception(MessageHelper.Common.DeserializeFailed("DoorMetadata"));



//             // In Case of Reader is Wiegand 
//             if (metadata.ReaderIn.ReaderModuleComponentId > -1)
//             {
//                   var res = door.AccessControlReaderConfiguration(
//                   deviceSlot.mac,
//                   (short)deviceSlot.slot_id,
//                   FirstSlot,
//                   metadata.AccessConfig,
//                   SecondSlot,
//                   -1,
//                   metadata.ReaderIn.ReaderNumber,
//                   -1,
//                   metadata.Relay.RelayNumber,
//                   metadata.Relay.RelayMin,
//                   metadata.Relay.RelayMax,
//                   metadata.Relay.DriveMode,
//                   -1,
//                   metadata.Sensor.SensorNumber,
//                   metadata.Sensor.HeldOpenDelay,
//                   -1,
//                   metadata.Rex.Rex0Number,
//                   -1,
//                   metadata.Rex.Rex1Number,
//                   metadata.Rex.DisableRex0Timezone,
//                   metadata.Rex.DisableRex1Timezone,
//                   -1,
//                   metadata.AltrReader.AltrRdrNumber,
//                   metadata.AltrReader.AltrRdrConf,
//                   metadata.Antipassback.AntipassbackMode,
//                   metadata.Antipassback.AreaIn,
//                   metadata.Antipassback.AreaOut,
//                   metadata.Spare,
//                   metadata.AccessControlFlag,
//                   metadata.OfflineMode,
//                   metadata.DefaultMode,
//                   metadata.LedMode,
//                   metadata.ApbDelay,
//                   metadata.RelayT2,
//                   metadata.HeldOpen2,
//                   metadata.RelayFollowerPulse,
//                   metadata.RelayFollowerDelay,
//                   metadata.ExtendFeatureType,
//                   metadata.InteriorPushButtonModuleComponentId,
//                   metadata.InteriorPushButtonInputNumber,
//                   metadata.InteriorPushButtonLongPress,
//                   metadata.InteriorPushButtonOutModuleComponentId,
//                   metadata.InteriorPushButtonOutRelayNumber
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, deviceSlot.mac,deviceSlot.slot_id));

//                   await repo.EjectSlotAsync<AcrSlot>(DeviceGuid,FirstSlot);
//             }

//             if (metadata.ReaderOut.ReaderModuleComponentId > -1)
//             {
//                   var res = door.AccessControlReaderConfiguration(
//                   deviceSlot.mac,
//                   (short)deviceSlot.slot_id,
//                   SecondSlot,
//                   2,
//                  FirstSlot,
//                   -1,
//                   metadata.ReaderIn.ReaderNumber,
//                   -1,
//                   metadata.Relay.RelayNumber,
//                   metadata.Relay.RelayMin,
//                   metadata.Relay.RelayMax,
//                   metadata.Relay.DriveMode,
//                   -1,
//                   metadata.Sensor.SensorNumber,
//                   metadata.Sensor.HeldOpenDelay,
//                   -1,
//                   metadata.Rex.Rex0Number,
//                   -1,
//                   metadata.Rex.Rex1Number,
//                   metadata.Rex.DisableRex0Timezone,
//                   metadata.Rex.DisableRex1Timezone,
//                   -1,
//                   metadata.AltrReader.AltrRdrNumber,
//                   metadata.AltrReader.AltrRdrConf,
//                   metadata.Antipassback.AntipassbackMode,
//                   metadata.Antipassback.AreaIn,
//                   metadata.Antipassback.AreaOut,
//                   metadata.Spare,
//                   metadata.AccessControlFlag,
//                   metadata.OfflineMode,
//                   metadata.DefaultMode,
//                   metadata.LedMode,
//                   metadata.ApbDelay,
//                   metadata.RelayT2,
//                   metadata.HeldOpen2,
//                   metadata.RelayFollowerPulse,
//                   metadata.RelayFollowerDelay,
//                   metadata.ExtendFeatureType,
//                   metadata.InteriorPushButtonModuleComponentId,
//                   metadata.InteriorPushButtonInputNumber,
//                   metadata.InteriorPushButtonLongPress,
//                   metadata.InteriorPushButtonOutModuleComponentId,
//                   metadata.InteriorPushButtonOutRelayNumber
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, deviceSlot.mac, deviceSlot.slot_id));

//                   await repo.EjectSlotAsync<AcrSlot>(DeviceGuid,SecondSlot);

//             }

    
//       }

//       public async Task UpdateAsync(
//             Guid DeviceGuid,
//             Guid DoorGuid,
//             string Metadata
//             )
//       {
//             var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
//             short FirstSlot = -1;
//             short SecondSlot = -1;
//             var doorSlot = await repo.GetSlotIdsByGuidAsync<AcrSlot>(DoorGuid);

//             if(doorSlot.Count() > 0)
//                   FirstSlot = (short)doorSlot.ElementAt(0);
            
//              if(doorSlot.Count() > 1)
//                   SecondSlot = (short)doorSlot.ElementAt(1);

//             var metadata = JsonSerializer.Deserialize<DoorMetadata>(Metadata);
//             if (metadata == null)
//                   throw new Exception(MessageHelper.Common.DeserializeFailed("DoorMetadata"));

//             // Setting Up Reader for both in/out readers

//             // Below is Setting Reader In
//             if (metadata.ReaderIn.ReaderModuleComponentId > -1)
//             {
//                   short readerInOsdpFlag = 0x00;
//                   if (metadata.ReaderIn.OsdpFlag)
//                   {
//                         readerInOsdpFlag += metadata.ReaderIn.OsdpBaudrate;
//                         readerInOsdpFlag |= metadata.ReaderIn.OsdpDiscover;
//                         readerInOsdpFlag |= metadata.ReaderIn.OsdpTracing;
//                         readerInOsdpFlag |= (short)(metadata.ReaderIn.OsdpAddress << 5);
//                         readerInOsdpFlag |= metadata.ReaderIn.OsdpSecureChannel;
//                         metadata.ReaderIn.LedDriveMode = 7;
//                   }
//                   else
//                   {
//                         metadata.ReaderIn.LedDriveMode = 1;
//                   }

//                   var res = door.ReaderSpecification(
//                         deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.ReaderIn.ReaderModuleComponentId,
//                         metadata.ReaderIn.ReaderNumber,
//                         metadata.ReaderIn.DataFormat,
//                         metadata.ReaderIn.KeypadMode,
//                         metadata.ReaderIn.LedDriveMode,
//                        readerInOsdpFlag
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ReaderSpecification, deviceSlot.mac, deviceSlot.slot_id));
//             }


//             // Below is Setting Reader Out
//             if (metadata.ReaderOut.ReaderModuleComponentId > -1)
//             {
//                   short readerOutOsdpFlag = 0x00;
//                   if (metadata.ReaderOut.OsdpFlag)
//                   {
//                         readerOutOsdpFlag += metadata.ReaderOut.OsdpBaudrate;
//                         readerOutOsdpFlag |= metadata.ReaderOut.OsdpDiscover;
//                         readerOutOsdpFlag |= metadata.ReaderOut.OsdpTracing;
//                         readerOutOsdpFlag |= (short)(metadata.ReaderOut.OsdpAddress << 5);
//                         readerOutOsdpFlag |= metadata.ReaderOut.OsdpSecureChannel;
//                         metadata.ReaderOut.LedDriveMode = 7;
//                   }
//                   else
//                   {
//                         metadata.ReaderOut.LedDriveMode = 1;
//                   }

//                   var res = door.ReaderSpecification(
//                         deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.ReaderOut.ReaderModuleComponentId,
//                         metadata.ReaderOut.ReaderNumber,
//                         metadata.ReaderOut.DataFormat,
//                         metadata.ReaderOut.KeypadMode,
//                         metadata.ReaderOut.LedDriveMode,
//                        readerOutOsdpFlag
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.ReaderSpecification, deviceSlot.mac, deviceSlot.slot_id));
//             }


//             // Output Spec
//             if (metadata.Relay.RelayModuleComponentId > -1)
//             {
//                   var res = output.OutputPointSpecification(
//                    deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                   metadata.Relay.RelayModuleComponentId,
//                   metadata.Relay.RelayNumber,
//                   metadata.Relay.DriveMode
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, deviceSlot.mac, deviceSlot.slot_id));
//             }

//             if (metadata.Sensor.SensorModuleComponentId > -1)
//             {
//                   // Input Spec
//                   var res = input.InputPointSpecification(
//                          deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.Sensor.SensorModuleComponentId,
//                         metadata.Sensor.SensorNumber,
//                         metadata.Sensor.SensorMode,
//                         metadata.Sensor.Debounce,
//                         metadata.Sensor.HoldTime
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, deviceSlot.mac, deviceSlot.slot_id));
//             }




//             // Set Input Spec for Rex 0 / 1

//             if (metadata.Rex.Rex0ModuleComponentId > -1)
//             {
//                   // Input Spec
//                   var res = input.InputPointSpecification(
//                          deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.Rex.Rex0ModuleComponentId,
//                         metadata.Rex.Rex0Number,
//                         metadata.Rex.Rex0SensorMode,
//                         metadata.Rex.Rex0Debounce,
//                         metadata.Rex.Rex0HoldTime
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification,deviceSlot.mac, deviceSlot.slot_id));
//             }

//             if (metadata.Rex.Rex1ModuleComponentId > -1)
//             {
//                   // Input Spec
//                   var res = input.InputPointSpecification(
//                          deviceSlot.mac,
//                         (short)deviceSlot.slot_id,
//                         metadata.Rex.Rex1ModuleComponentId,
//                         metadata.Rex.Rex1Number,
//                         metadata.Rex.Rex1SensorMode,
//                         metadata.Rex.Rex1Debounce,
//                         metadata.Rex.Rex1HoldTime
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.OutputPointSpecification, deviceSlot.mac, deviceSlot.slot_id));
//             }


//             // In Case of Reader is Wiegand 
//             if (metadata.ReaderIn.ReaderModuleComponentId > -1)
//             {
//                   var res = door.AccessControlReaderConfiguration(
//                    deviceSlot.mac,
//                   (short)deviceSlot.slot_id,
//                   FirstSlot,
//                   metadata.AccessConfig,
//                   SecondSlot,
//                   metadata.ReaderIn.ReaderModuleComponentId,
//                   metadata.ReaderIn.ReaderNumber,
//                   metadata.Relay.RelayModuleComponentId,
//                   metadata.Relay.RelayNumber,
//                   metadata.Relay.RelayMin,
//                   metadata.Relay.RelayMax,
//                   metadata.Relay.DriveMode,
//                   metadata.Sensor.SensorModuleComponentId,
//                   metadata.Sensor.SensorNumber,
//                   metadata.Sensor.HeldOpenDelay,
//                   metadata.Rex.Rex0ModuleComponentId,
//                   metadata.Rex.Rex0Number,
//                   metadata.Rex.Rex1ModuleComponentId,
//                   metadata.Rex.Rex1Number,
//                   metadata.Rex.DisableRex0Timezone,
//                   metadata.Rex.DisableRex1Timezone,
//                   metadata.AltrReader.AltrRdrModuleComponentId,
//                   metadata.AltrReader.AltrRdrNumber,
//                   metadata.AltrReader.AltrRdrConf,
//                   metadata.Antipassback.AntipassbackMode,
//                   metadata.Antipassback.AreaIn,
//                   metadata.Antipassback.AreaOut,
//                   metadata.Spare,
//                   metadata.AccessControlFlag,
//                   metadata.OfflineMode,
//                   metadata.DefaultMode,
//                   metadata.LedMode,
//                   metadata.ApbDelay,
//                   metadata.RelayT2,
//                   metadata.HeldOpen2,
//                   metadata.RelayFollowerPulse,
//                   metadata.RelayFollowerDelay,
//                   metadata.ExtendFeatureType,
//                   metadata.InteriorPushButtonModuleComponentId,
//                   metadata.InteriorPushButtonInputNumber,
//                   metadata.InteriorPushButtonLongPress,
//                   metadata.InteriorPushButtonOutModuleComponentId,
//                   metadata.InteriorPushButtonOutRelayNumber
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration,deviceSlot.mac,deviceSlot.slot_id));
//             }

//             if (metadata.ReaderOut.ReaderModuleComponentId > -1)
//             {
//                   var res = door.AccessControlReaderConfiguration(
//                   deviceSlot.mac,
//                   (short)deviceSlot.slot_id,
//                   SecondSlot,
//                   2,
//                   FirstSlot,
//                   metadata.ReaderIn.ReaderModuleComponentId,
//                   metadata.ReaderIn.ReaderNumber,
//                   -1,
//                   metadata.Relay.RelayNumber,
//                   metadata.Relay.RelayMin,
//                   metadata.Relay.RelayMax,
//                   metadata.Relay.DriveMode,
//                   -1,
//                   metadata.Sensor.SensorNumber,
//                   metadata.Sensor.HeldOpenDelay,
//                   -1,
//                   metadata.Rex.Rex0Number,
//                   -1,
//                   metadata.Rex.Rex1Number,
//                   metadata.Rex.DisableRex0Timezone,
//                   metadata.Rex.DisableRex1Timezone,
//                   -1,
//                   metadata.AltrReader.AltrRdrNumber,
//                   metadata.AltrReader.AltrRdrConf,
//                   metadata.Antipassback.AntipassbackMode,
//                   metadata.Antipassback.AreaIn,
//                   metadata.Antipassback.AreaOut,
//                   metadata.Spare,
//                   metadata.AccessControlFlag,
//                   metadata.OfflineMode,
//                   metadata.DefaultMode,
//                   metadata.LedMode,
//                   metadata.ApbDelay,
//                   metadata.RelayT2,
//                   metadata.HeldOpen2,
//                   metadata.RelayFollowerPulse,
//                   metadata.RelayFollowerDelay,
//                   metadata.ExtendFeatureType,
//                   metadata.InteriorPushButtonModuleComponentId,
//                   metadata.InteriorPushButtonInputNumber,
//                   metadata.InteriorPushButtonLongPress,
//                   metadata.InteriorPushButtonOutModuleComponentId,
//                   metadata.InteriorPushButtonOutRelayNumber
//                   );

//                   await bus.SendAsync(new AddCommandEvent(res));

//                   if (!res.IsSend)
//                         throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessControlReaderConfiguration, deviceSlot.mac,deviceSlot.slot_id));
//             }
//       }
// }