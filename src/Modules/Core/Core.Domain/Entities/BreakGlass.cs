using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class BreakGlass : BaseDomain
{
  public int SlotNo { get; private set; }
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int DeviceModuleId { get; private set; }
  public BreakGlass(
    int slotNo,
    Vendor vendor,
    int deviceModuleId
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }

  public BreakGlass(
    Guid guid,
    int slotNo,
    Vendor vendor,
    int deviceModuleId
  ) : base(guid)
  {
    SlotNo = slotNo;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
  }
}