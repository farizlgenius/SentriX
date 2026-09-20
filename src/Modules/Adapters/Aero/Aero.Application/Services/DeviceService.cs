using System.Text.Json;
using Adapter.Contract.Interfaces;
using Aero.Application.Enums;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Application.Metadata;
using Aero.Application.Metadata.Device;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Contract.Queries.ComponentMapping;
using Setting.Contract.Interfaces;
using Setting.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class DeviceService(
      IDeviceRepository repo,
      IComponentMapping mapping,
      IDeviceModuleRepository module,
      IMessageBus bus
      ) : IDeviceAdapter
{
      public async Task GetConfigurationAsync(string mac, string ip, CancellationToken ct = default)
      {
            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac,EntityType.Device));

            repo.ScpStructureStatusRead(
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
      }

      public async Task<Status> GetStatusAsync(string mac, string ip, CancellationToken ct = default)
      {
            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac,EntityType.Device));
            
            return repo.GetStatus((short)externalId);
      }

      public async Task InititalDeviceAsync(string mac,string Ip = "",CancellationToken ct = default)
      {
            // var scpDevice = await setting.GetAeroDriverSettingAsync();

            var scpDevice = await bus.QueryAsync(new AeroDriverSettingQuery());

            // var externalId = await mapping.GetExternalIdByMacAndEntityAsync(mac,EntityType.Device);

            var externalId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac,EntityType.Device));

            repo.AccessDatabaseSpecification(
             mac,
            (short)externalId,
            scpDevice.nCards,
            (short)scpDevice.nAlvl,
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
             (short)scpDevice.nTz,
            0,
             (short)scpDevice.HostResponseTimeout,
            0,
             (short)scpDevice.EscortTimeout,
             (short)scpDevice.MultiCardTimeout
            );

            repo.TimeSet(
              mac,
              (short)externalId);

            repo.DriverConfiguration(
                  mac,
                  (short)externalId,
                  0,
                  0,
                  -1,
                  0,
                  0,
                  0
            );

            module.SioPanelConfiguration(
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

            

      }

      public Task ResetAsync(string mac, string ip, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task UploadAllConfigurationAsync(string mac, string ip, CancellationToken ct = default)
      {
            // Start with Upload Scp Driver 
            // 1. Get Device Setting
            var device = await bus.QueryAsync(new DeviceByMacQuery(mac));
            // 2. Get ComponentId
            var componentId = await bus.QueryAsync(new ExternalIdByMacAndEntityQuery(mac,EntityType.Device));
            // 3.Json Serialize with Metadata Class 
            var deviceMetadata = JsonHelper.Deserialize<DeviceMetadata>(device.Metadata);
            // 4.Send command driver configuration
            if (deviceMetadata.PortOne)
            {
                  repo.DriverConfiguration(
                        device.Mac,
                        (short)componentId,
                        1,
                        1,
                        deviceMetadata.BaudRateOne,
                        0,
                        deviceMetadata.ProtocolOne,
                        0
                  );
            }

            if (deviceMetadata.PortTwo)
            {
                  repo.DriverConfiguration(
                        device.Mac,
                        (short)componentId,
                        2,
                        2,
                        deviceMetadata.BaudRateTwo,
                        0,
                        deviceMetadata.ProtocolTwo,
                        0
                  );
            }
            
            
      }
}