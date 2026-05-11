using System;
using HID.Aero.ScpdNet.Wrapper;

namespace Adapter.Aero.Helpers;

public sealed class DescriptionHelper
{
    #region SCPReplyNak

    public static string GetNakReasonDescription(short t)
    {
        switch (t)
        {
            case 0:
                return "Invalid Packet Header";
            case 1:
            case 2:
            case 3:
                return "Invalid command type (firmware revision mismatch)";
            case 4:
                return "command content error";
            case 5:
                return "Cannot execute - requires password logon";
            case 6:
                return "This port is in standby mode and cannot execute this command";
            case 7:
                return "Failed logon - password and/or encryption key";
            case 8:
                return "command not accepted, controller is running in degraded mode and only a limited number of commands are accepted";
            default:
                return "";
        }
    }

    #endregion
    public static string GetSourceDesc(short t)
    {
        switch (t)
        {
            case (short)tranSrc.tranSrcScpDiag:
                return "SCP diagnostics";
            case (short)tranSrc.tranSrcScpCom:
                return "SCP to HOST communication driver";
            case (short)tranSrc.tranSrcScpLcl:
                return "SCP local monitor points";
            case (short)tranSrc.tranSrcSioDiag:
                return "SIO diagnostics";
            case (short)tranSrc.tranSrcSioCom:
                return "SIO communication driver";
            case (short)tranSrc.tranSrcSioTmpr:
                return "SIO cabinet tamper";
            case (short)tranSrc.tranSrcSioPwr:
                return "SIO power monitor";
            case (short)tranSrc.tranSrcMP:
                return "Alarm monitor point";
            case (short)tranSrc.tranSrcCP:
                return "Strike control point";
            case (short)tranSrc.tranSrcACR:
                return "Access Control Reader (ACR)";
            case (short)tranSrc.tranSrcAcrTmpr:
                return "ACR: reader tamper monitor";
            case (short)tranSrc.tranSrcAcrDoor:
                return "ACR: door position sensor";
            case (short)tranSrc.tranSrcAcrRex0:
                return "ACR: 1st \"Request to exit\" input";
            case (short)tranSrc.tranSrcAcrRex1:
                return "ACR: 2nd \"Request to exit\" input";
            case (short)tranSrc.tranSrcTimeZone:
                return "time zone";
            case (short)tranSrc.tranSrcProcedure:
                return "procedure (action list)";
            case (short)tranSrc.tranSrcTrigger:
                return "trigger";
            case (short)tranSrc.tranSrcTrigVar:
                return "trigger variable";
            case (short)tranSrc.tranSrcMPG:
                return "Monitor point group";
            case (short)tranSrc.tranSrcArea:
                return "Access control area";
            case (short)tranSrc.tranSrcAcrTmprAlt:
                return "ACR: the alternate reader's tamper monitor source_number";
            case (short)tranSrc.tranSrcLoginService:
                return "LoginDto service";
            default:
                return "";
        }
    }

    public static string GetMessageTypeDesc(int t)
    {
        switch (t)
        {
            case (int)enSCPReplyType.enSCPReplyNAK:
                return "NAK";
            case (int)enSCPReplyType.enSCPReplyIDReport:
                return "ID Report";
            case (int)enSCPReplyType.enSCPReplyCommStatus:
                return "Communication Status";
            case (int)enSCPReplyType.enSCPReplyTranStatus:
                return "Transaction Status";
            case (int)enSCPReplyType.enSCPReplySrSio:
                return "SIO Status";
            case (int)enSCPReplyType.enSCPReplySrMp:
                return "Monitor Point Status";
            case (int)enSCPReplyType.enSCPReplySrCp:
                return "Control Point Status";
            case (int)enSCPReplyType.enSCPReplySrAcr:
                return "Access Control Reader Status";
            case (int)enSCPReplyType.enSCPReplySrTz:
                return "Time Zone Status";
            case (int)enSCPReplyType.enSCPReplySrTv:
                return "Trigger Variable Status";
            case (int)enSCPReplyType.enSCPReplySrMpg:
                return "Monitor Point Group Status";
            case (int)enSCPReplyType.enSCPReplySrArea:
                return "Access Control Area Status";
            case (int)enSCPReplyType.enSCPReplySioRelayCounts:
                return "SIO Relay Counts";
            case (int)enSCPReplyType.enSCPReplyStrStatus:
                return "String Status";
            case (int)enSCPReplyType.enSCPReplyCmndStatus:
                return "Command Status";
            case (int)enSCPReplyType.enSCPReplyWebConfigNetwork:
                return "Web Config: Network";
            case (int)enSCPReplyType.enSCPReplyWebConfigNotes:
                return "Web Config: Notes";
            case (int)enSCPReplyType.enSCPReplyWebConfigSessionTmr:
                return "Web Config: Session Timer";
            case (int)enSCPReplyType.enSCPReplyWebConfigWebConn:
                return "Web Config: Web Connection";
            case (int)enSCPReplyType.enSCPReplyWebConfigAutoSave:
                return "Web Config: Auto Save";
            case (int)enSCPReplyType.enSCPReplyWebConfigNetDiag:
                return "Web Config: Network Diagnosis";
            case (int)enSCPReplyType.enSCPReplyWebConfigTimeServer:
                return "Web Config: Time Server";
            case (int)enSCPReplyType.enSCPReplyWebConfigDiagnostics:
                return "Web Config: Diagnostics";
            case (int)enSCPReplyType.enSCPReplyWebConfigHostCommPrim:
                return "Web Config: Primary Host Communication";
            default:
                return string.Empty;
        }
    }

    public static string GetTranTypeDesc(short t)
    {
        switch (t)
        {
            case (short)tranType.tranTypeSys:
                return "System status";
            case (short)tranType.tranTypeSioComm:
                return "SIO communication status";
            case (short)tranType.tranTypeCardBin:
                return "Binary card data";
            case (short)tranType.tranTypeCardBcd:
                return "Card data report";
            case (short)tranType.tranTypeCardFull:
                return "32-bit card number length";
            case (short)tranType.tranTypeDblCardFull:
                return "52-bit (double) card number length";
            case (short)tranType.tranTypeI64CardFull:
                return "64-bit (I64) card number length";
            case (short)tranType.tranTypeI64CardFullIc32:
                return "64-bit (I64) card number length with 32-bit issue code";
            case (short)tranType.tranTypeCardID:
                return "32-bit card number length";
            case (short)tranType.tranTypeDblCardID:
                return "52-bit (double) card number length";
            case (short)tranType.tranTypeI64CardID:
                return "64-bit (I64) card number length";
            case (short)tranType.tranTypeCoS:
                return "Change-of-state status";
            case (short)tranType.tranTypeREX:
                return "REX";
            case (short)tranType.tranTypeCoSDoor:
                return "door status monitor change-of-state";
            case (short)tranType.tranTypeProcedure:
                return "procedure log";
            case (short)tranType.tranTypeUserCmnd:
                return "card_holder command request";
            case (short)tranType.tranTypeActivate:
                return "trigger or time zone change of state";
            case (short)tranType.tranTypeAcr:
                return "ACR mode changes";
            case (short)tranType.tranTypeMpg:
                return "Monitor point group status";
            case (short)tranType.tranTypeArea:
                return "Access area";
            case (short)tranType.tranTypeUseLimit:
                return "Use limit update ";
            case (short)tranType.tranTypeWebActivity:
                return "Web activity";
            case (short)tranType.tranTypeOperatingMode:
                return "New operation mode";
            case (short)tranType.tranTypeCoSElevator:
                return "Elevator relay status";
            case (short)tranType.tranTypeFileDownloadStatus:
                return "File download status";
            case (short)tranType.tranTypeCoSElevatorAccess:
                return "Elevator ACR floor available";
            case (short)tranType.tranTypeAcrExtFeatureStls:
                return "ACR extend stateless";
            case (short)tranType.tranTypeAcrExtFeatureCoS:
                return "ACR extend change of state";
            case (short)tranType.tranTypeAsci:
                return "ASCII diagnostic";
            case (short)tranType.tranTypeSioDiag:
                return "SIO comm diagnostics";
            default:
                return "";
        }

    }

    /**
     * SCPReplyCommStatus
     **/

    public static string GetReplyStatusDesc(int t)
    {
        switch (t)
        {
            case 0:
                return "Unknown";
            case 1:
                return "Communication failed. At least one communication channel is known to be offline.";
            case 2:
                return "Communication OK.";
            default:
                return "";
        }
    }

    public static string GetCommStatusDesc(int t)
    {
        switch (t)
        {
            case 0:
                return "Channel detached";
            case 1:
                return "Communication not yet attempted";
            case 2:
                return "Device offline";
            case 3:
                return "Device online";
            default:
                return "";
        }
    }

    public static string GetErrorCodeDesc(int t)
    {
        switch (t)
        {
            case 0:
                return "No error";
            case 1:
                return "Communication timeout";
            case 2:
                return "CommReplyTooLong";
            case 3:
                return "CommCommandTooLong";
            case 4:
                return "CRC error";
            case 5:
                return "CommSequenceNumber";
            case 6:
                return "Communication channel detached";
            case 7:
                return "CommDeleted";
            case 8:
                return "CommCipher";
            case 20:
                return "enSCPCommLogOn";
            case 21:
                return "enSCPCommNoErrAes";
            case 31:
                return "enSCPCommNoPassword";
            case 32:
                return "enSCPCommNoAesSet";
            case 33:
                return "enSCPCommNoAesScp";
            case 34:
                return "enSCPCommAesKey";
            default:
                return "";
        }
    }

    /**
     * SCPReplyTranStatus
     **/

    public static string GetStatusTranReportDesc(short t)
    {
        switch (t)
        {
            case 0:
                return "transaction reporting enabled";
            default:
                return "transaction reporting disabled";
        }
    }

    /**
     * SCPReplySrSio
     **/

    public static string GetSioModelDesc(short t)
    {
        switch (t)
        {
            case 196:
                return "HID Aero 1100 (internal SIO)";
            case 193:
                return "HID Aero x100";
            case 194:
                return "HID Aero x200";
            case 195:
                return "HID Aero x300";
            case 190:
                return "VertX V100";
            case 191:
                return "VertX V200";
            case 192:
                return "VertX V300";
            default:
                return "";
        }
    }

    /**
     * SCPReplySrAcr
     **/

    public static string GetAcrModeForStatus(short s)
    {
        switch (s)
        {
            case 1:
                return "Disable the ACR, no REX";
            case 2:
                return "Unlock (unlimited access)";
            case 3:
                return "Locked (no access, REX active)";
            case 4:
                return "facility code only";
            case 5:
                return "Card only";
            case 6:
                return "PIN only";
            case 7:
                return "Card and PIN required";
            case 8:
                return "Card or PIN required";
            case 16:
                return "Disable “2-card” mode (Clear actl_flags - ACR_F_2CARD)";
            case 17:
                return "Enable “2-card” mode (Set actl_flags - ACR_F_2CARD)";
            case 26:
                return "Clear “ACR_FE_NO_ARQ” extended actl_flags";
            case 27:
                return "Set “ACR_FE_NO_ARQ” extended actl_flags";
            case 29:
                return "Start linking mode (Set “ACR_FE_LINK_MODE” extended actl_flags)";
            case 30:
                return "Abort linking mode (Clear “ACR_FE_LINK_MODE” extended actl_flags)";
            case 31:
                return "Extended feature Change";
            case 32:
                return "Start linking mode for Alternate Reader (Set “ACR_FE_LINK_MODE_ALT” extended actl_flags)";
            case 33:
                return "Abort linking mode for Alternate Reader (Clear “ACR_FE_LINK_MODE_ALT” extended actl_flags)";
            default:
                return "";

        }
    }

    public static string GetAcrAccessModeDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Disable the ACR, no REX";
            case 2:
                return "Unlock (unlimited access)";
            case 3:
                return "Locked (no access, REX active)";
            case 4:
                return "facility code only";
            case 5:
                return "Card only";
            case 6:
                return "PIN only";
            case 7:
                return "Card and PIN required";
            case 8:
                return "Card or PIN required";
            case 16:
                return "Disable “2-card” mode (Clear actl_flags - ACR_F_2CARD)";
            case 17:
                return "Enable “2-card” mode (Set actl_flags - ACR_F_2CARD)";
            case 26:
                return "Clear “ACR_FE_NO_ARQ” extended actl_flags";
            case 27:
                return "Set “ACR_FE_NO_ARQ” extended actl_flags";
            case 29:
                return "Start linking mode (Set “ACR_FE_LINK_MODE” extended actl_flags)";
            case 30:
                return "Abort linking mode (Clear “ACR_FE_LINK_MODE” extended actl_flags)";
            case 31:
                return "Extended feature Change";
            case 32:
                return "Start linking mode for Alternate Reader (Set “ACR_FE_LINK_MODE_ALT” extended actl_flags)";
            case 33:
                return "Abort linking mode for Alternate Reader (Clear “ACR_FE_LINK_MODE_ALT” extended actl_flags)";
            default:
                return "";
        }
    }

    public static string GetExtendFeatureType(short s)
    {
        return s switch
        {
            0 => "None",
            1 => "Classroom",
            2 => "Office",
            3 => "Privacy",
            4 => "Apartment",
            _ => ""
        };
    }

    public static string GetNHardwareTypeDesc(short s)
    {
        return s switch
        {
            0 => "HID Aero",
            _ => "Other type_desc"
        };
    }

    /**
     * SCPReplyTypeCoS
     **/



    public static string GetAccessControlFlagDescExtend(short b)
    {
        return b switch
        {
            0x0001 => "On a new access grant, do not resume the extended door held open timer.",
            0x0002 => "Card and PIN reader mode: Do not accept PIN followed by CARD. Forces CARD to be read first",
            _ => ""
        };

    }

    /**
     * SCPReplySrTz
     **/

    public static string GetTimeZoneStatus(short s)
    {
        return s switch
        {
            0x01 => "tz active",
            0x02 => "time based scan state",
            0x04 => "time scan override.",
            _ => ""
        };
    }


    public static string GetTriggerVariableStatus(short s)
    {
        return s.ToString();
    }

    /**
     * SCPReplySrArea 
     **/

    public static string GetAreaFlagsDesc(short s)
    {
        return s switch
        {
            1 => "Set if area is enabled (open)",
            2 => "et if the area requires multiple occupancy",
            128 => "Set if this area has NOT been configured (no area checks are made!).",
            _ => ""
        };
    }


    /**
     * SCPReplyTransaction
     **/

    #region tranTypeSys
    public static string GetTranCodeTypeSysDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "SCP power-up diagnostics";
            case 2:
                return "Host communications offline";
            case 3:
                return "Host communications online";
            case 4:
                return "transaction count exceeds the preset limit";
            case 5:
                return "Configuration database save complete";
            case 6:
                return "Card database save complete";
            case 7:
                return "Card database cleared due to SRAM buffer overflow";
            default:
                return "";
        }
    }

    public static string GetErrorCodeTypeSysDesc(SCPReplyMessage message)
    {
        return "";
    }

    #endregion

    #region tranTypeSioComm

    public static string GetTranCodeTypeSioCommDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Communication disabled (result of host command)";
            case 2:
                return "Offline: timeout (no/bad response from unit)";
            case 3:
                return "Offline: invalid identification from SIO";
            case 4:
                return "Offline: command too long";
            case 5:
                return "Online: normal connection";
            case 6:
                return "hexLoad report: ser_num is address loaded (-1 = last record)";
            default:
                return "";
        }
    }

    public static string GetNHardwareIdDesc(short t)
    {
        switch (t)
        {
            case 0:
                return "VertX";
            case 217:
                return "HID Aero X1100";
            case 218:
                return "HID Aero X100";
            case 219:
                return "HID Aero X200";
            case 220:
                return "HID Aero X300";
            default:
                return "";
        }
    }

    public static string GetNEncConfig(byte b)
    {
        switch (b)
        {
            case 0:
                return "None";
            case 1:
                return "AES Default Key";
            case 2:
                return "AES Master/Secret Key";
            case 3:
                return "PKI (Reserved)";
            case 6:
                return "AES 256 Session Key derived from default key";
            default:
                return "";
        }
    }

    public static string GetNEncKeyStatus(byte b)
    {
        switch (b)
        {
            case 0:
                return "Not loaded to controller";
            case 1:
                return "Loaded to controller, unverified w/SIO";
            case 2:
                return "Loaded to controller, conflicts w/SIO";
            case 3:
                return "Loaded to controller, verified w/ SIO";
            case 4:
                return "AES 256 Verified";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeCardBin

    public static string GetTranCodeTypeCardBinDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Access denied, Invalid card format";
            default:
                return "";
        }
    }


    #endregion

    #region tranTypeCardBcd

    public static string GetTranCodeTypeCardBcdDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Access denied, Invalid card format, forward read";
            case 2:
                return "Access denied, Invalid card format, reverse read";
            default:
                return "";
        }
    }

    #endregion

    #region TypeCardFull,TypeDblCardFull,TypeI64CardFull,TypeI64CardFull,TypeI64CardFullIc32

    public static string GetTranCodeTypeCardFullDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Request rejected: access point \"locked\"";
            case 2:
                return "Request accepted: access point \"unlocked\"";
            case 3:
                return "Request rejected: invalid facility code";
            case 4:
                return "Request rejected: invalid facility code extension";
            case 5:
                return "Request rejected: not in card file";
            case 6:
                return "Request rejected: invalid issue code";
            case 7:
                return "Request granted: facility code verified, not used";
            case 8:
                return "Request granted: facility code verified, door used";
            case 9:
                return "Access denied - asked for host approval, then timed out";
            case 10:
                return "Reporting that this card is \"about to get access granted\" (expecting command 329: Send Host ResponseDto host response)";
            case 11:
                return "Access denied count exceeded";
            case 12:
                return "Access denied - asked for host approval, then host denied";
            case 13:
                return "Request rejected: Airlock is busy";
            default:
                return "";
        }
    }

    #endregion

    #region TypeCardID,TypeDblCardID,TypeI64CardID

    public static string GetTranCodeTypeCardIDDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Request rejected: deactivated card";
            case 2:
                return "Request rejected: before activation date";
            case 3:
                return "Request rejected: after expiration date";
            case 4:
                return "Request rejected: invalid time";
            case 5:
                return "Request rejected: invalid PIN";
            case 6:
                return "Request rejected: anti-passback violation";
            case 7:
                return "Request granted: APB violation, not used";
            case 8:
                return "Request granted: APB violation, used";
            case 9:
                return "Request rejected: duress code detected";
            case 10:
                return "Request granted: duress, used";
            case 11:
                return "Request granted: duress, not used";
            case 12:
                return "Request granted: full test, not used";
            case 13:
                return "Request granted: full test, used";
            case 14:
                return "Request denied: never allowed at this reader (all Tz's = 0)";
            case 15:
                return "Request denied: no second card presented";
            case 16:
                return "Request denied: occupancy limit reached";
            case 17:
                return "Request denied: the area is NOT enabled";
            case 18:
                return "Request denied: use limit";
            case 21:
                return "Granting access: used/not used transaction will follow";
            case 24:
                return "Request rejected: no escort card presented";
            case 25:
                return "Reserved";
            case 26:
                return "Reserved";
            case 27:
                return "Reserved";
            case 29:
                return "Request rejected: airlock is busy";
            case 30:
                return "Request rejected: incomplete CARD & PIN sequence";
            case 31:
                return "Request granted: double-card event";
            case 32:
                return "Request granted: double-card event while in uncontrolled state (locked/unlocked)";
            case 39:
                return "Granting access: requires escort, pending escort card";
            case 40:
                return "Request rejected: violates minimum occupancy count";
            case 41:
                return "Request rejected: card pending at another reader";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeCos

    public enum StatusCode
    {
        Inactive = 0,
        Active = 1,
        GroundFault = 2,
        Short = 3,
        OpenCircuit = 4,
        ForeignVoltage = 5,
        NonSettlingError = 6,
        SupervisoryFault = 7
    }

    [Flags]
    public enum StatusFlags : byte
    {
        None = 0x00,
        Offline = 0x08,
        Masked = 0x10,
        LocalMask = 0x20,
        EntryDelay = 0x40,
        NotAttached = 0x80
    }

    public static string DecodePowerFaultStatus(byte code)
    {
        return code switch
        {
            0 => "FLT inactive, battery good",
            1 => "FLT active, battery good",
            2 => "FLT inactive, battery low",
            3 => "FLT active, battery low",
            _ => "Unknown power fault status"
        };
    }

    public static string DecodeReaderTamperStatus(byte code)
    {
        return code switch
        {
            0 => "Online, tamper inactive",
            1 => "Online, tamper active",
            2 => "N/A",
            3 => "Communication broken (offline)",
            _ => "Unknown tamper status"
        };
    }

    public static string GetTranCodeTypeCosDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Disconnected (from an input point ID)";
            case 2:
                return "Unknown (offline): no report from the ID";
            case 3:
                return "Secure (or deactivate relay)";
            case 4:
                return "Alarm (or activated relay: perm or temp)";
            case 5:
                return "Fault";
            case 6:
                return "Exit delay in progress";
            case 7:
                return "Entry delay in progress";
            default:
                return "";
        }
    }

    public static string DecodeStatusTypeCoS(short b, short sourceType)
    {
        byte codeValue = (byte)(b & 0x07);
        var flags = (StatusFlags)(b & 0xF8);

        if (sourceType == (short)tranSrc.tranSrcAcrTmpr)
        {
            return DecodeReaderTamperStatus(codeValue);
        }
        else if (sourceType == (short)tranSrc.tranSrcSioPwr)
        {
            return DecodePowerFaultStatus(codeValue);
        }
        else
        {
            return ((StatusCode)codeValue).ToString();
        }
    }

    public static string DecodeStatusTypeCoS(short b)
    {
        byte codeValue = (byte)(b & 0x07);

        return ((StatusCode)codeValue).ToString();

    }


    public static short DecodeStatusTypeCoSNumber(short b, short sourceType)
    {
        byte codeValue = (byte)(b & 0x07);
        var flags = (StatusFlags)(b & 0xF8);

        if (sourceType == (short)tranSrc.tranSrcAcrTmpr)
        {
            return codeValue;
        }
        else if (sourceType == (short)tranSrc.tranSrcSioPwr)
        {
            return codeValue;
        }
        else
        {
            return codeValue;
        }
    }



    #endregion

    #region tranTypeREX

    public static string GetTranCodeTypeREXDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Exit cycle: door use not verified";
            case 2:
                return "Exit cycle: door not used";
            case 3:
                return "Exit cycle: door used";
            case 4:
                return "Host initiated request: door use not verified";
            case 5:
                return "Host initiated request: door not used";
            case 6:
                return "Host initiated request: door used";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeCosDoor

    [Flags]
    public enum AccessPointStatus : byte
    {
        None = 0x00,
        Unlocked = 0x01,
        ExitCycleInProgress = 0x02,
        ForcedOpen = 0x04,
        ForcedOpenMasked = 0x08,
        HeldOpen = 0x10,
        HeldOpenMasked = 0x20,
        HeldOpenPreAlarm = 0x40,
        ExtendedHeldOpenMode = 0x80
    }

    public static string GetTranCodeTypeCosDoorDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Disconnected";
            case 2:
                return "Unknown _RS bits: last known status";
            case 3:
                return "Secure";
            case 4:
                return "Alarm (forced, held open or both)";
            case 5:
                return "Fault (fault type is encoded in door_status byte)";
            default:
                return "";
        }
    }

    public static string GetAccessPointStatusFlagResult(byte status)
    {
        AccessPointStatus s = (AccessPointStatus)status;

        if (!s.HasFlag(AccessPointStatus.ForcedOpenMasked))
        {
            if (s.HasFlag(AccessPointStatus.ForcedOpen))
            {
                return "Forced Open";
            }
        }


        if (!s.HasFlag(AccessPointStatus.HeldOpenMasked))
        {
            if (s.HasFlag(AccessPointStatus.HeldOpen))
            {
                return "Held Open";
            }
        }



        if (s.HasFlag(AccessPointStatus.ExitCycleInProgress))
        {
            return "Unlocked";
        }

        if (s.HasFlag(AccessPointStatus.Unlocked))
        {
            return "Unlocked";
        }

        if (s.HasFlag(AccessPointStatus.None))
        {
            return "Secure";
        }

        return "";

        //if (s.HasFlag(AccessPointStatus.HeldOpenPreAlarm))
        //{
        //    result.Add("Held Open Pre Alarm");
        //}

        //if (s.HasFlag(AccessPointStatus.ExtendedHeldOpenMode))
        //{
        //    result.Add("extend Held Open");
        //}


    }

    public static string GetStatusTypeCosDoorDesc(byte b)
    {
        switch (b)
        {
            case 0x01:
                return "flag: set if access point is unlocked";
            case 0x02:
                return "flag: access (exit) cycle in progress";
            case 0x04:
                return "flag: forced open status";
            case 0x08:
                return "flag: forced open mask status";
            case 0x10:
                return "flag: held open status";
            case 0x20:
                return "flag: held open mask status";
            case 0x40:
                return "flag: held open pre-alarm status";
            case 0x80:
                return "flag: door is in \"extended held open\" mode";
            default:
                return "";
        }

    }

    #endregion

    #region tranTypeProcedure

    public static string GetTranCodeTypeProcedureDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Cancel procedure (abort delay)";
            case 2:
                return "Execute procedure (start new)";
            case 3:
                return "Resume procedure, if paused";
            case 4:
                return "Execute procedure with prefix 256 actions";
            case 5:
                return "Execute procedure with prefix 512 actions";
            case 6:
                return "Execute procedure with prefix 1024 actions";
            case 7:
                return "Resume procedure with prefix 256 actions";
            case 8:
                return "Resume procedure with prefix 512 actions";
            case 9:
                return "Resume procedure with prefix 1024 actions";
            case 10:
                return "command was issued to procedure with no actions - (NOP)";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeUserCmnd

    public static string GetTranCodeTypeUserCmnd(short t)
    {
        switch (t)
        {
            case 1:
                return "command entered by the user";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeActivate

    public static string GetTranCodeTypeActivate(short t)
    {
        switch (t)
        {
            case 1:
                return "Became inactive";
            case 2:
                return "Became active";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeAcr

    public static string GetTranCodeTypeAcr(short t)
    {
        switch (t)
        {
            case 1:
                return "Disabled";
            case 2:
                return "Unlocked";
            case 3:
                return "Locked (exit request enabled)";
            case 4:
                return "facility code only";
            case 5:
                return "Card only";
            case 6:
                return "PIN only";
            case 7:
                return "Card and PIN";
            case 8:
                return "PIN or card";
            default:
                return "";
        }
    }

    public static string GetAccessControlFlagDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Decrement use limits on access";
            case 2:
                return "Require use limit to be non-zero";
            case 4:
                return "Set to deny a duress request. The default behavior is to\r\ngrant access under duress and log event. See\r\nCommand 1105: Access Database Specification for\r\nmore information about duress requests.";
            case 8:
                return "Do not wait for door to open. Assume that the door\r\nwas used and log all access requests as used as soon as\r\nthe request is granted.";
            case 10:
                return "Do not pulse the door strike on REX cycle. Used for “quiet” exit.";
            case 20:
                return "Filter Change-of-state door transactions. This flag is normally set, unless detailed door sequence notifications are required.";
            case 40:
                return "Require two-card control at this reader";
            case 400:
                return "If online, check with HOST before GRANTING access. See remarks";
            case 800:
                return "If HOST is not available (offline or timeout) proceed with GRANT. See remarks.";
            case 1000:
                return "Enable cipher mode (if user command fits a card format then use it as card). Allows user to enter digits through the keypad as card number. See command 1117: trigger Specification (Expanded code Map).";
            case 4000:
                return "If set, log access grant transaction right away, then log used/not-used. This feature disabled when the ACR_F_ALLUSED (0x0008) access control flag is set.";
            case 8000:
                return "If set, show “wait” pattern on “card not in file” instead of “denied” response. See command 122: Reader LED/Buzzer Function Specs “wait” state.";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeMpg

    public static string GetTranCodeTypeMpg(short t)
    {
        switch (t)
        {
            case 1:
                return "First disarm command executed (mask_count was 0, all MPs got masked)";
            case 2:
                return "Subsequent disarm command executed (mask_count incremented, MPs already masked)";
            case 3:
                return "Override command: armed (mask_count cleared, all points unmasked)";
            case 4:
                return "Override command: disarmed (mask_count set, unmasked all points)";
            case 5:
                return "Force arm command, MPG armed, (may have active zones, mask_count is now zero)";
            case 6:
                return "Force arm command, MPG not armed (mask_count decremented)";
            case 7:
                return "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)";
            case 8:
                return "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)";
            case 9:
                return "Standard arm command, MPG still armed, (mask_count decremented)";
            case 10:
                return "Override arm command, MPG armed (mask_count is now zero)";
            case 11:
                return "Override arm command, MPG did not arm, (mask_count decremented)";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeArea

    public static string GetTranCodeTypeArea(short t)
    {
        switch (t)
        {
            case 1:
                return "Area disabled";
            case 2:
                return "Area enabled";
            case 3:
                return "Occupancy count reached zero";
            case 4:
                return "Occupancy count reached the \"downward-limit\"";
            case 5:
                return "Occupancy count reached the \"upward-limit\"";
            case 6:
                return "Occupancy count reached the \"max-occupancy-limit\"";
            case 7:
                return "Multi-occupancy mode changed";
            default:
                return "";
        }
    }

    public static string GetTypeAreaStatusDesc(short t)
    {
        switch (t)
        {
            case 1:
                return "Set if area is enabled (open)";
            case 2:
                return "Set if the area requires multiple occupancy";
            case 128:
                return "Set if this area has NOT been configured (no area checks are made!)";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeUseLimit

    public static string GetTranCodeTypeUseLimit(short t)
    {
        switch (t)
        {
            case 1:
                return "Use limit changed, reporting new limit";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeWebActivity

    public static string GetTranCodeTypeWebActivity(short t)
    {
        switch (t)
        {
            case 1:
                return "Save home notes";
            case 2:
                return "Save network settings";
            case 3:
                return "Save host communication settings";
            case 4:
                return "Add user";
            case 5:
                return "DeleteAsync user";
            case 6:
                return "Modify user";
            case 7:
                return "Save password strength and session timer";
            case 8:
                return "Save web server options";
            case 9:
                return "Save time server settings";
            case 10:
                return "Auto save timer settings";
            case 11:
                return "Load certificate";
            case 12:
                return "Logged out by link";
            case 13:
                return "Logged out by timeout";
            case 14:
                return "Logged out by user";
            case 15:
                return "Logged out by apply";
            case 16:
                return "Invalid login";
            case 17:
                return "Successful login";
            case 18:
                return "Network diagnostic saved";
            case 19:
                return "Card DB size saved";
            case 21:
                return "Diagnostic page saved";
            case 22:
                return "Security options page saved";
            case 23:
                return "Add-on package page saved";
            case 24:
                return "Not used";
            case 25:
                return "Not used";
            case 26:
                return "Not used";
            case 27:
                return "Invalid login limit reached";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeOperationMode

    public static string GetTranCodeTypeOperatingMode(short t)
    {
        switch (t)
        {
            case 1:
                return "Operating mode changed to mode 0";
            case 2:
                return "Operating mode changed to mode 1";
            case 3:
                return "Operating mode changed to mode 2";
            case 4:
                return "Operating mode changed to mode 3";
            case 5:
                return "Operating mode changed to mode 4";
            case 6:
                return "Operating mode changed to mode 5";
            case 7:
                return "Operating mode changed to mode 6";
            case 8:
                return "Operating mode changed to mode 7";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeCoSElevator

    public static string GetTranCodeTypeCosElevator(short t)
    {
        switch (t)
        {
            case 1:
                return "Floor status is secure";
            case 2:
                return "Floor status is public";
            case 3:
                return "Floor status is disable (override)";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeFileDownloadStatus

    public static string GetTranCodeTypeFileDownloadStatus(short t)
    {
        switch (t)
        {
            case 1:
                return "File transfer success";
            case 2:
                return "File transfer error";
            case 3:
                return "File delete successful";
            case 4:
                return "File delete unsuccessful";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeCoSElevatorAccess

    public static string GetTranCodeTypeCoSElevatorAccess(short t)
    {
        switch (t)
        {
            case 1:
                return "Elevator access";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeAcrExtFeatureStls

    public static string GetTranCodeTypeAcrExtFeatureStls(short t)
    {
        switch (t)
        {
            case 1:
                return "Extended status updated";
            default:
                return "";
        }
    }

    #endregion

    #region tranTypeAcrExtFeatureCoS

    public static string GetTranCodeTypeAcrExtFeatureCoS(short t)
    {
        switch (t)
        {
            case 3:
                return "Secure / Inactive";
            case 4:
                return "Alarm / Active";
            default:
                return "";
        }
    }

    #endregion

    #region ScpResplyStrStatus

    public enum ScpStructure
    {
        SCPSID_TRAN = 1,            // transaction
        SCPSID_TZ = 2,              // time zones
        SCPSID_HOL = 3,             // holiday
        SCPSID_MSP1 = 4,            // Msp1 ports (SIO drivers)
        SCPSID_SIO = 5,             // SIOs
        SCPSID_MP = 6,              // Monitor points
        SCPSID_CP = 7,              // Control points
        SCPSID_ACR = 8,             // Access control reader
        SCPSID_ALVL = 9,            // Access levels
        SCPSID_TRIG = 10,           // trigger
        SCPSID_PROC = 11,           // procedure
        SCPSID_MPG = 12,            // Monitor point groups
        SCPSID_AREA = 13,           // Access area
        SCPSID_EAL = 14,            // Elevator access levels
        SCPSID_CRDB = 15,           // Cardholder database
        SCPSID_FLASH = 20,          // FLASH specs: nRecords==MfgID, nRecSize==BlockSize, nActive==FlashSize
        SCPSID_BSQN = 21,           // Build sequence number (internal use)
        SCPSID_SAVE_STAT = 22,      // Flash save status (Bit 0: normal, Bit 1: dirty, Bit 2: auto save)
        SCPSID_MAB1_FREE = 23,      // Memory alloc block 1 (host config) free memory
        SCPSID_MAB2_FREE = 24,      // Memory alloc block 2 (card database) free memory
        SCPSID_ARQ_BUFFER = 26,     // Access request buffers (internal use only)
        SCPSID_PART_FREE_CNT = 27,  // Partition memory free info (internal use only)
        SCPSID_LOGIN_STANDARD = 33,  // Web logins - standard
        SCPSID_FILE_SYSTEM = 35
    }

    public static string ScpStructureToText(ScpStructure n)
    {
        return n switch
        {
            ScpStructure.SCPSID_TRAN => "transaction",
            ScpStructure.SCPSID_TZ => "time Zones",
            ScpStructure.SCPSID_HOL => "holiday",
            ScpStructure.SCPSID_MSP1 => "MSP1 ports",
            ScpStructure.SCPSID_SIO => "modules",
            ScpStructure.SCPSID_MP => "Monitor Points",
            ScpStructure.SCPSID_CP => "Control Points",
            ScpStructure.SCPSID_ACR => "Access Control reader",
            ScpStructure.SCPSID_ALVL => "Access Levels",
            ScpStructure.SCPSID_TRIG => "trigger",
            ScpStructure.SCPSID_PROC => "procedure",
            ScpStructure.SCPSID_MPG => "Monitor Point Groups",
            ScpStructure.SCPSID_AREA => "Areas",
            ScpStructure.SCPSID_EAL => "Elevator Access Levels",
            ScpStructure.SCPSID_CRDB => "credential",
            ScpStructure.SCPSID_FLASH => "Flash",
            ScpStructure.SCPSID_BSQN => "Build Sequence No",
            ScpStructure.SCPSID_SAVE_STAT => "Auto Save status",
            ScpStructure.SCPSID_MAB1_FREE => "Memory Configuration Database",
            ScpStructure.SCPSID_MAB2_FREE => "Memory Card Database",
            ScpStructure.SCPSID_ARQ_BUFFER => "Access Request Buffer",
            ScpStructure.SCPSID_LOGIN_STANDARD => "Login",
            ScpStructure.SCPSID_FILE_SYSTEM => "File Version",
            _ => "Not used",
        };
    }

    #endregion
}
