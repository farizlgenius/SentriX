using System.Text;
using Adapter.Aero.Model;
using HID.Aero.ScpdNet.Wrapper;

namespace Adapter.Aero.Helpers;

public static class TranHelper
{
      public static string GetCode(tranType Type,int Code)
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
                  case tranType.tranTypeCoS:
                  return Code switch
                  {
                        1 => "Disconnected",
                        2 => "Unknown",
                        3 => "Secure",
                        4 => "Alarm",
                        5 => "Fault",
                        6 => "Exit delay in progress",
                        7 => "Entry delay in progress",
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
                  return TypeSysRemark(msg.tran.tran_code,msg.tran.sys.error_code);
                  case (short)tranType.tranTypeSioComm:
                  return TypeSioCommRemark();
                  case (short)tranType.tranTypeCardBin:
                  return TypeCardBinRemark(msg.tran.c_bin.bit_count,msg.tran.c_bin.bit_array);
                  case (short)tranType.tranTypeCardBcd:
                  return TypeCardBcdRemark(msg.tran.c_bcd.digit_count,msg.tran.c_bcd.bcd_array);
                  case (short)tranType.tranTypeCardFull:
                  return TypeCardFullRemark();
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

      private static string TypeCardBinRemark(int bit,byte[] arr)
      {
            return $"Bit: {bit}, Data: {UtilitiesHelper.ByteToHexStr(arr)}";
      }

      private static string TypeCardBcdRemark(int digit,byte[] arr)
      {
            return $"Digit: {digit}, Data: {UtilitiesHelper.ByteToHexStr(arr)}";
      }

      private static string TypeCardFullRemark()
      {
            return string.Empty;
      }
}