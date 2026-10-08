namespace Aero.Application.Metadata.User;

public sealed class UserMetadata
{
      public short IssueCode { get; set; }
      public int UseCount { get; set; }
      public short ApbLoc { get; set; }
      public bool OneFreeApb { get; set; }
      public bool ApbExempt { get; set; }
      public bool PinExempt { get; set; }
}

