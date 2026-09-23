using Aero.Application.Enums;
using Aero.Application.Helpers;
using Aero.Application.Metadata.Device;
using Aero.Domain.Entities;
using Setting.Contract.DTOs.Setting;
using SharedKernel.Enums;

namespace Aero.Infrastructure.Helpers;

public static class ReplyMessageHelper
{
  // public static string ToJsonString(object obj)
  // {   
  //     if(obj == null)
  //         return "{}";

  //     var type = obj.GetType();
  //     var props = type.GetProperties();
  //     var pairs = props.Select(p =>
  //     {
  //         var value = p.GetValue(obj);
  //         return $"\"{p.Name}\" : \"{value}\"";
  //     });

  //     return  $"[{type.Name}] " + "{" + string.Join(", ",pairs) + " }";
  // }

  // public static string ToString(object obj)
  // {
  //     if(obj == null)
  //         return string.Empty;

  //     var sb = new StringBuilder();
  //     var type = obj.GetType();
  //     PropertyInfo[] props = type.GetProperties();

  //     foreach(PropertyInfo prop in props)
  //     {
  //         var value = prop.GetValue(obj,null);
  //         sb.AppendJoin(", ",$"{prop.Name}: {value}");
  //     }

  //     return $"[{type.Name}] " + sb.ToString();


  // }

  public static List<StructureStatusMetadata> BuildStructureStatus(ReplyMessage.SCPReplyStrStatus status,AeroDriverSettingDto driver)
  {

    var data = new List<StructureStatusMetadata>();
    foreach (var str in status.sStrSpec)
    {
      switch (str.nStrType)
      {
        case (short)SCPStructure.SCPSID_TRAN:
          data.Add(
            new StructureStatusMetadata(
              1,
              driver.nTransaction,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nTransaction == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_TZ:
          data.Add(
            new StructureStatusMetadata(
              2,
              driver.nTz,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nTz + 1 == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_HOL:
          data.Add(
            new StructureStatusMetadata(
             3,
              driver.nHol,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nHol == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_MSP1:
          data.Add(
            new StructureStatusMetadata(
              4,
              driver.nMsp1Port,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_SIO:
          data.Add(
            new StructureStatusMetadata(
             5,
              driver.nSio,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nSio == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_MP:
          data.Add(
            new StructureStatusMetadata(
              6,
              driver.nMp,
              str.nRecords,
              str.nRecSize,
              str.nActive,
               driver.nMp == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_CP:
          data.Add(
            new StructureStatusMetadata(
              7,
              driver.nCp,
              str.nRecords,
              str.nRecSize,
              str.nActive,
               driver.nCp == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_ACR:
          data.Add(
            new StructureStatusMetadata(
              8,
              driver.nAcr,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nAcr == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_ALVL:
          data.Add(
            new StructureStatusMetadata(
             9,
              driver.nAlvl,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nAlvl == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_TRIG:
          data.Add(
            new StructureStatusMetadata(
             10,
              driver.nTrgr,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nTrgr == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_PROC:
          data.Add(
            new StructureStatusMetadata(
              11,
              driver.nProc,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nProc == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_MPG:
          data.Add(
            new StructureStatusMetadata(
              12,
              driver.nMpg,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.nMpg == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_AREA:
          data.Add(
            new StructureStatusMetadata(
              13,
              128,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_EAL:
          data.Add(
            new StructureStatusMetadata(
              14,
              driver.MaxElAlvl,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              driver.MaxElAlvl == str.nRecords ? DeviceConfigurationStatus.sync :DeviceConfigurationStatus.unsync
            )
          );
          break;
        case (short)SCPStructure.SCPSID_CRDB:
          data.Add(
            new StructureStatusMetadata(
              15,
              driver.nCards,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_FLASH:
          data.Add(
            new StructureStatusMetadata(
              20,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_BSQN:
          data.Add(
            new StructureStatusMetadata(
              21,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_SAVE_STAT:
          data.Add(
            new StructureStatusMetadata(
             22,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_MAB1_FREE:
          data.Add(
            new StructureStatusMetadata(
              23,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_MAB2_FREE:
          data.Add(
            new StructureStatusMetadata(
              24,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_ARQ_BUFFER:
          data.Add(
            new StructureStatusMetadata(
             26,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_PART_FREE_CNT:
          data.Add(
            new StructureStatusMetadata(
              27,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_LOGIN_STANDARD:
          data.Add(
            new StructureStatusMetadata(
             33,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        case (short)SCPStructure.SCPSID_FILE_SYSTEM:
          data.Add(
            new StructureStatusMetadata(
             35,
              0,
              str.nRecords,
              str.nRecSize,
              str.nActive,
              DeviceConfigurationStatus.info
            )
          );
          break;
        default:
          break;
      }
    }

    return data;
  }



  public static string BuildNakMessage(ReplyMessage message)
  {
    switch (message.nak.reason)
    {
      case 0:
        return $"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Invalid Packet Header, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}";
      case 1:
      case 2:
      case 3:
        return $"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Invalid command type (firmware revision mismatch), data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}";

      case 4:
        return $"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: command content error, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}";

      case 5:
        return $"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Cannot execute - requires password logon, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}";

      case 6:
        return $"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: This port is in standby mode and cannot execute this command, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}";

      case 7:
        return $"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Failed logon - password and/or encryption key, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}";

      case 8:
        return $"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: command not accepted, controller is running in degraded mode and only a limited number of commands are accepted, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}";

      default:
        return $"{DateTime.UtcNow.ToLocalTime()} [Nak] reason: Unknown reason code {message.nak.reason}, data: {message.nak.data}, command: {UtilitiesHelper.ByteToHexStr(message.nak.command)}, description_code: {message.nak.description_code}";
    }
  }

  public static string CommStatusMessage(ReplyMessage message)
  {
    return $"{DateTime.UtcNow.ToLocalTime()} [CommStatus] Status: {message.comm.status}, Status Desc: {DescriptionHelper.GetReplyStatusDesc(message.comm.status)} Error: {message.comm.error_code} Error Desc: {DescriptionHelper.GetErrorCodeDesc((int)message.comm.error_code)} Channel ID: {message.comm.nChannelId}";
  }

  public static string IdReportMessage(ReplyMessage message)
  {
    return $"{DateTime.UtcNow.ToLocalTime()} [ID Report] SCP Version: {message.id.device_ver}, Firmware Version: {message.id.sft_rev_major}.{message.id.sft_rev_minor}, Serial Number: {message.id.serial_number}, Mac Address: {UtilitiesHelper.ByteToHexStr(message.id.mac_addr)}, TLS Status : {message.id.tls_status}";
  }


  public static string TranStatusMessage(ReplyMessage message)
  {
    return $"{DateTime.UtcNow.ToLocalTime()} [Transaction Status] Capacity: {message.tran_sts.capacity}, Oldest: {message.tran_sts.oldest}, Last report: {message.tran_sts.last_loggd}, Status: {message.tran_sts.disabled}, Status Desc: {DescriptionHelper.GetStatusTranReportDesc(message.tran_sts.disabled)}";
  }

  public static string Msp1DrvrMessage(ReplyMessage message)
  {
    return $"{DateTime.UtcNow.ToLocalTime()} [SrMsp1Drvr] Number: {message.sts_drvr.number}, Port: {message.sts_drvr.port}, Mode: {(message.sts_drvr.mode == 0 ? "Disabled" : "Enabled")}, Baudrate: {message.sts_drvr.baud_rate}";
  }


}
