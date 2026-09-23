using Aero.Application.Constants;
using Aero.Application.Enums;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Aero.Domain.Entities;
using Aero.Infrastructure.Helpers;
using Core.Contract.Commands.Device;
using Core.Contract.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using Setting.Contract.DTOs.Setting;
using Setting.Contract.Queries;
using SharedKernel.Enums;
using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class DeviceRepostory(
      IBaseRepository repo,
      IMessageBus bus,
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
            CC_MSP1 c = new CC_MSP1();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.msp1_number = Msp1Number;
            c.port_number = PortNumber;
            c.baud_rate = Baudrate;
            c.reply_time = ReplyTime;
            c.nProtocol = nProtocol;
            c.nDialect = nDialect;
            var result = repo.Send((short)enCfgCmnd.enCcMsp1, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.DriverConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.DriverConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.DriverConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.DriverConfiguration,
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

      public CommandResponse ElevatorAccessLevelSpecification(string Mac, short ScpId, short MaxEAlvl, short MaxFloor)
      {
            // CC_ELALVLSPC c = new CC_ELALVLSPC();
            // c.scp_number = ScpId;
            // c.max_elalvl = MaxEAlvl;
            // c.max_floors = MaxFloor;
            // var result = repo.Send((short)enCfgCmnd.enCcE, c);

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
            var result = SCPDLL.scpConfigCommand($"501 0 {ScpId} {MaxEAlvl} {MaxFloor}");
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ElevatorAccessLevelSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.ReadsConfiguration,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        null,
                       $"501 0 {ScpId} {MaxEAlvl} {MaxFloor}",
                        CommandStatus.PENDING,
                        string.Empty,
                        Vendor.aero,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ElevatorAccessLevelSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.ReadsConfiguration,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                      $"501 0 {ScpId} {MaxEAlvl} {MaxFloor}",
                       CommandStatus.FAILED,
                       string.Empty,
                       Vendor.aero,
                       false
                       );

            }

      }

      public CommandResponse ReadsConfiguration(string Mac, short ScpId, WebConfigReadType Type)
      {
            CC_WEB_CONFIG_READ c = new CC_WEB_CONFIG_READ();
            c.scp_number = ScpId;
            c.read_type = (short)Type;
            var result = repo.Send((short)enCfgCmnd.enCcWebConfigRead, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ReadsConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.ReadsConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ReadsConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.ReadsConfiguration,
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

      public CommandResponse ScpReset(string mac, short scpId)
      {
            CC_RESET c = new CC_RESET();
            c.scp_number = scpId;
            var command = ObjectHelper.ToAsciiString(c);
            var result = repo.Send((short)enCfgCmnd.enCcReset, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ScpReset, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.ScpReset,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ScpReset, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.ScpReset,
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

      public CommandResponse ScpStructureStatusRead(string Mac, short ScpId, List<short> StructureList)
      {
            CC_STRSRQ c = new CC_STRSRQ();
            c.nScpID = ScpId;
            c.nListLength = (short)StructureList.Count();

            for (int i = 0; i < (short)StructureList.Count(); i++)
            {
                  c.nStructId[i] = StructureList.ElementAt(i);
            }
            var result = repo.Send((short)enCfgCmnd.enCcStrSRq, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ScpStructureStatusRead, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.ScpStructureStatusRead,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ScpStructureStatusRead, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.ScpStructureStatusRead,
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

      public CommandResponse SetScpId(string Mac, short ScpId, short To)
      {
            CC_SCPID c = new CC_SCPID();
            c.scp_number = ScpId;
            c.scp_id = To;
            var result = repo.Send((short)enCfgCmnd.enCcScpID, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ScpSetId, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.ScpSetId,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ScpSetId, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.ScpSetId,
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

      public CommandResponse SetTransactionLogIndex(string Mac, short ScpId, bool IsEnable)
      {
            CC_TRANINDEX c = new CC_TRANINDEX();
            c.scp_number = ScpId;
            c.tran_index = IsEnable ? -2 : -1;
            var result = repo.Send((short)enCfgCmnd.enCcTranIndex, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.SetTransactionIndex, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.SetTransactionIndex,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.SetTransactionIndex, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.SetTransactionIndex,
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

      public CommandResponse TimeSet(string Mac, short ScpId)
      {
            CC_TIME c = new CC_TIME();
            c.scp_number = ScpId;
            c.custom_time = 0;
            var result = repo.Send((short)enCfgCmnd.enCcTime, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.TimeSet, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.TimeSet,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.TimeSet, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.TimeSet,
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

      public CommandResponse TransactionLogStatusAsync(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public Status GetStatus(short ScpId)
      {
            return SCPDLL.scpCheckOnline(ScpId) == 1 ? Status.Online : Status.Offline;
      }

      public async Task VerifyMemoryAllocateAsync(string mac,List<StructureStatusMetadata> data,CancellationToken ct= default)
      {
            //
            bool isVerify = true;

            // // Switch
            // foreach (var str in status.sStrSpec)
            // {
            //       switch (str.nStrType)
            //       {
            //             case (short)SCPStructure.SCPSID_TRAN: // 1 Transactions
            //                   isVerify = spec.nTransaction == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_TZ: // 2 Time zones
            //                   isVerify = spec.nTz + 1 == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_HOL: // 3 Holidays
            //                   isVerify = spec.nHol == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_MSP1: // 4 Msp1 ports (SIO drivers)
            //                   // isVerify = spec.n_msp1_port == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_SIO: // 5 SIOs
            //                   isVerify = spec.nSio == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_MP: // 6 Monitor points
            //                   isVerify = spec.nMp == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_CP: // 7 Control points
            //                   isVerify = spec.nCp == str.nRecords;

            //                   break;

            //             case (short)SCPStructure.SCPSID_ACR: // 8 Access control readers
            //                   isVerify = spec.nAcr == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_ALVL: // 9 Access levels
            //                   isVerify = spec.nAlvl == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_TRIG: // 10 Triggers
            //                   isVerify = spec.nTrgr == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_PROC: // 11 Procedures
            //                   isVerify = spec.nProc == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_MPG: // 12 Monitor point groups
            //                   isVerify = spec.nMpg == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_AREA: // 13 Access areas
            //                   //isVerify = spec.AreaBaseApb == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_EAL: // 14 Elevator access levels
            //                   isVerify = spec.MaxElAlvl == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_CRDB: // 15 Cardholder database
            //                   // isVerify = db.nCards == str.nRecords;
            //                   break;

            //             case (short)SCPStructure.SCPSID_FLASH: // 20 FLASH specs
            //                   break;

            //             case (short)SCPStructure.SCPSID_BSQN: // 21 Build sequence number
            //                   break;

            //             case (short)SCPStructure.SCPSID_SAVE_STAT: // 22 Flash save status
            //                   break;

            //             case (short)SCPStructure.SCPSID_MAB1_FREE: // 23 Memory alloc block 1 free
            //                   break;

            //             case (short)SCPStructure.SCPSID_MAB2_FREE: // 24 Memory alloc block 2 free
            //                   break;

            //             case (short)SCPStructure.SCPSID_ARQ_BUFFER: // 26 Access request buffers
            //                   break;

            //             case (short)SCPStructure.SCPSID_PART_FREE_CNT: // 27 Partition memory free info
            //                   break;
                        
            //             case (short)SCPStructure.SCPSID_LOGIN_STANDARD: // 27 Partition memory free info
            //                   break;

            //             case (short)SCPStructure.SCPSID_FILE_SYSTEM: // 27 Partition memory free info
            //                   break;

            //             default:
            //                   break;
            //       }

            //       if (!isVerify)
            //             break;
            // }

            isVerify = !data.Any(x => x.Status == DeviceConfigurationStatus.unsync);

            await bus.SendAsync(new ConfigurationStatusCommand(mac,isVerify));

      }
}