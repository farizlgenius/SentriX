using HID.Aero.ScpdNet.Wrapper;

namespace Adapter.Aero.Helpers;

public static class TranCodeHelper
{
      public static string GetDesc(tranType Type,int Code)
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
}