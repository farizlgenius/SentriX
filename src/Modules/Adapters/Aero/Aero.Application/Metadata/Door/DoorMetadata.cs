namespace Aero.Application.Metadata.Door;

public sealed class DoorMetadata
{
      public short OfflineMode {get; set;}
      public short DefaultMode {get; set;}
      public short DefaultLedMode {get; set;}
      // Spare
      public bool ForceCardPin {get; set;}
      public bool DoubleCard {get ;set;}
      public bool OutputSelectionTracking {get ;set;}
      public bool LockedOverride {get; set;}
      // Another
      public bool DecreaseUseLimit {get; set;}
      public bool RequireUseLimit {get; set;}
      public bool DeniedDuress {get; set;}
      public bool QuietRex {get; set;}
      public bool FilterStatus {get; set;}
      public bool DoubleCardAccess {get; set;}
      public bool HostPermission {get; set;}
      public bool HostOfflineGrant {get; set;}


}