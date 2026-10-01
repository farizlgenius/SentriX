using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class DoorRepository(
      ILogger<DoorRepository> logger,
      IBaseRepository repo
) : IDoorRepository
{
      public CommandResponse AccessControlReaderConfiguration(string mac, short scpId, short acrNo, short readerConfig, short pairAcr, short readerSio, short readerNo, short relaySio, short relayNo,short relayMin,short relayMax, short relayMode, short sendorSio, short sendorNo, short heldOpenDelay, short rexSio, short rexNo, short rex1Sio, short rex1No, short rex0Mask, short rex1Mask, short altrRdrSio, short altrRdrNo, short altrRdrSpec, short cdFormat, short apbMode, short apbIn, short apbOut, short spare, short actlFlags, short offlineMode, short defaultMode, short defaultLedMode, short preAalrm, short apbDelay)
      {
            CC_ACR c= new CC_ACR();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.acr_number = acrNo;
            c.access_cfg = readerConfig;
            c.pair_acr_number = pairAcr;
            c.rdr_sio = readerSio;
            c.rdr_number = readerNo;
            c.strk_sio = relaySio;
            c.strk_number = relayNo;
            c.strike_t_min = relayMin;
            c.strike_t_max = relayMax;
            c.strike_mode = relayMode;
            c.door_sio = sendorSio;
            c.door_number = sendorNo;
            c.dc_held = heldOpenDelay;
            c.rex0_sio = rexSio;
            c.rex0_number = rexNo;
            c.rex1_sio = rex1Sio;
            c.rex1_number = rex1No;
            c.rex_tzmask[0] = rex0Mask;
            c.rex_tzmask[1] = rex1Mask;
            c.altrdr_sio = altrRdrSio;
            c.altrdr_number = altrRdrNo;
            c.altrdr_spec = altrRdrSpec;
            c.cd_format = 255; // Support all table
            c.apb_mode = apbMode;
            c.apb_in = apbIn;
            c.apb_to = apbOut;
            c.spare = spare;
            c.actl_flags = actlFlags;
            c.offline_mode = offlineMode;
            c.default_mode = defaultMode;
            c.default_led_mode = defaultLedMode;
            c.pre_alarm = preAalrm;
            c.apb_delay = apbDelay;
            // c.strk_t2 = relayT2;
            // c.dc_held2 = heldOpen2;
            // c.strk_follow_pulse = relayFollowerPulse;
            // c.strk_follow_delay = relayFollowerDelay;
            // c.nAuthModFlags = 0;
            // c.nExtFeatureType = ExtendFeatureType;
            // c.uExtFeatureInfo.sIPBoverrides.iIPB_sio = InteriorPushButtonModuleComponentId;
            // c.uExtFeatureInfo.sIPBoverrides.iIPB_number = InteriorPushButtonInputNumber;
            // c.uExtFeatureInfo.sIPBoverrides.iIPB_long_press = InteriorPushButtonLongPress;
            // c.uExtFeatureInfo.sIPBoverrides.iIPB_out_sio = InteriorPushButtonOutModuleComponentId;
            // c.uExtFeatureInfo.sIPBoverrides.iIPB_out_num = InteriorPushButtonOutRelayNumber;
            // c.dfofFilterTime = 0;
            var result = repo.Send((short)enCfgCmnd.enCcACR, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.AccessControlReaderConfiguration, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.AccessControlReaderConfiguration,
                        SCPDLL.scpGetTagLastPosted(scpId),
                        DateTime.UtcNow,
                        null,
                        ObjectHelper.ToAsciiString(c),
                        CommandStatus.PENDING,
                        string.Empty,
                        Vendor.aero,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.AccessControlReaderConfiguration, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.AccessControlReaderConfiguration,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       ObjectHelper.ToAsciiString(c),
                       CommandStatus.PENDING,
                       string.Empty,
                       Vendor.aero,
                       false
                       );

            }
      }

      public CommandResponse ReaderSpecification(string mac, short scpId, short sioNo, short readerNo, short osdpFlag)
      {
            CC_RDR c = new CC_RDR();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.sio_number = sioNo;
            c.reader = readerNo;
            c.dt_fmt = 0x01;
            c.keypad_mode = 2;
            c.led_drive_mode = osdpFlag == 0 ? (short)1 : (short)2;
            c.osdp_flags = osdpFlag;
            var result = repo.Send((short)enCfgCmnd.enCcReader, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ReaderSpecification, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.ReaderSpecification,
                        SCPDLL.scpGetTagLastPosted(scpId),
                        DateTime.UtcNow,
                        null,
                         ObjectHelper.ToAsciiString(c),
                        CommandStatus.PENDING,
                        string.Empty,
                        Vendor.aero,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ReaderSpecification, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.ReaderSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                        ObjectHelper.ToAsciiString(c),
                       CommandStatus.PENDING,
                       string.Empty,
                        Vendor.aero,
                       false
                       );

            }

      }

}