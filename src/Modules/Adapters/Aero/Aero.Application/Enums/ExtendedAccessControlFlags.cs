using System;

namespace Aero.Application.Enums;

[Flags]
public enum ExtendedAccessControlFlags : short
{
    None                 = 0x0000,
    ACR_FE_NOEXTEND      = 0x0001,
    ACR_FE_NOPINCARD     = 0x0002,
    // Note: 0x0004 is skipped in the provided specification
    ACR_FE_DFO_FLTR      = 0x0008,
    ACR_FE_NO_ARQ        = 0x0010,
    ACR_FE_SHNTRLY       = 0x0020,
    ACR_FE_FLOOR_PIN     = 0x0040,
    ACR_FE_LINK_MODE     = 0x0080,
    ACR_FE_DCARD         = 0x0100,
    ACR_FE_OVERRIDE      = 0x0200,
    ACR_FE_CRD_OVR_EN    = 0x0400,
    ACR_FE_ELV_DISABLE   = 0x0800,
    ACR_FE_LINK_MODE_ALT = 0x1000,
    ACR_FE_REX_HOLD      = 0x2000,
    ACR_FE_HOST_BYPASS   = 0x4000,
    //ACR_FE_REX_EARLYTXN  = 0x8000
    // Note: 0x8000 (the final bit in a 16-bit short) is available if needed later.
}