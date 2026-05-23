using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using Npgsql.Internal;
using SharedKernel.Model;

namespace Adapter.Aero.Command;

public sealed class DoorCommand(ILogger<DoorCommand> logger) : BaseCommand,IDoorCommand
{ 
      public CommandResponse AccessControlReaderConfiguration(
            string Mac,
            short DeviceComponentId,
            short ComponentId,
            short AccessConfig,
            short PairComponentId,
            short ReaderModuleComponentId,
            short ReaderNumber,
            short RelayModuleComponentId,
            short RelayNumber,
            short RelayMin,
            short RelayMax,
            short RelayMode,
            short SensorModuleComponentId,
            short SensorNumber,
            short HeldOpenDelay,
            short Rex0ModuleComponentId,
            short Rex0Number,
            short Rex1ModuleComponentId,
            short Rex1Number,
            short DisableRex0Timezone,
            short DisableRex1Timezone,
            short AltrRdrModuleComponentId,
            short AltrRdrNumber,
            short AltrRdrConf,
            short AntipassbackMode,
            short AreaIn,
            short AreaOut,
            short Spare,
            short AccessControlFlag,
            short OfflineMode,
            short DefaultMode,
            short LedMode,
            short ApbDelay,
            short RelayT2,
            short HeldOpen2,
            short RelayFollowerPulse,
            short RelayFollowerDelay,
            short ExtendFeatureType,
            short InteriorPushButtonModuleComponentId,
            short InteriorPushButtonInputNumber,
            short InteriorPushButtonLongPress,
            short InteriorPushButtonOutModuleComponentId,
            short InteriorPushButtonOutRelayNumber

      )
      {
            CC_ACR c= new CC_ACR();
            c.lastModified = 0;
            c.scp_number = DeviceComponentId;
            c.acr_number = ComponentId;
            c.access_cfg = AccessConfig;
            c.pair_acr_number = PairComponentId;
            c.rdr_sio = ReaderModuleComponentId;
            c.rdr_number = ReaderNumber;
            c.strk_sio = RelayModuleComponentId;
            c.strk_number = RelayNumber;
            c.strike_t_min = RelayMin;
            c.strike_t_max = RelayMax;
            c.strike_mode = RelayMode;
            c.door_sio = SensorModuleComponentId;
            c.door_number = SensorNumber;
            c.dc_held = HeldOpenDelay;
            c.rex0_sio = Rex0ModuleComponentId;
            c.rex0_number = Rex0Number;
            c.rex1_sio = Rex1ModuleComponentId;
            c.rex1_number = Rex1Number;
            c.rex_tzmask[0] = DisableRex0Timezone;
            c.rex_tzmask[1] = DisableRex1Timezone;
            c.altrdr_sio = AltrRdrModuleComponentId;
            c.altrdr_number = AltrRdrNumber;
            c.altrdr_spec = AltrRdrConf;
            c.cd_format = 255; // Support all table
            c.apb_mode = AntipassbackMode;
            c.apb_in = AreaIn;
            c.apb_to = AreaOut;
            c.spare = Spare;
            c.actl_flags = AccessControlFlag;
            c.offline_mode = OfflineMode;
            c.default_mode = DefaultMode;
            c.default_led_mode = LedMode;
            c.pre_alarm = 0;
            c.apb_delay = ApbDelay;
            c.strk_t2 = RelayT2;
            c.dc_held2 = HeldOpen2;
            c.strk_follow_pulse = RelayFollowerPulse;
            c.strk_follow_delay = RelayFollowerDelay;
            c.nAuthModFlags = 0;
            c.nExtFeatureType = ExtendFeatureType;
            c.uExtFeatureInfo.sIPBoverrides.iIPB_sio = InteriorPushButtonModuleComponentId;
            c.uExtFeatureInfo.sIPBoverrides.iIPB_number = InteriorPushButtonInputNumber;
            c.uExtFeatureInfo.sIPBoverrides.iIPB_long_press = InteriorPushButtonLongPress;
            c.uExtFeatureInfo.sIPBoverrides.iIPB_out_sio = InteriorPushButtonOutModuleComponentId;
            c.uExtFeatureInfo.sIPBoverrides.iIPB_out_num = InteriorPushButtonOutRelayNumber;
            c.dfofFilterTime = 0;
            var result = Send((short)enCfgCmnd.enCcMpg, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.AccessControlReaderConfiguration, DeviceComponentId));

                  return new CommandResponse(
                        Mac,
                        DeviceComponentId,
                        CommandConstant.AccessControlReaderConfiguration,
                        SCPDLL.scpGetTagLastPosted(DeviceComponentId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        c.ToString(),
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.AccessControlReaderConfiguration, DeviceComponentId));
                  return new CommandResponse(
                        Mac,
                       DeviceComponentId,
                       CommandConstant.AccessControlReaderConfiguration,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       c.ToString(),
                       CommandStatus.PENDING.ToString(),
                       string.Empty,
                       false
                       );

            }

      }

      public CommandResponse ReaderSpecification(
            string Mac,
            short DeviceComponentId,
            short ModuleComponentId,
            short ReaderNumber,
            short DataFormat,
            short KeypadMode,
            short LedDriverMode,
            short OsdpFlag
      )
      {
            CC_RDR c = new CC_RDR();
            c.lastModified = 0;
            c.scp_number = DeviceComponentId;
            c.sio_number = ModuleComponentId;
            c.reader = ReaderNumber;
            c.dt_fmt = DataFormat;
            c.keypad_mode = KeypadMode;
            c.led_drive_mode = LedDriverMode;
            c.osdp_flags = OsdpFlag;
            var result = Send((short)enCfgCmnd.enCcMpg, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ReaderSpecification, DeviceComponentId));

                  return new CommandResponse(
                        Mac,
                        DeviceComponentId,
                        CommandConstant.ReaderSpecification,
                        SCPDLL.scpGetTagLastPosted(DeviceComponentId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        c.ToString(),
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ReaderSpecification, DeviceComponentId));
                  return new CommandResponse(
                        Mac,
                       DeviceComponentId,
                       CommandConstant.ReaderSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       c.ToString(),
                       CommandStatus.PENDING.ToString(),
                       string.Empty,
                       false
                       );

            }
            
      }
}