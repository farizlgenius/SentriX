using SharedKernel.Enums;

namespace Aero.Application.Metadata.Device;

public sealed class StructureStatusMetadata
{
      public int Id {get; set;}
      public int DriverRecord {get; set;}
      public int Record {get ;set; }
      public int RecordSize {get; set;}
      public int Active {get; set;}
      public DeviceConfigurationStatus Status {get; set;}

      public StructureStatusMetadata(
            int id,
            int driverRecord,
            int record,
            int recordSize,
            int active,
            DeviceConfigurationStatus status
      )
      {
            Id = id;
            DriverRecord = driverRecord;
            Record = record;
            RecordSize = recordSize;
            Active = active;
            Status = status;
      }
}