using System.Text.Json;
using Adapter.Contract.Interfaces;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Core.Contract.Interfaces;
using Setting.Contract.Interfaces;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Aero.Application.Services;

public sealed class DeviceService(
      IDeviceRepository repo,
      ISetting setting,
      IComponentMapping mapping,
      IDeviceModuleRepository module
      ) : IDeviceAdapter
{
      public async Task InititalDeviceAsync(string mac,string Ip = "",CancellationToken ct = default)
      {
            var scpDevice = await setting.GetAeroDriverSettingAsync();

            var externalId = await mapping.GetExternalIdByMacAndEntityAsync(mac,EntityType.Device);

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
}