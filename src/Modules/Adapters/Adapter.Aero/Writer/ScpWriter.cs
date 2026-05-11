using System;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Persistences.Entities;
using Adapter.Aero.Writer;
using AeroAdapter.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;

namespace AeroAdapter.Infrastructure.Writer;

public sealed class ScpWriter(ILogger<ScpWriter> logger, IWriterRepository writer) : BaseWriter, IScpWriter
{
      public async Task<bool> AccessDatabaseSpecification(short ScpId, string Mac, AccessDatabaseSpecification spec)
      {
            CC_SCP_ADBS c = new CC_SCP_ADBS();
            c.lastModified = 0;
            c.nScpID = ScpId;
            c.nCards = spec.n_card;
            c.nAlvl = spec.n_alvl;
            c.nPinDigits = spec.n_pin_digits;
            c.bIssueCode = spec.b_issue_code;
            c.bApbLocation = spec.b_apb_location;
            c.bActDate = spec.b_act_date;
            c.bDeactDate = spec.b_deact_date;
            c.bVacationDate = spec.b_vacation_date;
            c.bUpgradeDate = spec.b_upgrade_date;
            c.bUserLevel = spec.b_user_level;
            c.bUseLimit = spec.b_use_limit;
            c.bSupportTimedApb = spec.b_support_time_apb;
            c.nTz = spec.n_tz;
            c.bAssetGroup = spec.b_asset_group;
            c.nHostResponseTimeout = spec.n_host_response_timeout;
            c.nMxmTypeIndex = 0;
            c.nAlvlUse4Arq = spec.n_alvl_use4arg;
            c.nFreeformBlockSize = 0;
            c.nEscortTimeout = spec.n_escort_timeout;
            c.nMultiCardTimeout = spec.n_multi_card_timeout;
            c.nAssetTimeout = 0;
            var result = Send((short)enCfgCmnd.enCcScpAdbSpec, c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.AccessDatabaseSpecification, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.AccessDatabaseSpecification, SCPDLL.scpGetTagLastPosted(ScpId), MessageHelper.ToString(c));
                  return true;

            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.AccessDatabaseSpecification, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.AccessDatabaseSpecification, 0, MessageHelper.ToString(c), WriterStatus.FAILED.ToString());
                  return false;

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

      public async Task<bool> DriverConfiguration(short ScpId, string Mac, DriverConfiguration config)
      {

            CC_MSP1 c = new CC_MSP1();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.msp1_number = config.msp1_number;
            c.port_number = config.port_number;
            c.baud_rate = config.baudrate;
            c.reply_time = config.reply_time;
            c.nProtocol = config.n_protocol;
            c.nDialect = config.n_dialect;
            var result = Send((short)enCfgCmnd.enCcMsp1, c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.DriverConfiguration, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.DriverConfiguration, SCPDLL.scpGetTagLastPosted(ScpId), MessageHelper.ToString(c));
                  return true;

            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.DriverConfiguration, ScpId));
                  return false;

            }
      }

      public async Task<bool> ElevatorAccessLevelSpecification(short ScpId,string Mac, ElevatorAccessLevelSpecification spec)
      {
            // CC_ELALVLSPC c = new CC_ELALVLSPC();
            // c.scp_number = ScpId;
            // c.read_type = (short)Type;
            string comm = $"501 {spec.scp_id} {spec.max_ealvl} {spec.max_floors}";
            var result = SendASCIICommandAsync(comm);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.ElevatorAccessLevelSpecification, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.ElevatorAccessLevelSpecification, SCPDLL.scpGetTagLastPosted(ScpId), comm);
                  return true;

            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.ElevatorAccessLevelSpecification, ScpId));
                  return false;

            }
      }

      public async Task<bool> ReadsConfiguration(short ScpId, string Mac, WebConfigReadType Type)
      {
            CC_WEB_CONFIG_READ c = new CC_WEB_CONFIG_READ();
            c.scp_number = ScpId;
            c.read_type = (short)Type;
            var result = Send((short)enCfgCmnd.enCcWebConfigRead, c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.ReadsConfiguration, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.ReadsConfiguration, SCPDLL.scpGetTagLastPosted(ScpId), MessageHelper.ToString(c));
                  return true;

            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.ReadsConfiguration, ScpId));
                  return false;

            }

      }

      public async Task<bool> ScpDeviceSpecification(short ScpId,string Mac, ScpDeviceSpecification spec)
      {

            CC_SCP_SCP c = new CC_SCP_SCP();
            c.lastModified = 0;
            c.number = ScpId;
            c.ser_num_low = 0;
            c.ser_num_high = 0;
            c.rev_major = 0;
            c.rev_minor = 0;
            c.nMsp1Port = spec.n_msp1_port;
            c.nTransactions = spec.n_transcations;
            c.nSio = spec.n_sio;
            c.nMp = spec.n_mp;
            c.nCp = spec.n_cp;
            c.nAcr = spec.n_acr;
            c.nAlvl = spec.n_alvl;
            c.nTrgr = spec.n_trgr;
            c.nProc = spec.n_proc;
            c.gmt_offset = spec.gmt_offset;
            c.nDstID = spec.n_dst_id;
            c.nTz = spec.n_tz;
            c.nHol = spec.n_hol;
            c.nMpg = spec.n_mpg;
            c.nTranLimit = spec.n_tran_limit;
            c.nAuthModType = 0;
            c.nOperModes = spec.n_oper_mode;
            c.oper_type = spec.oper_type;
            c.nLanguages = spec.n_language;
            c.nSrvcType = 0;
            var result = Send((short)enCfgCmnd.enCcScpScp, c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.ScpDeviceSpecification, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.ScpDeviceSpecification, SCPDLL.scpGetTagLastPosted(ScpId), MessageHelper.ToString(c));
                  return true;

            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.ScpDeviceSpecification, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.ScpDeviceSpecification, 0, MessageHelper.ToString(c), WriterStatus.FAILED.ToString());
                  return false;

            }

      }

      public async Task<bool> SCPStructureStatusRead(short ScpId, string Mac, List<short> StructureList)
      {
            CC_STRSRQ c = new CC_STRSRQ();
            c.nScpID = ScpId;
            c.nListLength = (short)StructureList.Count();
            for (int i = 0; i < (short)StructureList.Count(); i++)
            {
                  c.nStructId[i] = StructureList.ElementAt(i);
            }
            var result = Send((short)enCfgCmnd.enCcStrSRq, c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.SCPStructureStatusRead, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.SCPStructureStatusRead, SCPDLL.scpGetTagLastPosted(ScpId), MessageHelper.ToString(c));
                  return true;

            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.SCPStructureStatusRead, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.SCPStructureStatusRead, 0, MessageHelper.ToString(c), WriterStatus.FAILED.ToString());
                  return false;

            }
      }

      public bool SendASCIICommandAsync(string Command)
      {
            return SCPDLL.scpConfigCommand(Command);
      }

      public async Task<bool> TimeSet(short ScpId, string Mac)
      {
            CC_TIME c = new CC_TIME();
            c.scp_number = ScpId;
            c.custom_time = 0;
            var result = Send((short)enCfgCmnd.enCcTime, c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.TimeSet, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.TimeSet, SCPDLL.scpGetTagLastPosted(ScpId), MessageHelper.ToString(c));
                  return true;

            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.TimeSet, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.TimeSet, 0, MessageHelper.ToString(c), WriterStatus.FAILED.ToString());
                  return false;

            }
      }

      public async Task<bool> WebConfigRead(short ScpId, string Mac, WebConfigReadType Type)
      {
            CC_WEB_CONFIG_READ c = new CC_WEB_CONFIG_READ();
            c.scp_number = ScpId;
            c.read_type = (short)Type;
            var result = Send((short)enCfgCmnd.enCcWebConfigRead, c);
            if (result)
            {
                  logger.LogInformation(MessageHelper.CommandSuccess(WriterType.ReadWebConfig, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.ReadWebConfig, SCPDLL.scpGetTagLastPosted(ScpId), MessageHelper.ToString(c));
                  return true;

            }
            else
            {
                  logger.LogError(MessageHelper.CommandUnsuccess(WriterType.ReadWebConfig, ScpId));
                  await writer.AddWriterAuditAsync(ScpId, Mac, WriterType.ReadWebConfig, 0, MessageHelper.ToString(c), WriterStatus.FAILED.ToString());
                  return false;

            }

      }
}
