namespace Aero.Application.Metadata.Device;

public sealed class StructureStatusMetadata
{
      public string Type {get; set; }= string.Empty;
      public int DriverRecord {get; set;}
      public int Record {get ;set; }
      public int RecordSize {get; set;}
      public int Active {get; set;}

      public StructureStatusMetadata(
            string type,
            int driverRecord,
            int record,
            int recordSize,
            int active
      )
      {
            Type = type;
            DriverRecord = driverRecord;
            Record = record;
            RecordSize = recordSize;
            Active = active;
      }
}