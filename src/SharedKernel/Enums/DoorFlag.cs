namespace SharedKernel.Enums;

[Flags]
public enum DoorFlag 
{
      NOEXTEND = 0x0001,
      NOPINCARD = 0x0002,
      DFO_FILTER=0x0008,
      NO_ARQ=0x0010,
      SHNTRLY=0x0020,
      FLOOR_PIN=0x0040,
      LINK_MODE=0x0080,
      DCARD=0x0100,
      OVERRIDE=0x0200,
      CRD_OVER_EN=0x0400,
      ELV_DISABLE=0x0800,
      

}