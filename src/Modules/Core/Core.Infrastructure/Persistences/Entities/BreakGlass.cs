using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class BreakGlass : BaseEntity
{
      public int slot_no { get; set; }
      public Vendor vendor { get; set; } = Vendor.aero;
      public int? door_id { get; set; }
      public Door? door { get; set; }
      public int device_module_id { get; set; }
      public DeviceModule device_module { get; set; } = default!;
      public BreakGlass()
      { }

      public BreakGlass(
            Domain.Entities.BreakGlass d
      )
      {
            slot_no = d.SlotNo;
            vendor = d.Vendor;
            device_module_id = d.DeviceModuleId;
      }
}