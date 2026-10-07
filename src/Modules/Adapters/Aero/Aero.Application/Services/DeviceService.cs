using System.Text.Json;
using Adapter.Contract.Interfaces;
using Aero.Application.Enums;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Application.Metadata;
using Aero.Application.Metadata.Device;
using Aero.Application.Metadata.Door;
using Core.Contract.Commands.Device;
using Core.Contract.Commands.Events;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Contract.Queries.CardFormat;
using Core.Contract.Queries.ComponentMapping;
using Core.Contract.Queries.Door;
using Core.Contract.Queries.Group;
using Core.Contract.Queries.Time;
using Setting.Contract.Interfaces;
using Setting.Contract.Queries;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class DeviceService(
      IDeviceRepository repo,
      IOutputRepository oRepo,
      IInputRepository iRepo,
      IGroupRepository gRepo,
      IDoorRepository dRepo,
      ITimeRepository tRepo,
      ICardFormatRepository cfmtRepo,
      IDeviceModuleRepository module,
      IMessageBus bus
      ) : IDeviceAdapter
{


      public async Task GetConfigurationAsync(string mac, string ip, CancellationToken ct = default)
      {
            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac, EntityType.Device));

            var res = repo.ScpStructureStatusRead(
                  mac,
                  (short)externalId,
                  [
                        (short)SCPStructure.SCPSID_TRAN,
                        (short)SCPStructure.SCPSID_TZ,
                        (short)SCPStructure.SCPSID_HOL,
                        (short)SCPStructure.SCPSID_MSP1,
                        (short)SCPStructure.SCPSID_SIO,
                        (short)SCPStructure.SCPSID_MP,
                        (short)SCPStructure.SCPSID_CP,
                        (short)SCPStructure.SCPSID_ACR,
                        (short)SCPStructure.SCPSID_ALVL,
                        (short)SCPStructure.SCPSID_TRIG,
                        (short)SCPStructure.SCPSID_PROC,
                        (short)SCPStructure.SCPSID_MPG,
                        (short)SCPStructure.SCPSID_AREA,
                        (short)SCPStructure.SCPSID_EAL,
                        (short)SCPStructure.SCPSID_CRDB,
                        (short)SCPStructure.SCPSID_FLASH,
                        (short)SCPStructure.SCPSID_BSQN,
                        (short)SCPStructure.SCPSID_SAVE_STAT,
                        (short)SCPStructure.SCPSID_MAB1_FREE,
                        (short)SCPStructure.SCPSID_MAB2_FREE,
                        (short)SCPStructure.SCPSID_ARQ_BUFFER,
                        (short)SCPStructure.SCPSID_PART_FREE_CNT,
                        (short)SCPStructure.SCPSID_LOGIN_STANDARD,
                        (short)SCPStructure.SCPSID_FILE_SYSTEM,
                  ]
            );

            await bus.SendAsync(new AdapterEventCommand(res));
      }

      public async Task<Status> GetStatusAsync(string mac, string ip, CancellationToken ct = default)
      {
            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac, EntityType.Device));

            return repo.GetStatus((short)externalId);
      }

      public async Task GetTransactionStatusAsync(string mac, string ip, CancellationToken ct = default)
      {
            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac, EntityType.Device));
            var res = repo.GetTransactionStatus(mac, (short)externalId);

            await bus.SendAsync(new AdapterEventCommand(res));
      }

      public async Task InititalDeviceAsync(string mac, string Ip = "", CancellationToken ct = default)
      {
            // var scpDevice = await setting.GetAeroDriverSettingAsync();

            var scpDevice = await bus.QueryAsync(new AeroDriverSettingQuery());

            // var externalId = await mapping.GetExternalIdByMacAndEntityAsync(mac,EntityType.Device);

            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac, EntityType.Device));


            var res = repo.AccessDatabaseSpecification(
             mac,
            (short)externalId,
            scpDevice.nCards,
            (short)scpDevice.nAlvlPerCard,
            (short)UtilitiesHelper.CalculatePinDigitValue(
                  scpDevice.PinDuressMode,
                  scpDevice.DuressConstDigit,
                  scpDevice.CardIdSize,
                  scpDevice.PinDigit
                  ),
            (short)scpDevice.IssueCodeBit,
            (short)(scpDevice.AreaBaseApb ? 1 : 0),
            2,
            2,
            1,
            0,
            0,
            (short)(scpDevice.UsedLimit ? 1 : 0),
             (short)(scpDevice.TimeBaseApb ? 1 : 0),
             (short)scpDevice.nAcr,
            0,
             (short)scpDevice.HostResponseTimeout,
            0,
             (short)scpDevice.EscortTimeout,
             (short)scpDevice.MultiCardTimeout
            );

            await bus.SendAsync(new AdapterEventCommand(res));

            res = repo.ElevatorAccessLevelSpecification(
                  mac,
                  (short)externalId,
                  (short)scpDevice.MaxElAlvl,
                  (short)scpDevice.MaxFloorPerAcr
                  );

            await bus.SendAsync(new AdapterEventCommand(res));

            res = repo.TimeSet(
              mac,
              (short)externalId);

            await bus.SendAsync(new AdapterEventCommand(res));



            res = repo.DriverConfiguration(
                  mac,
                  (short)externalId,
                  0,
                  3,
                  -1,
                  0,
                  0,
                  0
            );

            await bus.SendAsync(new AdapterEventCommand(res));

            res = module.SioPanelConfiguration(
                  mac,
                  (short)externalId,
                  0,
                 AeroModuleModelHelper.nInputByModel(DeviceModuleModel.x1100),
                 AeroModuleModelHelper.nOutputByModel(DeviceModuleModel.x1100),
                 AeroModuleModelHelper.nReaderByModel(DeviceModuleModel.x1100),
                 AeroModuleModelHelper.ModelNumber(DeviceModuleModel.x1100),
                 1,
                 0,
                 0,
                 3,
                 0,
                 -1,
                 -1,
                 -1
            );

            await bus.SendAsync(new AdapterEventCommand(res));

            // Transaction index 
            res = repo.SetTransactionLogIndex(
                  mac,
                  (short)externalId,
                  true
                  );

            await bus.SendAsync(new AdapterEventCommand(res));

            // Call memory allocate to check

            res = repo.ScpStructureStatusRead(
                  mac,
                  (short)externalId,
                  [
                        (short)SCPStructure.SCPSID_TRAN,
                        (short)SCPStructure.SCPSID_TZ,
                        (short)SCPStructure.SCPSID_HOL,
                        (short)SCPStructure.SCPSID_MSP1,
                        (short)SCPStructure.SCPSID_SIO,
                        (short)SCPStructure.SCPSID_MP,
                        (short)SCPStructure.SCPSID_CP,
                        (short)SCPStructure.SCPSID_ACR,
                        (short)SCPStructure.SCPSID_ALVL,
                        (short)SCPStructure.SCPSID_TRIG,
                        (short)SCPStructure.SCPSID_PROC,
                        (short)SCPStructure.SCPSID_MPG,
                        (short)SCPStructure.SCPSID_AREA,
                        (short)SCPStructure.SCPSID_EAL,
                        (short)SCPStructure.SCPSID_CRDB,
                        (short)SCPStructure.SCPSID_FLASH,
                        (short)SCPStructure.SCPSID_BSQN,
                        (short)SCPStructure.SCPSID_SAVE_STAT,
                        (short)SCPStructure.SCPSID_MAB1_FREE,
                        (short)SCPStructure.SCPSID_MAB2_FREE,
                        (short)SCPStructure.SCPSID_ARQ_BUFFER,
                        (short)SCPStructure.SCPSID_PART_FREE_CNT,
                        (short)SCPStructure.SCPSID_LOGIN_STANDARD,
                        (short)SCPStructure.SCPSID_FILE_SYSTEM,
                  ]
            );

            await bus.SendAsync(new AdapterEventCommand(res));



      }

      public async Task RemoveDeviceAsync(string mac, string ip, CancellationToken ct = default)
      {
            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac, EntityType.Device));

            var res = repo.DetachScpFromChannel(mac, (short)externalId);

            await bus.SendAsync(new AdapterEventCommand(res));
      }

      public async Task ResetAsync(string mac, string ip, CancellationToken ct = default)
      {
            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac, EntityType.Device));

            var res = repo.ScpReset(mac, (short)externalId);

            await bus.SendAsync(new AdapterEventCommand(res));
      }



      public async Task SetExternalIdAsync(string mac, string ip, int from, int to, CancellationToken ct = default)
      {
            var res = repo.SetScpId(
                  mac,
                  (short)from,
                  (short)to
                  );

            await bus.SendAsync(new AdapterEventCommand(res));
      }

      public async Task UploadAsync(
            string mac,
            string ip,
            CancellationToken ct = default
      )
      {
            // Start with Upload Scp Driver 
            // 1. Get Device Setting
            var device = await bus.QueryAsync(new DeviceByMacQuery(mac));
            // 2. Get ComponentId
            var componentId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac, EntityType.Device));
            // 3.Json Serialize with Metadata Class 
            var deviceMetadata = JsonHelper.Deserialize<DeviceMetadata>(device.Metadata);
            // 4.Send command driver configuration
            if (deviceMetadata.PortOne)
            {
                  var res = repo.DriverConfiguration(
                        device.Mac,
                        (short)componentId,
                        1,
                        1,
                        deviceMetadata.BaudRateOne,
                        0,
                        deviceMetadata.ProtocolOne,
                        0
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));
            }

            if (deviceMetadata.PortTwo)
            {
                  var res = repo.DriverConfiguration(
                        device.Mac,
                        (short)componentId,
                        2,
                        2,
                        deviceMetadata.BaudRateTwo,
                        0,
                        deviceMetadata.ProtocolTwo,
                        0
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));
            }
      }



      public async Task UploadAllConfigurationAsync(string mac, string ip, CancellationToken ct = default)
      {
            // Start with Upload Scp Driver 
            // 1. Get Device Setting
            var device = await bus.QueryAsync(new DeviceByMacQuery(mac));
            // 2. Get ComponentId
            var componentId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac, EntityType.Device));
            // 3.Json Serialize with Metadata Class 
            var deviceMetadata = JsonHelper.Deserialize<DeviceMetadata>(device.Metadata);
            // 4.Send command driver configuration
            if (deviceMetadata.PortOne)
            {
                  var res = repo.DriverConfiguration(
                        device.Mac,
                        (short)componentId,
                        1,
                        1,
                        deviceMetadata.BaudRateOne,
                        0,
                        deviceMetadata.ProtocolOne,
                        0
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));
            }

            if (deviceMetadata.PortTwo)
            {
                  var res = repo.DriverConfiguration(
                        device.Mac,
                        (short)componentId,
                        2,
                        2,
                        deviceMetadata.BaudRateTwo,
                        0,
                        deviceMetadata.ProtocolTwo,
                        0
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));
            }

            // Time 

            var tzs = await bus.QueryAsync(new TimeZoneByLocationQuery(device.LocationGuid));

            foreach (var tz in tzs)
            {
                  var tzId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(tz.Guid, EntityType.TimeZone));

                  var res = tRepo.ExtendedTimeZoneActSpecification(
                        device.Mac,
                        (short)componentId,
                        (short)tzId,
                        tz.Name.Equals("Always") ? (short)1 : tz.Name.Equals("Never") ? (short)0 : (short)2,
                        tz.Intervals.Select(x =>
                  (
                        (short)UtilitiesHelper.ConvertDayToBinary(
                              x.Days.Sunday,
                              x.Days.Monday,
                              x.Days.Tuesday,
                              x.Days.Wednesday,
                              x.Days.Thursday,
                              x.Days.Friday,
                              x.Days.Saturday
                              ),
                        (short)UtilitiesHelper.TimeOnlyToInt(x.Start),
                        (short)UtilitiesHelper.TimeOnlyToInt(x.End)
                        )).ToList()
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));
            }


            // CardFormat

            var cfmts = await bus.QueryAsync(new CardFormatQuery());
            foreach (var cfmt in cfmts)
            {
                  var cfmtId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(cfmt.Guid, EntityType.CardFormat));

                  var res = cfmtRepo.CardFormatterConfiguration(
                        device.Mac,
                        (short)componentId,
                        (short)cfmtId,
                        cfmt.Fac,
                        0,
                        1,
                        0,
                        cfmt.Bits,
                        cfmt.EvenParityLen,
                        cfmt.EvenParityLoc,
                        cfmt.OddParityLen,
                        cfmt.OddParityLoc,
                        cfmt.FacLen,
                        cfmt.FacLoc,
                        cfmt.CardNoLen,
                        cfmt.CardNoLoc,
                        cfmt.IssueCodeLen,
                        cfmt.IssueCodeLoc
                  );

                  await bus.SendAsync(new AdapterEventCommand(res));
            }

            // Door
            var doors = await bus.QueryAsync(new DoorByMacQuery(device.Mac));
            foreach (var d in doors)
            {
                  // Reader Configuation
                  foreach (var reader in d.Readers)
                  {
                        var moduleId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(reader.DeviceModuleGuid, EntityType.DeviceModule));

                        var meta = JsonHelper.Deserialize<ReaderMetadata>(reader.Metadata);
                        if (meta == null)
                              throw new Exception(MessageHelper.Common.DeserializeFailed("ReaderMetadata"));

                        short osdpFlag = 0x00;
                        if (reader.Mode == ReaderMode.Osdp)
                        {
                              osdpFlag += meta.Baudrate;
                              osdpFlag |= meta.AutoDiscover;
                              osdpFlag |= meta.Tracing;
                              osdpFlag |= (short)(meta.Address << 5);
                              osdpFlag |= meta.SecureChannel;
                        }

                        var res = dRepo.ReaderSpecification(
                              mac,
                              (short)componentId,
                              (short)moduleId,
                              (short)reader.SlotNo,
                              osdpFlag
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));

                  }


                  // Relay Configuration
                  var relayModuleId = -1;
                  if (d.Relay != null)
                  {
                        relayModuleId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(d.Relay.DeviceModuleGuid, EntityType.DeviceModule));

                        var meta = JsonHelper.Deserialize<RelayMetadata>(d.Relay.Metadata);
                        if (meta == null)
                              throw new Exception(MessageHelper.Common.DeserializeFailed("RelayMetadata"));

                        var res = oRepo.OutputPointSpecification(
                              mac,
                             (short)componentId,
                              (short)relayModuleId,
                              (short)d.Relay.SlotNo,
                              meta.OfflineMode,
                              meta.DriveMode
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));

                  }

                  // Sensor Configuration
                  var sensorModuleId = -1;
                  if (d.Sensor != null)
                  {
                        sensorModuleId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(d.Sensor.DeviceModuleGuid, EntityType.DeviceModule));

                        var meta = JsonHelper.Deserialize<InputMetadata>(d.Sensor.Metadata);
                        if (meta == null)
                              throw new Exception(MessageHelper.Common.DeserializeFailed("InputMetadata"));

                        var res = iRepo.InputPointSpecification(
                              mac,
                              (short)componentId,
                              (short)sensorModuleId,
                              (short)d.Sensor.SlotNo,
                              meta.Mode,
                              meta.Debounce,
                              meta.HoldTime
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));

                  }

                  // Rex Configuration
                  var rex0ModuleId = -1;
                  var rex1ModuleId = -1;
                  int i = 0;
                  foreach (var rex in d.Rexes)
                  {
                        if(i == 0)
                        {
                              rex0ModuleId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(rex.DeviceModuleGuid, EntityType.DeviceModule));
                        }else if(i == 1)
                        {
                              rex1ModuleId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(rex.DeviceModuleGuid, EntityType.DeviceModule));
                        }
                        

                        var meta = JsonHelper.Deserialize<InputMetadata>(rex.Metadata);
                        if (meta == null)
                              throw new Exception(MessageHelper.Common.DeserializeFailed("InputMetadata"));

                        var res = iRepo.InputPointSpecification(
                              mac,
                              (short)componentId,
                              (short)(i == 0 ? rex0ModuleId : rex1ModuleId),
                              (short)rex.SlotNo,
                              meta.Mode,
                              meta.Debounce,
                              meta.HoldTime
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));

                        i++;
                  }


                  // Buzzer Configuration
                  if (d.Buzzer != null)
                  {
                        var moduleId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(d.Buzzer.DeviceModuleGuid, EntityType.DeviceModule));
                        var outputId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(d.Guid, EntityType.Output));

                        var meta = JsonHelper.Deserialize<OutputMetadata>(d.Buzzer.Metadata);
                        if (meta == null)
                              throw new Exception(MessageHelper.Common.DeserializeFailed("OutputMetadata"));

                        var res = oRepo.OutputPointSpecification(
                              mac,
                              (short)componentId,
                              (short)moduleId,
                              (short)d.Buzzer.SlotNo,
                              meta.OfflineMode,
                              meta.DefaultMode
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));

                        res = oRepo.ControlPointConfiguration(
                              mac,
                              (short)componentId,
                              (short)moduleId,
                              (short)outputId,
                              (short)d.Buzzer.SlotNo,
                              1
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));

                  }

                  // and the trigger setting here

                  // BG Configuration
                  if (d.Bg != null)
                  {
                        var moduleId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(d.Bg.DeviceModuleGuid, EntityType.DeviceModule));
                        var inputId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(d.Guid, EntityType.Input));

                        var meta = JsonHelper.Deserialize<BgMetadata>(d.Bg.Metadata);
                        if (meta == null)
                              throw new Exception(MessageHelper.Common.DeserializeFailed("BgMetadata"));

                        var res = iRepo.InputPointSpecification(
                              mac,
                              (short)componentId,
                              (short)moduleId,
                              (short)d.Bg.SlotNo,
                              meta.Mode,
                              meta.Debounce,
                              meta.HoldTime
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));

                        res = iRepo.MonitorPointConfiguration(
                              mac,
                              (short)componentId,
                              (short)inputId,
                              (short)moduleId,
                              (short)d.Bg.SlotNo,
                              0,
                              0,
                              0,
                              0
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));

                  }

                  // Door Configuration
                  short spare = 0x00;
                  short accessFlag = 0x00;
                  DoorMetadata doorMetadata = new DoorMetadata();

                  if (!string.IsNullOrWhiteSpace(d.Metadata))
                  {
                        doorMetadata = JsonHelper.Deserialize<DoorMetadata>(d.Metadata);
                        if (doorMetadata == null)
                              throw new Exception(MessageHelper.Common.DeserializeFailed("DoorMetadata"));

                        if (doorMetadata.ForceCardPin) spare |= (short)ExtendedAccessControlFlags.ACR_FE_NOPINCARD;
                        if (doorMetadata.DoubleCard) spare |= (short)ExtendedAccessControlFlags.ACR_FE_DCARD;
                        if (doorMetadata.OutputSelectionTracking) spare |= (short)ExtendedAccessControlFlags.ACR_FE_FLOOR_PIN;
                        if (doorMetadata.LockedOverride) spare |= (short)ExtendedAccessControlFlags.ACR_FE_CRD_OVR_EN;
                        if (doorMetadata.HostPermission) spare |= (short)ExtendedAccessControlFlags.ACR_FE_HOST_BYPASS;

                        if (d.Type == DoorType.Dual) spare |= (short)ExtendedAccessControlFlags.ACR_FE_LINK_MODE;


                        if (doorMetadata.DecreaseUseLimit) accessFlag |= (short)AccessControlFlags.ACR_F_DCR;
                        if (doorMetadata.RequireUseLimit) accessFlag |= (short)AccessControlFlags.ACR_F_CUL;
                        if (doorMetadata.DeniedDuress) accessFlag |= (short)AccessControlFlags.ACR_F_DRSS;
                        if (doorMetadata.QuietRex) accessFlag |= (short)AccessControlFlags.ACR_F_QEXIT;
                        if (doorMetadata.FilterStatus) accessFlag |= (short)AccessControlFlags.ACR_F_FILTER;
                        if (doorMetadata.DoubleCardAccess) accessFlag |= (short)AccessControlFlags.ACR_F_2CARD;
                        if (doorMetadata.HostPermission) accessFlag |= (short)AccessControlFlags.ACR_F_HOST_CBG;
                        if (doorMetadata.HostOfflineGrant) accessFlag |= (short)AccessControlFlags.ACR_F_HOST_SFT;

                  }

                  var doorIds = await bus.QueryAsync(new ExternalIdsByGuidAndEntityQuery(d.Guid, EntityType.Door),ct);


                  RelayMetadata? relayMeta = null;
                  if (d.Relay != null)
                  {
                        relayMeta = JsonHelper.Deserialize<RelayMetadata>(d.Relay.Metadata) ?? throw new Exception(MessageHelper.Common.DeserializeFailed("RelayMetadata"));
                  }

                  SensorMetadata? sensorMeta = null;
                  if (d.Sensor != null)
                  {
                        sensorMeta = JsonHelper.Deserialize<SensorMetadata>(d.Sensor.Metadata) ?? throw new Exception(MessageHelper.Common.DeserializeFailed("SensorMetadata"));
                  }



                  RexMetadata? rexMeta0 = null;
                  RexMetadata? rexMeta1 = null;
                  if (d.Rexes.Count == 1)
                  {
                        rexMeta0 = JsonHelper.Deserialize<RexMetadata>(d.Rexes.ElementAt(0).Metadata) ?? throw new Exception(MessageHelper.Common.DeserializeFailed("RexMetadata"));
                  }

                  if (d.Rexes.Count() == 2)
                  {
                        rexMeta1 = JsonHelper.Deserialize<RexMetadata>(d.Rexes.ElementAt(1).Metadata) ?? throw new Exception(MessageHelper.Common.DeserializeFailed("RexMetadata"));
                  }

                  var readerGuid = d.Readers.Where(x => x.ReaderDirection == ReaderDirection.In).Select(x => x.DeviceModuleGuid).First();
                  var readerModuleId = (short)(d.Readers.Count == 0 ? -1 : await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(readerGuid, EntityType.DeviceModule), ct));
                  var readerSlot = (short)(d.Readers.Count == 0 ? -1 : d.Readers.Where(x => x.ReaderDirection == ReaderDirection.In).Select(x => x.SlotNo).First());

                  
                  var mask0Id = d.Rexes.Count == 0 ? 0 : d.Rexes.ElementAt(0).MaskGuid == null || d.Rexes.ElementAt(0).MaskGuid == Guid.Empty ? 0 : await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(d.Rexes.ElementAt(0).MaskGuid ?? Guid.Empty,EntityType.TimeZone),ct);
                  var mask1Id = d.Rexes.Count <= 1 ? 0 : d.Rexes.ElementAt(1).MaskGuid == null || d.Rexes.ElementAt(1).MaskGuid == Guid.Empty ? 0 : await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(d.Rexes.ElementAt(1).MaskGuid ?? Guid.Empty,EntityType.TimeZone),ct);

                  var doorRes = dRepo.AccessControlReaderConfiguration(
                        mac,
                        (short)componentId,
                        (short)doorIds.Min(),
                        (short)(d.Type == DoorType.Single ? 0 : 1),
                        d.Type == DoorType.Single && doorIds.Count() <= 1 ? (short)-1 : (short)doorIds.Max(),
                        readerModuleId,
                        readerSlot,
                        (short)(d.Relay == null ? -1 : relayModuleId),
                        (short)(d.Relay == null ? -1 : d.Relay.SlotNo),
                        (short)(d.Relay == null || relayMeta == null ? 1 : relayMeta.StrikeMin),
                        (short)(d.Relay == null || relayMeta == null ? 5 : relayMeta.StrikeMax),
                        (short)(d.Relay == null || relayMeta == null ? 0 : relayMeta.StrikeMode),
                        (short)(d.Sensor == null ? -1 : sensorModuleId),
                        (short)(d.Sensor == null ? -1 : d.Sensor.SlotNo),
                        (short)(d.Sensor == null || sensorMeta == null ? 1 : sensorMeta.DcHeld),
                        (short)(d.Rexes.Count == 0 ? -1 : rex0ModuleId),
                        (short)(d.Rexes.Count == 0 ? -1 : d.Rexes.ElementAt(0).SlotNo),
                        (short)(d.Rexes.Count <= 1 ? -1 : rex1ModuleId),
                        (short)(d.Rexes.Count <= 1 ? -1 : rex1ModuleId),
                        (short)(d.Rexes.Count == 0 ? 0 : mask0Id),
                        (short)(d.Rexes.Count <= 1 ? 0 : mask1Id),
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

                  await bus.SendAsync(new AdapterEventCommand(doorRes), ct);

                  if (d.Type == DoorType.Dual)
                  {
                        readerGuid = d.Readers.Where(x => x.ReaderDirection == ReaderDirection.Out).Select(x => x.DeviceModuleGuid).First();
                        readerModuleId = (short)(d.Readers.Count == 0 ? -1 : await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(readerGuid, EntityType.DeviceModule), ct));
                        readerSlot = (short)(d.Readers.Count == 0 ? -1 : d.Readers.Where(x => x.ReaderDirection == ReaderDirection.Out).Select(x => x.SlotNo).First());
                        
                        var res = dRepo.AccessControlReaderConfiguration(
                              mac,
                              (short)componentId,
                              (short)doorIds.Max(),
                              2,
                              (short)doorIds.Min(),
                              readerModuleId,
                              readerSlot,
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

                        await bus.SendAsync(new AdapterEventCommand(res), ct);

                  }

                  

                  // Group
                  var gps = await bus.QueryAsync(new GroupByMacQuery(device.Mac),ct);
                  foreach (var g in gps)
                  {
                        var groupId = await bus.QueryAsync(new ExternalIdByGuidAndEntityQuery(g.Guid, EntityType.Group),ct);
                        var doorExIds = await bus.QueryAsync(new ExternalIdMapGuidByGuidsAndEntityQuery(g.Components.Select(x => x.Doors),EntityType.Door),ct);
                        var timeExIds = await bus.QueryAsync(new ExternalIdMapGuidByGuidsAndEntityQuery(g.Components.Select(x => x.TimeZone),EntityType.TimeZone),ct);

                        var res = gRepo.AddAccessGroup(
                              mac,
                              (short)componentId,
                              (short)groupId,
                              0,
                              g.Components.Select(x => ((short)doorExIds[x.Doors],(short)timeExIds[x.TimeZone])).ToList()
                        );

                        await bus.SendAsync(new AdapterEventCommand(res));
                  }


                  // finally

                  await bus.SendAsync(new ConfigurationStatusCommand(mac, true, true));

            }
      }

      public async Task CommandAsync(string mac, string ip, short scpId, string command, CancellationToken ct = default)
      {
            var res = repo.AsciiCommandAsync(mac, scpId, command);

            await bus.SendAsync(new AdapterEventCommand(res));
      }
}