namespace Adapter.Aero.Model.Metadata;

public sealed class MpgMetadata
{
      public List<(short Type,short Number)> MpList { get; set; } = new List<(short Type, short Number)>();
      
}