using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class InteriorPushButtonOutput : BaseDomain
{
  public short SlotNo { get; private set; }
  public Vendor Vendor { get; private set; } = Vendor.aero;
  public int DeviceModuleId { get; private set; }
  public string Metadata {get; private set;}
  public InteriorPushButtonOutput(
    short slotNo,
    Vendor vendor,
    string metadata,
    int deviceModuleId
  ) : base(Guid.NewGuid())
  {
    SlotNo = slotNo;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
    Metadata = metadata;
  }

  public InteriorPushButtonOutput(
    Guid guid,
    short slotNo,
    Vendor vendor,
    string metadata,
    int deviceModuleId
  ) : base(guid)
  {
    SlotNo = slotNo;
    Vendor = vendor;
    DeviceModuleId = deviceModuleId;
    Metadata = metadata;
  }
}