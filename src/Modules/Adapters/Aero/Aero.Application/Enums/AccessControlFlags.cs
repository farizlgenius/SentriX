using System;

namespace Aero.Application.Enums;

[Flags]
public enum AccessControlFlags : short
{
    None = 0x0000,

    /// <summary>
    /// Decrement use limits on access
    /// </summary>
    ACR_F_DCR = 0x0001,

    /// <summary>
    /// Require use limit to be non-zero
    /// </summary>
    ACR_F_CUL = 0x0002,

    /// <summary>
    /// Set to deny a duress request. The default behavior is to grant access 
    /// under duress and log event.
    /// </summary>
    ACR_F_DRSS = 0x0004,

    /// <summary>
    /// Do not wait for door to open. Assume that the door was used and log 
    /// all access requests as used as soon as the request is granted.
    /// </summary>
    ACR_F_ALLUSED = 0x0008,

    /// <summary>
    /// Do not pulse the door strike on REX cycle. Used for “quiet” exit.
    /// </summary>
    ACR_F_QEXIT = 0x0010,

    /// <summary>
    /// Filter Change-of-state Door transactions. This flag is normally set, 
    /// unless detailed door sequence notifications are required.
    /// </summary>
    ACR_F_FILTER = 0x0020,

    /// <summary>
    /// Require two-card control at this reader
    /// </summary>
    ACR_F_2CARD = 0x0040,

    // Note: 0x0080, 0x0100, 0x0200 are skipped in the provided specification

    /// <summary>
    /// If online, check with HOST before GRANTING access.
    /// </summary>
    ACR_F_HOST_CBG = 0x0400,

    /// <summary>
    /// If HOST is not available (offline or timeout) proceed with GRANT.
    /// </summary>
    ACR_F_HOST_SFT = 0x0800,

    /// <summary>
    /// Enable cipher mode (if user command fits a card format then use it as card). 
    /// Allows user to enter digits through the keypad as card number.
    /// </summary>
    ACR_F_CIPHER = 0x1000,

    // Note: 0x2000 is skipped in the provided specification

    /// <summary>
    /// If set, log access grant transaction right away, then log used/not-used. 
    /// This feature disabled when the ACR_F_ALLUSED (0x0008) flag is set.
    /// </summary>
    ACR_F_LOG_EARLY = 0x4000,

    /// <summary>
    /// If set, show “wait” pattern on “card not in file” instead of “denied” response.
    /// </summary>
    ACR_F_CNIF_WAIT = unchecked((short)0x8000)
}