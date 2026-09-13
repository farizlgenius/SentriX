using Aero.Application.Constants;
using Aero.Application.Enums;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using Core.Contract.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class DeviceRepostory(
      IBaseRepository repo,
      ILogger<DeviceRepostory> logger) : IDeviceRepository
{
      public bool SystemLevelSpecification(
            short nPorts,
            short nScps
      )
      {
            CC_SYS c = new CC_SYS();
            c.nPorts = nPorts;
            c.nScps = nScps;
            c.nTimezones = 0;
            c.nHolidays = 0;
            c.bDirectMode = 1;
            c.debug_rq = 0;
            for (int i = 0; i < c.nDebugArg.Length; i++)
            {
                  c.nDebugArg[i] = 0;
            }
            var result = repo.Send((short)enCfgCmnd.enCcSystem, c);
            if (result)
                  logger.LogInformation("System level specification command sent successfully.");
            return result;
      }
      public bool CreateChannel(
            short nChannelId,
            short cType,
            short cPort
      )
      {
            CC_CHANNEL c = new CC_CHANNEL();
            c.nChannelId = nChannelId;
            c.cType = cType;
            c.cPort = cPort;
            c.baud_rate = 0;
            c.timer1 = 3000;
            c.timer2 = 0;
            for (int i = 0; i < c.cModemId.Length; i++)
            {
                  c.cModemId[i] = '\0';
            }
            c.cRTSMode = 0;
            var result = repo.Send((short)enCfgCmnd.enCcCreateChannel, c);
            if (result)
                  logger.LogInformation("Create channel command sent successfully.");
            return result;
      }
      public CommandResponse AccessDatabaseSpecification(string Mac, short ScpId, int nCard, short nAlvl, short nPinDigit, short bIssueCode, short bApbLocation, short bActDate, short bDeactDate, short bVacationDate, short bUpgradeDate, short bUserLevel, short bUseLimit, short bSupportTimeApb, short nTz, short bAssetGroup, short nHostResponseTimeout, short nAvlUse4Arg, short nEscortTimeout, short mMultiCardTimeout)
      {
            CC_SCP_ADBS c = new CC_SCP_ADBS();
            c.lastModified = 0;
            c.nScpID = ScpId;
            c.nCards = nCard;
            c.nAlvl = nAlvl;
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
            var result = repo.Send((short)enCfgCmnd.enCcScpAdbSpec, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.AccessDatabaseSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.AccessDatabaseSpecification,
                        SCPDLL.scpGetTagLastPosted(ScpId),
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.AccessDatabaseSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.AccessDatabaseSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       ObjectHelper.ToAsciiString(c),
                       CommandStatus.FAILED,
                       string.Empty,
                       Vendor.aero,
                       false
                       );

            }
      }

      public CommandResponse AsciiCommandAsync(string Mac, short ScpId, string Command)
      {
            throw new NotImplementedException();
      }



      public CommandResponse DeleteScp(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public CommandResponse DetachScpFromChannel(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public CommandResponse DriverConfiguration(string Mac, short ScpId, short Msp1Number, short PortNumber, short Baudrate, short ReplyTime, short nProtocol, short nDialect)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ElevatorAccessLevelSpecification(string Mac, short ScpId, short MaxEAlvl, short MaxFloor)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ReadsConfiguration(string Mac, short ScpId, WebConfigReadType Type)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ScpDeviceSpecification(string Mac, short ScpId, short nMsp1Port, int nTransaction, short nSio, short nMp, short nCp, short nAcr, short nAlvl, short nTrgr, short nProc, short gmtOffet, short nDstId, short nTz, short nHol, short nMpg, int nTranLimit, short nOperMode, short operType, short nLanguage)
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
            var result = repo.Send((short)enCfgCmnd.enCcScpScp, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ScpDeviceSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.ScpDeviceSpecification,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        null,
                        ObjectHelper.ToAsciiString(c),
                        SharedKernel.Enums.CommandStatus.PENDING,
                        string.Empty,
                        Vendor.aero,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ScpDeviceSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.ScpDeviceSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       ObjectHelper.ToAsciiString(c),
                       CommandStatus.FAILED,
                       string.Empty,
                       Vendor.aero,
                       false
                       );

            }
      }

      public CommandResponse ScpReset(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ScpStructureStatusRead(string Mac, short ScpId, List<short> StructureList)
      {
            throw new NotImplementedException();
      }

      public CommandResponse SetScpId(string Mac, short ScpId, short To)
      {
            throw new NotImplementedException();
      }

      public CommandResponse SetTransactionLogIndexAsync(string Mac, short ScpId, bool IsEnable)
      {
            throw new NotImplementedException();
      }

      public CommandResponse TimeSet(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public CommandResponse TransactionLogStatusAsync(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }
}