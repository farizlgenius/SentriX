using System;
using Adapter.Aero.Command;
using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Model;

namespace AeroAdapter.Infrastructure.Writer;

public sealed class ScpCommand(ILogger<ScpCommand> logger) : BaseCommand, IScpCommand
{

      public CommandResponse ScpDeviceSpecification(
           string Mac,
           short ScpId,
           short nMsp1Port,
           int nTransaction,
           short nSio,
           short nMp,
           short nCp,
           short nAcr,
           short nAlvl,
           short nTrgr,
           short nProc,
           short gmtOffet,
           short nDstId,
           short nTz,
           short nHol,
           short nMpg,
           int nTranLimit,
           short nOperMode,
           short operType,
           short nLanguage
      )
      {

            CC_SCP_SCP c = new CC_SCP_SCP();
            c.lastModified = 0;
            c.number = ScpId;
            c.ser_num_low = 0;
            c.ser_num_high = 0;
            c.rev_major = 0;
            c.rev_minor = 0;
            c.nMsp1Port = nMsp1Port;
            c.nTransactions = nTransaction;
            c.nSio = nSio;
            c.nMp = nMp;
            c.nCp = nCp;
            c.nAcr = nAcr;
            c.nAlvl = nAlvl;
            c.nTrgr = nTrgr;
            c.nProc = nProc;
            c.gmt_offset = gmtOffet;
            c.nDstID = nDstId;
            c.nTz = nTz;
            c.nHol = nHol;
            c.nMpg = nMpg;
            c.nTranLimit = nTranLimit;
            c.nAuthModType = 0;
            c.nOperModes = nOperMode;
            c.oper_type = operType;
            c.nLanguages = nLanguage;
            c.nSrvcType = 0;
            var command = LogMessageHelper.ToString(c);
            var result = Send((short)enCfgCmnd.enCcScpScp, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ScpDeviceSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ScpDeviceSpecification,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        string.Empty,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ScpDeviceSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ScpDeviceSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }

      }
      public CommandResponse AccessDatabaseSpecification(
            string Mac,
            short ScpId,
            int nCard,
            short nAlvlm,
            short nPinDigit,
            short bIssueCode,
            short bApbLocation,
            short bActDate,
            short bDeactDate,
            short bVacationDate,
            short bUpgradeDate,
            short bUserLevel,
            short bUseLimit,
            short bSupportTimeApb,
            short nTz,
            short bAssetGroup,
            short nHostResponseTimeout,
            short nAvlUse4Arg,
            short nEscortTimeout,
            short mMultiCardTimeout
      )
      {
            CC_SCP_ADBS c = new CC_SCP_ADBS();
            c.lastModified = 0;
            c.nScpID = ScpId;
            c.nCards = nCard;
            c.nAlvl = nAlvlm;
            c.nPinDigits = nPinDigit;
            c.bIssueCode = bIssueCode;
            c.bApbLocation = bApbLocation;
            c.bActDate = bActDate;
            c.bDeactDate = bDeactDate;
            c.bVacationDate = bVacationDate;
            c.bUpgradeDate = bUpgradeDate;
            c.bUserLevel = bUserLevel;
            c.bUseLimit = bUseLimit;
            c.bSupportTimedApb = bSupportTimeApb;
            c.nTz = nTz;
            c.bAssetGroup = bAssetGroup;
            c.nHostResponseTimeout = nHostResponseTimeout;
            c.nMxmTypeIndex = 0;
            c.nAlvlUse4Arq = nAvlUse4Arg;
            c.nFreeformBlockSize = 0;
            c.nEscortTimeout = nEscortTimeout;
            c.nMultiCardTimeout = mMultiCardTimeout;
            c.nAssetTimeout = 0;
            var result = Send((short)enCfgCmnd.enCcScpAdbSpec, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.AccessDatabaseSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.AccessDatabaseSpecification,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        string.Empty,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.AccessDatabaseSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.AccessDatabaseSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }


      }

      public CommandResponse AsciiCommandAsync(string Mac, short ScpId, string Command)
      {
            var result = SCPDLL.scpConfigCommand(Command);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.AsciiCommandAsync, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.AsciiCommandAsync,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        Command,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.AsciiCommandAsync, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.AsciiCommandAsync,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       Command,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }
      }

      public bool CreateChannel()
      {

            CC_CHANNEL c = new CC_CHANNEL();
            c.nChannelId = 1;
            c.cType = 7;
            c.cPort = 3333;
            c.baud_rate = 0;
            c.timer1 = 3000;
            c.timer2 = 0;
            for (int i = 0; i < c.cModemId.Length; i++)
            {
                  c.cModemId[i] = '\0';
            }
            c.cRTSMode = 0;
            var result = Send((short)enCfgCmnd.enCcCreateChannel, c);
            if (result)
                  logger.LogInformation("Create channel command sent successfully.");
            return result;
      }

      public CommandResponse DriverConfiguration(
            string Mac,
            short ScpId,
            short Msp1Number,
            short PortNumber,
            short Baudrate,
            short ReplyTime,
            short nProtocol,
            short nDialect
            )
      {

            CC_MSP1 c = new CC_MSP1();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.msp1_number = Msp1Number;
            c.port_number = PortNumber;
            c.baud_rate = Baudrate;
            c.reply_time = ReplyTime;
            c.nProtocol = nProtocol;
            c.nDialect = nDialect;
            var result = Send((short)enCfgCmnd.enCcMsp1, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.DriverConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.DriverConfiguration,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        string.Empty,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.DriverConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.DriverConfiguration,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }
      }

      public CommandResponse ElevatorAccessLevelSpecification(string Mac,short ScpId,short MaxEalvl,short MaxFloor)
      {
            // CC_ELALVLSPC c = new CC_ELALVLSPC();
            // c.scp_number = ScpId;
            // c.max_elalvl = MaxEalvl;
            // c.max_floors = MaxFloor;
            // var result = Send((short)enCfgCmnd.enCcElAlvlSpc, c);

            // CC_ELALVLSPC c = new CC_ELALVLSPC();
            // c.scp_number = ScpId;
            // c.read_type = (short)Type;
            // string comm = $"501 {spec.scp_id} {spec.max_ealvl} {spec.max_floors}";
            // var result = SendASCIICommandAsync(comm);
            // if (result)
            // {
            //       logger.LogInformation(MessageHelper.CommandSuccess(CommandType.ElevatorAccessLevelSpecification, ScpId));
            //       await writer.AddWriterAuditAsync(ScpId, Mac, CommandType.ElevatorAccessLevelSpecification, SCPDLL.scpGetTagLastPosted(ScpId), comm);
            //       return true;

            // }
            // else
            // {
            //       logger.LogError(MessageHelper.CommandUnsuccess(CommandType.ElevatorAccessLevelSpecification, ScpId));
            //       return false;

            // }
            throw new NotImplementedException();
      }

      public CommandResponse ReadsConfiguration(string Mac,short ScpId, WebConfigReadType Type)
      {
            CC_WEB_CONFIG_READ c = new CC_WEB_CONFIG_READ();
            c.scp_number = ScpId;
            c.read_type = (short)Type;
            var result = Send((short)enCfgCmnd.enCcWebConfigRead, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ReadsConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ReadsConfiguration,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        string.Empty,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ReadsConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ReadsConfiguration,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }

      }

      

      public CommandResponse ScpReset(string Mac,short ScpId)
      {

            CC_RESET c = new CC_RESET();
            c.scp_number = ScpId;
            var command = LogMessageHelper.ToString(c);
            var result = Send((short)enCfgCmnd.enCcReset, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ScpReset, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ScpReset,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        string.Empty,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ScpReset, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ScpReset,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }
      }

      public CommandResponse ScpStructureStatusRead(string Mac,short ScpId, List<short> StructureList)
      {
            CC_STRSRQ c = new CC_STRSRQ();
            c.nScpID = ScpId;
            c.nListLength = (short)StructureList.Count();

            for (int i = 0; i < (short)StructureList.Count(); i++)
            {
                  c.nStructId[i] = StructureList.ElementAt(i);
            }
            var command = LogMessageHelper.ToString(c);
            var result = Send((short)enCfgCmnd.enCcStrSRq, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ScpStructureStatusRead, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ScpStructureStatusRead,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        string.Empty,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ScpStructureStatusRead, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ScpStructureStatusRead,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }
      }


      public CommandResponse TimeSet(string Mac,short ScpId)
      {
            CC_TIME c = new CC_TIME();
            c.scp_number = ScpId;
            c.custom_time = 0;
            var result = Send((short)enCfgCmnd.enCcTime, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.TimeSet, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.TimeSet,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        string.Empty,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.TimeSet, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.TimeSet,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }
      }

      public CommandResponse SetScpId(string Mac, short ScpId, short To)
      {
            CC_SCPID c= new CC_SCPID();
            c.scp_number = ScpId;
            c.scp_id = To;
            var result = Send((short)enCfgCmnd.enCcScpID, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ScpSetId, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ScpSetId,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        string.Empty,
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ScpSetId, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ScpSetId,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }
      }
}
