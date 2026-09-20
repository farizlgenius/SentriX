namespace Aero.Application.Metadata.Device;

public sealed class StructureStatusMetadata
{
      public string Type {get; set; }= string.Empty;
      public int Record {get ;set; }
      public int RecordSize {get; set;}
      public int Active {get; set;}

      public StructureStatusMetadata(
            string type,
            int record,
            int recordSize,
            int active
      )
      {
            Type = type;
            Record = record;
            RecordSize = recordSize;
            Active = active;
      }
}