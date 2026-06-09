using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Markup;
using Adapter.Aero.Model;
using Events.Contract.Constants;
using HID.Aero.ScpdNet.Wrapper;

namespace Adapter.Aero.Helpers;

public static class TranHelper
{
      public static string GetEventModuleFromTranType(tranSrc src)
      {
            return src switch
            {
                  tranSrc.tranSrcScpDiag => EventModule.DEVICE,
                  tranSrc.tranSrcScpCom => EventModule.DEVICE,
                  tranSrc.tranSrcScpLcl => EventModule.DEVICE,
                  tranSrc.tranSrcSioDiag => EventModule.MODULE,
                  tranSrc.tranSrcSioCom => EventModule.MODULE,
                  tranSrc.tranSrcSioTmpr => EventModule.MODULE,
                  tranSrc.tranSrcSioPwr => EventModule.MODULE,
                  tranSrc.tranSrcMP => EventModule.INPUT,
                  tranSrc.tranSrcCP => EventModule.OUTPUT,
                  tranSrc.tranSrcACR => EventModule.DOOR,
                  tranSrc.tranSrcAcrTmpr => EventModule.DOOR,
                  tranSrc.tranSrcAcrDoor => EventModule.DOOR,
                  tranSrc.tranSrcAcrRex0 => EventModule.DOOR,
                  tranSrc.tranSrcAcrRex1 => EventModule.DOOR,
                  tranSrc.tranSrcTimeZone => EventModule.TIMEZONE,
                  tranSrc.tranSrcProcedure => EventModule.PROCEDURE,
                  tranSrc.tranSrcTrigger => EventModule.TRIGGER,
                  tranSrc.tranSrcTrigVar => EventModule.TRIGGER,
                  tranSrc.tranSrcMPG => EventModule.MPG,
                  tranSrc.tranSrcArea => EventModule.AREA,
                  tranSrc.tranSrcAcrTmprAlt => EventModule.DOOR,
                  tranSrc.tranSrcSioEmg => EventModule.MODULE,
                  tranSrc.tranSrcLoginService => EventModule.WEB,
                  _ => string.Empty
            };
      }
      public static string GetCode(tranSrc Src, tranType Type, int Code)
      {
            switch (Type)
            {
                  case tranType.tranTypeSys:
                        return Code switch
                        {
                              1 => "Power up Diag",
                              2 => "Host Offline",
                              3 => "Host Online",
                              4 => "Transaction Count Exceed",
                              5 => "Database save complete",
                              6 => "Card database save complete",
                              7 => "Card database cleared due to SRAM buffer overflow",
                              _ => string.Empty
                        };
                  case tranType.tranTypeSioComm:
                        return Code switch
                        {
                              1 => "Disabled",
                              2 => "Timeout",
                              3 => "Invalid Identification",
                              4 => "Too long",
                              5 => "Online",
                              6 => "hexLoad Report",
                              _ => "Offline"
                        };
                  case tranType.tranTypeCardBin:
                        return Code switch
                        {
                              1 => "Access Denied,Invalid card format",
                              _ => string.Empty
                        };
                  case tranType.tranTypeCardBcd:
                        return Code switch
                        {
                              1 => "Access denied,Invalid card format,forward read",
                              2 => "Access denied,Invalid card format,reverse read",
                              _ => string.Empty

                        };
                  case tranType.tranTypeCardFull:
                  case tranType.tranTypeDblCardFull:
                  case tranType.tranTypeI64CardFull:
                  case tranType.tranTypeI64CardFullIc32:
                        return Code switch
                        {
                              1 => "Rejected",
                              2 => "Accepted",
                              3 => "Rejected",
                              4 => "Rejected",
                              5 => "Rejected",
                              6 => "Rejected",
                              7 => "Granted",
                              8 => "Granted",
                              9 => "Denied",
                              10 => "Reporting",
                              11 => "Denied",
                              12 => "Denied",
                              13 => "Rejected",
                              _ => string.Empty
                        };
                  case tranType.tranTypeCardID:
                  case tranType.tranTypeDblCardID:
                  case tranType.tranTypeI64CardID:
                        return Code switch
                        {
                              1 => "Rejected",
                              2 => "Rejected",
                              3 => "Rejected",
                              4 => "Rejected",
                              5 => "Rejected",
                              6 => "Rejected",
                              7 => "Granted",
                              8 => "Granted",
                              9 => "Rejected",
                              10 => "Granted",
                              11 => "Granted",
                              12 => "Granted",
                              13 => "Granted",
                              14 => "Denied",
                              15 => "Denied",
                              16 => "Denied",
                              17 => "Denied",
                              18 => "Denied",
                              21 => "Granting",
                              24 => "Rejected",
                              25 => "Reserved",
                              26 => "Reserved",
                              27 => "Reserved",
                              29 => "Rejected",
                              30 => "Rejected",
                              31 => "Granted",
                              32 => "Granted",
                              39 => "Granting",
                              40 => "Rejected",
                              41 => "Rejected",
                              _ => string.Empty
                        };
                  case tranType.tranTypeHostCardFullPin:
                        return Code switch
                        {
                              1 => "Reporting",
                              _ => string.Empty
                        };
                  case tranType.tranTypeCoS:
                        switch (Src)
                        {
                              case tranSrc.tranSrcAcrTmpr:
                                    return Code switch
                                    {
                                          0 => "Online",
                                          1 => "Online",
                                          2 => "N/A",
                                          3 => "Broken",
                                          _ => string.Empty
                                    };
                              default:
                                    return Code switch
                                    {
                                          1 => "Disconnected",
                                          2 => "Unknown",
                                          3 => "Secure",
                                          4 => "Alarm",
                                          5 => "Fault",
                                          6 => "Exit delay",
                                          7 => "Entry delay",
                                          _ => string.Empty
                                    };

                        }
                  case tranType.tranTypeREX:
                        return Code switch
                        {
                              1 => "REX Exit",
                              2 => "REX Exit",
                              3 => "REX Exit",
                              4 => "REX Host Request",
                              5 => "REX Host Request",
                              6 => "REX Host Request",
                              9 => "REX Exit",
                              _ => string.Empty
                        };
                  case tranType.tranTypeCoSDoor:
                        return Code switch
                        {
                              1 => "Disconnected",
                              2 => "Unknown",
                              3 => "Secure",
                              4 => "Alarm",
                              5 => "Fault",
                              _ => string.Empty
                        };
                  case tranType.tranTypeProcedure:
                        return Code switch
                        {
                              1 => "Cancel",
                              2 => "Execute",
                              3 => "Resume",
                              4 => "Execute",
                              5 => "Execute",
                              6 => "Execute",
                              7 => "Resume",
                              8 => "Resume",
                              9 => "Resume",
                              10 => "NOP",
                              _ => string.Empty
                        };
                  case tranType.tranTypeUserCmnd:
                        return Code switch
                        {
                              1 => "User Command",
                              _ => string.Empty,
                        };
                  case tranType.tranTypeActivate:
                        return Code switch
                        {
                              1 => "Inactive",
                              2 => "Active",
                              _ => string.Empty
                        };
                  case tranType.tranTypeAcr:
                        return Code switch
                        {
                              1 => "Disabled",
                              2 => "Unlocked",
                              3 => "Locked",
                              4 => "FAC Only",
                              5 => "Card Only",
                              6 => "Card and PIN",
                              7 => "PIN or Card",
                              _ => string.Empty
                        };
                  case tranType.tranTypeMpg:
                        return Code switch
                        {
                              1 => "First disarm",
                              2 => "Subsequent",
                              3 => "Override Armed",
                              4 => "Override Disarmed",
                              5 => "Force Arm",
                              6 => "Force Arm",
                              7 => "Arm",
                              8 => "Arm",
                              9 => "Arm",
                              10 => "Override Arm",
                              11 => "Override Arm",
                              _ => string.Empty
                        };
                  case tranType.tranTypeArea:
                        return Code switch
                        {
                              1 => "Disabled",
                              2 => "Enabled",
                              3 => "Reached Zero",
                              4 => "Reached Min",
                              5 => "Reached Max",
                              6 => "Reached Max",
                              7 => "Mode Changed",
                              _ => string.Empty
                        };
                  case tranType.tranTypeUseLimit:
                        return Code switch
                        {
                              1 => "Limit Changed",
                              _ => string.Empty
                        };
                  case tranType.tranTypeWebActivity:
                        return "Web Activity";
                  case tranType.tranTypeOperatingMode:
                        return Code switch
                        {
                              1 => "Change to 0",
                              2 => "Change to 1",
                              3 => "Change to 2",
                              4 => "Change to 3",
                              5 => "Change to 4",
                              6 => "Change to 5",
                              7 => "Change to 6",
                              8 => "Change to 7",
                              _ => string.Empty
                        };
                  case tranType.tranTypeCoSElevator:
                        return Code switch
                        {
                              1 => "Secure",
                              2 => "Public",
                              3 => "Disabled",
                              _ => string.Empty
                        };
                  case tranType.tranTypeFileDownloadStatus:
                        return Code switch
                        {
                              1 => "Transfer",
                              2 => "Transfer",
                              3 => "Delete",
                              4 => "Delete",
                              5 => "OSDP",
                              6 => "OSDP",
                              7 => "OSDP",
                              8 => "OSDP",
                              _ => string.Empty
                        };
                  case tranType.tranTypeCoSElevatorAccess:
                        return Code switch
                        {
                              1 => "Elevator access",
                              _ => string.Empty
                        };
                  case tranType.tranTypeAcrExtFeatureStls:
                        return Code switch
                        {
                              1 => "Extended status updated",
                              _ => string.Empty
                        };
                  case tranType.tranTypeAcrExtFeatureCoS:
                        return Code switch
                        {
                              3 => "Secure",
                              4 => "Alarm",
                              _ => string.Empty
                        };
                  default:
                        return string.Empty;

            }
      }

      public static string GetRemark(SCPReplyMessageDto msg)
      {
            switch (msg.tran.tran_type)
            {
                  case (short)tranType.tranTypeSys:
                        return TypeSysRemark(msg.tran.tran_code, msg.tran.sys.error_code);
                  case (short)tranType.tranTypeSioComm:
                        return TypeSioCommRemark();
                  case (short)tranType.tranTypeCardBin:
                        return TypeCardBinRemark(msg.tran.tran_code, msg.tran.c_bin.bit_count, msg.tran.c_bin.bit_array);
                  case (short)tranType.tranTypeCardBcd:
                        return TypeCardBcdRemark(msg.tran.tran_code, msg.tran.c_bcd.digit_count, msg.tran.c_bcd.bcd_array);
                  case (short)tranType.tranTypeCardFull:
                  case (short)tranType.tranTypeDblCardFull:
                  case (short)tranType.tranTypeI64CardFull:
                  case (short)tranType.tranTypeI64CardFullIc32:
                        return TypeCardFullRemark(msg.tran.tran_code);
                  case (short)tranType.tranTypeCardID:
                  case (short)tranType.tranTypeDblCardID:
                  case (short)tranType.tranTypeI64CardID:
                        return TypeCardIDRemark(msg.tran.tran_code);
                  case (short)tranType.tranTypeHostCardFullPin:
                        return TypeHostCardFullPinRemark(msg.tran.tran_code);
                  case (short)tranType.tranTypeCoS:
                        return TypeCoSRemark(msg.tran.tran_code, (tranSrc)msg.tran.source_type, msg.tran.cos.status);
                  case (short)tranType.tranTypeREX:
                        return TypeREXRemark(msg.tran.tran_code);
                  case (short)tranType.tranTypeCoSDoor:
                        return TypeCosDoor(msg.tran.door.door_status, msg.tran.door.ap_status);
                  case (short)tranType.tranTypeProcedure:
                        return TypeProcedureRemark(msg.tran.tran_code);
                  case (short)tranType.tranTypeUserCmnd:
                        return TypeUserCmnd(msg.tran.usrcmd.keys);
                  // case (short)tranType.tranTypeAcr:
                  //       return TypeAcrRemark(msg.tran.acr.actl_flags, msg.tran.acr.actl_flags_e);
                  case (short)tranType.tranTypeMpg:
                        return TypeMpgRemark(msg.tran.tran_code);
                  case (short)tranType.tranTypeArea:
                        return TypeAreaRemark(msg.tran.area.status, msg.tran.area.occupancy);
                  case (short)tranType.tranTypeUseLimit:
                        return TypeUseLimitRemark(msg.tran.c_uselimit.use_count);
                  case (short)tranType.tranTypeWebActivity:
                        return TypeWebActivity(msg.tran.tran_code);
                  case (short)tranType.tranTypeCoSElevator:
                        return TypeCosElevatorRemark(msg.tran.floor.floorNumber);
                  case (short)tranType.tranTypeFileDownloadStatus:
                        return TypeFileDownloadStatusRemark(
                              msg.tran.tran_code,
                              msg.tran.file_download.fileType,
                              msg.tran.file_download.fileName
                              );
                  // case (short)tranType.tranTypeCoSElevatorAccess:
                  //       return TypeCosElevatorAccessRemark(
                  //             msg.tran.elev_access.cardholder_id,
                  //             msg.tran.elev_access.floors,
                  //             msg.tran.elev_access.nCardFormat
                  //             );
                  default:
                        return string.Empty;
            }
      }

      private static string TypeSysRemark(int Code, int Error)
      {

            if (Code == 1)
            {
                  StringBuilder result = new StringBuilder();
                  if ((Error & (1 << 2)) != 0) result.Append("External Reset ");
                  if ((Error & (1 << 3)) != 0) result.Append("Power on Reset ");
                  if ((Error & (1 << 4)) != 0) result.Append("Watchdog Timer ");
                  if ((Error & (1 << 5)) != 0) result.Append("Watchdog Timer ");
                  if ((Error & (1 << 6)) != 0) result.Append("Watchdog Timer ");
                  if ((Error & (1 << 7)) != 0) result.Append("Watchdog Timer ");
                  return result.ToString();
            }
            else
            {
                  return string.Empty;
            }

      }

      private static string TypeSioCommRemark()
      {
            return string.Empty;
      }

      private static string TypeCardBinRemark(int Code, int bit, byte[] arr)
      {
            switch (Code)
            {
                  case 1:
                        return $"Invalid card format, Bit: {bit}, Data: {UtilitiesHelper.ByteToHexStr(arr)}";
                  default:
                        return string.Empty;
            }

      }

      private static string TypeCardBcdRemark(int Code, int digit, byte[] arr)
      {
            switch (Code)
            {
                  case 1:
                        return $"Invalid card format,forward read, Digit: {digit}, Data: {UtilitiesHelper.ByteToHexStr(arr)}";
                  case 2:
                        return $"Invalid card format,reverse read, Digit: {digit}, Data: {UtilitiesHelper.ByteToHexStr(arr)}";
                  default:
                        return string.Empty;
            }
      }

      private static string TypeCardFullRemark(int Code)
      {
            switch (Code)
            {
                  case 1:
                        return "Door 'locked'";
                  case 2:
                        return "Door 'unlocked'";
                  case 3:
                        return "Invalid facility code";
                  case 4:
                        return "Invalid facility code extension";
                  case 5:
                        return "Card not found";
                  case 6:
                        return "Invalid issue code";
                  case 7:
                        return "Facility code verified,not used";
                  case 8:
                        return "Facility code verified,door used";
                  case 9:
                        return "Asked for host approval,then timed out";
                  case 10:
                        return "Reporting this card 'about to get granted'";
                  case 11:
                        return "Count exceeded";
                  case 12:
                        return "Ask for host approval,then denied";
                  case 13:
                        return "Airlock is busy";
                  default:
                        return string.Empty;
            }
      }

      private static string TypeCardIDRemark(int Code)
      {
            return Code switch
            {
                  1 => "Deactivated",
                  2 => "Before Active date",
                  3 => "Expired",
                  4 => "Invalid time",
                  5 => "Invalid PIN",
                  6 => "Antipassback violation",
                  7 => "Antipassback violation, not used",
                  8 => "Antipassback violation, used",
                  9 => "Duress code detected",
                  10 => "Duress, used",
                  11 => "Duress, not used",
                  12 => "Full test, not used",
                  13 => "Full test, used",
                  14 => "Never allowed",
                  15 => "No second card present",
                  16 => "Occupancy limit reached",
                  17 => "Area disabled",
                  18 => "Use limit",
                  21 => "Used/not used transaction will follow",
                  24 => "No Escort card presented",
                  25 => "Reserved",
                  26 => "Reserved",
                  27 => "Reserved",
                  29 => "Airlock busy",
                  30 => "Incomplete Card & PIN sequence",
                  31 => "Double card event",
                  32 => "Double card event while in uncontrolled state (locked/unlocked)",
                  39 => "Require escort, Pending escort card",
                  40 => "Violated minimum occupancy count",
                  41 => "Card pending at another reader",
                  _ => string.Empty
            };
      }

      private static string TypeHostCardFullPinRemark(int Code)
      {
            return Code switch
            {
                  1 => "Reporting this Card and PIN is 'requesting access'",
                  _ => string.Empty
            };
      }

      private static string TypeCoSRemark(int Code, tranSrc src, int Status)
      {
            switch (src)
            {
                  case tranSrc.tranSrcAcrTmpr:
                        return Code switch
                        {
                              0 => "Tamper inactive",
                              1 => "Tamper active",
                              2 => "N/A",
                              3 => "Communication broken",
                              _ => string.Empty
                        };
                  default:
                        return DecodeTypeCos(Status, src);
            }
      }

      private static string DecodeTypeCos(int status, tranSrc src)
      {
            List<string> result = new List<string>();


            switch (src)
            {
                  case tranSrc.tranSrcSioPwr:
                  case tranSrc.tranSrcSioTmpr:
                        switch (status & 0x07)
                        {
                              case 0x00:
                                    result.Add("FLT input inactive, local batt good");
                                    break;
                              case 0x01:
                                    result.Add("FLT input active, local batt good");
                                    break;
                              case 0x02:
                                    result.Add("FLT input inactive, local batt low");
                                    break;
                              case 0x03:
                                    result.Add("FLT input actuve, local batt low");
                                    break;
                              default:
                                    break;
                        }
                        break;
                  default:
                        switch (status & 0x07)
                        {
                              case 0x00:
                                    result.Add("Inactive");
                                    break;
                              case 0x01:
                                    result.Add("Active");
                                    break;
                              case 0x02:
                                    result.Add("Ground fault");
                                    break;
                              case 0x03:
                                    result.Add("Short");
                                    break;
                              case 0x04:
                                    result.Add("Open circuit");
                                    break;
                              case 0x05:
                                    result.Add("Foreign voltage");
                                    break;
                              case 0x06:
                                    result.Add("Non-settling error");
                                    break;
                              case 0x07:
                                    result.Add("Supervisory fault codes");
                                    break;
                              default:
                                    break;

                        }
                        break;
            }

            if ((status & 0x08) != 0)
                  result.Add("Offline");

            if ((status & 0x10) != 0)
                  result.Add("Masked");

            if ((status & 0x20) != 0)
                  result.Add("Entry or exit delay in progress");

            if ((status & 0x40) != 0)
                  result.Add("Entry delay in progress");

            if ((status & 0x80) != 0)
                  result.Add("Not attached");



            return string.Join(",", result);

      }

      private static string TypeREXRemark(int Code)
      {
            return Code switch
            {
                  1 => "Door used not verified",
                  2 => "Door not used",
                  3 => "Door used",
                  4 => "Door used not verified",
                  5 => "Door not used",
                  6 => "Door used",
                  _ => string.Empty
            };
      }

      private static string TypeCosDoor(int DoorStatus, int ApStatus)
      {
            List<string> result = new List<string>();

            switch (DoorStatus & 0x07)
            {
                  case 0x00:
                        result.Add("Inactive");
                        break;
                  case 0x01:
                        result.Add("Active");
                        break;
                  case 0x02:
                        result.Add("Ground fault");
                        break;
                  case 0x03:
                        result.Add("Short");
                        break;
                  case 0x04:
                        result.Add("Open circuit");
                        break;
                  case 0x05:
                        result.Add("Foreign voltage");
                        break;
                  case 0x06:
                        result.Add("Non-settling error");
                        break;
                  case 0x07:
                        result.Add("Supervisory fault codes");
                        break;
                  default:
                        break;

            }

            if ((ApStatus & 0x01) != 0)
                  result.Add("Unlocked");

            if ((ApStatus & 0x02) != 0)
                  result.Add("REX Exit In Progress");

            if ((ApStatus & 0x04) != 0)
                  result.Add("Forced Open");

            if ((ApStatus & 0x08) != 0)
                  result.Add("Forced Open Masked");

            if ((ApStatus & 0x10) != 0)
                  result.Add("Held Open");

            if ((ApStatus & 0x20) != 0)
                  result.Add("Held Open Masked");

            if ((ApStatus & 0x40) != 0)
                  result.Add("Held Open Pre-Alarm");

            if ((ApStatus & 0x80) != 0)
                  result.Add("Extended Held Open Mode");

            return string.Join(",", result);
      }

      private static string TypeProcedureRemark(int Code)
      {
            return Code switch
            {
                  1 => "Abort Delay",
                  2 => "Start New",
                  3 => "Resume, If Paused",
                  4 => "Procedure Prefix 256 Actions",
                  5 => "Procedure Prefix 512 Actions",
                  6 => "Procedure Prefix 1024 Actions",
                  7 => "Procedure Prefix 256 Actions",
                  8 => "Procedure Prefix 512 Actions",
                  9 => "Procedure Prefix 1024 Actions",
                  10 => "Command was issued to procedure with no action - (NOP)",
                  _ => string.Empty
            };
      }

      private static string TypeUserCmnd(char[] keys)
      {
            return new string(keys).TrimEnd('\0');
      }

      // private static string TypeAcrRemark(int flag, int spare)
      // {
      //       List<string> result = new List<string>();

      //       if ((flag & 0x0001) != 0)
      //             result.Add("Decrement use limit");

      //       if ((flag & 0x0002) != 0)
      //             result.Add("Require use limit non zero");

      //       if ((flag & 0x0004) != 0)
      //             result.Add("Deny duress request");



      // }

      private static string TypeMpgRemark(int Code)
      {
            return Code switch
            {
                  1 => "Mask count 0, All MPs masked",
                  2 => "Mask count incremented, MPs already masked",
                  3 => "Mask count cleared, all point unmasked",
                  4 => "Mask count set, unmasked all points",
                  5 => "MPG armed, may have active zoned, mask count is zero",
                  6 => "MPG not armed, mask count decrement",
                  7 => "MPG armed, did not have active zones, mask count is now zero",
                  8 => "MPG did not arm, had active zones, mask count is now zero",
                  9 => "MPG still armed, mask count decrement",
                  10 => "MPG armed, mask count is now zero",
                  11 => "MPG did not arm, mask count decremented",
                  _ => string.Empty
            };
      }

      private static string TypeAreaRemark(int status, int Occ)
      {
            List<string> Flag = new List<string>();

            if ((status & 1) != 0)
                  Flag.Add("Enable");

            if ((status & 2) != 0)
                  Flag.Add("Multi-Occ");

            if ((status & 128) != 0)
                  Flag.Add("Not Config");

            return $"Status: {string.Join(",", Flag)} , Occupancy: {Occ}";
      }

      private static string TypeUseLimitRemark(int count)
      {
            return $"New Limit: {count}";
      }

      private static string TypeWebActivity(int Code)
      {
            return Code switch
            {
                  1 => "Save home notes",
                  2 => "Save network settings",
                  3 => "Save host communication settings",
                  4 => "Add user",
                  5 => "Delete user",
                  6 => "Modify user",
                  7 => "Save password strength and session timer",
                  8 => "Save web server options",
                  9 => "Save time server settings",
                  10 => "Auto save timer settings",
                  11 => "Load certificate",
                  12 => "Logged out by link",
                  13 => "Logged out by timeout",
                  14 => "Logged out by user",
                  15 => "Logged out by apply",
                  16 => "Invalid login",
                  17 => "Successful login",
                  18 => "Network diagnostic saved",
                  19 => "Card DB size saved",
                  21 => "Diagnostic page saved",
                  22 => "Security options page saved",
                  23 => "Add-on package page saved",
                  24 => "Not used",
                  25 => "Not used",
                  26 => "Not used",
                  27 => "Invalid login limit reached",

                  28 => "Firmware download initiated",

                  29 => "Advanced networking routes saved",

                  30 => "Advanced networking reversion timer started",

                  31 => "Advanced networking reversion timer elapsed",

                  32 => "Advanced networking route changes reverted",

                  33 => "Advanced networking route changes cleared",

                  34 => "Certificate generation started",
                  _ => string.Empty

            };
      }

      private static string TypeCosElevatorRemark(int floor)
      {
            return $"Floor: {floor}";
      }

      private static string TypeFileDownloadStatusRemark(int Code, byte type, char[] name)
      {
            string filename = new string(name).TrimEnd('\0');
            string filetype = string.Empty;
            string code = string.Empty;
            switch (type)
            {
                  case 0:
                        filetype = "Host Comm certificate file (PEM)";
                        break;

                  case 1:
                        filetype = "User defined file";
                        break;

                  case 2:
                        filetype = "License file";
                        break;

                  case 3:
                        filetype = "Peer certificate";
                        break;

                  case 4:
                        filetype = "OSDP file transfer files";
                        break;

                  case 7:
                        filetype = "Linq certificate";
                        break;

                  case 8:
                        filetype = "Over-Watch certificate";
                        break;

                  case 9:
                        filetype = "Web server certificate";
                        break;

                  case 10:
                        filetype = "HID Origo™ certificate";
                        break;

                  case 11:
                        filetype = "Aperio certificate";
                        break;

                  case 12:
                        filetype = "Host translator service for OEM cloud certificate";
                        break;

                  case 13:
                        filetype = "Driver trust store";
                        break;

                  case 16:
                        filetype = "802.1x TLS authentication";
                        break;

                  case 18:
                        filetype = "HTS OEM cloud authentication";
                        break;

                  default:
                        filetype = $"Unknown file type ({type})";
                        break;
            }
            switch (Code)
            {
                  case 1:
                        code = "File transfer success";
                        break;
                  case 2:
                        code = "File transfer error";
                        break;
                  case 3:
                        code = "File delete successful";
                        break;
                  case 4:
                        code = "File delete unsuccessful";
                        break;
                  case 5:
                        code = "OSDP file transfer complete (primary ACR) - look at source number for ACR number";
                        break;
                  case 6:
                        code = "OSDP file transfer error (primary ACR) - look at source number for ACR number";
                        break;
                  case 7:
                        code = "OSDP file transfer complete (alternate ACR) - look at source number for ACR number";
                        break;
                  case 8:
                        code = "OSDP file transfer error (alternate ACR) - look at source number for ACR number";
                        break;
                  default:
                        break;
            }

            return $"File: {filename}, Type: {filetype}, Status: {code}";
      }

}