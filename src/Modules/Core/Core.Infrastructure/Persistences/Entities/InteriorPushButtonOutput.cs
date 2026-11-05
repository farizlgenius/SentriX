using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class InteriorPushButtonOutput : BaseEntity
{
      public short slot_no {get; set; }
      public Vendor vendor { get; set; } = Vendor.aero;
      public string metadata {get; set; } = string.Empty;
      public int? door_id { get; set; }
      public Door? door { get; set; }
      public int device_module_id { get; set; }
      public DeviceModule device_module { get; set; } = default!;
      public InteriorPushButtonOutput(){}
      public InteriorPushButtonOutput(Domain.Entities.InteriorPushButtonOutput d) : base(d.Guid)
      {
            slot_no = d.SlotNo;
            vendor = d.Vendor;
            device_module_id = d.DeviceModuleId;
            metadata = d.Metadata;
      }
      

}