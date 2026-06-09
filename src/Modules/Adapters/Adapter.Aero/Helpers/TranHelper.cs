using System.Text;
using System.Windows.Markup;
using Adapter.Aero.Model;
using HID.Aero.ScpdNet.Wrapper;

namespace Adapter.Aero.Helpers;

public static class TranHelper
{
      public static string GetCode(tranSrc Src,tranType Type,int Code)
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
                  
                 
                 default:
                 return string.Empty;
                 
            }
      }

      public static string GetRemark(SCPReplyMessageDto msg)
      {
            switch (msg.tran.tran_type)
            {
                  case (short)tranType.tranTypeSys:
                  return TypeSysRemark(msg.tran.tran_code,msg.tran.sys.error_code);
                  case (short)tranType.tranTypeSioComm:
                  return TypeSioCommRemark();
                  case (short)tranType.tranTypeCardBin:
                  return TypeCardBinRemark(msg.tran.tran_code,msg.tran.c_bin.bit_count,msg.tran.c_bin.bit_array);
                  case (short)tranType.tranTypeCardBcd:
                  return TypeCardBcdRemark(msg.tran.tran_code,msg.tran.c_bcd.digit_count,msg.tran.c_bcd.bcd_array);
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
                  return TypeCoSRemark(msg.tran.tran_code,(tranSrc)msg.tran.source_type);
                  default:
                  return string.Empty;
            }
      }

      private static string TypeSysRemark(int Code,int Error)
      {
            
            if(Code == 1)
            {
                  StringBuilder result = new StringBuilder();
                  if((Error & (1 << 2)) != 0) result.Append("External Reset ");
                  if((Error & (1 << 3)) != 0) result.Append("Power on Reset ");
                  if((Error & (1 << 4)) != 0) result.Append("Watchdog Timer ");
                  if((Error & (1 << 5)) != 0) result.Append("Watchdog Timer ");
                  if((Error & (1 << 6)) != 0) result.Append("Watchdog Timer ");
                  if((Error & (1 << 7)) != 0) result.Append("Watchdog Timer ");
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

      private static string TypeCardBinRemark(int Code,int bit,byte[] arr)
      {
            switch (Code)
            {
                  case 1:
                  return $"Invalid card format, Bit: {bit}, Data: {UtilitiesHelper.ByteToHexStr(arr)}";
                  default:
                  return string.Empty;
            }

      }

      private static string TypeCardBcdRemark(int Code,int digit,byte[] arr)
      {
            switch (Code)
            {
                  case 1:
                  return $"Invalid card format,forward read, Digit: {digit}, Data: {UtilitiesHelper.ByteToHexStr(arr)}";
                  case 2 :
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

       private static string TypeCoSRemark(int Code,tranSrc src,int Status)
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
                  return string.Empty;
            }
      }
}